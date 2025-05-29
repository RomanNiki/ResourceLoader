#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace CodeGeneration
{
    [Serializable]
    public class ConstantGeneratorKeys
    {
        public string pathSaveFile;
        public string nameGenerateClass;
        public string namespaceName = "";
        public string fileExtension = ".cs";
        public OptionsConstantKeyBuilder optionsBuilder;
        
        public void GenerateConstantFile(IEnumerable<string> keys)
        {
            string pathSave = BuildSavePath();

            var generator = new GeneratorConstantsClass();

            ContextGenerationConstant context = GenerateContext(keys);
            string fileText = generator.Generate(context);

            File.WriteAllText(pathSave, fileText);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private string BuildSavePath()
        {
            string fileName = nameGenerateClass + fileExtension;

            string pathSave = Path.Combine(pathSaveFile, fileName);
            if (!Directory.Exists(pathSaveFile))
            {
                Directory.CreateDirectory(pathSaveFile);
            }

            return pathSave;
        }

        private ContextGenerationConstant GenerateContext(IEnumerable<string> keys)
        {
            var context = new ContextGenerationConstant(nameGenerateClass, namespaceName);
            context.SetConstants(BuildConstants(keys));
            return context;
        }

        private Dictionary<string, string> BuildConstants(IEnumerable<string> keys)
        {
            var builder = new ConstantKeyBuilder(optionsBuilder);
            return builder.BuildConstants(keys);
        }
    }
}
#endif