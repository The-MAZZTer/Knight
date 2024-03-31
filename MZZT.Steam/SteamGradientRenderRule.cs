using System.Drawing;

namespace MZZT.Steam {
	[SteamName("gradient")]
	public class SteamGradientRenderRule(string left, string top, string right, string bottom,
		SteamSubstitutionValue<Color> startColor, SteamSubstitutionValue<Color> endColor) : SteamRenderRule(left, top, right, bottom) {
		public SteamSubstitutionValue<Color> StartColor { get; set; } = startColor;

		public SteamSubstitutionValue<Color> EndColor { get; set; } = endColor;
	}
}