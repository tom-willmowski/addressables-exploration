using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;

namespace Clash.Editor
{
    [CreateAssetMenu]
    public class AddressablesBuildScript : ScriptableObject
    {
        public static void Build()
        {
            Build(new BuildOptions()
            {
                BuildPath = "",
                Dev = true,
                Config = "main-dev",
                VersionCode = 1,
                BuildAddressables = true,
                UseAAB = false,
                IsPlayground = true,
                UseRemoteConsole = false
            });
        }
        
        public static void UpdateBuild()
        {
            string statePath = EditorUtility.OpenFilePanel("select addressables state path", "Assets/", "bin");
            //var changes = ContentUpdateScript.GatherModifiedEntries(AddressableAssetSettingsDefaultObject.Settings, statePath);
            //ContentUpdateScript.CreateContentUpdateGroup(AddressableAssetSettingsDefaultObject.Settings, changes, "update");
            ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, statePath);
        }
        
        public static void Build(BuildOptions options)
        {
            if (!options.BuildAddressables.Value)
            {
                return;
            }
            AddressableAssetSettingsDefaultObject.Settings.activeProfileId = 
                AddressableAssetSettingsDefaultObject.Settings.profileSettings
                    .GetProfileId(options.IsPlayground.Value ? 
                    "Default" : 
                    "Remote");
            
            BuildCache.PurgeCache(false);
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent(out var result);
            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.LogError(result.Error);
            }
        }
    }
}