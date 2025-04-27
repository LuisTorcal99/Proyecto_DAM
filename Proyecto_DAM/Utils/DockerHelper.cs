using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_DAM.Utils
{
    public static class DockerHelper
    {
        public static async Task StartDockerAsync()
        {
            bool sqlRunning = await IsContainerRunningAsync("SQL_Server_LT");
            bool rabbitRunning = await IsContainerRunningAsync("rabbitmq_LT");

            if (sqlRunning && rabbitRunning)
            {
                Console.WriteLine("Docker: SQL Server y RabbitMQ ya están corriendo.");
                return;
            }

            Console.WriteLine("Docker: levantando contenedores...");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker-compose",
                    Arguments = "-f docker-compose.yml up -d",
                    WorkingDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..")),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            Console.WriteLine("Docker output: " + output);
            if (!string.IsNullOrEmpty(error))
                Console.WriteLine("Docker error: " + error);
        }

        private static async Task<bool> IsContainerRunningAsync(string containerName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"ps --filter \"name={containerName}\" --format \"{{{{.Names}}}}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            process.WaitForExit();

            return !string.IsNullOrWhiteSpace(output);
        }
    }
}