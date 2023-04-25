using System;
using BorradorTesis;
using System.Collections;
using BigNumbers;

namespace Structures
{
	/// <summary>
	/// Descripción breve de SkeletonVertex.
	/// </summary>
	public class SkeletonVertex: Linked
	{
		/**/
		private RPoint vertex;		
		/**/
//		public BigRational epsilon = new BigRational(
		/**/
		private Segment left, right, bisector;
		/**/
		public BigRational qdist;
		/**/
		private bool reflex;
		/**/
		public SkeletonVertex parent, rchild, lchild;
		/**/
		public bool marked;
		/**/
		private CircularDoublyConnected belong;
		/**/
		public SkeletonVertex(RPoint vertex)
		{
			this.vertex = vertex;
			this.marked = true;
		}
		/**/
		public SkeletonVertex(RPoint vertex, Segment left, Segment right, 
			Segment bisector, bool reflex)
		{	
			this.right = right;
			this.left = left;
			this.vertex = vertex;
			this.reflex = reflex;
			this.bisector = bisector;				
		}
		/**/
		public SkeletonVertex(RPoint vertex, Segment left, Segment right, bool peak)
		{
			this.right = right;
			this.left = left;
			this.vertex = vertex;
		
			if(!this.left.Paralelo(this.right))
			{
				RPoint it = this.left.Intersection(this.right);		
				TurnType _leftturn = this.right.Turn(this.left.Inicio);
				TurnType _vertexturn = this.right.Turn(this.vertex);

				Segment _aux = new Segment(this.right.Inicio, it);
				Segment _temp = new Segment(left.Inicio, it);
				if(!_temp.Contrario(left))
				{
					if(_leftturn == _vertexturn || _leftturn == TurnType.colinear)
					{
						this.bisector = new Segment(it, this.vertex);
					}
					else
					{
						this.bisector = new Segment(this.vertex, it);
					}
				}
				else
				{
					if(_leftturn == _vertexturn || _leftturn == TurnType.colinear)
					{
						this.bisector = new Segment(this.vertex, it);
					}
					else
					{
						this.bisector = new Segment(it, this.vertex);
					}
				}
			}
			else if(this.left.Coincide(this.right))
			{
				this.bisector = new Segment(this.vertex, -this.left.VectorDirector.Y,
					this.left.VectorDirector.X);
			}
			else
			{
				/*si los segmentos son paralelos y no coinciden*/				
				
				this.bisector = new Segment(this.vertex, this.right.VectorDirector.X,
					this.right.VectorDirector.Y);
			}
		}	
		
