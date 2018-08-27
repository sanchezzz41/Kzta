using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Kzta.Extensions
{
    public static class ValidatorExtensions
    {
        public static Validator IsNullOrEmpty(this Validator validator, Expression<Func<string>> source)
        {
            var test = source.Compile();

            var result = source.Compile().Invoke();
            var member = source.Body as MemberExpression;
            if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
            {
                var message = GetDescription(member);
                if (message == null)
                    message = member?.Member.Name;
                validator.AddError($"Поле '{message}' является пустым.");
            }

            return validator;
        }

        public static Validator IsZeroOrLow(this Validator validator, Expression<Func<int>> source)
        {
            var result = source.Compile().Invoke();
            var member = source.Body as MemberExpression;
            if (result <= 0)
            {
                var message = GetDescription(member);
                if (message == null)
                    message = member?.Member.Name;
                validator.AddError($"В поле '{message}' число меньше или равно 0.");
            }
            return validator;
        }

        private static string GetDescription(MemberExpression member)
        {
            var colelction = member?.Member?.CustomAttributes;
            if (colelction == null)
                return null;
            var item = colelction.FirstOrDefault(x => x.AttributeType == typeof(DescriptionAttribute));
            if (item == null)
                return null;
            var res = item.ConstructorArguments.FirstOrDefault();
            if (res == null)
                return null;

            return res.Value.ToString();
        }
    }
}
