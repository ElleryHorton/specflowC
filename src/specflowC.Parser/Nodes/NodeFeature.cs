using System.Collections.Generic;

namespace specflowC.Parser.Nodes
{
    public class NodeFeature
    {
        public string Name { get; set; }

        public List<NodeHook> Hooks { get { return _hooks; } }

        private List<NodeHook> _hooks;

        public List<NodeScenario> Scenarios { get { return _scenarios; } }

        private List<NodeScenario> _scenarios = new List<NodeScenario>();

        public NodeFeature(string name)
        {
            Name = name;
            _hooks = new List<NodeHook>();
        }

        public NodeFeature(string name, List<NodeHook> hooks)
        {
            Name = name;
            _hooks = hooks;
        }
    }
}