using System;
using System.Runtime.InteropServices;

namespace MZZT.WinApi.PInvoke {
	public static class Kernel32 {
		public enum DRIVE : uint {
			UNKNOWN = 0x0,
			NO_ROOT_DIR = 0x1,
			REMOVABLE = 0x2,
			FIXED = 0x3,
			REMOTE = 0x4,
			CDROM = 0x5,
			RAMDISK = 0x6
		}

		[Flags]
		public enum FILE {
			CASE_SENSITIVE_SEARCH = 0x1,
			CASE_PRESERVED_NAMES = 0x2,
			UNICODE_ON_DISK = 0x4,
			PERSISTENT_ACLS = 0x8,
			FILE_COMPRESSION = 0x10,
			VOLUME_QUOTAS = 0x20,
			SUPPORTS_SPARSE_FILES = 0x40,
			SUPPORTS_REPARSE_POINTS = 0x80,
			VOLUME_IS_COMPRESSED = 0x8000,
			SUPPORTS_OBJECT_IDS = 0x10000,
			SUPPORTS_ENCRYPTION = 0x20000,
			NAMED_STREAMS = 0x40000,
			READ_ONLY_VOLUME = 0x80000,
			SEQUENTIAL_WRITE_ONCE = 0x100000,
			SUPPORTS_TRANSACTIONS = 0x200000,
			SUPPORTS_HARD_LINKS = 0x400000,
			SUPPORTS_EXTENDED_ATTRIBUTES = 0x800000,
			SUPPORTS_OPEN_BY_FILE_ID = 0x1000000,
			SUPPORTS_USN_JOURNAL = 0x2000000,
			DAX_VOLUME = 0x20000000
		}

		[Flags]
		public enum PROCESS {
			TERMINATE = 0x1,
			CREATE_THREAD = 0x2,
			VM_OPERATION = 0x8,
			VM_READ = 0x10,
			VM_WRITE = 0x20,
			DUP_HANDLE = 0x40,
			CREATE_PROCESS = 0x80,
			SET_QUOTE = 0x100,
			SET_INFORMATION = 0x200,
			QUERY_INFORMATION = 0x400,
			SUSPEND_RESUME = 0x800,
			QUERY_LIMITED_INFORMATION = 0x1000
		}

		[Flags]
		public enum PROCESS_CREATION {
			CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
			CREATE_DEFAULT_ERROR_MODE = 0x04000000,
			CREATE_NEW_CONSOLE = 0x00000010,
			CREATE_NEW_PROCESS_GROUP = 0x00000200,
			CREATE_NO_WINDOW = 0x08000000,
			CREATE_PROTECTED_PROCESS = 0x00040000,
			CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
			CREATE_SEPARATE_WOW_VDM = 0x00000800,
			CREATE_SHARED_WOW_VDM = 0x00001000,
			CREATE_SUSPENDED = 0x00000004,
			CREATE_UNICODE_ENVIRONMENT = 0x00000400,
			DEBUG_ONLY_THIS_PROCESS = 0x00000002,
			DEBUG_PROCESS = 0x00000001,
			DETACHED_PROCESS = 0x00000008,
			EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
			INHERIT_PARENT_AFFINITY = 0x00010000
		}

		public enum PROCESS_NAME {
			_0 = 0x0,
			NATIVE = 0x1
		}

		[Flags]
		public enum STARTF {
			FORCEONFEEDBACK = 0x00000040,
			FORCEOFFFEEDBACK = 0x00000080,
			PREVENTPINNING = 0x00002000,
			RUNFULLSCREEN = 0x00000020,
			TITLEISAPPID = 0x00001000,
			TITLEISLINKNAME = 0x00000800,
			UNTRUSTEDSOURCE = 0x00008000,
			USECOUNTCHARS = 0x00000008,
			USEFILLATTRIBUTE = 0x00000010,
			USEHOTKEY = 0x00000200,
			USEPOSITION = 0x00000004,
			USESHOWWINDOW = 0x00000001,
			USESIZE = 0x00000002,
			USESTDHANDLES = 0x00000100
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PROCESS_INFORMATION {
			public IntPtr hProcess;
			public IntPtr hThread;
			public int dwProcessId;
			public int dwThreadId;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SECURITY_ATTRIBUTES {
			public int nLength;
			public IntPtr lpSecurityDescriptor;
			public bool bInheritHandle;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct STARTUPINFO {
			public int cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public int dwX;
			public int dwY;
			public int dwXSize;
			public int dwYSize;
			public int dwXCountChars;
			public int dwYCountChars;
			public int dwFillAttribute;
			public STARTF dwFlags;
			public short wShowWindow;
			public short cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}

		[DllImport("kernel32.dll", EntryPoint = "CloseHandle",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", EntryPoint = "CreateProcess",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CreateProcess([Optional] string lpApplicationName,
			[Out, Optional] string lpCommandLine, [Optional] IntPtr lpProcessAttributes,
			[Optional] IntPtr lpThreadAttributes, bool bInheritHandles,
			PROCESS_CREATION dwCreationFlags, [Optional] string lpEnvironment,
			[Optional] string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo,
			[Out] out PROCESS_INFORMATION lpProcessInformation);

		[DllImport("kernel32.dll", EntryPoint = "GetCurrentProcessId",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetCurrentProcessId();

		[DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceEx",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetDiskFreeSpaceEx([Optional] string lpDirectoryName,
			[Optional] out ulong lpFreeBytesAvailable, [Optional] out ulong lpTotalNumberOfBytes,
			[Optional] out ulong lpTotalNumberOfFreeBytes);

		[DllImport("kernel32.dll", EntryPoint = "GetDriveType",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern DRIVE GetDriveType([Optional] string lpRootPathName);

		[DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName,
			string lpDefault, [Out] IntPtr lpReturnedString, int nSize, string lpFileName);

		[DllImport("kernel32.dll", EntryPoint = "GetShortPathName",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetShortPathName(string lpszLongPath, string lpszShortPath,
			int cchBuffer);

		[DllImport("kernel32.dll", EntryPoint = "GetVolumeInformation",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetVolumeInformation([Optional] string lpRootPathName,
			[Optional] IntPtr lpVolumeNameBuffer, int nVolumeNameSize,
			[Optional] out int lpVolumeSerialNumber, [Optional] out int lpMaximumComponentLength,
			[Optional] out FILE lpFileSystemFlags, [Optional] IntPtr lpFileSystemNameBuffer,
			int nFileSystemNameSize);

		[DllImport("kernel32.dll", EntryPoint = "GetVolumePathName",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetVolumePathName(string lpszFileName, IntPtr lpszVolumePathName,
			int cchBufferLength);
		
		[DllImport("kernel32.dll", EntryPoint = "GlobalAddAtom",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short GlobalAddAtom(string lpString);

		[DllImport("kernel32.dll", EntryPoint = "GlobalDeleteAtom",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short GlobalDeleteAtom(short uAtom);

		[DllImport("kernel32.dll", EntryPoint = "OpenProcess",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr OpenProcess(PROCESS dwDesiredAccess, bool bInheritHandle,
			int dwProcessId);

		[DllImport("kernel32.dll", EntryPoint = "QueryFullProcessImageName",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool QueryFullProcessImageName(IntPtr hProcess, PROCESS_NAME dwFlags,
			IntPtr lpExeName, out int dwSize);

		[DllImport("kernel32.dll", EntryPoint = "SearchPath",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int SearchPath([Optional] string lpPath, string lpFilename,
			string lpExtension, int nBufferLength, [Out] IntPtr lpBuffer,
			[Out, Optional] IntPtr lpFilePart);
	}
}
