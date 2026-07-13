namespace RestWithJWTSecurity.Requests;

public class GenerateTokenRequest
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string> Permissions = [];
    public List<string> Roles = [];
}
