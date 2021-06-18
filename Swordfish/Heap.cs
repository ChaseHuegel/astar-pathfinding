using UnityEngine;
using System.Collections;
using System;

namespace Swordfish
{
	public interface IHeapItem<T> : IComparable<T>
	{
		int HeapIndex { get; set; }
	}

	public class Heap<T> where T : IHeapItem<T>
	{
		private T[] items;
		private int count;
		public int Count { get { return count; } }

		public Heap(int size)
		{
			items = new T[size];
		}

		public bool IsFull()
		{
			return count >= (items.Length);
		}

		public void Add(T item)
		{
			item.HeapIndex = count;
			items[count] = item;
			SortUp(item);
			count++;
		}

		public T RemoveFirst()
		{
			count--;

			T firstItem = items[0];
			items[0] = items[count];
			items[0].HeapIndex = 0;

			SortDown(items[0]);

			return firstItem;
		}

		public void UpdateItem(T item)
		{
			SortUp(item);
		}

		public bool Contains(T item)
		{
			return Equals(items[item.HeapIndex], item);
		}

		protected void SortDown(T item)
		{
			while (true)
			{
				int childIndexLeft = item.HeapIndex * 2 + 1;
				int childIndexRight = item.HeapIndex * 2 + 2;
				int swapIndex = 0;

				if (childIndexLeft < count)
				{
					swapIndex = childIndexLeft;

					if (childIndexRight < count)
						if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
							swapIndex = childIndexRight;

					if (item.CompareTo(items[swapIndex]) < 0)
						Swap (item,items[swapIndex]);
					else
						return;
				}
				else
				{
					return;
				}

			}
		}

		protected void SortUp(T item)
		{
			int parentIndex = (item.HeapIndex-1)/2;

			while (true)
			{
				T parentItem = items[parentIndex];

				if (item.CompareTo(parentItem) > 0)
					Swap (item,parentItem);
				else
					break;

				parentIndex = (item.HeapIndex-1)/2;
			}
		}

		protected void Swap(T itemA, T itemB)
		{
			items[itemA.HeapIndex] = itemB;
			items[itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}
	}
}