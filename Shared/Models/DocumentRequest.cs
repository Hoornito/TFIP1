using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DocumentRequest
    {
        public byte[] DocumentContent { get; set; }
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public string FileName { get; set; }
        public DateTime InsertDate { get; set; }
        public string Status { get; set; }
    }
}
