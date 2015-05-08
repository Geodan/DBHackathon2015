using System;
using System.Windows.Forms;
using System.Windows.Threading;
using BlackBird.WPF.Utils;
using Mapsui.Geometries;
using Mapsui.UI.Xaml;
using Mapsui.Utilities;
using Timer = System.Threading.Timer;

namespace BlackBird.WPF.BlackBirdSystem
{
    public sealed class BlackBirdSystem
    {
        private readonly MainWindow _parent;
        private readonly MapControl _mapcontrol;
        private readonly LayerManager _layerManager;

        public BlackBirdSystem(MainWindow parent, BlackBirdSystemInfoObject infoObject)
        {
            _parent = parent;           
            _mapcontrol = infoObject.MapControl;

            MapUtils.CurrentMapControl = _mapcontrol;
            _layerManager = new LayerManager { LayerCollection = _mapcontrol.Map.Layers };

            _mapcontrol.TouchDown += MapcontrolTouchDown;
            _mapcontrol.TouchMove += MapcontrolTouchMove;
            _mapcontrol.TouchUp += MapcontrolTouchUp;
            _mapcontrol.MouseLeftButtonDown += MapcontrolMouseLeftButtonDown;
            _mapcontrol.MouseLeftButtonUp += MapcontrolMouseLeftButtonUp;
        }

        public LayerManager LayerHelper
        {
            get { return _layerManager; }
        }

        private System.Windows.Point _mouseDownPosition;
        private Timer _doubleClickTimer;
        private int _clickCount;

        private void TimerProc(object state)
        {
            if (_clickCount >= 2)
            {
                MapUtils.CurrentMapControl.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                {
                    ZoomTo(new Point(_mouseDownPosition.X, _mouseDownPosition.Y));
                    MapUtils.CurrentMapControl.OnViewChanged();
                }));
            }

            _doubleClickTimer = null;
            _clickCount = 0;
        }

        private void ZoomTo(Point location)
        {
            _mapcontrol.Viewport.Center = MapUtils.ConvertScreenToWorld(location.X, location.Y);
            _mapcontrol.Viewport.Resolution = ZoomHelper.ZoomIn(_mapcontrol.Map.Resolutions, _mapcontrol.Viewport.Resolution);
            _mapcontrol.Refresh();
        }

        private void MapcontrolMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_mouseDownPosition.X == e.GetPosition(_mapcontrol).X && _mouseDownPosition.Y == e.GetPosition(_mapcontrol).Y)
                MapUtils.OnClick(e);
        }

        private void MapcontrolMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mouseDownPosition = e.GetPosition(_mapcontrol);
            _clickCount++;

            if (_doubleClickTimer != null)
                return;

            _doubleClickTimer = new Timer(TimerProc);
            _doubleClickTimer.Change(SystemInformation.DoubleClickTime, 0);
        }

        private static void MapcontrolTouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            MapUtils.OnTouchUp(e);
        }

        private static void MapcontrolTouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            MapUtils.OnTouchMoved(e);
        }

        private static void MapcontrolTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            MapUtils.OnTouchDown(e);
        }
    }
}
