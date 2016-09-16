using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHang.Utils
{
    public static class StringUtils
    {
        public static bool HasLength(this string target)
        {
            return (target != null && target.Length > 0);
        }

        public static bool HasText(this string target)
        {
            if (target == null)
            {
                return false;
            }
            else
            {
                return HasLength(target.Trim());
            }
        }

        public static string[] Split(
            string s, string delimiters, bool trimTokens, bool ignoreEmptyTokens, string quoteChars)
        {
            if (s == null)
            {
                return new string[0];
            }
            if (string.IsNullOrEmpty(delimiters))
            {
                return new string[] { s };
            }
            if (quoteChars == null)
            {
                quoteChars = string.Empty;
            }

            char[] delimiterChars = delimiters.ToCharArray();

            // scan separator positions
            int[] delimiterPositions = new int[s.Length];
            int count = MakeDelimiterPositionList(s, delimiterChars, quoteChars, delimiterPositions);

            List<string> tokens = new List<string>(count + 1);
            int startIndex = 0;
            for (int ixSep = 0; ixSep < count; ixSep++)
            {
                string token = s.Substring(startIndex, delimiterPositions[ixSep] - startIndex);
                if (trimTokens)
                {
                    token = token.Trim();
                }
                if (!(ignoreEmptyTokens && token.Length == 0))
                {
                    tokens.Add(token);
                }
                startIndex = delimiterPositions[ixSep] + 1;
            }
            // add remainder            
            if (startIndex < s.Length)
            {
                string token = s.Substring(startIndex);
                if (trimTokens)
                {
                    token = token.Trim();
                }
                if (!(ignoreEmptyTokens && token.Length == 0))
                {
                    tokens.Add(token);
                }
            }
            else if (startIndex == s.Length)
            {
                if (!(ignoreEmptyTokens))
                {
                    tokens.Add(string.Empty);
                }
            }

            return tokens.ToArray();
        }

        private static int MakeDelimiterPositionList(string s, char[] delimiters, string quoteChars, int[] delimiterPositions)
        {
            int count = 0;
            int quoteNestingDepth = 0;
            char expectedQuoteOpenChar = '\0';
            char expectedQuoteCloseChar = '\0';

            for (int ixCurChar = 0; ixCurChar < s.Length; ixCurChar++)
            {
                char curChar = s[ixCurChar];

                for (int ixCurDelim = 0; ixCurDelim < delimiters.Length; ixCurDelim++)
                {
                    if (delimiters[ixCurDelim] == curChar)
                    {
                        if (quoteNestingDepth == 0)
                        {
                            delimiterPositions[count] = ixCurChar;
                            count++;
                            break;
                        }
                    }

                    if (quoteNestingDepth == 0)
                    {
                        // check, if we're facing an opening char
                        for (int ixCurQuoteChar = 0; ixCurQuoteChar < quoteChars.Length; ixCurQuoteChar += 2)
                        {
                            if (quoteChars[ixCurQuoteChar] == curChar)
                            {
                                quoteNestingDepth++;
                                expectedQuoteOpenChar = curChar;
                                expectedQuoteCloseChar = quoteChars[ixCurQuoteChar + 1];
                                break;
                            }
                        }
                    }
                    else
                    {
                        // check if we're facing an expected open or close char
                        if (curChar == expectedQuoteOpenChar)
                        {
                            quoteNestingDepth++;
                        }
                        else if (curChar == expectedQuoteCloseChar)
                        {
                            quoteNestingDepth--;
                        }
                    }
                }
            }
            return count;
        }
    }
}