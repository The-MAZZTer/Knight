using MZZT.WinApi.Wmi;
using MZZT.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static MZZT.WinApi.PInvoke.Kernel32;
using static MZZT.WinApi.PInvoke.Ntdll;
using static MZZT.WinApi.PInvoke.PsApi;

namespace MZZT.Diagnostics {
	public class Process(int id) : IEquatable<Process>, IEquatable<System.Diagnostics.Process> {
		private static Process current = null;
		public static Process Current {
			get {
				if (current == null) {
					int handle = GetCurrentProcessId();
					if (handle == 0) {
						return null;
					}
					current = new Process(handle);
				}
				return current;
			}
		}

		public static IEnumerable<Process> All {
			get {
				uint[] handles = new uint[1024];
				if (!EnumProcesses(handles, (uint)handles.Length, out uint got)) {
					throw new Win32Exception();
				}
				foreach (uint handle in handles.Where(x => (int)x != 0).Take((int)got)) {
					yield return new Process((int)handle);
				}
			}
		}

		public static Process Start(string filename, string commandLine = null,
			string workingDirectory = null) {

			return Create(filename, commandLine, workingDirectory, null);
		}

		internal static Process Create(string filename, string commandLine = null,
			string workingDirectory = null, Desktop desktop = null) {

			STARTUPINFO startupInfo = new() {
				cb = Marshal.SizeOf<STARTUPINFO>(),
				lpDesktop = desktop?.Name
			};
			if (!CreateProcess(filename, commandLine, IntPtr.Zero, IntPtr.Zero, false, 0, null,
				workingDirectory, ref startupInfo, out PROCESS_INFORMATION processInfo)) {

				throw new Win32Exception();
			}
			CloseHandle(processInfo.hProcess);
			CloseHandle(processInfo.hThread);
			return new Process(processInfo.dwProcessId);
		}

		public int Id {
			get; private set;
		} = id;

		public bool Equals(Process other) {
			if (other == null) {
				return false;
			}

			return this.Id == other.Id;
		}

		public bool Equals(System.Diagnostics.Process other) {
			if (other == null) {
				return false;
			}

			return this.Id == other.Id;
		}

		public override bool Equals(object obj) {
			if (obj is Process) {
				return this.Equals(obj as Process);
			}
			if (obj is System.Diagnostics.Process) {
				return this.Equals(obj as System.Diagnostics.Process);
			}
			return false;
		}

		public override int GetHashCode() {
			return this.Id;
		}

		public void Refresh() {
			this.gotCommandLine = false;
			this.gotFilename = false;
			if (this.gotWmi) {
				this.gotWmi = false;
				Win32_Process.Refresh();
			}
		}

		private string commandLine = null;
		private bool gotCommandLine = false;
		public string CommandLine {
			get {
				if (!this.gotCommandLine) {
					this.gotCommandLine = true;

					IntPtr handle = OpenProcess(PROCESS.QUERY_LIMITED_INFORMATION | PROCESS.VM_READ,
						false, this.Id);
					if (handle == IntPtr.Zero) {
						throw new Win32Exception();
					}
					try {
						int bufferSize = 8 * 1024;
						IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
						try {
							NTSTATUS ret = ZwQueryInformationProcess(handle,
								PROCESSINFOCLASS.ProcessBasicInformation, buffer,
								(uint)Marshal.SizeOf<PROCESS_BASIC_INFORMATION>(), out uint usize);
							if (ret != NTSTATUS.SUCCESS || usize == 0) {
								if (ret == NTSTATUS.ACCESS_DENIED) {
									throw new UnauthorizedAccessException();
								}
								throw new Win32Exception((int)ret);
							}
							PROCESS_BASIC_INFORMATION pbi =
								Marshal.PtrToStructure<PROCESS_BASIC_INFORMATION>(buffer);

							ret = ZwReadVirtualMemory(handle, pbi.PebBaseAddress, buffer,
								(uint)Marshal.SizeOf<PEB>(), out usize);
							if (ret != NTSTATUS.SUCCESS) {
								if (ret == NTSTATUS.ACCESS_DENIED) {
									throw new UnauthorizedAccessException();
								}
								throw new Win32Exception((int)ret);
							}
							PEB peb = Marshal.PtrToStructure<PEB>(buffer);

							ret = ZwReadVirtualMemory(handle, peb.ProcessParameters, buffer,
								(uint)Marshal.SizeOf<RTL_USER_PROCESS_PARAMETERS>(), out usize);
							if (ret != NTSTATUS.SUCCESS) {
								if (ret == NTSTATUS.ACCESS_DENIED) {
									throw new UnauthorizedAccessException();
								}
								throw new Win32Exception((int)ret);
							}
							RTL_USER_PROCESS_PARAMETERS process_params =
								Marshal.PtrToStructure<RTL_USER_PROCESS_PARAMETERS>(buffer);

							ret = ZwReadVirtualMemory(handle, process_params.CommandLine, buffer,
								(uint)bufferSize, out usize);
							if (ret != NTSTATUS.SUCCESS) {
								if (ret == NTSTATUS.ACCESS_DENIED) {
									throw new UnauthorizedAccessException();
								}
								throw new Win32Exception((int)ret);
							}

							this.commandLine = Marshal.PtrToStringUni(buffer);
						} finally {
							Marshal.FreeHGlobal(buffer);
						}
					} finally {
						CloseHandle(handle);
					}
				}
				return this.commandLine;
			}
		}

		private string filename = null;
		private bool gotFilename = false;
		public string ImageName {
			get {
				if (!this.gotFilename) {
					this.gotFilename = true;

					try {
						IntPtr handle = OpenProcess(PROCESS.QUERY_LIMITED_INFORMATION, false, this.Id);
						if (handle == IntPtr.Zero) {
							throw new Win32Exception();
						}
						try {
							IntPtr buffer = Marshal.AllocHGlobal(Marshal.SystemDefaultCharSize * 1024);
							try {
#pragma warning disable IDE0018 // Variable declaration can be inlined
								int capacity = 1024;
#pragma warning restore IDE0018
								if (!QueryFullProcessImageName(handle, PROCESS_NAME._0, buffer, out capacity)) {
									throw new Win32Exception();
								}
								this.filename = Marshal.PtrToStringAuto(buffer, capacity);
							} finally {
								Marshal.FreeHGlobal(buffer);
							}
						} finally {
							CloseHandle(handle);
						}
					} catch (Win32Exception) {
					}
				}
				return this.filename;
			}
		}

		public System.Diagnostics.Process ToNetProcess() => System.Diagnostics.Process.GetProcessById(this.Id);

		public Window MainWindow {
			get {
				IntPtr handle = this.ToNetProcess().MainWindowHandle;
				if (handle == IntPtr.Zero) {
					return null;
				}
				return new Window(handle);
			}
		}

		private Win32_Process wmi = null;
		private bool gotWmi = false;
		private Win32_Process Wmi {
			get {
				if (!this.gotWmi) {
					this.wmi = Win32_Process.Processes.FirstOrDefault(x => x.ProcessId == this.Id);
					this.gotWmi = true;
				}
				return this.wmi;
			}
		}

		public Process Parent {
			get {
				uint id = this.Wmi.ParentProcessId;
				if (id <= 0) {
					return null;
				}
				return new Process((int)id);
			}
		}
	}
}
