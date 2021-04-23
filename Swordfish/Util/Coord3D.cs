using System;
using UnityEngine;

namespace Swordfish
{
	//	A 3 dimensional coordinate
	public class Coord3D
	{
		public int x = 0;
		public int y = 0;
		public int z = 0;

		public override bool Equals(System.Object obj)
		{
			Coord3D coord = obj as Coord3D;

			if (coord == null)
			{
				return false;
			}

			return (this.x.Equals(coord.x) && this.y.Equals(coord.y) && this.z.Equals(coord.z));
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
		}

		//	Operators
		public static Coord3D operator+ (Coord3D a, Coord3D b)
		{
			return new Coord3D(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Coord3D operator- (Coord3D a, Coord3D b)
		{
			return new Coord3D(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static Coord3D operator* (Coord3D a, Coord3D b)
		{
			return new Coord3D(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static Coord3D operator/ (Coord3D a, Coord3D b)
		{
			return new Coord3D(a.x / b.x, a.y / b.y, a.z / b.z);
		}

		public void assignData(int _x, int _y, int _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}

		public Coord3D(int _x, int _y, int _z)
		{
			assignData(_x, _y, _z);
		}

		public Coord3D(float _x, float _y, float _z)
		{
			assignData(Mathf.RoundToInt(_x), Mathf.RoundToInt(_y), Mathf.RoundToInt(_z));
		}

		public Coord3D(double _x, double _y, double _z)
		{
			assignData(Mathf.RoundToInt((float)_x), Mathf.RoundToInt((float)_y), Mathf.RoundToInt((float)_z));
		}

		public UnityEngine.Vector3 toVector3()
		{
			return new UnityEngine.Vector3(x, y, z);
		}

		public static Coord3D fromVector3(UnityEngine.Vector3 _vector)
		{
			return new Coord3D(_vector.x, _vector.y, _vector.z);
		}

		public static UnityEngine.Vector3 toVector3(Coord3D _coord)
		{
			return new UnityEngine.Vector3(_coord.x, _coord.y, _coord.z);
		}
	}
}