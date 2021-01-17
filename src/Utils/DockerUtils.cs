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

            Process proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();
            proc.WaitForExit();// Waits here for the process to exit.

            int exitCode = proc.ExitCode;
            string exitOutput = string.Empty;
            using (StreamReader sr = proc.StandardOutput)
            {
                exitOutput = sr.ReadToEnd();
            }

            string exitError = string.Empty;
            using (StreamReader sr = proc.StandardError)
            {
                exitError = sr.ReadToEnd();
            }

            if (fi.Exists)
                fi.Delete();

            return new DockerBuildResult(exitCode, exitOutput, exitError);
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
