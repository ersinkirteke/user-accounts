using System;
using System.IO;

namespace service_harness.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {

        }

        public void SendMail(string confirmationLink)
        {
            Send(confirmationLink);
        }

        private void Send(string confirmationLink)
        {
            string docPath = Environment.CurrentDirectory;

            string message = "Dear player,\r\n" +
                            "In order to log in to your account so you can change your password click the following link:\r\n\r\n" +
                            confirmationLink +
                            "\r\n\r\nNote that this link is only valid for 1 hour, after which it expires!\r\n\r\n" +
                            "Happy playing!\r\n" +
                            "Greentube Support Team";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "email.txt")))
            {
                outputFile.Write(message);
            }
        }
    }
}
