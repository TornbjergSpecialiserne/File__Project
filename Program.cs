using FileProject;


Console.WriteLine("Download PDF's");

int threadDevider = 100000;

try
{
    ExcelManager excelManager = new ExcelManager();


    int foundFile = 0;

    int rowCount = excelManager.GetNumberOfGRI_Rows();
    int numberOfThreads = rowCount / threadDevider;

    List<(string, string)> collectedData = new List<(string, string)> ();
    Task[] tasks = new Task[]
    {
        excelManager.ReadMultipulRowsWithLinks(ExcelManager.GRI_dataPath, 0, 1000),
        excelManager.ReadMultipulRowsWithLinks(ExcelManager.GRI_dataPath, 1001, 2000)
    };    

    foreach (var task in tasks)
    {
        task.Start();
    }

    Task.WaitAll(tasks);

    for (global::System.Int32 i = 0; i < tasks.Length; i++)
    {
        
    }

    //int targetRows = 10;
    ////Read [targetRows] row with data
    //for (int i = 2; i < 2 + targetRows; i++)
    //{
    //    List<string> cells = excelManager.ReadGRIRow(i);

    //    //No links in cells
    //    if (cells.Count <= 1)
    //        continue;

    //    Console.Write($"{i} links: ");
    //    foreach (string item in cells)
    //    {
    //        Console.Write(item + ", ");
    //    }
    //    Console.WriteLine();

    //    //HTTP
    //    HTTP_Manager fileClient = new HTTP_Manager();
    //    //Download
    //    //bool downloadResult = await fileClient.DownloadFileAsync(cells[0], cells[1]);

    //    //if(downloadResult)
    //    //    foundFile++;
    //}

    //Console.WriteLine($"{foundFile} / {targetRows}");
}
catch(Exception e)
{
    Console.WriteLine(e);
}



Console.WriteLine("Jobs done. Press any key to end...");
Console.ReadKey();

