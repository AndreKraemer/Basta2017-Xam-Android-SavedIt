using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using SavedIt.Core.Models;
using Environment = System.Environment;

namespace SavedIt.Droid
{
    [Activity(Label = "SavedIt.Droid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private EditText _descriptionText;
        private EditText _priceText;
        private TextView _totalSavedText;
        private Button _saveButton;
        private Button _detailsButton;

        private SavedItemContext _db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _descriptionText = FindViewById<EditText>(Resource.Id.descriptionText);
            _priceText = FindViewById<EditText>(Resource.Id.priceText);
            _totalSavedText = FindViewById<TextView>(Resource.Id.totalSaved);
            _saveButton = FindViewById<Button>(Resource.Id.saveButton);
            _detailsButton = FindViewById<Button>(Resource.Id.detailsButton);

            _saveButton.Click += OnSave;
            _detailsButton.Click += NavigateToDetails;

            _db = new SavedItemContext(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            _db.Database.EnsureCreated();
            UpdateTotalLabel();
        }

        private void NavigateToDetails(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(DetailsActivity));
            StartActivity(intent);
        }

        private void OnSave(object sender, System.EventArgs e)
        {
            if (decimal.TryParse(_priceText.Text, out decimal price))
            {
                var item = new SavedItem
                {
                    Date = DateTime.Now,
                    Description = _descriptionText.Text,
                    Price = price
                };
                _db.SavedItems.Add(item);
                _db.SaveChanges();
                HideKeyboard();
                UpdateTotalLabel();

                _descriptionText.Text = string.Empty;
                _priceText.Text = string.Empty;
            }
        }

        private void UpdateTotalLabel()
        {
            _totalSavedText.Text = _db.SavedItems.Sum(x => x.Price).ToString("C");
        }

        private void HideKeyboard()
        {
            var imm = (InputMethodManager)
                GetSystemService(InputMethodService);
            if (imm.IsAcceptingText)
            {
                imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            }
        }
    }
}

