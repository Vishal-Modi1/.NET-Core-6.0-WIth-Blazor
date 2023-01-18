using System;
using System.ComponentModel.DataAnnotations;

namespace DataModels.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnlikeAttribute : ValidationAttribute
    {
        public string OtherProperty { get; private set; }
        public string Message { get; private set; }

        public UnlikeAttribute(string otherProperty, string message)
            : base("")
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
            Message = message;

        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType()
                    .GetProperty(OtherProperty);

                var otherPropertyValue = otherProperty
                    .GetValue(validationContext.ObjectInstance, null);

                if (value.ToString().ToLower().Equals(otherPropertyValue.ToString().ToLower()))
                {
                    return new ValidationResult(Message, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
