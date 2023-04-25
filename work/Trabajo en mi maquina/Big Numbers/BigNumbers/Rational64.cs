using System;

namespace BigNumbers
{
	/// <summary>
	/// Clase que representa los números racionales usando long
	/// para la parte entera, para el numerador y para el denominador.
	/// El signo está dado por el signo de la parte entera.
	/// La fraccion numerador/denominador es positiva, propia e irreducible.
	/// </summary>
	public sealed class Rational64
	{
		public static long creados=0;
		public long entero;
		public long numerador, denominador;

		/// <summary>
		/// Crea un Rational64 igual a <code>entero+(numerador/denominador)</code>
		/// </summary>
		/// <param name="entero">entero</param>
		/// <param name="numerador">numerador</param>
		/// <param name="denominador">denominador</param>
		public Rational64(long entero, long numerador, long denominador)
		{
			if (denominador==0) throw new DivideByZeroException("Rational64 error: Denominador=0." );
			
			if (numerador == 0)	denominador =1;
			
			this.entero = numerador/denominador;
			this.numerador = numerador%denominador;
			this.denominador = denominador;

			long mcd = BigInteger.MCD(this.numerador, this.denominador);
			if (mcd!=1)
			{
				this.numerador/=mcd;
				this.denominador/=mcd;
			}
			
			//si la fraccion es negativa sumarle uno y restarselo a entero
			if (Math.Sign(this.numerador)*Math.Sign(this.denominador)<0)
			{
				this.entero-=1;
				this.numerador=this.denominador+this.numerador;
				//como ya se tenia una fraccion propia, 1-fraccion tambien es propia.
			}

			this.entero+=entero;
			this.numerador=(this.numerador<0)?-this.numerador:this.numerador;
			this.denominador=(this.denominador<0)?-this.denominador:this.denominador;

			creados++;
		}

		/// <summary>
		/// Crea un Rational64 igual a <code>numerador/denominador</code>.
		/// </summary>
		/// <param name="numerador">numerador</param>
		/// <param name="denominador">denominador</param>
		public Rational64(long numerador, long denominador):this(0, numerador, denominador)
		{
		}
			
		/// <summary>
		/// Crea un Rational64 a partir de otro. Constructor copia.
		/// </summary>
		/// <param name="n">Rational64 a copiar.</param>
		public Rational64(Rational64 n)
		{
			this.entero = n.entero;
			this.numerador = n.numerador;
			this.denominador = n.denominador;

			creados++;
		}
		
		
		/// <summary>
		/// Multiplica dos Rational64
		/// </summary>
		/// <param name="n">factor</param>
		/// <param name="m">factor</param>
		/// <returns>n*m</returns>
		public static Rational64 operator * (Rational64 n, Rational64 m)
		{			
			long
				A = n.entero,
				a1 = n.numerador,
				a2 = n.denominador,
				B = m.entero,
				b1 = m.numerador,
				b2 = m.denominador;
			/*,
				C = (A*b1)/b2,
				c1 = (A*b1)%b2,
				D = (B*a1)/a2,
				d1 = (B*a1)%a2,
				z1 = a2*c1+b2*d1+a1*b1,
				z2 = a2*b2,
				T = z1/z2,
				t1 = z1%z2;
			return new Rational64(
				A*B+C+D+T,
				t1,
				z2
				);
				*/
			return new Rational64(A*B,A*b1,b2)+new Rational64(B*a1,a2)
				  +new Rational64(a1*b1,a2*b2);
		}

		/// <summary>
		/// Multiplica un Int64 y un Rational64
		/// </summary>
		/// <param name="n">factor</param>
		/// <param name="m">factor</param>
		/// <returns>n*m</returns>
		public static Rational64 operator * (Int64 n, Rational64 m)
		{			
			long
				A = n,
				B = m.entero,
				b1 = m.numerador,
				b2 = m.denominador;
			/*,
				C = (A*b1)/b2,
				c1 = (A*b1)%b2,
				D = (B*a1)/a2,
				d1 = (B*a1)%a2,
				z1 = a2*c1+b2*d1+a1*b1,
				z2 = a2*b2,
				T = z1/z2,
				t1 = z1%z2;
			return new Rational64(
				A*B+C+D+T,
				t1,
				z2
				);
				*/
			return new Rational64(A*B,
				A*b1,
				b2);
		}

