using System;
using System.Windows.Input;

namespace Kzta.Commands
{
    public class Command : ICommand
    {
        private Action _executeMethod;
        private Func<bool> _canExecuteMethod;

        public Command(Action p_execute, Func<bool> p_canExexute = null)
        {
            _executeMethod = p_execute;
            _canExecuteMethod = p_canExexute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod?.Invoke() ?? false;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            if (_canExecuteMethod != null)
            {
                _executeMethod();
            }
        }
    }
}
