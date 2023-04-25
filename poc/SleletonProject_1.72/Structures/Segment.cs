using System;
using BigNumbers;
using System.Drawing;

namespace Structures
{
	/// <summary>
	/// Descripción breve de Segment.
	/// </summary>
	public class Segment: ICloneable
	{
		/**/
		protected RPoint init, end, vdir;
		/**/
		protected bool empty;	
		/**/
		public Segment(RPoint init, RPoint end)
		{
			this.init = init;
			this.end = end;
			this.vdir = end - init;	
			this.empty = false;
		}
		/**/
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		/**/
		public Segment(RPoint pt, BigRational a, BigRational b)
		{
			this.vdir = new RPoint(a, b);
			this.init = pt;
            this.end = this.init + this.vdir;
			this.empty = false;
		}

		/**/
		public bool IntersectLine(Segment seg)
		{
			return !this.Paralelo(seg) || this.Coincide(seg);
		}
		/**/
		public BigRational Pendent
		{
			get
			{
				return this.Vertical ? null : this.vdir.Y / this.vdir.X; 
			}
		}
		/**/
		public override bool Equals(object obj)
		{
			Segment seg = (Segment)obj;
			return this.init == seg.init && this.end == seg.end;
		}

		/**/
		public TurnType Turn(RPoint pt)
		{
			return RPoint.SD(this.init, this.end, pt);			
		}

		/**/
		public void Paint(Graphics g, Pen p, int height)
		{
			g.DrawLine(p, this.init.XToLong, height - this.init.YToLong,
				this.end.XToLong, height - this.end.YToLong);
		}
		/**/
		public bool Vertical
		{
			get 
			{
				return this.init.X == this.end.X; 
			}
		}
        /**/
        public RPoint Intersection(Segment s)
        {
            if (this.Paralelo(s))
            {
                if (this.Coincide(s))
                {
                    return this.end;
                }
                throw new Exception("los segmentos son paralelos");
            }
            BigRational _y;
            BigRational _x = (this.vdir.X * s.vdir.X * (s.end.Y - this.end.Y) -
                this.vdir.X * s.vdir.Y * s.end.X + s.vdir.X * this.vdir.Y *
                this.end.X) / s.vdir.Determinante(vdir);
            if (s.vdir.X == 0)
            {
                _y = this.end.Y + (_x - this.end.X) * this.vdir.Y / this.vdir.X;
            }
            else
                _y = s.end.Y + (_x - s.end.X) * s.vdir.Y / s.vdir.X;
            return new RPoint(_x, _y);
        }

        /**/
        public RPoint Intersection2(Segment s)
        {
            if (this.Paralelo(s))
                return null;

            BigRational _y;
            BigRational _x = (this.vdir.X * s.vdir.X * (s.end.Y - this.end.Y) -
                this.vdir.X * s.vdir.Y * s.end.X + s.vdir.X * this.vdir.Y *
                this.end.X) / s.vdir.Determinante(vdir);
            if (s.vdir.X == 0)
            {
                _y = this.end.Y + (_x - this.end.X) * this.vdir.Y / this.vdir.X;
            }
            else
                _y = s.end.Y + (_x - s.end.X) * s.vdir.Y / s.vdir.X;

            return new RPoint(_x, _y);
        }
				
		/**/
		public virtual int ComparePendent(Segment s)
		{
			int result = 0;
			bool v1 = this.Vertical;
			bool v2 = s.Vertical;

			if(v1 && v2)
			{
				return result;
			}
			
			BigRational m1 = this.Pendent;
			BigRational m2 = s.Pendent;

			short sgm1 = 1, sgm2 = 1;

			if(!v1)
			{
				if(m1 == 0) sgm1 = 0;
				else if(!m1.sign) sgm1 = -1;
			}
			if(!v2)
			{
				if(m2 == 0) sgm2 = 0;
				else if(!m2.sign) sgm2 = -1;
			}
			if(v1 || v2)
			{
				if((v1 && sgm2 <= 0) || (v2 && sgm1 > 0))
				{
					result = -1;
				}
				else result = 1;
			}
			else
			{
				if(m1 == m2)
				{
					return result;
				}
				if(sgm1 != 0 && ((sgm1 != sgm2 && sgm2 != 1) || (sgm1 == sgm2 && m1 < m2)))
				{
					result = -1;
				}
				else 
				{
					result = 1;
				}
			}
			return result;
		}
		/**/
		public bool Paralelo(Segment s)
		{	
			return this.vdir.Determinante(s.vdir) == 0;			
		}
		
		/**/
		public bool Intersect(Segment s)
		{
			return RPoint.SD(init, end, s.init) != RPoint.SD(init, end, s.end) &&
				RPoint.SD(s.init, s.end, init) != RPoint.SD(s.init, s.end, end);
		}
		/**/
		public bool Coincide(Segment s)
		{
			return this.Paralelo(s) && this.vdir.Determinante
				(new RPoint(s.end.X - this.end.X, s.end.Y - this.end.Y)) == 0 ;
		}
		/**/
		public bool Contrario(Segment s)
		{
			return this.VectorDirector.X * s.VectorDirector.X < 0 ||
				this.VectorDirector.Y * s.VectorDirector.Y < 0;			
		}

		/**/
		public bool Contain(RPoint pt)
		{
			return RPoint.SD(this.init, this.end, pt) == TurnType.colinear;
		}

		/**/
		public RPoint Inicio
		{
			get 
			{
				return this.init;
			}
			set
			{
				this.init = value;
				this.vdir = end - init;	
			}
		}
		/**/
		public RPoint Fin
		{
			get 
			{
				return this.end;
			}
			set
			{
				this.end = value;
				this.vdir = end - init;
			}
		}
		
