using System;
using System.Threading;

namespace Utility
{
    public class Extra
    {
        static public void RepeatInvoke(Action action, uint invokeTimes, int deltaTime)
        {
            var thread = new Thread(new ThreadStart(() =>
            {
                while (invokeTimes-- > 0)
                {
                    action();
                    Thread.Sleep(deltaTime);
                }
            }));

            thread.Start();
        }
    }

}
