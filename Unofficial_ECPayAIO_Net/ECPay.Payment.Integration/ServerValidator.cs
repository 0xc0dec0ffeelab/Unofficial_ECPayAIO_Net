using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    internal class ServerValidator
    {
        public static IEnumerable<string> Validate(object source)
        {
            return Validate(string.Empty, source);
        }

        public static IEnumerable<string> Validate(object relation, object source)
        {
            try
            {
                PropertyInfo[] properties = source.GetType().GetProperties();
                foreach (PropertyInfo propInfo in properties)
                {
                    bool inherit = true;
                    object[] customAttributes = propInfo.GetCustomAttributes(typeof(ValidationAttribute), inherit);
                    try
                    {
                        object[] array = customAttributes;
                        foreach (object customAttribute in array)
                        {
                            ValidationAttribute validationAttribute = (ValidationAttribute)customAttribute;
                            if (!(
                                    (
                                        (object)validationAttribute.GetType() != typeof(RequiredAttribute)
                                        && (object)validationAttribute.GetType() != typeof(RangeAttribute)
                                        && (object)validationAttribute.GetType() != typeof(RegularExpressionAttribute)
                                        && (object)validationAttribute.GetType() != typeof(StringLengthAttribute)
                                    )
                                        ? validationAttribute.IsValid(new object?[4]
                                            {
                                                propInfo.Name,
                                                propInfo.GetValue(source, BindingFlags.GetProperty, null, null, null),
                                                source,
                                                relation
                                            })
                                        : validationAttribute.IsValid(propInfo.GetValue(source, BindingFlags.GetProperty, null, null, null))
                                 ))
                            {
                                yield return validationAttribute.FormatErrorMessage(propInfo.Name);
                            }
                        }
                    }
                    finally
                    {
                    }
                }
            }
            finally
            {
            }
        }
    }
}
