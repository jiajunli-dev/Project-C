namespace API.Utility;

public class ClerkUser
{
    public string id { get; set; }
    public string primary_email_address_id { get; set; }
    public string primary_phone_number_id { get; set; }
    public string primary_web3_wallet_id { get; set; }
    public string username { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public Dictionary<string, string> public_metadata { get; set; }
    public Dictionary<string, string> unsafe_metadata { get; set; }
    public List<string> email_addresses { get; set; }
    public long last_sign_in_at { get; set; }
    public bool banned { get; set; }
    public bool locked { get; set; }
    public long lockout_expires_in_seconds { get; set; }
    public int verification_attempts_remaining { get; set; }
    public long last_active_at { get; set; }
}
