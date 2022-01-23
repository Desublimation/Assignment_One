using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

//project adding 
using System.Text.RegularExpressions;   //for Regex.Split()


namespace FormulaEvaluator
{
    /// <summary>
    /// @aurthor Yinghua Yin(u1297837)
    /// this class is a library. it is going to solve the Formula of four operations
    /// and parentheses "(,+,-,*,/,)"
    /// 
    /// </summary>
    public static class Evaluator
    {

        // define a structure for value stack 
        static Stack<int> valStack = new Stack<int>();
        // define a structure for operators stack
        static Stack<char> optStack = new Stack<char>();
        public delegate int LookUp(String variableName);

        /// <summary>
        /// delete the space in original String
        /// 
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns> a string without space in it
        public static string deleteSpace(string originalString)
        {
            string result = "";
            if (originalString != null)
            {
                foreach (char c in originalString)
                {
                    if (c != ' ') result += c;
                }
            }
            return result;
        }


        /// <summary>
        /// decide if the string is a number, and in the range of [0-9]
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> true if it is a number,false if it is not
        public static bool isTokenInteger(string token)
        {
            if (token.Length == 0)
                return false;
            else
            {
                foreach (char c in token)
                {
                    if (!('0' <= token[0] && token[0] <= '9'))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// decide if the string is a Variable like x or y.
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> true if it is a variable or false if it is not
        public static bool isTokenVariable(string token)
        {
            return token.Length >= 2;
        }

        /// <summary>
        /// decide if the operator is multiply or devide
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> true if it is multiply or devide or false if it is not
        public static bool isTokenMulDiv(string token)
        {
            return (token.Length > 0) && (token[0] == '*' || token[0] == '/');
        }

        /// <summary>
        /// decide if it is left paranthesis
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> true if it is left paranthesis or false if it is not
        public static bool isTokenLeftParanthese(string token)
        {
            return (token.Length > 0) && (token[0] == '(');
        }

        /// <summary>
        ///decide if it is add or sub
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> true if it is add or sub or false if it is not
        public static bool isTokenAddSub(string token)
        {
            return (token.Length > 0) && (token[0] == '+' || token[0] == '-');
        }

        /// <summary>
        /// decide if it is right paranthesis
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> true if it is right paranthesis or false if it is not
        public static bool isTokenRightParanthese(string token)
        {
            return (token.Length > 0) && (token[0] == ')');
        }


        /// <summary>
        /// calculate the number by multiple and divide, if divided by zero
        /// throw the exception
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> 0 if calculate successful
        static void ActTokenInteger(String token)
        {
            // TODO ... 
            int newInteger = Convert.ToInt32(token);

            if (optStack.Count > 0 && (optStack.Peek() == '*' || optStack.Peek() == '/'))
            {

                if (valStack.Count == 0)
                    throw (new NullReferenceException("there is no element in value stack"));          //  expression invalid : value stack is empty (no integer before a * or /) 

                int lastInteger = valStack.Pop();
                char lastOpt = optStack.Pop();

                if (lastOpt == '/' && newInteger == 0)
                {
                    throw (new DivideByZeroException("divided by zero"));                 // expression invalid : Division by zero occurs
                }
                else
                {
                    if (lastOpt == '*')
                        valStack.Push(lastInteger * newInteger);
                    else if (lastOpt == '/')
                        valStack.Push(lastInteger / newInteger);
                }
            }
            else
            {
                valStack.Push(newInteger);
            }
        }

        /// <summary>
        /// calculate the variable by multiple and divide 
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns></returns>
        static void ActTokenVariable(String token, LookUp variableEvaluator)
        {
            // int newInteger = variableEvaluator(token);       // while in fact an exception should be threw by this delegate when the argument isn't valid.

            // TODO ...
            int newInteger = 0;
            try
            {
                newInteger = variableEvaluator(token);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }

            if (optStack.Count > 0 && (optStack.Peek() == '*' || optStack.Peek() == '/'))
            {

                if (valStack.Count == 0)
                    throw (new NullReferenceException("there is no variable"));
                int lastInteger = valStack.Pop();
                char lastOpt = optStack.Pop();

                if (lastOpt == '/' && newInteger == 0)
                {
                    throw (new DivideByZeroException("divide by zero"));

                }
                else
                {
                    if (lastOpt == '*') valStack.Push(lastInteger * newInteger);
                    else if (lastOpt == '/') valStack.Push(lastInteger / newInteger);
                }
            }
            else
            {
                valStack.Push(newInteger);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        static void ActTokenMulDiv(String token)
        {
            optStack.Push(token[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        static void ActTokenLeftParanthese(String token)
        {
            optStack.Push(token[0]);
        }

        /// <summary>
        /// calculate the function by using plus or minus,
        /// and if there are no two elements to realize the operator,
        /// throw the exception
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        static void ActTokenAddSub(String token)
        {
            if (optStack.Count > 0 && (optStack.Peek() == '+' || optStack.Peek() == '-'))
            {

                if (valStack.Count < 2)
                    throw (new NullReferenceException("element in Value Stack is less than 2"));
                else
                {
                    int lastInteger1 = valStack.Pop();
                    int lastInteger2 = valStack.Pop();
                    char lastOpt = optStack.Pop();
                    if (lastOpt == '+')
                        valStack.Push(lastInteger2 + lastInteger1);
                    else if (lastOpt == '-')
                        valStack.Push(lastInteger2 - lastInteger1);
                }
            }
            optStack.Push(token[0]);

        }

        /// <summary>
        /// doing the calculate when there is a fully brace
        /// if it divided by zero throw exception
        /// if there are no two elements to realize the calculator throw exception
        /// if there is no left paranthesis throw the exception
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> zero if this method is executed successfully
        static void ActTokenRightParanthese(String token)
        {
            if (optStack.Count > 0 && (optStack.Peek() == '+' || optStack.Peek() == '-'))
            {
                if (valStack.Count < 2)
                    throw (new NullReferenceException("element in Value Stack is less than 2"));
                int integer1 = valStack.Pop();
                int integer2 = valStack.Pop();
                char opt = optStack.Pop();
                if (opt == '+')
                    valStack.Push(integer2 + integer1);
                else if (opt == '-')
                    valStack.Push(integer2 - integer1);
            }

            if (optStack.Count == 0 || (optStack.Count > 0 && optStack.Peek() != '('))
                throw (new NullReferenceException("there is not left Paranthesis"));
            else optStack.Pop();            // pop the left paranthese in the operation stack

            if (optStack.Count > 0 && (optStack.Peek() == '*' || optStack.Peek() == '/'))
            {

                if (valStack.Count < 2)
                    throw (new NullReferenceException("there are less than 2 values in paranthese"));
                int integer1 = valStack.Pop();
                int integer2 = valStack.Pop();
                char opt = optStack.Pop();
                if (opt == '/' && integer1 == 0)
                {

                    throw (new DivideByZeroException("divide by zero"));
                }
                else
                {
                    if (opt == '*') valStack.Push(integer2 * integer1);
                    else if (opt == '/') valStack.Push(integer2 / integer1);
                }
            }


        }

        /// <summary>
        /// finish the function when there is only two elements left in value stack,
        /// and the last operator is add or minus
        /// 
        /// </summary>
        /// <returns></returns> the final number that 
        static int endProcessing()
        {
            int result = (valStack.Count > 0) ? valStack.Peek() : 0; ;
            if (optStack.Count > 0)
            {
                char opt = optStack.Pop();
                int integer1 = valStack.Pop();
                int integer2 = valStack.Pop();
                if (opt == '+')
                    result = integer2 + integer1;
                else if (opt == '-')
                    result = integer2 - integer1;
            }
            return result;
        }

        /// <summary>
        /// calculate the function given by user by using helping method,
        /// and return the currect final result of the function
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns></returns> a number that is a result of expression
        public static int Evaluate(String expression, LookUp variableEvaluator)
        {

            string expressionNoSpace = deleteSpace(expression);
            string[] substrings = Regex.Split(expressionNoSpace, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string substring in substrings)
            {
                if (substring.Length > 0)            // ignored when the substring is an empty
                {

                    if (isTokenInteger(substring))
                        ActTokenInteger(substring);
                    else if (isTokenVariable(substring))
                        ActTokenVariable(substring, variableEvaluator);
                    else if (isTokenMulDiv(substring))
                        ActTokenMulDiv(substring);
                    else if (isTokenLeftParanthese(substring))
                        ActTokenLeftParanthese(substring);
                    else if (isTokenAddSub(substring))
                        ActTokenAddSub(substring);
                    else if (isTokenRightParanthese(substring))
                        ActTokenRightParanthese(substring);



                }
            }



            int result = endProcessing();

            optStack.Clear();                       //  clear the static stack to eliminated the influence of the current expression on the next one
            valStack.Clear();
            return result;
        }
    }

    public static class error
    {

    }
}

