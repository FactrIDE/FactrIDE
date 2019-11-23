using Caliburn.Micro;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Providers
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SolutionDataBase : PropertyChangedBase
    {
        private string name = "";
        private List<ProjectData> projects = new List<ProjectData>();

        [JsonProperty("name")]
        public string Name
        {
            get { return name; }
            set { name = value; NotifyOfPropertyChange(() => Name); }
        }
        [JsonProperty("projects")]
        public List<ProjectData> Projects
        {
            get { return projects; }
            set { projects = value; NotifyOfPropertyChange(() => Projects); }
        }
    }

    public class ProjectData
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "";

        /// <summary>
        /// Can re relative.
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; } = "";
    }
}