using System;

namespace Structures
{
	/// <summary>
	/// Descripci�n breve de Linked.
	/// </summary>
	public class Linked
	{
		/**/
		public Linked next, previous;
		/**/
		public Linked()
		{
			//
			// TODO: agregar aqu� la l�gica del constructor
			//
		}
		/**/
        public Linked(Linked next, Linked previous)
		{	
			
			this.next = next;
            this.previous = previous;
		}
	}
}
