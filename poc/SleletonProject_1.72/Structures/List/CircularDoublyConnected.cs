using System;
using System.Collections;

namespace Structures
{
	/// <summary>
	/// Descripción breve de CircularDoublyConnected.
	/// </summary>
	public class CircularDoublyConnected: IEnumerable
	{
        private Linked cursor;
		private uint count;

        public CircularDoublyConnected()
		{
		}

        public Linked Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }

        /**/
		public void Add(Linked k)
		{
			if(count == 0)
			{
				this.cursor = k;
                cursor.next = cursor;
                cursor.previous = cursor;
			}
			else
			{
				k.next = cursor.next;
				k.previous = cursor;
                k.next.previous = k;
                k.previous.next = k;
                cursor = k;
			}
			
            this.count++;
		}

        public void AddNext(Linked n, Linked k)
        {
            k.next = n.next;
            k.previous = n;
            k.next.previous = k;
            k.previous.next = k;
            this.count++;
        }

        public void AddPrevious(Linked n, Linked k)
        {
            k.previous = n.previous;
            k.next = n;
            k.next.previous = k;
            k.previous.next = k;
            this.count++;
        }

        public void Replace(Linked rep, Linked va, Linked vb)
        {
            if (count == 2)
                count = 0;
            else
            {
                cursor = (va.next == vb) ? va.previous : vb.previous;
                RemoveNode(cursor.next);
                RemoveNode(cursor.next);
            }

            Add(rep);
        }

        public void Replace(Linked n, Linked k)
        {
            k.next = n.next;
            k.previous = n.previous;
            k.next.previous = k;
            k.previous.next = k;
            if (cursor == n)
                cursor = k;
        }

        public void RemoveNode(Linked n)
        {
            n.next.previous = n.previous;
            n.previous.next = n.next;
            if (cursor == n)
                cursor = n.next;
            count--;
        }

		
		/**/
		public uint Count
		{
			get
			{
				return this.count;
			}
		}
		/**/
		protected class CircularDoublyConnectedEnum: IEnumerator
		{
			/**/
			private CircularDoublyConnected list;			
			/**/
            private Linked current;
            private Linked mark;
            /**/
			private bool init; 
			/**/
			public CircularDoublyConnectedEnum(CircularDoublyConnected list)
			{
				this.list = list;
				this.mark = list.Cursor;
                Reset();
			}
			
			#region Miembros de IEnumerator

			public void Reset()
			{
				current = mark;
				this.init = false;
			}

			public object Current
			{
				get
				{
					if (!init)
						throw new Exception("No se ha iniciado la iteración");

                    //if (current == mark)
                    //    throw new Exception("Fin de la iteración");

					return this.current;
				}
			}

			public bool MoveNext()
			{
                if (!init)
                    return (init = true);
                else
                {
                    current = current.next;
                    return (current != mark);
                }
			}

			#endregion
		}		

		#region Miembros de IEnumerable

		public IEnumerator GetEnumerator()
		{
			return new CircularDoublyConnectedEnum(this);
		}
		
		#endregion
	}
}
