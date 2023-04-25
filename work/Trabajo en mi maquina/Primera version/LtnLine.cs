using System;
using BigNumbers;

namespace Crecimiento
{
	/// <summary>
	/// 
	/// </summary>
	public class LtnLine
	{
		#region constants
		long Decimals_Den=100;
		#endregion

		#region Subtypes
		public struct FPoint
		{
			public double X,Y;
			public FPoint(double x, double y)
			{
				X = x;
				Y = y;
			}
			
		}
		public struct Point
		{
			public int X,Y;
			public Point(int x, int y)
			{
				X = x;
				Y = y;
			}
			
		}
		private class Cross
		{
			public enum Type{COINCIDENT_S,COINCIDENT_D, VERTEX, CROSS};
			public Type type01;
			public Type type23;
			public RPoint Cut01;
			public RPoint Cut23;
			public int Segment01;
			public int Segment23;
			public System.Int64 dir01;
			public System.Int64 dir23;
			public bool flag01;
			public bool flag23;
			public Cross()
			{
				Cut01 = null;
				Cut23 = null;
				flag01 = false;
				flag23 = false;
			}
		}
		#endregion
		
		#region Constructores
		// CONSTRUCTORES

		public LtnLine()
		{
			cant=0;
			Vertex = new LtnPoint3D[100];
		}

		
		#endregion
		
		#region Metodos
		// METODOS
		#region Utiles
		private int SD(LtnPoint3D a, LtnPoint3D b, LtnPoint3D c)
		{
			FPoint A = new FPoint(a.x,a.y);
			FPoint B = new FPoint(b.x,b.y);
			FPoint C = new FPoint(c.x,c.y);

			return SD(A,B,C);
			/*System.Int64 A = (b.x-a.x)*(c.y-a.y);
			System.Int64 B = (c.x-a.x)*(b.y-a.y);
			if ( A > B ) return  1;
			if ( A < B ) return -1;
			return 0;*/
		}
		
		private int SD(FPoint a, FPoint b, FPoint c)
		{
			double A = (b.X-a.X)*(c.Y-a.Y);
			double B = (c.X-a.X)*(b.Y-a.Y);
			if ( A - B >   10e-6  ) return  1;
			if ( A - B < -(10e-6) ) return -1;
			return 0;
		}
		private int SD(RPoint a, RPoint b, RPoint c)
		{
			/*RPoint A = new RPoint((b.x-a.x),(b.y-a.y));
			RPoint B = new RPoint((c.x-a.x),(c.y-a.y));
			BigRational Temp1 = new BigRational( A.x.entero*B.y.entero,
											   A.x.entero*B.y.numerador,
											   B.y.denominador)	+ 
						       new BigRational( B.y.entero*A.x.numerador,
											   A.x.denominador );	
			BigRational Temp2 = new BigRational( A.y.entero*B.x.entero,
											   A.y.entero*B.x.numerador,
											   B.x.denominador)	+ 
							   new BigRational( B.x.entero*A.y.numerador,
											   A.y.denominador );
			if( Temp1.entero     > Temp2.entero + 2 ) return  1;
			if( Temp1.entero + 2 < Temp2.entero     ) return -1;*/

			BigRational ax = new BigRational( a.x.entero, a.x.numerador, a.x.denominador);
			BigRational ay = new BigRational( a.y.entero, a.y.numerador, a.y.denominador);
			BigRational bx = new BigRational( b.x.entero, b.x.numerador, b.x.denominador);
			BigRational by = new BigRational( b.y.entero, b.y.numerador, b.y.denominador);
			BigRational cx = new BigRational( c.x.entero, c.x.numerador, c.x.denominador);
			BigRational cy = new BigRational( c.y.entero, c.y.numerador, c.y.denominador);

			BigRational A = (bx-ax)*(cy-ay);
			BigRational B = (cx-ax)*(by-ay);


			
			if ( A > B  ) return  1;
			if ( A < B  ) return -1;
			return 0;
		}
		private int SD(RPoint a, LtnPoint3D b, RPoint c)
		{
			BigRational A = (b.x-a.x)*(c.y-a.y);
			BigRational B = (c.x-a.x)*(b.y-a.y);
			if ( A > B  ) return  1;
			if ( A < B  ) return -1;
			return 0;
		}
		public static double Det(double a, double b, double c, double d)
		{
			return (a*d + b*c);
		}
		
		private BigRational Min(BigRational a, BigRational b)
		{
			if( a < b)
				return a;
			else
				return b;
		}
		private BigRational Max(BigRational a, BigRational b)
		{
			if( b < a)
				return a;
			else
				return b;
		}
		private BigRational Abs(BigRational a)
		{
			return a*Sign( a);
		}
		private int Sign( BigRational a )
		{
			if(!a.sign) return -1;

			if( ( a.entero == 0) && ( a.numerador == 0 ) ) return 0;
			else return 1;
		}
		#endregion

		#region Edicion
		public int CantOfVertex()
		{
			return cant;
		}

		public LtnPoint3D getVertex( int Pos )
		{
			return Vertex[Pos];
		}

		public bool AddVertex(LtnPoint3D Pt)
		{
			Vertex[cant++] = Pt;
			return true;
		}
		public bool DeleteAll()
		{
			while(cant>0)
			{
				this.DeleteVertex(--cant);
			}
			return true;
		}
		public bool DeleteVertex(int Pos)
		{
			for(int i=Pos; i<cant-1; i++)
			{
				Vertex[i] = Vertex[i+1];
			}
			Vertex[cant-1] = null;
			cant--;
			return true;
		}
		public bool InsertVertex(int Pos, LtnPoint3D Pt)
		{
			for(int i=cant; i>Pos; i--)
			{
				Vertex[i] = Vertex[i-1];
			}
			Vertex[Pos] = Pt;
			cant++;
			return true;
		}

		public bool Replace( int Pos, LtnPoint3D Pt )
		{
			if( ( Pos < 0 ) || ( Pos >= CantOfVertex() ) ) return false;

			Vertex[Pos] = Pt;
			return true;
		}
		#endregion

		#region Otros

		private LtnPoint3D Round( BigRational x, BigRational y, 
			System.Int64 a1, System.Int64 b1,
			System.Int64 a2, System.Int64 b2,
			int Sgn_Dist )
		{
			LtnPoint3D Pt = new LtnPoint3D( BigInteger.BigIntegerToLong(x.entero),
										    BigInteger.BigIntegerToLong(y.entero), 0 );

			
/*			double Frac_X,Frac_Y;

			if( (x.numerador == 0) && (y.numerador == 0 )) return Pt;

			Frac_X = ((double)x.numerador) / x.denominador;
			Frac_Y = ((double)y.numerador) / y.denominador;

			double[,] D = new double[2,4];
            			
			D[0,0] = LtnLine.Det(a1,b1,Frac_X,Frac_Y);
			D[0,1] = D[0,0] - b1;
			D[0,2] = D[0,0] - b1 + a1;
			D[0,3] = D[0,0] + a1;

			D[1,0] = LtnLine.Det(a2,b2,Frac_X,Frac_Y);
			D[1,1] = D[1,0] - b2;
			D[1,2] = D[1,0] - b2 + a2;
			D[1,3] = D[1,0] + a2;

			int[] Val = new int[2];
			Val[0] = Val[1] = 15;

			for(int i = 0; i < 4; i++)
			{
				if( (D[0,i] < 0) && (D[1,i] < 0) )
				{
					int temp = (int)Math.Pow(2,i); 
					Val[0] -= temp;
					Val[1] -= temp;
				}

				if( (D[0,i] < 0) ^ (D[1,i] < 0) ) Val[1] -= (int)Math.Pow(2,i);
			}

			int V;

			if( Val[1] > 0 ) 
				V = Val[1];
			else 
				V = Val[0];
			
			
			System.Int64 A = a1*a1 + b1*b1;
			System.Int64 B = a2*a2 + b2*b2;

			double min = -1;
			int Vert = -1;

			for(int i=3; i>=0; i--)
			{
				int temp = (int)Math.Pow(2,i);
				if(V < temp) continue;

				V -= temp;

				double aux = (D[0,i]*D[0,i])/A + (D[1,i]*D[1,i])/B;

				if( (min < 0) || (min > aux) )
				{
					Vert = i;
					min = aux;
				}
			}

			if( Vert == 1 ) Pt.x++;
			if( Vert == 2 ) 
			{
				Pt.x++;
				Pt.y++;
			}
			if( Vert == 3 ) Pt.y++;
*/
			return Pt;
		}

		private LtnPoint3D Round( BigRational x, BigRational y, 
			System.Int64 a, System.Int64 b,
			int Sgn_Dist)
		{
			LtnPoint3D Pt = new LtnPoint3D( BigInteger.BigIntegerToLong(x.entero),
				BigInteger.BigIntegerToLong(y.entero), 0 );
			
			/*double Frac_X,Frac_Y;

			if( (x.numerador == 0) && (y.numerador == 0 )) return Pt;

			Frac_X = ((double)x.numerador) / x.denominador;
			Frac_Y = ((double)y.numerador) / y.denominador;

			double[] D = new double[4];
			D[0] = LtnLine.Det(a,b,Frac_X,Frac_Y);
			D[1] = D[0] - b;
			D[2] = D[0] - b + a;
			D[3] = D[0] + a;

			int Val = 0;

			for(int i = 1; i < 4; i++)
			{
				if( (D[Val] < 0) ||
					( (Math.Sign(D[i])*Sgn_Dist >= 0) && 
					(D[i] < D[Val]) ) ) Val = i;
			}
			
			if( Val == 1 ) Pt.x++;
			if( Val == 2 ) 
			{
				Pt.x++;
				Pt.y++;
			}
			if( Val == 3 ) Pt.y++;
			*/

			return Pt;
		}
		

