using System;
using BlackBird.WPF.BlackBirdSystem;
using BlackBird.WPF.Logging;
using BruTile.Web;
using Mapsui.Layers;

namespace BlackBird.WPF
{
    public partial class MainWindow
    {
        private readonly BlackBirdSystem.BlackBirdSystem _blackBirdSystem;

        public MainWindow()
        {
            InitializeComponent();
            Log4netLogger.Debug("Starting program");

            var blackBirdSystemInfoObject = new BlackBirdSystemInfoObject
            {
                MapControl = MapControl
            };

            _blackBirdSystem = new BlackBirdSystem.BlackBirdSystem(this, blackBirdSystemInfoObject);

            AddOsm();
        }

        private void AddOsm()
        {
            var osm = new TileLayer(new OsmTileSource()) { Name = "osm", Tag = Guid.Parse("7D1897F4-6D45-4FBA-919B-F39A7E8B8938").ToString() };
            _blackBirdSystem.LayerHelper.AddBackgroundLayer(osm);
        }
    }
}
