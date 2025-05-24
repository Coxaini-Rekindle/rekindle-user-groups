﻿using Rekindle.UserGroups.Application.Storage.Models;

namespace Rekindle.UserGroups.Application.Storage.Interfaces;

public interface IFileStorage
{
    Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default);

    Task<FileResponse> GetAsync(Guid fileId, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default);
}