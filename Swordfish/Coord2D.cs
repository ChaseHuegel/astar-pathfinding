using System;

namespace Swordfish
{
	//	A 2 dimensional coordinate
	[Serializable]
	public class Coord2D
	{
		public int x = 0;
		public int y = 0;

		public Coord2D(int _x, int _y)
		{
			x = _x;
			y = _y;
		}

		public static Coord2D fromVector3(UnityEngine.Vector3 _vector)
		{
			return new Coord2D((int)_vector.x, (int)_vector.z);
		}

		//	Operators
		public static bool operator!= (Coord2D a, Coord2D b)
		{
			return (a.x != b.x || a.y != b.y);
		}

		public static bool operator== (Coord2D a, Coord2D b)
		{
			return (a.x == b.x && a.y == b.y);
		}

		public static Coord2D operator+ (Coord2D a, Coord2D b)
		{
			return new Coord2D(a.x + b.x, a.y + b.y);
		}

		public static Coord2D operator- (Coord2D a, Coord2D b)
		{
			return new Coord2D(a.x - b.x, a.y - b.y);
		}

		public static Coord2D operator* (Coord2D a, Coord2D b)
		{
			return new Coord2D(a.x * b.x, a.y * b.y);
		}

		public static Coord2D operator/ (Coord2D a, Coord2D b)
		{
			return new Coord2D(a.x / b.x, a.y / b.y);
		}

		public UnityEngine.Vector2 toVector2()
		{
			return new UnityEngine.Vector2(x, y);
		}

		public static Coord2D fromVector2(UnityEngine.Vector2 _vector)
		{
			return new Coord2D((int)_vector.x, (int)_vector.y);
		}

		public static UnityEngine.Vector2 toVector2(Coord2D _coord)
		{
			return new UnityEngine.Vector2(_coord.x, _coord.y);
		}

	}
}