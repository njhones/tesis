using System;
using Structures;
using BorradorTesis.Plane_Sweep.Trees; 
using BorradorTesis.Plane_Sweep.Nodes;

namespace BorradorTesis.Plane_Sweep
{
	/// <summary>
	/// Descripción breve de EventQueve.
	/// </summary>
	public class EventQueue
	{
		/**/
		protected RBTree fset;
		/**/
		protected RBNode act;
		/**/
		public EventQueue()
		{
			this.fset = new RBTree();
		}
		/**/
		public Event Actual
		{
			get
			{
				return this.act != null ? (Event)this.act.value : null;
			}
		}
		/**/
		public virtual void Add(Event e)
		{
			Event _result = (Event)this.fset.Search(e);
			if(_result == null)
			{
				this.fset.Insert(e);				
			}
			else
			{
				if(_result.Close == null)
				{
					_result.Close = e.Close;
				}
				_result.AddOpen(e.Open);
				
			}
		}
		/**/
		public bool IsEmpty
		{
			get
			{
				return this.fset.IsEmpty;
			}
		}
		/**/
		public Event NextEvent()
		{
			RBNode _temp = this.act;
			this.act = this.fset.TreeSussesor(this.act);
			this.fset.Delete(_temp);				
			return this.act != null ? (Event)this.act.value : null;		
		}
		/**/
		public void Init()
		{
			this.act = this.fset.TreeMinimun(this.fset.Root);
		}
	}
}
