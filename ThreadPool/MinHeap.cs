using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{

	/// <summary>Implements a binary heap that prioritizes smaller values.</summary>
	public class MinBinaryHeap<TKey,TValue>	where TKey : IComparable<TKey>
	{
		private readonly List<KeyValuePair<TKey, TValue>> _items;

		/// <summary>Initializes an empty heap.</summary>
		public MinBinaryHeap()
		{
			_items = new List<KeyValuePair<TKey, TValue>>();
		}

		/// <summary>Initializes a heap as a copy of another heap instance.</summary>
		/// <param name="heapToCopy">The heap to copy.</param>
		/// <remarks>Key/Value values are not deep cloned.</remarks>
		public MinBinaryHeap(MinBinaryHeap<TKey,TValue> heapToCopy)
		{
			_items = new List<KeyValuePair<TKey, TValue>>(heapToCopy.Items);
		}

		/// <summary>Empties the heap.</summary>
		public void Clear() { _items.Clear(); }

		/// <summary>Adds an item to the heap.</summary>
		public void Insert(TKey key, TValue value)
		{
			// Create the entry based on the provided key and value
			Insert(new KeyValuePair<TKey, TValue>(key, value));
		}

		/// <summary>Adds an item to the heap.</summary>
		public void Insert(KeyValuePair<TKey, TValue> entry)
		{
			// Add the item to the list, making sure to keep track of where it was added.
			_items.Add(entry);
			int pos = _items.Count - 1;

			// If the new item is the only item, we're done.
			if (pos == 0) return;

			// Otherwise, perform log(n) operations, walking up the tree, swapping
			// where necessary based on key values
			while (pos > 0)
			{
				// Get the next position to check
				int nextPos = (pos - 1) / 2;

				// Extract the entry at the next position
				var toCheck = _items[nextPos];

				// Compare that entry to our new one.  If our entry has a smaller key, move it up.
				// Otherwise, we're done.
				if (entry.Key.CompareTo(toCheck.Key) < 0)
				{
					_items[pos] = toCheck;
					pos = nextPos;
				}
				else break;
			}

			// Make sure we put this entry back in, just in case
			_items[pos] = entry;
		}

		/// <summary>Returns the entry at the top of the heap.</summary>
		public KeyValuePair<TKey, TValue> Peek()
		{
			// Returns the first item
			if (_items.Count == 0) throw new InvalidOperationException("The heap is empty.");
			return _items[0];
		}

		/// <summary>Removes the entry at the top of the heap.</summary>
		public KeyValuePair<TKey, TValue> Remove()
		{
			// Get the first item and save it for later (this is what will be returned).
			if (_items.Count == 0) throw new InvalidOperationException("The heap is empty.");
			KeyValuePair<TKey, TValue> toReturn = _items[0];

			// Remove the first item if there will only be 0 or 1 items left after doing so.  
			if (_items.Count <= 2) _items.RemoveAt(0);
			// A reheapify will be required for the removal
			else
			{
				// Remove the first item and move the last item to the front.
				_items[0] = _items[_items.Count - 1];
				_items.RemoveAt(_items.Count - 1);

				// Start reheapify
				int current = 0, possibleSwap = 0;

				// Keep going until the tree is a heap
				while (true)
				{
					// Get the positions of the node's children
					int leftChildPos = 2 * current + 1;
					int rightChildPos = leftChildPos + 1;

					// Should we swap with the left child?
					if (leftChildPos < _items.Count)
					{
						// Get the two entries to compare (node and its left child)
						var entry1 = _items[current];
						var entry2 = _items[leftChildPos];

						// If the child has a lower key than the parent, set that as a possible swap
						if (entry2.Key.CompareTo(entry1.Key) < 0) possibleSwap = leftChildPos;
					}
					else break; // if can't swap this, we're done

					// Should we swap with the right child?  Note that now we check with the possible swap
					// position (which might be current and might be left child).
					if (rightChildPos < _items.Count)
					{
						// Get the two entries to compare (node and its left child)
						var entry1 = _items[possibleSwap];
						var entry2 = _items[rightChildPos];

						// If the child has a lower key than the parent, set that as a possible swap
						if (entry2.Key.CompareTo(entry1.Key) < 0) possibleSwap = rightChildPos;
					}

					// Now swap current and possible swap if necessary
					if (current != possibleSwap)
					{
						var temp = _items[current];
						_items[current] = _items[possibleSwap];
						_items[possibleSwap] = temp;
					}
					else break; // if nothing to swap, we're done

					// Update current to the location of the swap
					current = possibleSwap;
				}
			}

			// Return the item from the heap
			return toReturn;
		}

		/// <summary>Gets the number of objects stored in the heap.</summary>
		public int Count { get { return _items.Count; } }

		internal List<KeyValuePair<TKey, TValue>> Items { get { return _items; } }
	}
}
