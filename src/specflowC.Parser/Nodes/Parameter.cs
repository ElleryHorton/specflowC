namespace specflowC.Parser.Nodes
{
    public class Parameter
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsFromExampleTable { get; set; }
    }
}