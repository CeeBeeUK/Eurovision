using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Eurovision
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ClampDoubleAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Value must be between {0} and {1}.";

        public Double Min { get; private set; }
        public Double Max { get; private set; }

        public ClampDoubleAttribute(Double min, Double max): base(DefaultErrorMessage)
        {
            Min = min;
            Max = max;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, Min, Max);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if((Double)value<Min || (Double)value>Max)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "InRange"
            };

            clientValidationRule.ValidationParameters.Add("minimum", Min);
            clientValidationRule.ValidationParameters.Add("maximum", Max);

            return new[] { clientValidationRule };
        }
    }
}