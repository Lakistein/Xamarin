using System;
using SQLite;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Database.Sqlite;
using Android.Database;

namespace StoreComparatorXamarin
{
	public class MySQLiteHelper : SQLiteOpenHelper
	{
		private const string DATABASE_NAME = "stores.db";
		private const int DATABASE_VERSION = 1;

		//Store table
		private const string TABLE_STORE = "stores";
		private const string COLUMN_STORE_ID = "_id";
		private const string COLUMN_STORE_NAME = "name";

		private readonly string[] ALL_STORE_COLUMNS = {COLUMN_STORE_ID, COLUMN_STORE_NAME};

		private const string DATABASE_CREATE_STORE = "create table " + TABLE_STORE + "(" + COLUMN_STORE_ID + " integer primary key autoincrement, " + COLUMN_STORE_NAME + " text not null);";


		//Item table
		private const string TABLE_ITEM = "items";
		private const string COLUMN_ITEM_ID = "_id";
		private const string COLUMN_ITEM_NAME = "name";
		private const string COLUMN_ITEM_CATEGORY = "category";
		private const string COLUMN_ITEM_PRICE = "price";
		private const string COLUMN_ITEM_WEIGHT = "weight";
		private const string COLUMN_ITEM_STORE_ID = "store_id";

		private readonly string[] ALL_ITEM_COLUMNS = {COLUMN_ITEM_ID, COLUMN_ITEM_NAME, COLUMN_ITEM_CATEGORY, COLUMN_ITEM_PRICE, COLUMN_ITEM_WEIGHT, COLUMN_ITEM_STORE_ID};

		private const string DATABASE_CREATE_ITEM = "create table " + TABLE_ITEM + "(" + COLUMN_ITEM_ID + " integer primary key autoincrement, " + COLUMN_ITEM_NAME + " text not null, " + COLUMN_ITEM_CATEGORY + " text, " + COLUMN_ITEM_PRICE + " real, " + COLUMN_ITEM_WEIGHT + " real, " + COLUMN_ITEM_STORE_ID + " integer, " + "FOREIGN KEY (" + COLUMN_ITEM_STORE_ID + ") REFERENCES " + TABLE_STORE + " (" + COLUMN_STORE_ID + ") ON DELETE CASCADE);";

		public MySQLiteHelper(Context context) : base(context, DATABASE_NAME, null, DATABASE_VERSION)
		{

		}

