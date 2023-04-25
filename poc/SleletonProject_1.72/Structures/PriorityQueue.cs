using System;
using System.Collections;

namespace Structures
{
	/// <summary>
	/// Descripción breve de PriorityQueue.
	/// </summary>
	public class PriorityQueue
	{
		private ArrayList A;
		private int heap_size; 
		public PriorityQueue()
		{
			this.A = new ArrayList();
			this.heap_size = -1;
		}
		public void Insert(IComparable key)
		{
			heap_size ++;		
			int i = heap_size;
			int parent_index = i >> 1;
					
			while(i > 0 && ((IComparable)A[parent_index]).CompareTo(key) < 0)
			{
				if(i < A.Count)
					A[i] = A[parent_index];
				else
					A.Add(A[parent_index]);

				i = parent_index;
				parent_index = i >> 1;
			}
			if(i < A.Count)
				A[i] = key;
			else
				A.Add(key);
		}
		public IComparable Extract()
		{
			if(heap_size < 0)
				throw new Exception("heap underflow");

			IComparable max = (IComparable)A[0];
			A[0] = A[heap_size];
			heap_size--;
			Heapify(0);
			return max;
		}
		
		private void Heapify(int i)
		{
			int left = (i == 0) ? 1 : i << 1;
			int right = (i == 0) ? 2 : i << 1 | 1;
			int largest;

			if(left <= heap_size && ((IComparable)A[left]).CompareTo(A[i]) > 0)
				largest = left;
			else
				largest = i;

			if(right <= heap_size && ((IComparable)A[right]).CompareTo(A[largest]) > 0)
				largest = right;

			if(largest != i)
			{
				Swap(i, largest);
				Heapify(largest);
			}
		}
		private void Swap(int i, int j)
		{
			object temp = A[i];
			A[i] = A[j];
			A[j] = temp;
		}
		public bool IsEmpty
		{
			get
			{
				return this.heap_size < 0;
			}
		}
	}
}
