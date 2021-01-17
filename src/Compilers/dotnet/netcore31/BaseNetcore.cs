using System;

namespace dockerumble
{
    public abstract class BaseNetcore : ICompiler
    {
        public abstract string name { get; }
        public abstract string from { get; }

        protected BaseNetcore()
        {
        }

        public void BuildImage(string dockerImage, CompilerArgs args)
        {
            string dockerfileText = Template(TEMPLATE, from, args);

            Console.WriteLine($"Starting to create docker image {dockerImage}");
            DockerUtils.DockerBuildResult result = DockerUtils.Build(dockerImage, dockerfileText);
            if (result.exitCode == 0)
            {
                Console.WriteLine($"Docker image {dockerImage} was created");
            }
            else
            {
                Console.WriteLine($"Docker image {dockerImage} not created");
                Errors.Error(result.consoleError);
                Environment.Exit(result.exitCode);
            }
        }
        
        private string Template(string template, string from, CompilerArgs args)
        {
            return template
                .Replace("$FROM$", from)
                .Replace("$REPO_URL$", args.repo)
                .Replace("$REPO_NAME$", args.reponame)
                .Replace("$REPO_COMMIT$", args.repocommit)
                .Replace("$PASS_ARGS$", args.passArgs)
                .Replace("$LABELS$", DockerUtils.Labels(args.repo, args.repocommit))
                .Trim();
        }

        private const string TEMPLATE = @"
FROM $FROM$
$LABELS$

RUN apt-get update && apt-get install -y git
ADD ""https://www.random.org/cgi-bin/randbyte?nbytes=10&format=h"" skipcache

RUN mkdir app && cd app && git clone $REPO_URL$ && cd /app/$REPO_NAME$ && git reset --hard $REPO_COMMIT$
RUN cd /app/$REPO_NAME$ && dotnet build $PASS_ARGS$ --output /app/$REPO_NAME$.output
        ";
    }
}
