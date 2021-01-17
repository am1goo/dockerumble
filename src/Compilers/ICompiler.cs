namespace dockerumble
{
    public interface ICompiler
    { 
        string name { get; }
        void BuildImage(string dockerImage, CompilerArgs args);
    }
}