using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace DataDogIntegration
{
    public class DataDogMetricSeries
    {
        public List<Series> series { get; set; }
    }

    public class Series
    {
        public string metric { get; set; }

        public List<List<long>> points { get; set; }


    }

}