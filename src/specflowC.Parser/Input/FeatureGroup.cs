using specflowC.Parser.Nodes;
using System.Collections.Generic;

namespace specflowC.Parser
{
    public class FeatureGroup
    {
        public string FeatureName;

        public List<NodeStep> Steps = new List<NodeStep>();
    }
}