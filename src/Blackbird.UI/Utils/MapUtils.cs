using BlackBird.WPF.Logging;
using Mapsui.Geometries;
using Mapsui.UI.Xaml;

namespace BlackBird.WPF.Utils
{
    public delegate void MapControlTappedEventHandler(object sender, Point location);
    public delegate void MapControlTouchEventHandler(System.Windows.Input.TouchEventArgs touchEvent);
    public delegate void MapControlClickedEventHandler(System.Windows.Input.MouseButtonEventArgs e);

    public static class MapUtils
    {
        public static MapControl CurrentMapControl { get; set; }
        public static event MapControlTappedEventHandler MapTapped;
        public static event MapControlTouchEventHandler TouchDown;
        public static event MapControlTouchEventHandler TouchUp;
        public static event MapControlTouchEventHandler TouchMoved;
        public static event MapControlClickedEventHandler Click;

        public static void OnClick(System.Windows.Input.MouseButtonEventArgs e)
        {
            var handler = Click;
            if (handler != null) handler(e);
        }

        public static void OnMapTapped(Point location)
        {
            var handler = MapTapped;
            if (handler != null) handler(null, location);
        }

        public static void OnTouchDown(System.Windows.Input.TouchEventArgs touchEvent)
        {
            var handler = TouchDown;
            if (handler != null) handler(touchEvent);
        }

        public static void OnTouchMoved(System.Windows.Input.TouchEventArgs touchEvent)
        {
            var handler = TouchMoved;
            if (handler != null) handler(touchEvent);
        }

        public static void OnTouchUp(System.Windows.Input.TouchEventArgs touchEvent)
        {
            var handler = TouchUp;
            if (handler != null) handler(touchEvent);
        }

        public static Point ConvertScreenToWorld(double x, double y)
        {
            Point convertedPoint = null;
            if (CurrentMapControl != null)
            {
                convertedPoint = CurrentMapControl.Viewport.ScreenToWorld(x, y);
            }
            
            return convertedPoint;
        }

        public static Point ConvertScreenToWorld(Point sharpmapPoint)
        {
            return ConvertScreenToWorld(sharpmapPoint.X, sharpmapPoint.Y);
        }

        public static Point ConvertScreenToWorld(System.Windows.Point windowsPoint)
        {
            return ConvertScreenToWorld(windowsPoint.X, windowsPoint.Y);
        }

        public static Polygon ConvertScreenToWorld(Polygon sharpmapPolygon)
        {
            var linearRing = new LinearRing();
            foreach (var vertex in sharpmapPolygon.ExteriorRing.Vertices)
            {
                linearRing.Vertices.Add(ConvertScreenToWorld(vertex));
            }
            
            return new Polygon(linearRing);
        }

        public static Point ConvertWorldToScreen(double x, double y)
        {
            Log4netLogger.DebugFormat("Convert map point {0}, {1} to screen coördinates", x, y);

            Point convertedPoint = null;
            if (CurrentMapControl != null)
            {
                convertedPoint = CurrentMapControl.Viewport.WorldToScreen(x, y);
            }

            Log4netLogger.DebugFormat("Converted point: {0}, {1}", convertedPoint.X, convertedPoint.Y);
            return convertedPoint;
        }

        public static Point ConvertWorldToScreen(Point sharpmapPoint)
        {
            return ConvertWorldToScreen(sharpmapPoint.X, sharpmapPoint.Y);
        }

        public static Point ConvertWorldToScreen(System.Windows.Point windowsPoint)
        {
            return ConvertWorldToScreen(windowsPoint.X, windowsPoint.Y);
        }

        public static void RefreshMap()
        {
            if (CurrentMapControl != null)
            {
                CurrentMapControl.Refresh();
            }
        }
    }
}
