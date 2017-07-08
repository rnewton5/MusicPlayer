using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MusicPlayer.Core.Utilities
{
    public static class Converters
    {
        public static string TimeSpan_toString(TimeSpan time)
        {
            string duration = (new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds)).ToString().TrimStart(new char[] { '0', ':' });
            if (duration.Length == 1)
            {
                duration = "0:0" + duration;
            }
            else if (duration.Length == 2)
            {
                duration = "0:" + duration;
            }
            duration = duration.Replace('.', ':');
            return duration;
        }

        public static BitmapSource Image_toBitmapSource(Image myImage)
        {
            var bitmap = new Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr value);
    }
}
