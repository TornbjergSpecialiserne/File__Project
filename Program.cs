using FileProject;

namespace FileProject;

Console.WriteLine("Download PDF's");

int threadDevider = 1000;

try
{
    ExcelManager excelManager = new ExcelManager();
    HTTP_Manager httpManager = new HTTP_Manager();

    int rowCount = excelManager.GetNumberOfGRI_Rows();
    int numberOfThreads = rowCount / threadDevider;

    var collectedData = new List<URL_Data> ();

    //Deviede rows into chunks
    List<(int, int)> rowChunks = new List<(int, int)> ();
    for (int i = 0; i < numberOfThreads; i++)
    {
        int endValue = (i + 1) * threadDevider;
        if(endValue > rowCount)
            endValue = rowCount;

        rowChunks.Add((i * threadDevider + 1, endValue));
    }

    Console.WriteLine("Find links to data");

    //Devide tasks
    var tasks = new List<Task<URL_Data>>();
    for (int i = 0; i < rowChunks.Count; i++)
    {
        tasks.Add(excelManager.ReadMultipulRowsWithLinks(ExcelManager.GRI_dataPath, rowChunks[i].Item1, rowChunks[i].Item2));
    }

    //Wait for all rows to be read
    var taskResult = await Task.WhenAll(tasks);

    Console.WriteLine("Download data");

    //Collect results
    foreach (var item in taskResult)
    {
        collectedData.AddRange(item);
    }

    // Find bad results
    for (global::System.Int32 i = 0; i < collectedData.Count; i++)
    {
        if (collectedData[i].BR_Nummer == string.Empty)
            collectedData[i].validLink = false;
        else if(collectedData[i].URL == string.Empty)
            collectedData[i].validLink = false;
    }

    // Download
    // Devide into task
    // Can use rowChunks again because the lenght of the list should be the same lenght.
    List<Task> downloadTasks = new List<Task>();
    for (int i = 0; i < rowChunks.Count; i++)
    {
        //downloadTasks.Add(httpManager.ProxyDownload(collectedData), rowChunks[i].Item1, rowChunks[i].Item2);
    }

    await Task.WhenAll(downloadTasks);

    Console.WriteLine("Give rapport");

    //Write 
    // Devide into task
    

}
catch (Exception e)
{
    Console.WriteLine(e);
}



Console.WriteLine("Jobs done. Press any key to end...");
Console.ReadKey();

