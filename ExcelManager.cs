using OfficeOpenXml;

namespace FileProject
{
    public class ExcelManager
    {
        //File names
        public const string GRI_dataPath = "GRI_2017_2020.xlsx";
        public const string metaDataPath = "Metadata2006_2016.xlsx";

        public ExcelManager()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //Find XL dokuments with Links
            try
            {
                //Check for files
                if (!File.Exists(GRI_dataPath) || !File.Exists(metaDataPath))
                {
                    Console.WriteLine("XL files not found");
                    Console.ReadKey();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("XL not found");
                Console.ReadKey();
            }
        }

        public int GetNumberOfGRI_Rows()
        {
            using (var package = new ExcelPackage(GRI_dataPath))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                return worksheet.Rows.Count();
            }
        }

        public List<string> ReadMetaRow(int rowIndex) => ReadRowFromExcel(metaDataPath, rowIndex);

        public List<string> ReadGRIRow(int rowIndex) => ReadRowFromExcel(GRI_dataPath, rowIndex);


        public List<string> ReadRowFromExcel(string filePath, int rowIndex)
        {
            using (var package = new ExcelPackage(filePath))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                //Get cells
                List<string> values = new List<string>();
                //Cell A
                values.Add(worksheet.Cells[rowIndex, 1].Value?.ToString());
                //Cell AL
                ExcelRange al = worksheet.Cells[rowIndex, 38];
                if (al.Value != null && al.Value.ToString() != "")
                    values.Add(al.Value.ToString());
                else
                {
                    //Cell AM
                    ExcelRange am = worksheet.Cells[rowIndex, 39];
                    if (am.Value != null && am.Value.ToString() != "")
                        values.Add(am.Value.ToString());
                }

                return values;
            }
        }

        public async Task<List<(string, string)>> ReadMultipulRowsWithLinks(string filePath, int startRow, int endRow)
        {
            if(startRow >= endRow)
            {
                Console.WriteLine("Reading mul link where start is bigger then end");
                return null;
            }

            using (var package = new ExcelPackage(filePath))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                List<(string, string)> values = new List<(string, string)>();
                
                for (int i = startRow; i < endRow; i++)
                {
                    //Get cells         
                    //Cell A
                    string cellA = "";
                    cellA = worksheet.Cells[i, 1].Value?.ToString();

                    //Find Link
                    string link = "";
                    //Cell AL
                    ExcelRange al = worksheet.Cells[i, 38];
                    if (al.Value != null && al.Value.ToString() != "")
                        link = al.Value.ToString();
                    else
                    {
                        //Cell AM
                        ExcelRange am = worksheet.Cells[i, 39];
                        if (am.Value != null && am.Value.ToString() != "")
                            link = am.Value.ToString();
                    }

                    values.Add((cellA, link));
                }
                return values;
            }
        }

        public void WriteDownloadResult(bool resultState, int row)
        {
            using (var package = new ExcelPackage(metaDataPath))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                ExcelRange ATcell = worksheet.Cells[row, 46];

                if (resultState)
                    ATcell.Value = "Downloaded";
                else
                    ATcell.Value = "Not downloaded";
            }
        }
    }
}
