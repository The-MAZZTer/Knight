using System;
using System.Collections.Generic;
using System.Management;

namespace MZZT.WinApi.Wmi {
	public class Win32_Process : WmiBase {
		public static IEnumerable<Win32_Process> Processes {
			get {
				return GetCache<Win32_Process>();
			}
		}

		public static void Refresh() {
			Refresh<Win32_Process>();
		}

		protected Win32_Process(ManagementObject obj) : base(obj) { }

		public string CreationClassName { get; private set; }
		public string Caption { get; private set; }
		public string CommandLine { get; private set; }
		public DateTime CreationDate { get; private set; }
		public string CSCreationClassName { get; private set; }
		public string CSName { get; private set; }
		public string Description { get; private set; }
		public string ExecutablePath { get; private set; }
		public ushort ExecutionState { get; private set; }
		public string Handle { get; private set; }
		public uint HandleCount { get; private set; }
		public DateTime InstallDate { get; private set; }
		public ulong KernelModeTime { get; private set; }
		public uint MaximumWorkingSetSize { get; private set; }
		public uint MinimumWorkingSetSize { get; private set; }
		public string Name { get; private set; }
		public string OSCreationClassName { get; private set; }
		public string OSName { get; private set; }
		public ulong OtherOperationCount { get; private set; }
		public ulong OtherTransferCount { get; private set; }
		public uint PageFaults { get; private set; }
		public uint PageFileUsage { get; private set; }
		public uint ParentProcessId { get; private set; }
		public uint PeakPageFileUsage { get; private set; }
		public ulong PeakVirtualSize { get; private set; }
		public uint PeakWorkingSetSize { get; private set; }
		public uint Priority { get; private set; }
		public ulong PrivatePageCount { get; private set; }
		public uint ProcessId { get; private set; }
		public uint QuotaNonPagedPoolUsage { get; private set; }
		public uint QuotaPagedPoolUsage { get; private set; }
		public uint QuotaPeakNonPagedPoolUsage { get; private set; }
		public uint QuotaPeakPagedPoolUsage { get; private set; }
		public ulong ReadOperationCount { get; private set; }
		public ulong ReadTransferCount { get; private set; }
		public uint SessionId { get; private set; }
		public string Status { get; private set; }
		public DateTime TerminationDate { get; private set; }
		public uint ThreadCount { get; private set; }
		public ulong UserModeTime { get; private set; }
		public ulong VirtualSize { get; private set; }
		public string WindowsVersion { get; private set; }
		public ulong WorkingSetSize { get; private set; }
		public ulong WriteOperationCount { get; private set; }
		public ulong WriteTransferCount { get; private set; }
	}
}
