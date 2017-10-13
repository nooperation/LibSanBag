using LibSanBag;
using SanBag.ResourceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SanBag.Models
{
    class TextureResource
    {
        private static BitmapImage _blankPreview = new BitmapImage();

        public static BitmapImage ExtractImage(
            FileRecord record,
            Stream inStream,
            int width = 0,
            int height = 0,
            LibDDS.ConversionOptions.DXGI_FORMAT format = LibDDS.ConversionOptions.DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT)
        {
            using (var outStream = new MemoryStream())
            {
                record.Save(inStream, outStream);
                var ddsBytes = OodleLz.DecompressResource(outStream);
                if (ddsBytes[0] == 'D' && ddsBytes[1] == 'D' && ddsBytes[2] == 'S')
                {
                    var imageData = LibDDS.GetImageBytesFromDds(ddsBytes, width, height, format);

                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(imageData);
                    image.EndInit();

                    return image;
                }
            }

            return _blankPreview;
        }

        public static BitmapImage ExtractImage(
            FileRecord record,
            string bagPath,
            int width = 0,
            int height = 0,
            LibDDS.ConversionOptions.DXGI_FORMAT format = LibDDS.ConversionOptions.DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT)
        {
            using (var inStream = File.OpenRead(bagPath))
            {
                return ExtractImage(record, inStream, width, height, format);
            }
        }

        public static byte[] ExtractDds(FileRecord record, Stream inStream)
        {
            using (var outStream = new MemoryStream())
            {
                record.Save(inStream, outStream);
                return OodleLz.DecompressResource(outStream);
            }
        }
    }
}
