using System;
using System.Collections.Generic;
using System.Text;

namespace service_harness.Services
{
    public interface IEmailService
    {
        void SendMail(string confirmationLink);
    }
}
