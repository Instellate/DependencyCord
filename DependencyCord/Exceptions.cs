namespace DependencyCord;

[System.Serializable]
public class NotFoundException : System.Exception
{
    public NotFoundType NotFoundType { get; set; }

    public NotFoundException() { }
    public NotFoundException(string message, NotFoundType type) : base(message) { NotFoundType = type; }
    protected NotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
public enum NotFoundType
{
    Guild,
    Member,
    User
}

[System.Serializable]
public class InvalidBotTokenException : System.Exception
{
    public InvalidBotTokenException() { }
    public InvalidBotTokenException(string message) : base(message) { }
    public InvalidBotTokenException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidBotTokenException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}