		/// <summary>
		/// Multiplica un Int64 y un Rational64
		/// </summary>
		/// <param name="n">factor</param>
		/// <param name="m">factor</param>
		/// <returns>n*m</returns>
		public static Rational64 operator * ( Rational64 n,Int64 m)
		{			
			long
				A = m,
				B = n.entero,
				b1 = n.numerador,
				b2 = n.denominador;
			/*,
				C = (A*b1)/b2,
				c1 = (A*b1)%b2,
				D = (B*a1)/a2,
				d1 = (B*a1)%a2,
				z1 = a2*c1+b2*d1+a1*b1,
				z2 = a2*b2,
				T = z1/z2,
				t1 = z1%z2;
			return new Rational64(
				A*B+C+D+T,
				t1,
				z2
				);
				*/
			return new Rational64(A*B,
				A*b1,
				b2);
		}

		/// <summary>
		/// divide dos Rational64
		/// </summary>
		/// <param name="n">dividendo</param>
		/// <param name="m">divisor</param>
		/// <returns>dividendo/divisor</returns>
		public static Rational64 operator / (Rational64 n, Rational64 m)
		{
			return n*m.reciproco();
		}

		/// <summary>
		/// divide un Rational64 y un Int 64
		/// </summary>
		/// <param name="n">dividendo</param>
		/// <param name="m">divisor</param>
		/// <returns>dividendo/divisor</returns>
		public static Rational64 operator / (Rational64 n, Int64 m)
		{
			return new Rational64(n.entero*n.denominador+n.numerador,m*n.denominador);
		}
		/// <summary>
		/// divide un Int64 y un Rational64
		/// </summary>
		/// <param name="n">dividendo</param>
		/// <param name="m">divisor</param>
		/// <returns>dividendo/divisor</returns>
		public static Rational64 operator / (Int64 n,Rational64 m)
		{
			return n*m.reciproco();
		}
		
		/// <summary>
		/// suma de dos Rational64. (n+m)
		/// </summary>
		/// <param name="n">sumando</param>
		/// <param name="m">sumando</param>
		/// <returns>suma n+m</returns>
		public static Rational64 operator + (Rational64 n, Rational64 m)
		{
			long d1 = BigInteger.MCD(n.denominador,m.denominador);
			if (d1 == 1)
				return new Rational64(
					n.entero+m.entero,
					(n.numerador*m.denominador)+(n.denominador*m.numerador),
					n.denominador*m.denominador
					);
			long 
				t = (n.numerador*(m.denominador/d1))+((n.denominador/d1)*m.numerador),
				d2 = BigInteger.MCD(t,d1);
			return new Rational64(
				n.entero+m.entero,
				t/d2,
				(n.denominador/d1)*(m.denominador/d2)
				);
		}

		/// <summary>
		/// suma de un Rational64 y un Entero. (n+m)
		/// </summary>
		/// <param name="n">sumando</param>
		/// <param name="m">sumando</param>
		/// <returns>suma n+m</returns>
		public static Rational64 operator + (Rational64 n, Int64 m)
		{
			return new Rational64( n.entero+m, n.numerador, n.denominador );			
		}
		/// <summary>
		/// suma de un Entero y un Rational64. (n+m)
		/// </summary>
		/// <param name="n">sumando</param>
		/// <param name="m">sumando</param>
		/// <returns>suma n+m</returns>
		public static Rational64 operator + ( Int64 n, Rational64 m)
		{
			return new Rational64( m.entero+n, m.numerador, m.denominador );			
		}
		
		/// <summary>
		/// Diferencia entre dos <code>Rational64</code> (n-m)
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns></returns>
		public static Rational64 operator - (Rational64 n, Rational64 m)
		{
			long d1 = BigInteger.MCD(n.denominador,m.denominador);
			if (d1 == 1)
				return new Rational64(
					n.entero-m.entero,
					(n.numerador*m.denominador)-(n.denominador*m.numerador),
					n.denominador*m.denominador
					);
			long 
				t = (n.numerador*(m.denominador/d1))-((n.denominador/d1)*m.numerador),
				d2 = BigInteger.MCD(t,d1);
			return new Rational64(
				n.entero-m.entero,
				t/d2,
				(n.denominador/d1)*(m.denominador/d2)
				);
		}

