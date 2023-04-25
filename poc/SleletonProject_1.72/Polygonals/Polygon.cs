using System;
using System.Collections;
using BorradorTesis;
using BigNumbers;
using BorradorTesis.Plane_Sweep;
using BorradorTesis.Plane_Sweep.GrowthStructures;
using BorradorTesis.Plane_Sweep.Nodes;
using Structures;

namespace BorradorTesis.Polygonals
{
	/// <summary>
	/// Descripción breve de Polygon.
	/// </summary>
	public class Polygon : Polygonal
	{		
		/**/
		protected bool[] adversed;		
		/**/
		public Polygon(RPoint[] vertices):base(vertices)
		{
			
		}			
		/**/
		public new Polygon Parallel(short dist)
		{			
			RPoint[] _vertparalelos = new RPoint[this.cantvert];			
			for(uint i = 0; i < this.cantvert; i++)
			{
				RPoint _ad1 = this.vertices[(i - 1 + this.cantvert) % this.cantvert];
				RPoint _ad2 = this.vertices[(i+1) % this.cantvert];
				RPoint _pt = this.vertices[i];
 
				_ad1 = _ad1 - _pt;
				_ad2 = _ad2 - _pt;

				long _a1 = -(_ad1.XToLong);
				long _b1 = -(_ad1.YToLong);
				long _a2 = _ad2.XToLong;
				long _b2 = _ad2.YToLong;

				long _Den = (_a2 * _b1 - _b2 * _a1); 

				long _D = ((_a1 * _a1) + (_b1 * _b1));
				double _rD = Math.Abs(dist) * Math.Sqrt(_D);
				long _ent = (long)Math.Floor(_rD);
				_rD -= _ent;

				long _num = (long)Math.Ceiling(_rD);
 
				BigRational d1 = new BigRational(Math.Sign(dist) * _ent, 
					Math.Sign(dist) * _num, DECIMAL_DEN);

				if(_Den == 0)
				{
				
					/*existen infinitas soluciones de la interseccion de los segmenteos que 
					 * unen a pt(un punto en medio de un segmento)*/
					/*Ojo los signos estan intercambiados en la formula para que esten de 
					 * acuerdo con el signo de la distancia*/
					
					BigRational x = -_b1 * d1 / _D + _a1;
					x += (_pt.X - _a1);
					BigRational y = +_a1 * d1 / _D + _b1;
					y += (_pt.Y - _b1);				
					_vertparalelos[i] = new RPoint(x, y);
					continue;
				}
			
				_D = ((_a2 * _a2) + (_b2 * _b2));
				_rD = Math.Abs(dist) * Math.Sqrt(_D);
				_ent = (long)Math.Floor(_rD);
				_rD -= _ent;

				_num = (long)Math.Ceiling(_rD);

				BigRational d2 = new BigRational(Math.Sign(dist) * _ent, 
					Math.Sign(dist) * _num, DECIMAL_DEN);

				BigNumbers.BigRational _x = ( d2*_a1 - d1*_a2 ) / _Den;
				_x += _pt.X;

				BigNumbers.BigRational _y = ( d2*_b1 - d1*_b2 ) / _Den;
				_y += _pt.Y;
			
				_vertparalelos[i] = new RPoint(_x, _y);	
			}
			
			Polygon _result = new Polygon(_vertparalelos);

			#region Segmentos contrarios

			bool[] _adversed = new bool[this.cantvert];	
			Segment _os, _ps;		

			for(uint u = 0; u < this.cantvert; u++)
			{
				_os = new Segment(this.vertices[u], 
					this.vertices[(u + 1) % this.cantvert]);
				_ps = new Segment(_vertparalelos[u], 
					_vertparalelos[(u + 1) % this.cantvert]);
				
				if(_os.Contrario(_ps))
					_adversed[u] = true;					
			}

			#endregion
			
			return new Polygon(_vertparalelos, _adversed);
		}
		/**/
		private Polygon(RPoint[] vertex, bool[] adverse)
		{
			this.vertices = vertex;
			this.cantvert = (uint)vertex.Length;
			this.adversed = adverse;
		}
		/**/
		public override void Pinta(System.Drawing.Graphics g, System.Drawing.Pen p, System.Drawing.Pen adveP, int height)
		{
			for(uint u = 0; u < this.cantvert; u++)
			{				
				if(this.adversed != null && this.adversed[u])
				{
					g.DrawLine(adveP, this.vertices[u].XToLong,
						height - this.vertices[u].YToLong,
						this.vertices[(u + 1) % this.cantvert].XToLong,
						height - this.vertices[(u + 1) % this.cantvert].YToLong);
				}
				else
				{
					g.DrawLine(p, this.vertices[u].XToLong,
						height - this.vertices[u].YToLong,
						this.vertices[(u + 1) % this.cantvert].XToLong,
						height - this.vertices[(u + 1) % this.cantvert].YToLong);
				}
			}
		}

		/**/
		public override Segment[] Segments
		{
			get
			{
				Segment[] _result = new Segment[this.cantvert];
				for(uint  u = 0; u < this.cantvert; u ++)
				{
					_result[u] = new Segment
						(this.vertices[u], this.vertices[(u + 1) % this.cantvert]);
				}
				return _result;
			}
		}
		
		/**/
		public override object Clone()
		{			
			RPoint[] _vert = new RPoint[this.cantvert];
			for(uint u = 0; u < this.cantvert; u++)
			{
				_vert[u] = (RPoint)this.vertices[u].Clone();
			}			
			Polygon _result = new Polygon(_vert);
			if(this.adversed != null)
			{
				_result.adversed = (bool[])this.adversed.Clone();
			}
			return _result;
		}

		/**/
		public bool[] AdversedSegments
		{
			get
			{
				return this.adversed;
			}
		}
	
		/**/
		public Polygon SegmentAdverseOut(Polygon original, bool bystep)
		{	
			if(this.adversed == null)
			{
				return this;
			}
			Segment[] seg = this.Segments;
			return RemoveSegment(seg, 
				(bool[])this.adversed.Clone(), original.Segments, bystep, seg);						
			
		}
						
		/**/		
		private Polygon(Segment[] seg, bool[] ad):this(seg)
		{			
			ArrayList _adversed = new ArrayList(); 
			for(uint u = 0; u < seg.Length; u++)
			{
				if(!seg[u].Empty)
				{					
					_adversed.Add(ad[u]);
				}
			}			
			this.adversed = new bool[this.cantvert];

			for(int i = 0; i < this.cantvert; i++)
			{				
				this.adversed[i] = (bool)_adversed[i];
			}
			uint t;
			for(t = 0; t < this.cantvert && !this.adversed[t]; t++);
			if(t >= this.cantvert)
			{
				this.adversed = null;
			}
		}
		/**/
		private Polygon(Segment[] seg)
		{
			ArrayList _segments = new ArrayList();
			for(uint u = 0; u < seg.Length; u++)
			{
				if(!seg[u].Empty)
				{
					_segments.Add(seg[u]);
				}
			}
			this.vertices = new RPoint[_segments.Count];
			this.cantvert = (uint)this.vertices.Length;				

			for(int i = 0; i < cantvert; i++)
			{
				this.vertices[i] = ((Segment)_segments[i]).Inicio;
			}
		}
		/**/
		private static bool CompareRelativePosition(Segment[] seg, Segment[] orig, uint i, uint j, bool[] ad)
		{
			if(seg[j].Turn(seg[i].Inicio) != orig[j].Turn(orig[i].Inicio) && 
				! seg[i].Puntual)
			{												
				ad[i] = true;
			}			
			if(seg[i].Turn(seg[j].Fin) != orig[i].Turn(orig[j].Fin) && 
				! seg[j].Puntual)
			{
				ad[j] = true;
			}
			return ad[i] || ad[j];
		}
		/**/
		private Polygon(ArrayList seg)
		{			
			this.vertices = new RPoint[seg.Count];
			this.cantvert = (uint)this.vertices.Length;				

			for(int i = 0; i < cantvert; i++)
			{
				this.vertices[i] = ((Segment)seg[i]).Inicio;
			}
		}
		
		/**/
		private Polygon(ArrayList seg, ArrayList ad):this(seg)
		{
			if(ad.Count > 0)
			{
				uint u;				
				for(u = 0; u < this.cantvert && !this.adversed[u]; u++);
				if(u < this.cantvert)
				{
					this.adversed = new bool[ad.Count];
					for(int i = 0; i < this.cantvert; i++)
					{
						this.adversed[i] = (bool)ad[i];
					}
				}				
			}
		}
		/**/
		private RPoint ParallelPoint(uint adyacentPt1, uint point, uint adyacentPt2, short dist)
		{
			RPoint _ad1 = this.vertices[adyacentPt1];
			RPoint _ad2 = this.vertices[adyacentPt2];
			RPoint _pt = this.vertices[point];
 
			_ad1 = _ad1 - _pt;
			_ad2 = _ad2 - _pt;

			long _a1 = -(_ad1.XToLong);
			long _b1 = -(_ad1.YToLong);
			long _a2 = _ad2.XToLong;
			long _b2 = _ad2.YToLong;

			long _Den = (_a2 * _b1 - _b2 * _a1); 

			long _D = ((_a1 * _a1) + (_b1 * _b1));
			double _rD = Math.Abs(dist) * Math.Sqrt(_D);
			long _ent = (long)Math.Floor(_rD);
			_rD -= _ent;

			long _num = (long)Math.Ceiling(_rD);
 
			BigRational d1 = new BigRational(Math.Sign(dist) * _ent, 
				Math.Sign(dist) * _num, DECIMAL_DEN);
			
			if(_Den == 0)
			{				
				/*existen infinitas soluciones de la interseccion de los segmenteos que 
				 * unen a pt(un punto en medio de un segmento)*/
				/*Ojo los signos estan intercambiados en la formula para que esten de 
				 * acuerdo con el signo de la distancia*/				
				BigRational x = -_b1 * d1 / _D + _a1;
				x += (_pt.X - _a1);
				BigRational y = +_a1 * d1 / _D + _b1;
				y += (_pt.Y - _b1);				
				return new RPoint(x, y);

			}

			_D = ((_a2 * _a2) + (_b2 * _b2));
			_rD = Math.Abs(dist) * Math.Sqrt(_D);
			_ent = (long)Math.Floor(_rD);
			_rD -= _ent;

			_num = (long)Math.Ceiling(_rD);

			BigRational d2 = new BigRational(Math.Sign(dist) * _ent, 
				Math.Sign(dist) * _num, DECIMAL_DEN);

			BigNumbers.BigRational _x = ( d2*_a1 - d1*_a2 ) / _Den;
			_x += _pt.X;

			BigNumbers.BigRational _y = ( d2*_b1 - d1*_b2 ) / _Den;
			_y += _pt.Y;
			
			return new RPoint(_x, _y);			
		}

