using System;
using System.Collections.Generic;
using System.Linq;
using Happer;
using Happer.Http;

namespace Sangmado.Fida.Http
{
    public class PerResolveModuleContainer : IModuleContainer
    {
        private Dictionary<string, Func<Module>> _modules = new Dictionary<string, Func<Module>>();

        public void RegisterModule(Type moduleType, Func<Module> moduleFactory)
        {
            _modules.Add(moduleType.FullName, moduleFactory);
        }

        public IEnumerable<Module> GetAllModules()
        {
            return _modules.Values.Select(v => v());
        }

        public Module GetModule(Type moduleType)
        {
            return _modules[moduleType.FullName]();
        }
    }
}
