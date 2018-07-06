using Gecko;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Core
{
    class test
    {
        void run() {
            DomMouseEventArgs eventArgs = null;
            GeckoElement el = new GeckoElement(eventArgs.Target.NativeObject);
        }
    }
}
