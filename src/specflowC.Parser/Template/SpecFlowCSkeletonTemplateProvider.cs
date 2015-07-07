using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.BindingSkeletons;

namespace specflowC.Parser
{
    public class SpecFlowCSkeletonTemplateProvider : ResourceSkeletonTemplateProvider, ISkeletonTemplateProvider
    {
        protected override string GetTemplateFileContent()
        {
            var resourceStream = GetType().Assembly.GetManifestResourceStream(string.Format("{0}.TechTalk.SpecFlow.BindingSkeletons.DefaultSkeletonTemplates.sftemplate", GetType().Namespace));
            if (resourceStream == null)
                throw new Exception("Missing resource: DefaultSkeletonTemplates.sftemplate");

            using (var reader = new StreamReader(resourceStream))
                return reader.ReadToEnd();
        }
    }
}
