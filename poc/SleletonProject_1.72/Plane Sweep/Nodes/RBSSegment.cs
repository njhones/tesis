using System;

namespace BorradorTesis.Plane_Sweep.Nodes
{
	/// <summary>
	/// Descripción breve de RBSSegment.
	/// </summary>
	public class RBSSegment :SweepSegment
	{		
		/**/
		public readonly Color color;
		/**/
		public RBSSegment(RPoint inic, RPoint end, Color color): base(inic, end)
		{
			this.color = color;
		}
		/**/
		public RBSSegment(Segment seg, Color color): base(seg)
		{
			this.color = color;
		}	
		
	}
}
