using System.Collections.Generic;

namespace specflowC.Parser.Nodes
{
    public class NodeExamples
    {
        public List<string[]> Rows { get; set; }

        public NodeExamples()
        {
            Rows = new List<string[]>();
        }

        public int GetIndexOfParameter(string parameterName)
        {
            if (Rows.Count > 1)
            {
                for (int i = 0; i < Rows[0].Length; i++)
                {
                    if (Rows[0][i] == parameterName)
                        return i;
                }
            }
            return -1;
        }
    }
}