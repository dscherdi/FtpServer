// <copyright file="DotNetFileSystemEntry.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.IO;

using FubarDev.FtpServer.FileSystem.Generic;

using JetBrains.Annotations;

namespace FubarDev.FtpServer.FileSystem.DotNet
{
    public abstract class DotNetFileSystemEntry : IUnixFileSystemEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetFileSystemEntry"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system this entry belongs to.</param>
        /// <param name="fsInfo">The <see cref="FileSystemInfo"/> to extract the information from.</param>
        protected DotNetFileSystemEntry([NotNull] DotNetFileSystem fileSystem, [NotNull] FileSystemInfo fsInfo)
        {
            FileSystem = fileSystem;
            Info = fsInfo;
            LastWriteTime = new DateTimeOffset(Info.LastWriteTime);
            CreatedTime = new DateTimeOffset(Info.CreationTimeUtc);
            var accessMode = new GenericAccessMode(true, true, true);
            Permissions = new GenericUnixPermissions(accessMode, accessMode, accessMode);
        }

        /// <summary>
        /// Gets the underlying <see cref="DirectoryInfo"/>.
        /// </summary>
        public FileSystemInfo Info { get; }

        /// <inheritdoc/>
        public string Name => Info.Name;

        /// <inheritdoc/>
        public IUnixPermissions Permissions { get; }

        /// <inheritdoc/>
        public DateTimeOffset? LastWriteTime { get; }

        /// <inheritdoc/>
        public DateTimeOffset? CreatedTime { get; }

        /// <inheritdoc/>
        public long NumberOfLinks => 1;

        /// <inheritdoc/>
        public IUnixFileSystem FileSystem { get; }

        /// <inheritdoc/>
        public string Owner => "owner";

        /// <inheritdoc/>
        public string Group => "group";
    }
}