		/**/
		private static void RemoveSegment(uint inic, uint end, Segment[] seg, bool[] ad, bool bystep, Segment[] original, Segment[] pseg)
		{
			uint _cant = (uint)seg.Length;		
			uint _indizq = LeftIndex(seg, inic, ad, ref inic);
			uint _indder = RightIndex(seg, end, ad, ref end);	

			if(_indizq == inic)
			{
				//cadena completamente mal
				for(uint u = 0; u < _cant; u++)
				{
					seg[u].Empty = true;
					ad[u] = false;
				}
				return;
			}			
		
			if(end == inic)
			{
				seg[end].Empty = true;	
				ad[end] = false;
				ExtremeAnalyze(_indizq, _indder, seg, true, ad, bystep, original, pseg);
			}			
			else 
			{
				if(RemoveChain(seg, inic, end, ad))
				{
					ExtremeAnalyze(inic, end, seg, false, ad, bystep, original, pseg);
				}			

				else
				{
					Segment _izq  = (Segment)seg[_indizq].Clone();
					Segment _der  = (Segment)seg[_indder].Clone();	
		
					Segment _left  = (Segment)seg[inic].Clone();
					Segment _right  = (Segment)seg[end].Clone();

					Segment _oseg2 = original[inic];
					Segment _oseg3 = original[end];

					RPoint _it24 = _left.Intersection(_der);
			
					Segment _netrem4 = (Segment)_der.Clone();
					_netrem4.Inicio = _it24;

					Segment _nseg2 = (Segment)_left.Clone();
					_nseg2.Fin = _it24;

					RPoint _it13 = _right.Intersection(_izq);
				
					Segment _netrem1 = (Segment)_izq.Clone();
					_netrem1.Fin = _it13;

					Segment _nseg3 = (Segment)_right.Clone();
					_nseg3.Inicio = _it13;

					bool _bad4 = _netrem4.Contrario(original[_indder]);
					bool _bad1 = _netrem1.Contrario(original[_indizq]);

					
					#region Ambos mal por el primer criterio
					if(_nseg2.Contrario(_oseg2) && _nseg3.Contrario(_oseg3))
					{
						/*Eliminar a ambos*/
						seg[inic].Empty = seg[end].Empty = true;
						ad[inic] = ad[end] = false;
						ExtremeAnalyze(_indizq, _indder, seg, true, ad, bystep, original, pseg); 
					}
					#endregion
					
					#region El 2do mal por el primer criterio
					else if(_nseg2.Contrario(_oseg2))
					{
						/*Elimino al 2*/
						seg[inic].Empty = true;
						ad[inic] = false;								
						seg[_indizq].Fin = _it13;
						seg[end].Inicio = _it13;
						ad[end] = false;

						if(seg[_indizq].Puntual)
						{
							seg[_indizq].VectorDirector = original[_indizq].VectorDirector;
						}
						if(seg[end].Puntual)
						{
							seg[end].VectorDirector = original[end].VectorDirector;
						}
						//
						//uint izq = (uint)((inic - 1) % seg.Length);
						if(!_netrem1.Contrario(original[_indizq]))//LeftSearchAdverse(seg, ad, ref izq) &&
						{
							CompareRelativePosition(seg, original, _indizq, end, ad);//CompareRelativePosition(seg, original, izq, end, ad);
						}

						else//if(_netrem1.Contrario(original[_indizq]))
						{								
							ad[_indizq] = true;
							if(!bystep)
							{
								RemoveSegment(_indizq, _indizq, seg, ad, bystep, original, pseg);
							}
						}

						if(ad[_indizq] && !bystep)
						{
							RemoveSegment(_indizq, _indizq, seg, ad, bystep, original, pseg);
						}
						if(ad[end] && !bystep)
						{
							RemoveSegment(end, end, seg, ad, bystep, original, pseg);
						}
					}
					#endregion

					#region El 3ro mal por el primer criterio
					else if(_nseg3.Contrario(_oseg3))
					{
						/*Elimino al 3*/
						seg[end].Empty = true;
						ad[end] = false;
						seg[_indder].Inicio = _it24;
						seg[inic].Fin = _it24;
						ad[inic] = false;
						
						if(seg[_indder].Puntual)
						{
							seg[_indder].VectorDirector = original[_indder].VectorDirector;
						}
						if(seg[inic].Puntual)
						{
							seg[inic].VectorDirector = original[inic].VectorDirector;
						}
						//uint der = (uint)((end + 1) % seg.Length);
						if(!_netrem4.Contrario(original[_indder]))//RightSearchAdverse(seg, ad, ref der) && 
						{
							CompareRelativePosition(seg, original, inic, _indder, ad);//CompareRelativePosition(seg, original, inic, der, ad);
						}

						else//if(_netrem4.Contrario(original[_indder]))
						{
							ad[_indder] = true;
							if(!bystep)
							{
								RemoveSegment(_indder, _indder, seg, ad, bystep, original, pseg);
							}
						}

						if(ad[_indder] && !bystep)
						{
							RemoveSegment(_indder, _indder, seg, ad, bystep, original, pseg);
						}
						if(ad[inic] && !bystep)
						{
							RemoveSegment(inic, inic, seg, ad, bystep, original, pseg);
						}
					}
					#endregion


					else
					{
						/*en este punto ninguno de los dos esta mal por el primer
						 * criterio*/
					
						if(!_bad1 && !_bad4)
						{
							#region Ambos mal por el segundo criterio
							if((_netrem4.Turn(_nseg2.Inicio) != original[_indder].Turn(original[inic].Inicio)
								&& !_nseg2.Puntual) && (_netrem1.Turn(_nseg3.Fin) != original[_indizq].Turn(original[end].Fin)
								&& ! _nseg3.Puntual))
							{
								seg[inic].Empty = seg[end].Empty = true;
								ad[inic] = ad[end] = false;
								ExtremeAnalyze(_indizq, _indder, seg, true, ad, bystep, original, pseg); 
								return;
							}
							#endregion

							#region El 2do mal por el segundo criterio
							else if(_netrem4.Turn(_nseg2.Inicio) != original[_indder].Turn(original[inic].Inicio)
								&& !_nseg2.Puntual )
							{
								/*pregunto si el 2 esta mal por el segungo criterio*/							
								/*Elimino al 2*/
								seg[inic].Empty = true;
								ad[inic] = false;								
								seg[_indizq].Fin = _it13;
								seg[end].Inicio = _it13;
								ad[end] = false;
							
								if(seg[_indizq].Puntual)
								{
									seg[_indizq].VectorDirector = original[_indizq].VectorDirector;
								}
								if(seg[end].Puntual)
								{
									seg[end].VectorDirector = original[end].VectorDirector;
								}
								//uint izq = (uint)((inic - 1) % seg.Length);
								if(!_bad1)//LeftSearchAdverse(seg, ad, ref izq) &&
								{
									CompareRelativePosition(seg, original, _indizq, end, ad);//CompareRelativePosition(seg, original, izq, end, ad);
								}

								else//if(_netrem1.Contrario(original[_indizq]))
								{								
									ad[_indizq] = true;
									if(!bystep)
									{
										RemoveSegment(_indizq, _indizq, seg, ad, bystep, original, pseg);
									}
								}
								if(ad[_indizq] && !bystep)
								{
									RemoveSegment(_indizq, _indizq, seg, ad, bystep, original, pseg);
								}
								if(ad[end] && !bystep)
								{
									RemoveSegment(end, end, seg, ad, bystep, original, pseg);
								}								
								return;
							}
								#endregion

							#region El 3ro mal por el segungo criterio
							else if(_netrem1.Turn(_nseg3.Fin) != original[_indizq].Turn(original[end].Fin)
								&& ! _nseg3.Puntual && !_bad4)
							{
								
								
									/*Elimino al 3*/
									seg[end].Empty = true;
									ad[end] = false;
									seg[_indder].Inicio = _it24;
									seg[inic].Fin = _it24;
									ad[inic] = false;
								
									if(seg[_indder].Puntual)
									{
										seg[_indder].VectorDirector = original[_indder].VectorDirector;
									}
									if(seg[inic].Puntual)
									{
										seg[inic].VectorDirector = original[inic].VectorDirector;
									}
									//uint der = (uint)((end + 1) % seg.Length);
									if(!_bad4)//RightSearchAdverse(seg, ad, ref der) && 
									{
										CompareRelativePosition(seg, original, inic, _indder, ad);//CompareRelativePosition(seg, original, inic, der, ad);
									}

									else//if(_netrem4.Contrario(original[_indder]))
									{
										ad[_indder] = true;
										if(!bystep)
										{
											RemoveSegment(_indder, _indder, seg, ad, bystep, original, pseg);
										}
									}
									if(ad[_indder] && !bystep)
									{
										RemoveSegment(_indder, _indder, seg, ad, bystep, original, pseg);
									}
									if(ad[inic] && !bystep)
									{
										RemoveSegment(inic, inic, seg, ad, bystep, original, pseg);
									}
									return;
								
							}
							#endregion
						}						

						
						{
							/*en este punto ninguno de los dos esta mal por el primer
							 * criterio. Ademas puede que se ma haya formado un extermo
							 * mal o no.*/
							#region El extremo 1 mal
							if(_netrem1.Contrario(original[_indizq]))
							{
								/*Elimino al 3*/
								seg[end].Empty = true;
								ad[end] = false;
								seg[_indder].Inicio = _it24;
								seg[inic].Fin = _it24;
								ad[inic] = false;
								
								if(seg[_indder].Puntual)
								{
									seg[_indder].VectorDirector = original[_indder].VectorDirector;
								}
								if(seg[inic].Puntual)
								{
									seg[inic].VectorDirector = original[inic].VectorDirector;
								}
								//uint der = (uint)((end + 1) % seg.Length);
								if(!_netrem4.Contrario(original[_indder]))//RightSearchAdverse(seg, ad, ref der) && 
								{
									CompareRelativePosition(seg, original, inic, _indder, ad);//CompareRelativePosition(seg, original, inic, der, ad);
								}

								else//if(_netrem4.Contrario(original[_indder]))
								{
									ad[_indder] = true;
									if(!bystep)
									{
										RemoveSegment(_indder, _indder, seg, ad, bystep, original, pseg);
									}
								}

								if(ad[_indder] && !bystep)
								{
									RemoveSegment(_indder, _indder, seg, ad, bystep, original, pseg);
								}
								if(ad[inic] && !bystep)
								{
									RemoveSegment(inic, inic, seg, ad, bystep, original, pseg);
								}
							}
								#endregion

							#region El extremo 4 mal
							else if(_netrem4.Contrario(original[_indder]))
							{
								/*Elimino al 2*/
								seg[inic].Empty = true;
								ad[inic] = false;								
								seg[_indizq].Fin = _it13;
								seg[end].Inicio = _it13;
								ad[end] = false;
								
								if(seg[_indizq].Puntual)
								{
									seg[_indizq].VectorDirector = original[_indizq].VectorDirector;
								}
								if(seg[end].Puntual)
								{
									seg[end].VectorDirector = original[end].VectorDirector;
								}
								//uint izq = (uint)((inic - 1) % seg.Length);
								if(!_netrem1.Contrario(original[_indizq]))//LeftSearchAdverse(seg, ad, ref izq) &&
								{
									CompareRelativePosition(seg, original, _indizq, end, ad);//CompareRelativePosition(seg, original, izq, end, ad);
								}

								else//if(_netrem1.Contrario(original[_indizq]))
								{								
									ad[_indizq] = true;
									if(!bystep)
									{
										RemoveSegment(_indizq, _indizq, seg, ad, bystep, original, pseg);
									}
								}

								if(ad[_indizq] && !bystep)
								{
									RemoveSegment(_indizq, _indizq, seg, ad, bystep, original, pseg);
								}
								if(ad[end] && !bystep)
								{
									RemoveSegment(end, end, seg, ad, bystep, original, pseg);
								}
							}
							#endregion
							
							else
							{
								/*voy a eliminar a alguno de los 2 este caso al 3*/
								/*Elimino al 3*/
								seg[end].Empty = true;
								ad[end] = false;
								seg[_indder].Inicio = _it24;
								seg[inic].Fin = _it24;
								ad[inic] = false;
								
								if(seg[_indder].Puntual)
								{
									seg[_indder].VectorDirector = original[_indder].VectorDirector;
								}
								if(seg[inic].Puntual)
								{
									seg[inic].VectorDirector = original[inic].VectorDirector;
								}
								//uint der = (uint)((end + 1) % seg.Length);
								if(!_netrem4.Contrario(original[_indder]))//RightSearchAdverse(seg, ad, ref der) && 
								{
									CompareRelativePosition(seg, original, inic, _indder, ad);//CompareRelativePosition(seg, original, inic, der, ad);
								}

								else//if(_netrem4.Contrario(original[_indder]))
								{
									ad[_indder] = true;
									if(!bystep)
									{
										RemoveSegment(_indder, _indder, seg, ad, bystep, original, pseg);
									}
								}

								if(ad[_indder] && !bystep)
								{
									RemoveSegment(_indder, _indder, seg, ad, bystep, original, pseg);
								}
								if(ad[inic] && !bystep)
								{
									RemoveSegment(inic, inic, seg, ad, bystep, original, pseg);
								}
							}
						}						
					}								
					
				}
			}
			
		}
		
