
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
using Android.Util;

namespace StoreComparatorXamarin
{
	[Activity (Label = "SelectItemsToCompareActivity")]			
	public class SelectItemsToCompareActivity : Activity, ListView.IOnItemClickListener
	{
		public const string SELECTED_ITEMS_NAMES = "com.lazarlazarevic.storecomparer.SELECTED_ITEMS_NAMES";
		ArrayAdapter<string> adapter;
		ListView listView;
		List<Item> intersectionOfItems;
		byte checkedItems = 0;
		long[] store_ids;

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_select_items_to_compare);

			Intent intent = Intent;
			store_ids = intent.GetLongArrayExtra (SelectStoresActivity.STORE_IDS_MESSAGE);

			List<Item> items;
			intersectionOfItems = MainActivity.db.GetItemsFromStore (store_ids [0]);

			for (int i = 1; i < store_ids.Length; i++) {
				items = MainActivity.db.GetItemsFromStore (store_ids [i]);
				intersectionOfItems = Intersect (intersectionOfItems, items);
				items.Clear ();
			}

			listView = (ListView)FindViewById (Resource.Id.itemsToCompareList);
			List<string> s = new List<string> (intersectionOfItems.Select (x => x.Name));

			adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItemMultipleChoice, s);
			listView.Adapter = adapter;
			listView.ItemsCanFocus = false;
			listView.OnItemClickListener = this;

			Button compare = FindViewById<Button> (Resource.Id.btnCompareChoosenItems);
			compare.Click += (sender, e) => {


				if (checkedItems < 1) {
					Toast.MakeText (this, "You have to select at least one item.", ToastLength.Short).Show ();
					return;
				}
				Intent iintent = new Intent (this, typeof(ComparedItemsActivity));
				string[] itemsNames = new string[checkedItems];
				byte count = 0;
				int len = listView.Count;
				SparseBooleanArray check = listView.CheckedItemPositions;
				for (int i = 0; i < len; i++)
					if (check.Get (i)) {
						itemsNames [count++] = adapter.GetItem (i);
					}

				iintent.PutExtra (SELECTED_ITEMS_NAMES, itemsNames);
				iintent.PutExtra (SelectStoresActivity.STORE_IDS_MESSAGE, store_ids);
				StartActivity (iintent);
			};
		}

		public void OnItemClick (AdapterView parent, View view, int position, long id) {
			checkedItems = 0;
			int len = listView.Count;
			SparseBooleanArray check = listView.CheckedItemPositions;
			for (int i = 0; i < len; i++)
				if (check.Get (i)) {
					checkedItems++;
				}
		}

		private List<Item> Intersect (List<Item> A, List<Item> B) {
			List<Item> rtnList = new List<Item> ();
			foreach (var dto in A) {
				if (HasName (B, dto.Name)) {
					rtnList.Add (dto);
				}
			}
			intersectionOfItems.Clear ();
			return rtnList;
		}

		private bool HasName (List<Item> items, String name) {
			foreach (var item in items) {
				if (item.Name.Equals (name)) {
					return true;
				}
			}
			return false;
		}
	}
}

