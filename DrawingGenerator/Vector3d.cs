using System;


namespace DrawingGenerator
{

	public class Vector3d
	{
		public double x, y, z;

		public Vector3d(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public void Normalize()
		{
			var norm = Math.Sqrt(x * x + y * y + z * z);
			x /= norm;
			y /= norm;
			z /= norm;
		}

		public static Vector3d FromString(string input)
		{
			var elements = input.Split(' ');
			float x, y, z;

			if(elements.Length != 3)
			{
				Console.WriteLine(string.Format("Input vector {0} doesnt have 3 elements!\n", input));
				return null;
			}

			if(!float.TryParse(elements[0], out x) || !float.TryParse(elements[1], out y) || !float.TryParse(elements[2], out z))
			{
				Console.WriteLine(string.Format("Was not able to parse vector {0}", input));
				return null;
			}

			return new Vector3d(x, y, z);
		}
		public void NormalizeAngles()
		{
			y = NormalizeDeg(y);
		}

		public double this[int i]
		{
			get { if (i == 0) return x; else if (i == 1) return y; else if (i == 2) return z; else throw new IndexOutOfRangeException(); }
			set { if (i == 0) x = value; else if (i == 1) y = value; else if (i == 2) z = value; else throw new IndexOutOfRangeException(); }
		}

		public static Vector3d operator+(Vector3d lhs, Vector3d rhs)
		{
			return new Vector3d(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		public static Vector3d operator -(Vector3d lhs, Vector3d rhs)
		{
			return new Vector3d(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

		public static Vector3d operator*(Vector3d lhs, double multiplier)
		{
			return new Vector3d(lhs.x * multiplier, lhs.y * multiplier, lhs.z * multiplier);
		}
		static double NormalizeDeg(double a)
		{
			a = Math.IEEERemainder(a, 360.0);
			if (a >= 180.0)
				a -= 360.0;
			else if (a < -180.0)
				a += 360.0;
			return a;
		}

		public Vector3d VectorAngles()
		{
			double tmp, yaw, pitch;

			if (this[1] == 0 && this[0] == 0)
			{
				yaw = 0;
				if (this[2] > 0)
					pitch = 270;
				else
					pitch = 90;
			}
			else
			{
				yaw = Math.Atan2(this[1], this[0]) * 180 / Math.PI;
				if (yaw < 0)
					yaw += 360;

				tmp = Math.Sqrt(this[0] * this[0] + this[1] * this[1]);
				pitch = (Math.Atan2(-this[2], tmp) * 180 / Math.PI);
			}

			return new Vector3d(NormalizeDeg(pitch), NormalizeDeg(yaw), 0);
		}
	}
}
