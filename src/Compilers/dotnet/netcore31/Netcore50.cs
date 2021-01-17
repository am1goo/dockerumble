namespace dockerumble
{
    public class Netcore50 : BaseNetcore
    {
        public override string name { get { return "netcore50"; } }
        public override string from { get { return "mcr.microsoft.com/dotnet/sdk:5.0"; } }

        public Netcore50() 
        {
        }
    }
}