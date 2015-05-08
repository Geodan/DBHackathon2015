using Blackbird.WPF.Utils;

namespace Blackbird.WPF.UI.Controls
{
    public partial class LayerManagerControl
    {
        private LayerManager _layerManager;

        public LayerManagerControl()
        {
            InitializeComponent();
        }

        public LayerManager LayerManager
        {
            get { return _layerManager; }
            set
            {
                _layerManager = value;
                LayerManager.BackgroundLayerAdded += LayerManagerBackgroundLayerAdded;
                LayerManager.ContextLayerAdded += LayerManagerContextLayerAdded;
                LayerManager.LayerRemoved += LayerManagerLayerRemoved;
            }            
        }

        private void LayerManagerLayerRemoved(Mapsui.Layers.ILayer layer)
        {
            foreach (var child in LayerStack.Children)
            {
                if (!(child is LayerControl) || ((LayerControl)child).Layer != layer)
                    continue;

                LayerStack.Children.Remove(child as LayerControl);
                break;
            }
        }

        private void LayerManagerContextLayerAdded(Mapsui.Layers.ILayer layer, bool removableByUser)
        {
            LayerStack.Children.Add(new LayerControl(layer));
        }

        private void LayerManagerBackgroundLayerAdded(Mapsui.Layers.ILayer layer, bool removableByUser)
        {
            LayerStack.Children.Add(new LayerControl(layer));
        }        
    }
}
