namespace FileProject
{
    public class HTTP_Manager
    {
        private readonly HttpClient client = new();
        private string downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");


        

        public async Task<bool> DownloadFileAsync(string fileName, string url)
        {
            try
            {
                // Get the Downloads folder path
                string downloadsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads");

                // Generate a unique filename
                string uniqueFileName = Path.Combine(downloadsPath, $"temp_{Guid.NewGuid()}.pdf");

                // Download the file
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        using (var fileStream = new FileStream(uniqueFileName, FileMode.Create))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }

                        // Rename the file
                        File.Move(uniqueFileName, Path.Combine(downloadsPath, fileName + ".pdf"));
                    }
                    else
                        return false;

                    //Tell download status
                    return true;
                }


            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
                Console.WriteLine($"Current working directory: {Environment.CurrentDirectory}");
                Console.WriteLine($"Current working directory: {Environment.CurrentDirectory}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            return false;
        }

        public async Task<string> GetAsync(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://example.com");
                    response.EnsureSuccessStatusCode(); // Throws if not successful

                    int statusCode = (int)response.StatusCode;
                    return $"Status Code: {(int)statusCode}";
                }
            }
            catch (HttpRequestException e)
            {
                return ($"An error occurred: {e.Message}");
            }
        }

#if(DEBUG)
        public async Task ProxyDownload(int startRow, int endRow)
        {
            //Set random some of the download to be false
            Random r = new Random(22);

            for (int i = startRow; i < endRow; i++)
            {
                //75% chance for link be good
                int randomNumber = r.Next(100);
                //if (randomNumber > 75)
                //    item.Item3 = false;

                await Task.Delay(100);
            }
        }
#endif
    }
}
