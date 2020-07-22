using System;
using System.Windows.Input;

namespace WpfMvvm.MVVM
{    
    public class DelegateCommand : ICommand
    {
        private Action<object> _executeMethod;
        private Predicate<object> _canExecuteMethod;


        public DelegateCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod = null)            
        {
            if (executeMethod == null)
                throw new ArgumentNullException("executeMethod");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }              

        public void Execute(object parameter)
        {
            _executeMethod(parameter);            
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null ? true : _canExecuteMethod(parameter);
        }

        //1 option
        /*public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();                               
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }*/

        //2 option
        public event EventHandler CanExecuteChanged;
        
        public void RaiseCanExecuteChanged()
        {       
            CanExecuteChanged?.Invoke(this, new EventArgs());                      
        }

        //3 option
        /*readonly WeakEventManager weakEventManager = new WeakEventManager();

        public event EventHandler CanExecuteChanged
        {
            add { weakEventManager.AddEventHandler(value); }
            remove { weakEventManager.RemoveEventHandler(value); }
        }

        /// <summary>
		/// Raise a CanExecute change event.
		/// </summary>
		public void RaiseCanExecuteChanged() => weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));*/
    }
}