		/// <summary>
		/// Retorna una estructura con la informacion de la interseccion
		/// Cut01 Punto en que se intersectan
		/// Cut23 Si los segmentos son coincidentes en sentidos contrarios
		///      Este valor indica el final del intervalo coincidente.
		///      (Si se ve desde el segmento 23 Cut23 sera el principio y Cut01
		///      el final del intervalo coincidente) 
		/// Type01 el tipo de interseccion visto desde el segmento 01
		/// Type23 el tipo de interseccion visto desde el segmento 23
		/// Segment01  Estos son los segmentos  involucrados
		/// Segment23  en la interseccion.
		/// Dir01 Direccion a la que pudiera moverme si pasara del segmento 01
		///       al segmento 23.
		/// Dir23 Direccion a la que pudiera moverme si pasara del segmento 23
		///       al segmento 01.
		/// </summary>
		private Cross Intersect( RPoint[] R,int Cant, int Seg1, int Seg2, LtnPoint3D Pt,int sgn, out RPoint FPt)
		{
			Cross Cut;
			FPt = new RPoint();

			/* 
			 * Verifico que el segmento con el cual voy a buscar intersecciones 
			 * no sea ni el antrior, ni el siguiente al que estoy analizando asi
			 * como que no sea el mismo segmento.
			 */
			
			if( Seg2 == Seg1 ) 
			{
				Cut = new Cross();
				Cut.Cut01 = null;
				return Cut;
			}				
		
			// Closed me dice si la poligonal es cerrada
			bool Closed = ( ( getVertex( CantOfVertex()-1 ).x == getVertex(0).x ) &&
							( getVertex( CantOfVertex()-1 ).y == getVertex(0).y ) );
			
			RPoint[] Pts = new RPoint[4];

			/*
			 * En Pts guardare los extremos de los dos segmentos a analizar
			 * en los indices 0 y 1 el primero y
			 * en los indices 2 y 3 el segundo.
			 * En caso de que Pt no fuera null quiere decir que el analisis 
			 * del primer segmento se realizara hasta Pt y no hasta el extremo
			 * final del segmento.
			 */
 
			if( Pt != null )
			{
				Pts[0] = R[Seg1].Clone();
				Pts[1] = new RPoint( new BigRational(Pt.x,0,1),new BigRational(Pt.y,0,1) );
			}
			else
			{
				Pts[0] = R[Seg1].Clone();
				Pts[1] = R[Seg1+1].Clone();
			}
							
			Pts[2] = R[Seg2].Clone();
			Pts[3] = R[Seg2+1].Clone();

			RPoint Zero = new RPoint();

			/*
			 * Veo si las cajas que contienen a los segmentos no se intersectan ya que
			 * de ser asi los segmentos no podrian cortarse.
			 */				
			if( (Min(Pts[0].x,Pts[1].x) > Max(Pts[2].x,Pts[3].x)) || 
				(Min(Pts[0].y,Pts[1].y) > Max(Pts[2].y,Pts[3].y)) || 
				(Max(Pts[0].x,Pts[1].x) < Min(Pts[2].x,Pts[3].x)) || 
				(Max(Pts[0].y,Pts[1].y) < Min(Pts[2].y,Pts[3].y)) ) 
			{				
				Cut = new Cross();
				Cut.Cut01 = null;
				return Cut;
			}

			/*
			 * Traslado el sistema de coordenadas al extremo inicial del
			 * primer segmento.
			 */ 
			
			System.Int64 Det230 = this.SD(Pts[2], Pts[3], Pts[0]);
			System.Int64 Det231 = this.SD(Pts[2], Pts[3], Pts[1]);
			System.Int64 Det012 = this.SD(Pts[0], Pts[1], Pts[2]);
			System.Int64 Det013 = this.SD(Pts[0], Pts[1], Pts[3]);
			
			#region Coincidentes
			if( ( Det230 == 0) && ( Det231 == 0) )
			{
				bool Same_Dir;
				/*
				 * Estan en la misma recta por lo que primero veo si los segmentos 
				 * se tocan.
				 */ 
				if( ( ( Sign( Pts[2].x - Pts[1].x ) * Sign( Pts[2].x - Pts[0].x ) == 1 ) ||
					  ( Sign( Pts[2].y - Pts[1].y ) * Sign( Pts[2].y - Pts[0].y ) == 1 ) )&&
					( ( Sign( Pts[3].x - Pts[1].x ) * Sign( Pts[3].x - Pts[0].x ) == 1 ) ||
					  ( Sign( Pts[3].y - Pts[1].y ) * Sign( Pts[3].y - Pts[0].y ) == 1 ) )&&
					( ( Sign( Pts[1].x - Pts[2].x ) * Sign( Pts[1].x - Pts[3].x ) == 1 ) ||
					  ( Sign( Pts[1].y - Pts[2].y ) * Sign( Pts[1].y - Pts[3].y ) == 1 ) ) ) 
				{
					//no se cortan
					Cut = new Cross();
					Cut.Cut01 = null;
					return Cut;
				}
								
				/*
				 * Tengo que verificar los casos en que aunque los segmentos estan 
				 * en una misma recta solo tienen un punto en comun.
				 */ 
				
				if( ( Sign( Pts[3].x - Pts[2].x ) != Sign( Pts[1].x - Pts[0].x ) )||
					( Sign( Pts[3].y - Pts[2].y ) != Sign( Pts[1].y - Pts[0].y ) ) )
				{
					/*
					 * Los segmentos no tienen la misma orientacion
					 */ 

					Same_Dir = false;

					if( ( ( Pts[0].x == Pts[2].x ) && 
						  ( Pts[0].y == Pts[2].y ) ) ||
						( ( Pts[1].x == Pts[3].x ) && 
						  ( Pts[1].y == Pts[3].y ) ) )
					{
						/*
						 * La unica forma de que me interese la interseccion con el
						 * segmento anterior o posterior del analisado es que fueran 
						 * coincidentes lo cual no ocurre si entro aqui por lo que						 
						 * en esos casos paso al proximo segmento.
						 */
						if( Closed )
						{
							if( ( ( Seg1 == 0 ) && ( Seg2 == (CantOfVertex()-2) ) ) ||
								( ( Seg1 == (CantOfVertex()-2) ) && ( Seg2 == 0 ) ) ) 
							{
								Cut = new Cross();
								Cut.Cut01 = null;
								return Cut;
							}
						}
					
						if( ( Seg2 == (Seg1+1) ) || ( Seg2 == (Seg1-1) ) ) 
						{
							Cut = new Cross();
							Cut.Cut01 = null;
							return Cut;
						}
					}
						
					if( ( Pts[0].x == Pts[2].x ) && 
						( Pts[0].y == Pts[2].y ) )
					{
						Cut = new Cross();
						Cut.Cut01 = new RPoint(Pts[0].x,Pts[0].y);
						Cut.Cut23 = null;
						FPt.x = Pts[0].x;
						FPt.y = Pts[0].y;
						Cut.type01 = Cross.Type.VERTEX;
						Cut.type23 = Cross.Type.VERTEX;
						Cut.Segment01 = Seg1;
						Cut.Segment23 = Seg2;
						Cut.dir01 = 2;
						Cut.dir23 = 2;
						return Cut;
					}

					if( ( Pts[1].x == Pts[3].x ) && 
						( Pts[1].y == Pts[3].y ) )
					{
						Cut = new Cross();
						Cut.Cut01 = new RPoint(Pts[1].x,Pts[1].y);
						Cut.Cut23 = null;
						FPt.x = Pts[1].x;
						FPt.y = Pts[1].y;
						Cut.type01 = Cross.Type.VERTEX;
						Cut.type23 = Cross.Type.VERTEX;
						Cut.Segment01 = 2;
						Cut.Segment23 = 2;
						Cut.dir01 = -sgn;
						Cut.dir23 = -sgn;
						return Cut;
					}
				}
				else
				{
					/*
					 * Los segmentos tienen la misma orientacion
					 */
					Same_Dir = true;

					if( ( ( Pts[0].x == Pts[3].x ) && 
					  	  ( Pts[0].y == Pts[3].y ) ) ||
						( ( Pts[1].x == Pts[2].x ) && 
						  ( Pts[1].y == Pts[2].y ) ) )
					{
						/*
						 * La unica forma de que me interese la interseccion con el
						 * segmento anterior o posterior del analisado es que fueran 
						 * coincidentes lo cual no ocurre si entro aqui por lo que
						 * en esos casos paso al proximo segmento.
						 */

						if( Closed )
						{
							if( ( ( Seg1 == 0 ) && ( Seg2 == (this.CantOfVertex()-2) ) ) ||
								( ( Seg1 == (this.CantOfVertex()-2) ) && ( Seg2 == 0 ) ) ) 
							{
								Cut = new Cross();
								Cut.Cut01 = null;
								return Cut;
							}
						}
					
						if( ( Seg2 == (Seg1+1) ) || ( Seg2 == (Seg1-1) ) ) 
						{
							Cut = new Cross();
							Cut.Cut01 = null;
							return Cut;
						}
					}

					if( ( Pts[0].x == Pts[3].x ) && 
						( Pts[0].y == Pts[3].y ) )
					{
						Cut = new Cross();
						Cut.Cut01 = new RPoint(Pts[0].x,Pts[0].y);
						Cut.Cut23 = null;
						FPt.x = Pts[0].x;
						FPt.y = Pts[0].y;
						Cut.type01 = Cross.Type.VERTEX;
						Cut.type23 = Cross.Type.VERTEX;
						Cut.Segment01 = Seg1;
						Cut.Segment23 = Seg2;
						Cut.dir01 = 2;
						Cut.dir23 = sgn;
						return Cut;
					}

					if( ( Pts[1].x == Pts[2].x ) && 
						( Pts[1].y == Pts[2].y ) )
					{
						Cut = new Cross();
						Cut.Cut01 = new RPoint(Pts[1].x,Pts[1].y);
						Cut.Cut23 = null;
						FPt.x = Pts[1].x;
						FPt.y = Pts[1].y;
						Cut.type01 = Cross.Type.VERTEX;
						Cut.type23 = Cross.Type.VERTEX;
						Cut.Segment01 = Seg1;
						Cut.Segment23 = Seg2;
						Cut.dir01 = sgn;
						Cut.dir23 = 2;
						return Cut;
					}
						
				}

				if(Same_Dir)
				{
					Cut = new Cross();
					Cut.type01 = Cross.Type.COINCIDENT_S;
					Cut.type23 = Cross.Type.COINCIDENT_S;
					Cut.Segment01 = Seg1;
					Cut.Segment23 = Seg2;
					Cut.Cut23 = null;

					RPoint Pto = null;

					if( ( Pts[3].x == Pts[1].x ) && ( Pts[3].y == Pts[1].y ) )
					{
						Cut.dir01 = 2;
						Cut.dir23 = 2;
					}
					else
					{

						if( ( Sign( Pts[2].x - Pts[1].x ) * Sign( Pts[3].x - Pts[1].x ) < 1 ) &&
							( Sign( Pts[2].y - Pts[1].y ) * Sign( Pts[3].y - Pts[1].y ) < 1 ) )
						{
							if( ( Closed ) && ( Seg1 == Cant - 2 ) )
							{
								Pto = R[1];
							}
							else
							{
								if( Seg1 < Cant - 2 )
									Pto = R[Seg1+2];
							}

							Cut.dir01 = 2;
							Cut.dir23 = 2;

							if( ( Pto != null ) && ( SD( Pts[0],Pts[1],Pto ) != sgn ) )
							{
								Cut.dir01 = sgn;
							}
						}
						else
						{
							if( ( Closed ) && ( Seg2 == Cant - 2 ) )
							{
								Pto = R[1];
							}
							else
							{
								if( Seg2 < Cant - 2 )
									Pto = R[Seg2+2];
							}

							Cut.dir01 = 2;
							Cut.dir23 = 2;

							if( ( Pto != null ) && ( SD( Pts[2],Pts[3],Pto ) != sgn ) )
							{
								Cut.dir23 = sgn;
							}
						}
					}

					if( ( Pts[2].x == Pts[0].x ) && ( Pts[2].y == Pts[0].y ) )
					{
						Cut.Cut01 = new RPoint(Pts[0].x,Pts[0].y);
						FPt.x = Pts[0].x;
						FPt.y = Pts[0].y;						
					}
					else
					{
						if( ( Sign( Pts[2].x - Pts[0].x ) * Sign( Pts[3].x - Pts[0].x ) < 1 ) &&
							( Sign( Pts[2].y - Pts[0].y ) * Sign( Pts[3].y - Pts[0].y ) < 1 ) )
						{
							Cut.Cut01 = new RPoint(Pts[1].x,Pts[1].y);
							FPt.x = Pts[0].x;
							FPt.y = Pts[0].y;
						}
						else
						{
							Cut.Cut01 = new RPoint(Pts[2].x,Pts[2].y);
							FPt.x = Pts[2].x;
							FPt.y = Pts[2].y;							
						}
					}	
					return Cut;
				}
				else
				{
					Cut = new Cross();
					Cut.type01 = Cross.Type.COINCIDENT_D;
					Cut.type23 = Cross.Type.COINCIDENT_D;
					Cut.Segment01 = Seg1;
					Cut.Segment23 = Seg2;
					Cut.dir01 = sgn;
					Cut.dir23 = sgn;

					if( ( Sign( Pts[2].x - Pts[0].x ) * Sign( Pts[3].x - Pts[0].x ) < 1 ) &&
						( Sign( Pts[2].y - Pts[0].y ) * Sign( Pts[3].y - Pts[0].y ) < 1 ) )
					{
						Cut.Cut01 =  new RPoint(Pts[0].x,Pts[0].y);
						FPt.x = Pts[0].x;
						FPt.y = Pts[0].y;
					}
					else
					{
						Cut.Cut01 =  new RPoint(Pts[3].x,Pts[3].y);
						FPt.x = Pts[3].x;
						FPt.y = Pts[3].y;
					}

					if( ( Sign( Pts[0].x - Pts[2].x ) * Sign( Pts[1].x - Pts[2].x ) < 1 ) &&
						( Sign( Pts[0].y - Pts[2].y ) * Sign( Pts[1].y - Pts[2].y ) < 1 ) )
					{
						Cut.Cut23 =  new RPoint(Pts[2].x,Pts[2].y);
					}
					else
					{
						Cut.Cut23 =  new RPoint(Pts[1].x,Pts[1].y);
					}
					return Cut;
				}
			}
			#endregion

			/*
			 * La unica forma de que me interese la interseccion con el
			 * segmento anterior o posterior del analizado es que fueran 
			 * coincidentes lo cual no ocurre si llego aqui por lo que
			 * en esos casos paso al proximo segmento.
			 */ 
			if( Closed )
			{
				if( ( ( Seg1 == 0 ) && ( Seg2 == (Cant-2) ) ) ||
					( ( Seg1 == (Cant-2) ) && ( Seg2 == 0 ) ) ) 
				{
					Cut = new Cross();
					Cut.Cut01 = null;
					return Cut;
				}
			}
					
			if( ( Seg2 == (Seg1+1) ) || ( Seg2 == (Seg1-1) ) ) 
			{
				Cut = new Cross();
				Cut.Cut01 = null;
				return Cut;
			}

			/*
			 * Si cualquiera de los dos segmentos tiene a los dos extremos del
			 * otro del mismo lado los segmentos no se intersectan.
			 */ 
				
			if( ( Det230 == Det231 ) || 
				( Det012 == Det013 ) )
			{
				Cut = new Cross();
				Cut.Cut01 = null;
				return Cut;
			}
			/*
			 * La direccion01 es el sentido del segmento 23 si llego a el
			 * moviendome por el segmento 01. Si el segmento 23 no tiene ningun
			 * intervalo del lado alido de 01 (mismo lado que la distancia) entonces dir01 
			 * valdra 2. De la misma forma calculo dir23.
			 */
			#region En un Vertice
				
			if( ( Det012 == 0 ) || ( Det013 == 0 ) || ( Det230 == 0 ) || ( Det231 == 0 ) )
			{
				/*
				 * La interseccion ocurre en uno de los cuatro vertices.				 
				 */
				if( Det231 == 0 )
				{
					Cut = new Cross();
					Cut.Cut01 = new RPoint(Pts[1].x,Pts[1].y);	
					Cut.Cut23 = null;
					FPt.x = Pts[1].x;
					FPt.y = Pts[1].y;
					Cut.type23 = Cross.Type.VERTEX;
					
					if( (Det013 == 0) || (Det012 == 0) )
					{
						Cut.type01 = Cross.Type.VERTEX;
					}
					else
					{
						Cut.type01 = Cross.Type.CROSS;
					}

					/*
					 * Si la interseccion es al final del segmento, saver si 
					 * la direccion es correcta no solo depende del segmento
					 * analizado sino que tambien depende del segmento siguente.
					 */

					RPoint Pto=null;
					if( ( Closed ) && ( Seg1 == Cant - 2 ) )
					{
						Pto = R[1];
					}
					else
					{
						if( Seg1 < Cant - 2 )
							Pto = R[Seg1+2];
					}

					if( ( (Det013 == 0) && (Det012 != sgn) && 
						  ( (Pto==null) || (SD(Pts[1],Pto,Pts[2]) !=sgn) ) ) ||
						( (Det012 == 0) && (Det013 != sgn) && 
						  ( (Pto==null) || (SD(Pts[1],Pto,Pts[3]) !=sgn) ) ) )
					{
						Cut.dir01 = 2;
					}
					else
					{
						Cut.dir01 = Det230;
					}

					if( ( Closed ) && ( Seg2 == Cant - 2 ) )
					{
						Pto = R[1];
					}
					else
					{
						if( Seg2 < Cant - 2 )
							Pto = R[Seg2+2];
					}
					
					if( ( Det230 != sgn ) &&
						( ( Det013 != 0) || (Pt==null) || (SD(Pts[3],Pto,Pts[0]) !=sgn ) ) )
					{
						Cut.dir23 = 2;
					}
					else
					{
						Cut.dir23 = Det012;
						if(Cut.dir23 == 0) Cut.dir23 = -Det013;
					}

					Cut.Segment01 = Seg1;
					Cut.Segment23 = Seg2;			
					return Cut;
				}				
				
				if( Det230 == 0 )
				{
					Cut = new Cross();
					Cut.Cut01 = new RPoint(Pts[0].x,Pts[0].y);	
					Cut.Cut23 = null;
					FPt.x = Pts[0].x;
					FPt.y = Pts[0].y;
					Cut.type23 = Cross.Type.VERTEX;
					
					if( (Det013 == 0) || (Det012 == 0) )
					{
						Cut.type01 = Cross.Type.VERTEX;
					}
					else
					{
						Cut.type01 = Cross.Type.CROSS;
					}

					if( ( (Det013 == 0) && (Det012 != sgn) ) ||
						( (Det012 == 0) && (Det013 != sgn) ) )
					{
						Cut.dir01 = 2;
					}
					else
					{
						Cut.dir01 = -Det231;
					}
					
					RPoint Pto=null;
					if( ( Closed ) && ( Seg2 == Cant - 2 ) )
					{
						Pto = R[1];
					}
					else
					{
						if( Seg2 < Cant - 2 )
							Pto = R[Seg2+2];
					}
					
					if( ( Det231 != sgn ) &&
						( ( Det013 != 0) || (Pt==null) || (SD(Pts[3],Pto,Pts[1]) !=sgn ) ) )
					{
						Cut.dir23 = 2;
					}
					else
					{
						Cut.dir23 = Det012;
						if(Cut.dir23 == 0) Cut.dir23 = -Det013;
					}
					
					Cut.Segment01 = Seg1;
					Cut.Segment23 = Seg2;			
					return Cut;
				}
				
				if( Det012 == 0 )
				{
					Cut = new Cross();
					Cut.Cut01 = new RPoint(Pts[2].x,Pts[2].y);
					Cut.Cut23 = null;
					FPt.x = Pts[2].x;
					FPt.y = Pts[2].y;
					Cut.type01 = Cross.Type.VERTEX;
					Cut.type23 = Cross.Type.CROSS;
					Cut.Segment01 = Seg1;
					Cut.Segment23 = Seg2;
					
					Cut.dir23 = -Det013;
										
					if( Det013 != sgn )
						Cut.dir01 = 2;
					else
						Cut.dir01 = Det230;

					return Cut;
				}

				if( Det013 == 0 )
				{
					Cut = new Cross();
					Cut.Cut01 = new RPoint(Pts[3].x,Pts[3].y);
					Cut.Cut23 = null;
					FPt.x = Pts[3].x;
					FPt.y = Pts[3].y;
					Cut.type01 = Cross.Type.VERTEX;
					Cut.type23 = Cross.Type.CROSS;
					Cut.Segment01 = Seg1;
					Cut.Segment23 = Seg2;
					Cut.dir23 = Det012;
										
					if( Det012 != sgn )
						Cut.dir01 = 2;
					else
						Cut.dir01 = Det230;
					return Cut;
				}				
			}	
			#endregion

			#region Corta
			
			BigRational a1  =  Pts[1].x - Pts[0].x;
			BigRational b1  =  Pts[1].y - Pts[0].y;
			BigRational a2  =  Pts[3].x - Pts[2].x;
			BigRational b2  =  Pts[3].y - Pts[2].y;

			BigRational x,y;
			
			if (a2.entero ==0 && a2.numerador ==0)
			{
				BigRational m1 = b1/a1;
				x  = Pts[2].x ;
				y  = m1*(Pts[2].x - Pts[0].x) +  Pts[0].y ;
				
			}
			else if (a1.entero ==0 && a1.numerador ==0)
			{
				BigRational m2 = b2/a2;
				x  = Pts[0].x ;
				y  = m2*(Pts[0].x - Pts[2].x) +  Pts[2].y ;
				
			} 
			else
			{
				BigRational m1 = b1/a1;
				BigRational m2 = b2/a2;

				BigRational Den = (m1 - m2);

				x  = ( m2*(Pts[0].x - Pts[2].x) + ( Pts[2].y - Pts[0].y ) ) / Den;
				y  = m1*( ( m2*(Pts[0].x - Pts[2].x) + ( Pts[2].y - Pts[0].y ) ) )/ Den;
				
				x += Pts[0].x;
				y += Pts[0].y;
				
			}
			
			
				
			FPt.x = x;
			FPt.y = y;

			Cut = new Cross();
			Cut.Cut01 = new RPoint(x,y);				
			Cut.Cut23 = null;
			Cut.Segment01 = Seg1;
			Cut.Segment23 = Seg2;
			Cut.dir01 = Det230;
			Cut.dir23 = Det012;
			Cut.type01 = Cross.Type.CROSS;
			Cut.type23 = Cross.Type.CROSS;
			return Cut;
			#endregion
		}		

