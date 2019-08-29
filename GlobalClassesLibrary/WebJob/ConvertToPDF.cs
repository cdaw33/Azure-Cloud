using System.IO;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;

namespace GlobalClassesLibrary.WebJob
{
    public class ConvertToPDF
    {
        public static void ConvertDocToPDF(Stream input, Stream output)
        {
            //This code has been apdated from the Spire.PDF website
            //Creates a streamreader in order to read the data held in the input stream
            StreamReader reader = new StreamReader(input);
            //Reads all the file
            string doc2 = reader.ReadToEnd();
            //Creates a new instance of a Spire.PDF document and adds section pages and formatting to it
            PdfDocument doc = new PdfDocument();
            PdfSection section = doc.Sections.Add();
            PdfPageBase page = section.Pages.Add();
            PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 11);
            PdfStringFormat format = new PdfStringFormat();
            format.LineSpacing = 20f;
            PdfBrush brush = PdfBrushes.Black;
            PdfTextWidget textWidget = new PdfTextWidget(doc2, font, brush);
            float y = 0;
            PdfTextLayout textLayout = new PdfTextLayout();
            textLayout.Break = PdfLayoutBreakType.FitPage;
            textLayout.Layout = PdfLayoutType.Paginate;
            RectangleF bounds = new RectangleF(new PointF(0, y), page.Canvas.ClientSize);
            textWidget.StringFormat = format;
            textWidget.Draw(page, bounds, textLayout);
            //Saves the file as a stream using the output stream variable and converts it to a PDF
            doc.SaveToStream(output, FileFormat.PDF);

        }
    }
}
