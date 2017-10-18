//using Microsoft.Office.Interop.Excel;
//using System;
//using System.Drawing;
//using System.IO;
//using System.Threading;

//namespace CommonClassFiles
//{
//    public class //Txt2XLS
//    {
//        #region Methods

//    public static void ConvertMyTxtToExcel(string InputfileName, string xlsFileName)
//    {

//        // workbook.Windows[1].Zoom = 80;

//        object misValue = System.Reflection.Missing.Value;
//        var excellApp = new Application();
//        var excellWorkBook = excellApp.Workbooks.Add(misValue);
//        var excellWorkSheet = (Worksheet)excellWorkBook.Worksheets.Item[1];
//        excellApp.DisplayAlerts = false;

//        // Read the file as one string.
//        StreamReader myFile = new StreamReader(InputfileName);
//        string myFileContents = myFile.ReadToEnd();
//        myFile.Close();
//        string a1ColorBg; //Green=0,255,0  Red=255,0,0  Amber=255,153,0

//        if (myFileContents.ToUpper().Contains("PASS") && !myFileContents.ToUpper().Contains("FAIL") &&
//            !myFileContents.ToUpper().Contains("WARNING"))
//        {
//            a1ColorBg = "255, 255, 255"; // was green
//        }
//        else if (!myFileContents.ToUpper().Contains("FAIL") && myFileContents.ToUpper().Contains("WARNING"))
//        {
//            a1ColorBg = "255,255,255"; // was amber
//        }
//        else
//        {
//            a1ColorBg = "255,255,255"; // was red
//        }

//        AddDataFromFile(excellWorkSheet, InputfileName, a1ColorBg);
//        excellWorkBook.Windows[1].Zoom = 80;
//        Safe(misValue, excellWorkBook, xlsFileName);
//        CloseQuitAndRelease(excellApp, excellWorkSheet, excellWorkBook, misValue);


//        // Move the Txt file to TextResults
//        FileInfo fi = new FileInfo(InputfileName);
//        var directoryOfFile = fi.DirectoryName;
//        var fileNameOnly = fi.Name;
//        var archiveTextResultsFolder = directoryOfFile + "\\TextResults\\";

//        bool dirExists = Directory.Exists(archiveTextResultsFolder);
//        if (!dirExists)
//            Directory.CreateDirectory(archiveTextResultsFolder);
//        Thread.Sleep(5000);
//        var random = GenerateRandomString.GenerateString(4, 0);
//        File.Move(InputfileName, directoryOfFile + "\\TextResults\\" + random + fileNameOnly);
//    }



//    /// <summary>
//    /// The add data from file.
//    /// </summary>
//    /// <param name="excellWorkSheet" />
//    /// The excell work sheet. 
//    /// 
//    /// <param name="fileName" />
//    /// The file name. 
//    /// 

//    public static void AddDataFromFile(_Worksheet excellWorkSheet, string fileName, string a1ColorBg)
//    {
//        if (excellWorkSheet == null)
//        {
//            throw new ArgumentNullException("excellWorkSheet");
//        }

//        var lines = File.ReadAllLines(fileName);
//        var rowCounter = 1;
//        foreach (var line in lines)
//        {
//            var columnCounter = 1;
//            var values = line.Split('\n');
//            foreach (var value in values)
//            {
//                excellWorkSheet.Cells[rowCounter, columnCounter] = value;
//                columnCounter++;
//            }

//            rowCounter++;
//        }
//        ColorXls(excellWorkSheet, fileName, a1ColorBg);
//    }

//    public static void ColorXls(_Worksheet excellWorkSheet, string fileName, string a1ColorBg)
//    {
//        a1ColorBg = a1ColorBg.Replace(" ", "");
//        int r = Convert.ToInt32(a1ColorBg.Split(',')[0]);
//        int g = Convert.ToInt32(a1ColorBg.Split(',')[1]);
//        int b = Convert.ToInt32(a1ColorBg.Split(',')[2]);
//        Range newcolumnAllWhiteReports;
//        Range colorLines;

//        Range range = excellWorkSheet.UsedRange;
//        int rows_count = range.Rows.Count;
//        string output = null;

//        for (int i = 1; i <= rows_count; i++)
//        {
//            if (excellWorkSheet.Cells[i, 1].value == null)
//            {
//                output = "";
//            }
//            else
//            {
//                output = String.Format("[{0}] ", excellWorkSheet.Cells[i, 1].value);
//            }


