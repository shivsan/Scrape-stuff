using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace FileUtils
{
    public class FileUtil
    {
        

        public static List<List<string>> ReadCSV(string fileName)
        {
            var genericObj = new List<List<string>>();

            using (TextFieldParser parser = new TextFieldParser(fileName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    
                    {
                        if (fields != null)
                        {
                            var newRow = new List<string>();
                            genericObj.Add(newRow);

                            foreach (var field in fields)
                            {
                                newRow.Add(field);
                            }
                        }
                    }
                }
            }

            return genericObj;
        }
    }
}