using file_updater_wpf.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace file_updater_wpf.MVVM.ViewModels
{
    public class LofupViewModel : ObservableObject
    {
        private string fromPath;

        public string FromPath
        {
            get { return fromPath; }
            set { 
                fromPath = value;
                RaisePropertyChangedEvent(nameof(FromPath));
            }
        }

        private string toPath;

        public string ToPath
        {
            get { return toPath; }
            set {
                toPath = value;
                RaisePropertyChangedEvent(nameof(ToPath));
            }
        }

        private string fupFilePath;

        public string FupFilePath
        {
            get { return fupFilePath; }
            set { 
                fupFilePath = value;
                RaisePropertyChangedEvent(nameof(FupFilePath));
            }
        }


        public ICommand SelectFromPath => new RelayCommand<string>(
            DoSelectFromPath,
            x => true
        );

        private void DoSelectFromPath(string obj)
        {
            FromPath = ShowFolderBrowserDialog();
        }

        public ICommand SelectToPath => new RelayCommand<string>(
            DoSelectToPath,
            x => true
        );

        private void DoSelectToPath(string obj)
        {
            ToPath = ShowFolderBrowserDialog();
        }

        public ICommand SelectFupFilePath => new RelayCommand<string>(
            DoSelectFupFilePath,
            x => true
        );

        private void DoSelectFupFilePath(string obj)
        {
            FupFilePath = ShowFileBrowserDialog();
        }

        public ICommand RemoveFupFile => new RelayCommand<string>(
            DoRemoveFupFile,
            x => true
        );

        private void DoRemoveFupFile(string obj)
        {
            FupFilePath = "";
        }

        public ICommand UpdateFiles => new RelayCommand<string>(
            DoUpdateFiles,
            x => (FromPath != null && FromPath != "") && (ToPath != null && ToPath != "")
        );

        private void DoUpdateFiles(string obj)
        {
            var program = @"C:\Program Files\lofup\lofup.exe";
            var fupFile = fupFilePath != null && fupFilePath != "" ? $"\"{fupFilePath}\"" : "";
            var command = $"\"{program}\" \"{fromPath}\" \"{toPath}\" {fupFile}";

            Process.Start(command);
        }


        MainWindow mainWindow;

        public LofupViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }


        private string ShowFolderBrowserDialog()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(mainWindow).GetValueOrDefault())
            {
                var path = dialog.SelectedPath;
                return path;
            }
            return null;
        }

        private string ShowFileBrowserDialog()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "Text files (*.txt)|*.txt";
            if (dialog.ShowDialog(mainWindow).GetValueOrDefault())
            {
                var path = dialog.FileName;
                return path;
            }
            return null;
        }
    }
}
