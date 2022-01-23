using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FormulaEvaluator;
namespace Test_The_Evaluator_Console_App
{
    /// <summary>
    /// @author Yinghua
    /// test code in library named FormulaEvaluator if it can be operated successfully.
    /// 
    /// </summary>
    public class Program
    {
        static Dictionary<string, int> hashMap = new Dictionary<string, int>();
        /// <summary>
        /// test the result of subtracting of two elemtents
        /// 
        /// </summary>
        ///
        public static void testArithmeticChar()
        {
            Console.WriteLine("'5' - '0' = " + (int)('5' - '0'));
        }

        /// <summary>
        /// a test function to illustate the obtained by the proposed split method, and the effectiveness of the 
        /// self-implemented white space ignoring function
        /// 
        /// </summary>
        public static void testDeleteSpace()
        {
            string[] testString = { "(5+4)*2-1", " ( 5 + 4 ) * 2 - 1", "(5+4)*2-XY1" };

            foreach (string s in testString)
            {
                string[] substrings = Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

                Console.WriteLine("The original string is \"" + s + "\"");
                foreach (string substring in substrings)
                    Console.WriteLine("###" + substring + "  (with length " + substring.Length + ")");
            }

            Console.WriteLine("split strings after ignoring the whitespaces \n\n");
            foreach (string s in testString)
            {
                string splitString = Evaluator.deleteSpace(s);
                string[] substrings = Regex.Split(splitString, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

                Console.WriteLine("The white-space-ignored string is \"" + splitString + "\"");
                foreach (string substring in substrings)
                    Console.WriteLine("###" + substring + "  (with length " + (int)substring.Length + ")");
            }
        }

        /// <summary>
        /// test if the method can do the add correctly
        /// 
        /// </summary>
        public static void testAdd()
        {
            string testString = "5+4+4";
            int result = Evaluator.Evaluate(testString, null);
            Console.WriteLine("the resolution of " + testString + " is " + result);
        }

        /// <summary>
        /// test if the method can do the substracting correcly 
        /// 
        /// </summary>
        public static void testSub()
        {
            string testString = "5+4-4";
            int result = Evaluator.Evaluate(testString, null);
            Console.WriteLine("the resolution of " + testString + " is " + result);
        }

        /// <summary>
        /// test if the method can do the multiple correctly
        /// 
        /// </summary>
        public static void testMultiple()
        {
            string[] testStrings = { "5", "5*4*4", "18*3*5" };
            foreach (string testString in testStrings)
            {
                int result = Evaluator.Evaluate(testString, null);
                Console.WriteLine("the resolution of " + testString + " is " + result);
            }
        }

        /// <summary>
        /// test if the method can do the Divide correctly
        /// 
        /// </summary>
        public static void testDivide()
        {
            string[] testStrings = { "5", "5000/4/4", "18*3/2" };
            foreach (string testString in testStrings)
            {
                int result = Evaluator.Evaluate(testString, null);
                Console.WriteLine("the resolution of " + testString + " is " + result);
            }
        }

        /// <summary>
        /// test if the method can calculate correctly with paranthese
        /// 
        /// </summary>
        public static void testParanthese()
        {
            string[] testStrings = { "(5)", "(5000+400)/4/4", "(18*3+3)/(3+2)+3" };
            foreach (string testString in testStrings)
            {
                int result = Evaluator.Evaluate(testString, null);
                Console.WriteLine("the resolution of " + testString + " is " + result);
            }
        }

        /// <summary>
        /// test if all exception can be used correctly
        /// 
        /// </summary>
        public static void testException()
        {
            String[] testStrings = { "5+2)", "XY12/4/4", "5+4+", "5/0" };
            
            foreach (String s in testStrings)
            {
                int result = 0;
                try
                {
                    result = Evaluator.Evaluate(s, null);
                }
                catch(Exception e)
                {
                    Console.WriteLine("syntax invalid");
                }
                
                Console.WriteLine("the resolution of " + s + " is " + result);
            }
        }

        /// <summary>
        /// test if the method can count formula with variable correctly,
        /// and the variable has been given values
        /// 
        /// </summary>
        public static void testVariable()
        {

            string[] testStrings = { "X1", "XY20/4/4", "18*X1/2" };
            foreach (string testString in testStrings)
            {
                int result = Evaluator.Evaluate(testString, variableEvaluator);
                Console.WriteLine("the resolution of " + testString + " is " + result);
            }
        }

        /// <summary>
        /// return the value that related key in hashMap
        /// 
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static int variableEvaluator(string varName)
        {
            return hashMap[varName];
        }

        /// <summary>
        /// give the value to variables which are ready to be tested
        /// 
        /// </summary>
        static Program()
        {
            hashMap.Add("X1", 12);
            hashMap.Add("XY1", 40);
            hashMap.Add("XY20", 400);
        }




        static void Main(string[] args)
        {
            Console.WriteLine("test program");
            // FormulaEvaluator.Evaluator formulaEvaluator = new FormulaEvaluator.Evaluator();
            //test_arithmetic_char();
            //test_split_method();


            testAdd();
            testSub();
            testMultiple();
            testDivide();
            testParanthese();
            testVariable();
            
            testException();
            

            String testString = "(5+4)*2-1";
            Console.WriteLine(testString + " = " + Evaluator.Evaluate(testString, null));

            String testString2 = " ( 5 + 4 ) * 2 - 1";
            Console.WriteLine(testString2 + " = " + Evaluator.Evaluate(testString2, null));
        }
    }
}

