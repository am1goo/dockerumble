namespace dockerumble
{
    public class Netcore31 : BaseNetcore
    {
        public override string name { get { return "netcore31"; } }
        public override string from { get { return "mcr.microsoft.com/dotnet/sdk:3.1"; } }

        public Netcore31() 
        {
        }
    }
}