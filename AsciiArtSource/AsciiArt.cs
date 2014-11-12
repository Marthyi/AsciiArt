namespace AsciiArt
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Text;

    public static class AsciiArt
    {
        private const string DefaultFontname = "Microsoft Sans Serif";
        private const char DefaultSeparator = '=';
        private const int DefaultSize = 25;

        public static void Write(string text, int size = DefaultSize, string fontName = DefaultFontname)
        {
            Console.WriteLine(ToString(text,size,fontName));
        }

        public static string ToString(string text, int fontSize = DefaultSize, string fontName = DefaultFontname)
        {
            using (Bitmap img = ConvertTextToPicture(text, fontSize, fontName))
            {
                return ConvertPictureToText(2, 2, img);
            }
        }

        public static void WriteLineSeparator(char separator = DefaultSeparator)
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write(separator);
            }

            Console.WriteLine();
        }

        private static string ConvertValueToPixelArt(int brightnessRatio)
        {
            string[] pixelEquivalent = { "#", // < 10  0
                                         "@", // < 17  1
                                         "&", // < 24  2
                                         "$", // < 31  3
                                         "%", // < 38  4
                                         "|", // < 45  5
                                         "!", // < 52  6
                                         ";", // < 59  7
                                         ":", // < 66  8
                                         "'", // < 73  9
                                         "`", // < 80  10
                                         ".", // < 87  11
                                         " "  // >87   12
                                       };
            int index = 0;
            
            if (brightnessRatio >= 10)
            {
                index = brightnessRatio - 10;
                index -= (index % 7);
                index /= 7;
                if (index >= 12)
                {
                    index = 12;
                }
            }

            return pixelEquivalent[index];
        }

        private static int GetBritghnessForPixelArtArea(int x0, int y0, int width, int heigt, Bitmap image)
        {
            int brightness = 0;

            for (int x = x0; x < x0 + width; x++)
            {
                for (int y = y0; y < y0 + heigt; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    brightness += (int)(pixelColor.GetBrightness() * 100);
                }
            }

            return brightness;
        }

        private static string GetPixelArtValueArea(int x0, int y0, int width, int height, Bitmap image)
        {
            return ConvertValueToPixelArt(GetBritghnessForPixelArtArea(x0, y0, width, height, image));
        }

        private static string ConvertPictureToText(int pixelArtHeight, int pixelArtWidth, Bitmap image)
        {
            var sb = new StringBuilder();

            for (int y = 0; y < image.Height - pixelArtHeight; y += pixelArtHeight)
            {
                var row = new StringBuilder();
                for (int x = 0; x < image.Width - pixelArtWidth; x += pixelArtWidth)
                {
                    row.Append(GetPixelArtValueArea(x, y, pixelArtWidth, pixelArtHeight, image));
                }

                if (!string.IsNullOrWhiteSpace(row.ToString()))
                {
                    sb.AppendLine(row.ToString());
                }
            }

            return sb.ToString();
        }

        private static Bitmap ConvertTextToPicture(string text, int fontSize, string fontName = DefaultFontname)
        {
            var font = new Font(fontName, fontSize, FontStyle.Bold);
            var fontColor = new SolidBrush(Color.Black);

            int imageHeight = (int)(fontSize * 1.5);

            var bmp = new Bitmap(text.Length * fontSize, imageHeight);

            using (Graphics graphicImage = Graphics.FromImage(bmp))
            {
                graphicImage.FillRectangle(Brushes.White, 0, 0, text.Length * fontSize, imageHeight);
                graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
                graphicImage.DrawString(text, font, fontColor, 0, 0);
                graphicImage.Save();
            }

            return bmp;
        }
    }
}
