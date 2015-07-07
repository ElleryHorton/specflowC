using System.Collections.Generic;

namespace specflowC.Parser.Nodes
{
    public class NodeScenario
    {
        public string Name { get; set; }

        public List<NodeHook> Hooks { get { return _hooks; } }

        private List<NodeHook> _hooks;

        public List<NodeStep> Steps { get { return _steps; } }

        private List<NodeStep> _steps = new List<NodeStep>();

        public NodeScenario(string name)
        {
            Name = name;
            _hooks = new List<NodeHook>();
        }

        public NodeScenario(string name, List<NodeHook> hooks)
        {
            Name = name;
            _hooks = hooks;
        }
    }
}