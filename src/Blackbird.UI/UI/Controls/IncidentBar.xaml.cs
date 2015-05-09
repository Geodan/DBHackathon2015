using System;
using System.Timers;
using System.Windows;

namespace Blackbird.WPF.UI.Controls
{
    public partial class IncidentBar
    {
        private readonly Timer _timer;        
        private DateTime _incidentStart;

        public IncidentBar()
        {
            InitializeComponent();
            _timer = new Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    var elapsed = DateTime.Now - _incidentStart;
                    TxtMin0.Text = Math.Abs(((elapsed.Minutes % 10) - elapsed.Minutes) / 10).ToString();
                    TxtMin1.Text = (elapsed.Minutes % 10).ToString();
                    TxtSecond0.Text = Math.Abs(((elapsed.Seconds % 10) - elapsed.Seconds) / 10).ToString();
                    TxtSecond1.Text = (elapsed.Seconds % 10).ToString();
                });
            }
            catch (Exception)
            {

            }
        }

        private void BtnNewIncident_OnClick(object sender, RoutedEventArgs e)
        {
            CreateNewIncident();
        }

        private void CreateNewIncident()
        {
            var newIncident = new IncidentControl();

            IncidentStack.Children.Clear();
            IncidentStack.Children.Add(newIncident);
            
            TimerGrid.Visibility = Visibility.Visible;
            _incidentStart = DateTime.Now;
            _timer.Start();
        }
    }
}
