﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManager.Models.DbModels
{
    public class OrderDetailStatus : AbstractAppEntity
    {
        public string name { get; set; }

        public int sort { get; set; }
    }
}
