using System;
using System.Collections;
using BigNumbers;

namespace Structures
{
	/// <summary>
	/// Descripción breve de SkNode.
	/// </summary>
	public class SkNode: Linked
	{
		RPoint point;
		Segment left, right, bisector;
		SkNode parent;
		ArrayList children = new ArrayList();
		bool marked;
		NewIntersectionPoint itpt;
		public bool Reflex;
		NewIntersectionPoint[] intersecPoints = new NewIntersectionPoint[]{null, null};

		public SkNode(RPoint point, Segment leftSegment, Segment rightSegment, Segment bisectorSegment)
		{
			this.point = point;
			this.left = leftSegment;
			this.right = rightSegment;
			this.bisector = bisectorSegment;
		}		
		private void ComputeBisector()
		{
			if(!this.left.Paralelo(this.right))
			{
				RPoint it = this.left.Intersection(this.right);		
				
				TurnType _leftturn = this.right.Turn(this.left.Inicio);
				TurnType _vertexturn = this.right.Turn(this.point);

				Segment _aux = new Segment(this.right.Inicio, it);
				Segment _temp = new Segment(left.Inicio, it);

				if(!_temp.Contrario(left))
				{
					if(_leftturn == _vertexturn || _leftturn == TurnType.colinear)
					{
						this.bisector = new Segment(it, this.point);
					}
					else
					{
						this.bisector = new Segment(this.point, it);
					}
				}
				else
				{
					if(_leftturn == _vertexturn || _leftturn == TurnType.colinear)
					{
						this.bisector = new Segment(this.point, it);
					}
					else
					{
						this.bisector = new Segment(it, this.point);
					}
				}
			}

			else if(this.left.Coincide(this.right))
			{
				//throw new Exception("FALTA POR HACER ESTE CASO");
				this.bisector = new Segment(this.point, -this.left.VectorDirector.Y,
					this.left.VectorDirector.X);
			}

			else
			{
				this.bisector = new Segment(this.point, this.right.VectorDirector.X,
					this.right.VectorDirector.Y);
			}
		}
		public NewIntersectionPoint[] IntersecPoints
		{
			get
			{
				return this.intersecPoints;
			}
		}
		public void ResetIntersections()
		{
			this.intersecPoints[0] = null;
			this.intersecPoints[1] = null;
		}
		public NewIntersectionPoint Intersection
		{
			get
			{
				return itpt;
			}
			set
			{
				this.itpt = value;
			}
		}
		public SkNode(RPoint point, SkNode leftTree, SkNode rigthTree)
		{
			this.point = point;
            
			leftTree.parent = this;
			rigthTree.parent = this;
            
			children.AddRange(new object[] { leftTree, rigthTree });

			this.left = leftTree.left;
			this.right = rigthTree.right;
			this.Reflex = (leftTree.Reflex && leftTree is SkVertexLeaf) || (rigthTree.Reflex && rigthTree is SkVertexLeaf);
			leftTree.Marked = rigthTree.Marked = true;

			ComputeBisector();
		}

		public void AddChildByLeft(SkNode leftTree)
		{
			children.Insert(0, leftTree);
			leftTree.parent = this;
			this.left = leftTree.left;
			this.Reflex = this.Reflex || (leftTree.Reflex && leftTree is SkVertexLeaf);
			leftTree.Marked = true;
			ComputeBisector();
		}

		public void AddChildByRight(SkNode rightTree)
		{
			children.Add(rightTree);
			rightTree.parent = this;
			this.right = rightTree.right;
			this.Reflex = this.Reflex || (rightTree.Reflex && rightTree is SkVertexLeaf);
			rightTree.Marked = true;
			ComputeBisector();
		}

		public bool BisectorContainsPoint(RPoint p)
		{
			BigRational det = (bisector.Fin.Y - bisector.Inicio.Y) * (p.X - bisector.Fin.X) - 
				(bisector.Fin.X - bisector.Inicio.X) * (p.Y - bisector.Fin.Y);

			//si el punto está en la recta ..
			if (-RPoint.epsilon < det && det < RPoint.epsilon)
			{
				//si está en la dirección ..
				Segment vd = new Segment(this.point, p);
				return !bisector.Contrario(vd);
			}
			return false;
		}