		/**/
		private static bool RemoveChain(Segment[] seg, uint inic, uint end, bool[] adverse)
		{
			bool _result = false;
			uint _length = (uint)seg.Length;
			for(uint u = (inic + 1) % _length; u != end; u = (u + 1) % _length)
			{
				if(!seg[u].Empty)
				{
					seg[u].Empty = true;
					adverse[u] = false;
					_result = true;
				}
			}
			return _result;
		}
		/**/
		private static void ExtremeAnalyze(uint left, uint right, Segment[] seg, bool good, bool[] ad, bool bystep, Segment[] original, Segment[] pseg)
		{
			Segment _izq = (Segment)seg[left].Clone();
			
			Segment _der = (Segment)seg[right].Clone();

			if(_izq.Coincide(_der)&& !_izq.Puntual || _der.Coincide(_izq) && !_der.Puntual )//
			{
				#region Coincidentes
				
				bool _adversedLeftRight = _izq.Contrario(_der);

				if(!good)
				{
					if(_adversedLeftRight)
					{
						Segment _leftnew = (Segment)_izq.Clone();
						_leftnew.Fin = _der.Inicio;

						if(_leftnew.Contrario(_der))
						{
							seg[right].Fin = seg[left].Inicio;
							RemoveSegment(left, left, seg, ad, bystep, original, pseg);
						}
						else
						{
							seg[left].Fin = seg[right].Inicio;
							RemoveSegment(right, right, seg, ad, bystep, original, pseg);
						}
					}
					else
					{
						seg[left].Fin = seg[right].Inicio;
						RemoveSegment(left, right, seg, ad, bystep, original, pseg);
					}
				}
				else
				{
					if(_adversedLeftRight)
					{
						Segment _leftnew = (Segment)_izq.Clone();
						_leftnew.Fin = _der.Inicio;

						if(!_leftnew.Contrario(_der))
						{							
							seg[right].Fin = seg[left].Inicio;
							RemoveSegment(left, left, seg, ad, bystep, original, pseg);
						}
						else
						{
							seg[left].Fin = seg[right].Inicio;
							RemoveSegment(right, right, seg, ad, bystep, original, pseg);
						}
					}
					else
					{
						seg[left].Fin = seg[right].Inicio;						
					}
				}
			
				#endregion
			}
			else if(_izq.Paralelo(_der)&& !_izq.Puntual && !_der.Puntual )//
			{				
				RemoveSegment(left, right, seg, ad, bystep, original, pseg);
			}
			else
			{
				#region Hay interseccion

				if(_izq.Puntual)
				{
					
					_izq.VectorDirector = original[left].VectorDirector;
				}
				if(_der.Puntual)
				{
					
					_der.VectorDirector = original[right].VectorDirector;
				}
				RPoint _it = _izq.Intersection(_der);

				_izq.Fin = _it;
				_der.Inicio = _it;
					
				bool _adizq = _izq.Contrario(original[left]);
				bool _adder = _der.Contrario(original[right]);

				seg[left].Fin = _it;
				seg[right].Inicio = _it;

				
				if(_adizq && _adder)
				{
					ad[left] = ad[right] = true;
					if(!bystep)
					{
						RemoveSegment(left, right, seg, ad, bystep, original, pseg);
					}
				}
				else if(_adizq)
				{
					ad[left] = true;
					uint der = (uint)((right + 1) % seg.Length);
					if(RightSearchAdverse(seg, ad, ref der))
					{
						CompareRelativePosition(seg, original, right, der, ad);
					}					
					if(!bystep)
					{
						RemoveSegment(left, left, seg, ad, bystep, original, pseg);

						if(ad[der])
						{
							RemoveSegment(der, der, seg, ad, bystep, original, pseg);
						}
					}
				}
				else if(_adder)
				{
					ad[right] = true;
					uint izq = (uint)((left - 1) % seg.Length);
					if(LeftSearchAdverse(seg, ad, ref izq))
					{
						CompareRelativePosition(seg, original, izq, left, ad);
					}	
					if(!bystep)
					{
						
						RemoveSegment(right, right, seg, ad, bystep, original, pseg);
						
						/*Esto lo hago por si se me quda ese segmento asverso*/
						if(ad[izq])
						{
							RemoveSegment(izq, izq, seg, ad, bystep, original, pseg);
						}
					}
				}
				else
				{
					CompareRelativePosition(seg, original, left, right, ad);
					if(ad[left] && ad[right] && !bystep)
					{
						RemoveSegment(left, right, seg, ad, bystep, original, pseg);
					}
					else if(ad[left] && !bystep)
					{
						RemoveSegment(left, left, seg, ad, bystep, original, pseg);
					}
					else if(ad[right] && !bystep)
					{
						RemoveSegment(right, right, seg, ad, bystep, original, pseg);
					}
				}
				#endregion
			}
		}
		/**/
		private static bool RightSearchAdverse(Segment[] seg, bool[] ad, ref uint right)
		{
			while(seg[right].Empty)
			{
				right = (uint)((right + 1) % seg.Length);
			}
			return !ad[right];
		}
		/**/
		private static bool LeftSearchAdverse(Segment[] seg, bool[] ad, ref uint left)
		{
			while(seg[left].Empty)
			{
				left = (uint)((left - 1) % seg.Length);
			}
			return !ad[left];
		}
		/**/
		public static Polygon RemoveSegment(Segment[] seg, bool[] adverse, Segment[] original, bool bystep, Segment[] pseg)
		{
			uint i = 0;
			uint j = (uint)adverse.Length - 1;

			if(adverse[i] && adverse[j])
			{
				while(i + 1 < j && adverse[i + 1])
				{
					i++;
				}
				while(j - 1 > i && adverse[j - 1])
				{
					j--;
				}
				if(i + 1 >= j)
				{
					//Cadena completamente invertida
					return null;
				}
				else
				{
					RemoveSegment(j, i, seg, adverse, bystep, original, pseg);
					if(bystep)
					{						
						return new Polygon(seg, adverse);
					}
					i++; 
					j--;
				}
			}
			while(i <= j)
			{
				if(adverse[i])
				{
					uint k = i;
					
					while((i + 1) < adverse.Length && adverse[i + 1])
					{
						i++;
					}
					RemoveSegment(k, i, seg, adverse, bystep, original, pseg);	
					if(bystep)
					{
						return new Polygon(seg, adverse);
					}
				}

				i++;
			}
		
			
			//InvertedSideDetector(seg, original, adverse);
			
			uint u;
			for(u = 0; u < adverse.Length && !adverse[u]; u++);
			if(u < adverse.Length)
			{
				if(!bystep)
				{
					return RemoveSegment(seg, adverse, original, bystep, pseg);
				}
			}
			
			
			return new Polygon(seg, adverse);
		
		}

