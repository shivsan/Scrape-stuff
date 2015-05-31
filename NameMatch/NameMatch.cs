using System.Collections.Generic;
using FuzzyString;
using Objects;
using Objects.Websites;

namespace NameMatch
{
    public static class NameMatching
    {
        public static bool DoNamesMatch(string RetailName, string itemName)
        {
            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();

            // Choose which algorithms should weigh in for the comparison
            options.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);

            // Choose the relative strength of the comparison - is it almost exactly equal? or is it just close?
            FuzzyStringComparisonTolerance tolerance = FuzzyString.FuzzyStringComparisonTolerance.Normal;

            // Get a boolean determination of approximate equality
            bool result = itemName.ApproximatelyEquals(RetailName, options, tolerance);
            //itemName.OverlapCoefficient(RetailName);
            return result;
        }
        
        public static Product BestProduct(List<Product> products, string productName, double minOverLap)
        {
            double overLapValue = int.MaxValue;
            Product bestProduct = null;
            foreach(var product in products)
            {
                if(product.RetailName.ToLower().HammingDistance(productName.ToLower()) < overLapValue)
                {
                    overLapValue = product.RetailName.ToLower().HammingDistance(productName.ToLower());
                    bestProduct = product;
                }
            }

            //if (overLapValue > minOverLap)
                return bestProduct;
            //else
                //return null;
        }
    }
}