		public Segment Bisector
		{
			get
			{
				return this.bisector;
			}
		}
		public RPoint Point
		{
			get { return point; }
			set { point = value; }
		}
		public ArrayList Children
		{
			get { return children; }
			set { children = value; }
		}
		public Segment Right
		{
			get { return right; }
			set { right = value; }
		}

		public Segment Left
		{
			get { return left; }
			set { left = value; }
		}

		public SkNode Parent
		{
			get { return parent; }
			set { parent = value; }
		}
		public bool Marked
		{
			get { return marked; }
			set { marked = value; }
		}
		public RPoint BisectorIntersectsSegment(RPoint p1, RPoint p2)
		{
			TurnType t1 = bisector.Turn(p1);
			TurnType t2 = bisector.Turn(p2);

			if ((t1 == TurnType.left && t2 == TurnType.right) || (t1 == TurnType.right && t2 == TurnType.left))
			{
				//si está en la dirección ..
				RPoint ip = bisector.Intersection(new Segment(p1, p2));
				Segment vd = new Segment(this.point, ip);
				return (bisector.Contrario(vd)) ? null : ip;
			}

			return null;
		}

		public SkNode CreateInfinityParent()
		{
			RPoint vd = this.bisector.VectorDirector;
			RPoint infinity = this.point + new RPoint(10000 * vd.X, 10000 * vd.Y);
			SkNode p = new SkNode(infinity, this.left, this.right, this.bisector);
			p.children.Add(this);
			this.parent = p;
			return p;
		}
		public void ComputeIt(PriorityQueue queue)
		{
			NewIntersectionPoint _ip = null;
			NewIntersectionPoint _nextip = null; 
			NewIntersectionPoint _previousip = null;

			if(!this.Reflex && !(this.next as SkNode).Reflex && !(this.previous as SkNode).Reflex)
			{
				#region si se intersecta con next

				SkNode vCursor = this.next as SkNode;
				SkNode va = this;

				if (va.BisectorContainsPoint(vCursor.Point))
				{
					_ip = new NewIntersectionPoint(
						vCursor.Point.Distance(va.Left),
						vCursor.Point,
						va,
						vCursor,
						IntersecPointType.Right
						);
					_nextip = _ip;
				}
				else
				{
					RPoint ip = va.Bisector.Intersection2(vCursor.Bisector);
					if (ip != null)
					{
						Segment s1 = new Segment(va.Point, ip);
						Segment s2 = new Segment(vCursor.Point, ip);

						if (!s1.Contrario(va.Bisector) && !s2.Contrario(vCursor.Bisector))
						{
							_ip = new NewIntersectionPoint(
								ip.Distance(va.Left),
								ip,
								va,
								vCursor,
								IntersecPointType.NewRight
								);

							_ip.IsNewRoot = true;
							_nextip = _ip;
						}
					}
				}

				#endregion

				#region si se intersecta con previous

				vCursor = this.previous as SkNode;
				SkNode vb = this;

				if (vb.BisectorContainsPoint(vCursor.Point))
				{
					_ip = new NewIntersectionPoint(
						vCursor.Point.Distance(vb.Right),
						vCursor.Point,
						vCursor,
						vb,
						IntersecPointType.Left
						);
					_previousip = _ip;
				}
				else
				{
					RPoint ip = vb.Bisector.Intersection2(vCursor.Bisector);
					if (ip != null)
					{
						Segment s1 = new Segment(vb.Point, ip);
						Segment s2 = new Segment(vCursor.Point, ip);

						if (!s1.Contrario(vb.Bisector) && !s2.Contrario(vCursor.Bisector))
						{
							_ip = new NewIntersectionPoint(
								ip.Distance(vb.Right),
								ip,
								vCursor,
								vb,
								IntersecPointType.NewLeft
								);
                            
							_ip.IsNewRoot = true;

							_previousip = _ip;
						}
					}
				}
				#endregion
				
			}
			else // si puede cortar
			{
				#region Ver si Va intersecta a Vb
				
				_ip = null;
				SkNode vCursor = this.next as SkNode;
				SkNode va = this;

				while (vCursor != null)
				{
					//si pasa por el punto ..
					if (va.BisectorContainsPoint(vCursor.Point))
					{
						_ip = new NewIntersectionPoint(
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
							_ip = new NewIntersectionPoint(
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
								_ip = new NewIntersectionPoint(
									ip.Distance(va.Left),
									ip,
									va,
									vCursor,
									IntersecPointType.NewRight
									);

								_ip.IsNewRoot = true;
								
								break;
							}
						}
					}

					vCursor = (vCursor.Children.Count == 0) ? null : vCursor.Children[0] as SkNode;
				}

				if (_ip != null)
					_nextip = _ip;					

				#endregion

				_ip = null;

				#region Ver si Vb intersecta a Va
				
				SkNode vb = this; 
				vCursor = this.previous as SkNode;

				while (vCursor != null)
				{
					//si pasa por el punto ..
					if (vb.BisectorContainsPoint(vCursor.Point))
					{
						_ip = new NewIntersectionPoint(
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
							_ip = new NewIntersectionPoint(
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
								_ip = new NewIntersectionPoint(
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

					vCursor = (vCursor.Children.Count == 0) ? null : vCursor.Children[vCursor.Children.Count - 1] as SkNode;
				}

				if (_ip != null)
					_previousip = _ip;


				#endregion
			}
			if(_nextip != null)
			{
				this.intersecPoints[1] = _nextip;
				(this.next as SkNode).intersecPoints[0] = _nextip;
			}	
			if(_previousip != null)
			{
				this.intersecPoints[0] = _previousip;
				(this.previous as SkNode).intersecPoints[1] = _previousip;
			}
		}
		public void ComputeItOpen()
		{
			NewIntersectionPoint _ip = null;
			NewIntersectionPoint _nextip = null; 
			NewIntersectionPoint _previousip = null;
//			else // si puede cortar
			{
				#region Ver si Va intersecta a Vb
				
				_ip = null;
				SkNode vCursor = this.next as SkNode;
				SkNode va = this;

				while (vCursor != null)
				{
					//si pasa por el punto ..
					if (va.BisectorContainsPoint(vCursor.Point))
					{
						_ip = new NewIntersectionPoint(
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
							_ip = new NewIntersectionPoint(
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
								_ip = new NewIntersectionPoint(
									ip.Distance(va.Left),
									ip,
									va,
									vCursor,
									IntersecPointType.NewRight
									);

								_ip.IsNewRoot = true;
								
								break;
							}
						}
					}

					vCursor = (vCursor.Children.Count == 0) ? null : vCursor.Children[0] as SkNode;
				}

				if (_ip != null)
					_nextip = _ip;					

				#endregion

				_ip = null;

				#region Ver si Vb intersecta a Va
				
				SkNode vb = this; 
				vCursor = this.previous as SkNode;

				while (vCursor != null)
				{
					//si pasa por el punto ..
					if (vb.BisectorContainsPoint(vCursor.Point))
					{
						_ip = new NewIntersectionPoint(
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
							_ip = new NewIntersectionPoint(
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
								_ip = new NewIntersectionPoint(
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

					vCursor = (vCursor.Children.Count == 0) ? null : vCursor.Children[vCursor.Children.Count - 1] as SkNode;
				}

				if (_ip != null)
					_previousip = _ip;


				#endregion
			}
			if(_nextip != null)
			{
				this.intersecPoints[1] = _nextip;
				(this.next as SkNode).intersecPoints[0] = _nextip;
			}	
			if(_previousip != null)
			{
				this.intersecPoints[0] = _previousip;
				(this.previous as SkNode).intersecPoints[1] = _previousip;
			}
		}
		public void Discard()
		{
			if(this.intersecPoints[0] != null && this.intersecPoints[1] != null)
			{
				if(this.intersecPoints[0].Point.QuadraticDistance(this.Point)
					< this.intersecPoints[1].Point.QuadraticDistance(this.Point))
					this.intersecPoints[1].Discarted = true;
				else
					this.intersecPoints[0].Discarted = true;
			}
		}
	}
}
