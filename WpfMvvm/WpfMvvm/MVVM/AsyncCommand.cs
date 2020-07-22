using System;
using System.Windows.Input;
using System.Threading.Tasks;


namespace WpfMvvm.MVVM
{    
    public class AsyncCommand : IAsyncCommand
    {
        private bool _isExecuting;
        private readonly Func<Task> _executeMethod;
        private readonly Func<bool> _canExecuteMethod;
        
        public AsyncCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod = null)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {                    
                    _isExecuting = true;
                    await _executeMethod();
                }
                finally
                {
                    _isExecuting = false;
                }
            }
            RaiseCanExecuteChanged();
        }        

        public bool CanExecute()
        {
            var res = !_isExecuting && (_canExecuteMethod?.Invoke() ?? true);
            return res;
        }

        /*public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();                    
        }*/

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {                        
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);            
        }

        #region Explicit implementations
        
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync();
        }
        #endregion        
    }

    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }    
}
