using System;

namespace BigNumbers
{
	/// <summary>
	/// Clase para generar numeros aleatorios.
	/// </summary>
	public class BigRandom : Random
	{
		public BigRandom()
		{
		}
		/// <summary>
		/// Retorna un BigInteger pseudo-random.
		/// Peligroso porque el tamaño es aleatorio!!!
		/// </summary>
		/// <returns>BigInteger pseudo-random</returns>
		public BigInteger NextBigInteger()
		{
			int size = Next();
			BigInteger result = new BigInteger();
			for(int i=0; i<size; i++)
				result[i]=(uint)this.Next();
			return result;
		}
		/// <summary>
		/// Retorna un BigInteger pseudo-random
		/// </summary>
		/// <param name="size">cantidad maxima de digitos distintos de cero.</param>
		/// <returns>BigInteger pseudo-random</returns>
		public BigInteger NextBigInteger(int size)
		{
			BigInteger result = new BigInteger();
			for(int i=0; i<size; i++)
				result[i]=(uint)this.Next();
			return result;
		}
		/// <summary>
		/// Retorna un BigRational pseudo-random
		/// </summary>
		/// <param name="size">cantidad maxima de digitos distintos de cero en los BigIntegers aue lo componen.</param>
		/// <returns>BigRational pseudo-random</returns>
		public BigRational NextBigRational(int size)
		{
			return new BigRational(this.NextBigInteger(size), this.NextBigInteger(size));
		}
		/// <summary>
		/// Retorna un Rational32 pseudo-random
		/// </summary>
		/// <returns>Rational32 pseudo-random</returns>
		public Rational32 NextRational32()
		{
			return new Rational32(this.Next(), this.Next());
		}
		/// <summary>
		/// Retorna un Rational64 pseudo-random
		/// </summary>
		/// <returns>Rational64 pseudo-random</returns>
		public Rational64 NextRational64()
		{
			return new Rational64((long)this.NextDouble()*Int64.MaxValue, (long)this.NextDouble()*Int64.MaxValue);
		}
	}
}
