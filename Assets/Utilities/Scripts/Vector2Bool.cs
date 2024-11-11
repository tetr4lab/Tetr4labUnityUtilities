namespace Tetr4lab.UnityEngine {
    /// <summary>真偽値ペア</summary>
    public struct Vector2Bool {
        /// <summary></summary>
		public bool x;
        /// <summary></summary>
		public bool y;
        /// <summary></summary>
		public bool And => x & y;
        /// <summary></summary>
		public bool Or => x | y;
        /// <summary></summary>
		public bool Xor => x ^ y;
        /// <summary></summary>
        public static bool operator true (Vector2Bool a) { return a.x & a.y; }
        /// <summary></summary>
		public static bool operator false (Vector2Bool a) { return !a.x & !a.y; }
        /// <summary></summary>
		public static Vector2Bool operator & (Vector2Bool a, Vector2Bool b) { return new Vector2Bool (a.x & b.x, a.y & b.y); }
        /// <summary></summary>
		public static Vector2Bool operator | (Vector2Bool a, Vector2Bool b) { return new Vector2Bool (a.x | b.x, a.y | b.y); }
        /// <summary></summary>
		public static Vector2Bool operator ^ (Vector2Bool a, Vector2Bool b) { return new Vector2Bool (a.x ^ b.x, a.y ^ b.y); }
        /// <summary></summary>
        /// <summary></summary>
		public static Vector2Bool operator ! (Vector2Bool a) { return new Vector2Bool (!a.x, !a.y); }
        /// <summary></summary>
		public static bool operator == (Vector2Bool a, Vector2Bool b) { return a.x == b.x && a.y == b.y; }
        /// <summary></summary>
		public static bool operator != (Vector2Bool a, Vector2Bool b) { return a.x != b.x || a.y != b.y; }
        /// <summary></summary>
        public static readonly Vector2Bool True = new Vector2Bool (true, true);
        /// <summary></summary>
		public static readonly Vector2Bool False = new Vector2Bool (false, false);
        /// <summary></summary>
		public static readonly Vector2Bool XnotY = new Vector2Bool (true, false);
        /// <summary></summary>
		public static readonly Vector2Bool YnotX = new Vector2Bool (false, true);
        /// <summary></summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
		public Vector2Bool (bool x, bool y) {
			this.x = x;
			this.y = y;
		}
        /// <summary></summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public override bool Equals (object obj) {
			return (obj != null && obj is Vector2Bool && x == ((Vector2Bool) obj).x && y == ((Vector2Bool) obj).y);
		}
        /// <summary></summary>
        /// <returns></returns>
		public override int GetHashCode () {
			return (x ? 1 : 0) + (y ? 2 : 0);
		}
        /// <summary></summary>
        /// <returns></returns>
		public override string ToString () {
			return $"({x}, {y})";
		}
	}

}
