using System;
using System.IO;
using Android.App;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Content;
using ZXing.Common;
using ZXing;

namespace BarcodeGen
{
    [Activity(Label = "BarcodeGen", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();

            ImageView barcode = FindViewById<ImageView>(Resource.Id.barcode);
            EditText UsrIn = FindViewById<EditText>(Resource.Id.UsrIn);
            Button translateButton = FindViewById<Button>(Resource.Id.GenerateButton);

            String lastcode = prefs.GetString("last_code", "DEFAULT");

            if (!String.IsNullOrEmpty(lastcode))
            {
                DrawCode(lastcode, barcode);
            }

            translateButton.Click += (object sender, EventArgs e) =>
            {
                if (!String.IsNullOrEmpty(UsrIn.Text))
                {
                    DrawCode(UsrIn.Text, barcode);
                    editor.PutString("last_code", UsrIn.Text);
                    editor.Apply();
                }
            };
        }

        static void DrawCode(string code, ImageView barcode)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_39,
                Options = new EncodingOptions
                {
                    Height = 200,
                    Width = 600
                }
            };
            var bitmap = writer.Write(code);
            Drawable img = new BitmapDrawable(bitmap);
            barcode.SetImageDrawable(img);
        }
    }
}