﻿using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DocumentInfo
    {
        public string Name { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime PrintDate { get; set; }
        public string Status { get; set; }
    }
}
