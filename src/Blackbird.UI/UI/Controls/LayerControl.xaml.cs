using System.Windows;
using Mapsui.Layers;

namespace Blackbird.WPF.UI.Controls
{
    public partial class LayerControl
    {
        private readonly bool _initializing;
        public ILayer Layer { get; private set; }

        public LayerControl(ILayer layer)
        {
            _initializing = true;

            InitializeComponent();
            Layer = layer;            
            SetupGui();

            CbOnOff.IsChecked = Layer.Enabled;
            _initializing = false;
        }

        private void SetupGui()
        {
            TxtLayerName.Text = Layer.Name;
        }

        private void CbOnOff_OnChecked(object sender, RoutedEventArgs e)
        {
            if(_initializing)
                return;

            Layer.Enabled = true;
        }

        private void CbOnOff_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (_initializing)
                return;

            Layer.Enabled = false;
        }
    }
}
