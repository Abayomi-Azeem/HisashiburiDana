using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.EmailManager
{
    public class EmailTemplateHelper
    {
        public static string PasswordResetMailTemplate(string email, string subject, string passcode)
        {
            var mailTemplate = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Anime account team</title>
        </head>
        <body>
            <h1>Anime team {subject}</h1>
            <p>
                Please use this code to reset the password for the Anime account {email}.
            </p>
            <p>
                Here is your code: {passcode}
            </p>
              <p>
                If you don't recognize the Anime account {email}, you can click here to remove your email address from that account.
                Thanks,
                The Microsoft account team
            </p>
            <p>
                Thanks,
            <br/>
                The Anime account team
            </p>
        </body>
        </html>";

            return mailTemplate;
        }
    }
}