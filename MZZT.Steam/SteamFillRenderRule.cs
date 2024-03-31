using System.Drawing;

namespace MZZT.Steam {
	[SteamName("fill")]
	public class SteamFillRenderRule(string left, string top, string right, string bottom,
		SteamSubstitutionValue<Color> color) : SteamRenderRule(left, top, right, bottom) {
		public SteamSubstitutionValue<Color> Color { get; set; } = color;
	}
}