﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManager.Models.ViewSearchModel
{
    public class ProductTypesSearchModel
    {
        public const string ViewTitle = "產品類別";

        public int? pageNumber { get; set; }
        public int? maxPageNumber { get; set; }

        public string productTypeName { get; set; }
    }
}
