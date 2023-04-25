using System;
using BigNumbers;
using System.Collections;

namespace Growth_Polygon
{
	/// <summary>
	/// 
	/// </summary>
	public class PlaneSweep
	{
		private StatusTree ST;
		private EventQueue EQ;

		void FindNewEvent( Segment S1, Segment S2, Event Ev)
		{
			if( !Segment.Intersect( S1.GetUpperEnd(), S1.GetLowerEnd(),
				                    S2.GetUpperEnd(), S2.GetLowerEnd() ) ) return;

			R_Point Pt0 = Segment.Intersection( S1, S2 );

			R_Point P = Ev.GetPoint();

			Event E =null;
			Event E1=null;
			
			if( ( Pt0.y < P.y ) || 
				( ( Pt0.y == P.y ) &&
				( Pt0.x  > P.x ) ) )
			{//Se cortan despues de la sweepline
				if( ( Pt0.x != S1.GetLowerEnd().x ) ||
					( Pt0.y != S1.GetLowerEnd().y ) )
				{
					E = new Event(Pt0,S1);
					E.Add_S_Open(S1);
				}
				else
				{
					E = new Event(Pt0,S2);
					if( ( Pt0.x != S2.GetLowerEnd().x ) ||
						( Pt0.y != S2.GetLowerEnd().y ) )
					{						
						E.Add_S_Open(S2);
					}
				}
				
				if( E != null ) EQ.AddEvent( E,out E1 );				
			}
			return;	
		}
		private void HandleEvent( Event e, out ArrayList Seg_Close, out ArrayList Seg_Open, out Segment[] Extremes )
		{
			ST.setKey( e.GetPoint() );
			RBNode Temp = null;
			RBNode node1,node2;
			bool init = false,end = false;

			Extremes = new Segment[2];				
			int i;

			Seg_Open = e.Get_S_Open();
			ArrayList Aux = new ArrayList();
			Seg_Close = new ArrayList();

			Segment S_Close =  e.Get_S_Close();
			if( S_Close == null )
			{
				node1 = ST.FindPoint(e.GetPoint());
				
				if( node1 != null )
				{
					S_Close = node1.Value() as Segment;
					if( ( S_Close.GetLowerEnd().x != e.GetPoint().x ) ||
						( S_Close.GetLowerEnd().y != e.GetPoint().y ) )
					{
						if( Seg_Open == null ) Seg_Open = new ArrayList();
						Seg_Open.Add( S_Close );
					}
				}
			}

			if( S_Close != null )
			{
				ST.SetCompareBefore();
				node1 = ST.Find(S_Close);
				ST.SetCompareAfter();
				node2=node1;

				do 
				{
					node2 = ST.Antecessor(node2);
					if( node2 != null) 
					{
						Aux.Insert(0,node2);
						if( ( ( (Segment)node2.Value() ).Contain(e.GetPoint() ) ) &&
							( ( ( (Segment)node2.Value() ).GetLowerEnd().x != e.GetPoint().x ) ||
							  ( ( (Segment)node2.Value() ).GetLowerEnd().y != e.GetPoint().y ) ) )
						{
							if( Seg_Open == null ) Seg_Open = new ArrayList();
							Seg_Open.Add( ( (Segment)node2.Value() ) );
						}
					}
				}while( ( node2 != null ) && ( ( (Segment)node2.Value() ).Contain(e.GetPoint() ) ) );

				init = node2 != null;
				
				node2 = node1;
				Aux.Add(node2);

				do 
				{					
					node2 = ST.Successor(node2);						
					if( node2 != null) 
					{
						Aux.Add(node2);
						if( ( ( (Segment)node2.Value() ).Contain(e.GetPoint() ) ) &&
							( ( ( (Segment)node2.Value() ).GetLowerEnd().x != e.GetPoint().x ) ||
							  ( ( (Segment)node2.Value() ).GetLowerEnd().y != e.GetPoint().y ) ) )
						{
							if( Seg_Open == null ) Seg_Open = new ArrayList();
							Seg_Open.Add( ( (Segment)node2.Value() ) );
						}
					}
				}while( ( node2 != null ) && ( ( (Segment)node2.Value() ).Contain(e.GetPoint() ) ) );

				end = node2 != null;

				int I,E;

				if( init ) 	I=1;
				else 		I=0;

				if( end ) 	E=Aux.Count-1;
				else 		E=Aux.Count;

				for( i=I;i<E;i++)
				{
					Seg_Close.Add(((RBNode)Aux[i]).Value());
					if( ( ((Segment)((RBNode)Aux[i]).Value()).GetLowerEnd().x != e.GetPoint().x ) ||
						( ((Segment)((RBNode)Aux[i]).Value()).GetLowerEnd().y != e.GetPoint().y ) )
					{
						((Segment)((RBNode)Aux[i]).Value()).SetUpperEnd( e.GetPoint() );
					}


					ST.Delete((RBNode)Aux[i]);
				}
			}

			if( ( Seg_Open != null ) && ( Seg_Open.Count > 0 ) )
			{
				for(i=0;i<Seg_Open.Count;i++)
				{
					Object Out;
					ST.Insert(Seg_Open[i],out Out);
					
					if( ( i==0 ) && ( S_Close == null ) )
					{
						RBNode Auxiliar = ST.Antecessor((RBNode)Out);
						if( Auxiliar != null ) 
						{
							Aux.Add( Auxiliar );
							init = true;
						}
						
						Auxiliar = ST.Successor((RBNode)Out);
						if( Auxiliar != null ) 
						{
							Aux.Add( Auxiliar );
							end = true;
						}
					}

					if( ( (Segment)Seg_Open[i]).GetUpperEnd() != e.GetPoint() )
					{
						( (Segment)Seg_Open[i]).SetUpperEnd( e.GetPoint() );
					}
				}
				
				Seg_Open.Clear();
				Temp = null;
				Extremes[0] = Extremes[1] = null;
					
				if( init )
				{
					Extremes[0] = (Segment)((RBNode)Aux[0]).Value();
					Temp = Aux[0] as RBNode;
				}
				
				if( end )
				{
					Extremes[1] = (Segment)((RBNode)Aux[Aux.Count-1]).Value();					
				}

				Temp = ST.Successor( Temp );					
				while( ( Temp != null ) && ( Temp.Value() != Extremes[1] ) )
				{
					if( Temp != null ) Seg_Open.Add(Temp.Value());
					Temp = ST.Successor( Temp );					
				}
				

				
				node1 = node2 = null;

				if( init ) 
				{
					node1 = ST.Successor((RBNode)Aux[0]);
					FindNewEvent(Extremes[0],(Segment)node1.Value(),e);
				}
				if( end ) 
				{
					node2 = ST.Antecessor((RBNode)Aux[Aux.Count-1]);
					FindNewEvent(Extremes[1],(Segment)node2.Value(),e);
				}
				
			}
			else
			{
				Seg_Open = new ArrayList();
				
				if( init )
					Extremes[0] = (Segment)((RBNode)Aux[0]).Value();

				if( end )
					Extremes[1] = (Segment)((RBNode)Aux[Aux.Count-1]).Value();

				if( init && end )
				{
					FindNewEvent((Segment)((RBNode)Aux[0]).Value(),
						         (Segment)((RBNode)Aux[Aux.Count-1]).Value(),
						          e);
				}
			}
	}
		public PlaneSweep( EventQueue eq )
		{
			EQ = eq;
			ST = new StatusTree();
			if(!EQ.isEmpty() ) EQ.Init();
		}
		public bool NextStep( out R_Point pt, out ArrayList Seg_Close, out ArrayList Seg_Open, out Segment[] Extremes )
		{
			pt = null;
			Extremes = null;
			Seg_Open = null;
			Seg_Close = null;

			if( EQ.Actual() == null) return false;
            
			HandleEvent( EQ.Actual(),out Seg_Close,out Seg_Open,out Extremes );
			pt = EQ.Actual().GetPoint();
			EQ.NextEvent();
			return true;
		}
	}
	/// <summary>
	/// 
	/// </summary>
	public class EventQueue
	{
		public void Init()
		{
			Act = Set.Successor( null );
		}
		public Event Actual()
		{
			if( Act == null ) return null;

			return (Event)Act.Value();
		}
		public bool isEmpty()
		{
			return Set.isEmpty();
		}
		public Event NextEvent()
		{
			RBNode Temp = Act;
	
			Act = Set.Successor( Act );
			Set.Delete( Temp );
	
			if( Act != null ) 
				return (Event)Act.Value();
			else
				return null;
		}
		public void AddEvent( Event e, out Event Result )
		{
			if( Set == null ) Set = new EventTree();

			Object Aux;

			bool res = Set.Insert( e,out Aux );

			Result = (Event)Aux;
			if( !res )
			{
				if( Result.Get_S_Close() == null ) Result.Set_S_Close(e.Get_S_Close());
				Result.Add_S_Open(e.Get_S_Open());
			}
		}
		public EventQueue()
		{
			Act = null;
			Set = null;
		}
		
