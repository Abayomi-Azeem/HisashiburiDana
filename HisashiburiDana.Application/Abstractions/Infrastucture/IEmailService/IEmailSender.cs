using HisashiburiDana.Contract.EmailManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Infrastucture.IEmailService
{
    public interface IEmailSender
    {
        bool SendEmail(EmailModel model);
        public bool SendPasswordResetEmail(string recipient, string passcode);
    }
}
