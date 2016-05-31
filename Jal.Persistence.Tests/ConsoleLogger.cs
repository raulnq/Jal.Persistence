using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jal.Persistence.Impl;

namespace Jal.Persistence.Tests
{
    public class ConsoleLogger : AbstractRepositoryLogger
    {
        public override void Info(string message)
        {
            Console.WriteLine(message);
        }

        public override void Error(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
