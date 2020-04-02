namespace Characters
{
    public interface IUser
    {
        int UUID { get; set; }

        string FirstName { get; set; }

        string FamilyName { get; set; }

        string Password { get; set; }

        bool Sex { get; set; }

        string Team { get; set; }

        string PhotoUri { get; set; }

        string Address { get; set; }

        string EMailAddress { get; set; }

        string PhoneNumber { get; set; }

        string NickName { get; set; }

        string WechatOpenId { get; set; }
    }
}