		private Cross[][] FindAllIntersections(RPoint[] R,int Cant,int sgn)
		{
			bool Closed = ( (R[0].x == R[Cant-1].x) && (R[0].y == R[Cant-1].y) );

			Cross[][] Result = new Cross[Cant][];
			int[] Cant_Cuts = new int[Cant];
			
			for( int i = 0; i < Cant-1;i++)
			{
				Cross[] Cortes = null;

				int Cuts_cant=0;
				Cross[] Cuts	  = new Cross[ 5 ];
				RPoint[] Float_Cuts = new RPoint[ 5 ];
                
				for( int j = i+1; j < Cant-1; j++ )
				{
					RPoint FPt;
					Cuts[Cuts_cant] = Intersect( R,Cant,i, j, null,sgn,out FPt );

				
					if( Cuts[Cuts_cant].Cut01 != null ) 
					{
							Float_Cuts[Cuts_cant] = FPt;
						Cuts_cant++;
					}	
					if( Cuts_cant == Cuts.Length )
					{
						Cross[] temp = Cuts;
						Cuts = new Cross[Cuts.Length+5];
						temp.CopyTo(Cuts,0);
					
						RPoint[] f_temp = Float_Cuts;
						Float_Cuts = new RPoint[Float_Cuts.Length+5];
						f_temp.CopyTo(Float_Cuts,0);
					}
				}
				int step = 0;
				if( i == 0 ) 
					step = 2;
				else
                    if( i == Cant-2 ) 
						step = 0;
					else 
						step = 1;
				

				

				if( Cuts_cant+step == Cuts.Length )
				{
					Cross[] temp = Cuts;
					Cuts = new Cross[Cuts.Length+step];
					temp.CopyTo(Cuts,0);
					
					RPoint[] f_temp = Float_Cuts;
					Float_Cuts = new RPoint[Float_Cuts.Length+step];
					f_temp.CopyTo(Float_Cuts,0);
				}
				if(i==0)
				{
					Cuts[Cuts_cant]=new Cross();
					Cuts[Cuts_cant].Segment23 = Cant-2;
					Cuts[Cuts_cant].Segment01 = 0;
					Cuts[Cuts_cant].Cut01 = R[0];		
					Cuts[Cuts_cant].Cut23 = null;
					Cuts[Cuts_cant].dir01 = 3;
					Cuts[Cuts_cant].dir23 = 3;
					Cuts[Cuts_cant].type01 = Cross.Type.VERTEX;
					Cuts[Cuts_cant].type23 = Cross.Type.VERTEX;
					Cuts_cant++;
				}
				if( i < Cant-2 )
				{
					Cuts[Cuts_cant]=new Cross();
					Cuts[Cuts_cant].Segment23 = i+1;
					Cuts[Cuts_cant].Segment01 = i;
					Cuts[Cuts_cant].Cut01 = R[i+1];		
					Cuts[Cuts_cant].Cut23 = null;
					Cuts[Cuts_cant].dir01 = 3;
					Cuts[Cuts_cant].dir23 = 3;
					Cuts[Cuts_cant].type01 = Cross.Type.VERTEX;
					Cuts[Cuts_cant].type23 = Cross.Type.VERTEX;
					Cuts_cant++;
				}


				Cortes = new Cross[Cuts_cant+Cant_Cuts[i]];
            
				for(int l=0; l<Cuts_cant; l++) 
				{						
					Cortes[l] = Cuts[l];
					if(Cuts[l].Segment01 != i)
					{
						if(Result[Cuts[l].Segment01] == null)
						{
							Result[Cuts[l].Segment01] = new Cross[Cant];
							Cant_Cuts[Cuts[l].Segment01] = 0;
						}
						Result[Cuts[l].Segment01][Cant_Cuts[Cuts[l].Segment01]++] = Cuts[l];
					}
					else
					{
						if(Result[Cuts[l].Segment23] == null)
						{
							Result[Cuts[l].Segment23] = new Cross[Cant];
							Cant_Cuts[Cuts[l].Segment23] = 0;
						}
						Result[Cuts[l].Segment23][Cant_Cuts[Cuts[l].Segment23]++] = Cuts[l];
					}
				}
				for(int l=0; l<Cant_Cuts[i]; l++) 
				{
					Cortes[Cuts_cant+l] = Result[i][l];					
				}				

				#region Sorted

				Point sg = new Point(Sign(R[i+1].x-R[i].x),Sign(R[i+1].y-R[i].y));

				for(int l = 0; l < Cortes.Length-1; l++)
				{
					RPoint CorteL = null,
						   CorteK = null;
					
					if( ( Cortes[l].Cut23 != null ) && ( Cortes[l].Segment23 == i ) )
						CorteL =  Cortes[l].Cut23;
					else
						CorteL =  Cortes[l].Cut01;

					for(int k = l+1; k < Cortes.Length; k++)
					{
						if( ( Cortes[k].Cut23 != null ) && ( Cortes[k].Segment23 == i ) )
							CorteK =  Cortes[k].Cut23;
						else
							CorteK =  Cortes[k].Cut01;

						if( ( CorteL.x * sg.X < CorteK.x * sg.X ) ||
							( CorteL.y * sg.Y < CorteK.y * sg.Y ) ) continue;
						
						if( ( CorteL.x * sg.X == CorteK.x * sg.X ) &&
							( CorteL.y * sg.Y == CorteK.y * sg.Y ) )
						{
							RPoint P0 = R[i],
								   P1 = R[i+1],
								   P2 = null,
								   P3 = null;

							
							if( ( ( Cortes[k].Segment01 == i ) && ( Cortes[k].dir01 == 2 ) ) ||
								( ( Cortes[k].Segment23 == i ) && ( Cortes[k].dir23 == 2 ) ) ) continue;
														
							if( ( ( Cortes[l].Segment01 == i ) && ( Cortes[l].dir01 == sgn ) ) ||
								( ( Cortes[l].Segment23 == i ) && ( Cortes[l].dir23 == sgn ) ) ) 
							{
								if( Cortes[l].Segment23 == i )
									P2 = R[ Cortes[l].Segment01+1 ];
								else
									P2 = R[ Cortes[l].Segment23+1 ];
							}
							else
							{
								if( Cortes[l].Segment23 == i )
									P2 = R[ Cortes[l].Segment01 ];
								else
									P2 = R[ Cortes[l].Segment23 ];
							}

							if( ( ( Cortes[k].Segment01 == i ) && ( ( Cortes[k].dir01 == sgn ) || ( Cortes[k].dir01 == 3 ) ) ) ||
								( ( Cortes[k].Segment23 == i ) && ( ( Cortes[k].dir23 == sgn ) || ( Cortes[k].dir01 == 3 ) ) ) ) 
							{
								if( Cortes[k].Segment23 == i )
									P3 = R[ Cortes[k].Segment01+1 ];
								else
									P3 = R[ Cortes[k].Segment23+1 ];
							}
							else
							{
								if( Cortes[k].Segment23 == i )
									P3 = R[ Cortes[k].Segment01 ];
								else
									P3 = R[ Cortes[k].Segment23 ];
							}

							int Det01l = SD(P0,P1,P2),
								Det01k = SD(P0,P1,P3),
								Pos = SD(CorteK,P2,P3);
							
							if( ( Det01l == Det01k ) && (Pos == -sgn) ) continue;

							if( ( Det01l != Det01k ) && (Det01k == -sgn) ) continue;

							if(Pos==0)
							{//k y l son coincidentes. Ver que Pasa.
								if( ( ( Cortes[k].Segment01 == i ) && ( Cortes[k].dir01 == -sgn ) ) ||
									( ( Cortes[k].Segment23 == i ) && ( Cortes[k].dir23 == -sgn ) ) ) continue;
							}								
						}
						Cross Temp = Cortes[l];
						Cortes[l] = Cortes[k];
						Cortes[k] = Temp;
						CorteL = CorteK;
					}
				}
				#endregion
				Result[i] = Cortes;
			}
			
			
			
			return Result;
		}
		
