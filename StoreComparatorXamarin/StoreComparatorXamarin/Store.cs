using System;

namespace StoreComparatorXamarin
{
	public class Store
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public Store (long storeId, String storeName) {
			this.Id = storeId;
			this.Name = storeName;
		}
	}
}

