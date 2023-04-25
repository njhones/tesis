using System;

namespace BigNumbers
{
	/// <summary>
	/// Clase que representa los números racionales usando BigInteger
	/// para la parte entera, para el numerador y para el denominador.
	/// El signo está dado por el signo de la parte entera.
	/// La fraccion numerador/denominador es positiva, propia e irreducible.
	/// </summary>
	public sealed class BigRational
	{
		public static int creados=0;
		public BigInteger entero;
		public BigInteger numerador, denominador;

		/// <summary>
		/// Crea un BigRational igual a <code>entero+(numerador/denominador)</code>
		/// </summary>
		/// <param name="entero">entero</param>
		/// <param name="numerador">numerador</param>
		/// <param name="denominador">denominador</param>
		public BigRational(long entero, long numerador, long denominador)
		{
			if (denominador==0) throw new DivideByZeroException("Rational64 error: Denominador=0." );
			
			if (numerador == 0)	denominador =1;
			
			long ent = numerador/denominador;
			long num = numerador%denominador;
			long den = denominador;

			long mcd = BigInteger.MCD(num, den);
			if (mcd!=1)
			{
				num/=mcd;
				den/=mcd;
			}
			
			//si la fraccion es negativa sumarle uno y restarselo a entero
			if (Math.Sign(num)* Math.Sign(den)<0)
			{
				ent-=1;
				num=den+num;
				//como ya se tenia una fraccion propia, 1-fraccion tambien es propia.
			}

			ent+=entero;
			num = (num<0)?-num : num;
			den = (den<0)?-den : den;

			creados++;

            this.entero = BigInteger.LongToBigInteger(entero);
			this.numerador = BigInteger.LongToBigInteger(numerador);
			this.denominador = BigInteger.LongToBigInteger(denominador);

		}
		/// <summary>
		/// Crea un BigRational igual a <code>entero+(numerador/denominador)</code>
		/// </summary>
		/// <param name="entero">entero</param>
		/// <param name="numerador">numerador</param>
		/// <param name="denominador">denominador</param>
		public BigRational(BigInteger entero, BigInteger numerador, BigInteger denominador)
		{
			if (denominador==0) throw new DivideByZeroException("BigRational error: Denominador=0." );
			
			BigInteger[] d = BigInteger.DivisionAndRemainder(numerador, denominador);
			this.entero = d[0];
			this.numerador = d[1];
			this.denominador = new BigInteger(denominador);

			BigInteger mcd = BigInteger.MCD(this.numerador, this.denominador);
			if (mcd!=1)
			{
				this.numerador/=mcd;
				this.denominador/=mcd;
			}
			
			//si la fraccion es negativa sumarle uno y restarselo a entero
			if (this.numerador.sign!=this.denominador.sign)
			{
				this.entero-=1;
				this.numerador=this.denominador+this.numerador;
				//como ya se tenia una fraccion propia, 1-fraccion tambien es propia.
			}

			this.entero+=entero;
			this.numerador.sign=this.denominador.sign=true;

			creados++;
		}

		/// <summary>
		/// Crea un BigRational igual a <code>numerador/denominador</code>.
		/// </summary>
		/// <param name="numerador">numerador</param>
		/// <param name="denominador">denominador</param>
		public BigRational(BigInteger numerador, BigInteger denominador):this(0, numerador, denominador)
		{
		}
			
		/// <summary>
		/// Crea un BigRational a partir de otro. Constructor copia.
		/// </summary>
		/// <param name="n">BigRational a copiar.</param>
		public BigRational(BigRational n)
		{
			this.entero = new BigInteger(n.entero);
			this.numerador = new BigInteger(n.numerador);
			this.denominador = new BigInteger(n.denominador);

			creados++;
		}
		
		
		/// <summary>
		/// Multiplica dos BigRational
		/// </summary>
		/// <param name="n">factor</param>
		/// <param name="m">factor</param>
		/// <returns>n*m</returns>
		public static BigRational operator * (BigRational n, BigRational m)
		{			
			BigInteger
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
			return new BigRational(
				A*B+C+D+T,
				t1,
				z2
				);
				*/
			return new BigRational(
				(A*a2+a1)*(B*b2+b1),
				a2*b2);
		}

