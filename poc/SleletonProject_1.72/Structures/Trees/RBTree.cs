using System;
using BorradorTesis.Plane_Sweep.Nodes;

namespace Structures
{
	/// <summary>
	/// Descripción breve de RBTree.
	/// </summary>
	public class RBTree
	{
		#region Attributes

		protected RBNode root, nil;		
		
		#endregion
		
		public RBTree()
		{
			this.root = this.nil = new RBNode(null, null, null, null, Color.black);
		}
		/**/
		public RBNode Insert(IComparable c)
		{			
			RBNode x = new RBNode(this.nil, this.nil, this.nil, c, Color.red);
			this.TreeInsert(x);
			this.InsertFixUp(x);
			return x;
		}

		/**/
		private void LeftRotate(RBNode x)
		{
			RBNode y = x.right;
			x.right = y.left;

			if(y.left != nil)
			{
				y.left.parent = x;
			}

			y.parent = x.parent;

			if(x.parent == nil)
			{
				this.root = y;
			}
			else if(x == x.parent.left)
			{
				x.parent.left = y;
			}
			else
			{
				x.parent.right = y;
			}

			y.left = x;
			x.parent = y;
		}
		/**/
		private void RightRotate(RBNode x)
		{
			RBNode y = x.left;
			x.left = y.right;

			if(y.right != nil)
			{
				y.right.parent = x;
			}

			y.parent = x.parent;

			if(x.parent == nil)
			{
				this.root = y;
			}
			else if(x == x.parent.right)
			{
				x.parent.right = y;
			}
			else
			{
				x.parent.left = y;
			}

			y.right = x;
			x.parent = y;
		}
		/**/
		private void TreeInsert(RBNode node)
		{			
			RBNode _y = nil, _x = this.root;

			while(_x != nil)
			{
				_y = _x;

				if(node.value.CompareTo(_x.value) < 0)	
				{
					_x = _x.left;				
				}
				else
				{
					_x = _x.right;				
				}
			}
			node.parent = _y;
			if(_y == nil)
			{
				this.root = node;
			}
			else if(node.value.CompareTo(_y.value) < 0)
			{
				_y.left = node;
			}
			else
			{
				_y.right = node;
			}

		}
		
