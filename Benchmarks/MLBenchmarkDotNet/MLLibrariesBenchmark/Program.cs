using BenchmarkDotNet.Running;

namespace MLLibrariesBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<SkinBenchmark>();
        }
    }
}