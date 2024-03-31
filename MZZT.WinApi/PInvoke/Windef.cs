using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MZZT.WinApi.PInvoke {
	public static class Windef {
		public enum HT {
			BORDER = 18,
			BOTTOM = 15,
			BOTTOMLEFT = 16,
			BOTTOMRIGHT = 17,
			CAPTION = 2,
			CLIENT = 1,
			CLOSE = 20,
			ERROR = -2,
			GROWBOX = 4,
			HELP = 21,
			HSCROLL = 6,
			LEFT = 10,
			MENU = 5,
			MAXBUTTON = 9,
			MINBUTTON = 8,
			NOWHERE = 0,
			REDUCE = 8,
			RIGHT = 11,
			SIZE = 4,
			SYSMENU = 3,
			TOP = 12,
			TOPLEFT = 13,
			TOPRIGHT = 14,
			TRANSPARENT = -1,
			VSCROLL = 7,
			ZOOM = 9
		}

		public enum IMAGE_SUBSYSTEM : short {
			UNKNOWN = 0x0,
			NATIVE = 0x1,
			WINDOWS_GUI = 0x2,
			WINDOWS_CUI = 0x3,
			OS2_CUI = 0x5,
			POSIX_CUI = 0x7,
			WINDOWS_CE_GUI = 0x9,
			EFI_APPLICATION = 0xA,
			EFI_BOOT_SERVICE_DRIVER = 0xB,
			EFI_RUNTIME_DRIVER = 0xC,
			EFI_ROM = 0xD,
			XBOX = 0xE,
			WINDOWS_BOOT_APPLICATION = 0x10
		}
		
		[Flags]
		public enum KeyState {
			LMB = 0x1,
			RMB = 0x2,
			Shift = 0x4,
			Ctrl = 0x8,
			MMB = 0x10,
			Alt = 0x20
		}

		public const int MAX_PATH = 260;

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT {
			public POINT(Point point) {
				this.x = point.X;
				this.y = point.Y;
			}

			public int x;
			public int y;

			public Point ToPoint() => new Point(this.x, this.y);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public RECT(Rectangle rect) {
				this.left = rect.Left;
				this.top = rect.Top;
				this.right = rect.Left + rect.Width;
				this.bottom = rect.Top + rect.Height;
			}

			public int left;
			public int top;
			public int right;
			public int bottom;

			public Rectangle ToRectangle() => Rectangle.FromLTRB(this.left, this.top, this.right, this.bottom);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SIZE {
			public SIZE(Size size) {
				this.cx = size.Width;
				this.cy = size.Height;
			}

			public int cx;
			public int cy;

			public Size ToSize() =>new Size(this.cx, this.cy);
		}
	}
}