		private LtnLine FindValidSection(RPoint Init_Pt, RPoint[] R,int Cant, Cross[][] Intersections, int Seg, int i,int sgn)
		{
			bool Closed = ( (R[0].x == R[Cant-1].x) && (R[0].y == R[Cant-1].y) );

			LtnLine Result = new LtnLine();
			Result.AddVertex( new LtnPoint3D( BigInteger.BigIntegerToLong(Init_Pt.x.entero),
				BigInteger.BigIntegerToLong(Init_Pt.y.entero), 0 ) );

			int Seg1,Seg2,i1,i2;
			RPoint Pt;

			Seg1 = Seg;
			i1 = i;
			i2 = 0;
			
			do
			{				
				Pt = Intersections[Seg1][i1].Cut01;
				
				Seg2 = Seg1;
				i2 = i1;

				if(Intersections[Seg2][i2].dir01 == 3)
				{
					if( Intersections[Seg2][i2].Segment01 == Seg2 )
					{
						Intersections[Seg2][i2].flag01 = true;
					}
					else
					{
						Intersections[Seg2][i2].flag23 = true;
					}
				}

				Result.AddVertex( new LtnPoint3D( BigInteger.BigIntegerToLong(Pt.x.entero),
										    BigInteger.BigIntegerToLong(Pt.y.entero), 0 ));

				if( (Pt.x == Init_Pt.x) && (Pt.y == Init_Pt.y) )
				{
					continue;
				}

				if( ( ( Intersections[Seg2][i2].Segment01 == Seg2 ) && (Intersections[Seg2][i2].flag23) ) ||
					( ( Intersections[Seg2][i2].Segment23 == Seg2 ) && (Intersections[Seg2][i2].flag01) ) )
				{
					Result = null;
					return Result;
				}

				if( Intersections[Seg2][i2].Segment01 != Seg2)
					Seg2 = Intersections[Seg2][i2].Segment01;
				else
					Seg2 = Intersections[Seg2][i2].Segment23;
			
				i2=0;
				int init = i2;

				if( (!Closed) && ( Seg2 == Cant-1 ) )
				{
					return Result;
				}

				//Busco el punto de interseccion en el otro segmento

				while( ( i2 < Intersections[Seg2].Length ) &&
					   ( ( Intersections[Seg2][i2].Cut01.x != Intersections[Seg1][i1].Cut01.x ) ||
					     ( Intersections[Seg2][i2].Cut01.y != Intersections[Seg1][i1].Cut01.y ) ||
						 ( Intersections[Seg2][i2].Cut23 != null ) ) )
				{
					if( Intersections[Seg2][i2].Cut23 != null )
					{
						if( Intersections[Seg2][i2].Segment01 == Seg2 )
						{
							if( ( Intersections[Seg2][i2].Cut01.x == Intersections[Seg1][i1].Cut01.x ) &&
								( Intersections[Seg2][i2].Cut01.y == Intersections[Seg1][i1].Cut01.y ) )
							{
								break;
							}
						}
						else
						{
							if( ( Intersections[Seg2][i2].Cut23.x == Intersections[Seg1][i1].Cut01.x ) &&
								( Intersections[Seg2][i2].Cut23.y == Intersections[Seg1][i1].Cut01.y ) )
							{
								break;
							}
						}						
					}
					i2++;
				}
				init = i2;

				//Paso a la siguiente interseccion.
				while( ( i2 < Intersections[Seg2].Length ) &&
					   ( ( ( Intersections[Seg2][i2].Cut01.x == Intersections[Seg1][i1].Cut01.x ) &&
						   ( Intersections[Seg2][i2].Cut01.y == Intersections[Seg1][i1].Cut01.y ) ) ||
						 ( Intersections[Seg2][i2].Cut23 != null ) ) )
				{
					if( Intersections[Seg2][i2].Cut23 != null )
					{
						if( Intersections[Seg2][i2].Segment01 == Seg2 )
						{
							if( ( Intersections[Seg2][i2].Cut01.x != Intersections[Seg1][i1].Cut01.x ) ||
								( Intersections[Seg2][i2].Cut01.y != Intersections[Seg1][i1].Cut01.y ) )
							{
								break;
							}
						}
						else
						{
							if( ( Intersections[Seg2][i2].Cut23.x != Intersections[Seg1][i1].Cut01.x ) ||
								( Intersections[Seg2][i2].Cut23.y != Intersections[Seg1][i1].Cut01.y ) )
							{
								break;
							}
						}						
					}
					
					if( Intersections[Seg2][i2].Segment01 == Seg2 )
					{
						if( Intersections[Seg2][i2].Segment23 == Seg1 )
							Intersections[Seg2][i2].flag23 = true;
						Intersections[Seg2][i2].flag01 = true;
					}
					else
					{
						if( Intersections[Seg2][i2].Segment01 == Seg1 )
							Intersections[Seg2][i2].flag01 = true;
						Intersections[Seg2][i2].flag23 = true;
					}

					i2++;
				}
				
				if( i2 == Intersections[Seg2].Length ) 
				{
					for(int j = init; j<Intersections[Seg2].Length; j++)
					{
						if( Intersections[Seg2][j].Segment01 != Seg2)
							Intersections[Seg2][j].flag01 = true;
						else
							Intersections[Seg2][j].flag23 = true;
					}
					
					Result = null;
					return Result;
				}

				for(int j = init; j<i2-1; j++)
				{
					if( Intersections[Seg2][j].Segment01 != Seg2)
						Intersections[Seg2][j].flag01 = true;
					else
						Intersections[Seg2][j].flag23 = true;
				}
				
				if( (Intersections[Seg2][i2].Cut23 == null) ||
					(Intersections[Seg2][i2].Segment01 == Seg2 ) )
				{
					if( (Intersections[Seg2][i2].Cut01.x == Init_Pt.x) && 
						(Intersections[Seg2][i2].Cut01.y == Init_Pt.y) ) 
					{

						Pt = Intersections[Seg2][i2].Cut01;
						Result.AddVertex( new LtnPoint3D(
							  BigInteger.BigIntegerToLong(Pt.x.entero),
							  BigInteger.BigIntegerToLong(Pt.y.entero),0) );
						continue;
						
					}
				}
				else
				{
					if( (Intersections[Seg2][i2].Cut23.x == Init_Pt.x) && 
						(Intersections[Seg2][i2].Cut23.y == Init_Pt.y) ) 
					{
						Pt = Intersections[Seg2][i2].Cut23;
						Result.AddVertex( new LtnPoint3D(
							BigInteger.BigIntegerToLong(Pt.x.entero),
							BigInteger.BigIntegerToLong(Pt.y.entero),0) );
						continue;
					}
				}


				if( ( ( Intersections[Seg2][i2].Segment01 == Seg2 ) && (Intersections[Seg2][i2].flag23) ) ||
					( ( Intersections[Seg2][i2].Segment23 == Seg2 ) && (Intersections[Seg2][i2].flag01) ) )
				{
					Result = null;
					return Result;
				}
 
				if( ( Intersections[Seg2][i2].Segment01 == Seg2 ) || ( Intersections[Seg2][i2].dir01 == 3 ) ) 
				{
					if( ( Intersections[Seg2][i2].dir01 != 3 ) && ( Intersections[Seg2][i2].dir01 != sgn ) )
					{
						Result = null;
						return Result;
					}
				}
				else
				{
					if( Intersections[Seg2][i2].dir23 != sgn ) 
					{
						Result = null;
						return Result;
					}
				}
				Seg1 = Seg2;
				i1 = i2;				
			}while( (Pt.x != Init_Pt.x) || (Pt.y != Init_Pt.y) );

			return Result;
		}

