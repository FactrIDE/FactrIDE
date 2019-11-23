using Caliburn.Micro;

using FactrIDE.Gemini.Modules.SolutionExplorer.Extensions;

using Gemini.Modules.Inspector.Inspectors;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Inspectors
{
    public class EnumEditorViewModel<TEnum> : EditorBase<TEnum>, ILabelledInspector where TEnum : Enum
    {
        private readonly List<EnumValueViewModel<TEnum>> _items;
        public IEnumerable<EnumValueViewModel<TEnum>> Items => _items;

        public EnumEditorViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(EnumEditorViewModel<TEnum>).Namespace, typeof(EnumEditorView).Namespace);

            _items = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(x => new EnumValueViewModel<TEnum>
            {
                Value = x,
                Text = x.GetDescription()
            }).ToList();
        }
    }

    public class EnumEditorViewModel : EditorBase<Enum>, ILabelledInspector
    {
        private readonly List<EnumValueViewModel> _items;
        public IEnumerable<EnumValueViewModel> Items => _items;

        public EnumEditorViewModel(Type enumType)
        {
            ViewLocator.AddNamespaceMapping(typeof(EnumEditorViewModel).Namespace, typeof(EnumEditorView).Namespace);

            _items = Enum.GetValues(enumType).Cast<Enum>().Select(x => new EnumValueViewModel
            {
                Value = x,
                Text = x.GetDescription()
            }).ToList();
        }
    }
}