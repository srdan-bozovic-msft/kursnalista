using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MSC.Phone.Common.ViewModels
{
    public class Command : ICommand
    {
        private readonly Func<object, bool> _canExecuteAction;
        private readonly Action<object> _onExecute;

        public Command(Action<object> onExecute)
            : this(o => true, onExecute)
        {
        }

        public Command(Func<object, bool> canExecute, Action<object> onExecute)
        {
            _canExecuteAction = canExecute;
            _onExecute = onExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteAction(parameter);
        }

        public void ExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if(CanExecute(parameter))
                _onExecute(parameter);
        }
    }
}
