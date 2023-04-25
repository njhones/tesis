using System;
using System.Collections;
using BigNumbers;
using Structures;

namespace BorradorTesis.Plane_Sweep.Nodes
{
	/// <summary>
	/// Descripción breve de Beak.
	/// </summary>
	public class Beak: SweepSegment
	{
		/**/
		private ArrayList oseg;	
		/**/
		private uint u;
		/**/
		public Beak(RPoint inic, RPoint end, uint u): base(inic, end)
		{
			this.u = u;
			
		}
		/**/
		public void AddIntersection(SweepSegment seg, RPoint pt)
		{
			BigNumbers.BigRational dist = pt.QuadraticDistance(this.init);
			if(this.oseg == null)
			{
				oseg = new ArrayList();
				oseg.Add(new SegmentIntersection(dist, seg, pt));
			}
			else
			{
				int u = 0;
				for(u = 0; u < this.oseg.Count && ((SegmentIntersection)oseg[u]).distance < dist; u++);
				this.oseg.Insert(u, new SegmentIntersection(dist, seg, pt));
			}
		}
		/**/
		public ArrayList Intersectios
		{
			get
			{
				return this.oseg;
			}
		}
		
		/**/
		public uint Peak
		{
			get
			{
				return this.u;
			}
		}
	}
}
