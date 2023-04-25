using System;
using BigNumbers;

namespace Crecimiento
{
	/// <summary>
	/// Punto con coordenadas recionales
	/// </summary>
	public class RPoint
	{
		#region Constructores
		// CONSTRUCTORES

		public RPoint( BigRational X,
			BigRational Y )
		{
			x = X;
			y = Y;
		}
		public RPoint()
		{
			x = new BigRational(0,0,1);
			y = new BigRational(0,0,1);
		}
		#endregion
		
		#region Metodos
		// METODOS

		public RPoint Clone()
		{
			return new RPoint(x,y);
		}
		#endregion

		#region Atributos
		// ATRIBUTOS

		/// <summary>
		/// coordenadas del punto
		/// </summary>
		public BigRational x,y;
		#endregion
	}
}
