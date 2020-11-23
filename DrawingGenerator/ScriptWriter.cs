using System.Collections.Generic;

namespace DrawingGenerator
{
	public class ScriptWriter
	{
		int shotDelay;
		int reloadDelay;
		int clipSize;
		int cone;

		public ScriptWriter(int shotDelay, int reloadDelay, int clipSize, int cone)
		{
			this.shotDelay = shotDelay;
			this.reloadDelay = reloadDelay;
			this.clipSize = clipSize;
			this.cone = cone;
		}

		public void WriteScript(string path, List<Vector3d> angles)
		{
			string aimFormat = "---------|------|----------|-|-|1|tas_aim {0} {1} {2} {3}";
			string waitFormat = "---------|------|----------|-|-|{0}|";
			string shoot = "---------|------|---1------|-|-|1|";

			using (var streamWriter = new System.IO.StreamWriter(path))
			{
				int currentClip = clipSize;

				foreach(var angle in angles)
				{
					streamWriter.WriteLine(string.Format(aimFormat, angle[0], angle[1], shotDelay + 1, cone));
					streamWriter.WriteLine(string.Format(waitFormat, shotDelay - 1));
					streamWriter.WriteLine(shoot);
					--currentClip;

					if(currentClip == 0)
					{
						streamWriter.WriteLine(string.Format(waitFormat, reloadDelay));
						currentClip = clipSize;
					}
				}
			}
		}
	}
}
