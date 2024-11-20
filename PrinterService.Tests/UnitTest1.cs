namespace PrinterService.Tests;

using Xunit;

public class UnitTest1
{
    public UnitTest1()
    {
        UnitTestInit.Init();
    }

    [Fact]
    public void JPGFile_Success()
    {
        var imgPath = @"C:\Users\Lee\Pictures\ca3ff65d950289916b356e598814a86.jpg";
        imgPath.Print();
    }

    [Fact]
    public void DocxFile_Success()
    {
        var imgPath = Path.Combine(AppContext.BaseDirectory, "Files", "1.docx");
        imgPath.Print();
    }
}
