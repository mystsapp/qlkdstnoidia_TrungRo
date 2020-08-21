using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qlkdst.Models
{
    public class UserUpdPass
    {
        public string username { get; set; }
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
        public string repeatnewpassword { get; set; }
    }
}