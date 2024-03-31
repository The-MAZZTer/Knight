using System;
using System.Runtime.InteropServices;

namespace MZZT.WinApi.PInvoke {
	public static class Shell32 {
		[StructLayout(LayoutKind.Sequential)]
		public struct SHFILEINFO {
			public IntPtr hIcon;
			public int iIcon;
			public int dwAttributes;
			public IntPtr szDisplayName;
			public IntPtr szTypeName;
		}

		[Flags]
		public enum SHGFI : uint {
			ADDOVERLAYS = 0x000000020,
			ATTR_SPECIFIED = 0x000020000,
			ATTRIBUTES = 0x000000800,
			DISPLAYNAME = 0x000000200,
			EXETYPE = 0x000002000,
			ICON = 0x000000100,
			ICONLOCATION = 0x000001000,
			LARGEICON = 0x000000000,
			LINKOVERLAY = 0x000008000,
			OPENICON = 0x000000002,
			OVERLAYINDEX = 0x000000040,
			PIDL = 0x000000008,
			SELECTED = 0x000010000,
			SHELLICONSIZE = 0x000000004,
			SMALLICON = 0x000000001,
			SYSICONINDEX = 0x000004000,
			TYPENAME = 0x000000400,
			USEFILEATTRIBUTES = 0x000000010
		}

		[DllImport("shell32.dll", EntryPoint = "ExtractIconEx",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static uint ExtractIconEx(string lpszFile, int nIconIndex,
			[Optional] IntPtr[] phiconLarge, [Optional] IntPtr[] phiconSmall, uint nIcons);

		[DllImport("shell32.dll", EntryPoint = "SHGetFileInfo",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static int SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi,
			uint cbFileInfo, SHGFI uFlags);
	}
}
