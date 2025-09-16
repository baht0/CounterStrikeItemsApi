namespace WebAdminPanel.Models
{
    public class ApiBadRequestException(string message) : Exception(message) { }
}
