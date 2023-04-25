using System;
using Structures;

using System.Collections;

namespace BorradorTesis.Plane_Sweep.GrowthStructures
{
	/// <summary>
	/// Descripción breve de Interval.
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
		public void AddRight( RPoint pt )
		{
			Right.Add(pt);
		}
		public void AddLeft( RPoint pt )
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
}
