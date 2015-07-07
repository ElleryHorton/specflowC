using specflowC.Parser.Nodes;
using specflowC.Parser.Output;
using specflowC.Parser.Output.Helpers;
using System;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests
{
    public enum GeneratorType
    {
        InputGenerator, HeaderGenerator, CodeBehindGenerator, StepDefinitionGenerator
    }

    public static class GeneratorFactory
    {
        static public IList<string[]> Generate(GeneratorType type, IList<NodeFeature> features)
        {
            IGenerate generator;
            switch (type)
            {
                case GeneratorType.HeaderGenerator:
                    generator = new HeaderGenerator();
                    break;

                case GeneratorType.CodeBehindGenerator:
                    generator = new CodeBehindGenerator();
                    break;

                case GeneratorType.StepDefinitionGenerator:
                    generator = new StepDefinitionGenerator();
                    break;

                default:
                    throw new NotImplementedException();
            }
            return generator.Generate(new MSCppUnitTestLanguageConfig(), features);
        }
    }
}