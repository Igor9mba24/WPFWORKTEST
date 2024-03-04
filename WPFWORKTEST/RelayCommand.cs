using System;
using System.Windows.Input;

/// <summary>
/// Реализация интерфейса ICommand для создания команд, которые могут быть привязаны к элементам управления в пользовательском интерфейсе.
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute; // Делегат для выполнения команды.
    private readonly Func<object, bool> _canExecute; // Делегат для проверки возможности выполнения команды.

    /// <summary>
    /// Инициализирует новый экземпляр класса RelayCommand с указанными делегатами действия и проверки возможности выполнения команды.
    /// </summary>
    /// <param name="execute">Делегат для выполнения команды.</param>
    /// <param name="canExecute">Делегат для проверки возможности выполнения команды (необязательный).</param>
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute)); // Проверка наличия делегата для выполнения команды.
        _canExecute = canExecute; // Присваивание делегата для проверки возможности выполнения команды.
    }

    /// <summary>
    /// Событие, вызываемое при изменении возможности выполнения команды.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; } // Добавление подписчика на событие CommandManager.RequerySuggested.
        remove { CommandManager.RequerySuggested -= value; } // Удаление подписчика на событие CommandManager.RequerySuggested.
    }

    /// <summary>
    /// Определяет, может ли команда выполниться в текущем состоянии.
    /// </summary>
    /// <param name="parameter">Параметр, передаваемый команде.</param>
    /// <returns>True, если команда может быть выполнена, в противном случае — false.</returns>
    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);  // Проверка возможности выполнения команды с использованием делегата _canExecute.
    }

    /// <summary>
    /// Выполняет команду с указанным параметром.
    /// </summary>
    /// <param name="parameter">Параметр, передаваемый команде.</param>
    public void Execute(object parameter)
    { 
        _execute(parameter); // Выполнение команды с использованием делегата _execute.
    }
}