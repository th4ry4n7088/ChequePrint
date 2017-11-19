using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ChequePrint.Services
{
    public interface IPrinter
    {
        /// <summary>
        /// Returns the word version of a given amount
        /// </summary>
        /// <param name="amount">The amount to be converted. Shouldn't be more than 999999999999.99</param>
        /// <returns>The word version of the amount</returns>
        string PrintChequeAmount(decimal amount);
    }

    /// <summary>
    /// The main service
    /// </summary>
    public class Printer : IPrinter
    {
        // collection of text for numbers under 20
        private readonly string[] _less20 = { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };

        // collections of text for ten numbers
        private readonly string[] _tens = { "", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

        public string PrintChequeAmount(decimal amount)
        {
            // Making sure that the amount 
            if(amount > 999999999999.99m)
            {
                throw new ApplicationException("Only amount up to maximum 999,999,999,999.99 is allowed");
            }

            // Get the dollar amount and the cents amount
            string[] wholeNumbers = amount.ToString("F2").Split('.');

            StringBuilder sb = new StringBuilder();

            // Get the word version of the dollar amount
            sb.AppendFormat("{0} DOLLARS", PrintWholeNumber(decimal.Parse(wholeNumbers[0])));

            // If there there's cent amount
            if (wholeNumbers.Length > 1)
            {
                decimal cents = Math.Round(decimal.Parse(wholeNumbers[1]), 2);

                // Get the word version of the cents amount if it's more than 0
                if(cents > 0)
                    sb.AppendFormat(" AND {0} CENTS", PrintWholeNumber(cents));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Process the whole number part of the amount
        /// </summary>
        /// <param name="number">The whole number</param>
        /// <returns>The word version of the whole number</returns>
        private string PrintWholeNumber(decimal number)
        {
            // If the number is 0, return it straight away
            if (number == 0)
                return "ZERO";

            // First, convert the number into a string
            string strNumber = number.ToString();
            List<string> numberBlocks = new List<string>();

            // Break the number string into group of three, that represents the group of thousands
            // Add the group into a list of string, in reverse order
            do
            {
                if (strNumber.Length > 3)
                {
                    numberBlocks.Add(strNumber.Substring(strNumber.Length - 3));
                    strNumber = strNumber.Substring(0, strNumber.Length - 3);
                }
                else
                {
                    numberBlocks.Add(strNumber);
                    strNumber = "";
                }
            } while (strNumber.Length > 0);

            // Convert each thousand block into words and add it into the list of string, along with the "thousand word indicator"
            List<string> printedNumberBlocks = new List<string>();
            for (int i = numberBlocks.Count - 1; i >= 0; i--)
            {
                if(int.Parse(numberBlocks[i]) > 0)
                    printedNumberBlocks.Add(string.Format("{0}{1}", PrintNumberBlock(numberBlocks[i]), GetThousand(i)));
            }

            // Concantenate each entry of the list into a string
            return printedNumberBlocks.Aggregate((a,b) => string.Format("{0}{1}{2}", a, !string.IsNullOrEmpty(b) ? " " : "", b));
        }

        /// <summary>
        /// Process each group of three numbers
        /// </summary>
        /// <param name="numberBlock">string containing a group of three numbers</param>
        /// <returns>the word version the number</returns>
        private string PrintNumberBlock(string numberBlock)
        {
            StringBuilder sb = new StringBuilder();

            // Get the hundred part of the number
            if (numberBlock.Length == 3)
            {
                int firstNumber = int.Parse(numberBlock.Substring(0, 1));

                // Convert into word if the number if greater than 0
                if (firstNumber > 0)
                    sb.Append(_less20[firstNumber] + " HUNDRED");

                numberBlock = numberBlock.Substring(1);
            }

            // Get the remaining number
            int remainingNumber = int.Parse(numberBlock);

            // Process if the remaining number is greater than 0
            if(remainingNumber > 0)
            {
                if(sb.Length > 0)
                    sb.Append(" AND ");

                if (remainingNumber < 20)   // If the remaining number is less than 20
                    sb.Append(_less20[remainingNumber]);
                else // If it's greater than or equal to 20
                {
                    int ten = int.Parse(numberBlock.Substring(0, 1));
                    int one = int.Parse(numberBlock.Substring(1));

                    // Convert the tens number
                    sb.Append(_tens[ten]);

                    // Convert the ones number if it's greater than zero
                    if (one > 0)
                    {
                        sb.Append("-");
                        sb.Append(_less20[one]);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the "thousand indicator" string based on a given index
        /// </summary>
        /// <param name="index">The </param>
        /// <returns></returns>
        private string GetThousand(int index)
        {
            switch (index)
            {
                case 0:
                    return "";
                case 1:
                    return " THOUSAND";
                case 2:
                    return " MILLION";
                case 3:
                    return " BILLION";
                default:
                    return "";
            }
        }
    }
}