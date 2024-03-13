using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KL_Validation
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value is IFormFile)
            {
                var file = value as IFormFile;
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage(extension));
                    }
                }

                return ValidationResult.Success;
            }
            else if (value is List<IFormFile>)
            {
                var files = value as List<IFormFile>;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        if (!_extensions.Contains(extension.ToLower()))
                        {
                            return new ValidationResult(GetErrorMessage(extension));
                        }
                    }
                }

                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string extension)
        {
            return $"This {extension} extension is not allowed!";
        }
    }
}