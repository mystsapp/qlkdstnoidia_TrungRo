using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qlkdst.Models
{
    public class UploadMessage
    {
        public string message { get; set; }
        public int count { get; set; }
        public int errorCount { get; set; }

        public UploadMessage()
        {
            message = "";
            count = 0;
            errorCount = 0;
        }
    }
}