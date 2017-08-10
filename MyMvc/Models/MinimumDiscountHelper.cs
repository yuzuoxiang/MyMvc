﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models
{
    public class MinimumDiscountHelper : IDiscountHelper
    {
        public decimal ApplyDiscount(decimal totalParam)
        {
            if (totalParam < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (totalParam > 100)
            {
                return totalParam * 0.9m;
            }
            else if (totalParam >= 10m && totalParam <= 100)
            {
                return totalParam - 5;
            }
            else
            {
                return totalParam;
            }
        }
    }
}