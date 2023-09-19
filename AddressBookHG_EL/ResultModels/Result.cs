using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.ResultModels
{
    public class Result : IResult
    {
        //insert update delete içindir
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public Result()
        {

        }

        public Result(bool success, string? msg)
        {
            IsSuccess = success;
            Message = msg;
        }
    }
}
