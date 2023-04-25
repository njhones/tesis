using System;

namespace Structures
{
	/// <summary>
	/// Descripción breve de IntersectionPoint.
	/// </summary>
	public class IntersectionPoint: RPoint, IComparable
	{
		/**/
		private SkeletonVertex va, vb;
		/**/
		private bool spliteven;
		/**/
		private RPoint itersec;
		/**/
		private BigNumbers.BigRational dist;
		/**/
		public IntersectionPoint(){}
		/**/
		public IntersectionPoint(RPoint pt, SkeletonVertex va, 
			SkeletonVertex vb, bool split, BigNumbers.BigRational dist): base(pt.X, pt.Y)
		{
			this.itersec = pt;
			this.va = va;
			this.vb = vb;
			this.spliteven = split;
			this.dist = dist;
		}
		/**/
		public static IntersectionPoint Nearest(IntersectionPoint it1, 
			IntersectionPoint it2)
		{
			if(it1 == null)
			{
				return it2;
			}
			if(it2 == null)
			{
				return it1;
			}
			return (it1.dist <= it2.dist)? it1: it2;
		}
		/**/
		public bool SplitEvent
		{
			get
			{
				return this.spliteven;
			}

		}
		
		/**/
		public SkeletonVertex Va
		{
			get
			{
				return this.va;
			}
		}
		/**/
		public  SkeletonVertex Vb
		{
			get
			{
				return this.vb;
			}
		}

		/**/
		public RPoint ItPoint
		{
			get
			{
				return this.itersec;
			}
		}
		#region Miembros de IComparable

		public int CompareTo(object obj)
		{
			IntersectionPoint it = (IntersectionPoint)obj;
			if(this is InfiniteIntersection)
			{
				return 1;
			}
			if(it is InfiniteIntersection)
			{
				return -1;
			}
			if(this.dist == it.dist)
			{
				return 0;
			}
			return this.dist < it.dist ? -1 : 1; 
		}

		#endregion
	}
}
