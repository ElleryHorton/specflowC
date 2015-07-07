using BoDi;
using System.CodeDom;
using System.Collections.Generic;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.Configuration;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.UnitTestProvider;
using TechTalk.SpecFlow.Utils;

[assembly: GeneratorPlugin(typeof(NullUnitTestGeneratorProvider.SpecflowPlugin.NullUnitTestGeneratorPlugin))]

namespace NullUnitTestGeneratorProvider.SpecflowPlugin
{
    public class NullUnitTestGeneratorPlugin : IGeneratorPlugin
    {
        public void RegisterDependencies(ObjectContainer container)
        {
        }

        public void RegisterCustomizations(ObjectContainer container, SpecFlowProjectConfiguration generatorConfiguration)
        {
            container.RegisterTypeAs<NullUnitTestGeneratorProvider, IUnitTestGeneratorProvider>();
            container.RegisterTypeAs<MsTest2010RuntimeProvider, IUnitTestRuntimeProvider>();
        }

        public void RegisterConfigurationDefaults(SpecFlowProjectConfiguration specFlowConfiguration)
        {
        }
    }

    public class NullUnitTestGeneratorProvider : IUnitTestGeneratorProvider
    {
        public bool SupportsRowTests { get { return false; } }
        public bool SupportsAsyncTests { get { return false; } }

        // IUnitTestGeneratorProvider
        public NullUnitTestGeneratorProvider(CodeDomHelper codeDomHelper)
        {
        }

        public void SetTestClass(TestClassGenerationContext generationContext, string featureTitle, string featureDescription)
        {
        }

        public void SetTestClassCategories(TestClassGenerationContext generationContext, IEnumerable<string> featureCategories)
        {
        }

        public void SetTestClassIgnore(TestClassGenerationContext generationContext)
        {
        }

        public virtual void FinalizeTestClass(TestClassGenerationContext generationContext)
        {
        }

        public void SetTestClassInitializeMethod(TestClassGenerationContext generationContext)
        {
        }

        public void SetTestClassCleanupMethod(TestClassGenerationContext generationContext)
        {
        }


        public void SetTestInitializeMethod(TestClassGenerationContext generationContext)
        {
        }

        public void SetTestCleanupMethod(TestClassGenerationContext generationContext)
        {
        }


        public void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
        }

        public void SetTestMethodCategories(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, IEnumerable<string> scenarioCategories)
        {
        }

        public void SetTestMethodIgnore(TestClassGenerationContext generationContext, CodeMemberMethod testMethod)
        {
        }


        public void SetRowTest(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
        }

        public void SetRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, IEnumerable<string> arguments, IEnumerable<string> tags, bool isIgnored)
        {
        }

        public void SetTestMethodAsRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle, string exampleSetName, string variantName, IEnumerable<KeyValuePair<string, string>> arguments)
        {
        }
    }
}