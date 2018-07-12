class Test
{
    private delegate void MyDelegate(CancellationToken token);
    static MyDelegate obj;
    private static void MyMethod(CancellationToken token)
    {
        for (int i = 0; i < 1000; i++)
        {
            token.ThrowIfCancellationRequested();
            Console.Write(i + " ");
            Thread.Sleep(100);
        }
    }
    public static void Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        obj = new MyDelegate(MyMethod);
        IAsyncResult result = obj.BeginInvoke(token, null, null);
        new Thread(() => { Thread.Sleep(3000); cts.Cancel(); }).Start();
        try
        {
            obj.EndInvoke(result);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nOperation was canceled.");
        }
        Console.ReadLine();
    }
}
Output:
0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29



    using System;
using System.Threading;

public class Example
{
    public static void Main()
    {
        // Create the token source.
        CancellationTokenSource cts = new CancellationTokenSource();

        // Pass the token to the cancelable operation.
        ThreadPool.QueueUserWorkItem(new WaitCallback(DoSomeWork), cts.Token);
        Thread.Sleep(2500);

        // Request cancellation.
        cts.Cancel();
        Console.WriteLine("Cancellation set in token source...");
        Thread.Sleep(2500);
        // Cancellation should have happened, so call Dispose.
        cts.Dispose();
    }

    // Thread 2: The listener
    static void DoSomeWork(object obj)
    {
        CancellationToken token = (CancellationToken)obj;

        for (int i = 0; i < 100000; i++)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("In iteration {0}, cancellation has been requested...",
                                  i + 1);
                // Perform cleanup if necessary.
                //...
                // Terminate the operation.
                break;
            }
            // Simulate some work.
            Thread.SpinWait(500000);
        }
    }
}
// The example displays output like the following:
//       Cancellation set in token source...
//       In iteration 1430, cancellation has been requested...