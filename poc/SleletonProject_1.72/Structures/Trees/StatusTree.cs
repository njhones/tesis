using System;
using Structures;
using BorradorTesis.Plane_Sweep.Nodes;

namespace BorradorTesis.Plane_Sweep.Trees
{
	/// <summary>
	/// Descripción breve de StatusTree.
	/// </summary>
	public class StatusTree: RBTree
	{
		/// <summary>
		/// Descripción breve de StatusTree.
		/// </summary>
		public StatusTree()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}		
		/**/
		public RBNode FindPoint(RPoint pt)
		{
			RBNode x = this.root;

			while(x != this.nil)
			{
				SweepSegment ss = (SweepSegment)x.value;
				TurnType _turn = RPoint.SD(ss.UpperEnd, ss.LowerEnd, pt);

				if(_turn == TurnType.left)
				{
					x = x.left;
				}
				else if(_turn == TurnType.right)
				{
					x = x.right;
				}
				else
				{
					return x;
				}
			}			
			return x != this.nil ? x : null;
		}
				
	}
}
