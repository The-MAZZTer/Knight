using static MZZT.WinApi.PInvoke.Kernel32;

namespace MZZT.Extensions {
	public static class PathExtensions {
		public static string GetShortName(string path) {
			int size = GetShortPathName(path, null, 0);
			if (size <= 0) {
				return path;
				//throw new Win32Exception();
			}

			string output = new(' ', size - 1);
			if (GetShortPathName(path, output, size) <= 0) {
				return path;
				//throw new Win32Exception();
			}

			return output;
		}
	}
}
