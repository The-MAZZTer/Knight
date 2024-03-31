using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using static MZZT.WinApi.PInvoke.UxTheme;

namespace MZZT.Drawing {
	public class AccentColorSet {
		public static AccentColorSet[] AllSets {
			get {
				if (allSets == null) {
					allSets = new AccentColorSet[GetImmersiveColorSetCount()];
					for (uint i = 0; i < allSets.Length; i++) {
						allSets[i] = new AccentColorSet(i, false);
					}
				}

				return allSets;
			}
		}

		public static AccentColorSet ActiveSet {
			get {
				uint activeSet = GetImmersiveUserColorSetPreference(false, false);
				ActiveSet = AllSets[Math.Min(activeSet, AllSets.Length - 1)];
				return AccentColorSet.activeSet;
			}
			private set {
				if (activeSet != null) activeSet.Active = false;

				value.Active = true;
				activeSet = value;
			}
		}

		public bool Active { get; private set; }

		public Color this[string colorName] {
			get {
				uint colorType;

				IntPtr name = Marshal.StringToHGlobalAuto($"Immersive{colorName}");
				try {
					colorType = GetImmersiveColorTypeFromName(name);
					if (colorType == 0xFFFFFFFF) throw new InvalidOperationException();
				} finally {
					Marshal.FreeHGlobal(name);
				}

				return this[colorType];
			}
		}

		private Color this[uint colorType] {
			get {
				uint nativeColor = GetImmersiveColorFromColorSetEx(this.colorSet, colorType, false, 0);
				//if (nativeColor == 0)
				//	throw new InvalidOperationException();
				return Color.FromArgb(
					(byte)((0xFF000000 & nativeColor) >> 24),
					(byte)((0x000000FF & nativeColor) >> 0),
					(byte)((0x0000FF00 & nativeColor) >> 8),
					(byte)((0x00FF0000 & nativeColor) >> 16)
					);
			}
		}

		private AccentColorSet(uint colorSet, bool active) {
			this.colorSet = colorSet;
			this.Active = active;
		}

		private static AccentColorSet[] allSets;
		private static AccentColorSet activeSet;

		private readonly uint colorSet;

		// HACK: GetAllColorNames collects the available color names by brute forcing the OS function.
		//   Since there is currently no known way to retrieve all possible color names,
		//   the method below just tries all indices from 0 to 0xFFF ignoring errors.
		public IEnumerable<string> GetAllColorNames() {
			for (uint i = 0; i < 0xFFF; i++) {
				IntPtr typeNamePtr = GetImmersiveColorNamedTypeByIndex(i);
				if (typeNamePtr != IntPtr.Zero) {
					IntPtr typeName = (IntPtr)Marshal.PtrToStructure(typeNamePtr, typeof(IntPtr));
					yield return Marshal.PtrToStringAuto(typeName);
				}
			}
		}
	}
}
