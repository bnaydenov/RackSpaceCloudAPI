using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ConsoleClient
{
    class ArgumentParser
    {
        #region Parser Methods

        //Method which will parse the string input and return a hashtable
        public static Hashtable Parse(String[] args)
        {
            Hashtable argTable = new Hashtable();
            try
            {
                string[] keyvalue;
                string value;

                if ((null == args) || args.Length == 0)
                {
                    argTable.Add("Invalid", "true");
                    return argTable;
                }


                foreach (string s in args)
                {

                    if ((s.StartsWith("-") || s.StartsWith("/")) && (s.Contains("=") && !s.Contains("?")))
                    {
                        // Strip off - or /
                        string key = s.Substring(1, s.Length - 1);
                        keyvalue = key.Split('=');
                        key = keyvalue[0].ToString();
                        value = keyvalue[1].ToString();

                        if (value == null || value.Trim() == "")
                        {
                            argTable.Add("Invalid", "true");
                            return argTable;
                        }

                        AddKeyValuePair(argTable, key, value);

                    }
                    else if (s.StartsWith("-") || s.StartsWith("/"))
                    {
                        string key = s.Substring(1, s.Length - 1).ToLower();
                        argTable.Add(key,string.Empty);
                    }
                    else
                    {
                        // otherwise this is invalid value
                        if (s.Contains("?"))
                        {
                            AddKeyValuePair(argTable, "?", "true");
                        }
                        else
                        {
                            AddKeyValuePair(argTable, "Invalid", "true");
                        }
                    }
                }
                // return the hashtable with the command line arguments in it.
                return argTable;
            }
            catch (ArgumentOutOfRangeException outOfRangeEx)
            {
                Console.WriteLine("Arguments out of range, please check error log" + outOfRangeEx.Message + Environment.NewLine + outOfRangeEx.StackTrace);
                return argTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                return argTable;
            }
        }

        //Add the arguments in the form of a keyvalue pair to Hashtable
        public static void AddKeyValuePair(Hashtable argTable, string key, string value)
        {
            try
            {
                if (!argTable.ContainsKey(key))
                {
                    // add this to table
                    argTable.Add(key, value);
                }
                else
                {
                    //substitute the value with the latest one
                    argTable[key] = value;
                }
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message + Environment.NewLine + argEx.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        #endregion
    }
}
