using System;
using System.Collections;
using BorradorTesis.Plane_Sweep.Nodes;
using BigNumbers;

namespace BorradorTesis.Plane_Sweep
{
	/// <summary>
	/// Descripción breve de BeakIntersections.
	/// </summary>
	public class BeakIntersections
	{
		/**/
		private SweepSegment beak;
		/**/
		private ArrayList oseg;
		/**/
		public BeakIntersections(SweepSegment beak)
		{
			this.beak = beak;
			this.oseg = new ArrayList(20);
		}
		/**/
		public void AddIntersection(SweepSegment seg, RPoint pt)
		{
			BigNumbers.BigRational dist = pt.QuadraticDistance(this.beak.LowerEnd);
			int u = 0;
			for(u = 0; u < this.oseg.Count && ((SegmentIntersection)oseg[u]).distance < dist; u++);
			this.oseg.Insert(u, new SegmentIntersection(dist, seg));
		}
		/*Internal class*/
		protected class SegmentIntersection
		{
			/**/
			public readonly BigNumbers.BigRational distance;
			/**/
			public readonly SweepSegment seg;
			/**/
			public SegmentIntersection(BigRational dist, SweepSegment seg)
			{
				this.distance = dist;
				this.seg = seg;
			}
		}
	}
}
