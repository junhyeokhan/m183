﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess.Models
{
    public abstract class Document
    {
        public int Id { get; set; }

        public string Path { get; set; }
    }
}
