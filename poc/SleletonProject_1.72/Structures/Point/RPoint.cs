using System;
using System.Drawing;
using System.Windows.Forms;
using BigNumbers;

namespace Structures
{
	/// <summary>
	/// Descripción breve de Point.
	/// </summary>
	
	[Serializable]
	public class RPoint: ICloneable
	{
		/**/

		private BigRational x;
		/**/
		private BigRational y;	
		/**/
		public bool flag;
		/**/
		public static readonly BigRational epsilon = new BigRational(0, 1, 1000000);

		
		

		public RPoint(BigRational x, BigRational y)
		{
			this.x = x;
			this.y = y;			
		}
		/**/
		public RPoint(){} 
		/**/
		public RPoint(Point p)
		{
			this.x = new BigRational(p.X, 0, 1);
			this.y = new BigRational(p.Y, 0, 1);
			
		}
		/**/
		public BigRational X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}
		/**/
		public BigRational Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}
		/**/
		public long XToLong
		{
			get
			{
				return BigInteger.BigIntegerToLong(this.x.entero);
			}			
		}
		/**/
		public long YToLong
		{
			get
			{
				return BigInteger.BigIntegerToLong(this.y.entero);
			}			
		}
		/**/
		public Label InitialyzeLabel(int height)
		{
			
			Label _lab = new Label();

			_lab.BackColor = System.Drawing.Color.White;
			_lab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			_lab.Location = new Point((int)this.XToLong, height - (int)this.YToLong);				
			_lab.Size = new System.Drawing.Size(105, 20);				
			_lab.Text = _lab.Location.ToString();								
			_lab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			_lab.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			
			return _lab;
			
		}
		/**/
		public int CompareToY(RPoint pt)
		{
			if(this.y > pt.y)
			{
				return -1;
			}
			if(pt.Y > this.y)
			{
				return 1;
			}
			if(this.x < pt.x)
			{
				return -1;
			}
			if(this.x > pt.x)
			{
				return 1;
			}
			return 0;
		}
		/**/
		public override bool Equals(object obj)
		{
			RPoint pt = obj as RPoint;
			return pt.x == this.x && this.y == pt.y;
		}
		/**/
		public BigRational Determinante(RPoint p)
		{
			return (this.x * p.y) - (this.y * p.x);
		}
        /**/
        public static RPoint operator -(RPoint p1, RPoint p2)
        {
            return new RPoint(p1.x - p2.x, p1.y - p2.y);
        }
        /**/
        public static RPoint operator +(RPoint p1, RPoint p2)
        {
            return new RPoint(p1.x + p2.x, p1.y + p2.y);
        }
		/**/
		public static RPoint operator *(int x, RPoint pt)
		{
			return new RPoint(pt.x * x, pt.y * x);
		}
        /**/
		public BigRational QuadraticDistance(RPoint p)
		{
			return (this.x - p.x) * (this.x - p.x) + (this.y - p.y) * (this.y - p.y);
		}
		/**/
		public override string ToString()
		{
			return this.XToLong.ToString() + "." + this.YToLong.ToString();
		}
		/**/
		public static TurnType SD(RPoint p1, RPoint p2, RPoint p3)
		{
			BigRational _area = RPoint.Area2(p1, p2, p3);
			if(_area.ToLong < 0)
				return TurnType.right;
			else if(_area.ToLong > 0)
				return TurnType.left;
			return TurnType.colinear;
		}
		/**/
		private static BigRational Area2(RPoint a, RPoint b, RPoint c)
		{
			return (b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y);
		}
		
		/**/
		public BigRational Distance(Segment seg)
		{
			long A = -seg.VectorDirector.YToLong;
			long B = seg.VectorDirector.XToLong;
			RPoint pt = this - seg.Inicio;	
			double den = Math.Sqrt(A * A + B * B);
			long num = Math.Abs(A * pt.XToLong + B * pt.YToLong);
			BigRational _num = new BigRational(num, 0, 1); 			
			long ent = (long)Math.Floor(den);			
			BigRational _den = new BigRational(ent, (long)Math.Ceiling(den * 100), 100);
			BigRational _result = _num / _den;
			return _result;
		}

		/**/
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}


		#region Miembros de ICloneable

		public object Clone()
		{
			return new RPoint(new BigRational(this.x), new BigRational(this.y));			
		}

		#endregion

		public BigRational EuclidianDistance(RPoint pt)
		{
//			BigRational _br = this.QuadraticDistance(pt);
//			long _ent = BigInteger.BigIntegerToLong(_br.entero);
//			long _num = BigInteger.BigIntegerToLong(_br.numerador);
//			long _den = BigInteger.BigIntegerToLong(_br.denominador);
//			double _d = (_den * _ent + _num) / _den;
			double _rd = Math.Sqrt(BigInteger.BigIntegerToLong(this.QuadraticDistance(pt).entero));
			long ent = (long)Math.Floor(_rd);
			_rd = (_rd - ent) * 1000;
			long num = (long)Math.Ceiling(_rd);
            return new BigRational(ent, num, 1000);		
		}
	}
}
