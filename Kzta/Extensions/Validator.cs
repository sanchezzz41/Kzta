using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Kzta.Views.ValidatorViews;

namespace Kzta.Extensions
{
    /// <summary>
    /// Простой валидатор
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// Список ошибок
        /// </summary>
        private List<string> Errors { get; set; }

        private Validator()
        {
            Errors = new List<string>();
        }

        public static Validator Create()
        {
            return new Validator();
        }

        /// <summary>
        /// Проверяет, является ли модель null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public Validator IsNull<T>(Expression<Func<T>> source)
        {
            var result = source.Compile().Invoke();
            var member = source.Body as MemberExpression;
            if (result == null)
                //Errors.Add($"Модель {member?.Member.Name} равняется null.");
                AddError($"Модель {member?.Member.Name} равняется null.");
            return this;
        }

        /// <summary>
        /// Добавляет ошибку в список
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Validator AddError(string errorMessage)
        {
            Errors.Add($"{Errors.Count + 1}.{errorMessage}");
            return this;
        }

        /// <summary>
        /// Отображает ошибки
        /// </summary>
        /// <returns></returns>
        public bool RaiseError()
        {
            if (!Errors.Any())
                return false;
            var view = new ValidatorView();
            view.ErrorMessage.Text = GetErrorMessage();
            view.ShowDialog();
            return true;
        }

        private string GetErrorMessage()
        {
            var strBuilder = new StringBuilder();
            foreach (var item in Errors)
            {
               strBuilder = strBuilder.AppendLine(item);
            }

            return strBuilder.ToString();
        }
    }
}