		private RBNode Act;
		private EventTree Set;
	}

		
	#region Trees
	/// <summary>
	/// 
	/// </summary>
	public class RBNode  
	{
		private const bool Before = true;
		private const bool After = false;
		protected const bool Red = true;
		protected const bool Black = false;
	
		public void Empty( RBNode Nil )
		{
			if( LChild != Nil ) LChild.Empty(Nil);
			if( RChild != Nil ) RChild.Empty(Nil);	
		
			RChild = null;	
			LChild = null;
		}	
		public void setValue( Object elem )
		{
			value = elem;
		}	
		public void setLeftChild( RBNode child )
		{
			LChild = child;
		}	
		public void setRightChild( RBNode child )
		{
			RChild = child;
		}	
		public void setParent( RBNode P )
		{
			parent = P;
		}	
		public void setColor( bool C )
		{
			color = C;
		}	
		public bool Color()
		{
			return color;
		}	
		public Object Value()
		{
			return value;
		}	
		public RBNode Parent()
		{
			return parent;
		}	
		public RBNode RightChild()
		{
			return RChild;
		}	
		public RBNode LeftChild()
		{
			return LChild;
		}	
		public RBNode()
		{
			RChild = LChild = parent = null;
			color  = Black;
			value = null;
		}	
		public RBNode( RBNode LC, RBNode RC, RBNode P, bool C, Object elem)
		{
			LChild = LC;
			RChild = RC;
			parent = P ;
			color  = C ;
			value = elem;
		}	
				
