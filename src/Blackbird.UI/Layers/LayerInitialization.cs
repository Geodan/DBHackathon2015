using System.Collections.Generic;
using Mapsui.Layers;
using Mapsui.Providers.Wms;

namespace Blackbird.WPF.Layers
{
    public class LayerInitialization
    {
        public static List<ILayer> GetLayers()
        {
            var layers = new List<ILayer>();

            var tunnel = CreateWmsProvider("EPSG:3857", new List<string> { "tunnel" }, string.Format("{0}cite/wms?service=WMS&request=getcapabilities", Constants.Geoserver));
            var tunnelLayer = new ImageLayer("Tunnels") { DataSource = tunnel };
            layers.Add(tunnelLayer);

            var rails = CreateWmsProvider("EPSG:3857", new List<string> { "streckennetz" }, string.Format("{0}cite/wms?service=WMS&request=getcapabilities", Constants.Geoserver));
            var railsLayer = new ImageLayer("Rails") { DataSource = rails };
            layers.Add(railsLayer);

            var kmp = CreateWmsProvider("EPSG:3857", new List<string> { "kilometerpoints" }, string.Format("{0}cite/wms?service=WMS&request=getcapabilities", Constants.Geoserver));
            var kmpLayer = new ImageLayer("Kilometer points") { DataSource = kmp };
            layers.Add(kmpLayer);

            return layers;
        }

        private static WmsProvider CreateWmsProvider(string epsg, IEnumerable<string> layernames, string url)
        {
            var provider = new WmsProvider(url)
            {
                ContinueOnError = true,
                TimeOut = 20000,
                CRS = epsg
            };

            foreach (var layername in layernames)
            {
                provider.AddLayer(layername);
            }
            
            provider.SetImageFormat(provider.OutputFormats[0]);
            return provider;
        }
    }
}
