using System;
using System.Collections;

namespace BorradorTesis.Plane_Sweep.GrowthStructures
{
	/// <summary>
	/// Descripción breve de Region.
	/// </summary>
	public class Region
	{	
		#region Fields
		
		public bool flag;
		private Interval I;
		
		#endregion

		#region Edition
		
		private void Add( ArrayList From, ArrayList To, bool Invert )
		{
			if( Invert )
			{
				for( int i=From.Count-1; i>=0; i-- )
					To.Add( From[i] );
			}
			else
			{
				To.AddRange( From );
			}
		}
		public void SetInterval( Interval i)
		{
			I = i;
		}
		public ArrayList GetVertex(bool External)
		{	
			if( External )
			{
				I.SetChild(I.GetRightChild(),I.GetLeftChild());
			}

			ArrayList Result = new ArrayList();

			if( ( I.GetRightChild() == null ) &&
				( I.GetLeftChild( ) == null ) )
			{
				/*
				 * Si estoy cogiendo los vertices de la cara exterior
				 * Tengo que invertir las listas que estan a la izquierda 
				 * en lugar de las que estan a la derecha.
				 */ 
				if( I.GetRegion() != null )
				{
					Add(I.GetLeft(),Result,External);
					Add(I.GetRight(),Result,!External);
				}

				return Result;
			}

			Interval First = I;

			Interval PreviousAct = null;
			Interval Act = I;
			bool Invalid = false;

			bool Finish = false;
			do
			{
				if( Act.GetRegion() == null )
				{
					Result.Clear();
					Invalid = true;
				}
				else
				{
					Act.GetRegion().flag = false;
				}

				Interval L,R;
				Interval[] P;

				P = Act.GetParents();	
				L = Act.GetLeftChild();
				R = Act.GetRightChild();
				
				Interval Dest = null;
				if( Act.CantOfParents() < 2 ) 
				{	
					#region Principio
					if( PreviousAct == null )
					{
						if( !Invalid ) Add(Act.GetLeft(),Result,External);

						Dest = L;
						if( Dest == null ) Dest = R;
					}
					else
					{
						#endregion
						#region Desde Arriba
						if( ( P != null) && ( PreviousAct == P[0] ) )
						{
							if( !Invalid ) Add(Act.GetLeft(),Result,External);

							Dest = L;
							if( Dest == null ) Dest = R;
						
							if( Dest == null )
							{
								if( !Invalid ) Add(Act.GetRight(),Result,!External);
								Dest = P[0];
							}
						}
						#endregion
						#region Desde Izquierda
						if( PreviousAct == L ) 
						{
							if( ( External ) && ( Act == First ) )
								Finish = true;
							else
							{
								Dest = R;
								if( Dest == null ) 
								{
									if( !Invalid ) Add(Act.GetRight(),Result,!External);
									if( P != null ) Dest = P[0];
								}
								if( Dest == null )
								{
									if( Act != First)
									{
										if( !Invalid ) Add(Act.GetLeft(),Result,External);
										Dest = L;
									}
									else Finish = true;
								}
							}
						}
					
						#endregion
						#region Desde Derecha
						if( PreviousAct == R ) 
						{
							if( ( External ) && ( Act == First ) )
								Dest = R;
							else
							{
						
								if( !Invalid ) Add(Act.GetRight(),Result,!External);
							
								if( P == null )
								{
									if( Act != First)
									{
										if( !Invalid ) Add(Act.GetLeft(),Result,External);
										Dest = L;
									}
									else Finish = true;
								}
								else
									Dest = P[0];						

								if( Dest == null ) Dest = R;
							}
						}
						#endregion   
					}
				}
				else
				{
					#region Desde Arriba Izquierda
					if( PreviousAct == P[0] ) 
					{
						if( !Invalid ) Add(Act.GetLeft(),Result,External);

						Dest = L;
						if( Dest == null ) Dest = R;
						
						if( Dest == null )
						{
							if( !Invalid ) Add(Act.GetRight(),Result,!External);
							Dest = P[1];
						}
					}
					#endregion					
					#region Desde Arriba Derecha
					if( PreviousAct == P[1] ) 
					{
						Dest = P[0];						
					}
					#endregion					
					#region Desde Abajo Izquierda
					if( PreviousAct == L ) 
					{
						Dest = R;
						if( Dest == null ) 
						{
							if( !Invalid ) Add(Act.GetRight(),Result,!External);
							Dest = P[1];
						}						
					}
					#endregion
					#region Desde Abajo Derecha
					if( PreviousAct == R ) 
					{
						if( !Invalid ) Add(Act.GetRight(),Result,!External);
						Dest = P[1];												
					}
					#endregion
				}				
				PreviousAct = Act;
				Act = Dest;

			}while( !Finish );
			return Result;
		}	
		
		public void Invert()
		{
			ArrayList Queue = new ArrayList();
			Queue.Add(I);
			I.SetFlag(-1);

			while( Queue.Count > 0 )
			{
				Interval Act = (Interval)Queue[0];
				Queue.RemoveAt(0);

				Interval L = Act.GetLeftChild();
				Interval R = Act.GetRightChild();
				Interval[] P = Act.GetParents();

				if( ( L != null ) && ( L.GetFlag() >= 0 ) )
				{
					Queue.Add(L);
					L.SetFlag(-1);
				}
				
				if( ( R != null ) && ( R.GetFlag() >= 0 ) )
				{
					Queue.Add(R);
					R.SetFlag(-1);
				}
				
				if( P != null ) 
				{
					if( P[0].GetFlag() >= 0 ) 
					{
						Queue.Add(P[0]);
						P[0].SetFlag(-1);
					}
					if( P.Length == 2 )
					{
						if( P[1].GetFlag() >= 0 ) 
						{
							Queue.Add(P[1]);
							P[1].SetFlag(-1);
						}
					}
				}

				if( (P != null ) && ( P.Length == 2 ) )
				{
					Act.SetChild( P[0],P[1] );					
				}
				else
				{
					if( P == null )
						Act.SetChild( null,null );					
					else 
					{
						if( P[0].GetFlag() == -2 )
						{
							Interval[] Parents;
							Parents = Act.GetParents()[0].GetParents();

							if( Act == Parents[0] )
								Act.SetChild( null,P[0] );
							else
								Act.SetChild( P[0],null );
						}
						else
						{
							if( Act == P[0].GetLeftChild() )
								Act.SetChild( null,P[0] );
							else
								Act.SetChild( P[0],null );
						}
					}
					
				}

				if( ( L != null ) && ( R != null ) )
				{
					Interval[] Parents = {L,R};
					Act.SetParents( Parents );
				}
				else
				{
					if( ( L != null ) || ( R != null ) )
					{
						Interval[] Parents = new Interval[1];
						if( L!=null ) Parents[0] = L;
						if( R!=null ) Parents[0] = R;
						Act.SetParents(Parents);
					}
					else Act.SetParents( null );
				}
				Act.SetFlag(-2);
			}
		}
		#endregion

		public Region()
		{
			flag = true;            
		}
	}
}
