using System;

namespace StoreComparatorXamarin
{
	public class Item
	{
		public long Id { get; set; }

		public long StoreId { get; set; }

		public string Name { get; set; }

		public string Category { get; set; }

		public float Price { get; set; }

		public float Weight { get; set; }

		public Item (long itemId, string itemName, string category, float price, float weight, long storeId) {
			this.Id = itemId;
			this.Name = itemName;
			this.Category = category;
			this.Price = price;
			this.Weight = weight;
			this.StoreId = storeId;
		}
	}
}