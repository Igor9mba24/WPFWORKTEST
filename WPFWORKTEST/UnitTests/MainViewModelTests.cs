using NUnit.Framework;
using System;
using System.Collections.Generic;
using WPFWORKTEST.ViewModels;


namespace WPFWORKTEST.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        [Test]
        public void CalculateFunctionValue_LinearFunction_CalculatesCorrectly()
        {
            // Arrange
            var viewModel = new MainViewModel();
            viewModel.SelectedFunction = "Линейная";
            viewModel.CoefficientA = 2.0;
            viewModel.CoefficientB = 3.0;
            viewModel.SelectedCoefficientC = 1;
            var expected = 2.0 * 5.0 + 3.0 * Math.Pow(7.0, 0) + 1; // Пример расчета для x = 5, y = 7

            // Act
            var result = viewModel.CalculateFunctionValue(5.0, 7.0);

            // Assert
            Assert.AreEqual(expected, result, "Ошибка! Некорректный результат сложения.");
        }

        [Test]
        public void CalculateFunctionValue_QuadraticFunction_CalculatesCorrectly()
        {
            // Arrange
            var viewModel = new MainViewModel();
            viewModel.SelectedFunction = "Квадратичная";
            viewModel.CoefficientA = 5.0;
            viewModel.CoefficientB = 4.0;
            viewModel.SelectedCoefficientC = 20;
            var expected = 5.0 * Math.Pow(3.0, 2) + 4.0 * Math.Pow(2.0, 1) + 20; // Пример расчета для x = 3, y = 2

            // Act
            var result = viewModel.CalculateFunctionValue(3.0, 2.0);

            // Assert
            Assert.AreEqual(expected, result, "Ошибка! Некорректный результат сложения.");
        }

        [Test]
        public void CalculateFunctionValue_CubicFunction_CalculatesCorrectly()
        {
            // Arrange
            var viewModel = new MainViewModel();
            viewModel.SelectedFunction = "Кубическая";
            viewModel.CoefficientA = 1.0;
            viewModel.CoefficientB = 4.3;
            viewModel.SelectedCoefficientC = 300;
            var expected = 1.0 * Math.Pow(2.1, 3) + 4.3 * Math.Pow(2.0, 2) + 300; // Пример расчета для x = 2.1, y = 2

            // Act
            var result = viewModel.CalculateFunctionValue(2.1, 2.0);

            // Assert
            Assert.AreEqual(expected, result, "Ошибка! Некорректный результат сложения.");
        }

        [Test]
        public void CalculateFunctionValue_FourthFunction_CalculatesCorrectly()
        {
            // Arrange
            var viewModel = new MainViewModel();
            viewModel.SelectedFunction = "4-ой степени";
            viewModel.CoefficientA = 2.6;
            viewModel.CoefficientB = 3.7;
            viewModel.SelectedCoefficientC = 4000;
            var expected = 2.6 * Math.Pow(61.0, 4) + 3.7 * Math.Pow(32.0, 3) + 4000; // Пример расчета для x = 61, y = 32

            // Act
            var result = viewModel.CalculateFunctionValue(61.0, 32.0);

            // Assert
            Assert.AreEqual(expected, result, "Ошибка! Некорректный результат сложения.");
        }

        [Test]
        public void CalculateFunctionValue_FifthFunction_CalculatesCorrectly()
        {
            // Arrange
            var viewModel = new MainViewModel();
            viewModel.SelectedFunction = "5-ой степени";
            viewModel.CoefficientA = 158.0;
            viewModel.CoefficientB = 3201.0;
            viewModel.SelectedCoefficientC = 50000;
            var expected = 158.0 * Math.Pow(84.0, 5) + 3201.0 * Math.Pow(62.0, 4) + 50000; // Пример расчета для x = 84, y = 62

            // Act
            var result = viewModel.CalculateFunctionValue(84.0, 62.0);

            // Assert
            Assert.AreEqual(expected, result, "Ошибка! Некорректный результат сложения.");
        }
        [Test]
        public void UpdateCoefficientCList_MethodWorksCorrectly()
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            viewModel.SelectedFunction = "Квадратичная";
            viewModel.UpdateCoefficientCList();
            var quadraticCoefficients = viewModel.CoefficientCList;

            viewModel.SelectedFunction = "5-ой степени";
            viewModel.UpdateCoefficientCList();
            var fifthPowerCoefficients = viewModel.CoefficientCList;

            // Assert
            CollectionAssert.AreEqual(new List<int> { 10, 20, 30, 40, 50 }, quadraticCoefficients);
            CollectionAssert.AreEqual(new List<int> { 10000, 20000, 30000, 40000, 50000 }, fifthPowerCoefficients);
        }
        [Test]
        public void AddRow_AddsNewRow()
        {
            // Arrange
            var viewModel = new MainViewModel();
            var initialRowCount = viewModel.FunctionValues.Count;

            // Act
            viewModel.AddRow(null);

            // Assert
            Assert.AreEqual(initialRowCount + 1, viewModel.FunctionValues.Count);
        }
    }
}