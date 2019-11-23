using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;

using System;
using System.ComponentModel;
using System.Threading;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    public class FactorioModificationDependency : DependencyBase
    {
        public enum DependencyState
        {
            [Description("")]
            Required = 0,

            [Description("?")]
            Optional = 1,

            [Description("!")]
            NotCompatible = 2
        }

        [Flags]
        public enum VersionState
        {
            [Description("")]
            NONE = 0,

            [Description("=")]
            Exact = 1,

            [Description(">")]
            GreatherThan = 2,

            [Description(">=")]
            GreatherThanExact = GreatherThan | Exact,

            [Description("<")]
            LowerThan = 4,

            [Description("<=")]
            LowerThanExact = LowerThan | Exact,
        }


        private DependencyState state;
        public DependencyState State
        {
            get => state;
            set { state = value; NotifyOfPropertyChange(() => State); }
        }

        private VersionState versionRequirement;
        public VersionState VersionRequirement
        {
            get => versionRequirement;
            set { versionRequirement = value; NotifyOfPropertyChange(() => VersionRequirement); }
        }

        private ManualResetEventSlim ParseLock { get; } = new ManualResetEventSlim(true);

        public FactorioModificationDependency(string jsonString)
        {
            ParseJsonRaw(jsonString);
            PropertyChanged += FactorioModificationDependency_PropertyChanged;
        }

        private void FactorioModificationDependency_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(ParseLock.IsSet)
                ParseJsonRaw(JsonRaw());

        }
        private void ParseJsonRaw(string jsonRaw)
        {
            ParseLock.Wait();
            ParseLock.Reset();
            State = jsonRaw.StartsWith("?") ? DependencyState.Optional : (jsonRaw.StartsWith("!") ? DependencyState.NotCompatible : DependencyState.Required);

            jsonRaw = jsonRaw.TrimStart('?').TrimStart('!').TrimStart(' ').TrimEnd(' ');

            DisplayText = jsonRaw;

            var split = jsonRaw.Split(' ');
            if (split.Length == 1)
                Version = new Version();
            else
            {
                VersionRequirement =
                      (split[1].Contains(">") ? VersionState.GreatherThan : VersionState.NONE)
                    | (split[1].Contains("<") ? VersionState.LowerThan : VersionState.NONE)
                    | (split[1].Contains("=") ? VersionState.Exact : VersionState.NONE)
                    ;
                if (split.Length == 2)
                    Version = new Version();
                else
                    Version = Version.TryParse(split[2], out var version) ? version : new Version();
            }
            Name = split[0];
            ParseLock.Set();
        }

        public string JsonRaw() => $"{State.GetDescription()} {Name} {VersionRequirement.GetDescription()} {Version}".TrimStart();
    }
    /*
    [Flags]
    public enum FactorioModificationDependencyRequirement
    {
        NONE = 0,
        Exact = 1,
        GreatherThan = 2,
        LowerThan = 4,
    }

    public class FactorioModificationDependency1 : DependencyBase
    {
        public FactorioModificationDependencyRequirement Requirement { get; set; }
        public bool IsOptional { get; set; }
        public bool IsNotCompatible { get; set; }

        public string JsonRaw { get; set; }

        public FactorioModificationDependency1(string jsonString)
        {
            JsonRaw = jsonString;

            IsOptional = jsonString.StartsWith("?");
            IsNotCompatible = jsonString.StartsWith("!");
            if (IsOptional || IsNotCompatible)
                jsonString = jsonString.TrimStart('?').TrimStart('!').TrimStart(' ').TrimEnd(' ');

            DisplayText = jsonString;

            var split = jsonString.Split(' ');
            if (split.Length == 1)
                Version = string.Empty;
            else
            {
                Requirement =
                      (split[1].Contains(">") ? FactorioModificationDependencyRequirement.GreatherThan : FactorioModificationDependencyRequirement.NONE)
                    | (split[1].Contains("<") ? FactorioModificationDependencyRequirement.LowerThan : FactorioModificationDependencyRequirement.NONE)
                    | (split[1].Contains("=") ? FactorioModificationDependencyRequirement.Exact : FactorioModificationDependencyRequirement.NONE)
                    ;
                Version = split[2];
            }
            Name = split[0];
        }
    }
    */
}