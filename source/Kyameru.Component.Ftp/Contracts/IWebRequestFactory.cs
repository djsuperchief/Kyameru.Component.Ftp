using Kyameru.Component.Ftp.Enums;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Kyameru.Component.Ftp.Tests")]

namespace Kyameru.Component.Ftp.Contracts
{
    internal interface IWebRequestFactory
    {
        FtpWebRequest GetFtpWebRequest(string path, FtpOperation method, Settings.FtpSettings settings, bool closeConnection = false);
    }
}