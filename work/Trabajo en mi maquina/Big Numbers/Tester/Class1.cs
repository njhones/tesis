using System;
using BigNumbers;

namespace Tester
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			int ok=0, total=1000;

			/*** probar operaciones de Rationals32 ***/
			BigRandom r = new BigRandom();
			Rational32 d1, d2, d3, d4;
			
			for(int i=0; i<total; i++)
			{
				d1 = r.NextRational32();
				d2 = r.NextRational32();
				d3 = d1*d2;
				d4 = d3/d2;
				if (d4==d1)
					ok++;
			}
			Console.WriteLine("ok: {0}, not ok: {1}",ok,total-ok);
			Console.WriteLine("BigInteger creados: {0}",BigInteger.creados);
			Console.WriteLine("Rational32 creados: {0}",Rational32.creados);

			
			/*** fin de probar operaciones ***/

			/*** probar operaciones de BigRationals ***
			BigRandom r = new BigRandom();
			BigRational d1, d2, d3, d4;
			
			for(int i=0; i<total; i++)
			{
				d1 = r.NextBigRational(5);
				d2 = r.NextBigRational(5);
				d3 = d1*d2;
				d4 = d3/d2;
				if (d4==d1)
					ok++;
			}
			Console.WriteLine("ok: {0}, not ok: {1}",ok,total-ok);
			Console.WriteLine("BigInteger creados: {0}",BigInteger.creados);
			Console.WriteLine("BigRational creados: {0}",BigRational.creados);


			/*** fin de probar operaciones ***/

			/*** probar operaciones de BigIntegers ***
			BigRandom r = new BigRandom();
			BigInteger d1, d2, d3;
			d1 = r.NextBigInteger(1);
			d3 = r.NextBigInteger(1);
			d3/=1000;
			d2 = 0;
			Console.WriteLine(d3[0]);
			uint p=0;
			for(uint i=1; i<d3; i++)
			{
				d2+=d1;
				if (i*100/d3[0]>=10+p)
				{
					p=i*100/d3[0];
					Console.WriteLine(p+" %");
				}

//				if (d1 == d2)
//				{
//					ok++;
//					//System.Console.WriteLine("ok. d3.length="+d3.lenght);
//				}
//				else
//					Console.WriteLine("nope, d1 = {0}, d2 = {1}, d3 = {2}", d1, d2, d1);	
				//d3=d3*d2;
			}
			Console.WriteLine("ok: {0}.", (d1*d3==d2));
			/*** fin de probar operaciones ***/

			/*** para probar el MCD ***
			Random r = new Random();
			int d1, d2, d3;
			for(int i=0; i<100; i++)
			{
				d1 = r.Next();
				d2 = r.Next();
				d3 = BigInteger.MCD(d1, d2);
				if (BigInteger.MCD(d1/d3, d2/d3) == 1) System.Console.WriteLine("ok");
				else
					Console.WriteLine("nope, d1 = {0}, d2 = {1}, d3 = {2}", d1, d2, d3);	
			}
			/*** fin de probar MCD ***/
			/*cosas mias*/
			BigInteger bi = r.NextBigInteger();
			int a = 5;
			Console.ReadLine();
		}
	}
}
