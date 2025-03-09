namespace PlatyPdfs.App.Core.Models;

public class PdfFile
{
    public int OrderNumber
    {
        get;
        set;
    }

    public string FileName
    {
        get;
        set;
    }

    public string FilePath
    {
        get;
        set;
    }

    public string FileSize
    {
        get;
        set;
    }

    public string DateModified
    {
        get;
        set;
    }
}
