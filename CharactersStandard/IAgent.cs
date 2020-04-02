namespace Characters
{
    public interface IAgent
    {
        string Department { get; set; }
        string Description { get; set; }
        string EMailAddress { get; set; }
        string FamilyName { get; set; }
        string FirstName { get; set; }
        Graph[] Graphs { get; set; }
        string PhoneNumber { get; set; }
        string PhotoUri { get; set; }
        string[] Profession { get; set; }
        string Team { get; set; }
        int UUID { get; set; }
        string WeChatQRCodeUri { get; set; }
    }
}