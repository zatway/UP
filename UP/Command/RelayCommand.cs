using System;
using System.Windows.Input;

namespace UP.Command
{
    /// <summary>
    /// Реализация интерфейса ICommand, предоставляющая возможность
    /// задавать команды с помощью делегатов для выполнения действия
    /// и проверки возможности выполнения команды.
    /// </summary>
    public class RelayCommand : ICommand
    {
        // Делегат, представляющий метод, который будет выполнен при вызове команды.
        private readonly Action<object> _execute;

        // Делегат, представляющий метод, определяющий, можно ли выполнить команду.
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Создает экземпляр RelayCommand.
        /// </summary>
        /// <param name="execute">Метод, который будет выполнен при вызове команды.</param>
        /// <param name="canExecute">Метод, который определяет, можно ли выполнить команду (опционально).</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Событие, вызываемое при изменении условий выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Определяет, можно ли выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True, если команду можно выполнить; иначе False.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Выполняет действие команды.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Уведомляет систему о том, что условия выполнения команды изменились.
        /// Это вызывает событие CanExecuteChanged.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
