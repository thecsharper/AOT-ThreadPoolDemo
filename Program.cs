using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.NativeAot70)]
[RPlotExporter]
public class Program
{
    public static void Main(string[] args) => BenchmarkRunner.Run<Program>(null!, args);

    private const int WAITS_PER_THREAD = 63;

    [Params(1000)]
    public int N;

    private AutoResetEvent[]? HandlesToRegister;
    private RegisteredWaitHandle[]? RegisteredWaits;
    private WaitOrTimerCallback? CompleteDel;
    private AutoResetEvent? AllCompleted;
    private volatile int CompleteCount;

    [GlobalSetup]
    public void GlobalSetup()
    {
        CompleteDel = new WaitOrTimerCallback(CompleteFunc!);
        RegisteredWaits = new RegisteredWaitHandle[N * WAITS_PER_THREAD];
        HandlesToRegister = new AutoResetEvent[N * WAITS_PER_THREAD];
        AllCompleted = new AutoResetEvent(false);
        for (int i = 0; i < N * WAITS_PER_THREAD; i++)
        {
            HandlesToRegister[i] = new AutoResetEvent(false);
        }

        for (int i = 0; i < N * WAITS_PER_THREAD; i++)
        {
            RegisteredWaits[i] = ThreadPool.RegisterWaitForSingleObject(
                HandlesToRegister[i],
                CompleteDel,
                null,
                -1,
                executeOnlyOnce: false);
        }
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        foreach (var rw in RegisteredWaits!)
        {
            rw.Unregister(null);
        }
        foreach (var wh in HandlesToRegister!)
        {
            wh.Close();
        }
    }

    [Benchmark]
    public void BenchWait()
    {
        CompleteCount = 0;

        for (int i = 0; i < N; i++)
        {
            HandlesToRegister![i * WAITS_PER_THREAD].Set();
        }

        AllCompleted!.WaitOne();
    }

    private void CompleteFunc(object state, bool timedOut)
    {
        if (Interlocked.Add(ref CompleteCount, 1) == N)
        {
            AllCompleted!.Set();
        }
    }
}