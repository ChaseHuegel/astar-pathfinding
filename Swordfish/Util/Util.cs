using System;
using System.IO;
using UnityEngine;

namespace Swordfish
{
	public class Util
	{
        public static int GetOverflow(int value, int min, int max)
        {
            if (value < min) return value - min;
            if (value > max) return value - max;

            return 0;
        }

        public static float GetOverflow(float value, float min, float max)
        {
            if (value < min) return value - min;
            if (value > max) return value - max;

            return 0.0f;
        }

        public static int WrapInt(int _value, int _rangeMin, int _rangeMax)
        {
            int range = _rangeMax - _rangeMin + 1;

            if (_value < _rangeMin)
            {
                _value += range * ((_rangeMin - _value) / range + 1);
            }

            return _rangeMin + (_value - _rangeMin) % range;
        }

        public static float RangeToRange(float _input, float _low, float _high, float _newLow, float _newHigh)
        {
            return ((_input - _low) / (_high - _low)) * (_newHigh - _newLow) + _newLow;
        }

        public static float Distance (Vector3 firstPosition, Vector3 secondPosition)
        {
            Vector3 heading;

            heading.x = firstPosition.x - secondPosition.x;
            heading.y = firstPosition.y - secondPosition.y;
            heading.z = firstPosition.z - secondPosition.z;

            float distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;
            return Mathf.Sqrt(distanceSquared);
        }

        public static float DistanceUnsquared(Coord2D firstPosition, Coord2D secondPosition)
        {
            return (firstPosition.x - secondPosition.x) * (firstPosition.x - secondPosition.x) +
                    (firstPosition.y - secondPosition.y) * (firstPosition.y - secondPosition.y);
        }

        public static float DistanceUnsquared(Vector3 firstPosition, Vector3 secondPosition)
        {
            return (firstPosition.x - secondPosition.x) * (firstPosition.x - secondPosition.x) +
                    (firstPosition.y - secondPosition.y) * (firstPosition.y - secondPosition.y) +
                    (firstPosition.z - secondPosition.z) * (firstPosition.z - secondPosition.z);
        }

        public static Vector3[] CreateEllipse(float a, float b, float h, float k, float theta, int resolution)
        {
            Vector3[] positions = new Vector3[resolution+1];
            Quaternion q = Quaternion.AngleAxis (theta, Vector3.up);
            Vector3 center = new Vector3(h,k,0.0f);

            for (int i = 0; i <= resolution; i++) {
                float angle = (float)i / (float)resolution * 2.0f * Mathf.PI;
                positions[i] = new Vector3(a * Mathf.Cos (angle), 0.0f, b * Mathf.Sin (angle));
                positions[i] = q * positions[i] + center;
            }

            return positions;
        }

        public static Direction DirectionFromVector3(Vector3 _vector)
		{
			if (_vector == new Vector3(0, 0, 1)) { return Direction.NORTH; }
			if (_vector == new Vector3(1, 0, 0)) { return Direction.EAST; }
			if (_vector == new Vector3(0, 0, -1)) { return Direction.SOUTH; }
			if (_vector == new Vector3(-1, 0, 0)) { return Direction.WEST; }
			if (_vector == new Vector3(0, 1, 0)) { return Direction.ABOVE; }
			if (_vector == new Vector3(0, -1, 0)) { return Direction.BELOW; }

            return Direction.NORTH;
		}

        public static float Grayscale(Color _color)
        {
            return (_color.r + _color.g + _color.b) / 3;
        }

        public static float Grayscale(Color32 _color)
        {
            return (_color.r + _color.g + _color.b) / 3;
        }

        public static int IndexFrom2D(int _x, int _y, int _width)
        {
            return _x + (_width * _y);
        }
    }
}