		/// <summary>
		/// Diferencia entre un entero y un <code>Rational64</code> (n-m)
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns></returns>
		public static Rational64 operator - (System.Int64 n, Rational64 m)
		{
			return new Rational64( n-m.entero-1, m.denominador - m.numerador, m.denominador );			
		}

		/// <summary>
		/// Diferencia entre un <code>Rational64</code> y un entero (n-m)
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns></returns>
		public static Rational64 operator - ( Rational64 n, System.Int64 m)
		{
			return new Rational64( n.entero-m, n.numerador, n.denominador );			
		}

		/// <summary>
		/// opuesto de un número
		/// </summary>
		/// <param name="n">número</param>
		/// <returns>el opuesto de <code>n</code></returns>
		public static Rational64 operator - (Rational64 n)
		{
			Rational64 result = new Rational64(n);
			result.sign=!n.sign;
			return result;
		}
		
		
		/// <summary>
		/// Compara dos Rational64.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es igual a m.
		/// false en otro caso.</returns>
		public static bool operator ==(Rational64 n, Rational64 m)
		{
			return (
				n.entero == m.entero && 
				n.numerador == m.numerador &&
				n.denominador == m.denominador);
		}
		
		/// <summary>
		/// Compara dos Rational64.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es diferente de m.
		/// false en otro caso.</returns>
		public static bool operator !=(Rational64 n, Rational64 m)
		{
			return !(n==m);
		}
		/// <summary>
		/// Compara dos Rational64.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor que m.
		/// false en otro caso.</returns>
		public static bool operator <(Rational64 n, Rational64 m)
		{
			if (n.entero<m.entero) return true;
			if (n.entero>m.entero) return false;
			return (n.numerador*m.denominador<n.denominador*m.numerador);
		}
		/// <summary>
		/// Compara un Rational64 con un entero.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor que m.
		/// false en otro caso.</returns>
		public static bool operator <(Rational64 n, System.Int64 m)
		{
			if (n.entero<m) return true;
			if (n.entero>m) return false;
			return !(n.numerador>0);
		}
		/// <summary>
		/// Compara dos Rational64.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor que m.
		/// false en otro caso.</returns>
		public static bool operator >(Rational64 n, Rational64 m)
		{
			if (n.entero<m.entero) return false;
			if (n.entero>m.entero) return true;
			return (n.numerador*m.denominador>n.denominador*m.numerador);
		}
		/// <summary>
		/// Compara un Rational64 con un entero.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor que m.
		/// false en otro caso.</returns>
		public static bool operator >(Rational64 n, System.Int64 m)
		{
			if (n.entero<m) return false;
			if (n.entero>m) return true;
			return (n.numerador>0);
		}
		
		/// <summary>
		/// Compara dos Rational64.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator <=(Rational64 n, Rational64 m)
		{
			return !(n>m);
		}
		/// <summary>
		/// Compara un Rational64 con un entero.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator <=(Rational64 n, System.Int64 m)
		{
			return !(n>m);
		}
		/// <summary>
		/// Compara dos Rational64.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator >=(Rational64 n, Rational64 m)
		{
			return !(n<m);
		}
		/// <summary>
		/// Compara un Rational64 con un entero.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator >=(Rational64 n, System.Int64 m)
		{
			return !(n<m);
		}
		
		
		/// <summary>
		/// sign=true implica no negatividad
		/// </summary>
		public bool sign
		{
			get
			{
				return entero>=0;
			}
			set
			{
				if (value==sign) return;
				entero=(-entero)-1;
				this.numerador=denominador-numerador;
			}
		}
		
		public override bool Equals(object obj)
		{
			if (obj is Rational64) 
				return (this==(obj as Rational64));
			return false;
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		/// <summary>
		/// Halla el reciproco de un Rational64.
		/// </summary>
		/// <returns>El reciproco de un Rational64.</returns>
		public Rational64 reciproco()
		{
			return new Rational64(denominador, entero*denominador+numerador);
		}
	}
}
