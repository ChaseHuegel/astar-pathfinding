using System;

namespace Swordfish
{
    [Serializable]
	public class RangeI
	{
		public int from;
		public int to;

		public RangeI(int from, int to) { this.from = from; this.to = to; }

        public int RandomValue() { return (int)UnityEngine.Random.Range( (float)from, (float)to ); }
	}

    [Serializable]
	public class RangeF
	{
		public float from;
		public float to;

		public RangeF(float from, float to) { this.from = from; this.to = to; }

        public float RandomValue() { return UnityEngine.Random.Range( from, to ); }
	}
}