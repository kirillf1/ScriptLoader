using ScriptLoader.Core;
using ScriptLoader.UI.ViewModels;

namespace ScriptLoader.UI.Factories
{
    public interface IScriptViewModelFactory
    {
        public ScriptViewModel Create(Script script);
    }
}
