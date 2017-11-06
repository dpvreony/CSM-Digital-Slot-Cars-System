using System;

using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using RaceDirectorClientGUI.Services;
using RaceDirectorClientGUI.Views;

namespace RaceDirectorClientGUI.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<RaceTypeSelectViewModel, RaceTypeSelectPage>();
            Register<RaceDriverSetupViewModel, RaceDriverSetupPage>();
            Register<RaceHUDViewModel, RaceHUDPage>();
            Register<RaceResultsViewModel, RaceResultsPage>();
            Register<GarageViewModel, GaragePage>();
            Register<SettingsViewModel, SettingsPage>();
        }

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public GarageViewModel GarageViewModel => ServiceLocator.Current.GetInstance<GarageViewModel>();

        public RaceResultsViewModel RaceResultsViewModel => ServiceLocator.Current.GetInstance<RaceResultsViewModel>();

        public RaceHUDViewModel RaceHUDViewModel => ServiceLocator.Current.GetInstance<RaceHUDViewModel>();

        public RaceDriverSetupViewModel RaceDriverSetupViewModel => ServiceLocator.Current.GetInstance<RaceDriverSetupViewModel>();

        public RaceTypeSelectViewModel RaceTypeSelectViewModel => ServiceLocator.Current.GetInstance<RaceTypeSelectViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
