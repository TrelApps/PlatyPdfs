﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PlatyPdfs.App.Core.Models;

namespace PlatyPdfs.App.Core.Contracts.Services;

public interface IPdfFileService
{
    Task<IEnumerable<PdfFile>> GetGridDataAsync();
}
