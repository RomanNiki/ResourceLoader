using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeGeneration
{
    public class ConstantKeyBuilder
    {
        private readonly OptionsConstantKeyBuilder _optionsConstantKeyBuilder;

        public ConstantKeyBuilder(OptionsConstantKeyBuilder optionsConstantKeyBuilder)
        {
            _optionsConstantKeyBuilder = optionsConstantKeyBuilder;
        }

        public Dictionary<string, string> BuildConstants(IEnumerable<string> values)
        {
            var result = new Dictionary<string, string>();

            foreach (string key in values)
            {
                string keyConstant = BuildKey(key);

                if (string.IsNullOrEmpty(keyConstant))
                {
                    continue;
                }

                result[keyConstant] = key;
            }

            return result;
        }

        private string BuildKey(string key)
        {
            string[] parts = Regex.Split(key, _optionsConstantKeyBuilder.splitPattern);
            var result = new StringBuilder();

            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part))
                {
                    continue;
                }

                char first = char.ToUpper(part[0]);
                result.Append(first);

                if (part.Length == 1)
                {
                    continue;
                }

                string last = part.Substring(1, part.Length - 1);
                result.Append(last);
            }

            return result.ToString();
        }
    }
}