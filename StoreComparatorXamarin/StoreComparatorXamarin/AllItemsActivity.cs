
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
	[Activity (Label = "AllItemsActivity")]			
	public class AllItemsActivity : Activity
	{
		List<Item> allitems = new List<Item> ();
		ListView listView;
		ArrayAdapter<String> la;

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_all_items);

			listView = (ListView)FindViewById (Resource.Id.allItemsListView);

			allitems = MainActivity.db.GetAllItems ();

			List<string> l = new List<string> ();

			foreach (Item item in allitems) {
				l.Add (item.Name);
			}

			la = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, l);
			listView.Adapter = la;
		}
	}
}

