using System;

namespace BorradorTesis.Plane_Sweep.Nodes
{
	/// <summary>
	/// Descripción breve de RBNode.
	/// </summary>
	public class RBNode
	{
		#region Attributes
		
		public RBNode right, left, parent;
		public Color color;
		public IComparable value;

		#endregion
		
        public RBNode()
		{
			this.color = Color.black;
		}
		public RBNode(RBNode lchild, RBNode rchild, 
			RBNode parent, IComparable item, Color color)
		{
			this.left = lchild;
			this.right = rchild;
			this.parent = parent;
			this.color = color;
			this.value = item;
		}
	}
}
