using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample05
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }
        public void Run()
        {
            Console.WriteLine("Start Run");
            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions po = new ParallelOptions() { CancellationToken = cts.Token };
            Task t = Task.Run(() => Work(po), cts.Token);  // new method in .NET 4.5
            Console.WriteLine("Cancel in 3sec");
            cts.CancelAfter(3000);  // new method in .NET 4.5
            t.Wait();
            Console.WriteLine("End Run");
            Console.ReadLine();
        }
        private void Work(ParallelOptions po)
        {
            Console.WriteLine("Start Work");
            ParallelLoopResult loopResult = new ParallelLoopResult();
            double[] arr = new double[10] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            try
            {
                loopResult = Parallel.ForEach(arr, po, (element, loopState) =>
                {
                    Console.WriteLine(element);
                    DoSomeWork(element, loopState);
                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("End OperationCanceledException");
            }
            Console.WriteLine("End Work: {0}", loopResult.IsCompleted);
        }
        private void DoSomeWork(double element, ParallelLoopState loopState)
        {
            double temp = 1.1;
            for (int i = 0; i < 100000000; i++)
            {
                temp = Math.Sin(element) + Math.Sqrt(element) * Math.Pow(element, 3.1415) + temp;
                if (loopState.ShouldExitCurrentIteration)
                {
                    Console.WriteLine("Return DoSomeWork");
                    return;
                }
            }
        }
    }
}
