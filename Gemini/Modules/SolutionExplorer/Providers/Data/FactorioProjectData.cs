using Caliburn.Micro;

using FactrIDE.Gemini.Modules.SolutionExplorer.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    public class ProjectDataBase : PropertyChangedBase
    {
        private string name = string.Empty;
        private Version version = new Version(0, 0, 0);
        public virtual string Name
        {
            get { return name; }
            set { name = value; NotifyOfPropertyChange(() => Name); }
        }
        public virtual Version Version
        {
            get { return version; }
            set { version = value; NotifyOfPropertyChange(() => Version); }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class FactorioProjectData : ProjectDataBase
    {
        private string name = string.Empty;
        private Version version = new Version(0, 0, 0);
        private List<string> dependencies = new List<string>();
        private string description = string.Empty;
        private Version factorioVersion = new Version(0, 17, 0);
        private string homepage = string.Empty;
        private string contact = string.Empty;
        private string author = string.Empty;
        private string title = string.Empty;

        [JsonProperty("name")]
        public override string Name
        {
            get { return name; }
            set { name = value; NotifyOfPropertyChange(() => Name); }
        }
        [JsonProperty("version")]
        [JsonConverter(typeof(VersionJsonConverter))]
        public override Version Version
        {
            get { return version; }
            set { version = value; NotifyOfPropertyChange(() => Version); }
        }
        [JsonProperty("title")]
        public string Title
        {
            get { return title; }
            set { title = value; NotifyOfPropertyChange(() => Title); }
        }
        [JsonProperty("author")]
        public string Author
        {
            get { return author; }
            set { author = value; NotifyOfPropertyChange(() => Author); }
        }
        [JsonProperty("contact")]
        public string Contact
        {
            get { return contact; }
            set { contact = value; NotifyOfPropertyChange(() => Contact); }
        }
        [JsonProperty("homepage")]
        public string Homepage
        {
            get { return homepage; }
            set { homepage = value; NotifyOfPropertyChange(() => Homepage); }
        }
        [JsonProperty("factorio_version")]
        [JsonConverter(typeof(VersionJsonConverter))]
        public Version FactorioVersion
        {
            get { return factorioVersion; }
            set { factorioVersion = value; NotifyOfPropertyChange(() => FactorioVersion); }
        }
        [JsonProperty("description")]
        public string Description
        {
            get { return description; }
            set { description = value; NotifyOfPropertyChange(() => Description); }
        }
        [JsonProperty("dependencies")]
        public List<string> Dependencies
        {
            get { return dependencies; }
            set { dependencies = value; NotifyOfPropertyChange(() => Dependencies); }
        }

        [JsonProperty("factride_metadata")]
        private FactrIDEMetadata FactrIDEData { get; } = new FactrIDEMetadata();

        [JsonIgnore]
        public List<string> FilesToIgnore
        {
            get { return FactrIDEData.FilesToIgnore; }
            set { FactrIDEData.FilesToIgnore = value; NotifyOfPropertyChange(() => FilesToIgnore); }
        }
        [JsonIgnore]
        public List<string> FoldersToIgnore
        {
            get { return FactrIDEData.FoldersToIgnore; }
            set { FactrIDEData.FoldersToIgnore = value; NotifyOfPropertyChange(() => FoldersToIgnore); }
        }

        private class FactrIDEMetadata
        {
            [JsonProperty("ignore_files")]
            public List<string> FilesToIgnore { get; set; } = new List<string>();
            [JsonProperty("ignore_folders")]
            public List<string> FoldersToIgnore { get; set; } = new List<string>();
        }
    }
}