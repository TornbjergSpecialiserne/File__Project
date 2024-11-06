using System;
using FileProject;

namespace FileProject;

public class URL_Data
{
    public string BR_Nummer;
    public string URL;
    public bool validLink;

    public URL_Data(string BR_Nummer, string URL, bool validLink)
    {
        this.BR_Nummer = BR_Nummer;
        this.URL = URL;
        this.validLink = validLink;
    }
}
