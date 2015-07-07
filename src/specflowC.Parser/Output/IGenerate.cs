using specflowC.Parser.Nodes;
using specflowC.Parser.Output.Helpers;
using System.Collections.Generic;

namespace specflowC.Parser.Output
{
    public interface IGenerate
    {
        List<string[]> Generate(UnitTestLanguageConfig langConfig, IList<NodeFeature> features);
    }

    public abstract class Generator : IGenerate
    {
        protected List<string> Contents { get; set; }

        protected UnitTestLanguageConfig LanguageConfig { get; set; }

        public List<string[]> Generate(UnitTestLanguageConfig langConfig, IList<NodeFeature> features)
        {
            LanguageConfig = langConfig;
            var listOfFileContents = new List<string[]>();
            foreach (var feature in features)
            {
                Contents = new List<string>();
                LanguageConfig.SetFeatureName(feature.Name);
                listOfFileContents.Add(BuildContents(feature));
            }
            return listOfFileContents;
        }

        protected abstract string[] BuildContents(NodeFeature feature);
    }
}