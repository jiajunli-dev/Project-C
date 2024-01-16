namespace API.Utility;

public class ClerkInvitation
{
    public string id { get; set; }
    public string email_address { get; set; }
    public string status { get; set; }
    public bool revoked { get; set; }

    public Dictionary<string, string> public_metadata { get; set; }
}
