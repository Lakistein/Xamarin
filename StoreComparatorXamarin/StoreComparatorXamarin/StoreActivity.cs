
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
	[Activity (Label = "StoreActivity")]			
	public class StoreActivity : Activity
	{
		public const string ITEM_ID_MESSAGE = "com.lazarlazarevic.storecomparer.ITEM_ID_MESSAGE";
		public const string IS_ITEM_NEW_MESSAGE = "com.lazarlazarevic.storecomparer.IS_ITEM_NEW_MESSAGE";

		ListView listView;
		List<Item> items;
		ArrayAdapter<string> listAdapter;
		long storeId;
		List<string> s;
		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.activity_store);

			Intent i = Intent;
			//TextView t = (TextView) findViewById(R.id.textView);
			storeId = i.GetLongExtra (MainActivity.STORE_ID_MESSAGE, 0);
			//t.setText(Long.toString(storeId));

			listView = FindViewById<ListView> (Resource.Id.allItemsFromStoreListView);

			items = MainActivity.db.GetItemsFromStore (storeId);
			s = new List<string> (items.Select (x => x.Name));

			listAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, s);
			listView.Adapter = listAdapter;
			RegisterForContextMenu (listView);

			Button addNewItem = FindViewById<Button> (Resource.Id.btnAddItem);
			addNewItem.Click += (sender, e) => {
				Intent intent = new Intent(this, typeof(AddEditItemActivity));
				intent.PutExtra(MainActivity.STORE_ID_MESSAGE, storeId);
				intent.PutExtra(IS_ITEM_NEW_MESSAGE, true);
				StartActivity(intent);
			};
		}

		protected override void OnResume () {
			base.OnResume ();
			RefreshList ();
		}

		void RefreshList () {
			items = MainActivity.db.GetItemsFromStore (storeId);
			s.Clear ();
			s.AddRange(items.Select (x => x.Name));
			listAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, s);
			listView.Adapter = listAdapter;
			listAdapter.NotifyDataSetChanged ();
		}

		public override void OnCreateContextMenu (IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo) {
			base.OnCreateContextMenu (menu, v, menuInfo);
			if (v.Id == Resource.Id.allItemsFromStoreListView) {
				AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)menuInfo;
				menu.SetHeaderTitle (items [info.Position].Name);
				String[] menuItems = { "Edit", "Delete" };
				for (int i = 0; i < menuItems.Length; i++)
					menu.Add (Menu.None, i, i, menuItems [i]);
			}
		}

		public override bool OnContextItemSelected (IMenuItem item) {
			AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
			string[] menuItems = { "Edit", "Delete" };
			string menuItemName = menuItems [item.ItemId];
			string listItemName = items [info.Position].Name;

			if (menuItemName.Equals ("Edit")) {
				Intent intent = new Intent(this, typeof(AddEditItemActivity));
				intent.PutExtra(ITEM_ID_MESSAGE, items[info.Position].Id);
				intent.PutExtra(MainActivity.STORE_ID_MESSAGE, storeId);
				intent.PutExtra(IS_ITEM_NEW_MESSAGE, false);
				StartActivity(intent);
				return true;
			} else if (menuItemName.Equals ("Delete")) {
				MainActivity.db.DeleteItem (items[info.Position].Id);
				Toast.MakeText (this, "Item " + listItemName + " has been deleted!", ToastLength.Short).Show ();
				RefreshList ();
				return true;
			}
			return false;
		}
	}
}