		private bool color;
		private RBNode parent;
		private RBNode RChild;
		private RBNode LChild;
		private Object value;
	};

	/// <summary>
	/// 
	/// </summary>
	public abstract class RBTree  
	{
		private const bool Before = true;
		private const bool After = false;
		protected const bool Red = true;
		protected const bool Black = false;
		public RBTree()
		{
			Nil = new RBNode();
			Root = Nil;
		}

		public bool isEmpty()
		{
			return Root == null;
		}
		
		public RBNode Antecessor( RBNode z )
		{
			if( z == null ) z = Nil;

			if( z == Nil )
			{
				z = Root;
				while( z.RightChild() != Nil )
					z = z.RightChild();
				return z;
			}

			RBNode y = z;
			RBNode x = z.LeftChild();

			while( x != Nil )
			{
				y = x;
				x = x.RightChild();
			}

			if( y != z )
				return y;
			else
			{
				while( ( z != Root ) && ( z != z.Parent().RightChild() ) )
					z = z.Parent();

				if( z == Root )
					return null;
				else
					return z.Parent();
			}
		}
		
		public RBNode Successor( RBNode z )
		{
			if( z == null ) z = Nil;

			if( z == Nil )
			{
				z = Root;
				while( z.LeftChild() != Nil )
					z = z.LeftChild();
				return z;
			}

			RBNode y = z;
			RBNode x = z.RightChild();

			while( x != Nil )
			{
				y = x;
				x = x.LeftChild();
			}

			if( y != z )
				return y;
			else
			{
				while( ( z != Root ) && ( z != z.Parent().LeftChild() ) )
					z = z.Parent();

				if( z == Root )
					return null;
				else
					return z.Parent();
			}
		}
		
		public void Delete( RBNode z )
		{
			bool color = Red;
			RBNode y;
			RBNode x;

			if( ( z.LeftChild() == Nil ) ||
				( z.RightChild() == Nil ) )
				y = z;
			else
				y = Successor( z );

			if( y.LeftChild() != Nil )
				x = y.LeftChild();
			else
				x = y.RightChild();

			x.setParent( y.Parent() );

			if( y.Parent() == Nil )
			{
				Root = x;
			}
			else
			{
				if( y == y.Parent().LeftChild() )
					y.Parent().setLeftChild( x );
				else
					y.Parent().setRightChild( x );
			}

			if( y != z )
			{
				y.setParent( z.Parent() );
				y.setLeftChild( z.LeftChild() );
				y.setRightChild( z.RightChild() );

				y.LeftChild().setParent( y );
				y.RightChild().setParent( y );

				color = y.Color();
				y.setColor( z.Color() );
		
				if( z == Root )	
					Root = y;
				else
				{
					if( z == z.Parent().LeftChild() )
						z.Parent().setLeftChild( y );
					else
						z.Parent().setRightChild( y );		
				}
			}
			else color = y.Color();

			if( color == Black )
				DeleteFixUp( x );

			Nil.setParent( null );
			Nil.setLeftChild( null );
			Nil.setRightChild( null );
		}
		
