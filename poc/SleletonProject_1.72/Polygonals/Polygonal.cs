using System;
using BorradorTesis;
using System.Collections;
using System.Drawing;
using Structures;
using BorradorTesis.Plane_Sweep;
using BorradorTesis.Plane_Sweep.Nodes;
using BigNumbers;

namespace BorradorTesis.Polygonals
{
	/// <summary>
	/// Descripción breve de Polygonal.
	/// </summary>
	public class Polygonal : ICloneable
	{
		/**/
		protected RPoint[] vertices;
		/**/
		protected uint cantvert;
		/**/
		protected const long DECIMAL_DEN = 100;
		/**/
		public Polygonal(RPoint[] vertices)
		{			
			this.cantvert = (uint)vertices.Length;
			uint u = this.GetMostLeftDownVertex(vertices);
			if(RPoint.SD(vertices[(u - 1 + this.cantvert) % this.cantvert], 
				vertices[u], vertices[(u + 1) % this.cantvert]) == TurnType.right)
			{
				this.vertices = new RPoint[this.cantvert];
				for(uint t = 0; t < this.cantvert; t ++)
				{
					this.vertices[t] = vertices[this.cantvert - (1 + t)]; 
				}
			}
			else
			{
				this.vertices = vertices;	
			}			
		}
		/**/
		protected Polygonal(){}
		/**/
		protected Polygonal(ArrayList vertices)
		{
			this.cantvert = (uint)vertices.Count;
			this.vertices = new RPoint[this.cantvert];
			for(int i = 0; i < this.cantvert; i++)
			{
				this.vertices[i] = (RPoint)vertices[i];
			}			
		}
		/**/
		public uint CantidadVertices
		{
			get
			{
				return this.cantvert;
			}
		}

		/**/
		public RPoint[] Vertices
		{
			get
			{
				return this.vertices;
			}
		}

		/**/
		public virtual void Pinta(Graphics g, Pen p, Pen adveP, int height)
		{
			for(uint u = 1; u < this.cantvert; u++)
			{
				g.DrawLine(p, this.vertices[u - 1].XToLong,
					height - this.vertices[u - 1].YToLong,
					this.vertices[u].XToLong,
					height - this.vertices[u].YToLong);
			}
		}

		/**/
		public virtual Segment[] Segments
		{
			get
			{
				Segment[] _result = new Segment[this.cantvert - 1];
				for(uint  u = 0; u < this.cantvert - 1; u ++)
					_result[u] = new Segment
						(this.vertices[u], this.vertices[u + 1]);
				return _result;
			}
		}
		/**/
		protected  uint GetMostLeftDownVertex(RPoint[] vertices)
		{
			uint _result = 0;
			for(uint u = 1; u < this.cantvert; u ++)
			{
				if(vertices[u].X < vertices[_result].X || (vertices[u].X == vertices[_result].X && 
					vertices[u].Y < vertices[_result].Y))
					_result = u;
			}
			return _result;			
		}
		/**/
		public virtual Polygonal Parallel(short dist)
		{
			RPoint[] _vertparalelos = new RPoint[this.cantvert];			
			for(uint i = 1; i < this.cantvert - 1; i++)
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
			#region Extremo izquiedo

			RPoint _ad = this.vertices[1];
			RPoint pt = this.vertices[0];
			
			_ad = _ad - pt;

			long _a = _ad.XToLong;
			long _b = _ad.YToLong;

			long D = ((_a * _a) + (_b * _b));
			double rD = Math.Abs(dist) * Math.Sqrt(D);
			long ent = (long)Math.Floor(rD);
			rD -= ent;

			long num = (long)Math.Ceiling(rD);

			BigRational d = new BigRational(Math.Sign(dist) * ent, 
				Math.Sign(dist) * num, DECIMAL_DEN);

			BigRational _xe = -_b * d / D + _a;
			_xe += (pt.X - _a);
			BigRational _ye = +_a * d / D + _b;
			_ye += (pt.Y - _b);				
			_vertparalelos[0] = new RPoint(_xe, _ye);

			#endregion
			
			#region Extremo derecho
			
			Segment last = new Segment(this.vertices[this.cantvert - 2], this.vertices[this.cantvert - 1]);
			_ad = this.vertices[this.cantvert - 1] + 10 * last.VectorDirector;
			pt = this.vertices[this.cantvert - 1];

			_ad = _ad - pt;

			_a = _ad.XToLong;
			_b = _ad.YToLong;

			D = ((_a * _a) + (_b * _b));
			rD = Math.Abs(dist) * Math.Sqrt(D);
			ent = (long)Math.Floor(rD);
			rD -= ent;

			num = (long)Math.Ceiling(rD);

			d = new BigRational(Math.Sign(dist) * ent, 
				Math.Sign(dist) * num, DECIMAL_DEN);

			_xe = -_b * d / D + _a;
			_xe += (pt.X - _a);
			_ye = +_a * d / D + _b;
			_ye += (pt.Y - _b);				
			_vertparalelos[this.cantvert - 1] = new RPoint(_xe, _ye);
			
			#endregion

			return new Polygonal(this, _vertparalelos);		
		}
		/**/
		private Polygonal(Polygonal original, RPoint[] _vertex)
		{
			this.vertices = _vertex;
			this.cantvert = original.cantvert;
		}
		/**/
		public virtual CircularDoublyConnected CreateSkleton(bool interior)
		{
			CircularDoublyConnected _list = new CircularDoublyConnected();
			PriorityQueue _queue = new PriorityQueue();
			
			#region Crear la lista Enlazada con los primeros árboles

			Segment[] _osegs = this.Segments;	
			Polygonal _parallel = this.Parallel((short)((interior) ? 1 : -1));			
			
			uint u = 0;
			SkVertexLeaf sv = new SkVertexLeaf(this.vertices[u],
				new  Segment(_osegs[u].Inicio - (10 * _osegs[u].VectorDirector), _osegs[u].Inicio), _osegs[u],
				new Segment(this.vertices[u], _parallel.vertices[u]), u);
			_list.Add(sv);

			for (u = 1; u < this.cantvert - 1; u++)
			{
				sv = new SkVertexLeaf(this.vertices[u],
					_osegs[(u - 1 + this.cantvert) % this.cantvert], _osegs[u],
					new Segment(this.vertices[u], _parallel.vertices[u]), u);
				_list.Add(sv);
			}
				
			sv = new SkVertexLeaf(this.vertices[u],
				_osegs[u - 1], new Segment(_osegs[u - 1].Fin, _osegs[u - 1].Fin + (10 * _osegs[u - 1].VectorDirector)),
				new Segment(this.vertices[u], _parallel.vertices[u]), u - 1);
			_list.Add(sv);
			

			#endregion
			
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
					ghost.ComputeItOpen();
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

		#region Miembros de ICloneable

		public virtual object Clone()
		{
			RPoint[] _vertclone = new RPoint[this.cantvert];
			for(uint u = 0; u < this.cantvert; u++)
			{
				_vertclone[u] = this.vertices[u];
			}
			return  new Polygonal(_vertclone);
		}

		#endregion
	}
}
