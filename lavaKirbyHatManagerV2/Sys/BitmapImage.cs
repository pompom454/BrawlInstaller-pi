// man so much shimming
using Avalonia.Media.Imaging;
using System.IO;

namespace System.Windows.Media.Imaging
{
    public class BitmapImage
    {
        private Bitmap _bitmap;

        public BitmapImage()
        {
        }

        public BitmapImage(Stream stream)
        {
            _bitmap = new Bitmap(stream);
        }

        public BitmapImage(string path)
        {
            _bitmap = new Bitmap(path);
        }

        public Bitmap InnerBitmap => _bitmap;
    }
}