		public abstract bool Insert(Object Elem, out Object Result);
		public abstract RBNode Find( Object elem );			
		protected abstract int Compare( Object E1, Object E2 );
		protected void DeleteFixUp( RBNode x )
		{
			RBNode w;

			while( ( x != Root ) && 
				( x.Color() == Black ) )
			{
				if( x == x.Parent().LeftChild() )
				{
					w = x.Parent().RightChild();
					if( w.Color() == Red )
					{
						w.setColor( Black );
						x.Parent().setColor( Red );
						LeftRotate( x.Parent() );
						w = x.Parent().RightChild();
					}
		
					if( w != Nil )
					{
						if( ( w.LeftChild().Color() == Black ) &&
							( w.RightChild().Color() == Black ) )
						{
							w.setColor( Red );
							x = x.Parent();
						}
						else
						{
							if( w.RightChild().Color() == Black )
							{
								w.LeftChild().setColor( Black );
								w.setColor( Red );
								RightRotate( w );
								w = x.Parent().RightChild();
							}
					
							if( w != Nil )
							{
								w.setColor( x.Parent().Color() );
								x.Parent().setColor( Black );
								w.RightChild().setColor( Black );
								LeftRotate( x.Parent() );
							}
					
							x = Root;
						}
					}
					else x = Root;
				}
				else
				{
					w = x.Parent().LeftChild();
					if( w.Color() == Red )
					{
						w.setColor( Black );
						x.Parent().setColor( Red );
						RightRotate( x.Parent() );
						w = x.Parent().LeftChild();
					}
		
					if( w != Nil )
					{
						if( ( w.RightChild().Color() == Black ) &&
							( w.LeftChild().Color() == Black ) )
						{
							w.setColor( Red );
							x = x.Parent();
						}
						else
						{
							if( w.LeftChild().Color() == Black )
							{
								w.RightChild().setColor( Black );
								w.setColor( Red );
								LeftRotate( w );
								w = x.Parent().LeftChild();
							}
					
							if( w != Nil )
							{
								w.setColor( x.Parent().Color() );
								x.Parent().setColor( Black );
								w.LeftChild().setColor( Black );
								RightRotate( x.Parent() );
							}
							x = Root;
						}
					}
					else x = Root;
				}
			}
			x.setColor( Black );
		}
		
		protected void RightRotate( RBNode x )
		{
			RBNode y = x.LeftChild();
	
			x.setLeftChild( y.RightChild() );
			y.RightChild().setParent( x );
			y.setParent( x.Parent() );

			if( x.Parent() == Nil )
			{
				Root = y;
			}
			else
			{
				if( x == x.Parent().RightChild() )
					x.Parent().setRightChild( y );
				else
					x.Parent().setLeftChild( y );
			}

			y.setRightChild( x );
			x.setParent( y );
		}
		
		protected void LeftRotate( RBNode x )
		{
			
			RBNode y = x.RightChild();
	
			x.setRightChild( y.LeftChild() );
			y.LeftChild().setParent( x );
			y.setParent( x.Parent() );

			if( x.Parent() == Nil )
			{
				Root = y;
			}
			else
			{
				if( x == x.Parent().LeftChild() )
					x.Parent().setLeftChild( y );
				else
					x.Parent().setRightChild( y );
			}

			y.setLeftChild( x );
			x.setParent( y );
		}
		
		protected void InsertFixUp( RBNode z )
		{
			RBNode y;
	
			while( (z.Parent() != Root ) && ( z.Parent().Color() == Red ) )
			{
				if( z.Parent() == z.Parent().Parent().LeftChild() )
				{
					y = z.Parent().Parent().RightChild();
					if( y.Color() == Red )
					{
						z.Parent().setColor( Black );
						y.setColor( Black );
						z.Parent().Parent().setColor( Red );
						z = z.Parent().Parent();
					}
					else
					{
						if( z == z.Parent().RightChild() )
						{
							z = z.Parent();
							LeftRotate( z );
						}
						z.Parent().setColor( Black );
						z.Parent().Parent().setColor( Red );
						RightRotate( z.Parent().Parent() );
					}
				}
				else
				{
					y = z.Parent().Parent().LeftChild();
					if( y.Color() == Red )
					{
						z.Parent().setColor( Black );
						y.setColor( Black );
						z.Parent().Parent().setColor( Red );
						z = z.Parent().Parent();
					}
					else
					{
						if( z == z.Parent().LeftChild() )
						{
							z = z.Parent();
							RightRotate( z );
						}
						z.Parent().setColor( Black );
						z.Parent().Parent().setColor( Red );
						LeftRotate( z.Parent().Parent() );
					}
				}
			}
			Root.setColor( Black );
		}
		protected RBNode Root;
		protected RBNode Nil;
	};

