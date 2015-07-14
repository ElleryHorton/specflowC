namespace specflowC.Parser.Output.Helpers
{
    public abstract class UnitTestLanguageConfig
    {
        // all generators
        public string NameSpace { get; set; }

        public abstract void SetFeatureName(string featureName);

        public abstract bool UseNamespace { get; }

        public abstract bool UseInclude { get; set; }

        public abstract bool UseHeader { get; }

        public abstract string[] includeStatementsInHeader { get; }

        public abstract string[] includeStatementsInScenarios { get; }

        public abstract string[] includeStatementsInStepDefinition { get; }

        public abstract string[] headerStatementsInStatementsHeader { get; }

        public abstract string[] headerStatementsInScenarios { get; }

        public abstract string[] headerStatementsInStepDefinition { get; }

        public abstract string[] footerStatementsInStatementsHeader { get; }

        public abstract string[] footerStatementsInScenarios { get; }

        public abstract string[] footerStatementsInStepDefinition { get; }

        // header formatting (general)
        public abstract string FeatureClassDeclaration { get; }

        public abstract string ScenarioMethodDeclaration(string attributeName, string scenarioName);

        public abstract string StepMethodDeclaration(string stepName, string parameterString);

        // attributes or categories
        public abstract string FeatureClassAttribute(string attributeName);

        public abstract string FeatureClassInnerAttribute(string attributeName);

        public abstract string ScenarioMethodAttribute(string attributeName, string scenarioName);

        public abstract string StepMethodAttribute(string attributeName, string stepName);

        // extra header formatting (unit test framwork specific)
        public abstract string FeatureClassInitialization { get; }

        public abstract string PublicDeclaration { get; }

        public abstract string PrivateDeclaration { get; }

        // stepDefinition
        public abstract string StepMethod(string stepName, string parameterString);

        public abstract string ErrorTableParse { get; }

        public abstract string PendingStepDeclaration { get; }

        // tables
        public abstract string TableDeclaration { get; }

        public abstract string TableImplementationOpen(int tableNumber);

        public abstract string TableImplementationClose { get; }

    }
}