using System;
using System.Security.Policy;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Blackbird.WPF.API;
using Blackbird.WPF.Messaging;
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

            var msg = string.Format("{0} incident on section {1} km {2}",
                ((ComboBoxItem) CbWhat.SelectedItem).Content,
                TbStreckennetz.Text,
                TbKmPunkte.Text);

            ResultStack.Children.Add(new TextBlock
            {
                Text = msg,
                Margin = new Thickness(10),
                FontSize = 12
            });      

            ResultStack.Children.Add(new TextBlock { Text = "location", FontWeight = FontWeights.Bold, Foreground = brush });
            ResultStack.Children.Add(new TextBlock { Text = string.Format("Lon: {0} Lat: {1}", Math.Round(geocodedResult.Longitude, 4), Math.Round(geocodedResult.Latitude, 4)) });      

            ResultStack.Children.Add(new TextBlock { Text = "district", FontWeight = FontWeights.Bold, Foreground = brush });
            ResultStack.Children.Add(new TextBlock { Text = pinpointResults.District });            

            ImageTunnel.Visibility = pinpointResults.IsInTunnel ? Visibility.Visible : Visibility.Collapsed;

            //Tasks
            var tasks = pinpointResults.GetTasks();
            if (tasks != null)
            {
                TaskGrid.Visibility = Visibility.Visible;
                foreach (var task in tasks)
                {
                    TaskStack.Children.Add(new TextBlock
                    {
                        Text = task.TaskName,
                        FontWeight = FontWeights.Bold,
                        Foreground = brush,
                        Margin = new Thickness(0, 5, 0, 0)
                    });
                    TaskStack.Children.Add(new TextBlock {Text = task.Who});

                    var number = new TextBlock {Text = task.Number};
                    number.TextDecorations.Add(TextDecorations.Underline);
                    number.Tag = string.Format("{0};{1};{2}", 
                        "Blackbird", 
                        task.Number,
                        msg + string.Format(" https://www.google.de/maps/@{0},{1},19z",
                        Math.Round(geocodedResult.Latitude, 4),
                        Math.Round(geocodedResult.Longitude, 4)));
                    number.MouseLeftButtonDown += NumberMouseLeftButtonDown;
                    TaskStack.Children.Add(number);
                }
            }
        }

        private async void NumberMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var stringMessage = ((TextBlock) sender).Tag.ToString().Split(';');            
            var result = await SmsMessenger.SendMessage(stringMessage[0], stringMessage[1], stringMessage[2]);

            MessageBox.Show("SMS SEND");
        }
    }
}