	/// <summary>
	/// 
	/// </summary>
	public class StatusTree : RBTree  
	{
		private bool Upper;
		private R_Point key;
		private bool AfterLine;
	
		private int ComparePendent( Segment S1, Segment S2 )
		{
			BigRational M1 = S1.GetPendent();
			BigRational M2 = S2.GetPendent();
			int Result = 0;

			if( ( S1.isVertical() ) && ( S2.isVertical() ) )
				return Result;

			int sgnM1 = 1;
			int sgnM2 = 1;

			if( !S1.isVertical() )
			{
				if( M1 == 0 ) 
					sgnM1 = 0;
				else
					if( !M1.sign )  sgnM1 = -1;
			}
			
			if( !S2.isVertical() )
			{
				if( M2 == 0 ) 
					sgnM2 = 0;
				else
					if( !M2.sign )  sgnM2 = -1;
			}

			if( ( S1.isVertical() ) || ( S2.isVertical() ) )
			{
				if( ( ( S1.isVertical() ) && ( sgnM2 <= 0 ) ) ||
					( ( S2.isVertical() ) && ( sgnM1 > 0 ) ) )  
					Result = -1;
				else
					Result =  1;
			}
			else
			{

				if( M1 == M2 ) return Result;

				if( ( sgnM1 != 0 ) &&
					( ( ( sgnM1 != sgnM2 ) &&
					( sgnM2 !=     1 ) ) ||
					( ( sgnM1 == sgnM2 ) &&
					(   M1  <   M2 ) ) ) )
					Result = -1;
				else
					Result =  1;
			}

			if( AfterLine )
				return Result;
			else
				return -Result;
		}
		override protected int Compare( Object E1, Object E2 )
		{
			Segment S1 = E1 as Segment;
			Segment S2 = E2 as Segment;

			if( S1 == S2 ) return 0;
	
			S1.SetKey( key );
			S2.SetKey( key );	

			R_Point P1 = S1.GetKey();
			R_Point P2 = S2.GetKey();

			if( ( P1 == null ) || ( P2 == null ) )
			{
				/* Caso en que alguno de los dos segmentos es horizontal (paralelo a la sweep line)
					 * y las y no coinciden por lo que la interseccion es vacia.
					 */
				throw new Exception("Esta comparando con un segmento que no puede estar en el arbol de estado");
			}

			if( P1.x == P2.x )
			{
				int result = ComparePendent(S1,S2);

				if( result == 0 )
				{			
					if( ( S2.GetEnd().y > S1.GetEnd().y ) ||
						( ( S2.GetEnd().y == S1.GetEnd().y ) &&
						  ( S2.GetEnd().x  < S1.GetEnd().x ) ) )
						return -1;
					else
						return  1;
				}
				else
					return result;
			}
			else
			{
				if( P1.x < P2.x )
					return -1;
				else
					return  1;
			}
		}
		
		public void SetCompareBefore()
		{
			AfterLine = false;
		}

		public void SetCompareAfter()
		{
			AfterLine = true;
		}

		public void SetUpper( bool mode )
		{
			Upper = mode;
		}
		public void setKey( R_Point K )
		{
			key = K;	
		}
		public StatusTree():base()
		{ 			
			Upper = false;
 			AfterLine = true;
		}
		override public bool Insert( Object elem, out Object Result)
		{
			Upper = true;

			RBNode y = Nil;
			RBNode x = Root;

			y = Find( elem );

			if( y.Value() == elem ) 
			{
				Upper = false;
				Result = y;
				return false;
			}
	
			RBNode z = new RBNode(Nil,Nil,Nil,Red,elem);
			z.setParent( y );

			if( y == Nil )
			{
				Root = z;
			}
			else
			{
				if( Compare( y.Value(), z.Value() ) == 1 )
					y.setLeftChild( z );
				else
					y.setRightChild( z );
			}

			InsertFixUp( z );
			Upper = false;
			Result = z;
			return true;
		}
		override public RBNode Find( Object elem )
		{
			RBNode y = Nil;
			RBNode x = Root;

			while( x != Nil )
			{
				y = x;

				int Relation = Compare( x.Value(),elem  );
		
				if( Relation == 0 ) return x;
				if( Relation == 1 )
					x = x.LeftChild();
				else
					x = x.RightChild();
			}

			return y;
		}
		
