using ScriptLoader.Core;
using ScriptLoader.Core.FileService;
using ScriptLoader.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptLoader.UI.Factories
{

    public class ScriptViewModelFactory : IScriptViewModelFactory
    {
        private readonly IFileService fileService;

        public ScriptViewModelFactory(IFileService fileService) 
        {
            this.fileService = fileService;
        }
        public ScriptViewModel Create(Script script)
        {
            return new ScriptViewModel(script, fileService);
        }
    }
}
