using System;
using System.Collections.Generic;

namespace DrawingGenerator
{
	public class PointConverter
	{
		Vector3d start;
		Vector3d right;
		Vector3d down;
		Vector3d playerPos;

		public PointConverter(Vector3d start, Vector3d right, Vector3d down, Vector3d playerPos)
		{
			this.start = start;
			this.right = right;
			this.down = down;
			this.right.Normalize();
			this.down.Normalize();
			this.playerPos = playerPos;
		}

		public List<Vector3d> PointsToAngles(List<Point> points, double width)
		{
			List<Vector3d> diffs = new List<Vector3d>();
			
			int max = points[0].x;
			int min = max;

			foreach (var point in points)
			{
				max = Math.Max(max, point.x);
				min = Math.Min(min, point.x);
			}

			double scale = width / (max - min);

			foreach (var point in points)
			{
				var endpoint = start + right * point.x * scale + down * point.y * scale;
				var difference = endpoint - playerPos;

				diffs.Add(difference);
			}

			List<Vector3d> angles = new List<Vector3d>();

			foreach(var diff in diffs)
			{
				angles.Add(diff.VectorAngles());
			}

			return angles;
		}
	}
}
