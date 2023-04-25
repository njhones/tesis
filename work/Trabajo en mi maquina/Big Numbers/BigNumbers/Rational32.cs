using System;

namespace BigNumbers
{
	/// <summary>
	/// Clase que representa los números racionales usando int
	/// para la parte entera, para el numerador y para el denominador.
	/// El signo está dado por el signo de la parte entera.
	/// La fraccion numerador/denominador es positiva, propia e irreducible.
	/// </summary>
	public sealed class Rational32
	{
		public static int creados=0;
		public int entero;
		public int numerador, denominador;

		/// <summary>
		/// Crea un Rational32 igual a <code>entero+(numerador/denominador)</code>
		/// </summary>
		/// <param name="entero">entero</param>
		/// <param name="numerador">numerador</param>
		/// <param name="denominador">denominador</param>
		public Rational32(int entero, int numerador, int denominador)
		{
			if (denominador==0) throw new DivideByZeroException("Rational32 error: Denominador=0." );
			
			this.entero = numerador/denominador;
			this.numerador = numerador%denominador;
			this.denominador = denominador;

			int mcd = BigInteger.MCD(this.numerador, this.denominador);
			if (mcd!=1)
			{
				this.numerador/=mcd;
				this.denominador/=mcd;
			}
			
			//si la fraccion es negativa sumarle uno y restarselo a entero
			if ((numerador<0 && denominador>0) || (numerador>0 && denominador<0))
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
		/// Crea un Rational32 igual a <code>numerador/denominador</code>.
		/// </summary>
		/// <param name="numerador">numerador</param>
		/// <param name="denominador">denominador</param>
		public Rational32(int numerador, int denominador):this(0, numerador, denominador)
		{
		}
			
		/// <summary>
		/// Crea un Rational32 a partir de otro. Constructor copia.
		/// </summary>
		/// <param name="n">Rational32 a copiar.</param>
		public Rational32(Rational32 n)
		{
			this.entero = n.entero;
			this.numerador = n.numerador;
			this.denominador = n.denominador;

			creados++;
		}
		
		
		/// <summary>
		/// Multiplica dos Rational32
		/// </summary>
		/// <param name="n">factor</param>
		/// <param name="m">factor</param>
		/// <returns>n*m</returns>
		public static Rational32 operator * (Rational32 n, Rational32 m)
		{			
			int
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
			return new Rational32(
				A*B+C+D+T,
				t1,
				z2
				);
				*/
			return new Rational32(
				(A*a2+a1)*(B*b2+b1),
				a2*b2);
		}

		/// <summary>
		/// divide dos Rational32
		/// </summary>
		/// <param name="n">dividendo</param>
		/// <param name="m">divisor</param>
		/// <returns>dividendo/divisor</returns>
		public static Rational32 operator / (Rational32 n, Rational32 m)
		{
			return n*m.reciproco();
		}
		
		/// <summary>
		/// suma de dos Rational32. (n+m)
		/// </summary>
		/// <param name="n">sumando</param>
		/// <param name="m">sumando</param>
		/// <returns>suma n+m</returns>
		public static Rational32 operator + (Rational32 n, Rational32 m)
		{
			int d1 = BigInteger.MCD(n.denominador,m.denominador);
			if (d1 == 1)
				return new Rational32(
					n.entero+m.entero,
					(n.numerador*m.denominador)+(n.denominador*m.numerador),
					n.denominador*m.denominador
					);
			int 
				t = (n.numerador*(m.denominador/d1))+((n.denominador/d1)*m.numerador),
				d2 = BigInteger.MCD(t,d1);
			return new Rational32(
				n.entero+m.entero,
				t/d2,
				(n.denominador/d1)*(m.denominador/d2)
				);
		}
		
		/// <summary>
		/// Diferencia entre dos <code>Rational32</code> (n-m)
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns></returns>
		public static Rational32 operator - (Rational32 n, Rational32 m)
		{
			int d1 = BigInteger.MCD(n.denominador,m.denominador);
			if (d1 == 1)
				return new Rational32(
					n.entero-m.entero,
					(n.numerador*m.denominador)-(n.denominador*m.numerador),
					n.denominador*m.denominador
					);
			int 
				t = (n.numerador*(m.denominador/d1))-((n.denominador/d1)*m.numerador),
				d2 = BigInteger.MCD(t,d1);
			return new Rational32(
				n.entero-m.entero,
				t/d2,
				(n.denominador/d1)*(m.denominador/d2)
				);
		}

		/// <summary>
		/// opuesto de un número
		/// </summary>
		/// <param name="n">número</param>
		/// <returns>el opuesto de <code>n</code></returns>
		public static Rational32 operator - (Rational32 n)
		{
			Rational32 result = new Rational32(n);
			result.sign=!n.sign;
			return result;
		}
		
		
		/// <summary>
		/// Compara dos Rational32.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es igual a m.
		/// false en otro caso.</returns>
		public static bool operator ==(Rational32 n, Rational32 m)
		{
			return (
				n.entero == m.entero && 
				n.numerador == m.numerador &&
				n.denominador == m.denominador);
		}
		
		/// <summary>
		/// Compara dos Rational32.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es diferente de m.
		/// false en otro caso.</returns>
		public static bool operator !=(Rational32 n, Rational32 m)
		{
			return !(n==m);
		}
		/// <summary>
		/// Compara dos Rational32.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor que m.
		/// false en otro caso.</returns>
		public static bool operator <(Rational32 n, Rational32 m)
		{
			if (n.entero<m.entero) return true;
			if (n.entero>m.entero) return false;
			return (n.numerador*m.denominador<n.denominador*m.numerador);
		}
		/// <summary>
		/// Compara dos Rational32.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor que m.
		/// false en otro caso.</returns>
		public static bool operator >(Rational32 n, Rational32 m)
		{
			if (n.entero<m.entero) return false;
			if (n.entero>m.entero) return true;
			return (n.numerador*m.denominador>n.denominador*m.numerador);
		}
		
		/// <summary>
		/// Compara dos Rational32.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator <=(Rational32 n, Rational32 m)
		{
			return !(n>m);
		}
		/// <summary>
		/// Compara dos Rational32.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator >=(Rational32 n, Rational32 m)
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
			if (obj is Rational32) 
				return (this==(obj as Rational32));
			return false;
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		/// <summary>
		/// Halla el reciproco de un Rational32.
		/// </summary>
		/// <returns>El reciproco de un Rational32.</returns>
		public Rational32 reciproco()
		{
			return new Rational32(denominador, entero*denominador+numerador);
		}
	}
}
