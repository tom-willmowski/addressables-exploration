using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEngine;

namespace UnityEditor.Build.Pipeline.Tasks
{
    public class ChangeLineEndings : IBuildTask
    {
        [InjectContext(ContextUsage.In)]
        IBuildContent m_Content;
        
        public int Version => 1;
        
        private static string[] filesToConvert = new[]
        {
            ".cs",
            ".prefab",
            ".scene",
            ".mat",
            ".shader",
            ".asset",
            ".controller",
            ".anim",
            ".overrideController",
            ".mixer",
            ".inputactions",
            ".physicMaterial"
        };
        
        private static void Convert(string path)
        {
            if (!filesToConvert.Contains(Path.GetExtension(path)))
            {
                return;
            }
            var text = File.ReadAllText(path);
            if (!text.Contains("\r\n"))
            {
                return;
            }
            text = text.Replace("\r\n", "\n");
            File.WriteAllText(path, text);
            Debug.Log($"converting to unix line endings {path}");
        }
        
        public ReturnCode Run()
        {
            foreach (var asset in m_Content.Assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                foreach (var dependency in AssetDatabase.GetDependencies(path))
                {
                    Convert(dependency);
                }
                Convert(path);
            }
            return ReturnCode.Success;
        }

        public static void Run(List<string> assets)
        {
            foreach (var asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                foreach (var dependency in AssetDatabase.GetDependencies(path))
                {
                    Convert(dependency);
                }
                Convert(path);
            }
        }

        public static void RunForPaths(List<string> paths)
        {
            foreach (var path in paths)
            {
                foreach (var dependency in AssetDatabase.GetDependencies(path))
                {
                    Convert(dependency);
                }
                Convert(path);
            }
        }
    }
}