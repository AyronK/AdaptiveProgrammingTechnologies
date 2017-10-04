using System;
using System.Windows.Input;

namespace MVVMProject.ViewModel
{
    class RelayCommand : ICommand
    {
        #region Fields 
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        #endregion // Fields 

        #region Constructors 
        public RelayCommand(Action execute) : this(execute, null)
        { }
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors 

        #region ICommand Members 
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }
        public void Execute(object parameter)
        {
            _execute();
        }
        public event EventHandler CanExecuteChanged;
        #endregion // ICommand Members 

        #region API
        internal void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
