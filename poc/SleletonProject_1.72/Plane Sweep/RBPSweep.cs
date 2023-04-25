using System;
using Structures;
using System.Collections;
using BorradorTesis.Plane_Sweep.Trees;
using BorradorTesis.Plane_Sweep.Nodes;

namespace BorradorTesis.Plane_Sweep
{
	/// <summary>
	/// Descripción breve de RBPSweep.
	/// </summary>
	public class RBPSweep
	{
		/**/
		protected EventQueue eq;
		/**/
		protected StatusTree st;		
		/**/
		public RBPSweep(EventQueue eq)
		{
			this.eq = eq;
			this.st = new StatusTree();
			if(!this.eq.IsEmpty)
			{
				this.eq.Init();
			}
		}
		/**/
		public void FindIntersections()
		{
			while(this.eq.Actual != null)
			{
				this.HandleEvent(eq.Actual);
				this.eq.NextEvent();
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
		protected virtual void HandleEvent(Event e)
		{
			SweepSegment.key = e.Point;			
			RBNode _node1, _node2;
			RBNode left = null, right = null;			
			

			if(e.Close != null)
			{
				ArrayList _aux = new ArrayList();		

				SweepSegment.afterline = false;		 
				_node1 = st.SearchNode(e.Close);
				SweepSegment.afterline = true;		
	
				_node2 = st.TreePredecessor(_node1);	
				
				while(_node2 != null && ((SweepSegment)_node2.value).Contain(e.Point))
				{
					_aux.Add(_node2);				
					if(((SweepSegment)_node2.value).LowerEnd != e.Point)
					{
						e.AddOpen((SweepSegment)_node2.value);
						((SweepSegment)_node2.value).UpperEnd = e.Point;
					}
					_node2 = st.TreePredecessor(_node2);
				}

				left = _node2;
//				_aux.Add(_node1);
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
					if(_node1.value is Beak && !(((RBNode)_aux[i] ).value is Beak)) 					
					{
						((Beak)_node1.value).AddIntersection((SweepSegment)((RBNode)_aux[i]).value, e.Point);
					}
					if(!(_node1.value is Beak) && ((RBNode)_aux[i]).value is Beak)					
					{
						((Beak)((RBNode)_aux[i]).value).AddIntersection((SweepSegment)_node1.value, e.Point);
					}									
					this.st.Delete(((RBNode)_aux[i]));
				}
				this.st.Delete(_node1);
			}
			if(e.Open != null && e.Open.Count > 0)
			{
				RBNode _rbnode = this.st.Insert((IComparable)e.Open[0]);

				if(e.Close == null)
				{
					left = this.st.TreePredecessor(_rbnode);
					right = this.st.TreeSussesor(_rbnode);						
				}	

				for(int i = 1; i < e.Open.Count; i++)
				{
					_rbnode = this.st.Insert((IComparable)e.Open[i]);
									
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
				if(left != null && right != null)
				{
					this.FindNewEvent((SweepSegment)left.value, (SweepSegment)right.value, e.Point);
				}
			}
		}

	}
}
