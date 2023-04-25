using System;
using System.Collections;
using BigNumbers;

namespace Growth_Polygon
{
	/// <summary>
	/// 
	/// </summary>
	public class Polygonal
	{
		protected class S
		{
			public R_Point Pt;
			public bool Reversed;
			public int LTO_0;
			public int LTO_1;

			public S()
			{
				LTO_0 = LTO_1 = -1;
				Reversed = false;
				Pt = null;
			}
		}
		#region constants
		protected long Decimals_Den=100;
		#endregion

		#region fields
			protected IntersectionTree[] I_List;
			protected ArrayList List;
			protected int Count;
		#endregion

		#region properties
		
		public R_Point this[int index]
		{
			get
			{
				return Get_Vertex(index);
			}
			set
			{
                Replace_Vertex(value,index);				
			}
		}
		

		
			
		#endregion

		#region constructors
			public Polygonal()
		{
				Count = 0;
				List = new System.Collections.ArrayList();
			// 
			// TODO: Add constructor logic here
			//
		}
		#endregion

		
		#region editions
			public int CantOfVertex()
			{
				return Count;
			}			
			public int GetMostLeftDown()
			{
				int Result = 0;
				for(int i = 0; i < List.Count; i++ )
				{
					if( ( ( this[i]).x < ( this[Result] ).x ) ||
						( ( ( this[i]).x == (this[Result] ).x ) &&
						( ( this[i]).y < (this[Result] ).y ) ) )
					{
						Result = i;
					}

				}
				return Result;
			}
			public virtual bool Add_Vertex(R_Point V)
			{
				Polygonal.S Aux = new Polygonal.S();
				Aux.Pt = V;
				List.Add( Aux );
				Count++;
				return true;
			}
			protected virtual bool Add_Vertex(Polygonal.S V)
			{
				List.Add( V );
				Count++;
				return true;
			}
			public void Add_Range( ArrayList L, bool SameOrder )
			{
				if( SameOrder )
				{
					List.AddRange( L );
					Count += L.Count;
				}
				else
				{
					for(int i=L.Count-1; i>=0; i--)
					{
						Add_Vertex( (S)L[i] );
					}
				}
			}
			public virtual bool Insert_Vertex(R_Point V,int Pos)
			{
				S Aux = new Polygonal.S();
				Aux.Pt = V;
				Count++;
				List.Insert( Pos,Aux );
				return true;
			}

			public virtual bool Replace_Vertex(R_Point V,int Pos)
			{
				((S)List[Pos]).Pt = V;
				return true;
			}
			public virtual bool Remove_Vertex(int Pos)
			{
				Count--;
				List.RemoveAt( Pos );
				return true;
			}
			public virtual bool Remove_ALL( )
			{			
				Count = 0;
				List.Clear( );
				return true;
			}
			public R_Point Get_Vertex(int Pos)
			{
				return ((S)List[Pos]).Pt;
			}
			
			public ArrayList GetVertexes()
			{
				return List;
			}
			
			public ArrayList GetList( )
			{
				return List;
			}
			public int GetRelationIndex( int i1,int i2)
			{		
				if( i2 == 0 )
					return ((S)List[i1]).LTO_0;
				else
					return ((S)List[i1]).LTO_1;
			}
			public void SetRelationIndex( int val, int i1,int i2)
			{	
				if( i2 == 0 )
					((S)List[i1]).LTO_0 = val;
				else
					((S)List[i1]).LTO_1 = val;				
			}
			public void CopyRelationIndex( int id1,int id2)
			{	
				((S)List[id2]).LTO_0 = ((S)List[id1]).LTO_0;
				((S)List[id2]).LTO_1 = ((S)List[id1]).LTO_1;
			}
			public bool isInvert( int ind )
			{
				return ((S)List[ind]).Reversed;
			}
			public void SetInvert( int ind )
			{
				((S)List[ind]).Reversed = true;
			}
			public void ResetInvert( int ind )
			{
				((S)List[ind]).Reversed = false;
			}
		#endregion