		/**/
		public SkeletonVertex(RPoint vertex, Segment left, Segment right, 
			Segment bisector)
		{
			this.vertex = vertex;
			this.left = left;
			this.right = right;
			this.bisector = bisector;
		}
		/**/
		public RPoint Intersection(SkeletonVertex sv)
		{
			try
			{
				if(this.bisector.Coincide(sv.bisector))
				{
					return sv.vertex;
				}
				RPoint _result = this.bisector.Intersection(sv.bisector);
				BigRational _dist;
				if(this.bisector.Contrario(new Segment(this.vertex, _result)))
				{
					_dist = _result.QuadraticDistance(this.vertex);
					return _dist.ToLong == 0 ? this.vertex : null;					
				}
				if(sv.bisector.Contrario(new Segment(sv.vertex, _result)))
				{
					_dist = _result.QuadraticDistance(sv.vertex);					
					return _dist.ToLong == 0 ? sv.vertex : null;
				}
				return _result;
			}
			catch
			{
				return null;
			}
		}
		/**/
		public RPoint Vertex
		{
			get
			{
				return this.vertex;
			}
		}
		/**/
		public bool Reflex
		{
			get
			{
				return this.reflex;
			}
		}
		/**/
		public Segment Left
		{
			get
			{
				return this.left;
			}
		}
		/**/
		public Segment Right
		{
			get
			{
				return this.right;
			}
		}
		/**/
		public Segment Bisector
		{
			get
			{
				return this.bisector;
			}
		}
		/**/
		public SkeletonVertex Parent
		{
			get
			{		
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}
		/**/
		public CircularDoublyConnected List
		{
			get
			{
				return this.belong;
			}
			set
			{
				this.belong = value;
			}
		}
		/**/
		public IntersectionPoint InteractionPoint
		{
			get
			{
				RPoint _adit1 = this.Intersection((SkeletonVertex)this.next);
				RPoint _adit2 = this.Intersection((SkeletonVertex)this.previous);
				BigRational _diste;
				IntersectionPoint _near = null;

				if(_adit1 != null && _adit2 != null)
				{
					BigRational _qdinext = _adit1.QuadraticDistance(this.vertex);  
					BigRational _qdilast = _adit2.QuadraticDistance(this.vertex);

					if(_qdinext < _qdilast)
					{
						_diste = _adit1.Distance(this.Right);
						_near = new IntersectionPoint(_adit1, this, 
							(SkeletonVertex)this.next, false, _diste);
					}
					else
					{
						_diste = _adit2.Distance(this.Right);
						_near = new IntersectionPoint(_adit2, 
							(SkeletonVertex)this.previous,
							this, false, _diste);
					}
				}
				else if(_adit1 != null)
				{
					_diste = _adit1.Distance(this.Right);					
					_near = new IntersectionPoint(_adit1,
						this, (SkeletonVertex)this.next, false, _diste);
				}
				else if(_adit2 != null)
				{
					_diste = _adit2.Distance(this.Right);
					_near = new IntersectionPoint(_adit2,
						(SkeletonVertex)this.previous, this, false, _diste);
				}
				return _near;
			}
		}		
		/**/
		public IntersectionPoint MyIntersectionPoint()
		{
			RPoint _adit1 = this.Intersection((SkeletonVertex)this.next);
			RPoint _adit2 = this.Intersection((SkeletonVertex)this.previous);
			BigRational _diste;
			IntersectionPoint _near = new InfiniteIntersection();
			if(_adit1 != null && _adit2 != null)
			{
				BigRational _dist1 = _adit1.QuadraticDistance(this.vertex);
				BigRational _dist2 = _adit2.QuadraticDistance(this.vertex);
				if(_dist1 < _dist2)
				{
//					if(((SkeletonVertex)this.next).reflex)
//					{
//						_diste = _adit1.EuclidianDistance
//							(((SkeletonVertex)this.next).vertex);
//					}
//					else
					{
						_diste = _adit1.Distance(this.Right);
					}
					_near = new IntersectionPoint(_adit1, this, (SkeletonVertex)this.next,
						false, _diste);
				}
				else
				{
//					if(((SkeletonVertex)this.last).reflex)
//					{
//						_diste = _adit2.EuclidianDistance
//							(((SkeletonVertex)this.last).vertex);
//					}
//					else
					{
						_diste = _adit2.Distance(this.Right);
					}
					_near = new IntersectionPoint(_adit2, (SkeletonVertex)this.previous, this,
						false, _diste);
				}
			}
			else if(_adit1 != null)
			{
//				if(((SkeletonVertex)this.next).reflex)
//				{
//					_diste = _adit1.EuclidianDistance
//						(((SkeletonVertex)this.next).vertex);
//				}
//				else
				{
					_diste = _adit1.Distance(this.Right);
				}
				_near = new IntersectionPoint(_adit1, this, (SkeletonVertex)this.next,
					false, _diste);
			}
			else if(_adit2 != null)
			{
//				if(((SkeletonVertex)this.last).reflex)
//				{
//					_diste = _adit2.EuclidianDistance
//						(((SkeletonVertex)this.last).vertex);
//				}
//				else
				{
					_diste = _adit2.Distance(this.Right);
				}
				_near = new IntersectionPoint(_adit2, (SkeletonVertex)this.previous, this,
					false, _diste);
			}
			return _near;
		}
		/**/
		public IntersectionPoint GlobalIntersection()
		{			
			RPoint _adit1 = this.Intersection((SkeletonVertex)this.next);
			RPoint _adit2 = this.Intersection((SkeletonVertex)this.previous);
			IntersectionPoint _near = null;
			BigRational _diste;
			if(_adit1 != null && _adit2 != null)
			{
				BigRational _dist1 = _adit1.QuadraticDistance(this.vertex);
				BigRational _dist2 = _adit2.QuadraticDistance(this.vertex);
				if(_dist1 < _dist2)
				{
					if(this.FirstIntersection((SkeletonVertex)this.next, 
						(SkeletonVertex)this.next.next, _dist1))
					{
						_diste = _adit1.Distance(this.Right);					
						_near = new IntersectionPoint(_adit2, (SkeletonVertex)this.next, this,
							false, _diste);
					}
				}
				else 
				{
					if(this.FirstIntersection((SkeletonVertex)this.previous, 
						(SkeletonVertex)this.previous.previous, _dist2))
					{
						_diste = _adit2.Distance(this.Right);					
						_near = new IntersectionPoint(_adit2, (SkeletonVertex)this.previous, this,
							false, _diste);
					}
				}
			}
			else if(_adit1 != null)
			{	
				BigRational _dist1 = _adit1.QuadraticDistance(this.vertex);
				if(this.FirstIntersection((SkeletonVertex)this.next, 
					(SkeletonVertex)this.next.next, _dist1))
				{
					_diste = _adit1.Distance(this.Right);				
					_near = new IntersectionPoint(_adit1, this, (SkeletonVertex)this.next,
						false, _diste);
				}
			}
			else if(_adit2 != null)
			{
				BigRational _dist2 = _adit2.QuadraticDistance(this.vertex);
				if(this.FirstIntersection((SkeletonVertex)this.previous, 
					(SkeletonVertex)this.previous.previous, _dist2))
				{
					_diste = _adit2.Distance(this.Right);				
					_near = new IntersectionPoint(_adit2, (SkeletonVertex)this.previous, this,
						false, _diste);
				}
			}
			return _near;
		}
		/**/
		public IntersectionPoint ReflexInteractionPoint(Segment[] _osegs, Segment peak1, 
			Segment peak2, SkeletonVertex[] orsk)
		{
			IntersectionPoint _near = null;

			RPoint _adit1 = this.Intersection((SkeletonVertex)this.next);
			RPoint _adit2 = this.Intersection((SkeletonVertex)this.previous);
			BigRational _euclidist = null;

			if(_adit1 != null && _adit2 != null)
			{
				BigRational _dist1 = _adit1.EuclidianDistance(this.Vertex);
				BigRational _dist2 = _adit2.EuclidianDistance(this.Vertex);
				if(_dist1 < _dist2)
				{
					_euclidist = _dist1;
					_dist1 = _adit1.Distance(this.right);
					_near = new IntersectionPoint(_adit1, this, 
						(SkeletonVertex)this.next, false, _dist1);
				}
				else
				{
					_euclidist = _dist2;
					_dist2 = _adit2.Distance(this.right);
					_near = new IntersectionPoint(_adit2, 
						(SkeletonVertex)this.previous,
						this, false, _dist2);
				}
			}
			else if(_adit1 != null)
			{
				
				BigRational _dist1 = _adit1.EuclidianDistance(this.Vertex);
				_euclidist = _dist1;
				_dist1 = _adit1.Distance(this.right);
				_near = new IntersectionPoint(_adit1,
					this, (SkeletonVertex)this.next, false, _dist1);
			}
			else if(_adit2 != null)
			{				
				BigRational _dist2 = _adit2.EuclidianDistance(this.Vertex);
				_euclidist = _dist2;
				_dist2 = _adit2.Distance(this.right);
				_near = new IntersectionPoint(_adit2,
					(SkeletonVertex)this.previous, this, false, _dist2);
			}
			//////parche que no me gusta
			if(this.next.next == this.previous)
			{
				return _near;
			}
			if(this.next == this.previous)
			{
				return new IntersectionPoint(((SkeletonVertex)this.next).vertex,
					this, (SkeletonVertex)this.next, 
					false, ((SkeletonVertex)this.next).vertex.Distance(this.right));
			}
			////////////////////////////////
			for(uint u = 0; u < _osegs.Length; u++)
			{
				Segment seg = _osegs[u];
				if(seg.Turn(this.vertex) == TurnType.right)
				{
					continue;
				}
				if(seg != this.Left && seg != this.Right && seg != peak1 && seg != peak2)
				{
					/*Esta pregunta la hago para saber si la linea se
					 *  intersecta con la bisectriz*/
					if(!this.Bisector.Paralelo(seg))
					{
						RPoint _itbis = this.Bisector.Intersection(seg);
						if(!this.Bisector.Contrario(new Segment(this.Vertex, _itbis)) &&
							!this.Left.Paralelo(seg))
						{
							Segment _bis = this.Left.Bisector(seg);
								
							RPoint _itpt = _bis.Intersection(this.Bisector);
							if(!this.bisector.Contrario(new Segment(this.vertex, _itpt)))
							{
								SkeletonVertex _opp = orsk[u];
								BigRational _aux = _itpt.EuclidianDistance(this.Vertex);
								if(((Object)_euclidist) == null || _euclidist > _aux)
								{
									_near = new IntersectionPoint(_itpt,
										this, _opp, true,
										_itpt.Distance(this.right));
									_euclidist = _aux;
								}
//								IntersectionPoint it = new IntersectionPoint(_itpt,
//									this, _opp, true,
//									_itpt.EuclidianDistance(this.Vertex));
//
//								_near = IntersectionPoint.Nearest(_near, it);
							}
						}									
								
					}
				}
			}			
			return _near;
		}
		
		/**/
		public bool FirstIntersection(SkeletonVertex test, SkeletonVertex possible, 
			BigRational dist)
		{
			return false;
		}
		/**/
		private SkeletonVertex RightIntersection()
		{
			SkeletonVertex _nextnodo = (SkeletonVertex)this.next;
			SkeletonVertex _result = null;
			BigRational dist = null;
			RPoint _it = _nextnodo.Intersection(this);
			if(_it != null)
			{				
				RPoint _nit = _nextnodo.Intersection((SkeletonVertex)_nextnodo.next);
				if(_nit != null)
				{					
					if(_nit.QuadraticDistance(_nextnodo.vertex) >= _it.QuadraticDistance(_nextnodo.vertex))
					{
						_result = _nextnodo;
						dist = _it.QuadraticDistance(this.vertex);
//						_result.qdist = dist;
					}					
				}
				else
				{
					_result = _nextnodo;
					dist = _it.QuadraticDistance(this.vertex);
//					_result.qdist = dist;
				}
			}		
			SkeletonVertex _lchild = _nextnodo.lchild;
			while(_lchild != null)
			{
				_it = _lchild.Intersection(this);
				if(_it != null)
				{
					if(_it.QuadraticDistance(_lchild.vertex) < _lchild.qdist)
					{
						BigRational _aux = _it.QuadraticDistance(this.vertex);
						if((object)dist == null || ((object)dist != null && _aux < dist))
						{
							_result = _lchild;
//							_result.qdist = _aux;
							dist = _aux;
						}
					}						
				}
				_lchild = _lchild.lchild;
			}
			return _result;

		}
		/**/
		private SkeletonVertex LeftIntersection()
		{
			
			SkeletonVertex _lastnodo = (SkeletonVertex)this.previous;
			SkeletonVertex _result = null;
			BigRational dist = null;
			RPoint _it = _lastnodo.Intersection(this);
			if(_it != null)
			{				
				RPoint _nit = _lastnodo.Intersection((SkeletonVertex)_lastnodo.previous);
				if(_nit != null)
				{					
					if(_nit.QuadraticDistance(_lastnodo.vertex) >= _it.QuadraticDistance(_lastnodo.vertex))
					{
						_result = _lastnodo;
						dist = _it.QuadraticDistance(this.vertex);
//						_result.qdist = dist;
					}					
				}
				else
				{
					_result = _lastnodo;
					dist = _it.QuadraticDistance(this.vertex);
//					_result.qdist = dist;
				}
			}		
			SkeletonVertex _rchild = _lastnodo.rchild;
			while(_rchild != null)
			{
				_it = _rchild.Intersection(this);
				if(_it != null)
				{
					if(_it.QuadraticDistance(_rchild.vertex) < _rchild.qdist)
					{
						BigRational _aux = _it.QuadraticDistance(this.vertex);
						if((object)dist == null || ((object)dist != null && _aux < dist))
						{
							_result = _rchild;
//							_result.qdist = _aux;
							dist = _aux;
						}
					}
				}
				_rchild = _rchild.rchild;
			}
			return _result;

		}
		/**/
		public IntersectionPoint Global(RBTree queue)
		{		
			IntersectionPoint _near = null;				
			SkeletonVertex _right = this.RightIntersection();
			SkeletonVertex _left = this.LeftIntersection();
		

			if(_right != null && _left != null)
			{
				RPoint _itright = this.Intersection(_right);
				RPoint _itleft = this.Intersection(_left);
				BigRational _rightdist = this.vertex.QuadraticDistance(_itright);
				BigRational _leftdist = this.vertex.QuadraticDistance(_itleft);
				if(_rightdist <= _leftdist)
				{
					RPoint _it = this.Intersection(_right);
//					this.qdist = _rightdist;
//					_right.qdist = _it.QuadraticDistance(_right.vertex);
					BigRational _diste = _it.Distance(this.Right);						
					_near = new IntersectionPoint(_it, this, _right, false, _diste);						
				}
				else
				{
					RPoint _it = this.Intersection(_left);
//					this.qdist = _leftdist;
//					_left.qdist = _it.QuadraticDistance(_left.vertex);
					BigRational _diste = _it.Distance(this.Right);						
					_near = new IntersectionPoint(_it, _left, this, false, _diste);						
				}
			}
			else if(_right != null)
			{
				RPoint _it = this.Intersection(_right);
//				this.qdist = this.vertex.QuadraticDistance(_right.vertex);
//				_right.qdist = _it.QuadraticDistance(_right.vertex);
				BigRational _diste = _it.Distance(this.Right);						
				_near = new IntersectionPoint(_it, this, _right, false, _diste);					
				
			}
			else if(_left != null)
			{
				RPoint _it = this.Intersection(_left);
//				this.qdist = this.vertex.QuadraticDistance(_left.vertex);
//				_left.qdist = _it.QuadraticDistance(_left.vertex);
				BigRational _diste = _it.Distance(this.Right);						
				_near = new IntersectionPoint(_it, _left, this, false, _diste);						
				
			}
			return _near;
		}
	}

