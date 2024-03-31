using MZZT.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static System.Math;

namespace MZZT.Drawing {
	/// <summary>
	/// https://beesbuzz.biz/code/hsv_color_transforms.php
	/// http://stackoverflow.com/questions/3018313/algorithm-to-convert-rgb-to-hsv-and-hsv-to-rgb-in-range-0-255-for-both
	/// </summary>
	[DataContract]
	public class HsvShifter {
		public static Color HsvToRgb(double hue, double saturation, double value) {
			double r, g, b;
			if (saturation <= 0) {
				r = g = b = value;
			} else {
				hue = hue % 360 / 60d;
				byte i = (byte)hue;
				double ff = hue - i;
				double p = value * (1d - saturation);
				double q = value * (1d - (saturation * ff));
				double t = value * (1d - (saturation * (1d - ff)));

				switch (i) {
					case 0:
						r = value; g = t; b = p;
						break;
					case 1:
						r = q; g = value; b = p;
						break;
					case 2:
						r = p; g = value; b = t;
						break;
					case 3:
						r = p; g = q; b = value;
						break;
					case 4:
						r = t; g = p; b = value;
						break;
					case 5:
					default:
						r = value; g = p; b = q;
						break;
				}
			}
			return Color.FromArgb((byte)Round(r * 255), (byte)Round(g * 255), (byte)Round(b * 255));
		}

		public HsvShifter() { }

		public HsvShifter(double hueShift, double saturationScale, double valueScale,
			double alphaScale = 1) {

			this.HueShift = hueShift;
			this.SaturationScale = saturationScale;
			this.ValueScale = valueScale;
			this.AlphaScale = alphaScale;
		}
		
		[DataMember(Name = "hue")]
		public double HueShift {
			get; private set;
		}
		[DataMember(Name = "saturation")]
		public double SaturationScale {
			get; private set;
		}
		[DataMember(Name = "value")]
		public double ValueScale {
			get; private set;
		}
		[DataMember(Name = "alpha")]
		public double AlphaScale {
			get; private set;
		}

		[IgnoreDataMember]
		private double VSU;
		[IgnoreDataMember]
		private double VSW;

		public Color Shift(Color color) {
			this.VSU = this.ValueScale * this.SaturationScale * Cos(this.HueShift * PI / 180);
			this.VSW = this.ValueScale * this.SaturationScale * Sin(this.HueShift * PI / 180);

			return Color.FromArgb(
				this.AlphaScale == 1 ? color.A : ((byte)Max(0, Min(255, (int)(this.AlphaScale * color.A)))),
				(byte)Max(0, Min(255, (int)((.299 * this.ValueScale + .701 * this.VSU + .168 * this.VSW) * color.R
				+ (.587 * this.ValueScale - .587 * this.VSU + .330 * this.VSW) * color.G
				+ (.114 * this.ValueScale - .114 * this.VSU - .497 * this.VSW) * color.B))),
				(byte)Max(0, Min(255, (int)((.299 * this.ValueScale - .299 * this.VSU - .328 * this.VSW) * color.R
				+ (.587 * this.ValueScale + .413 * this.VSU + .035 * this.VSW) * color.G
				+ (.114 * this.ValueScale - .114 * this.VSU + .292 * this.VSW) * color.B))),
				(byte)Max(0, Min(255, (int)((.299 * this.ValueScale - .3 * this.VSU + 1.25 * this.VSW) * color.R
				+ (.587 * this.ValueScale - .588 * this.VSU - 1.05 * this.VSW) * color.G
				+ (.114 * this.ValueScale + .886 * this.VSU - .203 * this.VSW) * color.B)))
			);
		}

