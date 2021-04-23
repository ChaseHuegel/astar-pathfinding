using System;
using UnityEngine;

namespace Swordfish
{
	public enum Direction	//	DO NOT CHANGE ORDER! Hard coded values are used elsewhere!
	{
		NORTH,
		EAST,
		SOUTH,
		WEST,
		ABOVE,
		BELOW
	}

	public static class DirectionExtensions
	{
		public static Direction getOpposite(this Direction _dir)
		{
			switch (_dir)
			{
				case Direction.NORTH:
					return Direction.SOUTH;

				case Direction.EAST:
					return Direction.WEST;

				case Direction.SOUTH:
					return Direction.NORTH;

				case Direction.WEST:
					return Direction.EAST;

				case Direction.ABOVE:
					return Direction.BELOW;

				case Direction.BELOW:
					return Direction.ABOVE;

				default:
					return Direction.NORTH;
			}
		}

		public static Vector3 toVector3(this Direction _dir)
		{
			switch (_dir)
			{
				case Direction.NORTH:
					return new Vector3(0, 0, 1);

				case Direction.EAST:
					return new Vector3(1, 0, 0);

				case Direction.SOUTH:
					return new Vector3(0, 0, -1);

				case Direction.WEST:
					return new Vector3(-1, 0, 0);

				case Direction.ABOVE:
					return new Vector3(0, 1, 0);

				case Direction.BELOW:
					return new Vector3(0, -1, 0);

				default:
					return new Vector3(0, 0, 1);
			}
		}
	}
}