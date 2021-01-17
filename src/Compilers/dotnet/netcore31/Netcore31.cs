using System;
using System.IO;

namespace dockerumble
{
    public class Netcore31 : ICompiler
    {
        public string name { get { return "netcore31"; } }

        public Netcore31() 
        {
        }

        private const string TEMPLATE_FILENAME = "Templates/Netcore31.template";
        public void BuildImage(string dockerImage, CompilerArgs args)
        {
            FileInfo fi = new FileInfo(TEMPLATE_FILENAME);
            if (!fi.Exists)
            {
                Errors.Error($"file {TEMPLATE_FILENAME} not found");
                return;
            }

            using (StreamReader sr = fi.OpenText())
            {
                string template = sr.ReadToEnd();
                template = template
                    .Replace("$REPO_URL$", args.repo)
                    .Replace("$REPO_NAME$", args.reponame)
                    .Replace("$REPO_COMMIT$", args.repocommit)
                    .Replace("$PASS_ARGS$", args.passArgs)
                    .Replace("$LABELS$", DockerUtils.Labels(args.repo, args.repocommit));

                Console.WriteLine($"Starting to create docker image {dockerImage}");
                DockerUtils.DockerBuildResult result = DockerUtils.Build(dockerImage, template);
                if (result.exitCode == 0)
                {
                    Console.WriteLine($"Docker image {dockerImage} was created");
                }
                else
                {
                    Console.WriteLine($"Docker image {dockerImage} not created");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{result.consoleError}");
                    Console.ResetColor();
                    Environment.Exit(result.exitCode);
                }
            }
        }
    }
}