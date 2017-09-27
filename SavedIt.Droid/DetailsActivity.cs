using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using SavedIt.Core.Models;
using Environment = System.Environment;

namespace SavedIt.Droid
{
    [Activity]
    public class DetailsActivity : ListActivity
    {
        private SavedItemContext _db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _db = new SavedItemContext(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            ListAdapter = new SavedItemsAdapter(this, _db.SavedItems.ToList());
        }
    }

    public class SavedItemsAdapter : BaseAdapter<SavedItem>
    {
        private readonly DetailsActivity _context;
        private readonly List<SavedItem> _items;

        public SavedItemsAdapter(DetailsActivity context, List<SavedItem> items)
        {
            _context = context;
            _items = items;
        }

        public override long GetItemId(int position)
        {
            return _items[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this[position];
            var view = convertView ?? _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Description;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = $"{item.Price:C} ({item.Date:g})";
            return view;
        }

        public override int Count => _items.Count;

        public override SavedItem this[int position] => _items[position];

    }
}