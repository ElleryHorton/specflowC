using specflowC.Parser.Enums;
using specflowC.Parser.Nodes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace specflowC.Parser
{
    public static class GherkinParser
    {
        public static string DetermineLabel(string str)
        {
            if (str.StartsWith(EnumNames.hook))
                return EnumNames.hook;

            if (str.StartsWith(EnumNames.Feature))
                return EnumNames.Feature;

            // check before checking scenario
            if (str.StartsWith(EnumNames.ScenarioOutline))
                return EnumNames.ScenarioOutline;

            if (str.StartsWith(EnumNames.Scenario))
                return EnumNames.Scenario;

            if (str.StartsWith(EnumNames.Examples))
                return EnumNames.Examples;

            if (str.StartsWith(EnumNames.slim))
                return EnumNames.slim;

            if (str.StartsWith(EnumNames.Given))
                return EnumNames.Given;

            if (str.StartsWith(EnumNames.When))
                return EnumNames.When;

            if (str.StartsWith(EnumNames.Then))
                return EnumNames.Then;

            if (str.StartsWith(EnumNames.And))
                return EnumNames.And;

            return string.Empty;
        }

        public static string DetermineParameters(string str)
        {
            if (str.Contains(EnumNames.tick))
                return EnumNames.tick;

            if (str.Contains(EnumNames.quote))
                return EnumNames.quote;

            return string.Empty;
        }

        public static string ParseNameWithLabel(string str, string label)
        {
            string sub = str.Replace(label + EnumNames.labeltag, string.Empty);
            return CapitalCamelCase(sub);
        }

        public static string ParseMethodName(string str)
        {
            return CapitalCamelCase(str);
        }

        private static string CapitalCamelCase(string str)
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            string capital = textInfo.ToTitleCase(str);
            return ReplaceInvalidCharacters(RemoveWhiteSpace(capital));
        }

        public static string RemoveWhiteSpace(string str)
        {
            return Regex.Replace(str, @"\s+", string.Empty);
        }

        private static string ReplaceInvalidCharacters(string str)
        {
            var trimmedStr = Regex.Replace(str, @"[^a-zA-Z0-9]", string.Empty);
            return Regex.Replace(trimmedStr, @"^([0-9])", "_");
        }

        public static TokenGherkinStep ParseOutStepAndParameters(string gherkinStep)
        {
            if (SingleQuoteCountIsNotEven(gherkinStep))
            {
                return null;
            }
            if (ParameterBracketCountIsNotEven(gherkinStep))
            {
                return null;
            }

            if (gherkinStep.Contains(EnumNames.tick))
            {
                return TokenizeGherkinStep(gherkinStep, EnumNames.tick, EnumNames.tick, false);
            }
            else
            {
                return ParameterWasNotEscapedBySingleQuote(gherkinStep);
            }
        }

        private static bool SingleQuoteCountIsNotEven(string str)
        {
            return (str.Split(EnumNames.tick.ToCharArray()).Length - 1) % 2 != 0;
        }

        private static bool ParameterBracketCountIsNotEven(string gherkinStep)
        {
            return gherkinStep.Split(EnumNames.tickOpen.ToCharArray()).Length != gherkinStep.Split(EnumNames.tickClose.ToCharArray()).Length;
        }

        private static TokenGherkinStep ParameterWasNotEscapedBySingleQuote(string gherkinStep)
        {
            TokenGherkinStep tokens;
            if (gherkinStep.Contains(EnumNames.tickOpen))
            {
                return TokenizeGherkinStep(gherkinStep, EnumNames.tickOpen, EnumNames.tickClose, true);
            }

            tokens = new TokenGherkinStep();
            tokens.MethodName = CapitalCamelCase(gherkinStep);
            tokens.ParameterTokens = new List<string>();
            return tokens;
        }

        private static TokenGherkinStep TokenizeGherkinStep(string gherkinStep, string delimiterOpen, string delimiterClose, bool includeDelimits)
        {
            var sentenceTokens = new List<string>();
            var parameterTokens = new List<string>();
            int posStart = 0;
            bool isOpenTick = false;
            while (posStart >= 0 && posStart < gherkinStep.Length)
            {
                int posTick = isOpenTick ? FindPositionOfTick(gherkinStep, posStart, delimiterClose) : FindPositionOfTick(gherkinStep, posStart, delimiterOpen);

                if (!isOpenTick)
                {
                    int length = posTick - posStart;
                    if (length > 0) // no sentence token if parameters are back to back (it's an empty string)
                    {
                        if (includeDelimits)
                        {
                            posStart = Math.Max(posStart - delimiterOpen.Length, 0);
                            sentenceTokens.Add(CapitalCamelCase(gherkinStep.Substring(posStart, (posTick - posStart) - delimiterOpen.Length)));
                        }
                        else
                        {
                            sentenceTokens.Add(CapitalCamelCase(gherkinStep.Substring(posStart, posTick - posStart)));
                        }
                    }
                }
                else
                {
                    if (includeDelimits)
                    {
                        parameterTokens.Add(gherkinStep.Substring(posStart - delimiterOpen.Length, (posTick - posStart) + (delimiterOpen.Length + delimiterClose.Length)));
                    }
                    else
                    {
                        parameterTokens.Add(gherkinStep.Substring(posStart, posTick - posStart));
                    }
                }
                posStart = posTick + (isOpenTick ? delimiterClose.Length : delimiterOpen.Length);
                isOpenTick = !isOpenTick;
            }
            TokenGherkinStep tokens = new TokenGherkinStep();
            tokens.MethodName = string.Join(string.Empty, sentenceTokens);
            tokens.ParameterTokens = parameterTokens;
            return tokens;
        }

        private static int FindPositionOfTick(string gherkinStep, int posStart, string delimiter)
        {
            int posTick = gherkinStep.IndexOf(delimiter, posStart);
            if (posTick < 0) // go till end of string
            {
                posTick = gherkinStep.Length;
            }
            return posTick;
        }

        public static List<NodeHook> GetHooks(string str)
        {
            List<NodeHook> list = new List<NodeHook>();
            var hooks = RemoveWhiteSpace(str).Split(EnumNames.hook.ToCharArray());
            foreach (var hook in hooks.Distinct().Where(strHookName => strHookName != string.Empty))
            {
                list.Add(new NodeHook(hook));
            }
            return list;
        }
    }
}