using DAL;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace AfupeoAzureFunctionApp
{
    class InjectorDependecy
    {
        public IServiceCollection _container { get; }
        public InjectorDependecy()
        {
            this._container = new ServiceCollection();

            this.RegisterRepositories();

            this.RegisterServices();

            this.RegisterViews();

            this.RegisterViewModels();
        }

        private void RegisterViewModels() 
        {
        }

        private void RegisterViews()
        {
        }

        private void RegisterServices()
        {
            this._container.AddScoped<ISqlManager, SqlManager>();
        }

        private void RegisterRepositories()
        {
        }
    }
}
