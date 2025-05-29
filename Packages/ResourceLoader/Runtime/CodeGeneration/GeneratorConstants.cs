using System.Collections.Generic;
using System.Text;

namespace CodeGeneration
{
    public class GeneratorConstantsClass
    {
        private const string TemplateNamespace = "namespace #class_namespace#\n";
        private const string TemplateBody = "\tpublic static class #class_name#\n\t{\n#body_constants#\n\t}\n";
        private const string TemplateRecord = "\t\tpublic const string {0} = \"{1}\";";
        private const string NewLine = "\n";
        private const string KeyClassName = "#class_name#";
        private const string KeyClassNamespace = "#class_namespace#";
        private const string KeyBodyConstants = "#body_constants#";

        public string Generate(ContextGenerationConstant context)
        {
            var sb = new StringBuilder();

            foreach (KeyValuePair<string, string> contextConstant in context.Constants)
            {
                sb.AppendFormat(TemplateRecord, contextConstant.Key, contextConstant.Value);
                sb.Append(NewLine);
            }

            var bodyBuffer = new StringBuilder(TemplateNamespace);
            bodyBuffer.AppendLine("{");
            bodyBuffer.AppendLine(TemplateBody);
            bodyBuffer.Replace(KeyClassName, context.ClassName);
            bodyBuffer.Replace(KeyClassNamespace, context.Namespace);
            bodyBuffer.Replace(KeyBodyConstants, sb.ToString());
            bodyBuffer.AppendLine("}");

            return bodyBuffer.ToString();
        }
    }
}