		private bool Validate(RPoint[] R,int Cant,int[][] LinkToOriginal,RPoint V,int S1,int S2,int dist)
		{
			int sgnDist = Math.Sign(dist);
			dist = Math.Abs(dist);

			RPoint Seg = new RPoint();
			Point Sig = new Point();
			
			RPoint Zero = new RPoint();

			RPoint[] Pts = new RPoint[3];
			for(int i=0;i<3;i++)
			{
				Pts[i]=new RPoint();
			}
			
			LtnPoint3D Dif = new LtnPoint3D();

			Dif.x = Vertex[LinkToOriginal[S1][1]].x - Vertex[LinkToOriginal[S1][0]].x;
			Dif.y = Vertex[LinkToOriginal[S1][1]].y - Vertex[LinkToOriginal[S1][0]].y;
			
			for(int i=0; i<=Cant-2; i++)
			{	
				/*
				 *  Si el segmento a analizar es uno de los dos involucrados 
				 * en la interseccion no lo analizo.
				 */ 
				if( i == S1 ) continue; 
				if( ( S2!= -1 ) && ( i == S2 ) ) continue;
				if( ( S2 == -1 ) && ( i == S1-1 ) ) continue;

				LtnPoint3D Point = new LtnPoint3D(Vertex[LinkToOriginal[i][1]].x - Vertex[LinkToOriginal[i][0]].x,
					Vertex[LinkToOriginal[i][1]].y - Vertex[LinkToOriginal[i][0]].y,0);
				Seg.x = V.x - Vertex[LinkToOriginal[i][0]].x;
				Seg.y = V.y - Vertex[LinkToOriginal[i][0]].y;

				System.Int64 a  = Point.x;
				System.Int64 b  = Point.y; 
				System.Int64 A  = (a*a + b*b);

				/*Calculo de la distancia aproximada del segmento*/
				double aux = dist*Math.Sqrt(A);
				System.Int64 Ent = (long)Math.Floor( aux );
				aux -= Ent; 
				System.Int64 Num = (long)Math.Ceiling( aux*Decimals_Den );

				BigNumbers.BigRational d = new BigNumbers.BigRational(Ent, Num, Decimals_Den);/*6cifras decimales*/

				BigRational temp = b*Seg.x - a*Seg.y;

                #region Out of distance
				if( Abs(temp) >= d )
				{
					if( -Sign(temp) == sgnDist) 
						//esta del lado correcto
						continue;
					else
					{
						//No se consideran las intersecciones en uno de los vertices

						Pts[0].x = R[ i+1 ].x - R[ S1 ].x;
						Pts[0].y = R[ i+1 ].y - R[ S1 ].y;

						Pts[1].x = R[  S1  ].x - R[ i ].x;
						Pts[1].y = R[  S1  ].y - R[ i ].y;

						Pts[2].x = R[ S1+1 ].x - R[ i ].x;
						Pts[2].y = R[ S1+1 ].y - R[ i ].y;

						if( ( SD(Zero,Point,Pts[1])*SD(Zero,Point,Pts[2]) == -1 ) &&
							( SD(Zero,Dif,Pts[1])*SD(Zero,Dif,Pts[0]) == 1 ) ) return false;						
						else 
							continue;
					}
				}
				#endregion


				#region In Rectangle

				BigRational Proy;
				if( Point.x != 0 )
				{
					//Comparamos las X.
					Proy = Seg.x - temp*b/A;
					if( (Proy * Math.Sign( Point.x ) >= 0 ) && 
						(Proy * Math.Sign( Point.x ) <= Math.Abs(Point.x) ) ) return false;
				}
				else
				{
					//Comparamos las Y.
					Proy = Seg.y + temp*a/A;
					if( (Proy * Math.Sign( Point.y ) >= 0 ) && 
						(Proy * Math.Sign( Point.y ) <= Math.Abs(Point.y) ) ) return false;
				}
                #endregion
				
				#region In Extreme Circle

				BigRational Radius = d*d/A;

				BigRational AR = ( Seg.x*Seg.x + Seg.y*Seg.y );
				BigRational Ar = ( Seg.x - Point.x)*( Seg.x - Point.x) + 
					  ( Seg.y - Point.y)*( Seg.y - Point.y);

				BigRational Ras = ( Seg.x - Point.x)*( Seg.x - Point.x) + ( Seg.y - Point.y)*( Seg.y - Point.y);
				BigRational Ras1 = ( Seg.x*Seg.x + Seg.y*Seg.y );

				if( ( ( Seg.x*Seg.x + Seg.y*Seg.y ) < Radius) ||
					( ( Seg.x - Point.x)*( Seg.x - Point.x) + 
					( Seg.y - Point.y)*( Seg.y - Point.y) < Radius ) )
				{
					if( -Sign(temp) == sgnDist) 
						return false;
					else
						continue;
				}
				#endregion
				#region In the rest of Band

				//No se consideran las intersecciones en uno de los vertices ni coliniales
				Pts[0].x = R[ i+1 ].x - R[ S1 ].x;
				Pts[0].y = R[ i+1 ].y - R[ S1 ].y;

				Pts[1].x = R[  S1  ].x - R[ i ].x;
				Pts[1].y = R[  S1  ].y - R[ i ].y;

				Pts[2].x = R[ S1+1 ].x - R[ i ].x;
				Pts[2].y = R[ S1+1 ].y - R[ i ].y;

				if( ( SD(Zero,Point,Pts[1])*SD(Zero,Point,Pts[2]) == -1 ) &&
					( SD(Zero,Dif,Pts[1])*SD(Zero,Dif,Pts[0]) == 1 ) ) return false;						
				else 
					continue;

				#endregion
			}
			return true;
		}

		
		public LtnLine[] Parallel(int dist, out LtnLine R, out int cant_Result)
		{	
			LtnLine[] Result = null;
			cant_Result = 0;
			int i=0;

			R = new LtnLine();
			int[][] LinkToOriginal = new int[this.CantOfVertex()][];
			RPoint[] real_List = new RPoint[this.CantOfVertex()];

			/*
			 * R -------------- Poligonal paralela a distancia dist de la original 
			 * 				eliminando Segmentos que son un punto
			 * LinkToOriginal -- El elemento i indica el indice que tiene en la po-
			 * 				ligonal original el segmento correspondiente a i
			 * real_List ------- Lista de los verices R calculados en racionales para
			 * 				verificar su validez
			 */

			#region Calculo de R
			//Calculo la poligonal paralela sin tener en cuenta las 
			//intersecciones que puedan ocurrir

			LtnPoint3D Point_Aux = null;

			if( (Vertex[0].x == Vertex[this.CantOfVertex()-1].x) &&
				(Vertex[0].y == Vertex[this.CantOfVertex()-1].y) )
			{//la poligonal es cerrada
				LtnPoint3D[] Point = new LtnPoint3D[2];

				Point[0] = this.getVertex(this.CantOfVertex()-2).Clone();
				Point[1] = this.getVertex(1).Clone();

				/* 
				 *	Traslado el eje de coordenadas al punto al cual se le quiere hallar 
				 * el correspondiente en la paralela 
				 */

				for(System.Byte j=0; j<2; j++)
				{			
					Point[j].x -= Vertex[0].x;
					Point[j].y -= Vertex[0].y;
				}
				
				System.Int64 a1  = -Point[0].x;
				System.Int64 b1  = -Point[0].y;
				System.Int64 a2  =  Point[1].x;
				System.Int64 b2  =  Point[1].y;
				System.Int64 A   = (a1*a1 + b1*b1);
				System.Int64 B   = (a2*a2 + b2*b2);
				System.Int64 Den = ( a2*b1 - b2*a1 );

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
				x += Vertex[0].x;

				BigNumbers.BigRational y = ( d2*b1 - d1*b2 ) / Den;
				y += Vertex[0].y;

				// Lo aproximo a entero paratener la poligonal con vertices enteros
				LtnPoint3D V = this.Round(x,y,a1,b1,a2,b2,Math.Sign(dist));

				R.AddVertex(V);	
			
				//Actualizo el link a la original
				real_List[0] = new RPoint(x,y);
				LinkToOriginal[0] = new int[2];
				LinkToOriginal[0][0] = LinkToOriginal[0][1] = 0;
			}
			else
			{
				/* 
				 * La poligonal es abierta por lo que calculo los puntos que estan en la paralela 
				 * de los segmentos extremos a una distancia dist de los vertices extremos.
				 */
				
				LtnPoint3D[] Point = new LtnPoint3D[2];

				Point[0] = this.getVertex(1).Clone();
				Point[1] = this.getVertex(this.CantOfVertex()-2).Clone();

				//	Traslado el eje de coordenadas a los puntos extremos correspondientes.

				Point[0].x -= Vertex[0].x;
				Point[0].y -= Vertex[0].y;

				Point[1].x -= Vertex[this.CantOfVertex()-1].x;
				Point[1].y -= Vertex[this.CantOfVertex()-1].y;
				
				RPoint[] rTemp = new RPoint[2];
				
				double aux;
				System.Int64 Ent,Num;

				for( i = 0; i<2; i++)
				{
					int sg = -Math.Sign((int)(Math.Pow((double)-1,(double)i)));
					System.Int64 a1  = sg*Point[i].x;
					System.Int64 b1  = sg*Point[i].y; 
					System.Int64 A  = (a1*a1 + b1*b1);

					/*Calculo de la distancia aproximada del segmento*/
					aux = Math.Abs(dist)*Math.Sqrt(A);
					Ent = (System.Int64)Math.Floor( aux );
					aux -= Ent; 
					Num = (System.Int64)Math.Ceiling( aux *Decimals_Den );

					BigNumbers.BigRational d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
					
					BigRational x =  b1*d1/A;
					BigRational y = -a1*d1/A;

					rTemp[i] = new RPoint(x,y);					
					Point[i] = this.Round(rTemp[i].x,rTemp[i].y,a1,b1,Math.Sign(dist));
				}

				//Reposiciono el eje de cordenada para ambos resultados
				
					
				rTemp[0].x += Vertex[0].x;
				rTemp[0].y += Vertex[0].y;
				Point[0].x += Vertex[0].x;
				Point[0].y += Vertex[0].y;

				rTemp[1].x += Vertex[this.CantOfVertex()-1].x;
				rTemp[1].y += Vertex[this.CantOfVertex()-1].y;				
				Point[1].x += Vertex[this.CantOfVertex()-1].x;
				Point[1].y += Vertex[this.CantOfVertex()-1].y;

				//Actualizo el link a la original y la lista de los puntos racionales
				real_List[0] = rTemp[0];
				LinkToOriginal[0] = new int[2];
				LinkToOriginal[0][0] = LinkToOriginal[0][1] = 0;

				real_List[real_List.Length-1] = rTemp[1];

				LinkToOriginal[LinkToOriginal.Length-1] = new int[2];
				
				LinkToOriginal[LinkToOriginal.Length-1][0] = 
					LinkToOriginal[LinkToOriginal.Length-1][1] = this.CantOfVertex()-1;
				
				R.AddVertex(Point[0]);

				Point_Aux = Point[1];
			}

			/*
			 * Min es la minima x y y de todos los vertices y 
			 * Max es la maxima x y y de todos los vertices.   
			 */
			
			LtnPoint3D Min = new LtnPoint3D(),
				Max = new LtnPoint3D();

			if( Vertex[0].x > Vertex[1].x )
			{
				Min.x = Vertex[1].x;
				Max.x = Vertex[0].x;
			}
			else
			{
				Min.x = Vertex[0].x;
				Max.x = Vertex[1].x;
			}

			if( Vertex[0].y > Vertex[1].y )
			{
				Min.y = Vertex[1].y;
				Max.y = Vertex[0].y;
			}
			else
			{
				Min.y = Vertex[0].y;
				Max.y = Vertex[1].y;
			}

			for( i=0; i < cant-2; i++)
			{
				// voy calculando Min y Max mientras calculo la poligonal paralela

				if( Min.x > Vertex[i+2].x ) 
				{
					Min.x = Vertex[i+2].x;
				}
				else
				{
					if( Max.x < Vertex[i+2].x ) 
						Max.x = Vertex[i+2].x;
				}

				if( Min.y > Vertex[i+2].y ) 
				{
					Min.y = Vertex[i+2].y;
				}
				else
				{
					if( Max.y < Vertex[i+2].y ) 
						Max.y = Vertex[i+2].y;
				}

				LtnPoint3D[] Point = new LtnPoint3D[2];
				/* 
				 *	Traslado el eje de coordenadas al punto al cual se le quiere hallar 
				 * el correspondiente en la paralela 
				 */
				for(System.Byte j=0; j<3; j+=2)
				{
					Point[j/2] = Vertex[i+j].Clone();
					Point[j/2].x -= Vertex[i+1].x;
					Point[j/2].y -= Vertex[i+1].y;
				}
				
				// Calculo las distancias aproximada a cada uno de los dos segmentos involucrados (d1 y d2)
				System.Int64 a1  = -Point[0].x;
				System.Int64 b1  = -Point[0].y;
				System.Int64 a2  =  Point[1].x;
				System.Int64 b2  =  Point[1].y;
				System.Int64 A   = (a1*a1 + b1*b1);
				System.Int64 B   = (a2*a2 + b2*b2);
				System.Int64 Den = ( a2*b1 - b2*a1 );

				double aux = Math.Abs(dist)*Math.Sqrt(A);
				System.Int64 Ent = (System.Int64)Math.Floor( aux );
				aux -= Ent; 
				System.Int64 Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

				BigNumbers.BigRational d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/

				aux = Math.Abs(dist)*Math.Sqrt(B);
				Ent = (System.Int64)Math.Floor( aux );
				aux -= Ent; 
				Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

				BigNumbers.BigRational d2 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
				

				// x y y son las coordenadas del punto racional
				BigRational x = ( d2*a1 - d1*a2 ) / Den;
				x += Vertex[i+1].x;

				BigRational y = ( d2*b1 - d1*b2 ) / Den;
				y += Vertex[i+1].y;

				//Aproximo el punto a entero para adicionarlo a la poligonal paralela
				LtnPoint3D V = this.Round(x,y,a1,b1,a2,b2,(byte)Math.Sign(dist));

				if( ( V.x == R.getVertex(R.CantOfVertex()-1).x ) &&
					( V.y == R.getVertex(R.CantOfVertex()-1).y ) ) 
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
				R.AddVertex(V);
				real_List[ R.CantOfVertex()-1 ] = new RPoint(x,y);
				LinkToOriginal[R.CantOfVertex()-1] = new int[2];
				
				LinkToOriginal[R.CantOfVertex()-2][1] = 
					LinkToOriginal[R.CantOfVertex()-1][0] = 
					LinkToOriginal[R.CantOfVertex()-1][1] = i+1;
			}

			if( R.CantOfVertex() == 1 )	
			{
				Result = new LtnLine[1];
				Result[cant_Result++] = R;
				return Result;
			}

			if( (Vertex[0].x == Vertex[this.CantOfVertex()-1].x) &&
				(Vertex[0].y == Vertex[this.CantOfVertex()-1].y) )
			{//la poligonal es cerrada
				if( ( R.getVertex(0).x != R.getVertex(R.CantOfVertex()-1).x ) ||
					( R.getVertex(0).y != R.getVertex(R.CantOfVertex()-1).y ) ) 
				{
					LinkToOriginal[R.CantOfVertex()] = new int[2];

					/*
					 * Como la poligonal es cerrada adiciono el primer vertice
					 * al final de la poligonal y de la lista de los vertices
					 * racionales y actualizo el link a la original.
					 */ 
				  
					R.AddVertex(R.getVertex(0));

					real_List[R.CantOfVertex()-1] = real_List[0];
					LinkToOriginal[R.CantOfVertex()-1] = LinkToOriginal[0];
					LinkToOriginal[R.CantOfVertex()-2][1] = 0;
				}
			}
			else
			{//la poligonal es abierta
				if( ( R.getVertex(0).x != Point_Aux.x ) ||
					( R.getVertex(0).y != Point_Aux.y ) ) 
				{
					/*
					 * Como la poligonal es avierta adiciono el ultimo vertice 
					 * que lo tenia en Point_Aux a la Poligonal y arreglo la lista 
					 * de los verices racionales. Actualizo el link a la original.
					 */ 
					R.AddVertex(Point_Aux);
					real_List[R.CantOfVertex()-1] = real_List[real_List.Length-1];
					LinkToOriginal[R.CantOfVertex()-1] = LinkToOriginal[LinkToOriginal.Length-1];					
					LinkToOriginal[R.CantOfVertex()-2][1] = LinkToOriginal[R.CantOfVertex()-1][0];
				}
			}			
			#endregion
		
			/*
			 * Cuts ---- Lista de intersecciones de un segmento con todos los 
			 *			de la poligonal
			 * Valido -- Estado en que estamos en el analisis
			 */

			i=0;
			int real_List_Pos = 0;
			LtnPoint3D[] P=new LtnPoint3D[4]; 

			/*
			 * Closed me dice si la poligonal original es cerrada.
			 */
			bool Closed = ( ( getVertex( CantOfVertex()-1 ).x == getVertex(0).x ) &&
				            ( getVertex( CantOfVertex()-1 ).y == getVertex(0).y ) );
			
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
				P[0] = R.getVertex(  i  ).Clone();
				P[1] = R.getVertex( i+1 ).Clone();
				P[2] = Vertex[LinkToOriginal[ i ][ 0 ]].Clone();
				P[3] = Vertex[LinkToOriginal[ i ][ 1 ]].Clone();

				bool Change = false;
				while( !Good_Seg )
				{					
					/*
					 * En este ciclo eliminare los segmentos que son contrarios en
					 * la poligonal paralela que no forman un ciclo y aquellos 
					 * segmentos consecutivos que son coincidentes.
					 */ 
					#region segmentos contrarios
					if( !( ( Math.Sign( P[1].x - P[0].x ) * Math.Sign( P[3].x - P[2].x ) == -1 ) ||
						( Math.Sign( P[1].y - P[0].y ) * Math.Sign( P[3].y - P[2].y ) == -1 ) ) )
					{	
						//Si el segmento paralelo no es contrario continuo.
						Good_Seg = true;
					}
					else
					{
						/*
						 * El segmento paralelo es contrario al segmento original,
						 */ 

						int Last = -1, Next = -1;
						
						if( !( ( Closed ) || ( ( i >= 1 ) && ( i <= R.CantOfVertex() - 3 ) ) ) )
						{
							/*
							 * Si la poligonal no es cerrada y se esta analizando
							 * un de los dos segmentos extremos continuo porque los 
							 * extremos siempre son validos.
							 */ 
							Good_Seg = true;
						}
						else
						{
							/*
							 * Es cerrada o se esta analizando un segmento interior.
							 */
 
							/*
							 * Last es el indice del vertice anterior al inicio del 
							 * segmento que se esta analizando y
							 * Next es el indice del vertice siguiente al final del
							 * segmento que se esta analizando.
							 */ 
							
							Last = i - 1; 
							if( Last == -1 ) Last = R.CantOfVertex() - 2;

							Next = i + 2;
							if( Next ==  R.CantOfVertex() ) Next = 1;

							if( !( ( SD( Vertex[LinkToOriginal[Last][0]],P[2],P[3])*Math.Sign( dist )*
								SD( P[2],P[3],Vertex[LinkToOriginal[Next][0]])*Math.Sign( dist ) == -1 ) ) )
							{
								/*
								 * En este caso el segmento contrario forma un ciclo
								 * por lo que continuo el proceso.
								 */ 
								Change =true;
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
								LtnPoint3D[] Pt_Aux = new LtnPoint3D[4];
								Pt_Aux[0] =  Vertex[LinkToOriginal[Last][0]].Clone();
								Pt_Aux[1] =  P[2];
								Pt_Aux[2] =  P[3];
								Pt_Aux[3] =  Vertex[LinkToOriginal[Next][0]].Clone();

								System.Int64 a1  = ( Pt_Aux[1].x - Pt_Aux[0].x );
								System.Int64 b1  = ( Pt_Aux[1].y - Pt_Aux[0].y );
								System.Int64 a2  = ( Pt_Aux[3].x - Pt_Aux[2].x );
								System.Int64 b2  = ( Pt_Aux[3].y - Pt_Aux[2].y );
								System.Int64 A   = (a1*a1 + b1*b1);
								System.Int64 B   = (a2*a2 + b2*b2);
								System.Int64 Den = ( b2*a1 - a2*b1 );

								/*
								 * Calculo las distancias aproximadas de los 
								 * segmentos a intersectar.
								 */ 
								double aux = Math.Abs(dist)*Math.Sqrt(A);
								System.Int64 Ent = (System.Int64)Math.Floor( aux );
								aux -= Ent; 
								System.Int64 Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

								BigNumbers.BigRational d1 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/

								aux = Math.Abs(dist)*Math.Sqrt(B);
								Ent = (System.Int64)Math.Floor( aux );
								aux -= Ent; 
								Num = (System.Int64)Math.Ceiling( aux*Decimals_Den );

								BigNumbers.BigRational d2 = new BigNumbers.BigRational(Math.Sign( dist )*Ent, Math.Sign(dist)*Num, Decimals_Den);/*6cifras decimales*/
				

								Pt_Aux[0].x -= Pt_Aux[2].x;
								Pt_Aux[0].y -= Pt_Aux[2].y;

								/*
								 * CAlculo el punto de interseccion cuyas coordenadas
								 * son x y y.
								 */ 

								BigRational x  = ( a2*d1 - a1*d2 );
								x +=  Pt_Aux[0].y*a1*a2-Pt_Aux[0].x*b1*a2 ; 
								x /= Den;
								x += Pt_Aux[2].x;

								BigRational y = -( d2*b1 - d1*b2 ); 
								y += Pt_Aux[0].y*a1*b2 -  Pt_Aux[0].x*b1*b2;
								y /= Den;
								y += Pt_Aux[2].y;

								P[1] = this.Round(x,y,a1,b1,a2,b2,(byte)Math.Sign(dist));

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

								if( i + 2 == R.CantOfVertex() )
								{
									Same = ( (R.getVertex(1).x == P[1].x) && 
										(R.getVertex(1).y == P[1].y) );
								}
								else
								{
									Same = ( (R.getVertex(i+2).x == P[1].x) && 
										(R.getVertex(i+2).y == P[1].y) );
								}

								int Init,Step;

								if( Same ) 
								{
									R.DeleteVertex( i );
									Step = 2;
									Init = i;
								}
								else
								{
									R.Replace( i+1, P[1] );

									real_List[real_List_Pos].x = x;
									real_List[real_List_Pos].y = y;
									Step = 1;
									Init = i+1;
								}
								R.DeleteVertex( i );									

								
								for(int j = i; j < R.CantOfVertex(); j++) 
								{
									if( j >= Init )
										real_List[j] = real_List[j+Step];
									LinkToOriginal[j] = LinkToOriginal[j+Step];
								}

								LinkToOriginal[LinkToOriginal.Length-1] = null;
								if( Step == 2 ) 
									LinkToOriginal[LinkToOriginal.Length-2] = null;

								/*	
								 * Si despues de haber hecho el cambio i es el ultimo
								 * vertice de la poligonal o i era 0 entonces como la
								 * poligonal tienia que ser cerrada para que se diera
								 * ese caso, debo actualizar el extremo opuesto de la 
								 * poligonal para que siga estando cerrada.
								 */ 
								if( i == 0 ) 
								{
									R.Replace( R.CantOfVertex()-1, P[1] );

									real_List[R.CantOfVertex()-1].x = x;
									real_List[R.CantOfVertex()-1].y = y;									
								}
																
								if( i == R.CantOfVertex()-1 ) 									
								{
									R.Replace( 0, P[1]); 	

									real_List[0].x = x;
									real_List[0].y = y;									
								}
								
								/*
								 * Los segmentos extremos de una poligonal abierta no pueden ser
								 * invertidos. Por lo que cualquier caso extremo me garantiza que 
								 * la poligonal es cerrada.
								 */ 
								
								/* 
								 * Para que el next fuera 0 Direction tendria que valer -1 y la
								 * poligonal seria abierta lo cual ya vimos que no era posible.
								 */ 

								Next = i + 1;
								if( i ==  R.CantOfVertex()-1 ) 
								{
									Good_Seg = true;
									continue;
								}

								/* 
								 * Para que el last fuera mayor que el ultimo Pos tendria que ser 
								 * el ultimo y Direction valer -1 por lo que la poligonal seria 
								 * abierta lo cual ya vimos que no era posible.
								 */ 
								
								P[0] = R.getVertex( i  ).Clone();
								P[1] = R.getVertex( Next ).Clone();
								P[2] = Vertex[LinkToOriginal[ i ][ 0 ]].Clone();
								P[3] = Vertex[LinkToOriginal[ i ][ 1 ]].Clone();								
							}
						}
					}
					#endregion
				}
				LtnPoint3D Pts = null;
						
				if( ( Closed ) || ( i != 0 ) )
				{
					if( i == 0 )
					{
						Pts = R.getVertex( R.CantOfVertex()-2 ).Clone();
					}
					else
					{
						Pts = R.getVertex( i-1 ).Clone();
					}

					if( SD(Pts,P[0],P[1]) == 0 )
					{
						/*
						 * Si son coincidentes se pierde la arista. En caso de que se quiera 
						 * dar en la solucion debe cambiarse esto.
						 */ 
						if( i == 0 )
						{
							/* Si i es 0 entonces tengo que eliminar los dos extremos
							 * de la poligonal manteniendola cerrada por lo que remplazo
							 * el extremo inicial por el penultimo vertice y elimino el 
							 * ultimo. Actualizo consecuentemente la lista de los valores
							 * racionales y el link a la original.
							 */ 
							R.Replace(0,R.getVertex(R.CantOfVertex()-2));
							R.DeleteVertex(R.CantOfVertex()-1);

							real_List[0] = real_List[R.CantOfVertex()-1];
							real_List[R.CantOfVertex()] = null;

							if( ( Math.Sign( P[1].x - P[0].x ) !=  Math.Sign( P[1].x - Pts.x) ) ||
								( Math.Sign( P[1].y - P[0].y ) !=  Math.Sign( P[1].y - Pts.y) ) )
							{
								/*
								 * El punto i+1 es el que esta en el medio de los tres.
								 * Por lo que se deja el link del segmento i-1.
								 */ 
								LinkToOriginal[0] = LinkToOriginal[R.CantOfVertex()-1];
							}
							else
							{
								if( ( Math.Sign( Pts.x - P[0].x ) !=  Math.Sign( Pts.x - P[1].x) ) ||
									( Math.Sign( Pts.y - P[0].y ) !=  Math.Sign( Pts.y - P[1].y) ) )
								{
									/*
									 * El punto i-1 es el que esta en el medio de los tres.
									 * Por lo que se deja el link del segmento i.
									 */ 
									LinkToOriginal[R.CantOfVertex()-1] = LinkToOriginal[0];
								}
								else
								{
									if( Change )
									{
										LinkToOriginal[R.CantOfVertex()-1] = LinkToOriginal[0];
									}
									else
									{
										LinkToOriginal[0] = LinkToOriginal[R.CantOfVertex()-1];
									}

								}
							}
							LinkToOriginal[R.CantOfVertex()] = null;
						}
						else
						{
							/* Sino elimino el vetice i y actualizo consecuentemente 
							 * la lista de los valores racionales y el link a la 
							 * original.
							 */ 
							R.DeleteVertex(i);
							int[] aux = LinkToOriginal[i];

							for(int j = i; j <= R.CantOfVertex()-1; j++) 
							{
								LinkToOriginal[j] = LinkToOriginal[j+1];
								real_List[j] = real_List[j+1];
							}
							
							if( ( Math.Sign( Pts.x - P[0].x ) !=  Math.Sign( Pts.x - P[1].x) ) ||
								( Math.Sign( Pts.y - P[0].y ) !=  Math.Sign( Pts.y - P[1].y) ) ||
								( Change ) )
							{
								/*
								 * El punto i-1 es el que esta en el medio de los tres.
								 * Por lo que se deja el link del segmento i.
								 */ 
								LinkToOriginal[i-1] = aux;
							}
							
							real_List[R.CantOfVertex()] = null;
							LinkToOriginal[R.CantOfVertex()] = null;
						}
						
						P[0] = P[1];
						if( i < R.CantOfVertex() - 1 )
                            P[1] = R.getVertex( i+1 ).Clone();
					}					
				}
				i++;
				real_List_Pos++;
			}	
			Result = new LtnLine[R.CantOfVertex()];
			cant_Result = 0;

