namespace Log.Client.Contracts;

public class CreateUserRequest
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public int Age { get; set; }
}