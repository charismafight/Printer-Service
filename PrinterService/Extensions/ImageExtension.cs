
using System.Drawing;
using System.Drawing.Printing;

internal static class StringExtension
{
    // #if windows
    static StreamReader streamToPrint;
    public static void PrintImage(this string imagePath, bool vertical = true)
    {
        if (!Directory.Exists(imagePath))
        {
            return;
        }

        lock (streamToPrint)
        {
            streamToPrint = new StreamReader(imagePath);
            var pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            // Specify the printer to use.
            // pd.PrinterSettings.PrinterName = printer;
            pd.DefaultPageSettings.Landscape = vertical;  //设置横向打印，不设置默认是纵向的
            if (pd.PrinterSettings.IsValid)
            {
                pd.Print();
            }
        }
    }

    static void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
        float linesPerPage = 0;
        float yPos = 0;
        int count = 0;
        float leftMargin = ev.MarginBounds.Left;
        float topMargin = ev.MarginBounds.Top;
        string line = null;
        var printFont = new Font("Arial", 10);

        // Calculate the number of lines per page.
        linesPerPage = ev.MarginBounds.Height /
           printFont.GetHeight(ev.Graphics);

        // Print each line of the file.
        while (count < linesPerPage &&
           ((line = streamToPrint.ReadLine()) != null))
        {
            yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
            ev.Graphics.DrawString(line, printFont, Brushes.Black,
               leftMargin, yPos, new StringFormat());
            count++;
        }

        // If more lines exist, print another page.
        ev.HasMorePages = line != null;
    }

    // #endif
}
