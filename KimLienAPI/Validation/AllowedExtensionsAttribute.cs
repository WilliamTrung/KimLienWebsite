using System.ComponentModel.DataAnnotations;

namespace KimLienAPI.Validation
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        private readonly string errorMessage;

        public AllowedExtensionsAttribute(string? errorMessage = null, params string[] extensions)
        {
            _extensions = extensions;
            this.errorMessage = errorMessage == null ? $"Your image's filetype is not valid." : errorMessage;
        }
        private bool CheckFileExtension(IFormFile? file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (file != null)
                {
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return false;//return new ValidationResult(GetErrorMessage());
                    }
                }
            }
            return true;
        }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            bool result = false;
            if(value is IFormFile)
            {
                result = CheckFileExtension(value as IFormFile);
            } else if(value is List<IFormFile>)
            {
                var list = value as List<IFormFile>;
                if(list != null)
                {
                    foreach (var file in list)
                    {
                        result = CheckFileExtension(file);
                        if(!result) break;
                    }
                }
                
            }
            if (result)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return ValidationResult.Success;
#pragma warning restore CS8603 // Possible null reference return.
            } else
            {
                return new ValidationResult(GetErrorMessage());
            }
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }
    }
}
