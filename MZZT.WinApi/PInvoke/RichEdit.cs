using System;
using System.Runtime.InteropServices;
using static MZZT.WinApi.PInvoke.CommCtrl;

namespace MZZT.WinApi.PInvoke {
	public static class RichEdit {
		[StructLayout(LayoutKind.Sequential)]
		public struct NMHDR {
			public IntPtr hwndFrom;
			public uint idFrom;
			public NM code;
		}
	}
}
