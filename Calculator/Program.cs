/* Program to get sensor tracking data from an Oculus Service log. 
 * This data is put into csv format and saved to a new file.
 * See footnotes for more info
 */

using System;
using System.IO;
using System.Linq;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // path, filename and extension of original log file
            string path = @"C:\Users\Yoey\Documents\Logs\";
            string filename = "BeatSaber_050820";
            string extension = ".txt";

            // suffix and extension to be added to output file
            string suffix = "_SensorStats.csv";

            //check that the file exists. 
            if (!File.Exists(path + filename + extension))
            {
                // if it does not exist prompt the user to exit the program. 
                Console.WriteLine("File does not exist. Press any key to exit.");
                Console.ReadKey();
            }

            else
            {
                // if the file exists, read and filter the data
                var FinalList = ReadAllLinesAndApplyFilters(path, filename, extension);
                Console.WriteLine(FinalList.Length);


                for (int i = 0; i < 10; i++)
                   Console.WriteLine(FinalList[i]);

                // save the output to a new file
                File.WriteAllLines(path + filename + suffix, FinalList);



                Console.ReadKey();
            }
        }

        private static string[] ReplaceTextInArray(string[] array, string oldText, string newText)
        {
            string[] rArray = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                rArray[i] = array[i].Replace(oldText, newText);
            }
            return rArray;
        }
        private static string[] RemoveTextInArray(string[] array, int start, int num)
        {
            string[] rArray = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                rArray[i] = array[i].Remove(start, num);
            }
            return rArray;
        }
        private static string[] TrimArray(string[] array, char character)
        {
            string[] rArray = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                rArray[i] = array[i].TrimEnd(character);
            }
            return rArray;
        }
        private static string[] GetSubstringofArray(string[] array, int index, int length)
        {
            string[] strA = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                strA[i] = array[i].Substring(index,length);
            }
            return strA;
        }
        private static string[] CombineStringArrays(string[] stringArrayA, string[] stringArrayB)
        {
            string[] strAB = new string[stringArrayA.Length];
            for (int i = 0; i < strAB.Length; i++)
                strAB[i] = stringArrayA[i] + stringArrayB[i];
                
            return strAB;
        }

        static string[] ReadAllLinesAndApplyFilters(string path, string filename, string extension)
        {
            var fullList = File.ReadLines(path + filename + extension).Where(line => line.Contains("[HW:Health] Visibility:")).ToArray();
            int fullLength = fullList.Length;

            int shortenX = 10;
            int shortLength = (int)Math.Ceiling((decimal)fullLength / shortenX);
            string[] shortList = new string[shortLength];
            int j = 0;
            for (int i = 0; i < fullLength; i += shortenX)
            {
                shortList[j] = fullList[i];
                j++;
            }   

            var list1 = RemoveTextInArray(shortList, 0, 55);
            var list2 = RemoveTextInArray(list1, 3, 5);
            var list3 = RemoveTextInArray(list2, 6, 5);
            var list4 = RemoveTextInArray(list3, 9, 9);
            var list5 = ReplaceTextInArray(list4, ".", "0,");
            var list6 = ReplaceTextInArray(list5, "-", "1,");
            var list7 = ReplaceTextInArray(list6, "+", "2,");
            var list8 = ReplaceTextInArray(list7, "*", "3,");
            //var list9 = TrimArray(list8, ',');
            //var list10 = GetSubstringofArray(list0,0,18);
            // var list12 = ReplaceTextInArray(list10, " ", ",");


            var month = GetSubstringofArray(shortList, 3, 2);
            var day = GetSubstringofArray(shortList, 0, 2);
            var time = GetSubstringofArray(shortList, 6, 10);

            string[] output = new string[shortLength];
            string[] date = new string[shortLength];
            for (int i = 0; i < shortLength; i++)
            {
                date[i] = month[i] + "/" + day[i] + "/2020," + time[i];
                output[i] = list8[i] + date[i];

            }

           
            //var listOut = CombineStringArrays(list8, list12);
            Console.WriteLine(fullLength);

            return output;
        }
       
    }
}

//static string[] ApplyAllFilters1(string path, string filename, string extension)
//{
//    var list0 = File.ReadLines(path + filename + extension).Where(line => line.Contains("[HW:Health] Visibility:")).ToArray();
//    var list1 = ReplaceTextInArray(list0, " {INFO}    [HW:Health] Visibility: ", ", ");
//    var list2 = ReplaceTextInArray(list1, ".  R", ", R");
//    var list3 = ReplaceTextInArray(list2, ".  L", ", L");
//    var list4 = ReplaceTextInArray(list3, ".  O ....", "");

//    return list4;
//}


//.--.  R -...  L -.-.  O...
//var list1 = new string[List0.Length];
//string temp1 = list0[0].Replace(" {INFO}    [HW:Health] Visibility: ", ", ");

//foreach (string foundline in found)
//{
//    foundline.Replace(" {INFO}    [HW:Health] Visibility: ", ", ");
//    found2[i] = foundline;
//    i++;
//}

//Console.WriteLine("Writing filtered text to new file...");
//File.WriteAllLines(FileToRead + "_Visibility2" + extension, found2);
//Console.WriteLine("Done. Press any key to exit.");
//var list1 = RemoveTextInArray(list0, 0, 55);
//var list2 = RemoveTextInArray(list1, 3, 5);
//var list3 = RemoveTextInArray(list2, 6, 5);
//var list4 = RemoveTextInArray(list3, 9, 9);
//var list5 = ReplaceTextInArray(list4, ".", "0,");
//var list6 = ReplaceTextInArray(list5, "-", "1,");
//var list7 = ReplaceTextInArray(list6, "+", "2,");
//var list8 = ReplaceTextInArray(list7, "*", "3,");
/*  
 *  sample line of data we are going to keep and filter
 *            1         2         3         4         5         6         7         8
 *  01234567890123456789012345678901234567890123456789012345678901234567890123456789012
 *  06/05 18:09:52.866 {INFO}    [HW:Health] Visibility: H *...  R -...  L +...  O ....
*/
//static string[] TimeStamp(string path, string filename, string extension)
//{
//    var list0 = File.ReadLines(path + filename + extension).Where(line => line.Contains("[HW:Health] Visibility:")).ToArray();
//    var list1 = RemoveTextInArray(list0, 0, 55);
//    var list2 = RemoveTextInArray(list1, 3, 5);
//    var list3 = RemoveTextInArray(list2, 6, 5);
//    var list4 = RemoveTextInArray(list3, 9, 9);
//    var list5 = ReplaceTextInArray(list4, ".", "0,");
//    var list6 = ReplaceTextInArray(list5, "-", "1,");
//    var list7 = ReplaceTextInArray(list6, "+", "2,");
//    var list8 = ReplaceTextInArray(list7, "*", "3,");
//    //var list9 = TrimArray(list8, ',');
//    //var list9 = RemoveTextInArray(list0, 55, list0[0].Length - 1);
//    return list8;
//}