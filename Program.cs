using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTestFotmob
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine(p.Interval("10-100", "20-30"));
            //Console.WriteLine(p.Interval("50-5000, 10-100", ""));
            //Console.WriteLine(p.Interval("10-100, 200-300", "95-205"));
            //Console.WriteLine(p.Interval("10-100, 200-300, 400-500", " 95-205, 410-420"));
        }
        public string Interval(string include, string exclude)
        {
            List<int> interval = new List<int>(); // check if used
            List<int> inc = new List<int>();
            List<int> exc = new List<int>();
            if (include != "")
            {
                if (include.Contains(","))
                {
                    inc = SplitMulti(include); // method that splits up input string to int list if it contains multiple sets 
                }
                else
                {
                    inc = SplitSingle(include); // method that splits up input string to int list if it contains one set
                }
                inc.Sort();
            }
            if (exclude != "")
            {
                if (exclude.Contains(","))
                {
                    exc = SplitMulti(exclude); // method that splits up input string to int list if it contains multiple sets 
                }
                else
                {
                    exc = SplitSingle(exclude); // method that splits up input string to int list if it contains one set
                }
                exc.Sort();
            }
            string retVal = "";
            if (exc.Any())
            {
                retVal = CreateOutputString(inc, exc);
            }
            else
            {
                retVal = inc.First() + "-" + +inc.Last();
            }

            return retVal;
        }
        /// <summary>
        /// splits the input string if it contains "," int to pair values
        /// then splits the pair values on the "-" and returns a int list
        /// </summary>
        /// <param name="include"></param>
        /// <returns>List<int></returns>
        public List<int> SplitMulti(string input)
        {
            List<int> interval = new List<int>();
            string[] incArr = input.Split(",");
            for (int i = 0; i < incArr.Length; i++)
            {
                string[] incArr2 = incArr[i].Split("-");
                interval.Add(int.Parse(incArr2[0]));
                interval.Add(int.Parse(incArr2[1]));
            }
            return interval;
        }
        /// <summary>
        /// split the input string if only one set
        /// splits on "-"
        /// </summary>
        /// <param name="input"></param>
        /// <returns> list<int></returns>
        public List<int> SplitSingle(string input)
        {
            List<int> inp = new List<int>();
            string[] incArr = input.Split("-");
            inp.Add(int.Parse(incArr[0]));
            inp.Add(int.Parse(incArr[1]));
            return inp;
        }
        /// <summary>
        /// method that excludes the numbers to be excluded in the 
        /// given include values
        /// </summary>
        /// <param name="inc"></param>
        /// <param name="exc"></param>
        /// <returns></returns>
        public string CreateOutputString(List<int> inc, List<int> exc)
        {
            string retVal = "";
            List<int> interval = new List<int>();
            bool pair = false; //was last added exclude value yes if true 
            int e = 0; //exc counter
            int i = 1; //inc counter
            if (inc.First() < exc.First()) //checks id first inc value is smaler than first exc value if true then add to retVal
            {
                retVal = inc.First() + "-";
            }
            else //if first exc is smaller then remove the value and set pair to true
            {
                exc.RemoveAt(0);
                pair = true;
            }
            while (e < exc.Count && i < inc.Count)
            {

                if (exc[e] < inc[i] && !pair) // if exclude value is smaller than inc value and its not pair add
                {
                    retVal = retVal + (exc[e] - 1) + ", ";
                    e++;
                    if (e == exc.Count)
                    {
                        break;
                    }
                    pair = true;
                }
                else if (pair && exc[e] < inc[i]) //if pair last was exclude and exclude is smaller than current include
                {
                    retVal = retVal + (exc[e] + 1) + "-";
                    e++;
                    pair = false;
                    while (exc[e - 1] > inc[i]) //skip include values this include is bigger than last exclude
                    {
                        i++;
                    }
                    if (e < exc.Count && (exc[e]) > inc[i]) //add inc if inc is smaler than exc and e<exc.count
                    {
                        retVal = retVal + inc[i] + ", ";
                        i++;
                    }
                }
                else
                {
                    if (exc[e] > inc[i] && !pair)
                    {
                        retVal = retVal + inc[i] + "-";
                        i++;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            if (i < inc.Count) //if more include values left add last
            {
                retVal = retVal + inc.Last();
            }
            if (retVal.Substring(retVal.Length - 2).CompareTo(", ") == 0) // removes ", " if last exc>= last inc
            {
                retVal = retVal.Remove((retVal.Length - 2));
            }
            return retVal;
        }
    }
}
