using System;

namespace Swordfish
{
	public class ArrayUtils
	{
		public static void ShiftLeft<T>(T[] _array, int _shifts)
		{
			if (_shifts > 0)
			{
				Array.Copy(_array, _shifts, _array, 0, _array.Length - _shifts);
				Array.Clear(_array, _array.Length - _shifts, _shifts);
			}
		}

		public static void ShiftRight<T>(T[] _array, int _shifts)
		{
			if (_shifts > 0)
			{
				Array.Copy(_array, 0, _array, _shifts, _array.Length - _shifts);
				Array.Clear(_array, 0, _shifts);
			}
		}

		public static void ShiftUp<T>(ref T[] _array, int _shifts)
		{
			if (_shifts > 0)
			{
				for (int i = 0; i < _shifts; i++)
				{
					T[] temp = new T[_array.Length];

					T last = _array[_array.Length - 1];
					Array.Copy(_array, 0, temp, 1, _array.Length - 1);
					temp[0] = last;

					_array = temp;
				}
			}
		}
		
		public static void ShiftDown<T>(ref T[] _array, int _shifts)
		{
			if (_shifts > 0)
			{
				for (int i = 0; i < _shifts; i++)
				{
					T[] temp = new T[_array.Length];

					T first = _array[0];
					Array.Copy(_array, 1, temp, 0, _array.Length - 1);
					temp[_array.Length - 1] = first;

					_array = temp;
				}
			}
		}

		public class Container<T>
		{
			private T[] elements;

			public Container()
			{
				elements = new T[0];
			}

			public Container(T[] _elements)
			{
				elements = _elements;
			}

			public int Size()
			{
				return elements.Length;
			}

			public T[] GetArray()
			{
				return elements;
			}

			public T GetAt(int _index)
			{
				if (_index >= 0 && _index < elements.Length)
				{
					return elements[_index];
				}

				return default (T);
			}

			public void Clear()
			{
				elements = new T[0];
			}

			public void Add(T _entry)
			{
				T[] temp = new T[elements.Length + 1];

				for (int i = 0; i < elements.Length; i++)
				{
					temp[i] = elements[i];
				}

				temp[elements.Length] = _entry;
				elements = temp;
			}

			public void SetAt(int _index, T _entry)
			{
				elements[_index] = _entry;
			}

			public T RemoveAt(int _index)
			{
				T[] temp = new T[elements.Length - 1];

				if (temp.Length > 0)
				{
					for (int i = 0; i < _index; i++)
					{
						temp[i] = elements[i];
					}

					for (int i = _index + 1; i < elements.Length; i++)
					{
						temp[i - 1] = elements[i];
					}
				}

				T removedElement = elements[_index];
				elements = temp;

				return removedElement;
			}

			public T Remove()
			{
				return RemoveAt(elements.Length - 1);
			}

			public T Dequeue()
			{
				T[] temp = new T[elements.Length - 1];

				if (temp.Length > 0)
				{
					for (int i = 1; i < elements.Length; i++)
					{
						temp[i - 1] = elements[i];
					}
				}

				T removedElement = elements[0];
				elements = temp;

				return removedElement;
			}

			public void ShiftUp()
			{
				T[] temp = new T[elements.Length];

				T last = elements[elements.Length - 1];
				Array.Copy(elements, 0, temp, 1, elements.Length - 1);
				temp[0] = last;

				elements = temp;
			}

			public void ShiftDown()
			{
				T[] temp = new T[elements.Length];

				T first = elements[0];
				Array.Copy(elements, 1, temp, 0, elements.Length - 1);
				temp[elements.Length - 1] = first;

				elements = temp;
			}
		}
	}
}