		#region General
		virtual public void MinAngle_30()
		{
			for( int i = 0; i < Count-1; i++ )
			{
				/*R_Point Pt = Segment.Proyect( (R_Point)List[i-1], (R_Point)List[i], (R_Point)List[i+1] );
				if( R_Point.SD( (R_Point)List[i-1], (R_Point)List[i], (R_Point)List[i+1] ) != 
					R_Point.SD( (R_Point)List[i-1],       Pt       , (R_Point)List[ i ] ) )
				{
					BigRational COp_2 = R_Point.GetQuadraticDistance( (R_Point)List[i-1], Pt );
					BigRational Hip_2 = R_Point.GetQuadraticDistance( (R_Point)List[i-1], (R_Point)List[i] );
					BigRational Sin_2 = COp_2 / Hip_2;
					long ent = BigInteger.BigIntegerToLong( Sin_2.entero );
					long num = BigInteger.BigIntegerToLong( Sin_2.numerador );
					long den = BigInteger.BigIntegerToLong( Sin_2.denominador );
					double Sin = Math.Sqrt(ent + num/den);
					double angle = Math.Asin(Sin);
					if( angle < Math.PI/6 )
					{
					}
				}*/
			}
		}
			
		
		virtual public void InitializeIntersectionList( )
		{
			if( I_List == null )
				I_List = new IntersectionTree[List.Count-1];
		}
		virtual public void AddIntersection(R_Point pt, int Id_List, int Id_Seg )
		{
			if( I_List[Id_List] == null ) I_List[Id_List] = new IntersectionTree();

			Object result;
			I_List[ Id_List ].Insert( new Intersection(pt,Id_Seg,Id_List), out result );
		}
		virtual public void RemovePeak(int dist, out ArrayList Result)
		{
			int i;
			EventQueue EQ = new EventQueue();

			#region Inicializando EQ
			
			for( i = 0; i < this.CantOfVertex()-1; i++)
			{
				Segment Seg = new Segment((R_Point)this[i],(R_Point)this[i+1]);
				Seg.Id = i;
				
			
				Event E,E1;

				E = new Event( Seg.GetUpperEnd(), null );
				E.Add_S_Open( Seg );
				EQ.AddEvent( E,out E1 );
				
			
				E = new Event( Seg.GetLowerEnd(), Seg );
				EQ.AddEvent( E,out E1 );
			}
			#endregion

			
			PlaneSweep PS = new PlaneSweep(EQ);
			
			ArrayList SC = null;
			ArrayList SO = null;
			Segment[] Extremes = null;
			R_Point pt;
			ArrayList Regions = new ArrayList();

			#region Detect Intersections
			this.InitializeIntersectionList( );

			while( PS.NextStep( out pt, out SC, out SO, out Extremes) )
			{
				int n = SC.Count + SO.Count;
				if( n < 2 ) continue;

				for( i=0; i<n-1; i++)
				{					
					Segment S;

					Segment Seg_MajorId;
					Segment Seg_MinorId;

					if( i < SC.Count )
						S = (Segment)SC[i];
					else
					{
						S = (Segment)SO[i - SC.Count];

						/* 
						 * Si el segmento ya se analizo en la lista de cierre no lo
						 * analizo en la lista de apertura.
						 */ 
						if( S.flag ) continue;
					}
				
					bool InLast  = false,
						InFirst = false;

					// El segmento de mayor id se analiza cerrado alante y abierto atras
					if( ( S.GetEnd().x == pt.x ) &&
						( S.GetEnd().y == pt.y ) ) InLast = true;

					// El segmento de menor id se analiza cerrado atras y abierto alante
					if( ( S.GetInit().x == pt.x ) &&
						( S.GetInit().y == pt.y ) ) InFirst = true;;

					for( int j=i+1; j<n; j++)
					{
						if( j < SC.Count )
							Seg_MajorId = (Segment)SC[j];
						else
						{
							Seg_MajorId = (Segment)SO[j - SC.Count];
							/* 
							* Si el segmento ya se analizo en la lista de cierre no lo
							* analizo en la lista de apertura.
							*/
							if( Seg_MajorId.flag ) continue; 
						}

						// Si los segmentos son consecutivos no los analizo.
						if( Math.Abs( Seg_MajorId.Id - S.Id ) == 1 ) continue;

						if( Seg_MajorId.Id < S.Id )
						{
							Seg_MinorId = Seg_MajorId;
							Seg_MajorId = S;

							// El segmento de mayor id se analiza cerrado alante y abierto atras
							if( InLast ) continue;

							// El segmento de menor id se analiza cerrado atras y abierto alante
							if( ( Seg_MinorId.GetInit().x == pt.x ) &&
								( Seg_MinorId.GetInit().y == pt.y ) ) continue;
						}
						else 
						{
							Seg_MinorId = S;
							// El segmento de menor id se analiza cerrado atras y abierto alante
							if( InFirst ) continue;

							// El segmento de mayor id se analiza cerrado alante y abierto atras
							if( ( Seg_MajorId.GetEnd().x == pt.x ) &&
								( Seg_MajorId.GetEnd().y == pt.y ) ) continue;
						}


						if( ( R_Point.SD( Seg_MajorId.GetInit(), Seg_MajorId.GetEnd(), Seg_MinorId.GetInit() ) == Math.Sign( dist ) ) &&
							( R_Point.SD( Seg_MinorId.GetInit(), Seg_MinorId.GetEnd(), Seg_MajorId.GetEnd( ) ) == Math.Sign( dist ) ) )
						{
							this.AddIntersection(pt, Seg_MajorId.Id, Seg_MinorId.Id );
						}
					}
				}
			}
			#endregion

			IntersectionTree Intersections = new IntersectionTree();

			#region Process Peak
			
			bool All_Processed;
			
			do
			{	
				i = 1;
				bool Active = false;
				All_Processed = true;
				Intersection Act = null;
				int idAct = -1;
				bool Out = false;

				while( i < this.CantOfVertex()-1 )
				{
					if( Active )
					{
						RBNode Temp = null;
					
						if( I_List[i] != null ) Temp = ( (IntersectionTree)I_List[i] ).GetIntersection( idAct );
						

						while( ( Temp != null ) &&
							( ( (Intersection)Temp.Value() ).GetMinor() > Act.GetMinor() ) )
						{
							RBNode INode;
							Intersection I;
							if( Intersections.Contain( ( (Intersection)Temp.Value() ).GetMinor(), out INode ) )
							{
								I = (Intersection)INode.Value();

								int id = ( (Intersection)Temp.Value() ).GetMinor();

								R_Point P1,P2,Pt;
								P1 = P2 = Pt = null;

								if( id == I.GetMinor() )
								{
									INode = Intersections.Antecessor( INode );
								
									if( (INode != null ) && ( id == ((Intersection)INode.Value()).GetMajor() ) )
										P1 = I.GetPoint();
									else
										P1 = this[I.GetMinor()];								

									P2 = I.GetPoint();
									Pt = ( (Intersection)Temp.Value() ).GetPoint();
								}
								if( id == I.GetMajor() )
								{
									P1 = I.GetPoint();

									INode = Intersections.Successor( INode );
								
									if( (INode != null ) && ( id == ((Intersection)INode.Value()).GetMinor() ) )
										P2 = I.GetPoint();
									else
										P2 = this[I.GetMajor()+1];

									Pt = ( (Intersection)Temp.Value() ).GetPoint();
								}

								if( Pt != null )
								{
									if( ( ( P1.x - Pt.x ).sign ^ ( P2.x - Pt.x ).sign ) ||
										( ( P1.y - Pt.y ).sign ^ ( P2.y - Pt.y ).sign ) ) 
									{
										Act = (Intersection)Temp.Value();
										break;
									}
								}

								Temp = ( (IntersectionTree)I_List[i] ).Antecessor( Temp );
							}
							else 
							{
								Act = (Intersection)Temp.Value();
								break;
							}
						}
					}
				
					if( this[i].flag )
					{
						if( Active )
						{
							Object I_Aux = null;
							if( ( Act.GetMinor() != -1 ) && ( Act.GetMajor() != -1 ) )
							{
								this[i].flag = false;
								Act.GetPoint().flag = this[Act.GetMajor()].flag;
								Intersections.Insert( Act,out I_Aux );	
							}
							else
								All_Processed = false;
						}
						Act = new Intersection(null,-1,-1);
						idAct = i;
						Out = false;
						Active = true;
					}
				
					if( ( !Out) && ( Active ) && 
						( R_Point.SD( this[idAct],this[idAct+1],this[i+1] ) == Math.Sign( dist ) ) ) Out = true;
			
					if( ( Out ) &&
						( R_Point.SD( this[idAct],this[idAct+1],this[i+1] ) != Math.Sign( dist ) ) )
					{
						Out = false;
						Object I_Aux = null;
						if( ( Act.GetMinor() != -1 ) && ( Act.GetMajor() != -1 ) )
						{
							this[i].flag = false;
							Act.GetPoint().flag = this[Act.GetMajor()].flag;
							Intersections.Insert( Act,out I_Aux );						
						}
						else
							All_Processed = false;

						Active = false;
						Act = null;
					}
					i++;
				}
			
				if( Active )
				{
					Object I_Aux = null;
					if( ( Act.GetMinor() != -1 ) && ( Act.GetMajor() != -1 ) )
					{
						this[i].flag = false;
						Act.GetPoint().flag = this[Act.GetMajor()].flag;
						Intersections.Insert( Act,out I_Aux );					
					}
					else 
						All_Processed = false;
				}
			}while( !All_Processed );
			#endregion
			
			

			Result = List;

			List = new ArrayList( Result.Count );
			Count = 0;

			i = 0;
			RBNode I_Node = Intersections.GetMinor();

			do
			{			
				int minor = -1, 
					major = -1;
				R_Point Pt = null;

				if( I_Node != null )
				{
					minor = ( (Intersection)I_Node.Value() ).GetMinor();
					major = ( (Intersection)I_Node.Value() ).GetMajor();
					Pt	  = ( (Intersection)I_Node.Value() ).GetPoint();
				}
				else
				{
					minor = Result.Count;
				}
				
				for( int j = i; j < minor; j++ )
				{
					Add_Vertex( (S)Result[j] );
					i++;
				}

				if( I_Node != null )
				{
					if( ( minor >= i ) && ( ( Pt.x != ((S)Result[ minor ]).Pt.x ) || ( Pt.y != ((S)Result[ minor ]).Pt.y ) )  )
					{
						Add_Vertex( (S)Result[minor] );
					}
				
					if( ( Pt.x != ((S)Result[ major + 1 ]).Pt.x ) || ( Pt.y != ((S)Result[ major + 1 ]).Pt.y ) ) 
					{
						Add_Vertex( Pt );

						this.SetRelationIndex( ((S)Result[ major ]).LTO_0, Count - 1, 0 );
						this.SetRelationIndex( ((S)Result[ major ]).LTO_1, Count - 1, 1 );
						this[Count - 1].flag = ((S)Result[ major ]).Pt.flag;
					}				
					i = major + 1;
					I_Node = Intersections.Successor( I_Node );
				}
				
			}while( I_Node != null );

			for( int j = i; j < Result.Count; j++ )
			{
				Add_Vertex( (S)Result[j] );
			}
		}	

