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
		#region constants
		protected long Decimals_Den=100;
		#endregion

		#region fields
			protected ArrayList List;
			protected int Cant;
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
				Cant = 0;
				List = new System.Collections.ArrayList();
			// 
			// TODO: Add constructor logic here
			//
		}
		#endregion

		
		#region editions
			public int CantOfVertex()
			{
				return Cant;
			}			
			public int GetMostLeftDown()
			{
				int Result = 0;
				for(int i = 0; i < List.Count; i++ )
				{
					if( ( ( (R_Point)List[i]).x < ( (R_Point)List[Result] ).x ) ||
						( ( ( (R_Point)List[i]).x == (( R_Point)List[Result] ).x ) &&
						( ( (R_Point)List[i]).y < (( R_Point)List[Result] ).y ) ) )
					{
						Result = i;
					}

				}
				return Result;
			}
			public virtual bool Add_Vertex(R_Point V)
		{
				List.Add( V );
				Cant++;
			return true;
		}
			public virtual bool Insert_Vertex(R_Point V,int Pos)
			{
				Cant++;
				List.Insert( Pos,V );
				return true;
			}

			public virtual bool Replace_Vertex(R_Point V,int Pos)
			{
				Remove_Vertex(Pos);
				Insert_Vertex(V,Pos);
				return true;
			}
			public virtual bool Remove_Vertex(int Pos)
			{
				Cant--;
				List.RemoveAt( Pos );
				return true;
			}
			public virtual bool Remove_ALL( )
			{			
				Cant = 0;
				List.Clear( );
				return true;
			}
			public R_Point Get_Vertex(int Pos)
			{
				return (R_Point)List[Pos];
			}
			
			public ArrayList GetVertexes()
			{
				return List;
			}
			
		#endregion

		#region General
		    public abstract Polygonal[] Fractionate();	
		    protected abstract Polygonal[] Halved();
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
				Add_Vertex( (R_Point)list[i] );
			}
		}
		#endregion

		#region General
		public ArrayList GetValid( int sgDist,ArrayList Reversed )
		{
			int i;
			EventQueue EQ = new EventQueue();

			#region Inicializando EQ
			
			for( i = 0; i < List.Count; i++)
			{
				Segment Seg = new Segment((R_Point)List[i],(R_Point)List[(i+1) % List.Count]);
				if( (bool)Reversed[i] ) Seg.SetReversed();
				
			
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
			Cant = 0;
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

		virtual protected bool Get_Polygonal(int dist, out Polygonal R,out ArrayList Reversed, bool V_Int)
		{
			Reversed = new ArrayList();
			int i;
			int[][] LinkToOriginal = new int[this.CantOfVertex()][];

			R = new Polygonal();

			if( V_Int )
			{
				R_Point Point_Aux;
				
				R_Point[] Point = new R_Point[2];

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
				LinkToOriginal[0] = new int[2];
				LinkToOriginal[0][0] = LinkToOriginal[0][1] = 0;

				LinkToOriginal[LinkToOriginal.Length-1] = new int[2];
				
				LinkToOriginal[LinkToOriginal.Length-1][0] = 
					LinkToOriginal[LinkToOriginal.Length-1][1] = this.CantOfVertex()-1;
				
				R.Add_Vertex(Point[0]);

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
						LinkToOriginal[R.CantOfVertex()-1][0] = 
							LinkToOriginal[R.CantOfVertex()-1][1] = i+1;
						continue;
					}
				
					/*
					 * Adicion el vertice a la poligonal paralela,
					 * actualizo la lista de vertices racionales y
					 * actualizo el link a la original.
					 */ 
					R.Add_Vertex(V);
					LinkToOriginal[R.CantOfVertex()-1] = new int[2];
				
					LinkToOriginal[R.CantOfVertex()-2][1] = 
						LinkToOriginal[R.CantOfVertex()-1][0] = 
						LinkToOriginal[R.CantOfVertex()-1][1] = i+1;
				}

				/*
				 * Como la poligonal es avierta adiciono el ultimo vertice 
				 * que lo tenia en Point_Aux a la Poligonal y arreglo la lista 
				 * de los verices racionales. Actualizo el link a la original.
				 */ 
				R.Add_Vertex(Point_Aux);
				LinkToOriginal[R.CantOfVertex()-1] = LinkToOriginal[LinkToOriginal.Length-1];					
				LinkToOriginal[R.CantOfVertex()-2][1] = LinkToOriginal[R.CantOfVertex()-1][0];

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

				P[0] = R[ 0 ].Clone();
				P[1] = R[ 1 ].Clone();
				P[2] = this[LinkToOriginal[ 0 ][ 0 ]].Clone();
				P[3] = this[LinkToOriginal[ 0 ][ 1 ]].Clone();
				
				if( !( ( ( P[1].x - P[0].x ).sign ^ ( P[3].x - P[2].x ).sign ) ||
					( ( P[1].y - P[0].y ).sign ^ ( P[3].y - P[2].y ).sign ) ) )
				{	
					//El segmento paralelo inicial no es contrario.
					Reversed.Add(false);
				}
				else
				{	
					//El segmento paralelo inicial es contrario.
					Reversed.Add(true);
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
					P[0] = R[  i  ].Clone();
					P[1] = R[ i+1 ].Clone();
					P[2] = this[LinkToOriginal[ i ][ 0 ]].Clone();
					P[3] = this[LinkToOriginal[ i ][ 1 ]].Clone();

					R_Point Pt = null;
						
					Pt = R[ ( i + R.CantOfVertex() - 1 ) % R.CantOfVertex() ].Clone();

					if( R_Point.SD(Pt,P[0],P[1]) == 0 )
					{
						/*
						 * Si son coincidentes se pierde la arista. En caso de que se quiera 
						 * dar en la solucion debe cambiarse esto.
						 */ 
						
						/* Elimino el vetice i y actualizo consecuentemente 
						 * el link a la original. Ojo con el link
						 */ 
						
						R.Remove_Vertex(i);

						if( ( ( P[1].x - P[0].x ).sign ^ ( P[1].x - Pt.x ).sign ) ||
							( ( P[1].y - P[0].y ).sign ^ ( P[1].y - Pt.y ).sign ) )
						{								
							for(int j = i; j < R.CantOfVertex(); j++) 
							{
								LinkToOriginal[j] = LinkToOriginal[j+1];
							}
						}
						else
						{	
							for(int j = i-1; j < R.CantOfVertex(); j++) 
							{
								LinkToOriginal[j] = LinkToOriginal[j+1];
							}
						}
						LinkToOriginal[R.CantOfVertex()] = null;						

						if( ( i < R.CantOfVertex() ) &&
							( P[1].x == Pt.x ) &&
							( P[1].y == Pt.y ) )
						{
							/*
							 * Si son iguales elimino el vetice i y actualizo consecuentemente 
							 * el link a la original. Ojo con el link
							 */ 
							R.Remove_Vertex(i);

							for(int j = i; j < R.CantOfVertex(); j++) 
							{
								LinkToOriginal[j] = LinkToOriginal[j+1];
							}
												
							LinkToOriginal[R.CantOfVertex()] = null;						
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
							Reversed.Add(false);
						}
						else
						{
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
							
							Last = ( i - 1 ); 
							Next = ( i + 2 );

							if( ( Next >= R.CantOfVertex() ) ||
								( !( ( R_Point.SD( this[LinkToOriginal[Last][0]],P[2],P[3])*
								       R_Point.SD( P[2],P[3],this[LinkToOriginal[Next][0]]) == -1 ) ) ) )
							{
								/*
								 * En este caso el segmento contrario forma un ciclo
								 * por lo que continuo el proceso.
								 */ 
								Reversed.Add(true);
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
								Pt_Aux[0] =  this[LinkToOriginal[Last][0]].Clone();
								Pt_Aux[1] =  P[2];
								Pt_Aux[2] =  P[3];
								Pt_Aux[3] =  this[LinkToOriginal[( i + 1 ) % R.CantOfVertex()][1]].Clone();

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

								int Step;

								if( Same ) 
								{
									R.Remove_Vertex( i );
									Step = 2;
								}
								else
								{
									R.Replace_Vertex(P[1],i+1);
									Step = 1;
								}
								R.Remove_Vertex( i );									

								
								for(int j = i; j < R.CantOfVertex(); j++) 
								{
									LinkToOriginal[j] = LinkToOriginal[j+Step];
								}

								LinkToOriginal[LinkToOriginal.Length-1] = null;
								if( Step == 2 ) 
									LinkToOriginal[LinkToOriginal.Length-2] = null;

								Next = (i + 1);
								if( i >=  R.CantOfVertex()-1 ) 
								{
									Good_Seg = true;
									continue;
								}

								P[0] = R[ i  ].Clone();
								P[1] = R[Next].Clone();
								P[2] = this[LinkToOriginal[ i ][ 0 ]].Clone();
								P[3] = this[LinkToOriginal[ i ][ 1 ]].Clone();								
							}
						}
						#endregion
					}
					
					i++;
				}
			}
			return true;
		}
		virtual public ArrayList Parallel(int dist, out Polygonal Result)
		{
			int i = 0;
			while(  ( i < Cant) && 
				( this[i].x.numerador == 0 ) && 
				( this[i].y.numerador == 0 )  )
			{
				i++;
			}
			ArrayList Reversed = null;
			if ( !Get_Polygonal( dist, out Result,out Reversed , (i == Cant) ) )throw new Exception("Lanzo una excepcion");//Ojo
			if( Result.CantOfVertex() == 0 )
			{
				return new ArrayList();
			}

			int Ind1 = GetMostLeftDown();
			int Ind2 = Result.GetMostLeftDown();

			R_Point P1 = (R_Point)List[Ind1];
			R_Point P2 = (R_Point)Result[Ind2];

			ArrayList Aux = new ArrayList();

			if( ( P1.x < P2.x ) ||
				( ( P1.x == P2.x ) &&
				( P1.y < P2.y ) ) )
			{
				for( i = 0; i < List.Count; i++ )
				{
					Aux.Add( List[i] );
					Reversed.Insert(0,false);
				}
				for( i = Result.CantOfVertex()-1; i >= 0 ; i-- )
				{
					Aux.Add( Result[i] );
				}
				Reversed.Add(false);
			}
			else
			{
				for( i = 0; i < Result.CantOfVertex(); i++ )
				{
					Aux.Add( Result[i] );
				}
				
				Reversed.Add(false);

				for( i = List.Count-1; i >= 0 ; i-- )
				{
					Aux.Add( List[i] );
					Reversed.Add(false);
				}
			}
						
			Polygon P = new Polygon(Aux );
			ArrayList Faces = P.GetValid(-1, Reversed);

			return Faces;

		}	

		virtual public Polygonal[] Fractionate()
		{
			Polygonal[] Result;
            Polygonal[] Polygonal_Temp = Halved();
			
			if(Polygonal_Temp.Length == 1) 
				return Polygonal_Temp;

			Polygonal[] Polygonal_Aux1 = Polygonal_Temp[0].Fractionate();
			Polygonal[] Polygonal_Aux2 = Polygonal_Temp[1].Fractionate();

			int N = Polygonal_Aux1.Length + Polygonal_Aux2.Length;
			Result = new Polygonal[ N ];

			for( int i=0; i<N; i++ )
			{
				if( i < Polygonal_Aux1.Length ) 
					Result[i] = Polygonal_Aux1[i];
				else
					Result[i] = Polygonal_Aux2[i - Polygonal_Aux1.Length];
			}
			return Result;

		}

		virtual protected Polygonal[] Halved()
		{
			Polygonal[] Result;
			R_Point[] P= new R_Point[3];

			for( int i=0; i<Cant; i++)
			{				
				P[0] = (R_Point)List[i];
				
				int j = 0;
				do
				{
					if(i==j) j++;
				
					if( (j+1) < End )
					{
						P[1] = (R_Point)List[j];
						P[2] = (R_Point)List[j+1];
					}
					else
					{}
				}while();
			}
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
		override protected bool Get_Polygonal(int dist, out Polygonal R, out ArrayList Reversed, bool V_Int)
		{
			Reversed = new ArrayList();
			int i;
			int[][] LinkToOriginal = new int[this.CantOfVertex()][];

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
				LinkToOriginal[0] = new int[2];
				LinkToOriginal[0][0] = LinkToOriginal[0][1] = 0;	
				
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
						LinkToOriginal[R.CantOfVertex()-1][0] = 
						LinkToOriginal[R.CantOfVertex()-1][1] = i+1;
						continue;
					}
				
					/*
					 * Adicion el vertice a la poligonal paralela,
					 * actualizo la lista de vertices racionales y
					 * actualizo el link a la original.
					 */ 
					R.Add_Vertex(V);
					LinkToOriginal[R.CantOfVertex()-1] = new int[2];
				
					LinkToOriginal[R.CantOfVertex()-2][1] = 
					LinkToOriginal[R.CantOfVertex()-1][0] = 
					LinkToOriginal[R.CantOfVertex()-1][1] = i+1;
				}

				if( R.CantOfVertex() == 1 )	return true;

				if(LinkToOriginal[ R.CantOfVertex()-1][0] == LinkToOriginal[ R.CantOfVertex()-1][1])
					LinkToOriginal[ R.CantOfVertex()-1][1] = 0;

				if( ( R[0].x == R[R.CantOfVertex()-1].x ) &&
					( R[0].y == R[R.CantOfVertex()-1].y ) ) 
				{// El ultimo es igual al primero por lo que lo elimino 
					R.Remove_Vertex(R.CantOfVertex()-1);
					LinkToOriginal[ R.CantOfVertex()] = null;
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
					P[0] = R[ i ].Clone();
					P[1] = R[(i+1)%R.CantOfVertex()].Clone();
					P[2] = this[LinkToOriginal[ i ][ 0 ]].Clone();
					P[3] = this[LinkToOriginal[ i ][ 1 ]].Clone();

					R_Point Pt = null;
						
					Pt = R[ ( i + R.CantOfVertex() - 1 ) % R.CantOfVertex() ].Clone();

					if( R_Point.SD(Pt,P[0],P[1]) == 0 )
					{
						/*
						 * Si son coincidentes se pierde la arista. En caso de que se quiera 
						 * dar en la solucion debe cambiarse esto.
						 */ 
						
						/* Elimino el vetice i y actualizo consecuentemente 
						 * el link a la original. Ojo con el link
						 */ 
						
						R.Remove_Vertex(i);

						if( ( ( P[1].x - P[0].x ).sign ^ ( P[1].x - Pt.x ).sign ) ||
							( ( P[1].y - P[0].y ).sign ^ ( P[1].y - Pt.y ).sign ) )
						{								
							for(int j = i; j < R.CantOfVertex(); j++) 
							{
								LinkToOriginal[j] = LinkToOriginal[j+1];
							}
						}
						else
						{	
							int I = i-1;
							if(I<0)
							{
								I++;
								LinkToOriginal[R.CantOfVertex()] = LinkToOriginal[0];
							}
							
							for(int j = I; j < R.CantOfVertex(); j++) 
							{
								LinkToOriginal[j] = LinkToOriginal[j+1];
							}
						}
						LinkToOriginal[R.CantOfVertex()] = null;						

						if( ( i < R.CantOfVertex() ) &&
							( P[1].x == Pt.x ) &&
							( P[1].y == Pt.y ) )
						{
							/*
							 * Si son iguales elimino el vetice i y actualizo consecuentemente 
							 * el link a la original. Ojo con el link
							 */ 
							R.Remove_Vertex(i);

							for(int j = i; j < R.CantOfVertex(); j++) 
							{
								LinkToOriginal[j] = LinkToOriginal[j+1];
							}
												
							LinkToOriginal[R.CantOfVertex()] = null;						
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
							Reversed.Add(false);
						}
						else
						{
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
							
							Last = ( i + R.CantOfVertex() - 1 ) % R.CantOfVertex(); 
							Next = ( i + 2 ) % R.CantOfVertex();

							if( !( ( R_Point.SD( this[LinkToOriginal[Last][0]],P[2],P[3])*
								R_Point.SD( P[2],P[3],this[LinkToOriginal[Next][0]]) == -1 ) ) )
							{
								/*
								 * En este caso el segmento contrario forma un ciclo
								 * por lo que continuo el proceso.
								 */ 
								Reversed.Add(true);
								Good_Seg = true;
							}
							else
							{
								bool next = false;
								R_Point[] PAux = new R_Point[4];

								PAux[0] = R[(i+1)%R.CantOfVertex()].Clone();
								PAux[1] = R[(i+2)%R.CantOfVertex()].Clone();
								PAux[2] = this[LinkToOriginal[(i+1)%R.CantOfVertex()][ 0 ]].Clone();
								PAux[3] = this[LinkToOriginal[(i+1)%R.CantOfVertex()][ 1 ]].Clone();

								next = ( ( ( PAux[1].x - PAux[0].x ).sign ^ ( PAux[3].x - PAux[2].x ).sign ) ||
										 ( ( PAux[1].y - PAux[0].y ).sign ^ ( PAux[3].y - PAux[2].y ).sign ) );
								
								PAux[0] = R[(i + R.CantOfVertex() - 1)%R.CantOfVertex()].Clone();
								PAux[1] = R[ i ].Clone();
								PAux[2] = this[LinkToOriginal[(i + R.CantOfVertex() - 1)%R.CantOfVertex()][ 0 ]].Clone();
								PAux[3] = this[LinkToOriginal[(i + R.CantOfVertex() - 1)%R.CantOfVertex()][ 1 ]].Clone();

								if( ( ( PAux[1].x - PAux[0].x ).sign ^ ( PAux[3].x - PAux[2].x ).sign ) ||
									( ( PAux[1].y - PAux[0].y ).sign ^ ( PAux[3].y - PAux[2].y ).sign ) ||
									( next ) )
								{	
									int j=0;
									int First, Second;
									bool deleteFirst, deleteSecond;

									#region Pico Invertido
									if(next)
									{
										First  = i;
										Second = (i+1)%R.CantOfVertex();

										PAux[0] = this[LinkToOriginal[(i + R.CantOfVertex() - 1)%R.CantOfVertex()][ 0 ]].Clone();
										PAux[1] = this[LinkToOriginal[(i + R.CantOfVertex() - 1)%R.CantOfVertex()][ 1 ]].Clone();
										PAux[2] = this[LinkToOriginal[(i+2)%R.CantOfVertex()][ 0 ]].Clone();
										PAux[3] = this[LinkToOriginal[(i+2)%R.CantOfVertex()][ 1 ]].Clone();
									}
									else
									{						
										First  = (i + R.CantOfVertex() - 1)%R.CantOfVertex();
										Second = i;

										PAux[0] = this[LinkToOriginal[(i + R.CantOfVertex() - 2)%R.CantOfVertex()][ 0 ]].Clone();
										PAux[1] = this[LinkToOriginal[(i + R.CantOfVertex() - 2)%R.CantOfVertex()][ 1 ]].Clone();
										PAux[2] = this[LinkToOriginal[(i+1)%R.CantOfVertex()][ 0 ]].Clone();
										PAux[3] = this[LinkToOriginal[(i+1)%R.CantOfVertex()][ 1 ]].Clone();										
									}		

									int Aux1 = R_Point.SD(PAux[0],PAux[1],PAux[2]);
									int Aux2 = R_Point.SD(PAux[0],PAux[1],PAux[3]);

									deleteFirst = Aux1 != Aux2;

									if( ( Aux1 == 0 ) && ( Aux2 == 0 ) )
									{
										//Los Segmentos anterior y siguiente al pico estan en una misma recta.
										if( First != R.CantOfVertex()-1 )
										{
											for(j = 0; j<R.CantOfVertex()-3;j++)
											{
												LinkToOriginal[j] = LinkToOriginal[j+2];
											}											
										}
										else
										{
											if( Second != R.CantOfVertex()-1 )
											{
												for(j = 0; j<R.CantOfVertex()-3;j++)
												{
													LinkToOriginal[j] = LinkToOriginal[j+1];
												}											
											}
											else
											{
												for(j = First; j<R.CantOfVertex()-3;j++)
												{
													LinkToOriginal[j] = LinkToOriginal[j+3];
												}											
											}
										}

										LinkToOriginal[R.CantOfVertex()-3] = null;
										LinkToOriginal[R.CantOfVertex()-2] = null;
										LinkToOriginal[R.CantOfVertex()-1] = null;
											
										R.Remove_Vertex(First);
										R.Remove_Vertex(Second);
										R.Remove_Vertex(( Second + 1 ) % R.CantOfVertex());										
									}
									else
									{

										Aux1 = R_Point.SD(PAux[2],PAux[3],PAux[0]);
										Aux2 = R_Point.SD(PAux[2],PAux[3],PAux[1]);

										deleteSecond = Aux1 != Aux2;
										
										if( !deleteFirst && !deleteSecond )
										{
											deleteFirst  = true;
											deleteSecond = true;
										}
										
										R_Point[] Pt_Aux = new R_Point[4];

										if(deleteFirst)
										{
											Pt_Aux[0] =  this[LinkToOriginal[(First + R.CantOfVertex() - 1)%R.CantOfVertex()][0]].Clone();
											Pt_Aux[1] =  this[LinkToOriginal[(First + R.CantOfVertex() - 1)%R.CantOfVertex()][1]].Clone();
										}
										else
										{
											Pt_Aux[0] =  this[LinkToOriginal[First][0]].Clone();
											Pt_Aux[1] =  this[LinkToOriginal[First][1]].Clone();
										}

										if(deleteSecond)
										{
											Pt_Aux[2] =  this[LinkToOriginal[( Second + 1 ) % R.CantOfVertex()][0]].Clone();
											Pt_Aux[3] =  this[LinkToOriginal[( Second + 1 ) % R.CantOfVertex()][1]].Clone();
										}
										else
										{
											Pt_Aux[2] =  this[LinkToOriginal[Second][0]].Clone();
											Pt_Aux[3] =  this[LinkToOriginal[Second][1]].Clone();
										}

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

										if(deleteSecond)
										{
											int init = Second,
												step = 1;
											
											R.Replace_Vertex(P[1],( Second + 1 ) % R.CantOfVertex());
											
											if( ( deleteFirst ) && ( First != R.CantOfVertex()-1 ) )
											{
												init = First;
												step = 2;
											}

											for(j = init; j<R.CantOfVertex()-step;j++)
											{
												LinkToOriginal[j] = LinkToOriginal[j+step];
											}
											LinkToOriginal[j] = null;
											
											if( step == 2 )
												LinkToOriginal[R.CantOfVertex()-1] = null;

											R.Remove_Vertex(Second);											
											if( deleteFirst )
												R.Remove_Vertex(First);
										}
										else
										{
											R.Replace_Vertex(P[1], Second );
											for(j = First; j<R.CantOfVertex()-1;j++)
											{
												LinkToOriginal[j] = LinkToOriginal[j+1];
											}
											LinkToOriginal[j] = null;
											R.Remove_Vertex(First);												
										}	
									}
									#endregion

									if( i >=  R.CantOfVertex() ) 
									{
										Good_Seg = true;
										continue;
									}

									P[0] = R[i].Clone();
									P[1] = R[(i+1)%R.CantOfVertex()].Clone();
									P[2] = this[LinkToOriginal[ i ][ 0 ]].Clone();
									P[3] = this[LinkToOriginal[ i ][ 1 ]].Clone();
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
									Pt_Aux[0] =  this[LinkToOriginal[Last][0]].Clone();
									Pt_Aux[1] =  this[LinkToOriginal[Last][1]].Clone();
									Pt_Aux[2] =  this[LinkToOriginal[( i + 1 ) % R.CantOfVertex()][0]].Clone();
									Pt_Aux[3] =  this[LinkToOriginal[( i + 1 ) % R.CantOfVertex()][1]].Clone();

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

									int Step;

									if( Same ) 
									{
										R.Remove_Vertex( i );
										Step = 2;
									}
									else
									{
										R.Replace_Vertex(P[1],(i+1)%R.CantOfVertex());
										Step = 1;
									}
									R.Remove_Vertex( i );									

								
									for(int j = i; j < R.CantOfVertex(); j++) 
									{
										LinkToOriginal[j] = LinkToOriginal[j+Step];
									}

									LinkToOriginal[LinkToOriginal.Length-1] = null;
									if( Step == 2 ) 
										LinkToOriginal[LinkToOriginal.Length-2] = null;

									Next = (i + 1)%R.CantOfVertex();
									if( i >=  R.CantOfVertex() ) 
									{
										Good_Seg = true;
										continue;
									}

									P[0] = R[ i  ].Clone();
									P[1] = R[Next].Clone();
									P[2] = this[LinkToOriginal[ i ][ 0 ]].Clone();
									P[3] = this[LinkToOriginal[ i ][ 1 ]].Clone();		
								}
							}
						}
						#endregion
					}
					
					i++;
				}				
			}
			return true;
		}

		override public ArrayList Parallel(int dist, out Polygonal Result)
		{	
			int i = 0;
			while(  ( i < Cant) && 
				( this[i].x.numerador == 0 ) && 
				( this[i].y.numerador == 0 )  )
			{
				i++;
			}
			ArrayList Reversed = null;
			if ( !Get_Polygonal( dist, out Result,out Reversed , (i == Cant) ) )throw new Exception("Lanzo una excepcion");//Ojo
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
			ArrayList Faces = P.GetValid(Math.Sign(dist),Reversed);
			return Faces;

			

		}
		

		#endregion
	}
	
	
}
