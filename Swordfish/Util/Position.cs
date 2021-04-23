using System;

namespace Swordfish
{
	//	A position in 3D space
	public class Position
	{
		public float x = 0;
		public float y = 0;
		public float z = 0;

		public Position(float _x, float _y, float _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}

		public Position(int _x, int _y, int _z)
		{
			x = (float)_x;
			y = (float)_y;
			z = (float)_z;
		}

		public Position(double _x, double _y, double _z)
		{
			x = (float)_x;
			y = (float)_y;
			z = (float)_z;
		}

		public UnityEngine.Vector3 toVector3()
		{
			return new UnityEngine.Vector3(x, y, z);
		}

		public static Position fromVector3(UnityEngine.Vector3 _vector3)
		{
			return new Position(_vector3.x, _vector3.y, _vector3.z);
		}
	}
}