		#endregion

		
	}

	
	/// <summary>
	/// 
	/// </summary>
	public class Polygon: Polygonal
	{
		#region constructores
		public Polygon()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		public Polygon(ArrayList list)
		{
			for( int i=0; i<list.Count;i++)
			{
				if( list[i] is R_Point )
				{					
					Add_Vertex( (R_Point)list[i] );
				}
				else
				{
					Add_Vertex( (Polygonal.S)list[i] );
				}
			}
		}
		#endregion

		#region General
		public ArrayList GetValid( int sgDist )
		{
			int i;
			EventQueue EQ = new EventQueue();

			#region Inicializando EQ
			
			for( i = 0; i < this.CantOfVertex(); i++)
			{
				Segment Seg = new Segment((R_Point)this[i],(R_Point)this[(i+1) % List.Count]);
				
			
				Event E,E1;

				E = new Event( Seg.GetUpperEnd(), null );
				E.Add_S_Open( Seg );
				EQ.AddEvent( E,out E1 );
				
			
				E = new Event( Seg.GetLowerEnd(), Seg );
				EQ.AddEvent( E,out E1 );
			}
			#endregion

			
			PlaneSweep PS = new PlaneSweep(EQ);
			
			ArrayList SC = null;
			ArrayList SO = null;
			Segment[] Extremes = null;
			R_Point pt;
			ArrayList Regions = new ArrayList();
			int inter = 0;

			while( PS.NextStep( out pt, out SC, out SO, out Extremes) )
			{
				Interval L = null;
				Interval R = null;
				Interval[] P = null;

				#region Cierre
				if( SC.Count == 0 )
				{
					P = new Interval[1];
					if( ( Extremes[0] == null ) && ( Extremes[1] == null ) )
					{
						Region reg = null;
						int flag = System.Math.Sign( -sgDist - 1 );

						P[0] = new Interval(reg,flag,inter++,true);
						
						L = new Interval( P,P[0].GetRegion(),P[0].GetFlag(),inter++,true );
						R = new Interval( P,P[0].GetRegion(),P[0].GetFlag(),inter++,true );

						P[0].SetChild( L,R );
						
						if( flag == 0 ) 
						{
							reg = new Region();
							
							P[0].SetRegion( reg	);
							L.SetRegion(    reg );
							R.SetRegion(    reg );

							Regions.Add(reg);							
			
							P[0].SetLeft( new IntervalNode( pt, null, null ) );
							P[0].SetRight( P[0].GetLeft() );

							P[0].GetLeft().Val.S1 = (Segment)SO[0];
							P[0].GetLeft().Val.S2 = (Segment)SO[SO.Count-1];

							R.SetLeft(  P[0].GetLeft( ) );
							L.SetRight( P[0].GetRight() );
						}

						if( reg != null ) reg.SetInterval(P[0]);
					}
					else
					{
						if( Extremes[0] == null )
							P[0] = Extremes[1].GetLeft();
						else
							P[0] = Extremes[0].GetRight();

						L = new Interval( P,P[0].GetRegion(),P[0].GetFlag(),inter++,P[0].isExterior() );
						R = new Interval( P,P[0].GetRegion(),P[0].GetFlag(),inter++,P[0].isExterior() );

						P[0].SetChild( L,R );
					
						if( P[0].GetFlag() == 0 ) 
						{	
							L.SetLeft( P[0].GetLeft() );
							R.SetRight( P[0].GetRight() );
							
							R.SetLeft( new IntervalNode( pt, null, null ) );
							L.SetRight( R.GetLeft() );

							L.GetRight().Val.S1 = (Segment)SO[0];
							L.GetRight().Val.S2 = (Segment)SO[SO.Count-1];
						}
					}

					if( Extremes[0] != null ) Extremes[0].SetRight( L );
					if( Extremes[1] != null ) Extremes[1].SetLeft( R );
				}
				else
				{
					for( i = 0; i < SC.Count; i++ )
					{
						Segment S = (Segment)SC[i];
						
						if( S.GetRight().GetFlag() == 0 ) 
						{
							S.GetRight().AddLeft( pt );
							S.GetRight().GetLeft().Val.S1 = (Segment)SC[i];
								
							if( i < SC.Count-1 ) 
							{
								S.GetRight().GetLeft().Val.S2 = (Segment)SC[i+1];

								S.GetRight().GetLeft().Left = S.GetRight().GetRight();
								S.GetRight().GetRight().Right = S.GetRight().GetLeft();
							}
							else
							{
								if( SO.Count > 0 )
									S.GetRight().GetLeft().Val.S2 = (Segment)SO[SO.Count-1];
								else
									S.GetRight().GetLeft().Val.S2 = (Segment)SC[0];
							}							
						}
					}
					
					L = ((Segment)SC[0]).GetLeft();
					R = ((Segment)SC[SC.Count-1]).GetRight();
					
					if( ( SO.Count > 0 ) && ( L.GetFlag() == 0 ) )
					{						
						L.AddRight(pt);

						L.GetRight().Val.S1 = (Segment)SC[0];
						L.GetRight().Val.S2 = (Segment)SO[0];
					}
				}
				#endregion

				#region Apertura
				if( SO.Count == 0 )
				{
					P = new Interval[2];
					P[0] = L;
					P[1] = R;
					
					Region reg;
					
					int flag = System.Math.Sign( -sgDist - 1 );						
					
					reg = L.GetRegion();
											
					Interval I = new Interval(P,reg,L.GetFlag(),inter++,P[0].isExterior() );
					
					L.SetChild(null,I);
					R.SetChild(I,null);
					
					if( Extremes[0] != null ) Extremes[0].SetRight( I );
					if( Extremes[1] != null ) Extremes[1].SetLeft( I );	

					if( L.GetFlag() == 0 )
					{
						R.GetRegion().SetReferenced();

						R.GetLeft().Left = L.GetRight();
						L.GetRight().Right = R.GetLeft();

						I.SetLeft( L.GetLeft() );
						I.SetRight( R.GetRight() );
					}					
				}
				else
				{
					Interval I = null;
					Region reg = null;

					I = L;

					for(i=1; i<SO.Count; i++)
					{
						int flag;

						if( ( ( (Segment)SO[i-1]).GetInit().y > pt.y ) ||
							( ( ( (Segment)SO[i-1]).GetInit().y == pt.y ) &&
							( ( (Segment)SO[i-1]).GetInit().x <= pt.x ) ) )
							flag = I.GetFlag() + 1;
						else
							flag = I.GetFlag() - 1;

						if( ( flag == 0 ) && 
							( !((Segment)SO[i-1]).isCoincident((Segment)SO[i]) ) )
						{
							reg = new Region();
							Regions.Add(reg);							
						}
						
						I = new Interval(reg,flag,inter++,false);

						//Si reg es distinto de null es que flag es 0 y no son coincidentes
						if( reg != null ) 
						{
							reg.SetInterval(I);

							IntervalNode IN = new IntervalNode(pt,null,null);

							IN.Val.S1 = (Segment)SO[i-1];
							IN.Val.S2 = (Segment)SO[ i ];

							I.SetRight( IN );
							I.SetLeft( IN );							
						}
						reg = null;

						( (Segment)SO[i-1]).SetRight( I );
						( (Segment)SO[ i ]).SetLeft(  I );                        
					}

					( (Segment)SO[ 0 ]).SetLeft( L );
					
					
					( (Segment)SO[ SO.Count-1 ]).SetRight( R ); 
					
				}
				#endregion
			}

			ArrayList Faces = new ArrayList();

			for( i = 0; i<Regions.Count; i++ )
			{
				Region reg = (Region)Regions[i];
				/*if( !reg.isReferenced() )
				{*/
					ArrayList Face = reg.GetVertex();
					Faces.Add(Face);
				//}
			}//Ver que pasa con la region exterior

			return Faces;	
		}
		
