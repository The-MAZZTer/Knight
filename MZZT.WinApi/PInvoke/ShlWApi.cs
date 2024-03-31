using System;
using System.Runtime.InteropServices;

namespace MZZT.WinApi.PInvoke {
	public static class ShlWApi {
		[DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = false)]
		public static extern bool PathFindOnPath(IntPtr pszFile, string[] ppszOtherDirs);
	}
}
