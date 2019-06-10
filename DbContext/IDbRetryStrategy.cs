using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public interface IDbRetryStrategy
    {
        IEnumerable<TimeSpan> GetIntervals();
    }
}
