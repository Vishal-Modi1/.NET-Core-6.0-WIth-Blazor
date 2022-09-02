using System;
using System.ComponentModel.DataAnnotations;

namespace DataModels.CustomValidations
{
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //public class DateGreaterThanAttribute : ValidationAttribute
    //{
    //    string otherPropertyName;

    //    public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
    //        : base(errorMessage)
    //    {
    //        this.otherPropertyName = otherPropertyName;
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        ValidationResult validationResult = ValidationResult.Success;
    //        try
    //        {
    //            // Using reflection we can get a reference to the other date property, in this example the project start date
    //            var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
    //            // Let's check that otherProperty is of type DateTime as we expect it to be
    //            if (otherPropertyInfo.PropertyType.Equals(new DateTime().GetType()))
    //            {
    //                DateTime toValidate = (DateTime)value;
    //                DateTime referenceProperty = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
    //                // if the end date is lower than the start date, than the validationResult will be set to false and return
    //                // a properly formatted error message
    //                if (toValidate.CompareTo(referenceProperty) < 1)
    //                {
    //                    validationResult = new ValidationResult(ErrorMessageString);
    //                }
    //            }
    //            else
    //            {
    //                validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // Do stuff, i.e. log the exception
    //            // Let it go through the upper levels, something bad happened
    //            throw ex;
    //        }

    //        return validationResult;
    //    }
    //}

    public class DateGreaterThanAttribute : ValidationAttribute
    {

        public DateGreaterThanAttribute(string otherProperty, string errorMessage)
            : base("{0} must be greater than {1}")
        {
            OtherProperty = otherProperty;
            ErrorMessage = errorMessage;
        }

        public string OtherProperty { get; set; }
        public string ErrorMessage { get; set; }    

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(ErrorMessageString, name, otherName);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);

            if (firstComparable != null && secondComparable != null)
            {
                if (firstComparable.CompareTo(secondComparable) < 1)
                {
                    object obj = validationContext.ObjectInstance;
                    var thing = obj.GetType().GetProperty(OtherProperty);
                    var displayName = (DisplayAttribute)Attribute.GetCustomAttribute(thing, typeof(DisplayAttribute));

                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(
            ValidationContext validationContext)
        {
            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }
    }
}
