﻿using RoamlerLocationSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoamlerLocationSearch.Business.Extensions
{
    public static class Extensions
    {
        public static IQueryable<Location> orderLocationby(this IQueryable<Location> input)
        {
            //input.OrderBy(loc => CalculateDistance(loc))
            return input;
        }
    }
}