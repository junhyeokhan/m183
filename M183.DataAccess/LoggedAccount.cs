﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess
{
    public class LoggedAccount
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}