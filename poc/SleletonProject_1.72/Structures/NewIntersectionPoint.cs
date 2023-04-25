using System;

namespace Structures
{
	/// <summary>
	/// Descripción breve de NewIntersectionPoint.
	/// </summary>
	public class NewIntersectionPoint : IComparable 
	{
		private BigNumbers.BigRational distance;
		private RPoint point;
		private SkNode vLeft; //Left
		private SkNode vRight; //rigth
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

		public NewIntersectionPoint(BigNumbers.BigRational distance, RPoint point,
			SkNode vleft, SkNode vrigth, IntersecPointType type)
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

		public SkNode VRight
		{
			get { return vRight; }
			set { vRight = value; }
		}

		public SkNode VLeft
		{
			get { return vLeft; }
			set { vLeft = value; }
		}
		#region Miembros de IComparable

		public int CompareTo(object obj)
		{
			NewIntersectionPoint nit = obj as NewIntersectionPoint;
            if(this.distance < nit.distance)
				return 1;
			if(this.distance > nit.distance)
				return -1;
			return 0;
		}

		#endregion
	}
}
