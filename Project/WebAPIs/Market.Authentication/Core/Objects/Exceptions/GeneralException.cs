using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Authentication.Core.Objects.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException(string message) : base(message)
        {
        }

        public GeneralException(int code, string message) : base(message)
        {
            Code = code;
        }

        public int Code { get; set; }
    }
}