			Cross[][] Intersections = FindAllIntersections( real_List,R.CantOfVertex(),Math.Sign(dist));

			bool Finish = false;

			int Seg = 0;
			i = 0;
			int sgn = Math.Sign( dist );

			for(Seg=0; Seg<R.CantOfVertex()-1; Seg++)
			{
				/*
				 * Para cada uno de los segmentos que esten invertidos
				 * les marco como visitados todos sus intervalos ya que
				 * ninguno podria formar una seccion valida.
				 */ 
				if( ( Sign( real_List[Seg+1].x - real_List[Seg].x ) * 
					  Math.Sign( Vertex[LinkToOriginal[ Seg ][ 1 ]].x - Vertex[LinkToOriginal[ Seg ][ 0 ]].x ) == -1 ) ||
					( Sign( real_List[Seg+1].y - real_List[Seg].y ) * 
					  Math.Sign( Vertex[LinkToOriginal[ Seg ][ 1 ]].y - Vertex[LinkToOriginal[ Seg ][ 0 ]].y ) == -1 ) ) 
				{
					for( i = 0;i<Intersections[Seg].Length;i++)
					{
						if(Intersections[Seg][i].Segment23 == Seg) 
							Intersections[Seg][i].flag23 = true;
						else
							Intersections[Seg][i].flag01 = true;
					}					
				}
			}

