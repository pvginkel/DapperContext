using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperContext.Test
{
    public class FailureCallbacks
    {
        public Action Open { get; set; }
        public Action BeginTransaction { get; set; }
        public Action CommitTransaction { get; set; }
        public Action RollbackTransaction { get; set; }
        public Action ExecuteCommand { get; set; }
    }
}
