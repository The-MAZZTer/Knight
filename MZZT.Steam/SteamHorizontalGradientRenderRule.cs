using System.Drawing;

namespace MZZT.Steam {
	[SteamName("gradient_horizontal")]
	public class SteamHorizontalGradientRenderRule(string left, string top, string right, string bottom,
		SteamSubstitutionValue<Color> startColor, SteamSubstitutionValue<Color> endColor) : SteamGradientRenderRule(left, top, right, bottom, startColor, endColor) {
	}
}