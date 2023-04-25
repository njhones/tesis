using System;
using System.Collections;

namespace Structures
{
	/// <summary>
	/// Descripción breve de PageHolder.
	/// </summary>
	public class PageHolder
	{
		NewIntersectionPoint it;
		SkNode sknode;

		public PageHolder(NewIntersectionPoint it, SkNode sknode)
		{
			this.sknode = sknode;
			this.it = it;
		}
		public void DiscardIntersecPoints()
		{
			NewIntersectionPoint prev = (this.sknode.previous as SkVertexLeaf).IntersecPoint;
			if(prev != null && !prev.Discarted)
			{
				if(prev.Point.QuadraticDistance(this.sknode.Point) < this.it.Point.QuadraticDistance(this.sknode.Point))
					this.it.Discarted = true;
				else
					prev.Discarted = true;
			}
		}
		public NewIntersectionPoint Intersection
		{
			get
			{
				return this.it;
			}
		}
	}
}
