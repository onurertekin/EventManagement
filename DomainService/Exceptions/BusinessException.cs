using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Exceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public BusinessException(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.ErrorMessage = message;
        }
    }
}
