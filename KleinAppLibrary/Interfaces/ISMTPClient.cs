namespace KleinMapLibrary.Interfaces
{
    public interface ISMTPClient
    {
        void SendMail(string from, string reciver, string subject, string body);
    }
}