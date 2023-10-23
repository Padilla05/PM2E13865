using Plugin.Media;
using Plugin.Media.Abstractions;
using PM2E13865.Models;
using PM2E13865.Views;
using PM2E13865.Models;
using PM2E13865.Views;
using PM2E13865;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E13865
{

    public partial class MainPage : ContentPage
    {

        MediaFile FotoFile = null;
        public MainPage()
        {
            InitializeComponent();
            if (App.DBaseSitios == null) Debug.WriteLine("Creando base de datos");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            getLatitudeLongitude();

        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {

            if (FotoFile == null)
            {
                Message("Aviso", "Aun no se a tomado una foto: Presione la imagen de ejemplo para capturar una imagen");
                return;
            }


            if (string.IsNullOrEmpty(txtLatitud.Text) || string.IsNullOrEmpty(txtLongitud.Text))
            {
                Message("Aviso", "Aun no se obtiene la ubicacion");
                getLatitudeLongitude();
                return;
            }


            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                Message("Aviso", "Debe escribir una breve descripcion, Obligatorio");
                return;
            }


            try
            {
                var sitio = new Sitios()
                {
                    Id = 0,
                    latitud = double.Parse(txtLatitud.Text),
                    longitud = double.Parse(txtLongitud.Text),
                    descripcion = txtDescription.Text,
                    image = ConvertImageToByteArray(),
                    pathImage = FotoFile.Path
                };

                var result = await App.DBaseSitios.insertUpdateSitio(sitio);

                if (result > 0)
                {
                    Message("Aviso Exito", "Sitio agregado correctamente");
                    clearComp();
                }
                else
                {
                    Message("Aviso Error", "No se pudo agregar el sitio");
                }

            }
            catch (Exception ex)
            {

                Message("Error: ", ex.Message);
            }

        }



        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    FotoFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "MisUbicaciones",
                        AllowCropping = true,
                        CustomPhotoSize = 30,
                        CompressionQuality = 30

                    });


                    if (FotoFile != null)
                    {
                        imgFoto.Source = ImageSource.FromStream(() => {
                            return FotoFile.GetStream();
                        });
                    }


                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }

            }
            else
            {
                await Permissions.RequestAsync<Permissions.Camera>();
            }

        }

        private async void getLatitudeLongitude()
        {
            try
            {

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();


                if (status == PermissionStatus.Granted)
                {

                    var localizacion = await Geolocation.GetLocationAsync();

                    txtLatitud.Text = Math.Round(localizacion.Latitude, 5) + "";
                    txtLongitud.Text = Math.Round(localizacion.Longitude, 5) + "";

                }
                else
                {

                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
            }
            catch (Exception e)
            {



                if (e.Message.Equals("Location services are not enabled on device."))
                {

                    Message("Error", "Servicio de localizacion no encendido");
                }
                else
                {
                    Message("Error", e.Message);

                }

            }
        }


        #region Metodos Utiles

        private Byte[] ConvertImageToByteArray()
        {
            if (FotoFile != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FotoFile.GetStream();

                    stream.CopyTo(memory);

                    return memory.ToArray();
                }
            }

            return null;
        }

        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }

        private void clearComp()
        {
            imgFoto.Source = "ParisMuestra.png";
            txtDescription.Text = "";
            FotoFile = null;
            getLatitudeLongitude();
        }

        #endregion Metodos Utiles

        private async void btnList_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageList());
        }
    }
}
