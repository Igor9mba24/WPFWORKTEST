using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
/// Класс FunctionData представляет данные функции.
/// </summary>
public class FunctionData : INotifyPropertyChanged
{
    private double _x; // Поле для хранения значения переменной X.
    private double _y; // Поле для хранения значения переменной Y.
    private double _fxy; // Поле для хранения значения функции F(x,y).

    /// <summary>
    /// Событие, срабатывающее при изменении значения координаты X.
    /// </summary>
    public event EventHandler XChanged;
    /// <summary>
    /// Событие, срабатывающее при изменении значения координаты Y.
    /// </summary>
    public event EventHandler YChanged;

    /// <summary>
    /// Метод OnXChanged вызывает событие XChanged, уведомляя подписчиков о изменении свойства X.
    /// Если есть подписчики на событие XChanged, они будут уведомлены о событии без передачи дополнительных данных.
    /// </summary>
    private void OnXChanged()
    {
        XChanged?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// Метод OnYChanged вызывает событие YChanged, уведомляя подписчиков о изменении свойства Y.
    /// Если есть подписчики на событие YChanged, они будут уведомлены о событии без передачи дополнительных данных.
    /// </summary>
    private void OnYChanged()
    {
        YChanged?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// Свойство X представляет значение переменной X в модели представления.
    /// </summary>
    public double X
    {
        get { return _x; }
        set
        {
            if (_x != value)
            {
                // Устанавливаем новое значение переменной X.
                _x = value;
                // Вызываем метод OnPropertyChanged для уведомления об изменении свойства.
                OnPropertyChanged();
                // Вызываем метод OnXChanged для уведомления об изменении значения переменной X.
                OnXChanged();
            }
        }
    }
    /// <summary>
    /// Свойство Y представляет значение переменной Y в модели представления.
    /// </summary>
    public double Y
    {
        get { return _y; }
        set
        {
            if (_y != value)
            {
                // Устанавливаем новое значение переменной Y.
                _y = value;
                // Вызываем метод OnPropertyChanged для уведомления об изменении свойства.
                OnPropertyChanged();
                // Вызываем метод OnYChanged для уведомления об изменении значения переменной Y.
                OnYChanged();
            }
        }
    }
    /// <summary>
    /// Свойство Fxy представляет значение функции F(x, y).
    /// </summary>
    public double Fxy
    {
        get { return _fxy; }
        set
        {
            if (_fxy != value)
            {
                // Устанавливаем новое значение функции F(x,y).
                _fxy = value;
                // Вызываем метод OnPropertyChanged для уведомления об изменении свойства.
                OnPropertyChanged();
            }
        }
    }
    /// <summary>
    /// Конструктор по умолчанию класса FunctionData.
    /// Создает новый экземпляр класса FunctionData.
    /// </summary>
    public FunctionData()
    {

    }

    /// <summary>
    /// Конструктор FunctionData инициализирует новый экземпляр класса FunctionData с указанными значениями X и Y.
    /// </summary>
    /// <param name="x">Значение для свойства X.</param>
    /// <param name="y">Значение для свойства Y.</param>
    public FunctionData(double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Событие PropertyChanged происходит при изменении значения любого свойства в классе.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    /// <summary>
    /// Метод OnPropertyChanged вызывает событие PropertyChanged и уведомляет подписчиков о изменении значения свойства.
    /// Если есть подписчики на событие PropertyChanged, они будут уведомлены об изменении значения свойства с указанным именем.
    /// Параметр propertyName содержит имя измененного свойства (автоматически определено при помощи атрибута CallerMemberName).
    /// </summary>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}