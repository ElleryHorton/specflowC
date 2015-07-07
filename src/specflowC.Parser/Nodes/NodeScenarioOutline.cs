using System.Collections.Generic;

namespace specflowC.Parser.Nodes
{
    public class NodeScenarioOutline : NodeScenario
    {
        public NodeExamples Examples { get { return _examples; } }

        private NodeExamples _examples = new NodeExamples();

        public NodeScenarioOutline(string name, List<NodeHook> hooks)
            : base(name, hooks)
        {
        }
    }
}