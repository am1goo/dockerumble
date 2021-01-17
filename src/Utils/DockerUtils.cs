using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace dockerumble
{
    public static class DockerUtils
    {
        public struct DockerBuildResult 
        {
            public int exitCode;
            public string consoleOutput;
            public string consoleError;

            public DockerBuildResult(int exitCode, string consoleOutput, string consoleError)
            {
                this.exitCode = exitCode;
                this.consoleOutput = consoleOutput;
                this.consoleError = consoleError;
            }
        }

        public static DockerBuildResult Build(string dockerImage, string dockerfileText)
        {
            FileInfo fi = new FileInfo("Dockerfile");
            if (fi.Exists)
                fi.Delete();

            using (FileStream fs = fi.Open(FileMode.OpenOrCreate))
            {
                using (StreamWriter wr = new StreamWriter(fs))
                {
                    wr.Write(dockerfileText);
                }
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "docker";
            startInfo.Arguments = $"build -t {dockerImage} --rm .";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            string outputData = string.Empty;
            string errorData = string.Empty;

            Process proc = new Process();
            proc.StartInfo = startInfo;
            proc.OutputDataReceived += (s, e) =>
            {
                if (outputData.Length > 0)
                    outputData += Environment.NewLine;
                outputData += e.Data;

                Console2.WriteLine(e.Data, ConsoleColor.Gray);
            };
            proc.ErrorDataReceived += (s, e) =>
            {
                if (errorData.Length > 0)
                    errorData += Environment.NewLine;
                errorData += e.Data;

                Console2.WriteLine(e.Data, ConsoleColor.Gray);
            };
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();// Waits here for the process to exit.

            int exitCode = proc.ExitCode;

            proc.CancelOutputRead();
            proc.CancelErrorRead();
            proc.Dispose();           

            if (fi.Exists)
                fi.Delete();

            return new DockerBuildResult(exitCode, outputData, errorData);
        }

        private static readonly StringBuilder sb = new StringBuilder();
        public static string Labels(string repo, string repocommit)
        {
            sb.Length = 0;
            sb.AppendLine($"LABEL maintainer=\"{ProgramArgs.ApplicationName}\"");
            sb.AppendLine($"LABEL maintainer.email=\"{ProgramArgs.ApplicationEmail}\"");
            sb.AppendLine($"LABEL maintainer.version=\"{ProgramArgs.ApplicationVersion}\"");
            sb.AppendLine($"LABEL repo=\"{repo}\"");
            sb.AppendLine($"LABEL repo.commit=\"{repocommit}\"");
            return sb.ToString();
        }
    }
}