		/**/
		public RPoint VectorDirector
		{
			get
			{				
				return this.vdir;
			}
			set
			{
				this.vdir = value;
			}
		}		
			
		/**/
		public bool Empty
		{
			get
			{
				return this.empty;
			}
			set
			{
				this.empty = value;
			}
		}
		
		/**/
		public bool Horizontal
		{
			get
			{
				return this.init.Y == this.end.Y;
			}
		}

		/**/
		public bool Coincident(Segment s)
		{
			return (RPoint.SD( init, end, s.init) == 0  &&
				RPoint.SD(this.init, this.end, s.end) == 0 );
		}

		/**/
		public Segment ParallelSegment(short dist)
		{
			return null;
		}
		/**/
		public bool Puntual
		{
			get
			{
				return this.init.Equals(this.end);
			}
		}

		/**/
		public Segment Bisector(Segment seg)
		{
			RPoint pt = this.Intersection(seg);

			RPoint _ad1 = this.init;
			RPoint _ad2 = seg.end;
			RPoint _pt = this.Intersection(seg);
			if(_ad1.Equals(_pt))
			{
				_ad1 = this.end;
			}
			if(_ad2.Equals(_pt))
			{
				_ad2 = seg.init;
			}
 
			_ad1 = _ad1 - _pt;
			_ad2 = _ad2 - _pt;

			long _a1 = -(_ad1.XToLong);
			long _b1 = -(_ad1.YToLong);
			long _a2 = _ad2.XToLong;
			long _b2 = _ad2.YToLong;

			long _Den = (_a2 * _b1 - _b2 * _a1); 

			long _D = ((_a1 * _a1) + (_b1 * _b1));
			double _rD = Math.Abs(1) * Math.Sqrt(_D);
			long _ent = (long)Math.Floor(_rD);
			_rD -= _ent;

			long _num = (long)Math.Ceiling(_rD);
 
			BigRational d1 = new BigRational(Math.Sign(1) * _ent, 
				Math.Sign(1) * _num, 100);

			_D = ((_a2 * _a2) + (_b2 * _b2));
			_rD = Math.Abs(1) * Math.Sqrt(_D);
			_ent = (long)Math.Floor(_rD);
			_rD -= _ent;

			_num = (long)Math.Ceiling(_rD);

			BigRational d2 = new BigRational(Math.Sign(1) * _ent, 
				Math.Sign(1) * _num, 100);

			BigNumbers.BigRational _x = ( d2*_a1 - d1*_a2 ) / _Den;
			_x += _pt.X;

			BigNumbers.BigRational _y = ( d2*_b1 - d1*_b2 ) / _Den;
			_y += _pt.Y;

			return new Segment(_pt, new RPoint(_x, _y)); 
			
		}

		/**/
		public RPoint SegmentIntersectLine(Segment seg)
		{
			TurnType t_ini = this.Turn(seg.init);
			TurnType t_end = this.Turn(seg.end);
			if(t_end == TurnType.colinear)
				return seg.end;
			if(t_ini == TurnType.colinear)
				return seg.end;
			return (t_ini != t_end)? this.Intersection2(seg): null ;
//			return (t_ini == TurnType.colinear || t_end == TurnType.colinear || t_ini != t_end);
//				return true;			
//			return (t_ini != t_end);
//			return (this.Turn(seg.init) == TurnType.left && this.Turn(seg.end) == TurnType.right) ||
//			   (this.Turn(seg.init) == TurnType.right && this.Turn(seg.end) == TurnType.left);
		}
		/**/
		public bool Adverse(Segment s)
		{
			return (this.VectorDirector.X * s.VectorDirector.X < 0 &&
				this.VectorDirector.Y * s.VectorDirector.Y < 0) ;

//			return (this.VectorDirector.X * s.VectorDirector.X < 0 &&
//				this.VectorDirector.Y * s.VectorDirector.Y < 0) ||
//				((this.VectorDirector.X * s.VectorDirector.X > 0 &&
//				this.VectorDirector.Y * s.VectorDirector.Y < 0) || 
//				(this.VectorDirector.X * s.VectorDirector.X < 0 &&
//				this.VectorDirector.Y * s.VectorDirector.Y > 0) );			
		}
		/**/
		public RPoint IntersectionIfThen(Segment seg)
		{
			RPoint Result = new RPoint();
			

			if( ( this.Vertical ) || ( seg.Vertical ) )
			{
				Segment Aux;

				if( this.Vertical )
				{
					Aux = seg;
					Result.X = this.end.X;
				}
				else
				{
					Aux = this;
					Result.X = seg.end.X;
				}

				Result.Y = Aux.Pendent*( Result.X - Aux.end.X ) + Aux.end.Y;
			}
			else
			{
				BigRational M1 = this.Pendent;
				BigRational M2 = seg.Pendent;
				Segment Aux;

				if( (M1 == 0) || (M2 == 0) )
				{
					if(M1 == 0)
					{
						Aux = seg;
						Result.Y = this.end.Y;
					}
					else
					{
						Aux = this;
						Result.Y = seg.end.Y;
					}
					Result.X = ( Result.Y - Aux.end.Y )/Aux.Pendent + Aux.end.X;
				}
				else
				{
					RPoint P1 = this.end;
					RPoint P2 = seg.end;

					Result.X = ( M1 * P1.X - P1.Y - M2 * P2.X + P2.Y ) / ( M1 - M2 );
					Result.Y = M1*( Result.X - P1.X ) + P1.Y;
				}
			}
			
			return Result;
		}

		#region Miembros de ICloneable

		public object Clone()
		{
			return new Segment((RPoint)this.init.Clone(), (RPoint)this.end.Clone());			
		}

		#endregion

		
	}
}
