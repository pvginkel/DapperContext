using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public interface IDbRetryPolicy
    {
        int Retries { get; }
        TimeSpan Delay { get; }

        bool ShouldRetry(Exception exception);
    }
}