		/// <summary>
		/// Multiplica dos BigRational
		/// </summary>
		/// <param name="n">factor</param>
		/// <param name="m">factor</param>
		/// <returns>n*m</returns>
		public static BigRational operator * (Int64 n, BigRational m)
		{			
			BigInteger
				A = BigInteger.LongToBigInteger(n),
				a1 = BigInteger.LongToBigInteger(0),
				a2 = BigInteger.LongToBigInteger(1),
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
			return new BigRational(
				A*B+C+D+T,
				t1,
				z2
				);
				*/
			return new BigRational(
				(A*a2+a1)*(B*b2+b1),
				a2*b2);
		}

		/// <summary>
		/// Multiplica dos BigRational
		/// </summary>
		/// <param name="n">factor</param>
		/// <param name="m">factor</param>
		/// <returns>n*m</returns>
		public static BigRational operator * (BigRational n, Int64 m)
		{			
			BigInteger
				A = n.entero,
				a1 = n.numerador,
				a2 = n.denominador,
				B = BigInteger.LongToBigInteger(m),
				b1 = BigInteger.LongToBigInteger(0),
				b2 = BigInteger.LongToBigInteger(1);
			/*,
				C = (A*b1)/b2,
				c1 = (A*b1)%b2,
				D = (B*a1)/a2,
				d1 = (B*a1)%a2,
				z1 = a2*c1+b2*d1+a1*b1,
				z2 = a2*b2,
				T = z1/z2,
				t1 = z1%z2;
			return new BigRational(
				A*B+C+D+T,
				t1,
				z2
				);
				*/
			return new BigRational(
				(A*a2+a1)*(B*b2+b1),
				a2*b2);
		}

		/// <summary>
		/// divide dos BigRational
		/// </summary>
		/// <param name="n">dividendo</param>
		/// <param name="m">divisor</param>
		/// <returns>dividendo/divisor</returns>
		public static BigRational operator / (BigRational n, BigRational m)
		{
			return n*m.reciproco();
		}
		public static BigRational operator / (BigRational n, long m)
		{
			return n*(new BigRational(0,1,m));
		}

		
		/// <summary>
		/// suma de dos BigRational. (n+m)
		/// </summary>
		/// <param name="n">sumando</param>
		/// <param name="m">sumando</param>
		/// <returns>suma n+m</returns>
		public static BigRational operator + (BigRational n, BigRational m)
		{
			BigInteger d1 = BigInteger.MCD(n.denominador,m.denominador);
			if (d1 == 1)
				return new BigRational(
					n.entero+m.entero,
					(n.numerador*m.denominador)+(n.denominador*m.numerador),
					n.denominador*m.denominador
					);
			BigInteger 
				t = (n.numerador*(m.denominador/d1))+((n.denominador/d1)*m.numerador),
				d2 = BigInteger.MCD(t,d1);
			return new BigRational(
				n.entero+m.entero,
				t/d2,
				(n.denominador/d1)*(m.denominador/d2)
				);
		}

		/// <summary>
		/// suma de un BigRational <code>n</code> y un long <code>m</code>. (n+m)
		/// </summary>
		/// <param name="n">sumando</param>
		/// <param name="m">sumando</param>
		/// <returns>suma n+m</returns>
		public static BigRational operator + (BigRational n, long m)
		{
			return new BigRational(
					n.entero+m,
					n.numerador,
					n.denominador
					);
		}
		/// <summary>
		/// suma de un long <code>n</code> y un BigRational <code>m</code>. (n+m)
		/// </summary>
		/// <param name="n">sumando</param>
		/// <param name="m">sumando</param>
		/// <returns>suma n+m</returns>
		public static BigRational operator + (long n, BigRational m )
		{
			return new BigRational(
				m.entero+n,
				m.numerador,
				m.denominador
				);
		}

		
		/// <summary>
		/// Diferencia entre dos <code>BigRational</code> (n-m)
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns></returns>
		public static BigRational operator - (BigRational n, BigRational m)
		{
			BigInteger d1 = BigInteger.MCD(n.denominador,m.denominador);
			if (d1 == 1)
				return new BigRational(
					n.entero-m.entero,
					(n.numerador*m.denominador)-(n.denominador*m.numerador),
					n.denominador*m.denominador
					);
			BigInteger 
				t = (n.numerador*(m.denominador/d1))-((n.denominador/d1)*m.numerador),
                d2 = BigInteger.MCD(t,d1);
			return new BigRational(
				n.entero-m.entero,
				t/d2,
				(n.denominador/d1)*(m.denominador/d2)
				);
		}


		/// <summary>
		/// Diferencia entre un <code>BigRational</code> y un long (n-m)
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns></returns>
		public static BigRational operator - (BigRational n, long m)
		{
			return new BigRational(
					n.entero-m,
					n.numerador,
					n.denominador
					);
		}

