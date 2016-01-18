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
	[Activity (Label = "SelectStoresActivity")]			
	public class SelectStoresActivity : Activity, ListView.IOnItemClickListener
	{
		public const string STORE_IDS_MESSAGE = "com.lazarlazarevic.storecomparer.STORE_IDS_MESSAGE";

		ListView listView;
		List<Store> stores;
		ArrayAdapter<string> adapter;
		byte checkedItems = 0;

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_select_stores);

			stores = MainActivity.db.GetAllStores ();

			listView = (ListView)FindViewById (Resource.Id.currentStores);
			List<string> s = new List<string> (stores.Select (x => x.Name));
			adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItemMultipleChoice, s);
			listView.Adapter = adapter;
			listView.ItemsCanFocus = false;

			listView.OnItemClickListener = this;

			Button addItemsToCompare = FindViewById<Button> (Resource.Id.addItemsToCompare);
			addItemsToCompare.Click += (sender, e) => {
				if (checkedItems < 2) {
					Toast.MakeText (this, "You have to select at least two stores.", ToastLength.Short).Show ();
					return;
				}
				Toast.MakeText (this, "Choose items.", ToastLength.Short).Show ();
				Intent intent = new Intent (this, typeof(SelectItemsToCompareActivity));
				long[] storeIds = new long[checkedItems];
				byte count = 0;
				int len = listView.Count;
				SparseBooleanArray check = listView.CheckedItemPositions;
				for (int i = 0; i < len; i++)
					if (check.Get (i)) {
						Store ss = stores.ElementAt(i);
						storeIds [count++] = ss.Id;
					}

				intent.PutExtra (STORE_IDS_MESSAGE, storeIds);
				StartActivity (intent);
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
	}
}

