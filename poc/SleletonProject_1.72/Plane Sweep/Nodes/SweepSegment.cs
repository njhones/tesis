using System;
using BigNumbers;
using Structures;
using BorradorTesis.Plane_Sweep.GrowthStructures;

namespace BorradorTesis.Plane_Sweep.Nodes
{
	/// <summary>
	/// Descripción breve de SweepSegment.
	/// </summary>
	public class SweepSegment: Segment, IComparable
	{
		/**/
		public static bool afterline = true;		
		/**/
		private RPoint upper, lower;
		/**/
		public static RPoint key; 	
		/**/
		public Interval left, right;
		/**/
		public SweepSegment(Segment seg): base(seg.Inicio, seg.Fin)
		{			
			if(this.init.Y > this.end.Y || 
				(this.init.Y == this.end.Y && this.init.X < this.end.X))
			{
				this.upper = this.init;
				this.lower = this.end;
			}
			else
			{
				this.upper = this.end;
				this.lower = this.init;
			}
		}
		/**/
		public SweepSegment(RPoint init, RPoint end): base(init, end)
		{
			if(this.init.Y > this.end.Y || 
				(this.init.Y == this.end.Y && this.init.X < this.end.X))
			{
				this.upper = this.init;
				this.lower = this.end;
			}
			else
			{
				this.upper = this.end;
				this.lower = this.init;
			}
		}
		/**/
		public override int ComparePendent(Segment ss)
		{			
			int result = base.ComparePendent(ss);
			return afterline ? result : -result;
		}
		/**/
		public RPoint SweepInterseption
		{
			get
			{
				RPoint result = (RPoint)key.Clone();				

				if(this.Horizontal)
				{
					if(this.init.Y != key.Y)
					{						
						result = null;
					}					
				}			
				else if(this.Vertical)
				{
					result.X = this.init.X;
					result.Y = key.Y;
				}
				else
				{
					result.X = (this.vdir.X / this.vdir.Y) * (key.Y - this.init.Y) + this.init.X;
					result.Y = key.Y;
				}
				return result;
			}
		}
					
		/**/
		public RPoint UpperEnd
		{
			get
			{
				return this.upper;
			}
			set
			{				
				this.upper = value;
			}
		}
		/**/
		public RPoint LowerEnd
		{
			get
			{
				return this.lower;
			}
		}

		/**/
		public bool isReversal()
		{
			return false;
		}
		
		public Interval GetRight()
		{
			return right;
		}
		public Interval GetLeft()
		{
			return left;
		}
		public void SetLeft(Interval L)
		{
			left = L;
		}
		public void SetRight(Interval R)
		{
			right = R;
		}

		#region Miembros de IComparable

		public int CompareTo(object obj)
		{
			SweepSegment ss = (SweepSegment)obj;

			if(this == ss)
			{
				return 0;
			}

			RPoint it1 = this.SweepInterseption;
			RPoint it2 = ss.SweepInterseption;

			if(it1 == null || it2 == null)
			{
				throw new Exception("Esta comparando con un segmento que no puede estar en el arbol de estado");
			}
			if(it1.X == it2.X)
			{
				int result = this.ComparePendent(ss);
				return result == 0 ? this.end.CompareToY(ss.end): result;
			}			
			return it1.X < it2.X ? -1 : 1;
		}


		#endregion
	}
}