		#endregion
	}


	/// <summary>
	/// 
	/// </summary>
	public class Simple_Polygonal: Polygonal
	{
		#region constructors
		public Simple_Polygonal()
		{
			Count = 0;
			List = new System.Collections.ArrayList();
			I_List = null;
			// 
			// TODO: Add constructor logic here
			//
		}
		public Simple_Polygonal(ArrayList list)
		{
			for( int i=0; i<list.Count;i++)
			{
				Add_Vertex( new R_Point( (L_Point)list[i] ) );
				this[i].flag = true;
			}
			I_List = null;
		}
		#endregion

		#region General

		virtual protected bool Get_Polygonal(int dist, out Polygonal R, bool V_Int)
		{
			ArrayList list = new ArrayList();

			int i;
			
			R = new Polygonal();

			if( V_Int )
			{
				R_Point[] Point = null;
				
				for( i=0; i < CantOfVertex()-1; i++)
				{
					Point = new R_Point[2];

					Point[0] = this[i+1].Clone();
					Point[1] = this[ i ].Clone();

					//	Traslado el eje de coordenadas a los puntos extremos correspondientes.

					Point[0].x -= this[i].x;
					Point[0].y -= this[i].y;

					Point[1].x -= this[i+1].x;
					Point[1].y -= this[i+1].y;
								
					double aux;
					System.Int64 Ent,Num;

					System.Int64 a1, b1, A;				
					BigRational d1, x, y;

					for( int j = 0; j<2; j++)
					{
						int sg = -Math.Sign((int)(Math.Pow((double)-1,(double)j)));
						a1  = sg*( BigInteger.BigIntegerToLong( Point[j].x.entero ) );
						b1  = sg*( BigInteger.BigIntegerToLong( Point[j].y.entero ) ); 
						A  = (a1*a1 + b1*b1);

						/*Calculo de la distancia aproximada del segmento*/
						aux = Math.Abs(dist)*Math.Sqrt(A);
						Ent = (System.Int64)Math.Floor( aux );
						aux -= Ent; 
						Num = (System.Int64)Math.Ceiling( aux *Decimals_Den );

						d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
					
						x =  b1*d1/A;
						y = -a1*d1/A;

						Point[j] = new R_Point(x,y);
					}

					//Reposiciono el eje de cordenada para ambos resultados
				
					
					Point[0].x += this[i].x;
					Point[0].y += this[i].y;

					Point[1].x += this[i+1].x;
					Point[1].y += this[i+1].y;		

					//Actualizo el link a la original y la lista de los puntos racionales
					
					R.Add_Vertex(Point[0]);
					R.SetRelationIndex(  i , R.CantOfVertex()-1, 0 );
					R.SetRelationIndex( i+1, R.CantOfVertex()-1, 1 );
					R.Add_Vertex(Point[1]);

					if( ( i<this.CantOfVertex()-2 ) && 
						( R_Point.SD(this[i],this[i+1],this[i+2] ) == Math.Sign(dist) ) )
					{
						Point[1].flag = true;
					}
                    
					Point = null;
				}

			}
			return true;
		}
		virtual public ArrayList Parallel(int dist, out Polygonal Result)
		{
			Result = null;
			int i = 0;
			while(  ( i < Count) && 
				( this[i].x.numerador == 0 ) && 
				( this[i].y.numerador == 0 )  )
			{
				i++;
			}

			bool finish = true;
			do
			{
				Result = null;
				try
				{
					if ( !Get_Polygonal( dist, out Result, (i == Count) ) )throw new Exception("Lanzo una excepcion");//Ojo
					finish = false;
				}
				catch( InvertPeak  )
				{
                    finish = true;               			
					i=Count;			
				}
			}while( finish );

			if( Result.CantOfVertex() == 0 )
			{
				return new ArrayList();
			}

			/*ArrayList Faces = new ArrayList();
			return Faces;
			/*/
			ArrayList Old;
			Result.RemovePeak(dist,out Old);

			int Ind1 = GetMostLeftDown();
			int Ind2 = Result.GetMostLeftDown();

			R_Point P1 = this[Ind1];
			R_Point P2 = Result[Ind2];

			ArrayList Aux = new ArrayList();

			Polygon P = new Polygon( );

			if( ( P1.x < P2.x ) ||
				( ( P1.x == P2.x ) &&
				( P1.y < P2.y ) ) )
			{
				P.Add_Range( List, true );
				P.Add_Range( Result.GetList(), false );
			}
			else
			{
				P.Add_Range( Result.GetList(), true );
				P.Add_Range( List, false );								
			}
						
			
			ArrayList aux = P.GetValid(-1);
			ArrayList Faces = new ArrayList();

			for(i=0; i<aux.Count;i++)
			{
				int n = ((ArrayList)aux[i]).Count;
				R_Point Pt;
				ArrayList Temp; 

				int init=0;
				int j;
				if( n > 0 )
				{
					Pt = ((IntervalNode.VertexOfRegion)((ArrayList)aux[i])[0]).Pt;
				
					if( !Pt.flag )
					{
						j = n;
						Pt = ((IntervalNode.VertexOfRegion)((ArrayList)aux[i])[j-1]).Pt;
						while( ( j > 0 ) &&  ( !Pt.flag ))
						{
							j--;
							if( j > 0 ) Pt = ((IntervalNode.VertexOfRegion)((ArrayList)aux[i])[j-1]).Pt;
						}
						if( j != n ) init = j;
					}
				
					j = init;
					Pt = ((IntervalNode.VertexOfRegion)((ArrayList)aux[i])[j]).Pt;

					int cant = 0;
					do
					{			
						Temp = new ArrayList(); 

						while( ( !Pt.flag ) && ( cant != n ) )
						{
							Temp.Add( Pt );
							j = ( j + 1 )%n;
							Pt = ((IntervalNode.VertexOfRegion)((ArrayList)aux[i])[j]).Pt;
							cant++;
						}
						while( ( Pt.flag ) && ( cant != n ) )
						{
							j = ( j + 1 )%n;						
							Pt = ((IntervalNode.VertexOfRegion)((ArrayList)aux[i])[j]).Pt;
							cant++;
						}

						Faces.Add( Temp );
					}
					while( j != init );
				}
			}

			return Faces;/**/

		}	
		

