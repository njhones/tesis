using System;

namespace BigNumbers
{
	
	public sealed class BigInteger
	{
		public static int creados = 0;

		/// <summary>
		/// Representa la base del BigInteger.
		/// </summary>
		public static readonly UInt64 BASE = ((ulong)UInt32.MaxValue)+1;

		/// <summary>
		/// Representa los dígitos del número. El dígito cero es 
		/// el que tiene menos nivel de significación.
		/// </summary>
		UInt32[] number;
		/// <summary>
		/// Representa la cantidad de dígitos en uso.
		/// </summary>
		int used;
		/// <summary>
		/// Especifica el signo del número. sign = true implica que el número 
		/// es no negativo.
		/// </summary>
		bool signo = true;
		/// <summary>
		/// Especifica el signo del número. sign = true implica que el número 
		/// es no negativo.
		/// </summary>
		public bool sign 
		{
			get
			{
				if (this.length==1 && this[0]==0)
					signo = true;
				return signo;
			}
			set
			{
				if (this.length==1 && this[0]==0)
					signo = true;
				else
					signo = value;
			}
		}

		public BigInteger()
		{
			
			number = new UInt32[4];
			used = 1;
			sign = true;
			creados++;
		}
		/// <summary>
		/// Construye el número con capacidad inicial prefijada.
		/// </summary>
		/// <param name="capacity">capacidad inicial</param>
		public BigInteger(int capacity, int nada)
		{
			number = new UInt32[capacity];
			used = 1;
			sign = true;
			creados++;
		}
		

		/// <summary>
		/// Construye un nuevo número a partir de otro. Constructor copia.
		/// </summary>
		/// <param name="n">número a copiar.</param>
		public BigInteger(BigInteger n)
		{
			number = new UInt32[n.capacity];
			used = n.used;
			for(int i = 0; i<n.length; i++)
				number[i] = n[i];
			sign = n.sign;
			creados++;
		}


