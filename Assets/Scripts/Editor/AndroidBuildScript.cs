using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using UnityEditor;
using UnityEngine;

namespace Clash.Editor
{
    public class AndroidBuildScript
    {
        [MenuItem("CNXT/Build Android")]
        public static void BuildManual()
        {
            string buildPath = EditorUtility.SaveFilePanel("APK", "./", "cnxt", "apk");
            if (buildPath == string.Empty){
                Debug.LogError($"You need to provide a path to output the build in.");
                return;
            }
            bool isDev = EditorUtility.DisplayDialog("Build", "Is it development build?", "yes", "no");
            bool isPlayground = EditorUtility.DisplayDialog("Build", "Is it playground build?", "yes", "no");

            Build(new BuildOptions()
            {
                BuildPath = buildPath,
                Dev = isDev,
                Config = "main-dev",
                VersionCode = 1,
                BuildAddressables = true,
                UseAAB = false,
                IsPlayground = isPlayground,
                UseRemoteConsole = false
            });
        }

        public static void Build()
        {
            string[] args = Environment.GetCommandLineArgs();
            int executeMethodIndex = Array.IndexOf(args, "Clash.Editor.AndroidBuildScript.Build");
            Build(Environment.GetCommandLineArgs().Skip(executeMethodIndex).ToArray());
        }

        public static void Build(string[] args)
        {
            Parser p = new Parser(with =>
            {
                with.EnableDashDash = true;
                with.IgnoreUnknownArguments = true;
            });
            var result = p.ParseArguments<BuildOptions>(args)
                .WithParsed(Build);
            if (result.Tag == ParserResultType.Parsed)
            {
                return;
            }
            HelpText helpText = HelpText.AutoBuild(result, c => HelpText.DefaultParsingErrorsHandler(result, c));
            Debug.LogError(helpText.ToString());
        }

        public static void Build(BuildOptions o)
        {
            AddressablesBuildScript.Build(o);
            Build(o.BuildPath, GetBuildOptions(o));
        }

        private static void Build(string buildPath, UnityEditor.BuildOptions options)
        {
            if (buildPath == null)
            {
                throw new Exception("build path is null");
            }
            var buildReport = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, BuildTarget.Android, options);
            Debug.Log($"Logging Summary: success state: {buildReport.summary.result}, total duration {buildReport.summary.totalTime}");
            foreach (var step in buildReport.steps)
            {
                Debug.Log($"Logging Step: {step.ToString()}, {step.name} , {step.duration}");
            }
            if (buildReport.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded){
                throw new Exception("Build Failed");//MS: Tells jenkins there was an error. Hopefully
            }
        }

        private static UnityEditor.BuildOptions GetBuildOptions(BuildOptions o)
        {
            UnityEditor.BuildOptions options = UnityEditor.BuildOptions.None;
            if (o.Dev.Value)
            {
                options |= UnityEditor.BuildOptions.Development;
            }
            EditorUserBuildSettings.buildAppBundle = o.UseAAB.Value;
    #if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode = o.VersionCode;
            PlayerSettings.bundleVersion = "1." + o.VersionCode;
    #endif
            return options;
        }
    }
}
