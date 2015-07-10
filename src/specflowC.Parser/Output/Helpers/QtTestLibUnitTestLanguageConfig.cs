namespace specflowC.Parser.Output.Helpers
{
    public class QtTestLibUnitTestLanguageConfig : UnitTestLanguageConfig
    {
        public QtTestLibUnitTestLanguageConfig()
        {
            NameSpace = string.Empty;
        }

        private string _featureName;

        public override void SetFeatureName(string featureName)
        {
            _featureName = featureName;
        }

        public override bool UseNamespace { get { return false; } }

        private bool _useInclude = true;

        public override bool UseInclude { get { return _useInclude; } set { _useInclude = value; } }

        public override bool UseHeader { get { return true; } }

        public override string[] includeStatementsInHeader
        {
            get
            {
                return new[] {
                    "#include <QtTest>",
                    "#include <QString>",
                    "#include <QStringList>",
                    "#include <QVector>"
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
            get { return new string[] { }; }
        }

        public override string[] headerStatementsInScenarios
        {
            get { return new string[] { }; }
        }

        public override string[] headerStatementsInStepDefinition
        {
            get { return new string[] {
                string.Format("{0}::{0}()", _featureName),
                "{",
                "}"
            }; }
        }

        public override string[] footerStatementsInStatementsHeader
        {
            get { return new string[] {
                string.Format("QTEST_APPLESS_MAIN({0})", _featureName)
            }; }
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
            get { return string.Format("class {0} : public QObject", _featureName); }
        }

        public override string ScenarioMethodDeclaration(string attributeName, string scenarioName)
        {
            if (attributeName == string.Empty)
            {
                return string.Format("{0}()", scenarioName);
            }
            else
            {
                return string.Format("{0}()", scenarioName, attributeName.ToUpper());
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
            return string.Format("{0}();", _featureName);
        }

        public override string ScenarioMethodAttribute(string attributeName, string scenarioName)
        {
            return string.Empty;
        }

        public override string StepMethodAttribute(string attributeName, string stepName)
        {
            return string.Empty;
        }

        public override string FeatureClassInitialization
        {
            get
            {
                return "Q_OBJECT";
            }
        }

        public override string PublicDeclaration { get { return "public:"; } }

        public override string PrivateDeclaration { get { return "private Q_SLOTS:"; } }

        public override string ErrorTableParse { get { return "QFAIL(\"PARSE ERROR: Table is uneven\");"; } }

        public override string PendingStepDeclaration { get { return "QWARN(\"Pending implementation...\");"; } }

        public override string TableDeclaration { get { return "QVector<QStringList> table, int rows, int cols"; } }

        public override string TableImplementationOpen { get { return "QVector<QStringList> table = {"; } }

        public override string TableImplementationClose { get { return "};"; } }
    }
}