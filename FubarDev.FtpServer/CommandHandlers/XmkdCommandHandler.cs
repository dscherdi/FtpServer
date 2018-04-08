﻿//-----------------------------------------------------------------------
// <copyright file="XmkdCommandHandler.cs" company="Fubar Development Junker">
//     Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>
// <author>Mark Junker</author>
//-----------------------------------------------------------------------

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FubarDev.FtpServer.FileSystem;

namespace FubarDev.FtpServer.CommandHandlers
{
    /// <summary>
    /// Implements the <code>XMKD</code> command.
    /// </summary>
    public class XmkdCommandHandler : FtpCommandHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmkdCommandHandler"/> class.
        /// </summary>
        /// <param name="connection">The connection to create this command handler for</param>
        public XmkdCommandHandler(IFtpConnection connection)
            : base(connection, "XMKD")
        {
        }

        /// <inheritdoc/>
        public override async Task<FtpResponse> Process(FtpCommand command, CancellationToken cancellationToken)
        {
            var directoryName = command.Argument;
            var currentPath = Data.Path.Clone();
            var dirInfo = await Data.FileSystem.SearchDirectoryAsync(currentPath, directoryName, cancellationToken);
            if (dirInfo == null)
                return new FtpResponse(550, "Not a valid directory.");
            if (dirInfo.FileName == null)
                return new FtpResponse(550, "ROOT folder not allowed.");
            if (dirInfo.Entry != null)
            {
                await Connection.WriteAsync($"521-\"{currentPath.GetFullPath(dirInfo.FileName)}\" directory already exists", cancellationToken);
                return new FtpResponse(521, "Taking no action.");
            }

            try
            {
                var targetDirectory = currentPath.Count == 0 ? Data.FileSystem.Root : currentPath.Peek();
                var newDirectory = await Data.FileSystem.CreateDirectoryAsync(targetDirectory, dirInfo.FileName, cancellationToken);
                return new FtpResponse(257, $"\"{currentPath.GetFullPath(newDirectory.Name)}\" created.");
            }
            catch (IOException)
            {
                return new FtpResponse(550, "Bad pathname syntax.");
            }
        }
    }
}
