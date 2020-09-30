using Windows.UI;
using Windows.UI.Xaml.Media;

namespace GameBarWidget.Utilities
{
    public class ColorUtil
    {
        public static SolidColorBrush Brushify(Color? c)
        {
            return c == null ? new SolidColorBrush() : new SolidColorBrush(c.Value);
        }
    }
}