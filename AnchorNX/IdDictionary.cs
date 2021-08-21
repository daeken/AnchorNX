using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AnchorNX {
	public class IdDictionary {
		readonly ConcurrentDictionary<int, object> Objs = new();

		public ICollection<object> Values => Objs.Values;

		public bool Add(int id, object data) => Objs.TryAdd(id, data);

		public int Add(object data) {
			for(var id = 1; id < int.MaxValue; id++) {
				if(Objs.TryAdd(id, data)) {
					return id;
				}
			}

			throw new InvalidOperationException();
		}

		public object GetData(int id) => Objs.TryGetValue(id, out object data) ? data : null;

		public T GetData<T>(int id) {
			if(Objs.TryGetValue(id, out object data) && data is T value)
				return value;

			return default;
		}

		public object Delete(int id) => Objs.TryRemove(id, out var obj) ? obj : null;

		public ICollection<object> Clear() {
			var values = Objs.Values;
			Objs.Clear();
			return values;
		}
	}
}