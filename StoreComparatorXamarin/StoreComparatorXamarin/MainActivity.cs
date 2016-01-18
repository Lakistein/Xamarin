using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace StoreComparatorXamarin
{
	[Activity (Label = "StoreComparatorXamarin", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, ListView.IOnItemClickListener
	{
		public const string STORE_ID_MESSAGE = "com.lazarlazarevic.storecomparer.STORE_ID_MESSAGE";
		public static MySQLiteHelper db;

		List<Store> stores = new List<Store> ();
		ListView listView;
		ArrayAdapter<string> la;
		EditText editText;
		List<string> s;
		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.MainActivity);

			db = new MySQLiteHelper (ApplicationContext);
			stores = db.GetAllStores ();

			editText = (EditText)FindViewById (Resource.Id.txt_store_name);
			listView = (ListView)FindViewById (Resource.Id.storeListView);

			s = new List<string> (stores.Select (x => x.Name));

			la = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, s);
			listView.Adapter = la;
			listView.OnItemClickListener = this;
			RegisterForContextMenu (listView);

			//Buttons
			Button addStore = FindViewById<Button> (Resource.Id.btn_addStore);
			addStore.Click += (sender, e) => {
				if (editText.Text.Trim ().Length == 0) {
					Toast.MakeText (ApplicationContext, Resource.String.txt_empty_store_name, ToastLength.Short).Show ();
					return;
				}

				stores.Add (db.AddStore (editText.Text));
				RefreshList ();
				Toast.MakeText (ApplicationContext, "Store " + editText.Text + " has been added!", ToastLength.Short).Show ();
				editText.Text = string.Empty;
			};

			Button showAllItems = FindViewById<Button> (Resource.Id.button3);
			showAllItems.Click += (sender, e) => {
				StartActivity(typeof(AllItemsActivity));
			};

			Button compareStores = FindViewById<Button> (Resource.Id.btnCompareStores);
			compareStores.Click += (sender, e) => {
				if (stores.Count < 2) {
					Toast.MakeText (this, "You have to have at least two stores.", ToastLength.Short).Show ();
					return;
				}
				StartActivity (typeof(SelectStoresActivity));
			};
		}

		public void OnItemClick (AdapterView parent, View view, int position, long id) {
			OpenStore (stores.ElementAt (position).Id);
		}

		public override void OnCreateContextMenu (IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo) {
			base.OnCreateContextMenu (menu, v, menuInfo);

			if (v.Id == Resource.Id.storeListView) {
				AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)menuInfo;
				menu.SetHeaderTitle (stores [info.Position].Name);
				menu.Add (Menu.None, 0, 0, "Delete");
			}
		}

		public override bool OnContextItemSelected (IMenuItem item) {

			AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;

			if (item.ItemId == 0) {
				Toast.MakeText (this, "Store " + stores [info.Position].Name + " has been deleted!", ToastLength.Short).Show ();
				db.DeleteStore (stores [info.Position].Id);
				stores.Remove (stores [info.Position]);
				RefreshList ();
				return true;
			}
			return false;
		}

		private void OpenStore (long storeId) {
			Intent intent = new Intent (this, typeof(StoreActivity));
			intent.PutExtra (STORE_ID_MESSAGE, storeId);
			StartActivity (intent);
		}

		void RefreshList () {
			s.Clear ();
			s.AddRange(stores.Select (x => x.Name));
			la = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, s);
			listView.Adapter = la;
			la.NotifyDataSetChanged ();
		}
	}
}