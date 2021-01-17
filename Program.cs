using System;
using System.Collections.Generic;
using System.Reflection;

namespace dockerumble
{
    class Program
    {
        private const string CMD_BUILD = "build";
        private const string CMD_LIST_COMPILERS = "list-compilers";
        private const string CMD_HELP = "help";

        private const string ARG_DOCKER_IMAGE_SHORT = "-di";
        private const string ARG_DOCKER_IMAGE_NORMAL = "--docker-image";

        private const string ARG_COMPILER_SHORT = "-c";
        private const string ARG_COMPILER_NORMAL = "--compiler";

        private const string ARG_PASS_SHORT = "-p";
        private const string ARG_PASS_NORMAL = "--pass";

        private const string ARG_REPO_SHORT = "-r";
        private const string ARG_REPO_NORMAL = "--repo";

        private const string ARG_REPO_COMMIT_SHORT = "-rc";
        private const string ARG_REPO_COMMIT_NORMAL = "--repocommit";

        private static void Main(string[] args)
        {
            ProgramArgs.Parse(args);
            Console.WriteLine($"{ProgramArgs.ApplicationName} - tool to automatic build projects into docker images");

            RegisterCompilers();

            if (args.Length == 0)
            {
                Errors.Error("not enough arguments");
                return;
            }

            string command = args[0];
            switch (command)
            {
                case CMD_LIST_COMPILERS:
                    PrintCompilers();
                    break;

                case CMD_BUILD:

                    if (!ProgramArgs.GetArg(ARG_DOCKER_IMAGE_SHORT, ARG_DOCKER_IMAGE_NORMAL, out string dockerImage))
                    {
                        Errors.Arguments(ARG_DOCKER_IMAGE_SHORT, ARG_DOCKER_IMAGE_NORMAL);
                        return;
                    }

                    if (!ProgramArgs.GetArg(ARG_COMPILER_SHORT, ARG_COMPILER_NORMAL, out string compiler))
                    {
                        Errors.Arguments(ARG_COMPILER_SHORT, ARG_COMPILER_NORMAL);
                        return;
                    }

                    if (!GetCompiler(compiler, out ICompiler c))
                    {
                        Errors.Error($"unsupported compiler {compiler}");
                        return;
                    }

                    if (!ProgramArgs.GetArg(ARG_PASS_SHORT, ARG_PASS_NORMAL, out string passArgs))
                    {
                        passArgs = string.Empty;
                    }

                    if (!ProgramArgs.GetArg(ARG_REPO_SHORT, ARG_REPO_NORMAL, out string repo))
                    {
                        Errors.Arguments(ARG_REPO_SHORT, ARG_REPO_NORMAL);
                        return;
                    }

                    if (!GitUtils.TryParseRepoName(repo, out string reponame))
                    {
                        Errors.Error($"can't parse reponame from {repo}");
                        return;
                    }

                    if (!ProgramArgs.GetArg(ARG_REPO_COMMIT_SHORT, ARG_REPO_COMMIT_NORMAL, out string repocommit))
                    {
                        repocommit = "HEAD";
                    }

                    CompilerArgs compilerArgs = new CompilerArgs
                    {
                        repo = repo,
                        reponame = reponame,
                        repocommit = repocommit,
                        passArgs = passArgs,
                    };
                    c.BuildImage(dockerImage, compilerArgs);
                    break;

                case CMD_HELP:
                    Console.WriteLine("");
                    Console.WriteLine($"Usage: {ProgramArgs.ApplicationName} COMMAND [OPTIONS]");
                    Console.WriteLine("");
                    Console.WriteLine("Commands:");
                    Console.WriteLine($"  {CMD_BUILD}");
                    Console.WriteLine($"  {CMD_LIST_COMPILERS}");
                    Console.WriteLine($"  {CMD_HELP}");
                    break;

                default:
                    Errors.Error($"unknown command {command}");
                    goto case CMD_HELP;
            }
        }

        private static Dictionary<string, ICompiler> compilers = new Dictionary<string, ICompiler>();
        private static void RegisterCompilers()
        {
            Type expectType = typeof(ICompiler);
            List<Type> inheritTypes = new List<Type>();
            CommonUtils.GetInheritTypes(expectType, 0, inheritTypes);

            foreach (Type type in inheritTypes)
            {
                ICompiler compiler = (ICompiler)Activator.CreateInstance(type);
                compilers[compiler.name] = compiler;
            }
        }

        private static void PrintCompilers()
        {
            CommonUtils.PrintKeys(compilers);
        }

        private static bool GetCompiler(string name, out ICompiler compiler)
        {
            return compilers.TryGetValue(name, out compiler);
        }
    }
}
