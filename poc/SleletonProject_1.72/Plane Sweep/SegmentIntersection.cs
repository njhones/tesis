using System;
using Structures;

namespace BorradorTesis.Plane_Sweep
{
	/// <summary>
	/// Descripci�n breve de SegmentIntersection.
	/// </summary>
	public class SegmentIntersection
	{
		/**/
		public readonly BigNumbers.BigRational distance;
		/**/
		public readonly Segment Segment;
		/**/
		public readonly RPoint it; 
		/**/
		public SegmentIntersection(BigNumbers.BigRational dist, Segment seg, RPoint it)
		{
			this.distance = dist;
			this.Segment = seg;
			this.it = it;
		}
		
	}
}
