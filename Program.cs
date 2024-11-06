using FileProject;


Console.WriteLine("Download PDF's");

int threadDevider = 1000;

try
{
    ExcelManager excelManager = new ExcelManager();

    int rowCount = excelManager.GetNumberOfGRI_Rows();
    int numberOfThreads = rowCount / threadDevider;

    var collectedData = new List<(string, string)> ();

    //Deviede rows into chunks
    List<(int, int)> rowChunks = new List<(int, int)> ();
    for (int i = 0; i < numberOfThreads; i++)
    {
        int endValue = (i + 1) * threadDevider;
        if(endValue > rowCount)
            endValue = rowCount;

        rowChunks.Add((i * threadDevider + 1, endValue));
    }

    //Devide tasks
    var tasks = new List<Task<List<(string, string)>>>();
    for (int i = 0; i < rowChunks.Count; i++)
    {
        tasks.Add(excelManager.ReadMultipulRowsWithLinks(ExcelManager.GRI_dataPath, rowChunks[i].Item1, rowChunks[i].Item2));
    }

    //Wait for all rows to be read
    var taskResult = await Task.WhenAll(tasks);

    //Collect results
    foreach (var item in taskResult)
    {
        collectedData.AddRange(item);
    }


}
catch (Exception e)
{
    Console.WriteLine(e);
}



Console.WriteLine("Jobs done. Press any key to end...");
Console.ReadKey();

