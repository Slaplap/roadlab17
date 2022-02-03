using Interon.Roadlab.App.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace Interon.Roadlab.App.Core.ViewModels
{
    public abstract class ViewModelBase : ObservableProperty, INotifyPropertyChanged
    {
        public Dictionary<string, ICommand> Commands { get; protected set; }

        public ViewModelBase()
        {
            Commands = new Dictionary<string, ICommand>();
            LoadVersionData();
        }

        private bool isBusy = false;

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        protected bool SetSecureProperty<T>(ref T backingStore, T value)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            Xamarin.Essentials.SecureStorage.SetAsync(value.GetType().FullName.ToString(), backingStore.ToString());
             
            return true;
        }
        protected T  GetSecureProperty<T>(ref T backingStore)
        {
            var    stackTrace           = new StackTrace();
            string lastCSharpMethodName = null;
            for (int i = 0; ; i++)
            {
                if (stackTrace.GetFrame(i).GetILOffset() == StackFrame.OFFSET_UNKNOWN)
                    break;

                lastCSharpMethodName = stackTrace.GetFrame(i).GetMethod().Name;
            }

           
            string prop = "";
            if (lastCSharpMethodName != null)
            {
               prop  = Xamarin.Essentials.SecureStorage.GetAsync(lastCSharpMethodName.ToString()).Result;
            }

            if (prop == null)
            {
                prop = backingStore.ToString();
            }
            return (T)Convert.ChangeType(prop, typeof(T));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected new virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void LoadVersionData()
        {
            VersionTracking.Track();

            verfirstLaunch        = VersionTracking.IsFirstLaunchEver.ToString();
            verfirstLaunchCurrent = VersionTracking.IsFirstLaunchForCurrentVersion.ToString();
            verfirstLaunchBuild   = VersionTracking.IsFirstLaunchForCurrentBuild.ToString();
            vercurrentVersion     = VersionTracking.CurrentVersion.ToString();
            vercurrentBuild       = VersionTracking.CurrentBuild.ToString();
            if (VersionTracking.PreviousVersion != null) verpreviousVersion =  VersionTracking.PreviousVersion.ToString();
            if (VersionTracking.PreviousBuild != null) verpreviousBuild     =   VersionTracking.PreviousBuild.ToString();
            verfirstVersion =   VersionTracking.FirstInstalledVersion.ToString();
            verfirstBuild   =   VersionTracking.FirstInstalledBuild.ToString();
            foreach (string item in VersionTracking.VersionHistory)
                verversionHistory =   item.ToString();
            foreach (string item in VersionTracking.VersionHistory)
                verbuildHistory =   item.ToString();
        }

        public string verpreviousBuild { get; private set; }

        public string verpreviousVersion { get; private set; }

        public string verbuildHistory { get; private set; }

        public string verversionHistory { get; private set; }

        public string verfirstBuild { get; private set; }

        public string verfirstVersion { get; private set; }

        public string vercurrentBuild { get; private set; }

        public string vercurrentVersion { get; private set; }

        public string verfirstLaunchBuild { get; private set; }

        public string verfirstLaunchCurrent { get; private set; }

        public string verfirstLaunch { get; set; }

        protected async Task ToggleBusy()
        {
            IsBusy = !isBusy;
            //give back to the main thread so IsBusy can change
            await Task.Delay(10);
        }
        protected async Task Busy()
        {
            IsBusy = true;
            //give back to the main thread so IsBusy can change
            await Task.Delay(10);
        }
        protected async Task NotBusy()
        {
            IsBusy = false;
            //give back to the main thread so IsBusy can change
            await Task.Delay(10);
        }
    }
}