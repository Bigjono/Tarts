using System.Drawing;
using System.Drawing.Text;

namespace Bronson.Utils
{
        public class Barcode
        {
   
            public Bitmap CreateBarcode(string data, Color backgroundColour, Color fontColour)
            {
                /*
                 * for three of nine to work, it needs to be pre-append / appended with a *
                 * and the letters need to be in uppercase.
                 * http://www.squaregear.net/fonts/free3of9.shtml
                 */
                data = string.Format("*{0}*", data.ToUpper());
                ;

                var barCode = new Bitmap(1, 1);

                var threeOfNine = new Font("Free 3 of 9", 60,
                    FontStyle.Regular, GraphicsUnit.Point);

                var graphics = Graphics.FromImage(barCode);

                var dataSize = graphics.MeasureString(data, threeOfNine);

                barCode = new Bitmap(barCode, dataSize.ToSize());

                graphics = Graphics.FromImage(barCode);

                graphics.Clear(backgroundColour);

                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

                graphics.DrawString(data, threeOfNine,
                    new SolidBrush(fontColour), 0, 0);

                graphics.Flush();

                threeOfNine.Dispose();
                graphics.Dispose();

                return barCode;
            }
            
            /// <summary>
            /// Generate a Code39 barcode image with your the string provided. you will need the correct font installed.
            /// http://www.squaregear.net/fonts/free3of9.shtml
            /// </summary>
            /// <param name="data">data to generate barcode</param>
            /// <returns></returns>
            public Bitmap CreateBarcode(string data)
            {
                return CreateBarcode(data, Color.White, Color.Black);
            }
        }
    

}