		/// <summary>
		/// Diferencia entre un long y un <code>BigRational</code> (n-m)
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns></returns>
		public static BigRational operator - (long n, BigRational m )
		{
			return new BigRational(
				n -m.entero,
				-m.numerador,
				m.denominador
				);
		}
		/// <summary>
		/// opuesto de un número
		/// </summary>
		/// <param name="n">número</param>
		/// <returns>el opuesto de <code>n</code></returns>
		public static BigRational operator - (BigRational n)
		{
			BigRational result = new BigRational(n);
			result.sign=!n.sign;
			return result;
		}
		
		
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es igual a m.
		/// false en otro caso.</returns>
		public static bool operator ==(BigRational n, BigRational m)
		{
			return (
				n.entero == m.entero && 
				(n.numerador * m.denominador ==
				n.denominador * m.numerador));
		}

		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es igual a m.
		/// false en otro caso.</returns>
		public static bool operator ==(BigRational n, BigInteger m)
		{
			return ( n.entero == m && n.numerador == 0);
		}
		
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es igual a m.
		/// false en otro caso.</returns>
		public static bool operator ==(BigInteger n, BigRational m)
		{
			return (n == m.entero && m.numerador==0 );
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es diferente de m.
		/// false en otro caso.</returns>
		public static bool operator !=(BigRational n, BigRational m)
		{
			return !(n==m);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es diferente de m.
		/// false en otro caso.</returns>
		public static bool operator !=(BigRational n, BigInteger m)
		{
			return !(n==m);
		}
		
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es diferente de m.
		/// false en otro caso.</returns>
		public static bool operator !=(BigInteger n, BigRational m)
		{
			return !(n==m);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor que m.
		/// false en otro caso.</returns>
		public static bool operator <(BigRational n, BigRational m)
		{
			if (n.entero<m.entero) return true;
			if (n.entero>m.entero) return false;
			return (n.numerador*m.denominador<n.denominador*m.numerador);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor que m.
		/// false en otro caso.</returns>
		public static bool operator <(BigRational n, BigInteger m)
		{
			if (n.entero<m) return true;
			else return false;
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor que m.
		/// false en otro caso.</returns>
		public static bool operator <(BigInteger n, BigRational m)
		{
			if (n<m.entero) return true;
			else if (n==m.entero || m.numerador>0) return true;
			else return false;
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor que m.
		/// false en otro caso.</returns>
		public static bool operator >(BigRational n, BigRational m)
		{
			if (n.entero<m.entero) return false;
			if (n.entero>m.entero) return true;
			return (n.numerador*m.denominador>n.denominador*m.numerador);
		}
		
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor que m.
		/// false en otro caso.</returns>
		public static bool operator >(BigRational n, BigInteger m)
		{
			if (n.entero<m) return false;
			if (n.entero>m) return true;
			return (n.numerador>0);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor que m.
		/// false en otro caso.</returns>
		public static bool operator >(BigInteger n, BigRational m)
		{
			if (n>m.entero) return true;
			else return false;
			
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator <=(BigRational n, BigRational m)
		{
			return !(n>m);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator <=(BigRational n, BigInteger m)
		{
			return !(n>m);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es menor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator <=(BigInteger n, BigRational m)
		{
			return !(n>m);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator >=(BigRational n, BigRational m)
		{
			return !(n<m);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator >=(BigRational n, BigInteger m)
		{
			return !(n<m);
		}
		/// <summary>
		/// Compara dos BigRational.
		/// </summary>
		/// <param name="n">número</param>
		/// <param name="m">número</param>
		/// <returns>true si n es mayor o igual que m.
		/// false en otro caso.</returns>
		public static bool operator >=(BigInteger n, BigRational m)
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
				return entero.sign;
			}
			set
			{
				if (value==entero.sign) return;
				
				entero.sign = value;
				
				if( this.numerador != 0 )
				{
					entero.subtract(BigInteger.LongToBigInteger(1));
					this.numerador.subtract(this.denominador);
					this.numerador.sign=!this.numerador.sign;
				}
			}
		}
		
		public override bool Equals(object obj)
		{
			if (obj is BigRational) 
				return (this==(obj as BigRational));
			return false;
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		/// <summary>
		/// Halla el reciproco de un BigRational.
		/// </summary>
		/// <returns>El reciproco de un BigRational.</returns>
		public BigRational reciproco()
		{
			return new BigRational(denominador, entero*denominador+numerador);
		}
	}
}
