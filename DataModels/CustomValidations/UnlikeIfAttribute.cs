using System;
using System.ComponentModel.DataAnnotations;

namespace DataModels.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnlikeIfAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "The value of {0} cannot be the same as the value of the {1}.";
        private string _dependentProperty { get; set; }
        private object _targetValue { get; set; }
        public string OtherProperty { get; private set; }

        public UnlikeIfAttribute(string dependentProperty, object targetValue, string compareProperty)
            : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(compareProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = compareProperty;
            this._dependentProperty = dependentProperty;
            this._targetValue = targetValue;
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

                var field = validationContext.ObjectType.GetProperty(_dependentProperty);
                if (field != null)
                {
                    var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                    if ((dependentValue == null && _targetValue == null) || (dependentValue.Equals(_targetValue)))
                    {
                        if (value.Equals(otherPropertyValue))
                        {
                            return new ValidationResult(ErrorMessageString, new[] { validationContext.MemberName });
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

}
