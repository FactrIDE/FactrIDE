using Caliburn.Micro;

using DiscordRPC;

using FactrIDE.Gemini.Modules.SolutionExplorer;
using FactrIDE.Properties;

using Gemini.Framework;

using System.ComponentModel.Composition;

namespace FactrIDE.Gemini.Modules.DiscordRichPresence
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        public DiscordRpcClient Client { get; set; }
        private ISolutionExplorer SolutionExplorer { get; set; }
        private string ClientId { get; } = "630115853752336394";

        public override void Initialize()
        {
            SolutionExplorer = IoC.Get<ISolutionExplorer>();
            SolutionExplorer.PropertyChanged += SolutionExplorer_PropertyChanged;

            Settings.Default.PropertyChanged += Default_PropertyChanged;
            if (Settings.Default.EnableDiscordRichPresence)
            {
                Client = new DiscordRpcClient(ClientId);
                Client.Initialize();
            }

            Shell.ActiveDocumentChanged += Shell_ActiveDocumentChanged;
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Settings.Default.EnableDiscordRichPresence)))
            {
                if (Settings.Default.EnableDiscordRichPresence)
                {
                    if (Client == null)
                        Client = new DiscordRpcClient(ClientId);

                    if (Client?.IsInitialized == false)
                        Client.Initialize();
                }
                else
                {
                    if (Client?.IsInitialized == true)
                    {
                        Client.Dispose();
                        Client = null;
                    }
                }
            }
        }

        private void SolutionExplorer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!Settings.Default.EnableDiscordRichPresence) return;

            if (e.PropertyName.Equals(nameof(ISolutionExplorer.SelectedItem)) && Client.CurrentPresence?.Details.Equals(SolutionExplorer.SelectedProject.Name) != true)
                SetPresence();
        }

        private void Shell_ActiveDocumentChanged(object sender, System.EventArgs e)
        {
            if (!Settings.Default.EnableDiscordRichPresence) return;

            if(Shell.ActiveItem is IPersistedDocument persistedDocument)
            {
                if (persistedDocument.FileName == null)
                    persistedDocument.PropertyChanged += PersistedDocument_PropertyChanged;
                else
                    SetPresence(persistedDocument);
            }
            if (Shell.ActiveItem == null)
                SetPresence();
        }

        private void PersistedDocument_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!Settings.Default.EnableDiscordRichPresence) return;

            if (sender is IPersistedDocument persistedDocument && e.PropertyName.Equals(nameof(PersistedDocument.DisplayName)))
            {
                SetPresence(persistedDocument);
                persistedDocument.PropertyChanged -= PersistedDocument_PropertyChanged;
            }
        }

        private void SetPresence()
        {
            Client.SetPresence(new RichPresence()
            {
                Details = SolutionExplorer.SelectedProject.Name,
                Assets = new Assets()
                {
                    LargeImageKey = "factridesolution",
                    LargeImageText = "Factorio IDE",
                    SmallImageKey = "factorioproject"
                }
            });
        }
        private void SetPresence(IPersistedDocument persistedDocument)
        {
            Client.SetPresence(new RichPresence()
            {
                Details = SolutionExplorer.SelectedProject.Name,
                State = persistedDocument.FileName,
                Assets = new Assets()
                {
                    LargeImageKey = "factridesolution",
                    LargeImageText = "Factorio IDE",
                    SmallImageKey = System.IO.Path.GetExtension(persistedDocument.FileName) switch
                    {
                        ".lua" => "luafile",

                        ".md" => "markdownfile",

                        ".zip" => "zipfile",

                        "" => "textfile",
                        ".txt" => "textfile",
                        ".cfg" => "textfile",

                        ".png" => "image",
                        ".bmp" => "image",
                        ".jpeg" => "image",
                        ".jpg" => "image",

                        ".fms" => "factridesolution",
                        _ => ""
                    },
                    SmallImageText = System.IO.Path.GetExtension(persistedDocument.FileName) switch
                    {
                        ".lua" => "Lua File",

                        ".md" => "Markdown File",

                        ".zip" => "Zip File",

                        "" => "Text File",
                        ".txt" => "Text File",
                        ".cfg" => "Text File",

                        ".png" => "Image File",
                        ".bmp" => "Image File",
                        ".jpeg" => "Image File",
                        ".jpg" => "Image File",

                        ".fms" => "FactrIDE Solution",
                        _ => ""
                    }
                }
            });
        }
    }
}