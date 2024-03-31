using System;
using System.Runtime.InteropServices;

namespace MZZT.WinApi.PInvoke {
	public static class Ntdll {
		public enum NTSTATUS : uint {
			SUCCESS = 0x0,
			ACCESS_DENIED = 0xC0000022
		}

		public enum PROCESSINFOCLASS {
			ProcessBasicInformation = 0
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct PROCESS_BASIC_INFORMATION {
			public IntPtr ExitStatus;
			public IntPtr PebBaseAddress;
			public IntPtr AffinityMask;
			public IntPtr BasePriority;
			public UIntPtr UniqueProcessId;
			public IntPtr InheritedFromUniqueProcessId;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PEB {
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] Reserved1;
			public byte BeingDebugged;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
			public byte[] Reserved2;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public IntPtr[] Reserved3;
			public IntPtr Ldr;
			public IntPtr ProcessParameters;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 104)]
			public byte[] Reserved4;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
			public IntPtr[] Reserved5;
			public IntPtr PostProcessInitRoutine;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
			public byte[] Reserved6;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
			public IntPtr[] Reserved7;
			public uint SessionId;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RTL_USER_PROCESS_PARAMETERS {
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] Reserved1;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
			public IntPtr[] Reserved2;
			public int ImagePathNameLength;
			public IntPtr ImagePathName;
			public int CommandLineLength;
			public IntPtr CommandLine;
		}

		[DllImport("ntdll.dll", EntryPoint = "ZwQueryInformationProcess",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern NTSTATUS ZwQueryInformationProcess(IntPtr ProcessHandle,
			PROCESSINFOCLASS ProcessInformationClass, IntPtr ProcessInformation,
			uint ProcessInformationLength, out uint ReturnLength);

		[DllImport("ntdll.dll", EntryPoint = "ZwReadVirtualMemory",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern NTSTATUS ZwReadVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress,
			IntPtr Buffer, uint NumberOfBytesToRead, [Optional] out uint NumberOfBytesReaded);
	}
}
