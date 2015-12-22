using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtils
{
    public class FileListsCSV
    {
        public string GameName;
        public int GamePrice;

        public static List<FileListsCSV> ReadGames(string fileName)
        {
            var gamesList = new List<FileListsCSV>();
            var results = FileUtil.ReadCSV(fileName);

            foreach (var result in results)
            {
                gamesList.Add(new FileListsCSV()
                {
                    GameName = result[0],
                    GamePrice = Convert.ToInt16(result[1])
                });
            }
            return gamesList;
        }
    }

    public class IgnoreList
    {
        public string GameURL;

        public static List<IgnoreList> ReadIgnoreList(string fileName)
        {
            var ignoreList = new List<IgnoreList>();
            var results = FileUtil.ReadCSV(fileName);

            foreach (var result in results)
            {
                ignoreList.Add(new IgnoreList()
                {
                    GameURL = result[0],

                });
            }
            return ignoreList;
        }
    }

    public class BlackListList
    {
        public string BlackListText;

        public static List<BlackListList> ReadBlackListList(string fileName)
        {
            var BlackListList = new List<BlackListList>();
            var results = FileUtil.ReadCSV(fileName);

            foreach (var result in results)
            {
                BlackListList.Add(new BlackListList()
                {
                    BlackListText = result[0]
                });
            }
            return BlackListList;
        }
    }

    public class DiscountList
    {
        public double DiscountPercent;
        public double DiscountAmount;
        public string Website;

        public static List<DiscountList> ReadDiscountList(string fileName)
        {
            var DiscountList = new List<DiscountList>();
            var results = FileUtil.ReadCSV(fileName);

            foreach (var result in results)
            {
                DiscountList.Add(new DiscountList()
                {
                    Website = result[0],
                    DiscountAmount = Convert.ToInt16(result[1]),
                    DiscountPercent = Convert.ToInt16(result[2])
                });
            }

            return DiscountList;
        }
    }
}
