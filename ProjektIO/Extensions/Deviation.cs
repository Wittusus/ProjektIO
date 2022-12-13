namespace ProjektIO.Extensions
{
    public static class Deviation
    {
        public static double StdDev(this List<double> valueList)
        {
            if (valueList.Count < 2) return 0.0;
            double sumOfSquares = 0.0;
            double average = valueList.Average(); //.NET 3.0
            foreach (double value in valueList)
            {
                sumOfSquares += Math.Pow((value - average), 2);
            }
            return Math.Sqrt(sumOfSquares / (valueList.Count - 1));
        }
    }
}
