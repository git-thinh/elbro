﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace elbro
{

/*

var stopHandle = EasyTimer.SetTimeout(() =>
{
    // --- You code here ---
    // This piece of code will once after 1000 ms

}, 1000);

// In case you want to clear the timeout
stopHandle.Dispose();

*/

    internal static class ActionTimer
    {
        public static IDisposable SetInterval(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.Enabled = true;
            timer.Start();

            // Returns a stop handle which can be used for stopping
            // the timer, if required
            return timer as IDisposable;
        }

        public static IDisposable SetTimeout(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();

            // Returns a stop handle which can be used for stopping
            // the timer, if required
            return timer as IDisposable;
        }
    }
}
