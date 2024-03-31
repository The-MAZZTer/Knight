using System.Runtime.InteropServices;

namespace MZZT.WinApi.PInvoke {
	public static class PsApi {
		[DllImport("Psapi.dll", SetLastError = true)]
		public static extern bool EnumProcesses(
			[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In][Out]
			uint[] lpidProcess, uint cb,
			[MarshalAs(UnmanagedType.U4)] out uint lpcbNeeded
		);
	}
}
