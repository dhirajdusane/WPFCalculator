using System;
using System.Windows.Input;

namespace WPFCalculator
{
    internal class DelegateEvent : ICommand
    {
        private Action handleButtonClick;
        private Action<object> handleButtonClickParam;

        public DelegateEvent(Action handleButtonClick)
        {
            this.handleButtonClick = handleButtonClick;
        }

        public DelegateEvent(Action<object> handleButtonClickParam)
        {
            this.handleButtonClickParam = handleButtonClickParam;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (handleButtonClickParam != null)
                handleButtonClickParam(parameter);
            else
                handleButtonClick();
        }
    }
}