		private void Shift(byte[] array, int start, byte mask) {
			if (mask == 0) {
				return;
			}
			
			byte inb = array[start], ing = array[start + 1], inr = array[start + 2],
				ina = array[start + 3];
			byte outr = (byte)Max(0, Min(255, (int)(
				(.299 * this.ValueScale + .701 * this.VSU + .168 * this.VSW) * inr +
				(.587 * this.ValueScale - .587 * this.VSU + .330 * this.VSW) * ing +
				(.114 * this.ValueScale - .114 * this.VSU - .497 * this.VSW) * inb
			)));
			byte outg = (byte)Max(0, Min(255, (int)(
				(.299 * this.ValueScale - .299 * this.VSU - .328 * this.VSW) * inr +
				(.587 * this.ValueScale + .413 * this.VSU + .035 * this.VSW) * ing +
				(.114 * this.ValueScale - .114 * this.VSU + .292 * this.VSW) * inb
			)));
			byte outb = (byte)Max(0, Min(255, (int)(
				(.299 * this.ValueScale - .3 * this.VSU + 1.25 * this.VSW) * inr +
				(.587 * this.ValueScale - .588 * this.VSU - 1.05 * this.VSW) * ing +
				(.114 * this.ValueScale + .886 * this.VSU - .203 * this.VSW) * inb
			)));
			byte outa = this.AlphaScale == 1 ? ina : ((byte)Max(0, Min(255, (int)(
				this.AlphaScale * ina
			))));

			if (mask == 255) {
				array[start] = outb; array[start + 1] = outg; array[start + 2] = outr;
				array[start + 3] = outa;
				return;
			}

			array[start] = (byte)((outb * mask + inb * ~mask) / byte.MaxValue);
			array[start + 1] = (byte)((outg * mask + ing * ~mask) / byte.MaxValue);
			array[start + 2] = (byte)((outr * mask + inr * ~mask) / byte.MaxValue);
			if (this.AlphaScale != 1) {
				array[start + 3] = (byte)((outa * mask + ina * ~mask) / byte.MaxValue);
			}
		}

		public void Shift(Bitmap bitmap, Bitmap mask = null) {
			this.VSU = this.ValueScale * this.SaturationScale * Cos(this.HueShift * PI / 180);
			this.VSW = this.ValueScale * this.SaturationScale * Sin(this.HueShift * PI / 180);

			Bitmap activeMask = mask;
			try {
				if (mask == null) {
					activeMask = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
					using Graphics draw = Graphics.FromImage(activeMask);
					draw.Clear(Color.Black);
				}

				lock (bitmap) {
					BitmapData data = bitmap.LockBits(bitmap.GetBounds(),
						ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
					try {
						int dataLength = data.Stride * data.Height;
						byte[] dataBytes = new byte[dataLength];
						Marshal.Copy(data.Scan0, dataBytes, 0, dataLength);

						lock (activeMask) {
							BitmapData maskData = activeMask.LockBits(activeMask.GetBounds(),
								ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
							try {
								int maskLength = maskData.Stride * maskData.Height;
								byte[] maskBytes = new byte[maskLength];
								Marshal.Copy(maskData.Scan0, maskBytes, 0, maskLength);

								int linePointer = 0;
								int maskLinePointer = 0;
								int pixelPointer;
								int maskPixelPointer;
								for (int y = 0; y < Min(data.Height, maskData.Height); y++) {
									pixelPointer = linePointer;
									maskPixelPointer = maskLinePointer;
									for (int x = 0; x < Min(data.Width, maskData.Width); x++) {
										this.Shift(dataBytes, pixelPointer, maskBytes[maskPixelPointer + 3]);
										pixelPointer += 4;
										maskPixelPointer += 4;
									}
									linePointer += data.Stride;
									maskLinePointer += maskData.Stride;
								}

								Marshal.Copy(dataBytes, 0, data.Scan0, dataLength);
							} finally {
								activeMask.UnlockBits(maskData);
							}
						}
					} finally {
						bitmap.UnlockBits(data);
					}
				}
			} finally {
				if (mask == null) {
					activeMask.Dispose();
				}
			}
		}
	}
}
