using ELA.Tools.DatabaseMigration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ELA.App.StartupConfiguration
{
    public class LocalDevelopmentTasks
    {
        internal static void MigrateDatabase(IConfiguration configuration)
        {
            LocalDatabaseMigrator.Execute(configuration.GetConnectionString("Database"));
        }

        internal static void StartParcelServer(int port, string workingDirectory)
        {
            var exeToRun = "yarn";
            //var completeArguments = $"run start --port {port} --https";
            var completeArguments = $"run start --port {port}";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // On Windows, the node executable is a .cmd file, so it can't be executed
                // directly (except with UseShellExecute=true, but that's no good, because
                // it prevents capturing stdio). So we need to invoke it via "cmd /c".
                completeArguments = $"/c {exeToRun} {completeArguments}";
                exeToRun = "cmd";
            }

            var processStartInfo = new ProcessStartInfo(exeToRun)
            {
                Arguments = completeArguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                WorkingDirectory = workingDirectory
            };
            var process = Process.Start(processStartInfo);
            process.EnableRaisingEvents = true;
            
        }
    }
}
