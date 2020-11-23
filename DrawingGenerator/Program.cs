using System;
using System.Collections.Generic;
using System.Linq;


namespace DrawingGenerator
{
	class Program
	{
		static string ReadPath(string prompt)
		{
			Console.Write(prompt);
			string path = Console.ReadLine();
			return path;
		}

		static Vector3d ReadVector(string prompt)
		{
			Console.Write(prompt);
			bool first = true;
			Vector3d outVec;

			do
			{
				if (!first)
				{
					Console.WriteLine("Not a valid vector.");
				}

				first = false;
				outVec = Vector3d.FromString(Console.ReadLine());
			} while (outVec == null);

			return outVec;
		}


		static double ReadDouble(string prompt)
		{
			Console.Write(prompt);
			bool first = true;
			double outDouble;

			do
			{
				if (!first)
				{
					Console.WriteLine("Not a valid double.");
				}

				first = false;
			} while (!double.TryParse(Console.ReadLine(), out outDouble));

			return outDouble;
		}

		struct Gun
		{
			public int ClipSize;
			public int ShotDelay;
			public int ReloadDelay;
			public int Cone;
		}

		static Dictionary<string, Gun> GUNS = new Dictionary<string, Gun>()
			{
				{ "AR2", new Gun { ClipSize = 30, ShotDelay = 6, ReloadDelay = 105, Cone = 3} },
				{ "SMG", new Gun { ClipSize = 45, ShotDelay = 5, ReloadDelay = 100, Cone = 5} }
			};

		static Gun ReadGun(string prompt)
		{
			Console.Write(prompt);
			bool first = true;
			string gun;

			do
			{
				if (!first)
				{
					Console.WriteLine("Not a valid gun. Guns are {0}", GUNS.Keys);
				}

				first = false;
				gun = Console.ReadLine();
				gun = gun.ToUpper();
			} while (!GUNS.ContainsKey(gun));

			return GUNS[gun];
		}


		static void Main(string[] args)
		{
			string imagePath, outputPath;
			double width;
			Gun gun;
			Vector3d target, player, right, down;
			ImageLoader loader;
			PointConverter converter;

			if (args.Count() >= 8)
			{
				loader = new ImageLoader(args[0], 0.5f);
				loader.LoadBinaryImageConnectedComponents();
				loader.SortPoints();

				outputPath = args[1];
				target = Vector3d.FromString(args[2]);
				player = Vector3d.FromString(args[3]);
				right = Vector3d.FromString(args[4]);
				down = Vector3d.FromString(args[5]);
				width = double.Parse(args[6]);
				gun = GUNS[args[7]];
			}
			else
			{
				imagePath = ReadPath("Image path: ");
				loader = new ImageLoader(imagePath, 0.5f);
				loader.LoadBinaryImageConnectedComponents();
				loader.SortPoints();

				outputPath = ReadPath("Output path: ");
				target = ReadVector("Image position (on map): ");
				player = ReadVector("Player position: ");
				width = ReadDouble("Image width in hammer units: ");
				right = ReadVector("Right vector (translates x coordinate on image to map coordinates): ");
				down = ReadVector("Down vector (translates y coordinate on image to map coordinates): ");
				gun = ReadGun("Weapon (AR2, SMG or Pistol): ");
			}

			converter = new PointConverter(target, right, down, player);
			var angles = converter.PointsToAngles(loader.outPoints, width);
			ScriptWriter writer = new ScriptWriter(gun.ShotDelay, gun.ReloadDelay, gun.ClipSize, gun.Cone);
			writer.WriteScript(outputPath, angles);
		}
	}
}
