﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContext
{
    public class DbContextConfiguration
    {
        public IDbRetryPolicy RetryPolicy { get; }
        public IsolationLevel? DefaultIsolationLevel { get; }

        public DbContextConfiguration(IDbRetryPolicy retryPolicy = null, IsolationLevel? defaultIsolationLevel = null)
        {
            RetryPolicy = retryPolicy;
            DefaultIsolationLevel = defaultIsolationLevel;
        }
    }
}
