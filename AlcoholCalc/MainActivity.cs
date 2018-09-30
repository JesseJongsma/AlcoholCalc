using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Content;

namespace AlcoholCalc
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private double BAC = 0;
        private double Gender = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            // initialize spinners
            Spinner spinnerBac = FindViewById<Spinner>(Resource.Id.spinnerBAC);
            Spinner spinnerGender = FindViewById<Spinner>(Resource.Id.spinnerGender);

            initializeSpinner(spinnerBac, Resource.Array.BACArray);
            initializeSpinner(spinnerGender, Resource.Array.genderArray);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // ButtonClick
            Button button = FindViewById<Button>(Resource.Id.buttonCalc);
            EditText weightText = FindViewById<EditText>(Resource.Id.weightInput);
            TextView result = FindViewById<TextView>(Resource.Id.textViewResult);
            double weight;

            button.Click += delegate
            {
                if (BAC != 0 && Gender != 0)
                {
                    double.TryParse(weightText.Text, out weight);
                    InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                    inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                    double calculation = (BAC / 100 * ((weight * 1000) * Gender)) / 15;
                    calculation = Math.Ceiling(calculation);
                    result.Text = string.Format("{0} beers required", calculation.ToString());
                }
            };
        }

        private void initializeSpinner(Spinner spinner, int resource)
        {
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            var adapter = ArrayAdapter.CreateFromResource(
                    this, resource, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            Object item = spinner.GetItemAtPosition(e.Position);
            string toast = string.Format("The value is {0}", item);


            if (item.ToString() == "Male" || item.ToString() == "Female")
            {
                if (item.ToString() == "Male")
                    Gender = 0.68;
                else
                    Gender = 0.55;
            }
            else
            {
                string BACInput = item.ToString();
                string[] BACArray = BACInput.Split(' ');
                double BACAverage = (Convert.ToDouble(BACArray[0]) + Convert.ToDouble(BACArray[2])) / 2;
                BAC = BACAverage;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}

