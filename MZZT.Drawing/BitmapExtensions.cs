using MZZT.WinApi.PInvoke;
using System;
using System.Drawing;

namespace MZZT.Extensions {
	public static class BitmapExtensions {
		public static Icon ToIcon(this Bitmap bitmap) {
			IntPtr handle = bitmap.GetHicon();
			try {
				return Icon.FromHandle(handle).Clone() as Icon;
			} finally {
				User32.DestroyIcon(handle);
			}
		}
	}
}
