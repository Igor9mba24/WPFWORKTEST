using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WPFWORKTEST.ViewModels
{
    /// <summary>
    /// Главная модель представления приложения.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Словарь для хранения сохраненных коэффициентов.
        /// </summary>
        private Dictionary<string, Tuple<double, double>> SavedCoefficients = new Dictionary<string, Tuple<double, double>>();
        /// <summary>
        /// Словарь для хранения сохраненных выбранных значений коэффициента C.
        /// </summary>
        private Dictionary<string, int> SavedSelectedCoefficientC = new Dictionary<string, int>();
        /// <summary>
        /// Событие, срабатывающее при изменении значения коэффициента A.
        /// </summary>
        public event EventHandler AChanged;
        /// <summary>
        /// Событие, срабатывающее при изменении значения коэффициента B.
        /// </summary>
        public event EventHandler BChanged;
        /// <summary>
        /// Метод OnAChanged вызывает событие AChanged при изменении значения свойства CoefficientA.
        /// </summary>
        private void OnAChanged()
        {
            AChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Метод OnBChanged вызывает событие BChanged при изменении значения свойства CoefficientB.
        /// </summary>
        private void OnBChanged()
        {
            BChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Коллекция типов функций, доступных для выбора пользователю.
        /// </summary>
        public ObservableCollection<string> FunctionTypes { get; } = new ObservableCollection<string> { "Линейная", "Квадратичная", "Кубическая", "4-ой степени", "5-ой степени" };
        /// <summary>
        /// Команда для добавления новой строки в таблицу.
        /// </summary>
        public ICommand AddRowCommand { get; }
        /// <summary>
        /// Команда для обработки ввода текста.
        /// </summary>

        /// <summary>
        /// Свойство SelectedFunction представляет выбранную пользователем функцию.
        /// При установке нового значения свойства, проверяется, изменилось ли оно.
        /// Если изменение произошло, то сохраняются текущие значения коэффициентов для предыдущей выбранной функции.
        /// Затем обновляется выбранная функция, вызывается событие изменения свойства и обновляется список коэффициентов C.
        /// Если для новой выбранной функции есть сохраненные значения коэффициентов, то они восстанавливаются,
        /// иначе устанавливаются значения по умолчанию из словаря FunctionCoefficients.
        /// </summary>
        private string _selectedFunction;
        public string SelectedFunction
        {
            get { return _selectedFunction; }
            set
            {
                if (_selectedFunction != value)
                {
                    if (!string.IsNullOrEmpty(_selectedFunction))
                    {
                        SavedCoefficients[_selectedFunction] = new Tuple<double, double>(_coefficientA, _coefficientB);
                        SavedSelectedCoefficientC[_selectedFunction] = _selectedCoefficientC;
                    }
                    _selectedFunction = value;
                    OnPropertyChanged();

                    UpdateCoefficientCList();
                    if (FunctionCoefficients.ContainsKey(value))
                    {
                        var coefficients = FunctionCoefficients[value];
                        if (SavedCoefficients.ContainsKey(value))
                        {
                            CoefficientA = SavedCoefficients[value].Item1;
                            CoefficientB = SavedCoefficients[value].Item2;
                            SelectedCoefficientC = SavedSelectedCoefficientC[value];
                        }
                        else
                        {
                            CoefficientA = coefficients.Item1;
                            CoefficientB = coefficients.Item2;
                            SelectedCoefficientC = coefficients.Item3.FirstOrDefault();
                        }
                    }
                }
            }
        }

        private double _coefficientA;
        /// <summary>
        /// Коэффициент A для функции, участвующий в вычислении значений функции.
        /// </summary>
        public double CoefficientA
        {
            get { return _coefficientA; }
            set
            {
                if (_coefficientA != value)
                {
                    // Валидация ввода: проверка, что вводимое значение может быть преобразовано в число
                    if (double.TryParse(value.ToString(), out double result))
                    {
                        _coefficientA = value; // Установка нового значения коэффициента A.
                    OnPropertyChanged(nameof(CoefficientA)); // Вызов события PropertyChanged для уведомления об изменении свойства.
                    OnAChanged(); // Вызов метода обработки изменения коэффициента A.
                    UpdateFunctionValues(); // Обновление значений функции после изменения коэффициента A.
                    }
                }
            }
        }


        private double _coefficientB;
        /// <summary>
        /// Свойство CoefficientB представляет значение коэффициента B в функции.
        /// При установке нового значения проверяется, отличается ли оно от текущего значения.
        /// Если значение отличается, оно присваивается переменной _coefficientB, и вызывается метод OnPropertyChanged(),
        /// который уведомляет об изменении свойства, а также метод OnBChanged(), который уведомляет об изменении коэффициента B.
        /// </summary>
        public double CoefficientB
        {
            get { return _coefficientB; }
            set
            {
                if (_coefficientB != value)
                {
                    _coefficientB = value;
                    OnPropertyChanged();
                    OnBChanged();
                }
            }
        }

        private ObservableCollection<int> _coefficientCList;
        /// <summary>
        /// Список коэффициентов C, представленный в виде коллекции ObservableCollection<int>.
        /// При установке нового значения списка коэффициентов C, метод OnPropertyChanged вызывается 
        /// для уведомления об изменении свойства. Также вызывается метод UpdateFunctionValues для 
        /// обновления значений функций, зависящих от этого списка коэффициентов.
        /// </summary>
        public ObservableCollection<int> CoefficientCList
        {
            get { return _coefficientCList; }
            set
            {
                if (_coefficientCList != value)
                {
                    _coefficientCList = value;
                    OnPropertyChanged();
                    UpdateFunctionValues();
                }
            }
        }

        private int _selectedCoefficientC;
        /// <summary>
        /// Получает или устанавливает выбранный коэффициент C для функции.
        /// При изменении значения выбранного коэффициента C вызывает событие PropertyChanged,
        /// чтобы уведомить привязанные элементы интерфейса об изменении свойства.
        /// Также вызывает метод UpdateFunctionValues для обновления значений функций на основе нового коэффициента.
        /// </summary>
        public int SelectedCoefficientC
        {
            get { return _selectedCoefficientC; }
            set
            {
                if (_selectedCoefficientC != value)
                {
                    _selectedCoefficientC = value;
                    OnPropertyChanged();
                    UpdateFunctionValues();
                }
            }
        }

        private ObservableCollection<FunctionData> _functionValues;
        /// <summary>
        /// Коллекция данных о функциях, отображаемых в приложении.
        /// Этот свойство представляет собой ObservableCollection типа FunctionData.
        /// ObservableCollection предоставляет механизм автоматического уведомления об изменениях в коллекции,
        /// что обеспечивает правильное отображение данных в пользовательском интерфейсе при их изменении.
        /// </summary>
        public ObservableCollection<FunctionData> FunctionValues
        {
            get { return _functionValues; }
            set
            {
                if (_functionValues != value)
                {
                    // Устанавливаем новое значение коллекции
                    _functionValues = value;
                    // Уведомляем об изменении свойства
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Метод UpdateFunctionValues обновляет значения Fxy для каждого элемента в коллекции FunctionValues.
        /// Если коллекция FunctionValues не равна null, то для каждого элемента в ней
        /// вычисляется значение Fxy с помощью метода CalculateFunctionValue, используя значения X и Y элемента.
        /// </summary>
        public void UpdateFunctionValues()
        {
            if (FunctionValues != null)
            {
                foreach (var data in FunctionValues)
                {
                    data.Fxy = CalculateFunctionValue(data.X, data.Y);
                }
            }
        }
        /// <summary>
        /// Вычисляет значение функции F(x, y) в зависимости от выбранной функции.
        /// </summary>
        /// <param name="CoefficientA">Значение кэффициента A.</param>
        /// <param name="CoefficientB">Значение кэффициента B.</param>
        /// <param name="SelectedCoefficientC">Значение выбранного кэффициента C.</param>
        /// <param name="x">Значение переменной x.</param>
        /// <param name="y">Значение переменной y.</param>
        /// <returns>Значение функции F(x, y).</returns>
        public double CalculateFunctionValue(double x, double y)
        {
            switch (SelectedFunction)
            {
                case "Линейная":
                    return CoefficientA * x + CoefficientB * Math.Pow(y, 0) + SelectedCoefficientC;
                case "Квадратичная":
                    return CoefficientA * Math.Pow(x, 2) + CoefficientB * Math.Pow(y, 1) + SelectedCoefficientC;
                case "Кубическая":
                    return CoefficientA * Math.Pow(x, 3) + CoefficientB * Math.Pow(y, 2) + SelectedCoefficientC;
                case "4-ой степени":
                    return CoefficientA * Math.Pow(x, 4) + CoefficientB * Math.Pow(y, 3) + SelectedCoefficientC;
                case "5-ой степени":
                    return CoefficientA * Math.Pow(x, 5) + CoefficientB * Math.Pow(y, 4) + SelectedCoefficientC;
                // Добавьте обработку других функций, если необходимо
                default:
                    return 0; // Возврат значения по умолчанию, если функция не выбрана
            }
        }
        /// <summary>
        /// Словарь FunctionCoefficients содержит коэффициенты для различных типов функций.
        /// Каждый элемент словаря имеет ключ в виде строки, представляющей тип функции, и значение в виде кортежа,
        /// содержащего коэффициенты A и B, а также список значений коэффициента C.
        /// </summary>
        private Dictionary<string, Tuple<double, double, List<int>>> FunctionCoefficients = new Dictionary<string, Tuple<double, double, List<int>>>
        {
            // Каждый тип функции и соответствующие ему коэффициенты представлены в словаре.
            { "Линейная", new Tuple<double, double, List<int>>(0.0, 0.0, new List<int> { 1, 2, 3, 4, 5 }) },
            { "Квадратичная", new Tuple<double, double, List<int>>(0.0, 0.0, new List<int> { 10, 20, 30, 40, 50 }) },
            { "Кубическая", new Tuple<double, double, List<int>>(0.0, 0.0, new List<int> { 100, 200, 300, 400, 500 }) },
            { "4-ой степени", new Tuple<double, double, List<int>>(0.0, 0.0, new List<int> { 1000, 2000, 3000, 4000, 5000 }) },
            { "5-ой степени", new Tuple<double, double, List<int>>(0.0, 0.0, new List<int> { 10000, 20000, 30000, 40000, 50000 }) },
            
        };
        /// <summary>
        /// Конструктор класса MainViewModel.
        /// Инициализирует объект MainViewModel, подписывается на события изменения свойств A, B, X, Y
        /// для вызова метода UpdateFunctionValues() при изменении значений этих свойств.
        /// Также инициализирует коллекцию FunctionValues и команды AddRowCommand и TextInputCommand.
        /// Устанавливает начальное значение для SelectedFunction и вызывает метод UpdateCoefficientCList() для
        /// обновления списка коэффициентов C в зависимости от выбранной функции.
        /// </summary>
        public MainViewModel()
        {
            AChanged += (sender, args) => UpdateFunctionValues();
            BChanged += (sender, args) => UpdateFunctionValues();
            FunctionValues = new ObservableCollection<FunctionData>();
            AddRowCommand = new RelayCommand(AddRow);
            SelectedFunction = FunctionTypes.FirstOrDefault();
            UpdateCoefficientCList();
        }
        /// <summary>
        /// Обновляет список коэффициентов C в зависимости от выбранной функции.
        /// </summary>
        public void UpdateCoefficientCList()
        {
            // Создание нового списка коэффициентов C.
            CoefficientCList = new ObservableCollection<int>();
            // Выборка коэффициентов C в зависимости от выбранной функции.
            switch (SelectedFunction)
            {
                // Добавление значений коэффициентов C для линейной функции.
                case "Линейная":
                    for (int i = 1; i <= 5; i++)
                        CoefficientCList.Add(i);
                    break;
                // Добавление значений коэффициентов C для квадратичной функции.
                case "Квадратичная":
                    for (int i = 10; i <= 50; i += 10)
                        CoefficientCList.Add(i);
                    break;
                // Добавление значений коэффициентов C для кубической функции.
                case "Кубическая":
                    for (int i = 100; i <= 500; i += 100)
                        CoefficientCList.Add(i);
                    break;
                // Добавление значений коэффициентов C для функции 4-ой степени.
                case "4-ой степени":
                    for (int i = 1000; i <= 5000; i += 1000)
                        CoefficientCList.Add(i);
                    break;
                // Добавление значений коэффициентов C для функции 5-ой степени.
                case "5-ой степени":
                    for (int i = 10000; i <= 50000; i += 10000)
                        CoefficientCList.Add(i);
                    break;
            }
            // Если для выбранной функции имеются сохраненные значения коэффициентов C, используем их.
            if (FunctionCoefficients.ContainsKey(SelectedFunction))
            {
                var coefficients = FunctionCoefficients[SelectedFunction];
                CoefficientCList = new ObservableCollection<int>(coefficients.Item3);
            }
        }
        /// <summary>
        /// Метод AddRow добавляет новую строку в таблицу FunctionValues.
        /// </summary>
        /// <param name="parameter">Параметр (не используется)</param>
        /// <remarks>
        /// Создается новый объект FunctionData и подписываются на его события XChanged и YChanged,
        /// чтобы при изменении значений X или Y автоматически обновлялись значения функций.
        /// Новый объект добавляется в коллекцию FunctionValues.
        /// </remarks>
        public void AddRow(object parameter)
        {
            var newData = new FunctionData();
            newData.XChanged += (sender, args) => UpdateFunctionValues();
            newData.YChanged += (sender, args) => UpdateFunctionValues();
            FunctionValues.Add(newData);
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
}