namespace EffectiveMobile.Domain.Shared;

public class Error
{
    private const string Separator = "||";
    
    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }
    
    public string Code { get; }
    
    public string Message { get; }
    
    public ErrorType Type { get; }
    
    public string? InvalidField { get; } = null;
    
    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);
    
    public static Error ValueIsInvalid(string? name = null)
    {
        var label = name ?? "Value";
            
        return Validation("value.is.invalid", 
            $"{label} is invalid");
    }
    
    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);
    
    public string Serialize()
    {
        return string.Join(Separator, Code, Message, Type);
    }

    public static Error Deserialize(string serialize)
    {
        var parts = serialize.Split(Separator);

        if (parts.Length < 3)
            throw new ArgumentException("Invalid serialized format");

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new ArgumentException("Invalid serialized format");

        return new Error(parts[0], parts[1], type);
    }
}