
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StoreComparatorXamarin
{
	[Activity (Label = "ComparedItemsActivity")]			
	public class ComparedItemsActivity : Activity
	{
		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_compared_items);

			Intent intent = Intent;
			TextView totalPrice;
			totalPrice = (TextView)FindViewById(Resource.Id.txtTotalPrice);
			String[] names = intent.GetStringArrayExtra(SelectItemsToCompareActivity.SELECTED_ITEMS_NAMES);
			long[] stores_ids = intent.GetLongArrayExtra(SelectStoresActivity.STORE_IDS_MESSAGE);
			float[] prices = new float[stores_ids.Length];

			for (int i = 0; i < stores_ids.Length; i++)
			{
				float total = 0;
				for (int j = 0; j < names.Length; j++)
				{
					Item item = MainActivity.db.GetItemFromStoreWithName(stores_ids[i], names[j]);
					total += item.Price;
				}
				prices[i] = total;
			}

			float lowestPrice = 999999;
			int index = 0;
			for(int i = 0; i < prices.Length; i++)
			{
				if(prices[i] < lowestPrice)
				{
					lowestPrice = prices[i];
					index = i;
				}
			}

			Store store = MainActivity.db.GetStore(stores_ids[index]);
			totalPrice.Text = "The cheapest store is " + store.Name + " with total price of " + lowestPrice;
		}
	}
}

