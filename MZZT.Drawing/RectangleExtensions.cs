using System.Drawing;
using System.Drawing.Printing;

namespace MZZT.Extensions {
	public static class RectangleExtensions {
		public static PointF GetCenter(this Rectangle me) {
			return new PointF(me.Left + me.Width / 2f, me.Top + me.Height / 2f);
		}

		public static Rectangle Deflate(this Rectangle me, Margins padding) {
			me.X += padding.Left;
			me.Y += padding.Top;
			me.Width -= padding.Left + padding.Right;
			me.Height -= padding.Top + padding.Bottom;
			return me;
		}
	}
}
