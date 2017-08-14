using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASX.Market.Jobs.Core.Exceptions
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