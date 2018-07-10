using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.collection {
	class ListWithEvents<T> : List<T> {
		public delegate void AddDelegate();
		public event AddDelegate AddItem;

		new void Add(T item) {
			AddItem?.Invoke();
			base.Add(item);
		}
	}
}
