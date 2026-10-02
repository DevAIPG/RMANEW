using System;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public static class ErpFieldValidator
    {
        public static void ValidateTextLength(string fieldName, string value, int maxLength)
        {
            if (value == null) return;

            // SQL Server ignores excess trailing ASCII spaces when checking
            // character-column truncation. Inspect padding without altering data.
            var contentLength = value.Length;
            while (contentLength > 0 && value[contentLength - 1] == ' ')
                contentLength--;

            if (contentLength > maxLength)
                throw new InvalidOperationException(
                    $"ERP field {fieldName} exceeds {maxLength} characters " +
                    $"(length excluding trailing space padding: {contentLength}; supplied length: {value.Length}).");
        }
    }
}