		public RBNode FindPoint( R_Point elem )
		{
			RBNode y = Nil;
			RBNode x = Root;

			while( x != Nil )
			{
				y = x;

				Segment S = (Segment)x.Value();
				int Relation = -R_Point.SD(S.GetUpperEnd(),S.GetLowerEnd(),elem);
		
				if( Relation == 0 ) return x;
				if( Relation == 1 )
					x = x.LeftChild();
				else
					x = x.RightChild();
			}

			if( x == Nil ) 
				return null;
			else
				return x;
		}
		
		
	};

	/// <summary>
	/// 
	/// </summary>
	public class EventTree : RBTree  
	{
	
		override public bool Insert( Object elem, out Object Result)
		{
			RBNode y = Nil;
			RBNode x = Root;

			y = Find( elem );
	
			if( ( y != Nil ) && ( Compare( y.Value(), elem ) == 0 ) ) 
			{
				Result = y.Value();
				return false;
			}
	
			RBNode z = new RBNode(Nil,Nil,Nil,Red,elem);
			z.setParent( y );

			if( y == Nil )
			{
				Root = z;
			}
			else
			{
				if( Compare( z.Value(), y.Value() ) == 1 )
					y.setLeftChild( z );
				else
					y.setRightChild( z );
		
				InsertFixUp( z );
			}
	
			Result = z.Value();
			return true;			
		}
		override public RBNode Find( Object elem)
		{
			RBNode y = Nil;
			RBNode x = Root;

			while( x != Nil )
			{
				y = x;

				int Comparison =  Compare( elem, x.Value() );
				if( Comparison == 0 )
					return x;

				if( Comparison == 1 )
					x = x.LeftChild();
				else
					x = x.RightChild();
			}

			return y;
		}
		public EventTree():base() 
		{
		}
		override protected int Compare(Object E1, Object E2 )
		{
			Event Ev1 = ( Event )E1;
			Event Ev2 = ( Event )E2;

			R_Point P1 = Ev1.GetPoint();
			R_Point P2 = Ev2.GetPoint();

			
			if( ( P1.x == P2.x ) &&
				( P1.y == P2.y ) )
				return 0;

			if( P1.y > P2.y )
				return 1;
			else
			{
				if( P1.y < P2.y )
					return -1;
				else
				{
					if( P1.x < P2.x )
						return 1;
					else
					{
						if( P1.x > P2.x ) 
							return -1;
						else
							return  0;
					}
				} 
			}			

		}
	};
	#endregion

	#region Nodes
	
	/// <summary>
	/// 
	/// </summary>
	public class Segment
	{
		#region Fields

		private bool myUpper;
		private R_Point key;
		private R_Point Init;
		private R_Point End;
		private R_Point First;
		private R_Point Last;
		private R_Point Upper;
		private Interval Left;
		private Interval Right;
		private BigRational Pendent;
		private bool vertical;
		private bool Reversal;

		#endregion
		
