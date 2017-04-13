using System.Threading.Tasks;

namespace SPIIKcom.Services
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
	}
}
