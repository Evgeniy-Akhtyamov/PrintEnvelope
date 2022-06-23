using PrintEnvelope.Models;
using PrintEnvelope.ViewModels;
using PrintEnvelope.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace PrintEnvelope
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);            

            IUnityContainer container = new UnityContainer();

            container.RegisterType<IMainViewModel, MainViewModel>();            
            container.RegisterType<IRecipientRepository, RecipientRepository>();
            container.RegisterType<IPrintSettingsRepository, PrintSettingsRepository>();

            container.Resolve<MainView>().Show();
        }
    }
}
