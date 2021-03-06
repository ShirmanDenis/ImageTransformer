﻿using System;
using System.Drawing;

namespace ImageTransform.Client
{
    public static class ApiUris
    {
        public static Uri GetRegisteredFilters = new Uri("info/defined/filters", UriKind.Relative);

        public static Uri ProcessImage(string filterName, Rectangle rect)
        {
            return new Uri($"process/{filterName}/{rect.X},{rect.Y},{rect.Width},{rect.Height}", UriKind.Relative);
        }
    }
}