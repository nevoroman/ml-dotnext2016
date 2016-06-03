using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.CompilerServices;
using BenchmarkDotNet;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Extensions;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Running;
using MLLibrariesBenchmark;




namespace BenchmarkDotNet.Autogenerated
{
    public class Program : global::MLLibrariesBenchmark.SkinBenchmark
    {
        private IJob job = Job.Default.With(BenchmarkDotNet.Jobs.Mode.Throughput).WithWarmupCount(-1).WithTargetCount(-1).WithIterationTime(-1).WithLaunchCount(-1); // TODO

        public static void Main(string[] args)
        {
            try
            {
				Program instance = new Program();
				
				instance.setupAction();
				instance.targetAction();
                System.Console.WriteLine(EnvironmentHelper.GetCurrentInfo().ToFormattedString().AddPrefixMultiline("// "));
                System.Console.WriteLine();
                instance.RunBenchmark();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                throw;
            }
        }

        public Program()
        {
            setupAction = Init;
            idleAction = Idle;
            targetAction = Accord_DecisionTree;
        }

        private System.Double value;
        private Action setupAction;
        private Func<System.Double>  targetAction;
        private Func<int>  idleAction;

        public void RunBenchmark()
        {
            new MethodInvoker().Invoke(job, 1, setupAction, targetAction, idleAction);
        }

        private int Idle()
        {
            return 0;
        }
    }
}