namespace dockerumble
{
    public class Netcore21 : BaseNetcore
    {
        public override string name { get { return "netcore21"; } }
        public override string from { get { return "mcr.microsoft.com/dotnet/sdk:2.1"; } }

        public Netcore21() 
        {
        }
    }
}