		public Segment()
		{
			Init	= 
				End		= 
				key		= 			
				Upper	=  null;
			Left	= 
				Right	= null;
			myUpper = false;
			Reversal = false;
		}
		public Segment(R_Point I, R_Point E)
		{
			Init = I;
			End = E;
			
			vertical = ( I.x == E.x );
			if( !vertical )
				Pendent = ( End.y - Init.y ) /
					( End.x - Init.x );

			key = null;	
			Left = Right = null;
			Upper = null;

			if( ( Init.y > End.y ) ||
				( ( Init.y == End.y ) && 
				  ( Init.x < End.x ) ) )
			{
				First = Init;
				Last = End;
			}
			else
			{
				First = End;
				Last = Init;
			}	
			myUpper = false;
			Reversal = false;
		}
		public bool isCoincident( Segment S )
		{
			return ( ( R_Point.SD( Init,End,S.GetInit()) == 0 ) &&
				     ( R_Point.SD( Init,End,S.GetEnd() ) == 0 ) );
		}
		public bool isVertical()
		{
			return vertical;
		}
		public void SetKey( R_Point K )
		{
			if( ( Init.y == End.y ) )
			{
				/*
				 * Si el segmento es horizontal pondremos la key en null.
				 */ 
				if(Init.y == K.y)
				{
					if(key == null)
					{
						key = new R_Point();
					}
					key.y = K.y;
					key.x = K.x;
				}
				else key = null;
				return;
			}

			if(key == null)
			{
				key = new R_Point();
			}
						
			if( Init.x == End.x )
			{
				/*
				 * Si entro aqui es porque el segmento es vertical. Por tanto
				 * la sweep line lo corta y la interseccion tiene la misma x.
				 */
				key.x = Init.x;
				key.y = K.y;
				return; 
			}
			
			/*
			 * Si llego aqui el segmento no es horizontal asi que calculo la 
			 * interseccion con la sweep line.
			 */
			//key.x = ( ( Init.x - End.x ) * ( K.y - Init.y  ) ) / ( Init.y - End.y );
			BigRational temp;
			key.x  = ( Init.x - End.x   );
			temp = ( K.y    - Init.y  );
			key.x *= temp;
			BigRational aux = new BigRational(key.x.numerador,key.x.denominador);
			temp = ( Init.y - End.y   );
			key.x /= temp;
			key.x += Init.x;
			key.y = K.y;
			return;
		}
		public Interval GetRight()
		{
			return Right;
		}
		public Interval GetLeft()
		{
			return Left;
		}
		public bool isReversal()
		{
			return Reversal;
		}
		public void SetLeft(Interval L)
		{
			Left = L;
		}
		public void SetRight(Interval R)
		{
			Right = R;
		}
		public void SetReversed()
		{
			Reversal = true;
		}
		public R_Point GetInit()
		{
			return Init;
		}
		public R_Point GetEnd()
		{
			return End;
		}
		public R_Point GetKey()
		{
			return key;
		}
		public R_Point GetUpperEnd()
		{
			if( myUpper)
				return Upper;
			else
				return First;
		}
		public void SetUpperEnd(R_Point pt)
		{
			myUpper = true;
			Upper = pt;
		}
		public R_Point GetLowerEnd()
		{
			return Last;
		}
		public BigRational GetPendent()
		{			
			return Pendent;
		}
		public bool Contain(R_Point Pt)
		{
			R_Point Pt1,Pt2;
			Pt1 = GetInit();
			Pt2 = GetEnd();

			return ( ( ( Pt2.y - Pt1.y ) * ( Pt.x - Pt1.x ) ) == 
				( ( Pt.y - Pt1.y ) * ( Pt2.x - Pt1.x ) ) );
		}
		public static bool Intersect( R_Point P1, R_Point P2, R_Point P3, R_Point P4 )
		{
			return ( ( R_Point.SD(P1,P2,P3) != R_Point.SD(P1,P2,P4) ) &&
				( R_Point.SD(P3,P4,P1) != R_Point.SD(P3,P4,P2) ) );
		}
		public static bool Intersect( Segment S1, Segment S2 )
		{
			return Segment.Intersect(S1.GetInit(),S1.GetEnd(),S2.GetInit(),S2.GetEnd());
		}
		public static R_Point Intersection( Segment S1, Segment S2 )
		{			
			R_Point Result = new R_Point();

			if( ( S1.isVertical() ) || ( S2.isVertical() ) )
			{
				Segment Aux;

				if( S1.isVertical() )
				{
					Aux = S2;
					Result.x = S1.GetLowerEnd().x;
				}
				else
				{
					Aux = S1;
					Result.x = S2.GetLowerEnd().x;
				}

				Result.y = Aux.GetPendent()*( Result.x - Aux.GetLowerEnd().x ) + Aux.GetLowerEnd().y;
			}
			else
			{
				BigRational M1 = S1.GetPendent();
				BigRational M2 = S2.GetPendent();
				Segment Aux;

				if( (M1 == 0) || (M2 == 0) )
				{
					if(M1 == 0)
					{
						Aux = S2;
						Result.y = S1.GetLowerEnd().y;
					}
					else
					{
						Aux = S1;
						Result.y = S2.GetLowerEnd().y;
					}
					Result.x = ( Result.y - Aux.GetLowerEnd().y )/Aux.GetPendent() + Aux.GetLowerEnd().x;
				}
				else
				{
					R_Point P1 = S1.GetEnd();
					R_Point P2 = S2.GetEnd();

					Result.x = ( M1 * P1.x - P1.y - M2 * P2.x + P2.y ) / ( M1 - M2 );
					Result.y = M1*( Result.x - P1.x ) + P1.y;
				}
			}
			
			return Result;
		}
		public static R_Point Intersection(  R_Point P1, R_Point P2, R_Point P3, R_Point P4 )
		{			
			Segment S1 = new Segment(P1,P2);
			Segment S2 = new Segment(P3,P4);
			return Segment.Intersection(S1,S2);
		}
		public static R_Point Proyect( R_Point Pt, R_Point SB, R_Point SE )
		{
			R_Point Result = new R_Point();

			if( SE.x == SB.x )
			{
				Result.x = SE.x;
				Result.y = Pt.y;
				return Result;
			}

			if( SE.y == SB.y )
			{
				Result.y = SE.y;
				Result.x = Pt.x;
				return Result;
			}

			Pt.x -= SB.x;
			Pt.y -= SB.y;
			SE.x -= SB.x;
			SE.y -= SB.y;

			BigRational Temp = ( ( ( SE.x * Pt.x ) + ( SE.y * Pt.y ) ) / 
							     ( ( SE.x * SE.x ) + ( SE.y * SE.y ) ) );
			
			
			Result.x = SE.x * Temp + SB.x;
			Result.y = SE.y * Temp + SB.y;

			Pt.x += SB.x;
			Pt.y += SB.y;
			SE.x += SB.x;
			SE.y += SB.y;

			return Result;
		}
		public static R_Point Proyect( R_Point Pt, Segment S )
		{
			R_Point SB = S.GetInit();
			R_Point SE = S.GetEnd();
			return Segment.Proyect(Pt, SB, SE );
		}
	}
	/// <summary>
	/// 
	/// </summary>
	public class Event
	{
		#region Fields
		protected ArrayList S_Open;
		protected Segment S_Close;
		protected R_Point Point;

