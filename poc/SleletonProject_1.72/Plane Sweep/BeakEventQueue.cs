using System;
using BorradorTesis.Plane_Sweep.Nodes;

namespace BorradorTesis.Plane_Sweep
{
	/// <summary>
	/// Descripción breve de BeakEventQueue.
	/// </summary>
	public class BeakEventQueue: EventQueue
	{
		public BeakEventQueue()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		public void AddBeak(Event e)
		{
			Event _result = (Event)this.fset.Search(e);
			if(_result == null)
			{
				this.fset.Insert(e);				
			}
			else
			{
				if(e.Close != null)
				{
					if(_result.Open != null)
					{
						for(int i = 0; i < _result.Open.Count; i++)
						{
							if(!(_result.Open[i] is Beak))
							{
								((Beak)e.Close).AddIntersection((SweepSegment)_result.Open[i], e.Point);
							}
						}
					}
					if(_result.Close != null && !(_result.Close is Beak))
					{
						((Beak)e.Close).AddIntersection((SweepSegment)_result.Close, e.Point);
					}
				}
				else
				{
					if(_result.Open != null)
					{
						for(int i = 0; i < _result.Open.Count; i++)
						{
							if(!(_result.Open[i] is Beak))
							{
								((Beak)e.Open[0]).AddIntersection((SweepSegment)_result.Open[i], e.Point);
							}
						}
					}
					if(_result.Close != null && !(_result.Close is Beak))
					{
						((Beak)e.Open[0]).AddIntersection((SweepSegment)_result.Close, e.Point);
					}
				}
				if(_result.Close == null)
				{
					_result.Close = e.Close;
				}
				_result.AddOpen(e.Open);
				
			}
		}
	}
}
