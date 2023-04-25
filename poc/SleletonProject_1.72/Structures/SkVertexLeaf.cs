using System;
using System.Collections;

namespace Structures
{
	/// <summary>
	/// Descripción breve de SkVertexLeaf.
	/// </summary>
	public class SkVertexLeaf: SkNode
	{		
		public bool Painted;
		uint parallelindex;
		NewIntersectionPoint it;

		public SkVertexLeaf(RPoint point, Segment leftSegment, Segment rightSegment, Segment bisectorSegment,
			uint parallelindex): base(point, leftSegment, rightSegment, bisectorSegment)
		{
			this.parallelindex = parallelindex;
		}
		public NewIntersectionPoint IntersecPoint
		{
			get{return this.it;}
			set{this.it = value;}
		}
		public static void ComputeIt(SkVertexLeaf va, SkVertexLeaf vb, ArrayList intersect)
		{
			RPoint ip = va.Bisector.Intersection2(vb.Bisector);
			if (ip != null)
			{
				Segment s1 = new Segment(va.Point, ip);
				Segment s2 = new Segment(vb.Point, ip);

				if (!s1.Contrario(va.Bisector) && !s2.Contrario(vb.Bisector))
				{
					NewIntersectionPoint _ip = new NewIntersectionPoint(
						ip.Distance(va.Left),
						ip,
						va,
						vb,
						IntersecPointType.NewRight
						);
					
					va.it = _ip;
					intersect.Add(new PageHolder(_ip, va));
				}
			}
		}
	}	
}