		/**/
		public RBNode Delete(RBNode z)
		{
			RBNode x, y;
			Color _color = Color.red;
			if(z.left == this.nil || z.right == this.nil)
			{
				y = z;
			}
			else
			{
				y = this.TreeSussesor(z);
			}
			if(y.left != this.nil)
			{
				x = y.left;
			}
			else
			{
				x = y.right;
			}
			x.parent = y.parent;

			if(y.parent == this.nil)
			{
				this.root = x;
			}
			else 
			{
				if(y == y.parent.left)
				{
					y.parent.left = x;
				}
				else
				{
					y.parent.right = x;
				}
			}
			
			if(y != z)
			{
				//z.value = y.value;
				y.parent = z.parent;
				y.left = z.left;
				y.right = z.right;
				y.left.parent = y;
				y.right.parent = y;
				_color = y.color;
				y.color = z.color;
				if(z == this.root)
				{
					this.root = y;
				}
				else
				{
					if(z == z.parent.left)
					{
						z.parent.left = y;
					}
					else
					{
						z.parent.right = y;
					}
				}
			}
			else
			{
				_color = y.color;
			}
			if(_color == Color.black)
			{
				this.DeleteFixUp(x);
			}
			this.nil.parent = null;
			this.nil.right = null;
			this.nil.left = null;
			return y;
		}
		/**/
		public RBNode TreeSussesor(RBNode x)
		{
			if(x == null)
			{
				return this.TreeMinimun(this.root);
			}
			if(x.right != nil)
			{
				return this.TreeMinimun(x.right);
			}
			RBNode y = x.parent;
			while(y != nil && x == y.right)
			{
				x = y;
				y = y.parent;
			}
			return y == nil ? null : y;
		}
		/**/
		public RBNode TreeMinimun(RBNode x)
		{
			while(x.left != nil)
			{
				x = x.left;
			}
			return x == nil ? null : x;
		}
		/**/
		private void DeleteFixUp(RBNode x)
		{
			RBNode w;
			while(x != this.root && x.color == Color.black)
			{
				if(x == x.parent.left)
				{
					w = x.parent.right;
					if(w.color == Color.red)
					{
						w.color = Color.black;
						x.parent.color = Color.red;
						this.LeftRotate(x.parent);
						w = x.parent.right;
					}
					if(w.left.color == Color.black && Color.black == w.right.color )
					{
						w.color = Color.red;
						x = x.parent;
					}
					else 
					{
						if(w.right.color == Color.black)
						{
							w.left.color = Color.black;
							w.color = Color.red;
							this.RightRotate(w);
							w = x.parent.right;
						}
						w.color = x.parent.color;
						x.parent.color = Color.black;
						w.right.color = Color.black;
						this.LeftRotate(x.parent);
						x = this.root;
					}
				}
				else
				{
					w = x.parent.left;
					if(w.color == Color.red)
					{
						w.color = Color.black;
						x.parent.color = Color.red;
						this.RightRotate(x.parent);
						w = x.parent.left;
					}
					if(w.right.color == Color.black && Color.black == w.left.color)
					{
						w.color = Color.red;
						x = x.parent;
					}
					else 
					{
						if(w.left.color == Color.black)
						{
							w.right.color = Color.black;
							w.color = Color.red;
							this.LeftRotate(w);
							w = x.parent.left;
						}
						w.color = x.parent.color;
						x.parent.color = Color.black;
						w.left.color = Color.black;
						this.RightRotate(x.parent);
						x = this.root;
					}
				}
			}
			x.color = Color.black;
		}
		/**/
		public IComparable Search(IComparable c)
		{
			return this.Search(this.root, c).value;
		}
		/**/
		private RBNode Search(RBNode x, IComparable c)
		{
			if(x == this.nil || c.CompareTo(x.value) == 0)
			{
				return x;
			}
			if(c.CompareTo(x.value) < 0)
			{
				return this.Search(x.left, c);
			}
			return this.Search(x.right, c);
		}
		/**/
		public RBNode SearchNode(IComparable c)
		{
			RBNode r = this.Search(this.root, c);
			return r == nil ? null : r;
		}
		/**/
		public void Delete(IComparable c)
		{
			RBNode r = this.Search(this.root, c);
			if(r == this.nil)
				throw new Exception("no esta el elemento");
			this.Delete(r);
		}
		/**/
		public RBNode Root
		{
			get
			{
				return this.root;
			}
		}
		/**/
		public bool IsEmpty
		{
			get
			{
				return root == nil;
			}
		}
		/**/
		public RBNode TreeMaximun(RBNode x)
		{
			while(x.right != this.nil)
			{
				x = x.right;
			}
			return x;
		}
		/**/
		public RBNode TreePredecessor(RBNode x)
		{
			if(x == null)
			{
				return null;
			}
			if(x.left != this.nil)
			{
				return this.TreeMaximun(x.left);
			}
			RBNode y = x.parent;
			while(y != nil && x == y.left)
			{
				x = y;
				y = y.parent;
			}
			return y == nil ? null : y;
		}
		/**/
		private void InsertFixUp(RBNode x)
		{
			RBNode y;

			while(x != this.root && x.parent.color == Color.red)
			{
				if(x.parent == x.parent.parent.left)
				{
					y = x.parent.parent.right;
					if(y.color == Color.red)
					{
						x.parent.color = Color.black;
						y.color = Color.black;
						x.parent.parent.color = Color.red;
						x = x.parent.parent;
					}
					else 
					{
						if(x == x.parent.right)
						{
							x = x.parent;
							this.LeftRotate(x);
						}
						x.parent.color = Color.black;
						x.parent.parent.color = Color.red;
						this.RightRotate(x.parent.parent);
					}
				}
				else
				{
					y = x.parent.parent.left;
					if(y.color == Color.red)
					{
						x.parent.color = Color.black;
						y.color = Color.black;
						x.parent.parent.color = Color.red;
						x = x.parent.parent;
					}
					else 
					{
						if(x == x.parent.left)
						{
							x = x.parent;
							this.RightRotate(x);
						}
						x.parent.color = Color.black;
						x.parent.parent.color = Color.red;
						this.LeftRotate(x.parent.parent);
					}
				}
			}
			this.root.color = Color.black;
		}
	}
}
