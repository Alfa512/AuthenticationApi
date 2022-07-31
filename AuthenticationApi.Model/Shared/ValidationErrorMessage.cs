using System.Collections.Generic;
using System.Linq;

namespace AuthenticationApi.Model.Shared;

public class ValidationErrorMessage
{
    public ValidationErrorMessage()
    {
    }

    public ValidationErrorMessage(string errorType, string message)
    {
        ErrorType = errorType;
        Message = message;
    }

    public string ErrorType { get; set; }
    public string Message { get; set; }
}

public class ValidationErrorMessages
{
    public ValidationErrorMessages()
    {
        ErrorMessages = new List<ValidationErrorMessage>();
    }

    public List<ValidationErrorMessage> ErrorMessages { get; private set; }
    public bool HasErrors => ErrorMessages != null && ErrorMessages.Any();

    public bool AddErrorMessage(string key, string message)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        key = key.Trim();

        if (!ErrorMessages.Any())
        {
            ErrorMessages.Add(new ValidationErrorMessage(key, message));
            return true;
        }

        if (ErrorMessages.Any(m => m.ErrorType == key))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            ErrorMessages.Add(new ValidationErrorMessage(key, message));
            return true;
        }

        return false;
    }

    public bool DeleteErrorMessage(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        if (ErrorMessages.Any(m => m.ErrorType == key))
        {
            var message = ErrorMessages.First(m => m.ErrorType == key);
            ErrorMessages.Remove(message);
            return true;
        }

        return false;
    }

    public bool UpdateErrorMessage(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        if (ErrorMessages.Any(m => m.ErrorType == key))
        {
            var message = ErrorMessages.First(m => m.ErrorType == key);
            ErrorMessages.Remove(message);
            return true;
        }

        return false;
    }

    public void ClearErrors()
    {
        ErrorMessages.Clear();
    }

}