		#endregion
		#region Constructors
		public Event()
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		public Event( R_Point pt, Segment s )
		{
			Point = pt;		
			S_Close = s;
			S_Open = null;
		}

		#endregion
		#region edition
		public R_Point GetPoint()
		{
			return Point;
		}
		
		public ArrayList Get_S_Open()
		{
			return S_Open;
		}
		public Segment Get_S_Close()
		{
			return S_Close;
		}
		public void SetPoint(R_Point pt)
		{
			Point = pt;
		}
		
		public void Add_S_Open(Segment S)
		{
			if( S_Open == null ) S_Open = new ArrayList();

			S_Open.Add(S);
		}
		public void Add_S_Open(ArrayList S)
		{
			if( S==null) return;

			if( S_Open == null ) 
				S_Open = S;
			else
			{
				int count = S.Count;
				for(int i =0; i<count;i++) S_Open.Add((Segment)S[i]);
			}
		}
		public void Set_S_Close(Segment S)
		{
			if( S_Close == null )
			{
				S_Close = S;
			}			
		}
		#endregion
	}
		
	#endregion
	
	#region Growth's Structure
	/// <summary>
	/// 
	/// </summary>
	public class Interval
	{
		#region Fields
		public int ID;
		private int flag;
		private Region reg;
		private Interval I_Left;
		private Interval I_Right;
		private Interval[] Parents;
		private ArrayList Left;
		private ArrayList Right;
		#endregion
		#region edition
		public int CantOfParents()
		{
			if( Parents == null) 
				return 0;
			else 
				return Parents.Length;
		}
		public int GetFlag()
		{
			return flag;
		}
		public void SetFlag(int f)
		{
			flag = f;
		}
		public Interval[] GetParents()
		{
			return Parents;
		}
		public Interval GetLeftChild()
		{
			return I_Left;
		}
		public Interval GetRightChild()
		{
			return I_Right;
		}
		public ArrayList GetLeft()
		{
			return Left;
		}
		public ArrayList GetRight()
		{
			return Right;
		}
		public Region GetRegion()
		{ 
			return reg;
		}
		public void SetChild( Interval L, Interval R )
		{			
			I_Left = L;
			I_Right = R;
		}
		public void SetParents( Interval[] P )
		{
			Parents = P;
		}
		public void SetLeft( ArrayList L )
		{
			Left = L;
		}
		public void SetRight( ArrayList R )
		{
			Right = R;
		}
		public void SetRegion( Region r )
		{
			reg = r;
		}
		public void AddRight( R_Point pt )
		{
			Right.Add(pt);
		}
		public void AddLeft( R_Point pt )
		{
			Left.Add(pt);
		}
		#endregion
		
		public Interval(Region R,int f,int id)
		{
			I_Left = null;
			I_Right = null;
			Parents = null;
			Left = new ArrayList();
			Right = new ArrayList();
			reg = R;
			flag = f;
			ID = id;
		}
		public Interval(Interval[] P,Region R,int f,int id)
		{
			I_Left = null;
			I_Right = null;
			Parents = P;
			Left = new ArrayList();
			Right = new ArrayList();
			reg = R;
			flag = f;
			ID = id;
		}
	}
	/// <summary>
	/// 
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
	
	
	public enum TYPE {OPEN, CLOSE, INTERSECTION, MIDDLE };
	#endregion	
}
	
