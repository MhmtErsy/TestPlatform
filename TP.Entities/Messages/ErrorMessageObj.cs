using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities.Messages
{
    public class ErrorMessageObj
    {
        public ErrorMessageCode Code { get; set; }
        public string Message { get; set; }

        public ErrorMessageObj(ErrorMessageCode code,string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
