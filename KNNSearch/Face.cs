using System;

namespace KNNSearch
{
    public class Face
    {
        public string Url { get; set; }
        public Guid Guid { get; set; }
        public double[] Metrics { get; set; }
    }
}
