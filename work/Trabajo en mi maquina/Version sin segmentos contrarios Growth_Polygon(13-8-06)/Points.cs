using System;
using BigNumbers;

namespace Growth_Polygon
{
	/// <summary>
	/// 
	/// </summary>
	public class R_Point
	{
		#region fields
		public bool flag;
		public BigRational x;
		public BigRational y;
		#endregion

		#region Constructors
		public R_Point()
		{
			// 
			// TODO: Add constructor logic here
			//
			x = new BigRational(0,1) ;
			y = new BigRational(0,1);
			flag = false;
		}

		public R_Point(Rational64 X, Rational64 Y)
		{
			// 
			// TODO: Add constructor logic here
			//
			x = new BigRational(X.entero,X.numerador,X.denominador) ;
			y = new BigRational(Y.entero,Y.numerador,Y.denominador);
			flag = false;
		}
		public R_Point(BigRational X, BigRational Y)
		{
			// 
			// TODO: Add constructor logic here
			//
			x = X ;
			y = Y;
			flag = false;
		}
		public R_Point( L_Point pt )
		{
			x = new BigRational(pt.x,0,1);
            y = new BigRational(pt.y,0,1);
			flag = false;
		}
		#endregion

		#region Utils
		public R_Point Clone()
		{
			return new R_Point( x,y );
		}
		public static int SD(R_Point a, R_Point b, R_Point c)
		{
			BigRational A = (b.x-a.x)*(c.y-a.y);
			BigRational B = (c.x-a.x)*(b.y-a.y);
			if ( A > B  ) return  1;
			if ( A < B  ) return -1;
			return 0;
		}
		
		public static BigRational GetQuadraticDistance(R_Point P1, R_Point P2)
		{
			return (P1.x - P2.x) * (P1.x - P2.x) + (P1.y - P2.y) * (P1.y - P2.y);
		}
		#endregion
	}
	public class L_Point
	{
		public long x;
		public long y;
		public int flag;

		public L_Point( long X,long Y )
		{
			x = X;
			y = Y;
		}
		public L_Point( R_Point pt )
		{
			x = BigInteger.BigIntegerToLong(pt.x.entero);
			y = BigInteger.BigIntegerToLong(pt.y.entero);
		}
		public L_Point( L_Point pt )
		{
			x = pt.x;
			y = pt.y;
		}
		public L_Point( )
		{
		}

		

		#region Utils
		public L_Point Clone()
		{
			L_Point Result = new L_Point();
			Result.x = x;
			Result.y = y;
			return Result;
		}
		public static int SD(L_Point a, L_Point b, L_Point c)
		{
			BigInteger Aux1 = BigInteger.LongToBigInteger( b.x - a.x );
			BigInteger Aux2 = BigInteger.LongToBigInteger( c.y - a.y );

			BigInteger A = Aux1*Aux2;
			
			Aux1 = BigInteger.LongToBigInteger( c.x - a.x );
			Aux2 = BigInteger.LongToBigInteger( b.y - a.y );

			BigInteger B = Aux1*Aux2;

			if ( A > B  ) return  1;
			if ( A < B  ) return -1;
			return 0;
		}
		
		#endregion
	}
}