			RPoint Pto = null;
			int init=0;
			for(Seg=0; Seg<R.CantOfVertex()-1; Seg++)
			{
				/*
				 * Para cada una de las intersecciones que sean coincidentes 
				 * invertidas marco como visitados todos los intervalos afectados
				 * ya que ninguno podria formar una seccion valida.
				 */ 
				i = 0;
				init = 0;
				Pto = null;
				RPoint End = null;

				while( i<Intersections[Seg].Length )
				{
					Pto = real_List[Seg];

					while( ( i < Intersections[Seg].Length ) &&
						( Intersections[Seg][i].Cut23 == null ) ) 
					{
						if( ( Intersections[Seg][i].Cut01.x != Pto.x ) ||
							( Intersections[Seg][i].Cut01.y != Pto.y ) )
						{
							init = i;
							Pto = Intersections[Seg][i].Cut01;
						}
						i++;
					}
					if( i == Intersections[Seg].Length ) break;

					if( Intersections[Seg][i].Segment01 == Seg )
					{
						End = Intersections[Seg][i].Cut23;
					}
					else
					{
						End = Intersections[Seg][i].Cut01;						
					}

					i = init;

					while( ( i < Intersections[Seg].Length ) &&
						   ( ( Sign( Intersections[Seg][i].Cut01.x - real_List[Seg].x ) *
						       Sign( Intersections[Seg][i].Cut01.x - End.x ) < 1 ) &&
						     ( Sign( Intersections[Seg][i].Cut01.y - real_List[Seg].y ) *
						       Sign( Intersections[Seg][i].Cut01.y - End.y ) < 1 ) ) )
					{
						if( Intersections[Seg][i].Cut23 != null )
						{
							RPoint End_Aux = null;
							
							if( Intersections[Seg][i].Segment01 == Seg )
							{
								Intersections[Seg][i].flag01 = true;
								End_Aux = Intersections[Seg][i].Cut23;
							}
							else
							{
								Intersections[Seg][i].flag23 = true;
								End_Aux = Intersections[Seg][i].Cut01;
							}

							if( ( ( Sign(End.x - real_List[Seg].x ) ) * ( Sign( End.x - End_Aux.x ) ) == -1 ) &&
								( ( Sign(End.y - real_List[Seg].y ) ) * ( Sign( End.y - End_Aux.y ) ) == -1 ) ) 
							{
								End = End_Aux;
							}							
						}
						else
						{
							if( ( Intersections[Seg][i].Cut01.x == End.x ) &&
								( Intersections[Seg][i].Cut01.y == End.y ) )  
							{
								End = null;
								init = i;
								break;
							}
							
							if( Intersections[Seg][i].Segment01 == Seg )
								Intersections[Seg][i].flag01 = true;
							else
								Intersections[Seg][i].flag23 = true;							
						}
						i++;
					}
				}
			}

			Seg = 0;
			i=0;
			Pto = real_List[Seg];

