using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.EmailManager
{
    public class EmailModel
    {
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Recipient { get; set; }
        public string Passcode { get; set; }
        public static EmailModel CreateInstance(string sender, string recipient, string passcode)
        {
            return new EmailModel
            {
                Sender = sender,
                Subject = "Password reset code",
                Recipient = recipient,
                Passcode = passcode
            };
        }
    }
}
