namespace dockerumble
{
    public static class GitUtils
    {
        public static bool TryParseRepoName(string repo, out string result)
        {
            string[] splitted = repo.Split('/');
            if (splitted.Length <= 1)
            {
                result = string.Empty;
                return false;
            }

            result = splitted[splitted.Length - 1];
            return true;
        }
    }
}
