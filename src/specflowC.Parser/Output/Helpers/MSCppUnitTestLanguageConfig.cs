namespace specflowC.Parser.Output.Helpers
{
    public class MSCppUnitTestLanguageConfig : UnitTestLanguageConfig
    {
        public MSCppUnitTestLanguageConfig()
        {
            NameSpace = "CppUnitTest";
        }

        private string _featureName;

        public override void SetFeatureName(string featureName)
        {
            _featureName = featureName;
        }

        public override bool UseNamespace { get { return true; } }

        private bool _useInclude = true;

        public override bool UseInclude { get { return _useInclude; } set { _useInclude = value; } }

        public override bool UseHeader { get { return false; } }

        public override string[] includeStatementsInHeader
        {
            get
            {
                return new[] {
                    "#include \"CppUnitTest.h\"",
                    "#include \"CppUnitTestHooks.h\"",
                    "#include \"trim.hpp\"",
                    "#include <vector>"
                };
            }
        }

        public override string[] includeStatementsInScenarios
        {
            get
            {
                return new[] {
                    string.Format("#include \"{0}.h\"", _featureName)
                };
            }
        }

        public override string[] includeStatementsInStepDefinition
        {
            get
            {
                return new[] {
                    string.Format("#include \"{0}.h\"", _featureName)
                };
            }
        }

        public override string[] headerStatementsInStatementsHeader
        {
            get
            {
                return new[] {
                    string.Format("using namespace Microsoft::VisualStudio::CppUnitTestFramework;"),
                    string.Format("using namespace std;")
                };
            }
        }

        public override string[] headerStatementsInScenarios
        {
            get { return new string[] { }; }
        }

        public override string[] headerStatementsInStepDefinition
        {
            get { return new string[] { }; }
        }

        public override string[] footerStatementsInStatementsHeader
        {
            get { return new string[] { }; }
        }

        public override string[] footerStatementsInScenarios
        {
            get { return new string[] { }; }
        }

        public override string[] footerStatementsInStepDefinition
        {
            get { return new string[] { }; }
        }

        public override string FeatureClassDeclaration
        {
            get { return string.Format("TEST_CLASS({0})", _featureName); }
        }

        public override string ScenarioMethodDeclaration(string attributeName, string scenarioName)
        {
            if (attributeName == string.Empty)
            {
                return string.Format("TEST_METHOD({0})", scenarioName);
            }
            else
            {
                return string.Format("TEST_METHOD_HOOK{0}({1})", attributeName.ToUpper(), scenarioName);
            }
        }

        public override string StepMethodDeclaration(string stepName, string parameterString)
        {
            return string.Format("void {0}", StepMethod(stepName, parameterString));
        }

        public override string StepMethod(string stepName, string parameterString)
        {
            return string.Format("{0}({1})", stepName, parameterString);
        }

        public override string FeatureClassAttribute(string attributeName)
        {
            return string.Empty;
        }

        public override string FeatureClassInnerAttribute(string attributeName)
        {
            return string.Format("TEST_CLASS_HOOK_{0}()", attributeName.ToUpper());
        }

        public override string ScenarioMethodAttribute(string attributeName, string scenarioName)
        {
            return string.Empty;
        }

        public override string StepMethodAttribute(string attributeName, string stepName)
        {
            return string.Empty;
        }

        public override string FeatureClassInitialization { get { return string.Empty; } }

        public override string PublicDeclaration { get { return "public:"; } }

        public override string PrivateDeclaration { get { return "private:"; } }

        public override string ErrorTableParse { get { return "Assert::Fail(L\"PARSE ERROR: Table is uneven\");"; } }

        public override string PendingStepDeclaration { get { return "Assert::Fail(L\"Pending implementation...\");"; } }

        public override string TableDeclaration { get { return "std::vector<std::vector<std::string>> table, int rows, int cols"; } }

        public override string TableImplementationOpen(int tableNumber) { return string.Format("std::vector<std::vector<std::string>> table{0} = {{{{", tableNumber); }

        public override string TableImplementationClose { get { return "}};"; } }
    }
}