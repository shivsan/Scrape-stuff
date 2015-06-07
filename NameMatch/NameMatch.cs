using System.Collections.Generic;
using FuzzyString;
using Objects;
using Objects.Websites;
using System.Linq;
using Objects.Websites.Sites;

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
        
        public static Product BestProduct(List<Product> products, string productName, double minOverLap, Website website = null)
        {
            double overLapValue = int.MaxValue;
            Product bestProduct = null;
            int lowestPrice = int.MaxValue;

            foreach(var product in products)
            {
                int price = product.Price;
                var hDistance = product.RetailName.ToLower().HammingDistance(productName.ToLower());

                if (hDistance <= overLapValue 
                    && NameContainedInProductTitle(product.RetailName, productName) >= 70 
                    && price <= lowestPrice)
                {
                    overLapValue = product.RetailName.ToLower().HammingDistance(productName.ToLower());
                    bestProduct = product;
                    lowestPrice = price;
                }
            }

            //if (overLapValue > minOverLap)
                return bestProduct;
            //else
                //return null;
        }

        public static int NameContainedInProductTitle(string productName, string titleName)
        {
            int percentageOfAcceptance = 0;
            int wordsMatched = 0;
            char[] delimiters = new char[1] { ' ' };
            List<string> splitWordsProduct = productName.ToLower().Replace("ps3", "").Split(delimiters).ToList().Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
            List<string> splitWordsTitle = titleName.ToLower().Replace("ps3", "").Split(delimiters).ToList().Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
            int totalWeightage = splitWordsTitle.Sum(w=>w.Length);
            int weightage = 0;

            foreach (var splitWordTitle in splitWordsTitle)
            {
                foreach (var splitWordProduct in splitWordsProduct)
                {
                    if(splitWordTitle.ToLower().Equals(splitWordProduct.ToLower()))
                    {
                        wordsMatched++;
                        weightage += splitWordTitle.Length;
                    }
                }
            }

            return weightage * 100 / totalWeightage;
        }
    }
}
