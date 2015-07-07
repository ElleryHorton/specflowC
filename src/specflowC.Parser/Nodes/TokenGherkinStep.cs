using System.Collections.Generic;

namespace specflowC.Parser.Nodes
{
    public class TokenGherkinStep
    {
        public string MethodName;
        public List<string> ParameterTokens = new List<string>();
    }
}