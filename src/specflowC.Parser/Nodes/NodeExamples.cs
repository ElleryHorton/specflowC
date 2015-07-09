using System;
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
            if (Rows == null)
            {
                return -1;
            }
            if (Rows.Count > 1)
            {
                return Array.FindIndex(Rows[0], column => column == parameterName);
            }
            return -1;
        }
    }
}