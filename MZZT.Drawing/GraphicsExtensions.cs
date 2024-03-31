using System.Drawing;
using System.Drawing.Imaging;

namespace MZZT.Extensions {
	public static class GraphicsExtensions {
		/// <summary>
		/// Missing function declaration in the standard list.
		/// </summary>
		/// <param name="g">Graphics object to invoke DrawImage on.</param>
		/// <param name="image">Image to draw.</param>
		/// <param name="destRect">Destination rectangle to draw to.</param>
		/// <param name="srcRect">Source rectangle to draw with,</param>
		/// <param name="srcUnit">Units the rectangle numbers are in.</param>
		/// <param name="imageAttr">ImageAttributes to use.</param>
		public static void DrawImage(this Graphics g, Image image, Rectangle destRect,
			Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr) {

			g.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height,
				srcUnit, imageAttr);
		}
	}
}
