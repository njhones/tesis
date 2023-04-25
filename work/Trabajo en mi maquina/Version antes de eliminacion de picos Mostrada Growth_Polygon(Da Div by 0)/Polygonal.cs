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
				if( this.isInvert(i) ) Seg.SetReversed();
				
			
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
						if( flag == 0 ) 
						{
							reg = new Region();
							Regions.Add(reg);							
						}

						P[0] = new Interval(reg,flag,inter++);
						if( reg != null ) reg.SetInterval(P[0]);
					}
					else
					{
						if( Extremes[0] == null )
							P[0] = Extremes[1].GetLeft();
						else
							P[0] = Extremes[0].GetRight();
					}
					
					L = new Interval( P,P[0].GetRegion(),P[0].GetFlag(),inter++ );
					R = new Interval( P,P[0].GetRegion(),P[0].GetFlag(),inter++ );

					P[0].SetChild( L,R );
					R.AddLeft(pt);

					if( Extremes[0] != null ) Extremes[0].SetRight( L );
					if( Extremes[1] != null ) Extremes[1].SetLeft( R );
				}
				else
				{
					for( i = 0; i < SC.Count; i++ )
					{
						Segment S = (Segment)SC[i];
						S.GetRight().AddLeft( pt );
					}
					L = ((Segment)SC[0]).GetLeft();
					R = ((Segment)SC[SC.Count-1]).GetRight();
					
					if( SO.Count > 0 )L.AddRight(pt);
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
					bool newRegion = false;
				
					if( ( Extremes[0] == null ) && ( Extremes[1] == null ) && ( flag == 0 ) )
					{						
						reg = new Region();
						Regions.Add(reg);
						newRegion = true;	
					}
					else 
					{
						if( ( L.GetRegion() == null ) || ( R.GetRegion() == null ) )
						{
							reg = null;
							L.SetRegion( null );
							R.SetRegion( null );
						}
						else reg = L.GetRegion();
					}
							
					Interval I = new Interval(P,reg,L.GetFlag(),inter++ );
					if( newRegion ) reg.SetInterval(I);

					L.SetChild(null,I);
					R.SetChild(I,null);
					
					if( Extremes[0] != null ) Extremes[0].SetRight( I );
					if( Extremes[1] != null ) Extremes[1].SetLeft( I );	
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
							( ( !((Segment)SO[i-1]).isReversal() ) &&
							( !((Segment)SO[ i ]).isReversal() ) &&
							( !((Segment)SO[i-1]).isCoincident((Segment)SO[i]) ) ) )
						{
							reg = new Region();
							Regions.Add(reg);							
						}
						
						I = new Interval(reg,flag,inter++);
						if( reg != null ) reg.SetInterval(I);

						I.AddLeft(pt);
						reg = null;

						( (Segment)SO[i-1]).SetRight( I );
						( (Segment)SO[ i ]).SetLeft(  I );                        
					}

					( (Segment)SO[ 0 ]).SetLeft( L );
					if( ( (Segment)SO[ 0 ]).isReversal() )
					{
						L.SetRegion( null );						
					}
					
					( (Segment)SO[ SO.Count-1 ]).SetRight( R ); 
					if( ( (Segment)SO[ SO.Count-1 ]).isReversal() )
					{
						R.SetRegion( null );						
					}
				}
				#endregion
			}

			int init = 0;
			ArrayList Faces = new ArrayList();

			if( System.Math.Sign( -sgDist - 1 ) == 0 )
			{
				Region reg = (Region)Regions[0];
				reg.Invert();
				init = 1;

				reg = (Region)Regions[Regions.Count-1];
				ArrayList Face = reg.GetVertex( true );
				Faces.Add(Face);
			}
			
			

			for( i = init; i<Regions.Count; i++ )
			{
				Region reg = (Region)Regions[i];
				if( reg.flag )
				{
					ArrayList Face = reg.GetVertex(false);
					Faces.Add(Face);
				}
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
			// 
			// TODO: Add constructor logic here
			//
		}
		public Simple_Polygonal(ArrayList list)
		{
			for( int i=0; i<list.Count;i++)
			{
				Add_Vertex( new R_Point( (L_Point)list[i] ) );
			}
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
				R_Point Point_Aux;
				
				R_Point[] Point = new R_Point[2];
				int[] LTO = null;

				Point[0] = this[1].Clone();
				Point[1] = this[CantOfVertex()-2].Clone();

				//	Traslado el eje de coordenadas a los puntos extremos correspondientes.

				Point[0].x -= this[0].x;
				Point[0].y -= this[0].y;

				Point[1].x -= this[CantOfVertex()-1].x;
				Point[1].y -= this[CantOfVertex()-1].y;
								
				double aux;
				System.Int64 Ent,Num;

				System.Int64 a1, b1, a2, b2, A, B, Den;				
				BigRational d1, d2, x, y;
				R_Point V;

				for( i = 0; i<2; i++)
				{
					int sg = -Math.Sign((int)(Math.Pow((double)-1,(double)i)));
					a1  = sg*( BigInteger.BigIntegerToLong( Point[i].x.entero ) );
					b1  = sg*( BigInteger.BigIntegerToLong( Point[i].y.entero ) ); 
					A  = (a1*a1 + b1*b1);

					/*Calculo de la distancia aproximada del segmento*/
					aux = Math.Abs(dist)*Math.Sqrt(A);
					Ent = (System.Int64)Math.Floor( aux );
					aux -= Ent; 
					Num = (System.Int64)Math.Ceiling( aux *Decimals_Den );

					d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
					
					x =  b1*d1/A;
					y = -a1*d1/A;

					Point[i] = new R_Point(x,y);
				}

				//Reposiciono el eje de cordenada para ambos resultados
				
					
				Point[0].x += this[0].x;
				Point[0].y += this[0].y;

				Point[1].x += this[CantOfVertex()-1].x;
				Point[1].y += this[CantOfVertex()-1].y;		

				//Actualizo el link a la original y la lista de los puntos racionales
				LTO = new int[2];
				
				LTO[0] = LTO[1] = this.CantOfVertex()-1;
				
				R.Add_Vertex(Point[0]);
				R.SetRelationIndex( 0, 0, 0 );
				R.SetRelationIndex( 0, 0, 1 );

				Point_Aux = Point[1];

				for( i=0; i < CantOfVertex()-2; i++)
				{
					Point = new R_Point[2];
					/* 
					 *	Traslado el eje de coordenadas al punto al cual se le quiere hallar 
					 * el correspondiente en la paralela 
					 */
					for(System.Byte j=0; j<3; j+=2)
					{
						Point[j/2] = this[(i+j)%CantOfVertex()].Clone();
						Point[j/2].x -= this[i+1].x;
						Point[j/2].y -= this[i+1].y;
					}
				
					// Calculo las distancias aproximada a cada uno de los dos segmentos involucrados (d1 y d2)
					a1  = -( BigInteger.BigIntegerToLong( Point[0].x.entero ) );
					b1  = -( BigInteger.BigIntegerToLong( Point[0].y.entero ) );
					a2  =    BigInteger.BigIntegerToLong( Point[1].x.entero );
					b2  =    BigInteger.BigIntegerToLong( Point[1].y.entero );
					A   = (a1*a1 + b1*b1);
					B   = (a2*a2 + b2*b2);
					Den = ( a2*b1 - b2*a1 );

					aux = Math.Abs(dist)*Math.Sqrt(A);
					Ent = (System.Int64)Math.Floor( aux );
					aux -= Ent; 
					Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

					d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/

					aux = Math.Abs(dist)*Math.Sqrt(B);
					Ent = (System.Int64)Math.Floor( aux );
					aux -= Ent; 
					Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

					d2 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
				

					// x y y son las coordenadas del punto racional
					x = ( d2*a1 - d1*a2 ) / Den;
					x += this[i+1].x;

					y = ( d2*b1 - d1*b2 ) / Den;
					y += this[i+1].y;

					//Aproximo el punto a entero para adicionarlo a la poligonal paralela
					V = new R_Point(x,y);

					if( ( V.x == R[R.CantOfVertex()-1].x ) &&
						( V.y == R[R.CantOfVertex()-1].y ) ) 
					{
						/*
						 * Si el punto obtenido es igual al anterior
						 * actualizo el link a la original de forma que me salte
						 * el vertice que fue eliminado.
						 */
						R.SetRelationIndex(i+1,R.CantOfVertex()-1,0);
						R.SetRelationIndex(i+1,R.CantOfVertex()-1,1);
						continue;
					}
				
					/*
					 * Adicion el vertice a la poligonal paralela,
					 * actualizo la lista de vertices racionales y
					 * actualizo el link a la original.
					 */ 
					R.Add_Vertex(V);
					
					R.SetRelationIndex(i+1,R.CantOfVertex()-1,0);
					R.SetRelationIndex(i+1,R.CantOfVertex()-1,1);
					R.SetRelationIndex(i+1,R.CantOfVertex()-2,1);
				}

				/*
				 * Como la poligonal es avierta adiciono el ultimo vertice 
				 * que lo tenia en Point_Aux a la Poligonal y arreglo la lista 
				 * de los verices racionales. Actualizo el link a la original.
				 */ 
				R.Add_Vertex(Point_Aux);
				
				R.SetRelationIndex(LTO[0],R.CantOfVertex()-1,0);
				R.SetRelationIndex(LTO[1],R.CantOfVertex()-1,1);
				R.SetRelationIndex(R.GetRelationIndex(R.CantOfVertex()-1,0),R.CantOfVertex()-2,1);

				if( ( R.CantOfVertex() == 2 )	&&
					( ( R[0].x == R[1].x ) ||
					( R[0].y == R[1].y ) ) )
				{
					R.Remove_Vertex(1);
					return true;
				}

				/*
			 * Cuts ---- Lista de intersecciones de un segmento con todos los 
			 *			de la poligonal
			 * Valido -- Estado en que estamos en el analisis
			 */
				R_Point[] P=new R_Point[4]; 

				P[0] = R[ 0 ];
				P[1] = R[ 1 ];
				P[2] = this[R.GetRelationIndex(0,0)];
				P[3] = this[R.GetRelationIndex(0,1)];
				
				if( ( ( P[1].x - P[0].x ).sign ^ ( P[3].x - P[2].x ).sign ) ||
					( ( P[1].y - P[0].y ).sign ^ ( P[3].y - P[2].y ).sign ) )
				{	
					//El segmento paralelo inicial es contrario.
					R.SetInvert( 0 );
				}
				
				i=1;
				
				while( i < R.CantOfVertex()-1 )
				{
					/*
					 * Good_Seg me dira si el segmento que se esta analizando 
					 * es un segmento correcto o no.
					 * En P tendre en sus dos primeros valores (0 y 1) los extremos
					 * del segmento que se esta analizando en la poligonal paralela y
					 * en las dos posiciones restantes (2 y 3) los extremos del mismo 
					 * segmento pero en la poligonal original.
					 */ 
					bool Good_Seg = false;
					P[0] = R[  i  ];
					P[1] = R[ i+1 ];
					P[2] = this[R.GetRelationIndex(i,0)];
					P[3] = this[R.GetRelationIndex(i,1)];

					R_Point Pt = null;
						
					Pt = R[ ( i + R.CantOfVertex() - 1 ) % R.CantOfVertex() ];

					if( R_Point.SD(Pt,P[0],P[1]) == 0 )
					{
						/*
						 * Si son coincidentes se pierde la arista. En caso de que se quiera 
						 * dar en la solucion debe cambiarse esto.
						 */ 
						
						/* Elimino el vetice i y actualizo consecuentemente 
						 * el link a la original. Ojo con el link
						 */ 
						
						if( !( ( ( P[1].x - P[0].x ).sign ^ ( P[1].x - Pt.x ).sign ) ||
							   ( ( P[1].y - P[0].y ).sign ^ ( P[1].y - Pt.y ).sign ) ) )
						{								
							R.CopyRelationIndex(i,i-1);
						}
						R.Remove_Vertex(i);

						if( ( i < R.CantOfVertex() ) &&
							( P[1].x == Pt.x ) &&
							( P[1].y == Pt.y ) )
						{
							/*
							 * Si son iguales elimino el vetice i y actualizo consecuentemente 
							 * el link a la original. Ojo con el link
							 */ 
							R.Remove_Vertex(i);						
						}
						
						P[0] = R[ i-1 ];
						P[1] = R[  i  ];
						P[2] = this[R.GetRelationIndex(i-1,0)];
						P[3] = this[R.GetRelationIndex(i-1,1)];

						if( ( ( P[1].x - P[0].x ).sign ^ ( P[3].x - P[2].x ).sign ) ||
							( ( P[1].y - P[0].y ).sign ^ ( P[3].y - P[2].y ).sign ) )
							R.SetInvert(i-1);
						else
							R.ResetInvert(i-1);
						continue;
					}

					while( !Good_Seg )
					{					
						/*
						 * En este ciclo eliminare los segmentos que son contrarios en
						 * la poligonal paralela que no forman un ciclo y aquellos 
						 * segmentos consecutivos que son coincidentes.
						 */ 
						#region segmentos contrarios
						if( !( ( ( P[1].x - P[0].x ).sign ^ ( P[3].x - P[2].x ).sign ) ||
							( ( P[1].y - P[0].y ).sign ^ ( P[3].y - P[2].y ).sign ) ) )
						{	
							//Si el segmento paralelo no es contrario continuo.
							Good_Seg = true;
						}
						else
						{
							/*
							 * El segmento paralelo es contrario al segmento original,
							 */ 
							R.SetInvert(i);

							bool back = true;
							bool next = true;

							R_Point[] PAux = new R_Point[4];

							PAux[0] = R[i - 1];
							PAux[1] = R[ i ];
							PAux[2] = this[R.GetRelationIndex( i-1, 0 )];
							PAux[3] = this[R.GetRelationIndex( i-1, 1 )];

							back = !( ( ( PAux[1].x - PAux[0].x ).sign ^ ( PAux[3].x - PAux[2].x ).sign )  ||
								      ( ( PAux[1].y - PAux[0].y ).sign ^ ( PAux[3].y - PAux[2].y ).sign ) );
								
							if( i != R.CantOfVertex()-2)
							{
								PAux[0] = R[(i+1)%R.CantOfVertex()];
								PAux[1] = R[(i+2)%R.CantOfVertex()];
								PAux[2] = this[R.GetRelationIndex( (i+1)%R.CantOfVertex(), 0 )];
								PAux[3] = this[R.GetRelationIndex( (i+1)%R.CantOfVertex(), 1 )];

								next = !( ( ( PAux[1].x - PAux[0].x ).sign ^ ( PAux[3].x - PAux[2].x ).sign ) ||
									( ( PAux[1].y - PAux[0].y ).sign ^ ( PAux[3].y - PAux[2].y ).sign ) );
							}
							int Last = -1, Next = -1;
						
							/*
							 * Last es el indice del vertice anterior al inicio del 
							 * segmento que se esta analizando y
							 * Next es el indice del vertice siguiente al final del
							 * segmento que se esta analizando.
							 */ 
							
							Last = ( i - 1 ); 
							Next = ( i + 2 );

							if( back && next )
							{
								if( ( Next >= R.CantOfVertex() ) ||
									( !( ( R_Point.SD( this[R.GetRelationIndex( Last, 0 )],P[2],P[3])*
									R_Point.SD( P[2],P[3],this[R.GetRelationIndex( Next, 0 )]) == -1 ) ) ) )
								{
									/*
									 * En este caso el segmento contrario forma un ciclo
									 * por lo que continuo el proceso.
									 */ 
									Good_Seg = true;
								}
								else
								{
									/*
									 * El segmento contrario no forma un ciclo por lo que 
									 * tengo que eliminarlo.
									 * Para hacerlo calculo el punto de interseccion entre 
									 * los segmentos anterior y posterior al segmento 
									 * contrario y sustituirlo por los dos extremos del
									 * segmento analizado.
									 */ 
									R_Point[] Pt_Aux = new R_Point[4];
									Pt_Aux[0] =  this[R.GetRelationIndex( Last, 0 )].Clone();
									Pt_Aux[1] =  P[2];
									Pt_Aux[2] =  P[3];
									Pt_Aux[3] =  this[R.GetRelationIndex( ( i + 1 ) % R.CantOfVertex(), 1 )];

									a1  = BigInteger.BigIntegerToLong( Pt_Aux[1].x.entero - Pt_Aux[0].x.entero );
									b1  = BigInteger.BigIntegerToLong( Pt_Aux[1].y.entero - Pt_Aux[0].y.entero );
									a2  = BigInteger.BigIntegerToLong( Pt_Aux[3].x.entero - Pt_Aux[2].x.entero );
									b2  = BigInteger.BigIntegerToLong( Pt_Aux[3].y.entero - Pt_Aux[2].y.entero );
									A   = (a1*a1 + b1*b1);
									B   = (a2*a2 + b2*b2);
									Den = ( b2*a1 - a2*b1 );

									/*
									 * Calculo las distancias aproximadas de los 
									 * segmentos a intersectar.
									 */ 
									aux = Math.Abs(dist)*Math.Sqrt(A);
									Ent = (System.Int64)Math.Floor( aux );
									aux -= Ent; 
									Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

									d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/

									aux = Math.Abs(dist)*Math.Sqrt(B);
									Ent = (System.Int64)Math.Floor( aux );
									aux -= Ent; 
									Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

									d2 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
				
									Pt_Aux[0].x -= Pt_Aux[2].x;
									Pt_Aux[0].y -= Pt_Aux[2].y;

									/*
									 * CAlculo el punto de interseccion cuyas coordenadas
									 * son x y y.
									 */ 

									x  = ( a2*d1 - a1*d2 );
									x +=  Pt_Aux[0].y*a1*a2-Pt_Aux[0].x*b1*a2 ; 
									x /= Den;
									x += Pt_Aux[2].x;

									y = -( d2*b1 - d1*b2 ); 
									y += Pt_Aux[0].y*a1*b2 -  Pt_Aux[0].x*b1*b2;
									y /= Den;
									y += Pt_Aux[2].y;

									P[1] = new R_Point(x,y);

									Pt_Aux[0].x += Pt_Aux[2].x;
									Pt_Aux[0].y += Pt_Aux[2].y;

									bool Same = false;
								
									/*
									 * Si el punto calculado es igual al punto siguiente
									 * al extremo final del segmento analizado quito los 
									 * dos extremos del segmento. De lo contrario cambio
									 * el extremo inicial por el punto calculado y quito
									 * solamente el extremo final.
									 */

									Same = ( (this[Next].x == P[1].x) && 
										(this[Next].y == P[1].y) );

									if( Same ) 
										R.Remove_Vertex( i );
									else
										R.Replace_Vertex(P[1],i+1);
								
									R.Remove_Vertex( i );									

									Next = (i + 1);
									if( i >=  R.CantOfVertex()-1 ) 
									{
										Good_Seg = true;
										continue;
									}

									P[0] = R[ i  ];
									P[1] = R[Next];
									P[2] = this[R.GetRelationIndex( i, 0 )];
									P[3] = this[R.GetRelationIndex( i, 1 )];								
								}
							}else Good_Seg = true;
						}
						#endregion
					}
					
					i++;
				}
			}
			//Tratamiento de Picos Invertidos

		/*	bool first,second,last,next;
			first = second = last = next = false;

			for( i=0; i<R.CantOfVertex()-2; i++)
			{
				L_Point pt = null;

                if( i!=0 ) last = R.isInvert(i-1);
				first = R.isInvert(i);
				second = R.isInvert(i+1);
				next = R.isInvert(i+2);

				if( !last && first && second && !next )
				{
					pt = new L_Point();
					pt.flag = i;
					pt.x = -1;
					pt.y = -1;
					list.Add(pt);
				}
			}*/
			//Esto es para interrumpir los picos cuando cortan a la original
			list.Clear();
			for(i=0; i<R.CantOfVertex()-2; i++)
			{
				R_Point S1 = R[ i ];
				R_Point S2 = R[i+1];
				R_Point S3 = R[i+2];

				R_Point SB1,SE1;
				R_Point SB2,SE2;

				if( R.isInvert(i) || R.isInvert(i+1) ) continue;
				if( R_Point.SD(S1,S2,S3) == Math.Sign(dist) ) continue;

				SB1 = Segment.Proyect(this[R.GetRelationIndex( i, 1 )],S1,S2);
				SE1 = S2;

				SB2 = S2;
				SE2 = Segment.Proyect(this[R.GetRelationIndex( (i+1)%R.CantOfVertex(), 0 )],S2,S3);

				R_Point P1,P2;				

				for(int k=0;k<R.CantOfVertex();k++)
				{
					P1 = this[R.GetRelationIndex( k, 0 )];					
					P2 = this[R.GetRelationIndex( k, 1 )];		
			
					if( Segment.Intersect(SB1,SE1,P1,P2) )
					{
						SE1 = Segment.Intersection(SB1,SE1,P1,P2);
					}

					if( Segment.Intersect(SB2,SE2,P1,P2) )
					{
						SB2 = Segment.Intersection(SB2,SE2,P1,P2);
					}
				}

				if( (SE1 != S2) && (SB2 != S2) )
				{
					list.Add( i+1 );
					SE1.flag = true;
					list.Add( SE1 );
					SB2.flag = true;
					list.Add( SB2 );
				}
			}
			for(i=list.Count-3;i>=0;i-=3)
			{
				R.Replace_Vertex((R_Point)list[i+2],(int)list[i]);
				R.Insert_Vertex((R_Point)list[i+1],(int)list[i]);
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

			int Ind1 = GetMostLeftDown();
			int Ind2 = Result.GetMostLeftDown();

			R_Point P1 = this[Ind1];
			R_Point P2 = Result[Ind2];

			ArrayList Aux = new ArrayList();

			if( ( P1.x < P2.x ) ||
				( ( P1.x == P2.x ) &&
				( P1.y < P2.y ) ) )
			{
				for( i = 0; i < List.Count; i++ )
				{
					this[i].flag = true;
					Aux.Add( this[i] );
				}
				for( i = Result.CantOfVertex()-1; i >= 0 ; i-- )
				{
					Aux.Add( Result[i] );
				}
			}
			else
			{
				for( i = 0; i < Result.CantOfVertex(); i++ )
				{
					Aux.Add( Result[i] );
				}
				
				for( i = List.Count-1; i >= 0 ; i-- )
				{
					this[i].flag = true;
					Aux.Add( this[i] );
				}
			}
						
			Polygon P = new Polygon(Aux );
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
					Pt = (R_Point)((ArrayList)aux[i])[0];
				
					if( !Pt.flag )
					{
						j = n;
						Pt = (R_Point)((ArrayList)aux[i])[j-1];
						while( ( j > 0 ) &&  ( !Pt.flag ))
						{
							j--;
							if( j > 0 ) Pt = (R_Point)((ArrayList)aux[i])[j-1];
						}
						if( j != n ) init = j;
					}
				
					j = init;
					Pt = (R_Point)((ArrayList)aux[i])[j];

					int cant = 0;
					do
					{			
						Temp = new ArrayList(); 

						while( ( !Pt.flag ) && ( cant != n ) )
						{
							Temp.Add( Pt );
							j = ( j + 1 )%n;
							Pt = (R_Point)((ArrayList)aux[i])[j];
							cant++;
						}
						while( ( Pt.flag ) && ( cant != n ) )
						{
							j = ( j + 1 )%n;						
							Pt = (R_Point)((ArrayList)aux[i])[j];
							cant++;
						}

						Faces.Add( Temp );
					}
					while( j != init );
				}
			}

			return Faces;

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
				R_Point[] Point = new R_Point[2];

				Point[0] = this[CantOfVertex()-1].Clone();
				Point[1] = this[1].Clone();

				/* 
				 *	Traslado el eje de coordenadas al punto al cual se le quiere hallar 
				 *  el correspondiente en la paralela 
				 */

				for(System.Byte j=0; j<2; j++)
				{			
					Point[j].x -= this[0].x;
					Point[j].y -= this[0].y;
				}
				
				Int64 a1  = -( BigInteger.BigIntegerToLong( Point[0].x.entero ) );			
				Int64 b1  = -( BigInteger.BigIntegerToLong( Point[0].y.entero ) );
				Int64 a2  =    BigInteger.BigIntegerToLong( Point[1].x.entero );
				Int64 b2  =    BigInteger.BigIntegerToLong( Point[1].y.entero );
				Int64 A   = (a1*a1 + b1*b1);
				Int64 B   = (a2*a2 + b2*b2);
				Int64 Den = ( a2*b1 - b2*a1 );

				/*Calculo de la distancia aproximada del segmento*/
				double aux = Math.Abs(dist)*Math.Sqrt(A);
				System.Int64 Ent = (System.Int64)Math.Floor( aux );
				aux -= Ent; 

				System.Int64 Num = (System.Int64)Math.Ceiling( aux *Decimals_Den );

				BigNumbers.BigRational d1 = new BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/

				aux = Math.Abs(dist)*Math.Sqrt(B);
				Ent = (System.Int64)Math.Floor( aux );
				aux -= Ent; 
				Num = (System.Int64)Math.Ceiling( aux *Decimals_Den );

				BigNumbers.BigRational d2 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/

				//x y y son las coordenadas racionales del punto calculado

				BigNumbers.BigRational x = ( d2*a1 - d1*a2 ) / Den;
				x += this[0].x;

				BigNumbers.BigRational y = ( d2*b1 - d1*b2 ) / Den;
				y += this[0].y;

				// Lo aproximo a entero paratener la poligonal con vertices enteros
				R_Point V = new R_Point(x,y);

				R.Add_Vertex(V);	
			
				//Actualizo el link a la original
				R.SetRelationIndex( 0, 0, 0 );
				R.SetRelationIndex( 0, 0, 1 );
				
				for( i=0; i < CantOfVertex()-1; i++)
				{
					Point = new R_Point[2];
					/* 
					 *	Traslado el eje de coordenadas al punto al cual se le quiere hallar 
					 * el correspondiente en la paralela 
					 */
					for(System.Byte j=0; j<3; j+=2)
					{
						Point[j/2] = this[(i+j)%CantOfVertex()].Clone();
						Point[j/2].x -= this[i+1].x;
						Point[j/2].y -= this[i+1].y;
					}
				
					// Calculo las distancias aproximada a cada uno de los dos segmentos involucrados (d1 y d2)
					a1  = -( BigInteger.BigIntegerToLong( Point[0].x.entero ) );
					b1  = -( BigInteger.BigIntegerToLong( Point[0].y.entero ) );
					a2  =    BigInteger.BigIntegerToLong( Point[1].x.entero );
					b2  =    BigInteger.BigIntegerToLong( Point[1].y.entero );
					A   = (a1*a1 + b1*b1);
					B   = (a2*a2 + b2*b2);
					Den = ( a2*b1 - b2*a1 );

					aux = Math.Abs(dist)*Math.Sqrt(A);
					Ent = (System.Int64)Math.Floor( aux );
					aux -= Ent; 
					Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

					d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/

					aux = Math.Abs(dist)*Math.Sqrt(B);
					Ent = (System.Int64)Math.Floor( aux );
					aux -= Ent; 
					Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

					d2 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
				

					// x y y son las coordenadas del punto racional
					x = ( d2*a1 - d1*a2 ) / Den;
					x += this[i+1].x;

					y = ( d2*b1 - d1*b2 ) / Den;
					y += this[i+1].y;

					//Aproximo el punto a entero para adicionarlo a la poligonal paralela
					V = new R_Point(x,y);

					if( ( V.x == R[R.CantOfVertex()-1].x ) &&
						( V.y == R[R.CantOfVertex()-1].y ) ) 
					{
						/*
						 * Si el punto obtenido es igual al anterior
						 * actualizo el link a la original de forma que me salte
						 * el vertice que fue eliminado.
						 */
						R.SetRelationIndex( i+1, R.CantOfVertex()-1, 0 );
						R.SetRelationIndex( i+1, R.CantOfVertex()-1, 1 );
						continue;
					}
				
					/*
					 * Adicion el vertice a la poligonal paralela,
					 * actualizo la lista de vertices racionales y
					 * actualizo el link a la original.
					 */ 
					R.Add_Vertex(V);
					
					R.SetRelationIndex( i+1, R.CantOfVertex()-1, 0 );
					R.SetRelationIndex( i+1, R.CantOfVertex()-1, 1 );
					R.SetRelationIndex( i+1, R.CantOfVertex()-2, 1 );
				}

				if( R.CantOfVertex() == 1 )	return true;

				if(R.GetRelationIndex( R.CantOfVertex()-1, 0) == R.GetRelationIndex( R.CantOfVertex()-1, 1))
					R.SetRelationIndex( 0, R.CantOfVertex()-1, 1);

				if( ( R[0].x == R[R.CantOfVertex()-1].x ) &&
					( R[0].y == R[R.CantOfVertex()-1].y ) ) 
				{// El ultimo es igual al primero por lo que lo elimino 
					R.Remove_Vertex(R.CantOfVertex()-1);
				}

				if( R.CantOfVertex() == 1 )	return true;

				/*
				 * Cuts ---- Lista de intersecciones de un segmento con todos los 
				 *			de la poligonal
				 * Valido -- Estado en que estamos en el analisis
				 */

				i = 0;
				R_Point[] P=new R_Point[4]; 
				
				while( i < R.CantOfVertex() )
				{
					/*
					 * Good_Seg me dira si el segmento que se esta analizando 
					 * es un segmento correcto o no.
					 * En P tendre en sus dos primeros valores (0 y 1) los extremos
					 * del segmento que se esta analizando en la poligonal paralela y
					 * en las dos posiciones restantes (2 y 3) los extremos del mismo 
					 * segmento pero en la poligonal original.
					 */ 
					bool Good_Seg = false;
					P[0] = R[ i ];
					P[1] = R[(i+1)%R.CantOfVertex()];
					P[2] = this[R.GetRelationIndex( i, 0 )];
					P[3] = this[R.GetRelationIndex( i, 1 )];

					R_Point Pt = null;
						
					Pt = R[ ( i + R.CantOfVertex() - 1 ) % R.CantOfVertex() ];

					if( R_Point.SD(Pt,P[0],P[1]) == 0 )
					{
						/*
						 * Si son coincidentes se pierde la arista. En caso de que se quiera 
						 * dar en la solucion debe cambiarse esto.
						 */ 
						
						/* Elimino el vetice i y actualizo consecuentemente 
						 * el link a la original. Ojo con el link
						 */ 
						
						
						if( !( ( ( P[1].x - P[0].x ).sign ^ ( P[1].x - Pt.x ).sign ) ||
							   ( ( P[1].y - P[0].y ).sign ^ ( P[1].y - Pt.y ).sign ) ) )
						{	
							if( i==0 )
							{
								R.Replace_Vertex( R[R.CantOfVertex()-1],0 );
								R.Remove_Vertex(R.CantOfVertex()-1);
							}
							else
							{
								R.CopyRelationIndex( i, i-1 );
								R.Remove_Vertex(i);
							}							
						}					
						else  R.Remove_Vertex(i);
					
						if( ( i < R.CantOfVertex() ) &&
							( P[1].x == Pt.x ) &&
							( P[1].y == Pt.y ) )
						{
							/*
							 * Si son iguales elimino el vetice i y actualizo consecuentemente 
							 * el link a la original. Ojo con el link
							 */ 
							R.Remove_Vertex(i);					
						}
						continue;
					}
					
					while( !Good_Seg )
					{
						/*
						 * En este ciclo eliminare los segmentos que son contrarios en
						 * la poligonal paralela que no forman un ciclo y aquellos 
						 * segmentos consecutivos que son coincidentes.
						 */ 
						#region segmentos contrarios
						
						if( !( ( ( P[1].x - P[0].x ).sign ^ ( P[3].x - P[2].x ).sign ) ||
							( ( P[1].y - P[0].y ).sign ^ ( P[3].y - P[2].y ).sign ) ) )
						{	
							//Si el segmento paralelo no es contrario continuo.
							Good_Seg = true;
						}
						else
						{
							R.SetInvert(i);
							/*
							 * El segmento paralelo es contrario al segmento original,
							 */ 

							int Last = -1, Next = -1;
						
							/*
							 * Last es el indice del vertice anterior al inicio del 
							 * segmento que se esta analizando y
							 * Next es el indice del vertice siguiente al final del
							 * segmento que se esta analizando.
							 */ 
							bool back = false;
							bool next = false;

							R_Point[] PAux = new R_Point[4];

							PAux[0] = R[(i + R.CantOfVertex() - 1)%R.CantOfVertex()];
							PAux[1] = R[ i ];
							PAux[2] = this[R.GetRelationIndex((i + R.CantOfVertex() - 1)%R.CantOfVertex(), 0)];
							PAux[3] = this[R.GetRelationIndex((i + R.CantOfVertex() - 1)%R.CantOfVertex(), 1)];

							back = !( ( ( PAux[1].x - PAux[0].x ).sign ^ ( PAux[3].x - PAux[2].x ).sign )  ||
								( ( PAux[1].y - PAux[0].y ).sign ^ ( PAux[3].y - PAux[2].y ).sign ) );
								
							PAux[0] = R[(i+1)%R.CantOfVertex()];
							PAux[1] = R[(i+2)%R.CantOfVertex()];
							PAux[2] = this[R.GetRelationIndex( (i+1)%R.CantOfVertex(), 0 )];
							PAux[3] = this[R.GetRelationIndex( (i+1)%R.CantOfVertex(), 1 )];

							next = !( ( ( PAux[1].x - PAux[0].x ).sign ^ ( PAux[3].x - PAux[2].x ).sign ) ||
								( ( PAux[1].y - PAux[0].y ).sign ^ ( PAux[3].y - PAux[2].y ).sign ) );
							
							if(back && next)
							{
								Last = ( i + R.CantOfVertex() - 1 ) % R.CantOfVertex(); 
								Next = ( i + 2 ) % R.CantOfVertex();

								if( !( ( R_Point.SD( this[R.GetRelationIndex( Last, 0 )],P[2],P[3])*
									R_Point.SD( P[2],P[3],this[R.GetRelationIndex( Next, 0 )]) == -1 ) ) )
								{
									/*
									 * En este caso el segmento contrario forma un ciclo
									 * por lo que continuo el proceso.
									 */ 
									Good_Seg = true;
								}
								else
								{
									/*
										 * El segmento contrario no forma un ciclo por lo que 
										 * tengo que eliminarlo.
										 * Para hacerlo calculo el punto de interseccion entre 
										 * los segmentos anterior y posterior al segmento 
										 * contrario y sustituirlo por los dos extremos del
										 * segmento analizado.
										 */ 
									R_Point[] Pt_Aux = new R_Point[4];
									Pt_Aux[0] =  this[R.GetRelationIndex( Last, 0 )].Clone();
									Pt_Aux[1] =  this[R.GetRelationIndex( Last, 1 )];
									Pt_Aux[2] =  this[R.GetRelationIndex( ( i + 1 ) % R.CantOfVertex(), 0 )];
									Pt_Aux[3] =  this[R.GetRelationIndex( ( i + 1 ) % R.CantOfVertex(), 1 )];

									a1  = BigInteger.BigIntegerToLong( Pt_Aux[1].x.entero - Pt_Aux[0].x.entero );
									b1  = BigInteger.BigIntegerToLong( Pt_Aux[1].y.entero - Pt_Aux[0].y.entero );
									a2  = BigInteger.BigIntegerToLong( Pt_Aux[3].x.entero - Pt_Aux[2].x.entero );
									b2  = BigInteger.BigIntegerToLong( Pt_Aux[3].y.entero - Pt_Aux[2].y.entero );
									A   = (a1*a1 + b1*b1);
									B   = (a2*a2 + b2*b2);
									Den = ( b2*a1 - a2*b1 );

									/*
										 * Calculo las distancias aproximadas de los 
										 * segmentos a intersectar.
										 */ 
									aux = Math.Abs(dist)*Math.Sqrt(A);
									Ent = (System.Int64)Math.Floor( aux );
									aux -= Ent; 
									Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

									d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/

									aux = Math.Abs(dist)*Math.Sqrt(B);
									Ent = (System.Int64)Math.Floor( aux );
									aux -= Ent; 
									Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

									d2 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
				
									Pt_Aux[0].x -= Pt_Aux[2].x;
									Pt_Aux[0].y -= Pt_Aux[2].y;

									/*
										 * Calculo el punto de interseccion cuyas coordenadas
										 * son x y y.
										 */ 

									x  = ( a2*d1 - a1*d2 );
									x +=  Pt_Aux[0].y*a1*a2-Pt_Aux[0].x*b1*a2 ; 
									x /= Den;
									x += Pt_Aux[2].x;

									y = -( d2*b1 - d1*b2 ); 
									y += Pt_Aux[0].y*a1*b2 -  Pt_Aux[0].x*b1*b2;
									y /= Den;
									y += Pt_Aux[2].y;

									P[1] = new R_Point(x,y);

									Pt_Aux[0].x += Pt_Aux[2].x;
									Pt_Aux[0].y += Pt_Aux[2].y;

									bool Same = false;
								
									/*
											 * Si el punto calculado es igual al punto siguiente
											 * al extremo final del segmento analizado quito los 
											 * dos extremos del segmento. De lo contrario cambio
											 * el extremo inicial por el punto calculado y quito
											 * solamente el extremo final.
											 */

									Same = ( (R[Next].x == P[1].x) && 
										(R[Next].y == P[1].y) );

									if( Same ) 
										R.Remove_Vertex( i );
									else
										R.Replace_Vertex(P[1],(i+1)%R.CantOfVertex());
								
									R.Remove_Vertex( i );									

									Next = (i + 1)%R.CantOfVertex();
									if( i >=  R.CantOfVertex() ) 
									{
										Good_Seg = true;
										continue;
									}

									P[0] = R[ i  ];
									P[1] = R[Next];
									P[2] = this[R.GetRelationIndex( i, 0 )];
									P[3] = this[R.GetRelationIndex( i, 1 )];		
								}
							}else Good_Seg = true;
						}
						#endregion
					}
					
					i++;
				}				
			}
			//Solucion Picos Invertidos
			
			//Esto es para interrumpir los picos no invertidos cuando cortan a la original
			list.Clear();
			for(i=-1; i<R.CantOfVertex()-1; i++)
			{
				R_Point S1 = R[(i+R.CantOfVertex())%R.CantOfVertex()];


				R_Point S2 = R[(i+1)%R.CantOfVertex()];
				R_Point S3 = R[(i+2)%R.CantOfVertex()];

				R_Point SB1,SE1;
				R_Point SB2,SE2;

				if( R.isInvert((i+R.CantOfVertex())%R.CantOfVertex()) || R.isInvert(i+1) ) continue;
				if( R_Point.SD(S1,S2,S3) == Math.Sign(dist) ) continue;

				SB1 = Segment.Proyect(this[R.GetRelationIndex( (i+R.CantOfVertex())%R.CantOfVertex(),1)],S1,S2);
				SE1 = S2;

				SB2 = S2;
				SE2 = Segment.Proyect(this[R.GetRelationIndex( (i+1)%R.CantOfVertex(), 0)],S2,S3);

				R_Point P1,P2;				

				for(int k=0;k<R.CantOfVertex();k++)
				{
					P1 = this[R.GetRelationIndex( k, 0)];					
					P2 = this[R.GetRelationIndex( k, 1)];		
			
					if( Segment.Intersect(SB1,SE1,P1,P2) )
					{
						SE1 = Segment.Intersection(SB1,SE1,P1,P2);
					}

					if( Segment.Intersect(SB2,SE2,P1,P2) )
					{
						SB2 = Segment.Intersection(SB2,SE2,P1,P2);
					}
				}

				if( (SE1 != S2) & (SB2 != S2) )
				{
					list.Add( i+1 );
					SE1.flag = true;
					list.Add( SE1 );
					SB2.flag = true;
					list.Add( SB2 );
				}
			}
			for(i=list.Count-3;i>=0;i-=3)
			{
                R.Replace_Vertex((R_Point)list[i+2],(int)list[i]);
				R.Insert_Vertex((R_Point)list[i+1],(int)list[i]);
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
