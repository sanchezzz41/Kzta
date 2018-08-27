using System;
using Kzta.Commands;

namespace Kzta.ViewModels
{
    public class BaseViewModel
    {
        public Command OkayCommand { get; set; }

        public Command CancelCommand { get; set; }

        public BaseViewModel()
        {
            
        }
        /// <inheritdoc />
        public BaseViewModel(Action ok, Action cancel)
        {
            OkayCommand = new Command(ok, () => true);
            CancelCommand = new Command(cancel, () => true);
        }

        /// <inheritdoc />
        public BaseViewModel(Action ok, Action cancel, Func<bool> canExecuteOk, Func<bool> canExecuteCancel)
        {
            OkayCommand = new Command(ok, canExecuteOk);
            CancelCommand = new Command(cancel, canExecuteCancel);
        }
    }
}
