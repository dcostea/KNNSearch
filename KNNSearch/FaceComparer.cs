using System.Collections.Generic;

namespace KNNSearch
{
    public class FaceComparer : IComparer<Face>
    {
        public int Compare(Face x, Face y)
        {
            // TODO: Handle x or y being null, or them not having names
            return x.Metrics.GetHashCode().CompareTo(y.Metrics.GetHashCode());
        }
    }
}
