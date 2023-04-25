using System;

namespace Structures
{
	/// <summary>
	/// Descripción breve de Linked.
	/// </summary>
	public class Linked
	{
		/**/
		public Linked next, previous;
		/**/
		public Linked()
		{
			//
			// TODO: agregar aquí la lógica del constructor
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
