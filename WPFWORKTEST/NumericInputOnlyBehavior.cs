using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFWORKTEST
{
    /// <summary>
    /// Класс NumericInputOnlyBehavior является поведением для ограничения ввода только числовых значений в TextBox.
    /// </summary>
    public class NumericInputOnlyBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// Метод OnAttached переопределяется для подключения обработчика событий PreviewTextInput к TextBox при добавлении поведения.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
        }
        /// <summary>
        /// Метод OnDetaching переопределяется для отключения обработчика событий PreviewTextInput от TextBox при удалении поведения.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
        }
        /// <summary>
        /// Обработчик события PreviewTextInput, который вызывается при вводе текста в TextBox.
        /// </summary>
        /// <param name="sender">объект, инициировавший событие (в данном случае TextBox).</param>
        /// <param name="e">аргументы события, содержащие информацию о вводимом тексте.</param>
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                return;
            
            // Получаем текущий текст в текстовом поле
            string currentText = textBox.Text;

            // Формируем новый текст с учетом вставляемого символа
            string newText = currentText.Substring(0, textBox.CaretIndex) + e.Text + currentText.Substring(textBox.CaretIndex);

            // Проверяем, что новый текст соответствует формату числа
            if (!Regex.IsMatch(newText, @"^-?\d*\.?\d*$"))
            {
                e.Handled = true;
                return;
            }

            // Если вставляется точка и новый текст не соответствует формату числа
            if ((e.Text == "." || e.Text == ",") && (textBox.CaretIndex == 0 || !char.IsDigit(currentText[textBox.CaretIndex - 1])))
            {
                // Проверяем, что новый текст содержит только одну точку
                if (newText.Count(c => c == '.') == 1)
                    return;
            }
            
            // Разрешаем вставку, если ничего не противоречит правилам
            e.Handled = false;
        }
    }
}