		public override bool Equals(object obj)
		{
			return this==(BigInteger) obj;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		
		public void reset()
		{
			used=1;
			number.Initialize();
		}
		/// <summary>
		/// Cantidad de dígitos en uso.
		/// </summary>
		public int length
		{
			get
			{
				return used;
			}
		}

		/// <summary>
		/// Representa la cantidad de dígitos posibles. El último tiene índice capacity-1.
		/// </summary>
		public int capacity
		{
			get
			{
				return number.Length;
			}
			set
			{
				if (value >= number.Length)
				{
					uint[] temp = new uint[value];
					Array.Copy(number, temp, length);
					number = temp;
				}
			}
		}

		
		/// <summary>
		/// Representa el dígito i-ésimo. El dígito cero es el de menor significación.
		/// </summary>
		public UInt32 this [int i]
		{
			get
			{
				if (i<0)throw new ArgumentOutOfRangeException("i", i, "Index out of range.");
				if (i>=0&&i<used)
					return number[i];
				return 0;				
			}
			set
			{
				if (i>=capacity)
					capacity=i+i/5+1;
				number[i] = value;
				if( ( i == used -1 ) && ( value == 0 ) )
				{
					int j = i;
					while( ( used > 1 ) && ( number[j] == 0 ) )
					{
						used--;
						j--;
					}
				}
				if (i>=used && value!=0)
					used=i+1;		
			}
		}
		/// <summary>
		/// Elimina los ceros a la izquierda.
		/// Provoca que la capacidad sea igual al total de digitos usados.
		/// </summary>
		public void trim()
		{
			if (length==capacity) return;
			UInt32[] temp = new UInt32[used+1];
			Array.Copy(number, temp, used);
			number = temp;
		}//trim

		
		#region operadores booleanos


		/// <summary>
		/// Compara dos números.
		/// </summary>
		/// <param name="n">número a comparar</param>
		/// <param name="m">número a comparar</param>
		/// <returns>true si ambos números son iguales, false en otro caso.</returns>
		public static bool operator == (BigInteger n, BigInteger m)
		{
			if (n.sign != m.sign || n.length != m.length) return false;
			for (int i=0; i<n.length; i++)
				if (n[i]!=m[i]) return false;
			return true;
		}
		/// <summary>
		/// Compara dos números.
		/// </summary>
		/// <param name="n">número a comparar</param>
		/// <param name="m">número a comparar</param>
		/// <returns>true si ambos números son distintos, false en otro caso.</returns>
		public static bool operator != (BigInteger n, BigInteger m)
		{
			return !(n==m);
		}

		/// <summary>
		/// Compara dos números.
		/// </summary>
		/// <param name="n">número a comparar</param>
		/// <param name="m">número a comparar</param>
		/// <returns>true si n es mayor que m, false en otro caso.</returns>
		public static bool operator > (BigInteger n, BigInteger m)
		{
			if ( (n.sign && !m.sign) || n.length > m.length) return true;
			if ( (!n.sign && m.sign) || n.length < m.length) return false;
			for (int i=n.length-1; i>=0; i--)
			{
				if (n[i] > m[i]) return n.signo;
				if (n[i] < m[i]) return !n.signo;
			}
			return false;
		}
		/// <summary>
		/// Compara dos números.
		/// </summary>
		/// <param name="n">número a comparar</param>
		/// <param name="m">número a comparar</param>
		/// <returns>true si n es menor que m, false en otro caso.</returns>
		public static bool operator < (BigInteger n, BigInteger m)
		{
			if ( (n.sign && !m.sign) || n.length > m.length) return false;
			if ( (!n.sign && m.sign) || n.length < m.length) return true;
			for (int i=n.length-1; i>=0; i--)
			{
				if (n[i] > m[i]) return !n.signo;
				if (n[i] < m[i]) return n.signo;
			}
			return false;
		}

		/// <summary>
		/// Compara dos números.
		/// </summary>
		/// <param name="n">número a comparar</param>
		/// <param name="m">número a comparar</param>
		/// <returns>true si n es menor o igual que m, false en otro caso.</returns>
		public static bool operator <= (BigInteger n, BigInteger m)
		{
			return !(n>m);
		}
		/// <summary>
		/// Compara dos números.
		/// </summary>
		/// <param name="n">número a comparar</param>
		/// <param name="m">número a comparar</param>
		/// <returns>true si n es mayor o igual que m, false en otro caso.</returns>
		public static bool operator >= (BigInteger n, BigInteger m)
		{
			return !(n<m);
		}
		
		#endregion
		
		#region operadores aritmeticos
		/// <summary>
		/// Halla el opuesto de un número.
		/// </summary>
		/// <param name="n">número</param>
		/// <returns>el opuesto de n.</returns>
		public static BigInteger operator -(BigInteger n)
		{
			BigInteger result = new BigInteger (n);
			result.sign = !result.sign;
			return result;
		}

		/// <summary>
		/// Suma n con m. (n+m)
		/// </summary>
		/// <param name="n">sumando 1</param>
		/// <param name="m">sumando 2</param>
		/// <returns>la suma de n con m.</returns>
		public static BigInteger operator + (BigInteger n, BigInteger m)
		{
			if (n.sign != m.sign) return n - (-m);
			
			int max = Math.Max(n.length,m.length);
			BigInteger result = new BigInteger(max,1);

			ulong k=0;
	
			for(int j=0; j<max; j++)
			{	
				result[j]=(uint)(((ulong)n[j]+(ulong)m[j]+k)%BASE);
				k = ((ulong)n[j]+(ulong)m[j]+k)/BASE;
			}
			result[max]=(uint)k;
			
			result.sign = n.sign;
			return result;
		}//operator +

		/// <summary>
		/// Suma m al número actual. (n+=m)
		/// </summary>
		/// <param name="m">sumando</param>
		public void add(BigInteger m)
		{
			if (sign != m.sign) subtract(-m);
			
			int max = Math.Max(length,m.length);

			ulong k=0;
			uint temp=0;
	
			for(int j=0; j<max; j++)
			{	
				temp=(uint)(((ulong)this[j]+(ulong)m[j]+k)%BASE);
				k = ((ulong)this[j]+(ulong)m[j]+k)/BASE;
				this[j]=temp;
			}
			this[max]=(uint)k;
		}//add


		/// <summary>
		/// Resta m a n. (n-m)
		/// </summary>
		/// <param name="n">minuendo</param>
		/// <param name="m">sustraendo </param>
		/// <returns>la diferencia de n con m.</returns>
		public static BigInteger operator -(BigInteger n, BigInteger m)
		{
			if (m==0) return n;
			//si tienen distinto signo...
			if (n.sign != m.sign) return n + (-m);
			
			BigInteger result = new BigInteger(Math.Max(n.length,m.length),1);

			/// obtener el de mayor modulo y el de menor
			BigInteger mayor = new BigInteger(n), menor = new BigInteger(m);
			mayor.sign = true;
			menor.sign = true;
			if (mayor>=menor)
			{
				mayor = n;
				menor = m;
			}
			else
			{
				mayor = m;
				menor = n;
			}

			uint k=0;
			for(int j=0; j<mayor.length; j++)
			{
				result[j]=mayor[j]-menor[j]-k;
				if (k==0)//no hubo acarreo
					k=(mayor[j]<menor[j])?1u:0u;
				else
					k=(mayor[j]<menor[j]+k || menor[j]+k<menor[j])?1u:0u;
			}
			result.sign = (n==mayor)?n.sign:!m.sign;
			return result;
		}//operator -

		/// <summary>
		/// Resta m a n. (n-m)
		
		/// <summary>
		/// Resta m al número actual. (n-=m)
		/// </summary>
		/// <param name="m">sustraendo </param>
		public void subtract(BigInteger m)
		{
			///la proxima linea es necesaria pues si this.sign=false y m es cero
			///y se hace add(-m),
			///en realidad no se le cambia el signo a m por ser cero.
			///Entraríamos a llamados infinitos de add-subtract.
			if (m==0) return;
			//si tienen distinto signo...
			if (this.sign != m.sign) add(-m);
			
			BigInteger result = new BigInteger(Math.Max(this.length,m.length),1);

			/// obtener el de mayor modulo y el de menor
			BigInteger mayor = new BigInteger(this), menor = new BigInteger(m);
			mayor.sign = true;
			menor.sign = true;
			if (mayor>=menor)
			{
				mayor = this;
				menor = m;
			}
			else
			{
				mayor = m;
				menor = this;
			}

			uint k=0, temp=0;
			for(int j=0; j<mayor.length; j++)
			{
				temp=mayor[j]-menor[j]-k;
				if (k==0)//no hubo acarreo
					k=(mayor[j]<menor[j])?1u:0u;
				else
					k=(mayor[j]<menor[j]+k || menor[j]+k<menor[j])?1u:0u;
				this[j]=temp;
			}
			sign = (m==mayor)?!m.sign:sign;
		}

		/// <summary>
		/// Multiplica u con v.
		/// </summary>
		/// <param name="u">factor 1</param>
		/// <param name="v">factor 2</param>
		/// <returns>el producto de ambos.</returns>
		public static BigInteger operator * (BigInteger u, BigInteger v)
		{
			int m = u.length,
				n = v.length;
			uint k=0;
			ulong t=0;
			BigInteger w = new BigInteger(m+n,1);

			for(int j=0; j<n; j++)
			{
				if (v[j]==0) w[j+m]=0;
				else
				{
					k=0;
					for (int i=0; i<m; i++)
					{
						t=(ulong)u[i]*(ulong)v[j]+(ulong)w[i+j]+(ulong)k;
						w[i+j]=(uint)(t%BASE);
						k=(uint)(t/BASE);
					}
					w[j+m]=k;
				}
			}
			w.sign = (u.sign && v.sign) || (!u.sign && !v.sign);
			return w;
		}//operator *

		
		/// <summary>
		/// Multiplica u con v.
		/// <summary>
		/// Divide divisor a dividendo.
		/// </summary>
		/// <param name="dividendo">Dividendo</param>
		/// <param name="divisor">Divisor</param>
		/// <returns>la division dividendo / divisor .</returns>
		public static BigInteger operator /(BigInteger dividendo, BigInteger divisor)
		{
			return DivisionAndRemainder(dividendo, divisor)[0];
		}//operator /

		/// <summary>
		/// Divide divisor a dividendo.
		/// <summary>
		/// Calcula el resto de la division divisor / dividendo.
		/// </summary>
		/// <param name="dividendo">Dividendo</param>
		/// <param name="divisor">Divisor</param>
		/// <returns>resto de la division dividendo / divisor .</returns>
		public static BigInteger operator % (BigInteger dividendo, BigInteger divisor)
		{
			return DivisionAndRemainder(dividendo, divisor)[1];
		}//operator %
		#endregion

		public static implicit operator BigInteger (uint n)
		{
			BigInteger res = new BigInteger();
			res[0] = n;
			return res;
		}

		public static implicit operator BigInteger(long l)
		{
			BigInteger res = new BigInteger();
			res.signo = (l>=0);
			
			if (l<0) l=-l;

			res[0] = lowUInt32( (ulong) l);
			res[1] = highUInt32((ulong) l);
			creados++;
			return res;
		}

		public static implicit operator BigInteger (ulong n)
		{
			BigInteger res = new BigInteger();
			res[0] = lowUInt32(n);
			res[1] = highUInt32(n);
			return res;
		}

		/// <summary>
		/// Convierte los segundos 4 bytes (del 4 al 7) de n a UInt32.
		/// </summary>
		public static uint highUInt32(ulong n)
		{
			byte[] bytes = BitConverter.GetBytes(n);
			return BitConverter.ToUInt32(bytes,4);
		}
		/// <summary>
		/// Convierte los primeros 4 bytes (del 0 al 3) de n a UInt32.
		/// </summary>
		public static uint lowUInt32(ulong n)
		{
			byte[] bytes = BitConverter.GetBytes(n);
			return BitConverter.ToUInt32(bytes,0);
		}


		/// <summary>
		/// Calcula la división entera y el resto de n con m.
		/// El signo de la parte entera es negativo si los signos de n y de m son distintos, positivo en otro caso.
		/// El resto tiene el mismo signo que n.
		/// </summary>
		/// <param name="n">Un número cualquiera.</param>
		/// <param name="m">Número de un sólo dígito</param>
		/// <returns>Un arreglo de dos BigInteger donde el primer elemento es la división entera y el segundo es el resto.</returns>
		public static BigInteger[] DivisionAndRemainder(BigInteger dividendo, BigInteger divisor)
		{
			if (divisor.length==1)
			{
				if (divisor == 0) throw new DivideByZeroException("División por cero.");
				else
					return simpleDivisionAndRemainder(dividendo, divisor);
			}
			
			BigInteger result = new BigInteger();
			
			int n = divisor.length,
				m = dividendo.length-n;

			//D1: Normalizar
			ulong d = BigInteger.BASE/((ulong)divisor[n-1]+1);
			BigInteger
				u = dividendo*d,
                v = divisor*d;
            
			ulong qt, rt;
			ulong prod=0, ak=0;
			uint t;
			
			for (int j=m; j>=0; j--)
			{
				//D3 calcular qt
				qt = (((ulong)u[j+n])*BASE+(ulong)u[j+n-1])/(ulong)v[n-1];
				rt = (((ulong)u[j+n])*BASE+(ulong)u[j+n-1])%(ulong)v[n-1];

				if (qt == BASE || ((qt*(ulong)v[n-2])>(BASE*rt+(ulong)u[j+n-2])) )
				{
					qt--;
					rt+=(ulong)v[n-1];
					if (rt<BASE && (qt==BASE||(qt*(ulong)v[n-2])>(BASE*rt+(ulong)u[j+n-2])) )
					{
						qt--;
						rt+=(ulong)v[n-1];
					}
				}

				//D4 Multiplicar y restar
				ak=0;
				for (int i=j; i<n+j+1; i++)
				{
					prod=qt*(ulong)v[i-j]+ak;
					ak=highUInt32(prod);
					t=lowUInt32(prod);
					u[i]-=t;
					if (u[i]+t<u[i])//hay acarreo
						ak++;
				}

				//si la dif fue negativa
				if (ak!=0)
				{
					u.sign = false;
					BigInteger bn = new BigInteger();
					bn[n+2] = 1;
					
					u.add(bn);

					//D6: Add Back
					qt--;
					u.add(v);
					//ignorar acarreo:
					u[j+n+1]=0;
				}

				result[j] = lowUInt32(qt);
			}

			u[n]=0;
			result.sign=(dividendo.sign==divisor.sign);
			
			BigInteger resto = u/d;
			resto.sign=u.sign;

			BigInteger[] res = new BigInteger[2];
			res[0] = result;
			res[1] = resto;
			return res;
		}

		/// <summary>
		/// Calcula la división entera y el resto de n con m. m es un número de un solo dígito.
		/// El signo de la parte entera es negativo si los signos de n y de m son distintos, positivo en otro caso.
		/// El resto tiene el mismo signo que n.
		/// </summary>
		/// <param name="n">Un número cualquiera.</param>
		/// <param name="m">Número de un sólo dígito</param>
		/// <returns>Un arreglo de dos BigInteger donde el primer elemento es la división entera y el segundo es el resto.</returns>
		private static BigInteger[] simpleDivisionAndRemainder(BigInteger n, BigInteger m)
		{			
			if (m.length!=1) throw new ArgumentException("argumento no válido. (m.lenght!=1)", "m");
			if (m[0]==0) throw new DivideByZeroException();
			
			BigInteger[] res = new BigInteger[2];
			if (m[0]==1)
			{
				res[0] = (m.sign)?(new BigInteger(n)):( -(new BigInteger(n)));
				res[1]=0;
				return res;
			}


			BigInteger result = new BigInteger(n.length,1);
			ulong resto = 0;
			ulong divisor = (ulong)m[0];
			
			for (int j=n.length-1; j>=0; j--)
			{
				result[j]=lowUInt32((resto*BASE+(ulong)n[j])/divisor);
				resto = lowUInt32((resto*BASE+(ulong)n[j])%divisor);
			}
			result.sign = (n.sign == m.sign);
			
			res = new BigInteger[2];
			res [0]=result;
			res[1]=resto;
			res[1].sign=n.sign;
			return res;
		}
		

		/// <summary>
		/// Multiplicación por un número de un solo digito.
		/// </summary>
		/// <param name="n">Un número cualquiera.</param>
		/// <param name="m">Número de un sólo dígito</param>
		/// <returns>El producto de ambos.</returns>
		private static BigInteger simpleMultiply(BigInteger n, BigInteger m)
		{
			if (m.length!=1) throw new ArgumentException("argumento no válido.", "m");
			uint factor = m[0];

			BigInteger result = new BigInteger(n.capacity,1);
			ulong prod = 0;

			for(int i=0; i<n.length; i++)
			{
                prod += (ulong)n[i]*(ulong)factor;
				result[i]=lowUInt32(prod);
				prod = highUInt32(prod);
			}
			result[n.length]=lowUInt32(prod);
			result.sign = (n.sign==m.sign);
			return result;
		}
		

		/// <summary>
		/// Calcula el máximo común divisor de dos números.
		/// </summary>
		/// <param name="n">Un número</param>
		/// <param name="m">Un número</param>
		/// <returns>el máximo común divisor de n y m.</returns>
		public static BigInteger MCD(BigInteger n, BigInteger m)
		{
			if ((n.length==1&&n[0]==0) || (m.length==1 && m[0]==0))
				return LongToBigInteger(1);

			BigInteger r,u,v;
			
			u=n;
			v=new BigInteger(m);
			while (v!=0)
			{
				r=u%v;
				u=v;
				v=r;
			}
			u.signo=true;
			return u;
		}
	
		/// <summary>
		/// Calcula el máximo común divisor de dos números.
		/// </summary>
		/// <param name="n">Un número</param>
		/// <param name="m">Un número</param>
		/// <returns>el máximo común divisor de n y m.</returns>
		public static uint MCD(uint n, uint m)
		{
			uint r,u,v;
			u=n;
			v=m;
			while (v!=0)
			{
				r=u%v;
				u=v;
				v=r;
			}
			return u;
		}
	
		/// <summary>
		/// Calcula el máximo común divisor de dos números.
		/// </summary>
		/// <param name="n">Un número</param>
		/// <param name="m">Un número</param>
		/// <returns>el máximo común divisor de n y m.</returns>
		public static ulong MCD(ulong n, ulong m)
		{
			ulong r,u,v;
			u=n;
			v=m;

			while (v!=0)
			{
				r=u%v;
				u=v;
				v=r;
			}
			return u;
		}
	
		/// <summary>
		/// Calcula el máximo común divisor de dos números.
		/// </summary>
		/// <param name="n">Un número</param>
		/// <param name="m">Un número</param>
		/// <returns>el máximo común divisor de n y m.</returns>
		public static long MCD(long n, long m)
		{
			long r,u,v;
			u=n;
			v=m;
			while (v!=0)
			{
				r=u%v;
				u=v;
				v=r;
			}
			return Math.Abs(u);
		}
	
		/// <summary>
		/// Calcula el máximo común divisor de dos números.
		/// </summary>
		/// <param name="n">Un número</param>
		/// <param name="m">Un número</param>
		/// <returns>el máximo común divisor de n y m.</returns>
		public static int MCD(int n, int m)
		{
			int r,u,v;
			u=n;
			v=m;
			while (v!=0)
			{
				r=u%v;
				u=v;
				v=r;
			}
			return Math.Abs(u);
		}

		public static BigInteger LongToBigInteger(long l)
		{
			BigInteger result= l;

			return result;
		}
		public static long BigIntegerToLong(BigInteger l)
		{
			if (l.length>2) throw new OverflowException("Overflow typecasting a BigInteger to long");
			if (l.length==2 && ((int)l.number[1])<0) 
				            throw new OverflowException("Overflow typecasting a BigInteger to long");

            long temp = (l.length>1)?l.number[1]:0;
			temp = (temp<<32)+l.number[0];
						
			return (l.sign)?temp:-temp;
		}
	
	}
}