			do
			{
				while( ( i < Intersections[Seg].Length ) &&
					   ( ( ( Intersections[Seg][i].Segment23 == Seg ) && ( Intersections[Seg][i].flag23 ) ) ||
					     ( ( Intersections[Seg][i].Segment01 == Seg ) && ( Intersections[Seg][i].flag01 ) ) ) )
				{
					i++;
				}

				if( i < Intersections[Seg].Length )
				{
					Pto = Intersections[Seg][i].Cut01;
				}
				else
				{
					Seg++;
					i=0;
					if(Seg == R.CantOfVertex()-1)
					{
						Finish = true;						
					}
					continue;
				}
				init = i;
	
				while( ( i < Intersections[Seg].Length ) &&
					   ( ( ( Intersections[Seg][i].Cut01.x == Pto.x ) &&
					       ( Intersections[Seg][i].Cut01.y == Pto.y ) ) ||
						 ( Intersections[Seg][i].Cut23 != null ) ) )
				{
					if( ( Intersections[Seg][i].Cut23 != null ) &&
						( Intersections[Seg][i].Segment23 == Seg ) &&
					    ( ( Intersections[Seg][i].Cut23.x != Pto.x ) ||
						  ( Intersections[Seg][i].Cut23.y != Pto.y ) ) ) break;
					i++;
				}				
								
				if( ( i >= Intersections[Seg].Length ) ||
					( ( Intersections[Seg][i].dir01 == 3 ) &&
					  ( Intersections[Seg][i].flag01 ) ) )
				{
					for( i = init; i<Intersections[Seg].Length;i++)
					{
						if( Intersections[Seg][i].Segment23 == Seg ) 
							Intersections[Seg][i].flag23 = true;
						else
							Intersections[Seg][i].flag01 = true;
					}
					Seg++;
					i=0;
					if(Seg == R.CantOfVertex()-1)
					{
						Finish = true;
						continue;
					}
					if( ( Intersections[Seg][i].Cut23 != null ) &&
						( Intersections[Seg][i].Segment23 == Seg ) )
						Pto = Intersections[Seg][i].Cut23;  
					else
						Pto = Intersections[Seg][i].Cut01;  
					continue;
				}
				
				if( ( ( Intersections[Seg][i].Segment23 == Seg ) && ( Intersections[Seg][i].flag01 ) ) ||
					( ( Intersections[Seg][i].Segment01 == Seg ) && ( Intersections[Seg][i].flag23 ) ) )
				{
					for( init = init; init<i;init++)
					{
						if( Intersections[Seg][init].Segment23 == Seg ) 
							Intersections[Seg][init].flag23 = true;
						else
							Intersections[Seg][init].flag01 = true;
					}

                    if( ( Intersections[Seg][i].Cut23 != null ) &&
						( Intersections[Seg][i].Segment23 == Seg ) ) 
						Pto = Intersections[Seg][i].Cut23;  
					else
						Pto = Intersections[Seg][i].Cut01;                       					
					continue;
				}			

				if( Intersections[Seg][init].Segment23 == Seg )
					Intersections[Seg][init].flag23 = true;
				else
					Intersections[Seg][init].flag01 = true;
				
				if( ( (Intersections[Seg][i].Segment01 == Seg) && 
					  ( (Intersections[Seg][i].dir01 == 3) || 
					    (Intersections[Seg][i].dir01 == Math.Sign(dist) ) ) ) ||
					( (Intersections[Seg][i].Segment23 == Seg) && 
					  ( (Intersections[Seg][i].dir23 == 3) || 
					    (Intersections[Seg][i].dir23 == Math.Sign(dist) ) ) ) )
				{
					LtnLine Section = FindValidSection(Pto,real_List,R.CantOfVertex(),Intersections,Seg,i,Math.Sign(dist));
				
					if( Section != null) Result[cant_Result++] = Section;
				}
				
				if( Intersections[Seg][i].dir01 == 3 )
				{	
					Intersections[Seg][i].flag01 = true;

					for( i = i+1; i<Intersections[Seg].Length;i++)
					{
						if( Intersections[Seg][i].Segment23 == Seg ) 
							Intersections[Seg][i].flag23 = true;
						else
							Intersections[Seg][i].flag01 = true;
					}

					Seg++;
					i=0;
					if(Seg == R.CantOfVertex()-1)
					{
						Finish = true;
						continue;
					}					
				}				
				if( ( Intersections[Seg][i].Cut23 != null ) &&
					( Intersections[Seg][i].Segment23 == Seg ) )
					Pto = Intersections[Seg][i].Cut23;  
				else
					Pto = Intersections[Seg][i].Cut01;    
			}while( !Finish );
			#region Busqueda de un primer punto valido

//			LtnLine Poligonal_Aux;
//			Seg = -1;
//			int Cut = -1;
//
//			/*
//			 * Seg --- Indice del vertice donde comienza el segmento en el cual 
//			 *		se encontro un punto valido
//			 * Cut --- Indica el indice que tiene la interseccion que fue valida
//			 */
//			
//			while( (Seg < R.CantOfVertex() - 2) && (!Valido) )
//			{//OJO Se debera comenzar por el punto de comienzo que pasen como parametro
//				;
//				R.FindIntersection(real_List,++Seg,out Cuts);
//				if (Cuts != null)
//				{
//					i = 0;
//					int seg = 0;
//					if(Cuts[i].Segment01 == i)
//						seg = Cuts[i].Segment23;
//					else
//						seg = Cuts[i].Segment01;
//
//					while( ( i < Cuts.Length ) && 
//						( Valido = !this.Validate(real_List,R.CantOfVertex(),LinkToOriginal,Cuts[i].Cut,Seg,seg ,dist) ) ) 
//					{
//						i++;
//						if(Cuts[i].Segment01 == i)
//							seg = Cuts[i].Segment23;
//						else
//							seg = Cuts[i].Segment01;
//					}
//
//					Valido = !Valido;
//					if( Valido ) 
//					{
//						Cut = i;	
//						if( Cut == 0 )
//						{
//							Valido = this.Validate(real_List,R.CantOfVertex(),LinkToOriginal,real_List[Seg],Seg,-1,dist);
//							if( Valido ) 
//							{
//								Cut = -1;	
//								Cuts = null;
//							}
//							else 
//							{
//								if(Cuts[i].Segment01 == i+1)
//									seg = Cuts[i+1].Segment23;
//								else
//									seg = Cuts[i+1].Segment01;
//
//								if( ( ( i < Cuts.Length-1) &&
//									  ( !this.Validate(real_List,R.CantOfVertex(),LinkToOriginal,Cuts[i+1].Cut,Seg,seg ,dist) ) ) ||
//									( ( i == Cuts.Length-1) &&
//									  ( !this.Validate(real_List,R.CantOfVertex(),LinkToOriginal,real_List[Seg+1],Seg+1,-1,dist) ) ) )
//								{
//									/*
//									 * Si el punto valido es un punto aislado
//									 * retorno una poligonal con ese unico
//									 * punto.
//									 */ 
//									Poligonal_Aux = new LtnLine();
//									Poligonal_Aux.AddVertex( new LtnPoint3D(Cuts[i].Cut.x.entero,Cuts[i].Cut.y.entero,0));
//									Result[cant_Result++] = Poligonal_Aux;
//									return Result;
//								}
//								Valido = true;
//							}
//						}
//						else
//						{
//							if(Cuts[i].Segment01 == i+1)
//								seg = Cuts[i+1].Segment23;
//							else
//								seg = Cuts[i+1].Segment01;
//
//							if( ( ( i < Cuts.Length-1) &&
//								( !this.Validate(real_List,R.CantOfVertex(),LinkToOriginal,Cuts[i+1].Cut,Seg,seg,dist) ) ) ||
//								( ( i == Cuts.Length-1) &&
//								( !this.Validate(real_List,R.CantOfVertex(),LinkToOriginal,real_List[Seg+1],Seg+1,-1,dist) ) ) )
//							{
//								/*
//								 * Si el punto valido es un punto aislado
//								 * retorno una poligonal con ese unico
//								 * punto.
//								 */ 
//								Poligonal_Aux = new LtnLine();
//								Poligonal_Aux.AddVertex( new LtnPoint3D(Cuts[i].Cut.x.entero,Cuts[i].Cut.y.entero,0));
//								Result[cant_Result++] = Poligonal_Aux;
//								return Result;
//							}
//						}
//					}
//				}
//				else
//				{
//					Valido = Validate(real_List,R.CantOfVertex(),LinkToOriginal,real_List[Seg],Seg,-1,dist);
//
//					if( Valido)
//					{
//						if( ( ( Seg < R.CantOfVertex()-1 ) &&
//							( !this.Validate(real_List,R.CantOfVertex(),LinkToOriginal,real_List[Seg+1],Seg+1,-1,dist) ) ) ||
//							( ( Seg == 0 ) && ( Closed ) &&
//							( !this.Validate(real_List,R.CantOfVertex(),LinkToOriginal,real_List[R.CantOfVertex()-2],R.CantOfVertex()-2,-1,dist) ) ) )
//						{
//							/*
//							 * Si el punto valido es un punto aislado
//							 * retorno una poligonal con ese unico
//							 * punto.
//							 */ 
//							Poligonal_Aux = new LtnLine();
//							Poligonal_Aux.AddVertex( R.getVertex(Seg) );
//							Result[cant_Result++] = Poligonal_Aux;
//							return Result;
//						}
//					}
//				}
//			}
			#endregion
//		
//			if(!Valido)
//			{
//				/*
//				 * La poligonal no tiene ningun vertice valido y ninguna 
//				 * de las intersecciones entre sus segmentos es valida
//				 */
//
//				// OJO ver hasta donde es valido
//				
//				return null;
//			}
			
			#region Calculo de la paralela final
//
//			RPoint InitC = null;
//
//			/*
//			 * InitC --- Valor de la interseccion donde se comenzo el analisis
//			 *			null si se comenzo de un vertice.
//			 * Dir ----- Direccion del segmento donde se comenzo el analisis
//			 */
//			
//			Poligonal_Aux = new LtnLine();
//			RPoint[] List_Aux = new RPoint[real_List.Length];
//			int cant_List_Aux = 0;
//			
//			Point Dir = new Point( ( Sign( real_List[Seg+1].x -
//				real_List[ Seg ].x ) ),
//				( Sign( real_List[Seg+1].y - 
//				real_List[ Seg ].y ) ) );	
//			
//			if( Cuts != null ) 
//			{//Si hubo interseccion
//				if( ( Cuts[Cut].Cut.x == real_List[Seg+1].x ) &&
//					( Cuts[Cut].Cut.y == real_List[Seg+1].y ) )
//				{
//					/*
//					 * Si la interseccion es igual al extremo final del segmento, 
//					 * paso al siguiente para esta en el extremo inicial.
//					 */ 
//					Seg++;
//				}
//				else
//				{
//					if( ( Cuts[Cut].Cut.x != real_List[Seg].x ) ||
//						( Cuts[Cut].Cut.y != real_List[Seg].y ) )				
//						/*
//						 * Si la interseccion no es igual al extremo inicial del
//						 * segmento guardo en InitC el valor del punto de la interseccion.
//						 */ 
//						InitC = Cuts[Cut].Cut;
//				}				
//			}
//			int Pos=Seg;
//
//			/*
//			 * Comienzo una oligonal insertandole ese primer punto InitC o el vertice
//			 * inicial del segmento si InitC es null
//			 */ 
//			if( InitC == null )
//			{
//				Poligonal_Aux.AddVertex(R.getVertex(Seg));
//				List_Aux[cant_List_Aux++] = real_List[Seg];				
//			}
//			else
//			{
//				Poligonal_Aux.AddVertex(new LtnPoint3D(InitC.x.entero,InitC.y.entero,0));
//				List_Aux[cant_List_Aux++] = InitC;
//			}
//
//			P = new LtnPoint3D[4];
//			P[1] = Poligonal_Aux.getVertex(0);	
//		
//			do
//			{				
//				P[0] = P[1];
//				P[1] = R.getVertex(Pos+1);
//				P[2] = Vertex[LinkToOriginal[ Pos ][0]];
//				P[3] = Vertex[LinkToOriginal[ Pos ][1]];
//				
//				Poligonal_Aux.AddVertex( P[1] );
//				List_Aux[ cant_List_Aux++ ] = real_List[Pos + 1];
//				
//				/*
//				 * Hallo las intersecciones del segmento que se esta analizando con el
//				 * resto de los segmentos de la poligonal
//				 */ 
//				Poligonal_Aux.FindIntersection(List_Aux,Poligonal_Aux.CantOfVertex()-2, out Cuts);
//
//					
//				if(Cuts != null)
//				{
//					/*
//					 * Como siempre empiezo por el vertice inicial de un segmento 
//					 * valido, para quedarme con la primera seccion valida elimino 
//					 * todos los lazos que se vayan formando salvo si el cruce ocurre 
//					 * en el vertice 0. Ya que en ese caso estariamos en presencia de 
//					 * la seccion valida.
//					 */ 
//
//					for( i = 0; i < Cuts.Length; i++)
//					{
//						if( Cuts[i].type == Cross.Type.COINCIDENT) continue;
//						if( Cuts[i].Segment >= Poligonal_Aux.CantOfVertex()-1) 
//							/*
//							 * Si esto ocurre significa que el segmento con el
//							 * cual se formaba el lazo ya habia sido eliminado
//							 * en un lazo anterior por lo que paso a la 
//							 * interseccion siguiente.
//							 */
//							continue;
//
//						if( ( Cuts[i].Segment == 0 ) &&
//							( Cuts[i].Cut.x == List_Aux[0].x) && 
//							( Cuts[i].Cut.y == List_Aux[0].y) )
//						{
//							/*
//							 * Si esto ocurre es que el lazo formado es la seccion
//							 * valida por lo que la retorno.
//							 */ 
//							Poligonal_Aux.Replace(Poligonal_Aux.CantOfVertex()-1,Poligonal_Aux.getVertex(0));
//							return Poligonal_Aux;
//						}
//
//						/*
//						 * Si llego aqui es que debo eliminar el lazo. Por tanto elimino
//						 * el lazo y actualizo la lista de los vertices racionales.
//						 */ 
//						
//						int Inc = 0;
//						if( ( Cuts[i].type == Cross.Type.VERTEX ) &&
//							( Cuts[i].Cut.x == List_Aux[Cuts[i].Segment+1].x ) &&
//							( Cuts[i].Cut.y == List_Aux[Cuts[i].Segment+1].y ) &&
//							( !( ( Cuts[i].Cut.x == List_Aux[cant_List_Aux-1].x ) &&
//							( Cuts[i].Cut.y == List_Aux[cant_List_Aux-1].y ) ) ) )
//						{
//							Inc = 1;
//						}
//
//						int Total =Poligonal_Aux.CantOfVertex()-Cuts[i].Segment-2-Inc;
//						
//						
//						
//						for(int j=1; j < Total; j++)
//						{
//							Poligonal_Aux.DeleteVertex(Poligonal_Aux.CantOfVertex()-2);
//						}
//
//						if( Cuts[i].type == Cross.Type.VERTEX )
//						{
//                            							
//							Poligonal_Aux.DeleteVertex(Poligonal_Aux.CantOfVertex()-2);
//							List_Aux[Cuts[i].Segment+1+Inc] = List_Aux[ cant_List_Aux-1];
//						}
//						else
//						{
//							Poligonal_Aux.Replace(Poligonal_Aux.CantOfVertex()-2,
//								new LtnPoint3D( Cuts[i].Cut.x.entero,
//								Cuts[i].Cut.y.entero, 0));
//						
//							List_Aux[Cuts[i].Segment+1] = Cuts[i].Cut;
//							List_Aux[Cuts[i].Segment+2] = List_Aux[ cant_List_Aux-1];							
//						}
//						cant_List_Aux = Poligonal_Aux.CantOfVertex();
//					}					
//				}
//				Pos++;
//				if( (Closed) && (Pos == R.CantOfVertex()-1 ) ) Pos = 0;
//			}
//			while( ( ( Closed ) && ( Pos != Seg ) ) ||
//				( ( !Closed ) && Pos != R.CantOfVertex()-1 ) );
//            	
			#endregion
//
//			return Poligonal_Aux;
			 return Result;

		}
		
		
	
		#endregion
		
		#endregion

		#region Atributos
		// ATRIBUTOS

		/// <summary>
		/// Lista de vertices de la poligonal
		/// </summary>
		private LtnPoint3D[] Vertex;
		/// <summary>
		/// cantidad de vertices de la poligonal
		/// </summary>
		private int cant;		

		#endregion

		
		
		 

		
	}
}
