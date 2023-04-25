using System;
using Structures;
using System.Collections;

namespace BorradorTesis.Plane_Sweep.Nodes
{
	/// <summary>
	/// Descripción breve de Event.
	/// </summary>
	public class Event: IComparable
	{
		/**/
		private RPoint p;
		/**/
		private ArrayList open;
		/**/
		private SweepSegment close;
		/**/
		public Event(RPoint p, SweepSegment close)
		{
			this.p = p;		
			this.close = close;			
		}		
		/**/
		public Event(RPoint p, SweepSegment close, SweepSegment open)
		{
			this.p = p;	
			this.close = close;		
			this.open = new ArrayList();
			this.open.Add(open);
		}
		/**/
		public Event(SweepSegment open, RPoint p)
		{
			this.p = p;	
			this.open = new ArrayList();
			this.open.Add(open);
		}		
		/**/
		public SweepSegment Close
		{
			get
			{
				return this.close;
			}
			set
			{
				this.close = value;
			}
		}
		/**/
		public ArrayList Open
		{
			get 
			{
				return this.open;
			}
		}
		/**/
		public void AddOpen(ArrayList OpenSeg)
		{
			if(OpenSeg != null)
			{
				if(this.open == null)
				{
					this.open = new ArrayList();
				}
				this.open.AddRange(OpenSeg);		
			}
		}
		/**/
		public void AddOpen(SweepSegment s)
		{
			if(this.open == null)
			{
				this.open = new ArrayList();
			}
			this.open.Add(s);
		}

		/**/
		public RPoint Point
		{
			get
			{
				return this.p;
			}
			set
			{
				this.p = value;
			}
		}


		#region Miembros de IComparable

		public int CompareTo(object obj)
		{
			Event e = obj as Event;
			if(this.p.Y == e.p.Y && this.p.X == e.p.X)
			{
				return 0;
			}
			if(this.p.Y > e.p.Y)
			{
				return -1;
			}
			if(this.p.Y < e.p.Y)
			{
				return 1;
			}
			if(p.X < e.p.X)
			{
				return -1;
			}
			return 1;
		}


		#endregion
	}
}