		/**/
		private static uint LeftIndex(Segment[] seg, uint left, bool[] adverse, ref uint inic)
		{
			uint length = (uint)seg.Length;
			uint u;
			for(u = (left - 1 + length) % length; u != left && 
				(adverse[u] || seg[u].Empty); u = (u - 1 + length) % length)
			{
				if(adverse[u])
				{
					inic = u;
				}
			};
			return u;
		}
		/**/
		private static uint RightIndex(Segment[] seg, uint right, bool[] adverse, ref uint inic)
		{
			uint length = (uint)seg.Length;
			uint u;
			for(u = (right + 1) % length; u != right && 
				(adverse[u] || seg[u].Empty); u = (u + 1) % length)
			{
				if(adverse[u])
				{
					inic = u;
				}
			};
			return u;
		}
		/**/
		public ArrayList GetBeak(int sgdist)
		{
			ArrayList _result = new ArrayList();
			
			for(uint u = 0; u < this.cantvert; u++)
			{
				TurnType _turn = RPoint.SD(this.vertices[(u - 1 + this.cantvert) % this.cantvert],
					this.vertices[u], this.vertices[(u + 1) % this.cantvert]);
				if((_turn == TurnType.right && sgdist >= 0) || (_turn == TurnType.left && sgdist < 0))
				{
					_result.Add(new Beak(this.vertices[(u - 1 + this.cantvert) % this.cantvert],
							this.vertices[u], u));						
				}					
			}
						
			return _result;
		}
		/**/
		public ArrayList PaintParallell(CircularDoublyConnected cc, short dist)
		{
			RPoint init  = null;
			SkVertex _sv = (cc.Cursor as SkVertex).Minimun;
			Segment[] _psegs = (this.Parallel(dist) as Polygon).Segments;
			ArrayList _segs = new ArrayList();
			Segment[] _oseg = this.Segments;
			Segment _pseg = null;

			//limpiando
			this.Clean(cc);

			// segmento del esqueleto que estoy analizando
			Segment _skedge = null;	

			while(true)
			{
				//en este punto estoy en una hoja
				_skedge = new Segment(_sv.Point, _sv.Parent.Point);	
				if(_sv.IsLeaf)
				{
					_sv.Painted = true;
					_pseg = this.ParallelSegmentComputation(_oseg, _psegs, _sv);
				}
				this.IntersectionComputation(_segs, _pseg, _skedge, ref init);

				if(_sv.LastChild)
				{
					
					if(_sv.Parent.Parent != null)
						_sv = _sv.Parent;
					else
					{
						
						_sv = (cc.Cursor.next as SkVertex).Children[0] as SkVertex ;
						_skedge = new Segment(_sv.Point, _sv.Parent.Point);	
						cc.Cursor = cc.Cursor.next;
						while(true)
						{
							this.IntersectionComputation(_segs, _pseg, _skedge, ref init);
							if(_sv.IsLeaf)
							{
								if(_sv.Painted)
									return _segs;
								break;
							}
							else
							{
								_sv = _sv.Children[0] as SkVertex;
								_skedge = new Segment(_sv.Point, _sv.Parent.Point);
							}
						}
					}
				}
				else
				{
					_sv.Painted = true;
					_sv = _sv.Parent.Children[_sv.Parent.Children.IndexOf(_sv) + 1] as SkVertex;
					_skedge = new Segment(_sv.Point, _sv.Parent.Point);	

					while(true)
					{
						this.IntersectionComputation(_segs, _pseg, _skedge, ref init);
						if(!_sv.IsLeaf)
						{
							_sv = _sv.Children[0] as SkVertex;
							_skedge = new Segment(_sv.Point, _sv.Parent.Point);
							continue;
						}
						
						break;
					}				
				}
			}
		}
		/**/
		public Polygon RemoveOriginalIntersection(Polygon original, int sgdist)
		{
			BeakEventQueue _eq = new BeakEventQueue();
			Segment[] _oseg = original.Segments;
			ArrayList _beaks = this.GetBeak(sgdist);
			Segment[] _pseg = this.Segments;
			ArrayList _vertex = new ArrayList((ICollection)this.vertices.Clone());

			foreach(Segment seg in _oseg)
			{
				SweepSegment ss = new SweepSegment(seg);
				_eq.Add(new Event(ss, ss.UpperEnd));
				_eq.Add(new Event(ss.LowerEnd, ss));
			}

			foreach(Beak beak in _beaks)
			{				
				_eq.AddBeak(new Event(beak, beak.UpperEnd));
				_eq.AddBeak(new Event(beak.LowerEnd, beak));
			}
			RBPSweep ps = new RBPSweep(_eq);
			ps.FindIntersections();

			int lot = 0;
			bool _wayfound = false;

			foreach(Beak beak in _beaks)
			{
				if(beak.Intersectios != null)
				{
					foreach(SegmentIntersection si in beak.Intersectios)
					{
						_wayfound = false;
						if(si.Segment.Intersect(_pseg[beak.Peak]))
						{
							_vertex[(int)((beak.Peak + lot) % _vertex.Count)] = si.it;							
							_vertex.Insert((int)((beak.Peak + 1 + lot++) % _vertex.Count),
								si.Segment.Intersection(_pseg[beak.Peak]));									
							_wayfound = true;
						}
						else
						{
							uint _oindex;
							for(_oindex = 0; _oindex < _oseg.Length && 
								!(_oseg[_oindex].Equals(si.Segment)); _oindex++);	
							Segment	_psegmtleft = _pseg[(beak.Peak - 1 + this.cantvert) % this.cantvert];
							Segment	_psegmtright = _pseg[beak.Peak];
							TurnType _intturn = _psegmtleft.Turn(si.Segment.Inicio);
							TurnType _endturn = _psegmtleft.Turn(si.Segment.Fin);
							ArrayList _aux = new ArrayList();

							if(_intturn == TurnType.left || (_intturn == TurnType.colinear &&
								_endturn == TurnType.right))
							{
								#region A favor del sentido 
//								Segment _osegmt;
//								while(true)
//								{
//									_osegmt = _oseg[(_oindex + 1) % _oseg.Length];
//									if(_osegmt.Intersect(_psegmtleft))
//									{
//										break;										
//									}
//									else if(_osegmt.Intersect(_psegmtright))
//									{
//										_vertex[(int)((beak.Peak + lot++) % this.cantvert)] = si.it;
//										if(_aux.Count > 0)
//										{
//											_vertex.InsertRange((int)((beak.Peak + lot++) % this.cantvert),
//												_aux);
//											lot += _aux.Count;
//										}
//										else
//										{
//											_vertex.Insert((int)((beak.Peak + lot++) % 
//												this.cantvert), _osegmt.Inicio);										
//										}
//										_vertex.Insert((int)((beak.Peak + lot++) % 
//											this.cantvert), _osegmt.Intersection(_psegmtright));	
//										_wayfound = true;
//										break;
//										
//									}
//									else
//									{	
//										if(_aux.Count == 0)
//										{
//											_aux.Add(_osegmt.Inicio);
//										}
//										_aux.Add(_osegmt.Fin);
//									}
//									_oindex++;
//								}
								#endregion
								
							}
							else
							{
								#region En contra del sentido
								Segment _osegmt;
//								int j;
								while(true)
								{
									_osegmt = _oseg[(_oindex - 1 + _oseg.Length) % _oseg.Length];
									if(_osegmt.Intersect(_psegmtleft))
									{
										break;
									}
									else if(_osegmt.Intersect(_psegmtright))
									{
										_vertex[(int)((beak.Peak + lot) % _vertex.Count)] = si.it;
										if(_aux.Count > 0)
										{
											_vertex.InsertRange((int)((beak.Peak + lot) % _vertex.Count),
												_aux);
											lot += _aux.Count;
										}
										else
										{
//											j = (int)((beak.Peak +  lot++) % _vertex.Count);
//											if(j == 0)
//											{
//												_vertex.Add(_osegmt.Fin);
//											}
//											else
//											{
//												_vertex.Insert(j, _osegmt.Fin);										
//											}
											_vertex.Insert((int)((beak.Peak + 1 + lot++) % _vertex.Count),
												_osegmt.Fin);		
										}
//										j = (int)((beak.Peak +  lot++) % _vertex.Count);
//										if(j == 0)
//										{
//											_vertex.Add(_osegmt.Intersection(_psegmtright));
//										}
//										else
//										{
//											_vertex.Insert(j, _osegmt.Intersection(_psegmtright));	
//										}
										_vertex.Insert((int)((beak.Peak + 1 + lot++) % _vertex.Count),
											_osegmt.Intersection(_psegmtright));	
										_wayfound = true;
										break;
									}
									else
									{
										if(_aux.Count == 0)
										{
											_aux.Add(_osegmt.Fin);
										}
										_aux.Add(_osegmt.Inicio);
									}
									_oindex--;
								}
								#endregion
							}
						}
						if(_wayfound)
						{
							break;
						}
					}
				}
			}
			for(int i = 1; i < _vertex.Count; i ++)
			{
				if(((RPoint)_vertex[i - 1]).Equals(_vertex[i]))
				{
					_vertex.RemoveAt(i);
					i--;
				}
				else if(RPoint.SD((RPoint)_vertex[i - 1], (RPoint)_vertex[i], 
						(RPoint)_vertex[(i + 1) % _vertex.Count]) == TurnType.colinear)
				{
					_vertex.RemoveAt(i);
					i--;
				}
			}
			RPoint[] _pvertex = new RPoint[_vertex.Count];

			for(int i = 0; i < _vertex.Count; i++)
			{
				_pvertex[i] = (RPoint)_vertex[i];
			}
			return new Polygon(_pvertex, null);			
		}		
		/**/
		public ArrayList GetValid(int sgdist)
		{
			int i;
			EventQueue _eq = new EventQueue();
			Segment[] _seg = this.Segments;
			foreach(Segment seg in _seg)
			{
				SweepSegment ss = new SweepSegment(seg);
				_eq.Add(new Event(ss, ss.UpperEnd));
				_eq.Add(new Event(ss.LowerEnd, ss));
			}
			
			PlaneSweep ps = new PlaneSweep(_eq);

			ArrayList so = null, sc = null;			
			ArrayList regions = new ArrayList();
			SweepSegment right = null, left = null;
			RPoint pt = null;
			int inter = 0;

			while(ps.NextStep(ref pt, ref sc, ref so, ref left, ref right))
			{
				Interval L = null, R = null;
				Interval[] P = null;

				#region Cierre
				if( sc.Count == 0 )
				{
					P = new Interval[1];
					if( ( left == null ) && ( right == null ) )
					{
						Region reg = null;
						int flag = System.Math.Sign( -sgdist - 1 );
						if( flag == 0 ) 
						{
							reg = new Region();
							regions.Add(reg);							
						}

						P[0] = new Interval(reg,flag,inter++);
						if( reg != null ) reg.SetInterval(P[0]);
					}
					else
					{
						if( left == null )
							P[0] = right.GetLeft();
						else
							P[0] = left.GetRight();
					}
					
					L = new Interval( P,P[0].GetRegion(),P[0].GetFlag(),inter++ );
					R = new Interval( P,P[0].GetRegion(),P[0].GetFlag(),inter++ );

					P[0].SetChild( L,R );
					R.AddLeft(pt);

					if( left != null ) left.SetRight( L );
					if( right != null ) right.SetLeft( R );
				}
				else
				{
					for( i = 0; i < sc.Count; i++ )
					{
						SweepSegment S = (SweepSegment)sc[i];
						S.GetRight().AddLeft( pt );
					}
					L = ((SweepSegment)sc[0]).GetLeft();
					R = ((SweepSegment)sc[sc.Count-1]).GetRight();
					
					if(so != null && so.Count > 0 )L.AddRight(pt);
				}
				#endregion

				#region Apertura
				if(so == null || so.Count == 0 )
				{
					P = new Interval[2];
					P[0] = L;
					P[1] = R;
					
					Region reg;
					
					int flag = System.Math.Sign( -sgdist - 1 );						
					bool newRegion = false;
				
					if( ( left == null ) && ( right == null ) && ( flag == 0 ) )
					{						
						reg = new Region();
						regions.Add(reg);
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
					
					if( left != null ) left.SetRight( I );
					if( right != null ) right.SetLeft( I );	
				}
				else
				{
					Interval I = null;
					Region reg = null;

					I = L;

					for(i=1; i<so.Count; i++)
					{
						int flag;

						if( ( ( (SweepSegment)so[i-1]).Inicio.Y > pt.Y ) ||
							( ( ( (SweepSegment)so[i-1]).Inicio.Y == pt.Y ) &&
							( ( (SweepSegment)so[i-1]).Inicio.X <= pt.X ) ) )
							flag = I.GetFlag() + 1;
						else
							flag = I.GetFlag() - 1;

						if( ( flag == 0 ) && 
							( ( !( (SweepSegment)so[i-1]).isReversal() ) &&
							(   !( (SweepSegment)so[ i ]).isReversal() ) &&
							(   !( (SweepSegment)so[i-1]).Coincident((Segment)so[i]) ) ) )
						{
							reg = new Region();
							regions.Add(reg);							
						}
						
						I = new Interval(reg,flag,inter++);
						if( reg != null ) reg.SetInterval(I);

						I.AddLeft(pt);
						reg = null;

						( (SweepSegment)so[i-1]).SetRight( I );
						( (SweepSegment)so[ i ]).SetLeft(  I );                        
					}

					( (SweepSegment)so[ 0 ]).SetLeft( L );
					if( ( (SweepSegment)so[ 0 ]).isReversal() )
					{
						L.SetRegion( null );						
					}
					
					( (SweepSegment)so[ so.Count-1 ]).SetRight( R ); 
					if( ( (SweepSegment)so[ so.Count-1 ]).isReversal() )
					{
						R.SetRegion( null );						
					}
				}
				#endregion
			}

			int init = 0;
			ArrayList aux = new ArrayList();

			if( System.Math.Sign( -sgdist - 1 ) == 0 )
			{
				Region reg = (Region)regions[0];
				reg.Invert();
				init = 1;

				reg = (Region)regions[regions.Count-1];
				ArrayList Face = reg.GetVertex( true );
				aux.Add(Face);
			}
			
			

			for( i = init; i<regions.Count; i++ )
			{
				Region reg = (Region)regions[i];
				if( reg.flag )
				{
					ArrayList Face = reg.GetVertex(false);
					aux.Add(Face);
				}
			}//Ver que pasa con la region exterior
			ArrayList Faces = new ArrayList();

			for(i=0; i<aux.Count;i++)
			{
				int n = ((ArrayList)aux[i]).Count;
				RPoint Pt;
				ArrayList Temp; 

				int initio=0;
				int j;
				if( n > 0 )
				{
					Pt = (RPoint)((ArrayList)aux[i])[0];
				
					if( !Pt.flag )
					{
						j = n;
						Pt = (RPoint)((ArrayList)aux[i])[j-1];
						while( ( j > 0 ) &&  ( !Pt.flag ))
						{
							j--;
							if( j > 0 ) Pt = (RPoint)((ArrayList)aux[i])[j-1];
						}
						if( j != n ) init = j;
					}
				
					j = init;
					Pt = (RPoint)((ArrayList)aux[i])[j];

					int cant = 0;
					do
					{			
						Temp = new ArrayList(); 

					while( ( !Pt.flag ) && ( cant != n ) )
					{
						Temp.Add( Pt );
						j = ( j + 1 )%n;
						Pt = (RPoint)((ArrayList)aux[i])[j];
						cant++;
					}
					while( ( Pt.flag ) && ( cant != n ) )
					{
						j = ( j + 1 )%n;						
						Pt = (RPoint)((ArrayList)aux[i])[j];
						cant++;
					}

						Faces.Add( Temp );
					}
					while( j != initio );
				}
			}

			return Faces;
							
		}

		/**/
		public ArrayList MySkeleton()
		{
			Segment[] _osegs = this.Segments;			
			ArrayList _strightsegs = new ArrayList();
			Polygon _parallel = this.Parallel(1);
			CircularDoublyConnected _list = new CircularDoublyConnected(); 
			RBTree _queue = new RBTree();			
			SkeletonVertex[] _oskvertex = new SkeletonVertex[this.cantvert];
			for(uint u = 0; u < this.cantvert; u++)
			{
				TurnType _turn = RPoint.SD(this.vertices[(u - 1 + this.cantvert) % this.cantvert],
					this.vertices[u], this.vertices[(u + 1) % this.cantvert]);

				SkeletonVertex sv = new SkeletonVertex(this.vertices[u],
					_osegs[(u - 1 + this.cantvert) % this.cantvert],	_osegs[u], 
					new Segment(this.vertices[u], _parallel.vertices[u]),
					_turn == TurnType.right);
				_list.Add(sv);
				_oskvertex[u] = sv;
				sv.List = _list;
			}
			foreach(SkeletonVertex sv in _list)
			{				
				IntersectionPoint _near = sv.MyIntersectionPoint();	
				_queue.Insert(_near);
			}
			while(!_queue.IsEmpty)
			{
				RBNode _act = _queue.TreeMinimun(_queue.Root);
				_queue.Delete(_act);
				IntersectionPoint _itpt = (IntersectionPoint)_act.value;
				if(_itpt is InfiniteIntersection || _itpt.Va.marked || _itpt.Vb.marked)
				{
					continue;
				}
				if(_itpt.Va.previous == _itpt.Vb)
				{
					_strightsegs.Add(new Segment(_itpt.Va.Vertex, _itpt.Vb.Vertex));
					_itpt.Va.marked = _itpt.Vb.marked = true;
					continue;
				}
				if(_itpt.Va.previous.previous == _itpt.Vb)
				{
					_strightsegs.Add(new Segment(_itpt.Va.Vertex, _itpt.ItPoint));
					_strightsegs.Add(new Segment(_itpt.Vb.Vertex, _itpt.ItPoint));
					_strightsegs.Add(new Segment(
						((SkeletonVertex)_itpt.Vb.next).Vertex, _itpt.ItPoint));
					_itpt.Va.marked = _itpt.Vb.marked = 
						((SkeletonVertex)_itpt.Vb.next).marked = true;
					continue;
				}
				_strightsegs.Add(new Segment(_itpt.Va.Vertex, _itpt.ItPoint));
				_strightsegs.Add(new Segment(_itpt.Vb.Vertex, _itpt.ItPoint));
				_itpt.Va.marked = _itpt.Vb.marked = true;

				SkeletonVertex Vnew = new SkeletonVertex(_itpt.ItPoint, 
					_itpt.Va.Left, _itpt.Vb.Right,
					_itpt.Va.Reflex || _itpt.Vb.Reflex);				
				Vnew.List = _itpt.Va.List;

				_itpt.Va.List.Replace(Vnew, _itpt.Va, _itpt.Vb);
				_itpt.Va.Parent = Vnew;
				_itpt.Vb.Parent = Vnew;
				IntersectionPoint itnew = Vnew.MyIntersectionPoint();
				_queue.Insert(itnew);
				
			}
			return _strightsegs;
		}
		/**/
		public SkeletonVertex[] SKeleton()
		{			
			Segment[] _osegs = this.Segments;				
			Polygon _parallel = this.Parallel(1);
			CircularDoublyConnected _list = new CircularDoublyConnected(); 
			RBTree _queue = new RBTree();			
			SkeletonVertex[] _oskvertex = new SkeletonVertex[this.cantvert];
			for(uint u = 0; u < this.cantvert; u++)
			{
				TurnType _turn = RPoint.SD(this.vertices[(u - 1 + this.cantvert) % this.cantvert],
					this.vertices[u], this.vertices[(u + 1) % this.cantvert]);

				SkeletonVertex sv = new SkeletonVertex(this.vertices[u],
					_osegs[(u - 1 + this.cantvert) % this.cantvert],	_osegs[u], 
					new Segment(this.vertices[u], _parallel.vertices[u]),
					_turn == TurnType.right);
				_list.Add(sv);
				_oskvertex[u] = sv;
				sv.List = _list;
			}
			foreach(SkeletonVertex sv in _list)
			{				
				IntersectionPoint _near = sv.Global(_queue);
				if(_near != null)
				{
					_queue.Insert(_near);
				}
			}
			
			while(!_queue.IsEmpty)
			{

				RBNode _act = _queue.TreeMinimun(_queue.Root);
				_queue.Delete(_act);
				IntersectionPoint _itpt = (IntersectionPoint)_act.value;
				if(_itpt.Va.marked && _itpt.Vb.marked)
				{
					//this.MakeSkeletonByStep(_queue);
					continue;
				}
				if(_itpt.Va.marked)
				{
					BigRational dist = _itpt.ItPoint.QuadraticDistance(_itpt.Va.Vertex);
					BigRational distp = _itpt.Va.Vertex.QuadraticDistance(
						_itpt.Va.parent.Vertex);
					if(dist < distp)
					{					
						SkeletonVertex _parent = _itpt.Va.parent;
						SkeletonVertex _leftnode, _mostleftnode, _mostrightnode;	
						if(_parent != null)
						{
							_mostrightnode = _parent.rchild;
							SkeletonVertex ghost = _parent; 						
							while(ghost.parent != null)
							{
								ghost = ghost.parent;
							}
							_mostleftnode = ghost.lchild;

							_parent.rchild.next = ghost.next;
							ghost.next.previous = _parent.rchild;
							_mostleftnode.previous = ghost.previous;
							ghost.previous.next = _mostleftnode;
							
							while(_parent != null)
							{
								_leftnode = _parent.lchild;
								_leftnode.next = _parent.rchild;
								_parent.rchild.previous = _leftnode;
								_parent.lchild.parent = null;
								_parent.rchild.parent = null;
								_parent.lchild.marked = false;
								_parent.rchild.marked = false;
								_parent = _parent.parent;	
								if(_parent != null)
								{
									_parent.rchild = _leftnode;
								}
							}

						
						
							/*caculo del nuevo vertice*/
							_itpt.Va.marked = _itpt.Vb.marked = true;
							SkeletonVertex Vnew = new SkeletonVertex(_itpt.ItPoint, 
								_itpt.Va.Left, _itpt.Vb.Right,
								_itpt.Va.Reflex || _itpt.Vb.Reflex);				
							Vnew.List = _itpt.Va.List;
							_itpt.Va.List.Replace(Vnew, _itpt.Va, _itpt.Vb);
							_itpt.Va.parent = Vnew;
							_itpt.Vb.parent = Vnew;
							_itpt.Va.qdist = Vnew.Vertex.QuadraticDistance(_itpt.Va.Vertex);
							_itpt.Vb.qdist = Vnew.Vertex.QuadraticDistance(_itpt.Vb.Vertex);
							Vnew.lchild = _itpt.Va;
							Vnew.rchild = _itpt.Vb;
						
							while(!_queue.IsEmpty)
							{
								RBNode _actaux = _queue.TreeMinimun(_queue.Root);
								_queue.Delete(_actaux);
							}
											
							SkeletonVertex _ghost = Vnew;
							while(_ghost.next != Vnew)
							{
								IntersectionPoint _near = _ghost.Global(_queue);
								if(_near != null)
								{
									_queue.Insert(_near);
								}
								_ghost = (SkeletonVertex)_ghost.next;
							}
							IntersectionPoint _near1 = _ghost.Global(_queue);
							if(_near1 != null)
							{
								_queue.Insert(_near1);
							}
						}
						continue;						
					}
					continue;
				}
				if(_itpt.Vb.marked)                                                    
				{
					BigRational dist = _itpt.ItPoint.QuadraticDistance(_itpt.Vb.Vertex);
					if(dist < _itpt.Vb.qdist)
					{
						SkeletonVertex _parent = _itpt.Vb.parent;
						SkeletonVertex _rightnode, _mostleftnode, _mostrightnode;		
						if(_parent != null)
						{
							_mostleftnode = _parent.rchild;

							SkeletonVertex ghost = _parent; 						
							while(ghost.parent != null)
							{
								ghost = ghost.parent;
							}
						
							_mostrightnode = ghost.rchild;	

							_parent.lchild.previous = ghost.previous;
							ghost.previous.next = _parent.lchild;
							_mostrightnode.next = ghost.next;
							ghost.next.previous = _mostrightnode;

							while(_parent != null)
							{
								_rightnode = _parent.rchild;
								_rightnode.previous = _parent.lchild;
								_parent.lchild.next = _rightnode;
								_parent.lchild.parent = null;
								_parent.lchild.marked = false;
								_parent.rchild.marked = false;
								_parent.rchild.parent = null;
								_parent = _parent.parent;	
								if(_parent != null)
								{
									_parent.lchild = _rightnode;
								}
							}
						
							ghost = _mostleftnode;						
						
							/*caculo del nuevo vertice*/
							_itpt.Va.marked = _itpt.Vb.marked = true;
							SkeletonVertex Vnew = new SkeletonVertex(_itpt.ItPoint, 
								_itpt.Va.Left, _itpt.Vb.Right,
								_itpt.Va.Reflex || _itpt.Vb.Reflex);				
							Vnew.List = _itpt.Va.List;
							_itpt.Va.List.Replace(Vnew, _itpt.Va, _itpt.Vb);
							_itpt.Va.parent = Vnew;
							_itpt.Vb.parent = Vnew;
							_itpt.Va.qdist = Vnew.Vertex.QuadraticDistance(_itpt.Va.Vertex);
							_itpt.Vb.qdist = Vnew.Vertex.QuadraticDistance(_itpt.Vb.Vertex);
							Vnew.lchild = _itpt.Va;
							Vnew.rchild = _itpt.Vb;
						
						
							while(!_queue.IsEmpty)
							{
								RBNode _actaux = _queue.TreeMinimun(_queue.Root);
								_queue.Delete(_actaux);
							}
							SkeletonVertex _ghost = Vnew;
							while(_ghost.next != Vnew)
							{
								IntersectionPoint _near = _ghost.Global(_queue);
								if(_near != null)
								{
									_queue.Insert(_near);
								}
								_ghost = (SkeletonVertex)_ghost.next;
							}
							IntersectionPoint _near1 = _ghost.Global(_queue);
							if(_near1 != null)
							{
								_queue.Insert(_near1);
							}
										
						}
						continue;			
						
					}
					continue;
				}
				//				if(_itpt is InfiniteIntersection || _itpt.Va.marked || _itpt.Vb.marked)
				//				{
				//					this.MakeSkeletonByStep(_queue);
				//					return;
				//				}
				if(_itpt.Va.previous == _itpt.Vb)
				{
					_itpt.Va.marked = _itpt.Vb.marked = true;
					_itpt.Va.parent = _itpt.Vb;
					_itpt.Vb.Parent = _itpt.Va;
					continue;
				}
				if(_itpt.Va.previous.previous == _itpt.Vb)
				{					
					SkeletonVertex _Vnew = new SkeletonVertex(_itpt.ItPoint, 
						_itpt.Va.Left, _itpt.Vb.Right,
						_itpt.Va.Reflex || _itpt.Vb.Reflex);		
					
					_itpt.Va.marked = _itpt.Vb.marked = 
						((SkeletonVertex)_itpt.Vb.next).marked = true;

					_itpt.Va.parent = _itpt.Vb.Parent = 
						((SkeletonVertex)_itpt.Vb.next).parent = _Vnew; 
					continue;
				}				
				_itpt.Va.marked = _itpt.Vb.marked = true;

				SkeletonVertex Vnew1 = new SkeletonVertex(_itpt.ItPoint, 
					_itpt.Va.Left, _itpt.Vb.Right,
					_itpt.Va.Reflex || _itpt.Vb.Reflex);				
				Vnew1.List = _itpt.Va.List;
				
			
				_itpt.Va.List.Replace(Vnew1, _itpt.Va, _itpt.Vb);
				_itpt.Va.parent = Vnew1;
				_itpt.Vb.parent = Vnew1;
				_itpt.Va.qdist = Vnew1.Vertex.QuadraticDistance(_itpt.Va.Vertex);
				_itpt.Vb.qdist = Vnew1.Vertex.QuadraticDistance(_itpt.Vb.Vertex);
				Vnew1.lchild = _itpt.Va;
				Vnew1.rchild = _itpt.Vb;
				IntersectionPoint itnew = Vnew1.Global(_queue);
				if(itnew != null)
				{
					_queue.Insert(itnew);
				}				
			}	




				/////////////////////////////////
//				RBNode _act = _queue.TreeMinimun(_queue.Root);
//				_queue.Delete(_act);
//				IntersectionPoint _itpt = (IntersectionPoint)_act.value;
//				if(_itpt is InfiniteIntersection || _itpt.Va.marked || _itpt.Vb.marked)
//				{
//					continue;
//				}
//				if(_itpt.Va.last == _itpt.Vb)
//				{
//					_itpt.Va.marked = _itpt.Vb.marked = true;
//					_itpt.Va.Parent = _itpt.Vb;
//					_itpt.Vb.Parent = _itpt.Va;
//					continue;
//				}
//				if(_itpt.Va.last.last == _itpt.Vb)
//				{					
//					SkeletonVertex _Vnew = new SkeletonVertex(_itpt.ItPoint, 
//						_itpt.Va.Left, _itpt.Vb.Right,
//						_itpt.Va.Reflex || _itpt.Vb.Reflex);		
//					
//					_itpt.Va.marked = _itpt.Vb.marked = 
//						((SkeletonVertex)_itpt.Vb.next).marked = true;
//
//					_itpt.Va.Parent = _itpt.Vb.Parent = 
//						((SkeletonVertex)_itpt.Vb.next).Parent = _Vnew; 
//					continue;
//				}				
//				_itpt.Va.marked = _itpt.Vb.marked = true;
//
//				SkeletonVertex Vnew = new SkeletonVertex(_itpt.ItPoint, 
//					_itpt.Va.Left, _itpt.Vb.Right,
//					_itpt.Va.Reflex || _itpt.Vb.Reflex);				
//				Vnew.List = _itpt.Va.List;
//
//				_itpt.Va.List.Replace(Vnew, _itpt.Va, _itpt.Vb);
//				_itpt.Va.Parent = Vnew;
//				_itpt.Vb.Parent = Vnew;
//				Vnew.lchild = _itpt.Va;
//				Vnew.rchild = _itpt.Vb;
//				IntersectionPoint itnew = Vnew.Global(_queue);
//				if(itnew != null)
//				{
//					_queue.Insert(itnew);
//				}				
//			}
			return _oskvertex;
		}
		/**/
		public SkeletonVertex[] Inicialitation(RBTree _queue)
		{
			Segment[] _osegs = this.Segments;				
			
            Polygon _parallel = this.Parallel(1);
			CircularDoublyConnected _list = new CircularDoublyConnected(); 			
			SkeletonVertex[] _oskvertex = new SkeletonVertex[this.cantvert];

			for (uint u = 0; u < this.cantvert; u++)
			{
				TurnType _turn = RPoint.SD(this.vertices[(u - 1 + this.cantvert) % this.cantvert],
					this.vertices[u], this.vertices[(u + 1) % this.cantvert]);

				SkeletonVertex sv = new SkeletonVertex(this.vertices[u],
					_osegs[(u - 1 + this.cantvert) % this.cantvert],	_osegs[u], 
					new Segment(this.vertices[u], _parallel.vertices[u]),
					_turn == TurnType.right);

                _list.Add(sv);
				_oskvertex[u] = sv;
				
                sv.List = _list;
			}

			foreach (SkeletonVertex sv in _list)
			{				
				IntersectionPoint _near = sv.Global(_queue);
				if(_near != null)
				{
					_queue.Insert(_near);
				}
			}

			return _oskvertex;
		}
		/**/
		public void MakeSkeletonByStep(RBTree _queue)
		{
			if(!_queue.IsEmpty)
			{
				RBNode _act = _queue.TreeMinimun(_queue.Root);
				_queue.Delete(_act);
				IntersectionPoint _itpt = (IntersectionPoint)_act.value;
				if(_itpt.Va.marked && _itpt.Vb.marked)
				{
					this.MakeSkeletonByStep(_queue);
					return;
				}
				if(_itpt.Va.marked)
				{
					BigRational dist = _itpt.ItPoint.QuadraticDistance(_itpt.Va.Vertex);
					BigRational distp = _itpt.Va.Vertex.QuadraticDistance(
						_itpt.Va.parent.Vertex);
					if(dist < distp)
					{					
						SkeletonVertex _parent = _itpt.Va.parent;
						SkeletonVertex _leftnode, _mostleftnode, _mostrightnode;	
						if(_parent != null)
						{
							_mostrightnode = _parent.rchild;
							SkeletonVertex ghost = _parent; 						
							while(ghost.parent != null)
							{
								ghost = ghost.parent;
							}
							_mostleftnode = ghost.lchild;

							_parent.rchild.next = ghost.next;
							ghost.next.previous = _parent.rchild;
							_mostleftnode.previous = ghost.previous;
							ghost.previous.next = _mostleftnode;
							
							while(_parent != null)
							{
								_leftnode = _parent.lchild;
								_leftnode.next = _parent.rchild;
								_parent.rchild.previous = _leftnode;
								_parent.lchild.parent = null;
								_parent.rchild.parent = null;
								_parent.lchild.marked = false;
								_parent.rchild.marked = false;
								_parent = _parent.parent;	
								if(_parent != null)
								{
									_parent.rchild = _leftnode;
								}
							}

						
						
							/*caculo del nuevo vertice*/
							_itpt.Va.marked = _itpt.Vb.marked = true;
							SkeletonVertex Vnew = new SkeletonVertex(_itpt.ItPoint, 
								_itpt.Va.Left, _itpt.Vb.Right,
								_itpt.Va.Reflex || _itpt.Vb.Reflex);				
							Vnew.List = _itpt.Va.List;
							_itpt.Va.List.Replace(Vnew, _itpt.Va, _itpt.Vb);
							_itpt.Va.parent = Vnew;
							_itpt.Vb.parent = Vnew;
							_itpt.Va.qdist = Vnew.Vertex.QuadraticDistance(_itpt.Va.Vertex);
							_itpt.Vb.qdist = Vnew.Vertex.QuadraticDistance(_itpt.Vb.Vertex);
							Vnew.lchild = _itpt.Va;
							Vnew.rchild = _itpt.Vb;
						
							while(!_queue.IsEmpty)
							{
								RBNode _actaux = _queue.TreeMinimun(_queue.Root);
								_queue.Delete(_actaux);
							}
											
							SkeletonVertex _ghost = Vnew;
							while(_ghost.next != Vnew)
							{
								IntersectionPoint _near = _ghost.Global(_queue);
								if(_near != null)
								{
									_queue.Insert(_near);
								}
								_ghost = (SkeletonVertex)_ghost.next;
							}
							IntersectionPoint _near1 = _ghost.Global(_queue);
							if(_near1 != null)
							{
								_queue.Insert(_near1);
							}
						}
						return;						
					}
					return;
				}
				if(_itpt.Vb.marked)                                                    
				{
					BigRational dist = _itpt.ItPoint.QuadraticDistance(_itpt.Vb.Vertex);
					if(dist < _itpt.Vb.qdist)
					{
						SkeletonVertex _parent = _itpt.Vb.parent;
						SkeletonVertex _rightnode, _mostleftnode, _mostrightnode;		
						if(_parent != null)
						{
							_mostleftnode = _parent.rchild;

							SkeletonVertex ghost = _parent; 						
							while(ghost.parent != null)
							{
								ghost = ghost.parent;
							}
						
							_mostrightnode = ghost.rchild;	

							_parent.lchild.previous = ghost.previous;
							ghost.previous.next = _parent.lchild;
							_mostrightnode.next = ghost.next;
							ghost.next.previous = _mostrightnode;

							while(_parent != null)
							{
								_rightnode = _parent.rchild;
								_rightnode.previous = _parent.lchild;
								_parent.lchild.next = _rightnode;
								_parent.lchild.parent = null;
								_parent.lchild.marked = false;
								_parent.rchild.marked = false;
								_parent.rchild.parent = null;
								_parent = _parent.parent;	
								if(_parent != null)
								{
									_parent.lchild = _rightnode;
								}
							}
						
							ghost = _mostleftnode;						
						
							/*caculo del nuevo vertice*/
							_itpt.Va.marked = _itpt.Vb.marked = true;
							SkeletonVertex Vnew = new SkeletonVertex(_itpt.ItPoint, 
								_itpt.Va.Left, _itpt.Vb.Right,
								_itpt.Va.Reflex || _itpt.Vb.Reflex);				
							Vnew.List = _itpt.Va.List;
							_itpt.Va.List.Replace(Vnew, _itpt.Va, _itpt.Vb);
							_itpt.Va.parent = Vnew;
							_itpt.Vb.parent = Vnew;
							_itpt.Va.qdist = Vnew.Vertex.QuadraticDistance(_itpt.Va.Vertex);
							_itpt.Vb.qdist = Vnew.Vertex.QuadraticDistance(_itpt.Vb.Vertex);
							Vnew.lchild = _itpt.Va;
							Vnew.rchild = _itpt.Vb;
						
						
							while(!_queue.IsEmpty)
							{
								RBNode _actaux = _queue.TreeMinimun(_queue.Root);
								_queue.Delete(_actaux);
							}
							SkeletonVertex _ghost = Vnew;
							while(_ghost.next != Vnew)
							{
								IntersectionPoint _near = _ghost.Global(_queue);
								if(_near != null)
								{
									_queue.Insert(_near);
								}
								_ghost = (SkeletonVertex)_ghost.next;
							}
							IntersectionPoint _near1 = _ghost.Global(_queue);
							if(_near1 != null)
							{
								_queue.Insert(_near1);
							}
										
						}
						return;			
						
					}
					return;
				}
				if(_itpt.Va.previous == _itpt.Vb)
				{
					_itpt.Va.marked = _itpt.Vb.marked = true;
					_itpt.Va.parent = _itpt.Vb;
					_itpt.Vb.Parent = _itpt.Va;
					return;
				}
				if(_itpt.Va.previous.previous == _itpt.Vb)
				{					
					SkeletonVertex _Vnew = new SkeletonVertex(_itpt.ItPoint, 
						_itpt.Va.Left, _itpt.Vb.Right,
						_itpt.Va.Reflex || _itpt.Vb.Reflex);		
					
					_itpt.Va.marked = _itpt.Vb.marked = 
						((SkeletonVertex)_itpt.Vb.next).marked = true;

					_itpt.Va.parent = _itpt.Vb.Parent = 
						((SkeletonVertex)_itpt.Vb.next).parent = _Vnew; 
					return;
				}				
				_itpt.Va.marked = _itpt.Vb.marked = true;

				SkeletonVertex Vnew1 = new SkeletonVertex(_itpt.ItPoint, 
					_itpt.Va.Left, _itpt.Vb.Right,
					_itpt.Va.Reflex || _itpt.Vb.Reflex);				
				Vnew1.List = _itpt.Va.List;
				
			
				_itpt.Va.List.Replace(Vnew1, _itpt.Va, _itpt.Vb);
				_itpt.Va.parent = Vnew1;
				_itpt.Vb.parent = Vnew1;
				_itpt.Va.qdist = Vnew1.Vertex.QuadraticDistance(_itpt.Va.Vertex);
				_itpt.Vb.qdist = Vnew1.Vertex.QuadraticDistance(_itpt.Vb.Vertex);
				Vnew1.lchild = _itpt.Va;
				Vnew1.rchild = _itpt.Vb;
				IntersectionPoint itnew = Vnew1.Global(_queue);
				if(itnew != null)
				{
					_queue.Insert(itnew);
				}				
			}			
		}

		/**/
		public CircularDoublyConnected CreateSk(bool interior)
		{
			//Lista circular con los árboles
			CircularDoublyConnected _list = new CircularDoublyConnected();
			PriorityQueue _queue = new PriorityQueue();
			
			#region Crear la lista Enlazada con los primeros árboles

			Segment[] _osegs = this.Segments;	
			Polygon _parallel = this.Parallel((short)((interior) ? 1 : -1));
			bool _convex = true;

			for (uint u = 0; u < this.cantvert; u++)
			{
				TurnType _turn = RPoint.SD(this.vertices[(u - 1 + this.cantvert) % this.cantvert],
					this.vertices[u], this.vertices[(u + 1) % this.cantvert]);

				SkVertexLeaf sv = new SkVertexLeaf(this.vertices[u],
					_osegs[(u - 1 + this.cantvert) % this.cantvert], _osegs[u],
					new Segment(this.vertices[u], _parallel.vertices[u]), u);
				_list.Add(sv);

				if(_turn == TurnType.right)
				{
					_convex = false;
					sv.Reflex = true;
				}
			}
			#endregion

			
			if(_convex)
			{
				if(interior)
				{
					#region Calcular la intersección de prioridad
					return null;
					#endregion
				}
				else
				{
					return _list;
				}
			}
			else
			{
				#region Encolar las intersecciones
				
				ArrayList _intersecPoints = new ArrayList();
				foreach(SkNode sk in _list)
					SkVertexLeaf.ComputeIt(sk as SkVertexLeaf, sk.next as SkVertexLeaf, _intersecPoints);
				
				foreach(PageHolder ph in _intersecPoints)
					ph.DiscardIntersecPoints();
				
				foreach(PageHolder ph in _intersecPoints)
					if(ph.Intersection != null && !ph.Intersection.Discarted)
						_queue.Insert(ph.Intersection);

				#endregion

				while(_list.Count > 2)
				{
					if(_queue.IsEmpty)
						return _list;

					NewIntersectionPoint IP = (NewIntersectionPoint)_queue.Extract();

					if(IP.VLeft.Marked && IP.VRight.Marked)
						continue;
					if(IP.VLeft.Marked && IP.Point.QuadraticDistance(IP.VLeft.Point) > 
						IP.VLeft.Point.QuadraticDistance(IP.VLeft.Parent.Point))
						continue;
					if(IP.VRight.Marked && IP.Point.QuadraticDistance(IP.VRight.Point) > 
						IP.VRight.Point.QuadraticDistance(IP.VRight.Parent.Point))
						continue;

					#region Crear el nuevo SkVertex

					SkNode _init = null;
					SkNode _end = _init;

					if (IP.Type == IntersecPointType.Left || IP.Type == IntersecPointType.NewLeft)
					{
						SkNode tmp = IP.VRight;
						SkNode cusor = IP.VLeft.Parent;

						while (cusor != null)
						{
							for (int i = cusor.Children.Count - 2; i >= 0; i--)
							{
								_list.AddPrevious(tmp, cusor.Children[i] as SkNode);
								tmp = cusor.Children[i] as SkNode;
								tmp.Parent = null;
								tmp.Marked = false;
							}
							cusor = cusor.Parent;
							_init = tmp;
						}

						_list.RemoveNode(tmp.previous);
						
						if (IP.Type == IntersecPointType.Left)
						{
							IP.VLeft.AddChildByRight(IP.VRight);
							_list.Replace(IP.VRight, IP.VLeft);
							IP.VLeft.Marked = false;
							IP.VRight.Marked = true;
							_end = IP.VLeft;
						}
						else
						{
							tmp = new SkNode(IP.Point, IP.VLeft, IP.VRight);
							_list.Replace(IP.VRight, tmp);
							_end = tmp;
						}
					}
					else
					{
						SkNode tmp = IP.VLeft;
						SkNode cusor = IP.VRight.Parent;

						while (cusor != null)
						{
							for (int i = 1; i < cusor.Children.Count; i++)
							{
								_list.AddNext(tmp, cusor.Children[i] as SkNode);
								tmp = cusor.Children[i] as SkNode;
								tmp.Parent = null;
								tmp.Marked = false;
							}
							cusor = cusor.Parent;
							_end = tmp;
						}

						_list.RemoveNode(tmp.next);

						if (IP.Type == IntersecPointType.Right)
						{
							IP.VRight.AddChildByLeft(IP.VLeft);
							IP.VRight.Marked = false;
							IP.VLeft.Marked = true;
							_list.Replace(IP.VLeft, IP.VRight);
							_init = IP.VRight;
						}
						else
						{
							tmp = new SkNode(IP.Point, IP.VLeft, IP.VRight);
							_list.Replace(IP.VLeft, tmp);
							_init = tmp;
						}
					}
					#endregion

					//encolar las nuevas intersecciones
					if(_init == null)
					{
						_init = _end;
						_end = null;
					}
					SkNode ghost = _init;
					do
					{
						ghost.ResetIntersections();
						ghost.ComputeIt(_queue);
						ghost = ghost.next as SkNode;
					}
					while(ghost.previous != _end && _end != null);

					ghost = _init;
					do
					{						
						ghost.Discard();
						ghost = ghost.next as SkNode;
					}
					while(ghost.previous != _end && _end != null);

					ghost = _init;
					do
					{						
						foreach(NewIntersectionPoint it in ghost.IntersecPoints)
						{
							if(it != null && !it.Discarted)
								_queue.Insert(it);
						}
						ghost = ghost.next as SkNode;
					}
					while(ghost.previous != _end && _end != null);

				}

				if ((_list.Cursor.next as SkNode).Children.Count > 0)
					_list.Cursor = _list.Cursor.next;

				(_list.Cursor as SkNode).AddChildByRight(_list.Cursor.next as SkNode);
				_list.RemoveNode(_list.Cursor.next);

				return _list;
				
			}
		}
		/**/
		
		/**/
        public delegate void Step(CircularDoublyConnected list);
		public event Step OnStep;

        private CircularDoublyConnected CreateSkeleton(bool interior)
        {
            //Lista circular con los árboles
            CircularDoublyConnected _list = new CircularDoublyConnected();

            #region Crear la lista Enlazada con los primeros árboles

            Segment[] _osegs = this.Segments;	
            Polygon _parallel = this.Parallel((short)((interior) ? 1 : -1));

            for (uint u = 0; u < this.cantvert; u++)
            {
                SkVertex sv = new SkVertex(this.vertices[u],
                    _osegs[(u - 1 + this.cantvert) % this.cantvert], _osegs[u],
                    new Segment(this.vertices[u], _parallel.vertices[u]));

                _list.Add(sv);
            }

            #endregion

            #region Iteraciones

            while (_list.Count > 2)
            {
                foreach (SkVertex _tree in _list)
                    _tree.IntersecPoints.Clear();

                //para cada (arbolito-arbolito siguiente) de la lista
                //ver sus puntos de intersección ..
                foreach (SkVertex _tree in _list)
                    ComputeIntersectionPoint(_tree, _tree.next as SkVertex);

                //descartar puntos
                foreach (SkVertex _tree in _list)
                    _tree.DiscardIntersecPoints();

                IntersecPoint IP = null;
                foreach (SkVertex _tree in _list)
                    foreach (IntersecPoint ip in _tree.IntersecPoints)
                        if (!ip.Discarted && (IP == null || ip.Distance < IP.Distance))
                            IP = ip;

                //si no hay mas intersecciones se retorna la lista de árboles ..
                if (IP == null)
                    return _list;

                //crear el nuevo SkVertex
                if (IP.Type == IntersecPointType.Left || IP.Type == IntersecPointType.NewLeft)
                {
                    SkVertex tmp = IP.VRight;
                    SkVertex cusor = IP.VLeft.Parent;

                    while (cusor != null)
                    {
                        for (int i = cusor.Children.Count - 2; i >= 0; i--)
                        {
                            _list.AddPrevious(tmp, cusor.Children[i] as SkVertex);
                            tmp = cusor.Children[i] as SkVertex;
                            tmp.Parent = null;
                        }
                        cusor = cusor.Parent;
                    }

                    _list.RemoveNode(tmp.previous);

                    if (IP.Type == IntersecPointType.Left)
                    {
                        IP.VLeft.AddChildByRight(IP.VRight);
                        _list.Replace(IP.VRight, IP.VLeft);
                    }
                    else
                    {
                        tmp = new SkVertex(IP.Point, IP.VLeft, IP.VRight);
                        _list.Replace(IP.VRight, tmp);
                    }
                }
                else
                {
                    SkVertex tmp = IP.VLeft;
                    SkVertex cusor = IP.VRight.Parent;

                    while (cusor != null)
                    {
                        for (int i = 1; i < cusor.Children.Count; i++)
                        {
                            _list.AddNext(tmp, cusor.Children[i] as SkVertex);
                            tmp = cusor.Children[i] as SkVertex;
                            tmp.Parent = null;
                        }
                        cusor = cusor.Parent;
                    }

                    _list.RemoveNode(tmp.next);

                    if (IP.Type == IntersecPointType.Right)
                    {
                        IP.VRight.AddChildByLeft(IP.VLeft);
                        _list.Replace(IP.VLeft, IP.VRight);
                    }
                    else
                    {
                        tmp = new SkVertex(IP.Point, IP.VLeft, IP.VRight);
                        _list.Replace(IP.VLeft, tmp);
                    }
                }

                //if (OnStep != null)
                //    OnStep(_list);
            }

            #endregion

            if ((_list.Cursor.next as SkVertex).Children.Count > 0)
                _list.Cursor = _list.Cursor.next;

            (_list.Cursor as SkVertex).AddChildByRight(_list.Cursor.next as SkVertex);
            _list.RemoveNode(_list.Cursor.next);

            return _list;
        }

        public CircularDoublyConnected CreateInteriorSkeleton()
        {
            return CreateSkeleton(true);
        }

        public CircularDoublyConnected CreateExteriorSkeleton()
        {
            CircularDoublyConnected _list = CreateSkeleton(false);
            SkVertex cursor = _list.Cursor as SkVertex;

            for (int i = 0; i < _list.Count; i++)
            {
                _list.Replace(cursor, cursor.CreateInfinityParent());
                cursor = cursor.next as SkVertex;
            }

            return _list;
        }

        private void ComputeIntersectionPoint(SkVertex va, SkVertex vb)
        {
            //tener en cuenta que va.next = vb (vb.previous = va)

            IntersecPoint _ip = null;

            #region Ver si Va intersecta a Vb

            SkVertex vCursor = vb;
            while (vCursor != null)
            {
                //si pasa por el punto ..
                if (va.BisectorContainsPoint(vCursor.Point))
                {
                    _ip = new IntersecPoint(
                            vCursor.Point.Distance(va.Left),
                            vCursor.Point,
                            va,
                            vCursor,
                            IntersecPointType.Right
                        );
                    break;
                }

                //si el nodo tiene padre ..
                else if (vCursor.Parent != null)
                {
                    //si el Bisector corta el segmento vCursor--vCursor.Parent ?
                    RPoint ip = va.BisectorIntersectsSegment(vCursor.Point, vCursor.Parent.Point);
                    if (ip != null)
                    {
                        _ip = new IntersecPoint(
                                ip.Distance(va.Left),
                                ip,
                                va,
                                vCursor,
                                IntersecPointType.NewRight
                            );
                        break;
                    }
                }

                //si corta el Bisector de la raíz
                else
                {
                    RPoint ip = va.Bisector.Intersection2(vCursor.Bisector);
                    if (ip != null)
                    {
                        Segment s1 = new Segment(va.Point, ip);
                        Segment s2 = new Segment(vCursor.Point, ip);

                        if (!s1.Contrario(va.Bisector) && !s2.Contrario(vCursor.Bisector))
                        {
                            _ip = new IntersecPoint(
                                    ip.Distance(va.Left),
                                    ip,
                                    va,
                                    vCursor,
                                    IntersecPointType.NewRight
                                );

                            _ip.IsNewRoot = true;

                            va.IntersecPoints.Add(_ip);
                            vb.IntersecPoints.Add(_ip);

                            return;
                        }
                    }
                }

                vCursor = (vCursor.Children.Count == 0) ? null : vCursor.Children[0] as SkVertex;
            }

            if (_ip != null)
            {
                va.IntersecPoints.Add(_ip);
                vb.IntersecPoints.Add(_ip);
            }

            #endregion

            _ip = null;

            #region Ver si Vb intersecta a Va

            vCursor = va;
            while (vCursor != null)
            {
                //si pasa por el punto ..
                if (vb.BisectorContainsPoint(vCursor.Point))
                {
                    _ip = new IntersecPoint(
                            vCursor.Point.Distance(vb.Right),
                            vCursor.Point,
                            vCursor,
                            vb,
                            IntersecPointType.Left
                        );
                    break;
                }

                //si el nodo tiene padre ..
                else if (vCursor.Parent != null)
                {
                    //si el Bisector corta el segmento vCursor--vCursor.Parent ?
                    RPoint ip = vb.BisectorIntersectsSegment(vCursor.Point, vCursor.Parent.Point);
                    if (ip != null)
                    {
                        _ip = new IntersecPoint(
                                ip.Distance(vb.Right),
                                ip,
                                vCursor,
                                vb,
                                IntersecPointType.NewLeft
                            );
                        break;
                    }
                }

                //si corta el Bisector de la raíz
                else
                {
                    RPoint ip = vb.Bisector.Intersection2(vCursor.Bisector);
                    if (ip != null)
                    {
                        Segment s1 = new Segment(vb.Point, ip);
                        Segment s2 = new Segment(vCursor.Point, ip);

                        if (!s1.Contrario(vb.Bisector) && !s2.Contrario(vCursor.Bisector))
                        {
                            _ip = new IntersecPoint(
                                    ip.Distance(vb.Right),
                                    ip,
                                    vCursor,
                                    vb,
                                    IntersecPointType.NewLeft
                                );
                            
                            _ip.IsNewRoot = true;

                            break;
                        }
                    }
                }

                vCursor = (vCursor.Children.Count == 0) ? null : vCursor.Children[vCursor.Children.Count - 1] as SkVertex;
            }

            if (_ip != null)
            {
                va.IntersecPoints.Add(_ip);
                vb.IntersecPoints.Add(_ip);
            }

            #endregion
        }
		/**/
		private void Clean(CircularDoublyConnected cc)
		{
			foreach(SkVertex sv in cc)
			{
				Clean(sv);
			}
		}
		private void Clean(SkVertex sv)
		{
			sv.Painted = false;
			foreach(SkVertex sk in sv.Children)
			{
				Clean(sk);
			}
		}
		public ArrayList PaintParallel(CircularDoublyConnected cc, short dist)
		{
			RPoint init  = null;
			SkVertex _sv = (cc.Cursor as SkVertex).Minimun;
 			Segment[] _psegs = (this.Parallel(dist) as Polygon).Segments;
			ArrayList _segs = new ArrayList();
			Segment[] _oseg = this.Segments;
			Segment _pseg = null;

			//limpiando
			this.Clean(cc);

			// segmento del esqueleto que estoy analizando
			Segment _skedge = null;	

			while(true)
			{
				//en este punto estoy en una hoja
				_skedge = new Segment(_sv.Point, _sv.Parent.Point);	
				if(_sv.IsLeaf)
				{
					_sv.Painted = true;
					_pseg = this.ParallelSegmentComputation(_oseg, _psegs, _sv);
				}
				this.IntersectionComputation(_segs, _pseg, _skedge, ref init);

				if(_sv.LastChild)
				{
					
					if(_sv.Parent.Parent != null)
						_sv = _sv.Parent;
					else
					{
						
						_sv = (cc.Cursor.next as SkVertex).Children[0] as SkVertex ;
						_skedge = new Segment(_sv.Point, _sv.Parent.Point);	
						cc.Cursor = cc.Cursor.next;
						while(true)
						{
							this.IntersectionComputation(_segs, _pseg, _skedge, ref init);
							if(_sv.IsLeaf)
							{
								if(_sv.Painted)
									return _segs;
								break;
							}
							else
							{
								_sv = _sv.Children[0] as SkVertex;
								_skedge = new Segment(_sv.Point, _sv.Parent.Point);
							}
						}
					}
				}
				else
				{
					_sv.Painted = true;
					_sv = _sv.Parent.Children[_sv.Parent.Children.IndexOf(_sv) + 1] as SkVertex;
					_skedge = new Segment(_sv.Point, _sv.Parent.Point);	

					while(true)
					{
						this.IntersectionComputation(_segs, _pseg, _skedge, ref init);
						if(!_sv.IsLeaf)
						{
							_sv = _sv.Children[0] as SkVertex;
							_skedge = new Segment(_sv.Point, _sv.Parent.Point);
							continue;
						}
						
						break;
					}				
				}
			}
		}
		/**/
		private void IntersectionComputation(ArrayList segs, Segment pseg, Segment skedge, ref RPoint init)
		{
			RPoint it = null;
			TurnType t_ini = pseg.Turn(skedge.Inicio);
			TurnType t_end = pseg.Turn(skedge.Fin);
			if(t_end == TurnType.colinear)
			{
				it = skedge.Fin;
				if(init != null)
				{
					segs.Add(new Segment(init, it));
					init = null;
				}				
				return;
			}
			if(t_ini == TurnType.colinear)
			{
				it = skedge.Inicio;
				if(init != null)
				{
					segs.Add(new Segment(init, it));
					init = null;	
				}	
				return;
			}
			if(t_ini != t_end) 
			{
				it = pseg.Intersection2(skedge);
				if(init == null)
					init = it;
				else
				{
					segs.Add(new Segment(init, it));
					init = null;
				}
			}			
		}
		/**/
		private Segment ParallelSegmentComputation(Segment[] _osegs, Segment[] _psegs, SkVertex _sv)
		{
			for(uint i = 0; i < this.cantvert; i++)
			{
				if(_osegs[i].Inicio.Equals(_sv.Right.Inicio))
				{
					return _psegs[i];
				}
			}
			return null;
		}
	}
}
