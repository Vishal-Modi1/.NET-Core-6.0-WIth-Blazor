using System.ComponentModel.DataAnnotations;

namespace DataModels.CustomValidations
{
    public class RequiredRangeIfAttribute : ValidationAttribute
    {
        RangeAttribute _innerAttribute;
        private string _dependentProperty { get; set; }
        private double _minValue { get; set; }
        private double _maxValue { get; set; }
        private object _targetValue { get; set; }
        private string _errorMessage { get; set; }

        public RequiredRangeIfAttribute(string dependentProperty, object targetValue, double minValue, double maxValue, string errorMessage)
        {
            this._dependentProperty = dependentProperty;
            this._minValue = minValue;
            this._maxValue = maxValue;
            this._targetValue = targetValue;
            this._errorMessage = errorMessage;
            _innerAttribute = new RangeAttribute(this._minValue, this._maxValue);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = validationContext.ObjectType.GetProperty(_dependentProperty);
            if (field != null)
            {
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                if ((dependentValue == null && _targetValue == null) || (dependentValue.Equals(_targetValue)))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string name = validationContext.DisplayName;
                        string specificErrorMessage = ErrorMessage;

                        return new ValidationResult(_errorMessage, new[] { validationContext.MemberName });
                    }
                }
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(_dependentProperty));
            }
        }
    }
}
