using ScriptLoader.Core;
using ScriptLoader.Core.FileService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ScriptLoader.UI.ViewModels
{

    public class ScriptViewModel : INotifyPropertyChanged
    {
        static readonly Dictionary<ScriptLoadState, string> _scriptStates;
        static ScriptViewModel()
        {
            _scriptStates = new Dictionary<ScriptLoadState, string>
            {
                [ScriptLoadState.Added] = "Внесен",
                [ScriptLoadState.Founded] = "Найден",
                [ScriptLoadState.Failed] = "Ошибка сохранения"
            };
        }
        public ScriptViewModel(Script script, IFileService fileService)
        {
            this.script = script;
            _fileService = fileService;
            ScriptLoadState = ScriptLoadState.Founded;
        }

        private readonly Script script;
        private readonly IFileService _fileService;
        public string ScriptBody => script.Body;
        public string Name => script.Name;
        public string State
        {
            get
            {
                if (_scriptStates.TryGetValue(ScriptLoadState, out var val))
                    return val;
                return "unknown";
            }
        }

        public ScriptLoadState ScriptLoadState
        {
            get { return _scriptLoadState; }
            set
            {
                _scriptLoadState = value;
                OnPropertyChanged(nameof(State));
            }
        }
        private ScriptLoadState _scriptLoadState;
        public async Task SaveScript()
        {
            try
            {
                await _fileService.SaveFile(Name, script.Body);
                ScriptLoadState = ScriptLoadState.Added;
            }
            catch
            {
                ScriptLoadState = ScriptLoadState.Failed;
            }

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
    public enum ScriptLoadState
    {
        Founded,
        Added,
        Failed
    }
}
