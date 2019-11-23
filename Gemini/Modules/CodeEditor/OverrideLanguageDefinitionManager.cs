using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using Gemini.Modules.CodeEditor;

namespace FactrIDE.Gemini.Modules.Lua
{
    [Export(typeof(LanguageDefinitionManager))]
    public class OverrideLanguageDefinitionManager : LanguageDefinitionManager
    {
        [ImportingConstructor]
        public OverrideLanguageDefinitionManager([ImportMany] ILanguageDefinition[] importedLanguageDefinitions)
        {
            if (LanguageDefinitions is List<ILanguageDefinition> languageDefinitions)
            {
                languageDefinitions.Clear();
                foreach (var importedLanguageDefinition in importedLanguageDefinitions)
                {
                    var defaultLanguage = languageDefinitions.Find(l => string.Equals(l.Name, importedLanguageDefinition.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (defaultLanguage != null)
                        languageDefinitions.Remove(defaultLanguage);

                    languageDefinitions.Add(importedLanguageDefinition);
                }
            }
        }
    }
}