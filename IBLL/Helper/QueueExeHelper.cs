using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.Helper
{
    public class QueueExeHelper : Registry
    {
        public QueueExeHelper()
        {
            // Schedule an IJob to run at an interval
            // 立即执行每两秒一次的计划任务。（指定一个时间间隔运行，根据自己需求，可以是秒、分、时、天、月、年等。）

            Schedule(() =>
            {
                // 做你想做的事儿。
                GlabolData.Instance.Process();

            }).ToRunNow().AndEvery(1).Minutes();

        }
    }
}
