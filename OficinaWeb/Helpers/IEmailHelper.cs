namespace OficinaWeb.Helpers
{
    public interface IEmailHelper
    {
        Response SendEmail(string to, string subject, string body);

        bool CheckEmailExists(string email, int id, bool isClient);
    }
}
