using System;
using System.Drawing;

namespace BorradorTesis
{
	/// <summary>
	/// Descripción breve de MathPoint.
	/// </summary>
	public class MathPoint
	{
		public static Giro SD(PointF p1, PointF p2, PointF p3)
		{
			float _area = MathPoint.Area2(p1, p2, p3);
			if(_area < 0)
				return Giro.izq;
			else if(_area > 0)
				return Giro.der;
			return Giro.coll;
		}
		private static float Area2(PointF a, PointF b, PointF c)
		{
			return (b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y);
		}
	}
}
