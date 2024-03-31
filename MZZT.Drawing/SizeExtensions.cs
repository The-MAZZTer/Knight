using System.Drawing;

namespace MZZT.Extensions {
	public static class SizeExtensions {
		public static RectangleF ScaleToContainInside(this Size source, Size container) {
			float sourceAspect = source.Width / (float)source.Height;
			float destAspect = container.Width / (float)container.Height;
			SizeF finalSize = container;
			if (destAspect > sourceAspect) {
				finalSize.Width = (float)container.Height / source.Height * source.Width;
			} else if (sourceAspect > destAspect) {
				finalSize.Height = (float)container.Width / source.Width * source.Height;
			}

			return new RectangleF(new PointF((container.Width - finalSize.Width) / 2,
				(container.Height - finalSize.Height) / 2), finalSize);
		}
	}
}
