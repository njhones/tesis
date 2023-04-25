using System;
using Structures;
using System.Collections;
using BorradorTesis.Plane_Sweep.Trees;
using BorradorTesis.Plane_Sweep.Nodes;
using System.Drawing;

namespace BorradorTesis.Plane_Sweep
{
	/// <summary>
	/// Descripción breve de PlaneSweep.
	/// </summary>
	public class PlaneSweep
	{
		/**/
		protected EventQueue eq;
		/**/
		protected StatusTree st;
		/**/
		public PlaneSweep(EventQueue eq)
		{
			this.eq = eq;
			this.st = new StatusTree();
			if(!this.eq.IsEmpty)
			{
				this.eq.Init();
			}
		}
		/**/
		public virtual void FindNewEvent(SweepSegment s1, SweepSegment s2, RPoint pt)
		{
			if(s1.Intersect(s2))
			{
				RPoint it = s1.Intersection(s2);
				
				if(it.Y < pt.Y || (it.Y == pt.Y && it.X > pt.X))
				{
					Event ev;
					if(!it.Equals(s1.LowerEnd))
					{
						ev = new Event(it, s1, s1);	
						s1.UpperEnd = it;
					}
					else if(!it.Equals(s2.LowerEnd))
					{
						ev = new Event(it, s2, s2);	
						s2.UpperEnd = it;
					}
					else
					{
						ev = new Event(it, s1);
					}
					this.eq.Add(ev);
				}
			}
		}
		/**/
		public virtual bool NextStep(ref RPoint pt, ref ArrayList arr_close, ref ArrayList arr_open, ref SweepSegment left, ref SweepSegment right)
		{			
			if(eq.Actual != null)
			{
				left = right = null;
				RBNode leftnode, rightnode;
				this.HandleEvent(eq.Actual, ref arr_close, ref arr_open, out leftnode, out rightnode);
				if(leftnode != null)
				{
					left = (SweepSegment)leftnode.value;
				}
				if(rightnode != null)
				{
					right = (SweepSegment)rightnode.value;
				}
				pt = eq.Actual.Point;
				this.eq.NextEvent();
				return true;
			}
			return false;
		}
		/**/
		protected virtual void HandleEvent(Event e, ref ArrayList arr_close, ref ArrayList arr_open, out RBNode left, out RBNode right)
		{
			SweepSegment.key = e.Point;		//this.st.Key = e.Point;			
			RBNode _node1, _node2;
			left = right = null;					
			arr_open = e.Open;
			arr_close = new ArrayList();
				

//			if(e.Close == null)
//			{
////				_node1 = st.FindPoint(e.Point);
////
////				if(_node1 != null)
////				{
////					_close = ((SweepSegment)_node1.value).Segment;
////					if(!_close.LowerEnd.Equals(e.Point))
////					{
////						arr_open.Add(_close);
////					}
////				}
//			}
			if(e.Close != null)
			{
				ArrayList _aux = new ArrayList();		

				SweepSegment.afterline = false;		 //this.st.SetCompare = false;
				_node1 = st.SearchNode(e.Close);
				SweepSegment.afterline = true;				

				_node2 = st.TreePredecessor(_node1);	
				
				while(_node2 != null && ((SweepSegment)_node2.value).Contain(e.Point))
				{
					_aux.Insert(0, _node2);
					if(((SweepSegment)_node2.value).LowerEnd != e.Point)
					{
						e.AddOpen((SweepSegment)_node2.value);
						((SweepSegment)_node2.value).UpperEnd = e.Point;
					}
					_node2 = st.TreePredecessor(_node2);
				}
			
				left = _node2;
				_aux.Add(_node1);
				_node2 = st.TreeSussesor(_node1);				

				while(_node2 != null && ((SweepSegment)_node2.value).Contain(e.Point))
				{
					_aux.Add(_node2);
					if(((SweepSegment)_node2.value).LowerEnd != e.Point)
					{
						e.AddOpen((SweepSegment)_node2.value);	
						((SweepSegment)_node2.value).UpperEnd = e.Point;
					}
					_node2 = st.TreeSussesor(_node2);
				}
				
				right = _node2;
				
				for(int i = 0; i < _aux.Count; i++)
				{					
					arr_close.Add(((RBNode)_aux[i]).value);					
					this.st.Delete(((RBNode)_aux[i]));
				}
			}
			if(e.Open != null && e.Open.Count > 0)
			{				
				for(int i = 0; i < e.Open.Count; i++)
				{
					RBNode _rbnode = this.st.Insert((IComparable)arr_open[i]);

					if(i == 0 && e.Close == null)
					{
						left = this.st.TreePredecessor(_rbnode);
						right = this.st.TreeSussesor(_rbnode);						
					}					
				}
				e.Open.Clear();
				RBNode _temp = this.st.TreeSussesor(left);
				while(_temp != null && (right == null || _temp.value != right.value))
				{
					e.Open.Add(_temp.value);
					_temp = this.st.TreeSussesor(_temp);
				}
				if(left != null)
				{					
					this.FindNewEvent((SweepSegment)left.value, (SweepSegment)e.Open[0], e.Point);
				}
				if(right != null)
				{
					this.FindNewEvent((SweepSegment)e.Open[e.Open.Count - 1], (SweepSegment)right.value, e.Point);
				}
			}
			else
			{	
				//arr_open = new ArrayList();
				if(left != null && right != null)
				{
					this.FindNewEvent((SweepSegment)left.value, (SweepSegment)right.value, e.Point);
				}
			}
		}
	}
}