//            if (output.Contains("Pass"))
//            {
//                colorLines = excellWorkSheet.get_Range("a" + i);
//                colorLines.Interior.Color = Color.FromArgb(185, 255, 185).ToArgb();
//            }
//            else if (output.Contains("Fail"))
//            {
//                colorLines = excellWorkSheet.get_Range("a" + i);
//                colorLines.Interior.Color = Color.FromArgb(100, 100, 255).ToArgb(); // B,G,R
//            }
//            else if (output.ToUpper().Contains("EXPECTED SCENARIO:"))
//            {
//                colorLines = excellWorkSheet.get_Range("a" + i);
//                colorLines.Interior.Color = Color.FromArgb(17, 238, 221).ToArgb();
//            }
//            else if (output.ToUpper().Contains("TEST CASE ID:"))
//            {
//                colorLines = excellWorkSheet.get_Range("a" + i);
//                colorLines.Interior.Color = Color.FromArgb(0, 0, 0).ToArgb();
//                colorLines.Font.Color = ColorTranslator.ToOle(Color.White);
//                colorLines.Font.Bold = true;
//            }
//            else if (output.ToUpper().Contains("LEGEND:"))
//            {
//                colorLines = excellWorkSheet.get_Range("a" + i);
//                colorLines.Interior.Color = Color.FromArgb(0, 0, 0).ToArgb();
//                colorLines.Font.Color = ColorTranslator.ToOle(Color.White);
//                colorLines.Font.Bold = true;
//            }
//            else if (output.ToUpper().Contains("WARNING"))
//            {
//                colorLines = excellWorkSheet.get_Range("a" + i);
//                colorLines.Interior.Color = ColorTranslator.ToOle(Color.Orange);  //Color.FromArgb(255, 255, 255).ToArgb();
//                colorLines.Font.Color = ColorTranslator.ToOle(Color.White);
//                colorLines.Font.Bold = true;
//            }
//            else
//            {
//                colorLines = excellWorkSheet.get_Range("a" + i);
//                colorLines.Interior.Color = Color.FromArgb(255, 255, 255).ToArgb();
//                colorLines.Font.Bold = true;
//            }
//        }

//        newcolumnAllWhiteReports = excellWorkSheet.get_Range("a2");
//        newcolumnAllWhiteReports.Font.Bold = true;
//        newcolumnAllWhiteReports.Interior.Color = Color.FromArgb(b, g, r).ToArgb();
//        newcolumnAllWhiteReports.EntireColumn.AutoFit();

//    }

//    /// <summary>
//    /// The close quit and release.
//    /// </summary>
//    /// <param name="excellApp" />
//    /// The excell app. 
//    /// 
//    /// <param name="excellWorkSheet" />
//    /// The excell work sheet. 
//    /// 
//    /// <param name="excellWorkBook" />
//    /// The excell work book. 
//    /// 
//    /// <param name="misValue" />
//    /// The mis value. 
//    /// 
//    public static void CloseQuitAndRelease(
//        Application excellApp, Worksheet excellWorkSheet, Workbook excellWorkBook, object misValue)
//    {
//        excellWorkBook.Close(true, misValue, misValue);
//        excellApp.Quit();
//        ReleaseObject(excellApp);
//        ReleaseObject(excellWorkBook);
//        ReleaseObject(excellWorkSheet);
//    }

//    /// <summary>
//    /// The release object.
//    /// </summary>
//    /// <param name="obj" />
//    /// The obj. 
//    /// 
//    public static void ReleaseObject(object obj)
//    {
//        try
//        {
//            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("Unable to release the Object " + ex);
//        }
//        finally
//        {
//            GC.Collect();
//        }
//    }

//    /// <summary>
//    /// The safe.
//    /// </summary>
//    /// <param name="misValue" />
//    /// The mis value. 
//    /// 
//    /// <param name="excellWorkBook" />
//    /// The excell work book. 
//    /// 
//    /// <param name="excellfilename" />The name of the excell file 
//    public static void Safe(object misValue, Workbook excellWorkBook, string excellfilename)
//    {
//        excellWorkBook.SaveAs(
//            excellfilename,
//            XlFileFormat.xlWorkbookNormal,
//            misValue,
//            misValue,
//            misValue,
//            misValue,
//            XlSaveAsAccessMode.xlExclusive,
//            2,
//            misValue,
//            misValue,
//            misValue,
//            misValue);
//    }



//    #endregion
//}
//}
