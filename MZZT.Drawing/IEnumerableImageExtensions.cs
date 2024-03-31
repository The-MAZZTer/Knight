using System.Collections.Generic;
using System.Drawing;
using static System.Math;

namespace MZZT.Extensions {
	public static class IEnumerableImageExtensions {
		public static Image GetClosestSize(this IEnumerable<Image> images, Size targetSize) {
			double bestMatchDistance = double.MaxValue;
			Image bestMatch = null;
			foreach (Image image in images) {
				lock (image) {
					if (image.Width < targetSize.Width || image.Height < targetSize.Height) {
						continue;
					}

					double distance = image.Width - targetSize.Width +
						(image.Height - targetSize.Height) +
						((double)image.Width / image.Height -
						(double)targetSize.Width / targetSize.Height);
					if (bestMatch == null || distance < bestMatchDistance) {
						bestMatch = image;
						bestMatchDistance = distance;
					}
				}
			}
			if (bestMatch == null) {
				foreach (Image image in images) {
					lock (image) {
						if (image.Width >= targetSize.Width && image.Height >= targetSize.Height) {
							continue;
						}

						double distance = Abs(image.Width - targetSize.Width) +
							Abs(image.Height - targetSize.Height) +
							Abs((double)image.Width / image.Height -
							(double)targetSize.Width / targetSize.Height);
						if (bestMatch == null || distance < bestMatchDistance) {
							bestMatch = image;
							bestMatchDistance = distance;
						}
					}
				}
			}
			return bestMatch;
		}
	}
}
