﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spryd.Server.Models
{
    public class SharedItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public Session Session { get; set; }
        public string PathUrl { get; set; }
    }
}