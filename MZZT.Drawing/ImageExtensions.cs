using System.Drawing;
using System.Drawing.Drawing2D;
using static System.Math;

namespace MZZT.Extensions {
	public static class ImageExtensions {
		/// <summary>
		/// Convenience method to get a Rectangle representing the bounds of the image.
		/// </summary>
		/// <param name="image">The image to get the bounds of.</param>
		/// <returns>Rectangle for the bounds.</returns>
		public static Rectangle GetBounds(this Image image) {
			return new Rectangle(Point.Empty, image.Size);
		}

		public static Bitmap ScaleToContainInside(this Image image, Size container) {
			Size imageSize;
			lock (image) {
				imageSize = image.Size;
			}

			if (imageSize.Width <= container.Width && imageSize.Height <= container.Height &&
				(imageSize.Width == container.Width || imageSize.Height == container.Height)) {

				lock (image) {
					return new Bitmap(image);
				}
			}

			Size finalSize = Size.Round(imageSize.ScaleToContainInside(container).Size);
			Bitmap pixelBitmap = new(finalSize.Width, finalSize.Height, image.PixelFormat);
			using (Graphics draw = Graphics.FromImage(pixelBitmap)) {
				draw.CompositingMode = CompositingMode.SourceCopy;
				draw.CompositingQuality = CompositingQuality.HighQuality;
				draw.InterpolationMode = InterpolationMode.HighQualityBicubic;
				draw.PixelOffsetMode = PixelOffsetMode.HighQuality;
				draw.SmoothingMode = SmoothingMode.HighQuality;

				lock (image) {
					draw.DrawImage(image, pixelBitmap.GetBounds());
				}
			}
			return pixelBitmap;
		}

		public static Bitmap ScaleToContainInside(this Image image, Size container, bool pixelate) {
			if (!pixelate) {
				return ScaleToContainInside(image, container);
			}

			Size imageSize;
			lock (image) {
				imageSize = image.Size;
			}

			Size finalSize = Size.Round(imageSize.ScaleToContainInside(container).Size);
			int scalex = (int)Ceiling((double)finalSize.Width / imageSize.Width);
			int scaley = (int)Ceiling((double)finalSize.Height / imageSize.Height);
			if (scalex == 1 && scaley == 1) {
				return ScaleToContainInside(image, finalSize);
			}

			Bitmap pixelBitmap = new(imageSize.Width * scalex, imageSize.Height * scaley,
				image.PixelFormat);
			using (Graphics draw = Graphics.FromImage(pixelBitmap)) {
				draw.CompositingMode = CompositingMode.SourceCopy;
				draw.CompositingQuality = CompositingQuality.AssumeLinear;
				draw.InterpolationMode = InterpolationMode.NearestNeighbor;
				draw.PixelOffsetMode = PixelOffsetMode.Half;
				draw.SmoothingMode = SmoothingMode.None;

				lock (image) {
					draw.DrawImage(image, pixelBitmap.GetBounds());
				}
			}

			if (pixelBitmap.Size.Equals(finalSize)) {
				return pixelBitmap;
			}

			using (pixelBitmap) {
				return ScaleToContainInside(pixelBitmap, finalSize);
			}
		}
	}
}
