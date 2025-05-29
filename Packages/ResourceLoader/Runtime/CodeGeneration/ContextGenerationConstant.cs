using System.Collections.Generic;

namespace CodeGeneration
{
    public class ContextGenerationConstant
    {
        public string ClassName { get; }
        public string Namespace { get; }
        public Dictionary<string, string> Constants { get; } = new();

        public ContextGenerationConstant(string className, string ns)
        {
            ClassName = className;
            Namespace = ns;
        }

        public void SetConstants(Dictionary<string, string> constants)
        {
            Constants.Clear();

            foreach (KeyValuePair<string, string> keyValuePair in constants)
            {
                Constants.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void AddConstant(string key, string value)
        {
            Constants[key] = value;
        }
    }
}