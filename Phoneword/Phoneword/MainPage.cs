using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Phoneword
{
    class MainPage : ContentPage
    {
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translatedNumber;
        public MainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);

            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };

            panel.Children.Add(new Label
            {
                Text = "Ingresa una telepalabra:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });

            panel.Children.Add(phoneNumberText = new Entry
            {
                Text = "1-800-LALOTUPAPA",
            });

            panel.Children.Add(translateButton = new Button
            {
                Text = "Traducir"
            });

            panel.Children.Add(callButton = new Button
            {
                Text = "Llamar",
                IsEnabled = false,
            });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Phoneword.PhonewordTranslator.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Llamar " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Pailas";
            }
        }

        async void OnCall(object sender, EventArgs e)
        {
            if (await this.DisplayAlert(
                "Marcar un número",
                "¿Quieres llamar a " + translatedNumber + "?",
                "Si",
                "No"
                ))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("No puedo llamar", "El numero de teléfono no es válido", "Ok");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("No puedo llamar", "Este teléfono no puede hacer llamadas", "Ok");
                }
                catch (Exception)
                {
                    await DisplayAlert("No puedo llamar", "El discado de teléfono falló", "Ok");
                }
            }
        }

    }
}
