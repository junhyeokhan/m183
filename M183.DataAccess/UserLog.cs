﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess
{
    public class UserLog
    {
        public int Id { get; set; }

        //TODO: Decision on data type
        public int Action { get; set; }

        public virtual User User { get; set; }
    }
}