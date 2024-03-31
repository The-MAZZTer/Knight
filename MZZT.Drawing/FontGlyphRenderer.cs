using MZZT.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;

namespace MZZT.Drawing {
	public class FontGlyphRenderer(FontFamily fontFamily, FontStyle fontStyle = FontStyle.Regular) {
		public FontFamily FontFamily { get; set; } = fontFamily;
		public FontStyle FontStyle { get; set; } = fontStyle;

		private (Bitmap, Graphics) CreateBitmapAndGraphics(Size bitmapSize) {
			Bitmap bitmap = new(bitmapSize.Width, bitmapSize.Height, PixelFormat.Format32bppArgb);
			try {
				Graphics graphics = Graphics.FromImage(bitmap);
				try {
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
					graphics.SmoothingMode = SmoothingMode.AntiAlias;
					graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
					return (bitmap, graphics);
				} catch {
					graphics.Dispose();
					throw;
				}
			} catch {
				bitmap.Dispose(); 
				throw;
			}
		}

		private StringFormat StringFormat => new() {
			Alignment = StringAlignment.Center,
			FormatFlags = StringFormatFlags.NoFontFallback,
			HotkeyPrefix = HotkeyPrefix.None,
			LineAlignment = StringAlignment.Center,
			Trimming = StringTrimming.None
		};

		private void DrawBitmapGlyph(Graphics graphics, Size bitmapSize, string text, Color color, float fontSize, GraphicsUnit unit) {
			using Font font = new(this.FontFamily, fontSize, this.FontStyle, unit);
			using Brush brush = new SolidBrush(color);
			graphics.TranslateTransform(bitmapSize.Width * 0.0146484375f, bitmapSize.Height * 0.06640625f);
			graphics.DrawString(text, font, brush, new RectangleF(PointF.Empty, bitmapSize), this.StringFormat);
		}

		public Bitmap DrawBitmapGlyph(Size bitmapSize, string text, Color color, float fontSize, GraphicsUnit unit = GraphicsUnit.Point) {
			(Bitmap bitmap, Graphics graphics) = this.CreateBitmapAndGraphics(bitmapSize);
			try {
				using (graphics) {
					this.DrawBitmapGlyph(graphics, bitmapSize, text, color, fontSize, unit);
					return bitmap;
				}
			} catch {
				bitmap.Dispose();
				throw;
			}
		}

		public Bitmap DrawBitmapGlyph(Size bitmapSize, string text, Color color) {
			(Bitmap bitmap, Graphics graphics) = this.CreateBitmapAndGraphics(bitmapSize);
			try {
				using (graphics) {
					float minSize = 0;
					float maxSize = bitmapSize.Height + 1;

					while (minSize + 1 < maxSize) {
						float testSize = (minSize + maxSize) / 2;
						using Font font = new(this.FontFamily, testSize, this.FontStyle, GraphicsUnit.Pixel);
						SizeF result = graphics.MeasureString(text, font, bitmapSize.Width, this.StringFormat);
						if (result.Width > bitmapSize.Width || result.Height > bitmapSize.Height) {
							maxSize = testSize;
						} else {
							minSize = testSize;
						}
					}

					this.DrawBitmapGlyph(graphics, bitmapSize, text, color, minSize, GraphicsUnit.Pixel);
				}
				return bitmap;
			} catch {
				bitmap.Dispose();
				throw;
			}
		}

		public Icon DrawIconGlyph(Size iconSize, string text, Color color, float fontSize, GraphicsUnit unit = GraphicsUnit.Point) {
			using Bitmap bitmap = this.DrawBitmapGlyph(iconSize, text, color, fontSize, unit);
			return bitmap.ToIcon();
		}

		public Icon DrawIconGlyph(Size iconSize, string text, Color color) {
			using Bitmap bitmap = this.DrawBitmapGlyph(iconSize, text, color);
			return bitmap.ToIcon();
		}

		public Bitmap DrawBitmapGlyph(Size iconSize, IEnumerable<string> glyphs, IEnumerable<Color> colors) {
			Bitmap[] bitmaps = glyphs.Zip(colors, (x, y) => (x, y)).Select(x => this.DrawBitmapGlyph(iconSize, x.x, x.y)).ToArray();
			Bitmap final = null;
			Graphics g = null;
			try {
				(final, g) = this.CreateBitmapAndGraphics(iconSize);
				g.CompositingMode = CompositingMode.SourceCopy;
				foreach (Bitmap bitmap in bitmaps) {
					g.DrawImageUnscaled(bitmap, Point.Empty);
					g.CompositingMode = CompositingMode.SourceOver;
				}
				return final;
			} catch {
				final?.Dispose();
				g?.Dispose();
				throw;
			} finally {
				foreach (Bitmap bitmap in bitmaps) {
					bitmap.Dispose();
				}
			}
		}

		public Bitmap DrawBitmapGlyph(Size iconSize, IEnumerable<string> glyphs, IEnumerable<Color> colors, float fontSize, GraphicsUnit unit = GraphicsUnit.Point) {
			Bitmap[] bitmaps = glyphs.Zip(colors, (x, y) => (x, y)).Select(x => this.DrawBitmapGlyph(iconSize, x.x, x.y, fontSize, unit)).ToArray();
			Bitmap final = null;
			Graphics g = null;
			try {
				(final, g) = this.CreateBitmapAndGraphics(iconSize);
				g.CompositingMode = CompositingMode.SourceCopy;
				foreach (Bitmap bitmap in bitmaps) {
					g.DrawImageUnscaled(bitmap, Point.Empty);
					g.CompositingMode = CompositingMode.SourceOver;
				}
				return final;
			} catch {
				final?.Dispose();
				g?.Dispose();
				throw;
			} finally {
				foreach (Bitmap bitmap in bitmaps) {
					bitmap.Dispose();
				}
			}
		}
		public Icon DrawIconGlyph(Size iconSize, IEnumerable<string> glyphs, IEnumerable<Color> colors, float fontSize, GraphicsUnit unit = GraphicsUnit.Point) {
			using Bitmap bitmap = this.DrawBitmapGlyph(iconSize, glyphs, colors, fontSize, unit);
			return bitmap.ToIcon();
		}

		public Icon DrawIconGlyph(Size iconSize, IEnumerable<string> glyphs, IEnumerable<Color> colors) {
			using Bitmap bitmap = this.DrawBitmapGlyph(iconSize, glyphs, colors);
			return bitmap.ToIcon();
		}
	}
}
