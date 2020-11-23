using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace DrawingGenerator
{
	public struct Point
	{
		public int x, y;

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(Object obj)
		{
			if(obj == null || !(obj is Point))
			{
				return false;
			}
			else
			{
				var rhs = (Point)obj;

				return x == rhs.x && y == rhs.y;
			}
		}
		public override int GetHashCode()
		{
			return x + 2789 * y;
		}
	}

	public class ImageLoader
	{
		string path;
		float threshold;
		HashSet<Point> pointsAdded;
		List<List<Point>> points;
		public List<Point> outPoints { get; private set; }
		Bitmap img;

		public ImageLoader(string path, float threshold)
		{
			this.path = path;
			this.threshold = threshold;
			pointsAdded = new HashSet<Point>();
			points = new List<List<Point>>();
		}

		bool AboveThreshold(Color color)
		{
			float brightness = color.GetBrightness();
			return brightness < threshold;
		}

		void FindConnectedComponentRecursive(int x, int y, List<Point> pointList)
		{
			Point p = new Point(x, y);
			if (x >= img.Width || x < 0 || y < 0 || y >= img.Height || pointsAdded.Contains(p))
				return;

			var pixel = img.GetPixel(x, y);

			if(AboveThreshold(pixel))
			{
				pointList.Add(p);
				pointsAdded.Add(p);

				for(int i=-1; i <= 1; ++i)
				{
					for(int u=-1; u <= 1; ++u)
					{
						if (i == 0 && u == 0)
							continue;
						FindConnectedComponentRecursive(x + i, y + u, pointList);
					}
				}
			}
		}

		public void SortPoints()
		{
			outPoints = new List<Point>();
			foreach(var plist in points.OrderBy(a => -a.Count))
			{
				foreach(var point in plist)
				{
					outPoints.Add(point);
				}
			}
		}

		public void LoadBinaryImageConnectedComponents()
		{
			using (img = new Bitmap(path))
			{
				for(int x=0; x < img.Width; ++x)
				{
					for(int y=0; y < img.Height; ++y)
					{
						Point p = new Point(x, y);
						if (pointsAdded.Contains(p))
							continue;

						var pixel = img.GetPixel(x, y);

						if(AboveThreshold(pixel))
						{
							var list = new List<Point>();
							FindConnectedComponentRecursive(x, y, list);
							points.Add(list);
						}	
					}
				}
			}
		}
	}
}
