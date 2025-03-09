
using PlatyPdfs.App.Core.Contracts.Services;
using PlatyPdfs.App.Core.Models;

namespace PlatyPdfs.App.Core.Services;

public class PdfFileService : IPdfFileService
{
    private List<PdfFile> _allpdfs;

    public IEnumerable<PdfFile> AllPdfs()
    {

        return Enumerable.Empty<PdfFile>();

    }

    public async Task<IEnumerable<PdfFile>> GetGridDataAsync()
    {
        _allpdfs ??= new List<PdfFile>(AllPdfs());

        await Task.CompletedTask;
        return _allpdfs;
    }
}

