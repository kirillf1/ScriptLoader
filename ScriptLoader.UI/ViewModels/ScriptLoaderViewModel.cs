using ScriptLoader.Core.ScriptLoaders;
using ScriptLoader.UI.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ScriptLoader.UI.ViewModels
{
    public class ScriptLoaderViewModel : INotifyPropertyChanged
    {
        public ScriptLoaderViewModel(IScriptLoader scriptLoader,IScriptViewModelFactory scriptViewModelFactory)
        {
            _scriptLoader = scriptLoader;
            this.scriptViewModelFactory = scriptViewModelFactory;
        }
        private readonly IScriptLoader _scriptLoader;
        private readonly IScriptViewModelFactory scriptViewModelFactory;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool CanLoadScripts => !IsLoding;
        public bool IsLoding
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoding));
                OnPropertyChanged(nameof(CanLoadScripts));
            }
        }
        private bool isLoading = false;
        public string Address { get; set; } = "";
        public ObservableCollection<ScriptViewModel> Collection { get; set; } = new();
        /// <summary>
        /// Загружает скрипты в коллекцию
        /// </summary>
        /// <returns></returns>
        public async Task LoadScripts()
        {
            if (string.IsNullOrEmpty(Address) || !Address.StartsWith("http"))
                throw new Exception("Invalid address");
            IsLoding = true;
            Collection.Clear();
            try
            {

                var scriptsTasks = _scriptLoader.LoadScript(Address);
                var saveFileTasks = new List<Task>();
                await foreach (var scriptTask in scriptsTasks)
                {
                    try
                    {
                        var script = await scriptTask.WaitAsync(TimeSpan.FromSeconds(30));
                        var scriptViewModel = scriptViewModelFactory.Create(script);
                        Collection.Add(scriptViewModel);
                        // можно было создать триггер OnLoading когда добавляется элемент
                        // в ListView

                        saveFileTasks.Add(scriptViewModel.SaveScript());

                    }
                    catch
                    {
                        //todo можно добавить например логгер или отображать ошибку в ListView
                    }
                }
                await Task.WhenAll(saveFileTasks);
            }
            finally
            {
                IsLoding = false;
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
