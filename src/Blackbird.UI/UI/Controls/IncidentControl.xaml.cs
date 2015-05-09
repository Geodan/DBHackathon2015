using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Blackbird.WPF.API;
using Blackbird.WPF.Utils;

namespace Blackbird.WPF.UI.Controls
{
    public partial class IncidentControl
    {
        private readonly WebRequests _webRequests;

        public IncidentControl()
        {
            InitializeComponent();
            _webRequests = new WebRequests();
        }

        private async void BtnGo_OnClick(object sender, RoutedEventArgs e)
        {
            ResultStack.Children.Clear();

            var geocodedResult = await _webRequests.Geocode(TbStreckennetz.Text, TbKmPunkte.Text);
            var spherical = Utils.Projection.SphericalMercator.FromLonLat(geocodedResult.Longitude, geocodedResult.Latitude);

            const int margin = 5;
            var start = new Mapsui.Geometries.Point(spherical.X - margin, spherical.Y - margin);
            var end = new Mapsui.Geometries.Point(spherical.X + margin, spherical.Y + margin);
            MapUtils.CurrentMapControl.ZoomToBox(start, end);

            var pinpointResults = await _webRequests.Pinpoint(geocodedResult.Longitude.ToString(), geocodedResult.Latitude.ToString());
            ResultGrid.Visibility = Visibility.Visible;

            var brush = (SolidColorBrush)FindResource("Brush01");

            ResultStack.Children.Add(new TextBlock { Text = string.Format("{0} incident on section {1} km {2}",
                ((ComboBoxItem)CbWhat.SelectedItem).Content,
                TbStreckennetz.Text,
                TbKmPunkte.Text),
                Margin = new Thickness(10),
                FontSize = 12
            });      

            ResultStack.Children.Add(new TextBlock { Text = "location", FontWeight = FontWeights.Bold, Foreground = brush });
            ResultStack.Children.Add(new TextBlock { Text = string.Format("Lon: {0} Lat: {1}", Math.Round(geocodedResult.Longitude, 4), Math.Round(geocodedResult.Latitude, 4)) });      

            ResultStack.Children.Add(new TextBlock { Text = "district", FontWeight = FontWeights.Bold, Foreground = brush });
            ResultStack.Children.Add(new TextBlock { Text = pinpointResults.District });            

            ImageTunnel.Visibility = pinpointResults.IsInTunnel ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