		#endregion
	}


	/// <summary>
	/// 
	/// </summary>
	public class Simple_Polygon: Simple_Polygonal
	{
		#region constructores
		public Simple_Polygon()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		public Simple_Polygon(ArrayList list)
		{
			for( int i=0; i<list.Count;i++)
			{
				if( list[i] is L_Point )
				{
					if( i < list.Count-1 )
                        Add_Vertex( new R_Point( (L_Point)list[i] ) );
					this[i].flag = true;
				}
				else
					Add_Vertex( (R_Point)list[i] );
			}
		}
		#endregion

		#region General
		override protected bool Get_Polygonal(int dist, out Polygonal R, bool V_Int)
		{
			ArrayList list = new ArrayList();

			int i;
			
			R = new Polygonal();

			if( V_Int )
			{
				R_Point[] Point = null;
				
				for( i=0; i < CantOfVertex(); i++)
				{
					Point = new R_Point[2];

					Point[0] = this[(i+1)%CantOfVertex()].Clone();
					Point[1] = this[ i ].Clone();

					//	Traslado el eje de coordenadas a los puntos extremos correspondientes.

					Point[0].x -= this[i].x;
					Point[0].y -= this[i].y;

					Point[1].x -= this[(i+1)%CantOfVertex()].x;
					Point[1].y -= this[(i+1)%CantOfVertex()].y;
								
					double aux;
					System.Int64 Ent,Num;

					System.Int64 a1, b1, A;				
					BigRational d1, x, y;

					for( int j = 0; j<2; j++)
					{
						int sg = -Math.Sign((int)(Math.Pow((double)-1,(double)j)));
						a1  = sg*( BigInteger.BigIntegerToLong( Point[j].x.entero ) );
						b1  = sg*( BigInteger.BigIntegerToLong( Point[j].y.entero ) ); 
						A  = (a1*a1 + b1*b1);

						/*Calculo de la distancia aproximada del segmento*/
						aux = Math.Abs(dist)*Math.Sqrt(A);
						Ent = (System.Int64)Math.Floor( aux );
						aux -= Ent; 
						Num = (System.Int64)Math.Ceiling( aux *Decimals_Den );

						d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
					
						x =  b1*d1/A;
						y = -a1*d1/A;

						Point[j] = new R_Point(x,y);
					}

					//Reposiciono el eje de cordenada para ambos resultados
				
					
					Point[0].x += this[i].x;
					Point[0].y += this[i].y;

					Point[1].x += this[(i+1)%CantOfVertex()].x;
					Point[1].y += this[(i+1)%CantOfVertex()].y;		

					//Actualizo el link a la original y la lista de los puntos racionales
					
					R.Add_Vertex(Point[0]);
					R.SetRelationIndex(           i         , R.CantOfVertex()-1, 0 );
					R.SetRelationIndex( (i+1)%CantOfVertex(), R.CantOfVertex()-1, 1 );
					R.Add_Vertex(Point[1]);
                    
					Point = null;
				}

			}
			return true;
		}
		override public ArrayList Parallel(int dist, out Polygonal Result)
		{	
			int i = 0;
			while(  ( i < Count) && 
				( this[i].x.numerador == 0 ) && 
				( this[i].y.numerador == 0 )  )
			{
				i++;
			}
			bool finish = true;
			do
			{
				Result = null;
				try
				{
					if ( !Get_Polygonal( dist, out Result, (i == Count) ) )throw new Exception("Lanzo una excepcion");//Ojo
					finish = false;
				}
				catch( InvertPeak )
				{
					finish = true; 
               		i=Count;		
				}
			}while( finish );

			if( Result.CantOfVertex() == 0 )
			{
				return new ArrayList();
			}
			if( Result.CantOfVertex() <=2 )
			{
				ArrayList Res = new ArrayList();
				Res.Add( Result );
				return Res;
			}
			Polygon P = new Polygon(Result.GetVertexes() );
			ArrayList Faces = P.GetValid(Math.Sign(dist));
			return Faces;

			

		}
		

		#endregion
	}
		
	
}