    public class SkVertex : Linked
    {
        RPoint point;
        Segment left, right, bisector;
        SkVertex parent;
        ArrayList children = new ArrayList();
        ArrayList intersecPoints = new ArrayList();
		public bool Painted;
		
		
        public ArrayList IntersecPoints
        {
            get { return intersecPoints; }
            set { intersecPoints = value; }
        }

        public RPoint Point
        {
            get { return point; }
            set { point = value; }
        }

        public Segment Bisector
        {
            get { return bisector; }
            set { bisector = value; }
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

        public SkVertex Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public ArrayList Children
        {
            get { return children; }
            set { children = value; }
        }
		public bool IsLeaf
		{
			get
			{
				return this.children.Count == 0;
			}
		}
		public bool LastChild
		{
			get
			{
				return this.parent.children.IndexOf(this) == this.parent.children.Count - 1;
			}
		}
        public SkVertex(RPoint point, Segment leftSegment, Segment rightSegment, Segment bisectorSegment)
        {
            this.point = point;
            this.left = leftSegment;
            this.right = rightSegment;
            this.bisector = bisectorSegment;
        }

        public SkVertex(RPoint point, SkVertex leftTree, SkVertex rigthTree)
        {
            this.point = point;
            
            leftTree.parent = this;
            rigthTree.parent = this;
            
            children.AddRange(new object[] { leftTree, rigthTree });

            this.left = leftTree.left;
            this.right = rigthTree.right;

			ComputeBisector();
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

        public void AddChildByLeft(SkVertex leftTree)
        {
            children.Insert(0, leftTree);
            leftTree.parent = this;
            this.left = leftTree.left;
            ComputeBisector();
        }

        public void AddChildByRight(SkVertex rightTree)
        {
            children.Add(rightTree);
            rightTree.parent = this;
            this.right = rightTree.right;
            ComputeBisector();
        }

        public void DiscardIntersecPoints()
        {
            if (intersecPoints.Count == 2)
            {
                IntersecPoint p1 = intersecPoints[0] as IntersecPoint;
                IntersecPoint p2 = intersecPoints[1] as IntersecPoint;

                if (BisectorContainsPoint(p1.Point) && BisectorContainsPoint(p2.Point))
                {
                    if (p1.Point.QuadraticDistance(point) < p2.Point.QuadraticDistance(point))
                        p2.Discarted = true;
                    else
                        p1.Discarted = true;
                }
            }
        }

        //public IntersecPoint GetFirstIntersection()
        //{
        //    //puntos de intersección en la raíz ..
        //    IntersecPoint rp1 = null;
        //    IntersecPoint rp2 = null;

        //    #region Intersección con el de la izquierda ..

        //    //analizar la intersección del de la izquierda
        //    SkVertex treeLeft = this.previous as SkVertex;

        //    SkVertex vCursor = this;
        //    while (vCursor != null)
        //    {
        //        //si pasa por el punto ..
        //        if (treeLeft.BisectorContainsPoint(vCursor.point))
        //        {
        //            return new IntersecPoint(
        //                    vCursor.point.Distance(treeLeft.left),
        //                    vCursor.point,
        //                    treeLeft,
        //                    vCursor,
        //                    IntersecPointType.Right
        //                );
        //        }

        //        //si el nodo tiene padre ..
        //        else if (vCursor.parent != null)
        //        {
        //            //si el Bisector corta el segmento vCursor--vCursor.Parent ?
        //            RPoint ip = treeLeft.BisectorIntersectsSegment(vCursor.point, vCursor.parent.point);
        //            if (ip != null)
        //            {
        //                return new IntersecPoint(
        //                        ip.Distance(treeLeft.left),
        //                        ip,
        //                        treeLeft,
        //                        vCursor,
        //                        IntersecPointType.NewRight
        //                    );
        //            }
        //        }

        //        //si corta el Bisector de la raíz
        //        else
        //        {
        //            RPoint ip = treeLeft.bisector.Intersection2(vCursor.bisector);
        //            if (ip != null)
        //            {
        //                Segment s1 = new Segment(treeLeft.point, ip);
        //                Segment s2 = new Segment(vCursor.point, ip);

        //                if (!s1.Contrario(treeLeft.bisector) && !s2.Contrario(vCursor.bisector))
        //                {
        //                    rp1 = new IntersecPoint(
        //                            ip.Distance(treeLeft.left),
        //                            ip,
        //                            treeLeft,
        //                            vCursor,
        //                            IntersecPointType.NewRight
        //                        );
        //                }
        //            }
        //        }

        //        vCursor = (vCursor.children.Count == 0) ? null : vCursor.children[0] as SkVertex;
        //    }

        //    #endregion

        //    #region Intersección con el de la derecha ..

        //    //analizar la intersección del de la izquierda
        //    SkVertex treeRight = this.next as SkVertex;

        //    vCursor = this;
        //    while (vCursor != null)
        //    {
        //        //si pasa por el punto ..
        //        if (treeRight.BisectorContainsPoint(vCursor.point))
        //        {
        //            return new IntersecPoint(
        //                    vCursor.point.Distance(treeRight.right),
        //                    vCursor.point,
        //                    vCursor,
        //                    treeRight,
        //                    IntersecPointType.Left
        //                );
        //        }

        //        //si el nodo tiene padre ..
        //        else if (vCursor.parent != null)
        //        {
        //            //si el Bisector corta el segmento vCursor--vCursor.Parent ?
        //            RPoint ip = treeRight.BisectorIntersectsSegment(vCursor.point, vCursor.parent.point);
        //            if (ip != null)
        //            {
        //                return new IntersecPoint(
        //                        ip.Distance(treeRight.right),
        //                        ip,
        //                        vCursor,
        //                        treeRight,
        //                        IntersecPointType.NewLeft
        //                    );
        //            }
        //        }

        //        //si corta el Bisector de la raíz
        //        else
        //        {
        //            RPoint ip = treeRight.bisector.Intersection2(vCursor.bisector);
        //            if (ip != null)
        //            {
        //                Segment s1 = new Segment(treeRight.point, ip);
        //                Segment s2 = new Segment(vCursor.point, ip);

        //                if (!s1.Contrario(treeRight.bisector) && !s2.Contrario(vCursor.bisector))
        //                {
        //                    rp2 = new IntersecPoint(
        //                            ip.Distance(treeRight.right),
        //                            ip,
        //                            vCursor,
        //                            treeRight,
        //                            IntersecPointType.NewLeft
        //                        );
        //                }
        //            }
        //        }

        //        vCursor = (vCursor.children.Count == 0) ? null : vCursor.children[children.Count - 1] as SkVertex;
        //    }

        //    #endregion

        //    //devolver el más cercano entre rp1 y rp2
        //    if (rp1 == null)
        //        return rp2;
        //    else if (rp2 == null)
        //        return rp1;
        //    else
        //        return (rp1.Point.QuadraticDistance(point) < rp2.Point.QuadraticDistance(point)) ? rp1 : rp2;
        //}


        public bool BisectorContainsPoint(RPoint p)
        {
//            BigRational epsilon = new BigRational(0, 1, 1000000);

            BigRational det = (bisector.Fin.Y - bisector.Inicio.Y) * (p.X - bisector.Fin.X) - 
                (bisector.Fin.X - bisector.Inicio.X) * (p.Y - bisector.Fin.Y);
            //BigRational det = bisector.VectorDirector.Determinante(p);

            //si el punto está en la recta ..
            if (-RPoint.epsilon < det && det < RPoint.epsilon)
            {
                //si está en la dirección ..
                Segment vd = new Segment(this.point, p);

                return !bisector.Contrario(vd);
            }

            return false;
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

        public SkVertex CreateInfinityParent()
        {
            RPoint vd = this.bisector.VectorDirector;
            RPoint infinity = this.point + new RPoint(10000 * vd.X, 10000 * vd.Y);
            SkVertex p = new SkVertex(infinity, this.left, this.right, this.bisector);
            p.children.Add(this);
            this.parent = p;
            return p;
        }
		public SkVertex Minimun
		{
			get
			{
				return (this.children.Count > 0) ? (this.children[0] as SkVertex).Minimun : this; 

			}
		}
    }

    public enum IntersecPointType
    {
        Left,
        Right,
        NewLeft,
        NewRight
    }

    public class IntersecPoint
    {
        private BigNumbers.BigRational distance;
        private RPoint point;
        private SkVertex vLeft; //Left
        private SkVertex vRight; //rigth
        private IntersecPointType type;
        private bool isNewRoot = false;
        bool discarted = false;

        public bool Discarted
        {
            get { return discarted; }
            set { discarted = value; }
        }

        public bool IsNewRoot
        {
            get { return isNewRoot; }
            set { isNewRoot = value; }
        }

        public IntersecPoint(BigNumbers.BigRational distance, RPoint point,
            SkVertex vleft, SkVertex vrigth, IntersecPointType type)
        {
            this.distance = distance;
            this.point = point;
            this.vLeft = vleft;
            this.vRight = vrigth;
            this.type = type;
        }

        public IntersecPointType Type
        {
            get { return type; }
            set { type = value; }
        }

        public BigNumbers.BigRational Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public RPoint Point
        {
            get { return point; }
            set { point = value; }
        }

        public SkVertex VRight
        {
            get { return vRight; }
            set { vRight = value; }
        }

        public SkVertex VLeft
        {
            get { return vLeft; }
            set { vLeft = value; }
        }
    }
}
