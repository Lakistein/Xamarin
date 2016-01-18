
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
	[Activity (Label = "AddEditItemActivity")]			
	public class AddEditItemActivity : Activity
	{
		Item item;
		AutoCompleteTextView name, category;
		EditText price, weight;
		Intent intent;
		bool isNew = false;
		String oldName;

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_add_edit_item);

			intent = Intent;
			isNew = intent.GetBooleanExtra (StoreActivity.IS_ITEM_NEW_MESSAGE, false);
			List<Item> items = MainActivity.db.GetAllItems ();
			HashSet<string> hashNames = new HashSet<string> ();
			HashSet<string> hashCategories = new HashSet<string> ();

			foreach (Item item in items) {
				hashNames.Add (item.Name);
				hashCategories.Add (item.Category);
			}

			List<string> alNames = new List<string> (hashNames);
			List<string> alCategories = new List<string> (hashCategories);
			ArrayAdapter<string> adapterNames = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleDropDownItem1Line, alNames);
			ArrayAdapter<string> adapterCategories = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleDropDownItem1Line, alCategories);

			name = FindViewById <AutoCompleteTextView> (Resource.Id.txtName);
			category = FindViewById<AutoCompleteTextView> (Resource.Id.txtCategory);
			price = FindViewById<EditText> (Resource.Id.txtPrice);
			weight = FindViewById<EditText> (Resource.Id.txtWeight);

			name.Adapter = adapterNames;
			category.Adapter = adapterCategories;

			if (!isNew) {
				item = MainActivity.db.GetItem (intent.GetLongExtra (StoreActivity.ITEM_ID_MESSAGE, -1));
				name.Text = item.Name;
				category.Text = item.Category;
				price.Text = item.Price.ToString ();
				weight.Text = item.Weight.ToString ();
				oldName = item.Name;
			}

			Button btnDone = FindViewById<Button> (Resource.Id.btndone);
			btnDone.Click += (sender, e) => {
				if (name.Text.Trim ().Equals ("") || category.Text.Trim ().Equals ("") || price.Text.Trim ().Equals ("") || weight.Text.Trim ().Equals ("")) {
					Toast.MakeText (this, "All fields have to be filled", ToastLength.Short).Show ();
					return;
				}
				long lo = intent.GetLongExtra (MainActivity.STORE_ID_MESSAGE, -1);
				Item it = MainActivity.db.GetItemFromStoreWithName (lo, name.Text.ToUpper ());
				if (isNew && it != null) {
					Toast.MakeText (this, "Item " + name.Text + " already exists, use another name.", ToastLength.Short).Show ();
					return;
				} else if (!isNew && !name.Text.ToUpper ().Equals (oldName) && MainActivity.db.GetItemFromStoreWithName (intent.GetLongExtra (MainActivity.STORE_ID_MESSAGE, -1), name.Text.ToUpper ()) != null) {
					Toast.MakeText (this, "Item " + name.Text + " already exists, use another name.", ToastLength.Short).Show ();
					return;
				}

				if (isNew)
					AddNewItem ();
				else
					EditItem ();
				Finish ();
			};
		}

		void AddNewItem () {
			MainActivity.db.AddItem (name.Text.Trim ().ToUpper (),
				category.Text.Trim ().ToUpper (),
				float.Parse (price.Text),
				float.Parse (weight.Text),
				intent.GetLongExtra (MainActivity.STORE_ID_MESSAGE, -1));
			Toast.MakeText (this, "Item " + name.Text + " has been added!", ToastLength.Short).Show ();
		}

		void EditItem () {
			MainActivity.db.UpdateItem (item.Id,
				name.Text.ToUpper (),
				category.Text.ToUpper (),
				float.Parse (price.Text),
				float.Parse (weight.Text));
			
			Toast.MakeText (this, "Item " + name.Text + " has been edited!", ToastLength.Short).Show ();
		}
	}
}

