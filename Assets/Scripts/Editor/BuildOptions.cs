using CommandLine;

namespace Clash.Editor
{
    public class BuildOptions
    {
        [Option('b', "buildPath", Required = true, HelpText = "Set build path.")]
        public string BuildPath { get; set; }
        
        [Option('c', "config", Default = "main-dev", Required = true, HelpText = "Set default config.")]
        public string Config { get; set; }
        
        [Option('v', "versionCode", Default = 1, Required = true, HelpText = "Set version code.")]
        public int VersionCode { get; set; }
        
        [Option('d', "dev", Required = true, HelpText = "Set development build.")]
        public bool? Dev { get; set; }
        
        [Option('a', "buildAddressables", Default = false, Required = true, HelpText = "Build addressables.")]
        public bool? BuildAddressables { get; set; }
        
        [Option(longName:"aab", Default = false, Required = false, HelpText = "Use aab.")]
        public bool? UseAAB { get; set; }
        
        [Option("playground", Default = false, Required = false, HelpText = "Build the playground mode")]
        public bool? IsPlayground { get; set; }
        
        [Option("remoteConsole", Default = false, Required = false, HelpText = "Use remote console")]
        public bool? UseRemoteConsole { get; set; }

    }
}