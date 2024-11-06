using FileProject;


Console.WriteLine("Download PDF's");

int threadDevider = 100000;

try
{
    ExcelManager excelManager = new ExcelManager();


    int startrow = 2;
    int rowCount = excelManager.GetNumberOfGRI_Rows();
    int numberOfThreads = rowCount / threadDevider;

    List<(string, string)> collectedData = new List<(string, string)> ();


    var tasks = new List<Task<List<(string, string)>>>();

    tasks.Add(excelManager.ReadMultipulRowsWithLinks(ExcelManager.GRI_dataPath, startrow, 10));
    tasks.Add(excelManager.ReadMultipulRowsWithLinks(ExcelManager.GRI_dataPath, 11, 20));


    var taskResult = await Task.WhenAll(tasks);

    Console.WriteLine(taskResult);

    foreach (var item in taskResult)
    {
        Console.WriteLine(item);
    }

    //Write result
    foreach (var item in collectedData)
    {
        Console.WriteLine(item);
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}



Console.WriteLine("Jobs done. Press any key to end...");
Console.ReadKey();

