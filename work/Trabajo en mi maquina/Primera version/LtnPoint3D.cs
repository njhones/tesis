using System;

namespace Crecimiento
{
	/// <summary>
	/// 
	/// </summary>
	public class LtnPoint3D
	{
		#region Constructores
		// CONSTRUCTORES

		public LtnPoint3D( System.Int64 X,
			System.Int64 Y, 
			System.Int64 Z )
		{
			x = X;
			y = Y;
			z = Z;
		}
		public LtnPoint3D()
		{
			x = y = z = 0;
		}
		#endregion
		
		#region Metodos
		// METODOS

		public LtnPoint3D Clone()
		{
			return new LtnPoint3D(x,y,z);
		}
		#endregion

		#region Atributos
		// ATRIBUTOS

		/// <summary>
		/// coordenadas del punto
		/// </summary>
		public System.Int64 x,y,z;
		#endregion
	}
}