		public override void OnCreate (SQLiteDatabase db) {
			db.ExecSQL(DATABASE_CREATE_STORE);
			db.ExecSQL(DATABASE_CREATE_ITEM);
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_STORE);
			db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_ITEM);
			OnCreate(db);
		}

		public Store AddStore(String name)
		{
			SQLiteDatabase db = WritableDatabase;
			ContentValues values = new ContentValues();
			values.Put(COLUMN_STORE_NAME, name);
			long id = db.Insert(TABLE_STORE, null, values);
			ICursor cursor = db.Query(TABLE_STORE, ALL_STORE_COLUMNS, COLUMN_STORE_ID + " = " + id, null, null, null, null);
			cursor.MoveToFirst();
			Store store = CursorToStore(cursor);
			cursor.Close();
			db.Close();
			return store;
		}

		public List<Store> GetAllStores()
		{
			SQLiteDatabase db = WritableDatabase;
			List<Store> stores = new List<Store>();
			ICursor cursor = db.Query(TABLE_STORE, ALL_STORE_COLUMNS, null, null, null, null, null);

			cursor.MoveToFirst();

			while (!cursor.IsAfterLast)
			{
				Store s = CursorToStore(cursor);
				stores.Add(s);
				cursor.MoveToNext();
			}
			db.Close();
			cursor.Close();
			return stores;
		}

		public Store GetStore(long id)
		{
			SQLiteDatabase db = WritableDatabase;
			ICursor cursor = db.Query(TABLE_STORE, ALL_STORE_COLUMNS, COLUMN_STORE_ID + " = " + id, null, null, null, null);

			cursor.MoveToFirst();
			Store i = CursorToStore(cursor);
			db.Close();
			cursor.Close();
			return i;
		}

		public void DeleteStore(long id)
		{
			SQLiteDatabase db = WritableDatabase;
			db.ExecSQL("PRAGMA foreign_keys = ON;");

			db.Delete(TABLE_STORE, COLUMN_STORE_ID + " = " + id, null);
			db.Close();
		}

		private Store CursorToStore(ICursor c)
		{
			return new Store(c.GetLong(0), c.GetString(1));
		}

		public Item AddItem(String itemName, String category, float price, float weight, long storeId)
		{
			SQLiteDatabase db = WritableDatabase;
			ContentValues values = new ContentValues();

			values.Put(COLUMN_ITEM_NAME, itemName);
			values.Put(COLUMN_ITEM_CATEGORY, category);
			values.Put(COLUMN_ITEM_PRICE, price);
			values.Put(COLUMN_ITEM_WEIGHT, weight);
			values.Put(COLUMN_ITEM_STORE_ID, storeId);

			long id = db.Insert(TABLE_ITEM, null, values);
			ICursor c = db.Query(TABLE_ITEM, ALL_ITEM_COLUMNS, COLUMN_ITEM_ID + " = " + id, null, null, null, null);
			c.MoveToFirst();
			Item i = CursorToItem(c);
			db.Close();
			c.Close();
			return i;
		}

		public List<Item> GetAllItems()
		{
			SQLiteDatabase db = ReadableDatabase;
			List<Item> items = new List<Item>();
			ICursor cursor = db.Query(TABLE_ITEM, ALL_ITEM_COLUMNS, null, null, null, null, null);

			cursor.MoveToFirst();
			while (!cursor.IsAfterLast)
			{
				Item s = CursorToItem(cursor);
				items.Add(s);
				cursor.MoveToNext();
			}
			db.Close();
			cursor.Close();
			return items;
		}

		public List<Item> GetItemsFromStore(long id)
		{
			SQLiteDatabase db = ReadableDatabase;
			List<Item> items = new List<Item>();
			ICursor cursor = db.Query(TABLE_ITEM, ALL_ITEM_COLUMNS, COLUMN_ITEM_STORE_ID + " = " + id, null, null, null, null);

			cursor.MoveToFirst();
			while (!cursor.IsAfterLast)
			{
				Item s = CursorToItem(cursor);
				items.Add(s);
				cursor.MoveToNext();
			}
			db.Close();
			cursor.Close();
			return items;
		}

		public Item GetItemFromStoreWithName(long id, String name)
		{
			SQLiteDatabase db = WritableDatabase;
			String[] args = {id.ToString(), name};
			Item items = null;
			ICursor cursor = db.Query(TABLE_ITEM, ALL_ITEM_COLUMNS, COLUMN_ITEM_STORE_ID + " = ? AND " + COLUMN_ITEM_NAME + " = ?", args, null, null, null);
			if(cursor.Count > 0)
			{
				cursor.MoveToFirst();
				items = CursorToItem(cursor);
			}
			db.Close();
			cursor.Close();
			return items;
		}

		public Item GetItem(long itemId)
		{
			SQLiteDatabase db = WritableDatabase;
			ICursor cursor = db.Query(TABLE_ITEM, ALL_ITEM_COLUMNS, COLUMN_ITEM_ID + " = " + itemId, null, null, null, null);

			cursor.MoveToFirst();
			Item i = CursorToItem(cursor);
			db.Close();
			cursor.Close();
			return i;
		}

		public int UpdateItem(long itemId, String itemName, String category, float price, float weight)
		{
			SQLiteDatabase db = WritableDatabase;
			ContentValues values = new ContentValues();

			values.Put(COLUMN_ITEM_NAME, itemName);
			values.Put(COLUMN_ITEM_CATEGORY, category);
			values.Put(COLUMN_ITEM_PRICE, price);
			values.Put(COLUMN_ITEM_WEIGHT, weight);

			int i = db.Update(TABLE_ITEM, values, COLUMN_ITEM_ID + " = " + itemId, null);
			db.Close();

			return i;
		}

		public void DeleteItem(long id)
		{
			SQLiteDatabase db = WritableDatabase;

			db.Delete(TABLE_ITEM, COLUMN_ITEM_ID + " = " + id, null);
			db.Close();
		}

		private Item CursorToItem(ICursor c)
		{
			return new Item(c.GetLong(0), c.GetString(1), c.GetString(2), c.GetFloat(3), c.GetFloat(4), c.GetLong(5));
		}
	}
}

