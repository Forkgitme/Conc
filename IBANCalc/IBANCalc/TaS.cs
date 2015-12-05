using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class TaS
{
    private int bit;

    public TaS()
    {
        bit = 0;
    }

    public void Lock()
    {
        while (Interlocked.Exchange(ref bit, 1) == 1) { };
    }

    public void Unlock()
    {
        Interlocked.Exchange(ref bit, 0);
    }
}
