using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Exceptions
{
    public class ExceptionBunch : Exception
    {
        public IList<Exception> InnerExceptions { get; private set; }
        public ExceptionBunch(string message, IList<Exception> innerExceptions) : base(message)
        {
            this.InnerExceptions = innerExceptions;
        }
    }
}