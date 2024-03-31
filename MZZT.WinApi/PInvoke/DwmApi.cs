using System;
using System.Runtime.InteropServices;
using static MZZT.WinApi.PInvoke.UxTheme;
using static MZZT.WinApi.PInvoke.Windef;

namespace MZZT.WinApi.PInvoke {
	public static class DwmApi {
		[StructLayout(LayoutKind.Sequential)]
		public struct DWM_COLORIZATION_PARAMS {
			public uint clrColor;
			public uint clrAfterGlow;
			public uint nIntensity;
			public uint clrAfterGlowBalance;
			public uint clrBlurBalance;
			public uint clrGlassReflectionIntensity;
			public bool fOpaque;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DWM_THUMBNAIL_PROPERTIES {
			public DWM_TNP dwFlags;
			public RECT rcDestination;
			public RECT rcSource;
			public byte opacity;
			public bool fVisible;
			public bool fSourceClientAreaOnly;
		}

		[Flags]
		public enum DWM_TNP {
			RECTDESTINATION = 0x1,
			RECTSOURCE = 0x2,
			OPACITY = 0x4,
			VISIBLE = 0x8,
			SOURCECLIENTAREAONLY = 0x10
		}

		[DllImport("dwmapi.dll", EntryPoint = "DwmExtendFrameIntoClientArea",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

		[DllImport("dwmapi.dll", EntryPoint = "DwmpGetColorizationParameters", PreserveSig = false,
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern void DwmpGetColorizationParameters(out DWM_COLORIZATION_PARAMS parameters);

		[DllImport("dwmapi.dll", EntryPoint = "DwmQueryThumbnailSourceSize",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DwmQueryThumbnailSourceSize(IntPtr hThumbnail, out SIZE pSize);

		[DllImport("dwmapi.dll", EntryPoint = "DwmRegisterThumbnail",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DwmRegisterThumbnail(IntPtr hwndDestination, IntPtr hwndSource,
			out IntPtr phThumbnailId);

		[DllImport("dwmapi.dll", EntryPoint = "DwmUnregisterThumbnail",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DwmUnregisterThumbnail(IntPtr hThumbnailId);

		[DllImport("dwmapi.dll", EntryPoint = "DwmUpdateThumbnailProperties",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DwmUpdateThumbnailProperties(IntPtr hThumbnailId,
			ref DWM_THUMBNAIL_PROPERTIES ptnProperties);
	}
}
