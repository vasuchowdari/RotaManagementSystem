using System.Net;
using System.Net.Mail;
using System.Text;
using RMS.AppServiceLayer.Helpers.Dto;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.Mailers.Dto;
using RMS.AppServiceLayer.TimeFormAdjustments.Dto;
using RMS.AppServiceLayer.Users.Dto;

namespace RMS.AppServiceLayer.Helpers.Services
{
    public static class MailerService
    {
        // Service Methods
        public static void SendPasswordResetEmail(UserDto userDto)
        {
            var userPasswordEmailDto = new UserPasswordEmailDto
            {
                RecipientAddress = userDto.Email,
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname,
                Password = userDto.Password
            };

            var msg = CommonSetup(userDto.Email, EmailType.PasswordReset);

            // step 2 - specific to Email being sent
            msg.Body = CompileMessageBody(userPasswordEmailDto);

            PostEmail(msg);
        }

        public static void SendUserRegisteredEmail(UserDto userDto)
        {
            var userPasswordEmailDto = new UserPasswordEmailDto
            {
                RecipientAddress = userDto.Email,
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname,
                Password = userDto.Password,
                Login = userDto.Login
            };

            var msg = CommonSetup(userDto.Email, EmailType.NewUserRegistration);
            msg.Body = CompileMessageBody(userPasswordEmailDto);

            PostEmail(msg);
        }

        public static void SendLeaveRequestEmail(LeaveRequestDto leaveRequestDto)
        {
            var leaveRequestEmailDto = new LeaveRequestEmailDto
            {
                RecipientAddress = "john@jwise.co.uk",
                StaffName = leaveRequestDto.StaffName,
                LeaveTypName = leaveRequestDto.LeaveTypeName,
                StartDateTime = leaveRequestDto.StartDateTime.ToString(),
                EndDateTime = leaveRequestDto.EndDateTime.ToString(),
                AmountRequested = leaveRequestDto.AmountRequested.ToString(),
                Notes = leaveRequestDto.Notes
            };

            var msg = CommonSetup(leaveRequestEmailDto.RecipientAddress, EmailType.LeaveRequest);
            msg.Body = CompileMessageBody(leaveRequestEmailDto);

            PostEmail(msg);
        }

        public static void SendTimeAdjustmentEmail(TimeAdjustmentFormDto timeAdjustmentFormDto)
        {
            var recipientAddress = "john@jwise.co.uk";

            var msg = CommonSetup(recipientAddress, EmailType.TimeAdjustment);
            msg.Body = CompileMessageBody(timeAdjustmentFormDto);

            PostEmail(msg);
        }

        public static void SendTempMaddieEmail(TempMaddieEmailDto tempEmailDto)
        {
            tempEmailDto.RecipientAddress = "maddie.walker@nouvita.co.uk";
            //tempEmailDto.RecipientAddress = "john@jwise.co.uk";

            var msg = CommonSetup(tempEmailDto.RecipientAddress, EmailType.TempMaddie);
            msg.Body = CompileMessageBody(tempEmailDto);

            PostEmail(msg);
        }


        // Private Common Methods
        private static MailMessage CommonSetup(string recipientAddress, EmailType emailType)
        {
            var msg = InitialiseMailMessage(emailType);

            msg.To.Add(recipientAddress);

            return msg;
        }
        
        private static MailMessage InitialiseMailMessage(EmailType emailType)
        {
            var msgObj = new MailMessage();
            msgObj.From = new MailAddress("donotreply@nouvita.co.uk");
            msgObj.IsBodyHtml = false;
            msgObj.Subject = SetEmailSubject(emailType);

            return msgObj;
        }

        private static string SetEmailSubject(EmailType emailType)
        {
            var subject = string.Empty;

            switch (emailType)
            {
                case EmailType.NewUserRegistration:
                    subject = "Welcome to RMS";
                    break;
                case EmailType.PasswordReset:
                    subject = "RMS Password Reset Request";
                    break;
                case EmailType.TempMaddie:
                    subject = "HARRYPOTTER";
                    break;
                case EmailType.TimeAdjustment:
                    subject = "Time Clock Adjustment Form";
                    break;
                case EmailType.LeaveRequest:
                    subject = "Leave Request";
                    break;
                default:
                    subject = "RMS Generic Email Subject";
                    break;
            }

            return subject;
        }

        private static void PostEmail(MailMessage msgObj)
        {
            var client = GenerateSmtpClient(Mailer.Default.TestEmail, Mailer.Default.NouvitaTestEmail);
            client.Send(msgObj);
        }
        
        private static SmtpClient GenerateSmtpClient(bool isTest, bool isNouvita)
        {
            var client = new SmtpClient();

            if (!isNouvita)
            {
                client.Host = isTest ? Mailer.Default.TestEmailServer : Mailer.Default.LiveEmailServer;
                client.Credentials = new NetworkCredential(Mailer.Default.DeliveryAddress, Mailer.Default.EmailPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
            }
            else
            {
                client.Host = Mailer.Default.RmsTestEmailServer;
                client.Credentials = new NetworkCredential(Mailer.Default.DeliveryAddress, Mailer.Default.EmailPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
            }

            return client;
        }


        // seperate class(es)?
        private static string CompileMessageBody(LeaveRequestEmailDto leaveRequestEmailDto)
        {
            var sb = new StringBuilder();

            // Staff Name
            sb.Append("Staff Name: ");
            sb.Append(leaveRequestEmailDto.StaffName);
            sb.Append("\n");

            // Leave Type Name
            sb.Append("Leave Type: ");
            sb.Append(leaveRequestEmailDto.LeaveTypName);
            sb.Append("\n");

            // Start Date
            sb.Append("Start Date: ");
            sb.Append(leaveRequestEmailDto.StartDateTime);
            sb.Append("\n");

            // End Date
            sb.Append("End Date: ");
            sb.Append(leaveRequestEmailDto.EndDateTime);
            sb.Append("\n");

            // Amount Requested
            sb.Append("Amount Requested: ");
            sb.Append(leaveRequestEmailDto.AmountRequested);
            sb.Append("\n");

            // Half Day on Start

            // Half Day on End

            // Notes
            sb.Append("Additional Notes: ");
            sb.Append(leaveRequestEmailDto.Notes);
            sb.Append("\n");

            return sb.ToString();
        }

        private static string CompileMessageBody(UserPasswordEmailDto emailInputDto)
        {
            var sb = new StringBuilder();

            // Greeting
            sb.Append("Welcome ");
            sb.Append(emailInputDto.Firstname);
            sb.Append(" ");
            sb.Append(emailInputDto.Lastname);
            sb.Append(",\n \n");

            // Blurb
            sb.Append("You have been registered on the RMS. Please find enclosed in this email your ");
            sb.Append("user login details.");
            sb.Append("\n \n");

            sb.Append("If you have any queries, please contact an Administrator. Thank you.");
            sb.Append("\n \n");


            // Login
            sb.Append("Login: ");
            sb.Append(emailInputDto.Login);
            sb.Append("\n");

            // Password
            sb.Append("Password: ");
            sb.Append(emailInputDto.Password);
            sb.Append("\n \n");

            // Link
            sb.Append("Please click on the link below to be taken to the login page.\n");
            sb.Append("RMS Link: http://80.229.27.248:8093/RMSV2Dev/#/login");


            return sb.ToString();
        }

        private static string CompileMessageBody(TimeAdjustmentFormDto timeAdjustmentFormDto)
        {
            var sb = new StringBuilder();

            sb.Append("Staff Name: ");
            sb.Append(timeAdjustmentFormDto.StaffName);
            sb.Append("\n");

            sb.Append("Location: ");
            sb.Append(timeAdjustmentFormDto.ShiftLocation);
            sb.Append("\n");

            sb.Append("Shift Date: ");
            sb.Append(timeAdjustmentFormDto.ShiftStartDateTime);
            sb.Append("\n");

            sb.Append("Time In: ");
            sb.Append(timeAdjustmentFormDto.ActualStartDateTime);
            sb.Append("\n");

            sb.Append("Time Out: ");
            sb.Append(timeAdjustmentFormDto.ActualEndDateTime);
            sb.Append("\n");

            sb.Append("Missed Clock In: ");
            sb.Append(timeAdjustmentFormDto.MissedClockIn);
            sb.Append("\n");

            sb.Append("Missed Clock Out: ");
            sb.Append(timeAdjustmentFormDto.MissedClockOut);
            sb.Append("\n");

            sb.Append("Late In: ");
            sb.Append(timeAdjustmentFormDto.LateIn);
            sb.Append("\n");

            sb.Append("Early Out: ");
            sb.Append(timeAdjustmentFormDto.EarlyOut);
            sb.Append("\n");

            sb.Append("Reason: ");
            sb.Append(timeAdjustmentFormDto.Notes);
            sb.Append("\n");

            return sb.ToString();
        }

        private static string CompileMessageBody(TempMaddieEmailDto emailInputDto)
        {
            var sb = new StringBuilder();

            // Shift Resource Type
            sb.Append("Shift Resource Type: ");
            sb.Append(emailInputDto.ResourceTypeName);
            sb.Append("\n");

            // Shift DateTimes
            sb.Append("Shift Start Date & Time: ");
            sb.Append(emailInputDto.ShiftStartDateTime);
            sb.Append("\n");
            sb.Append("Shift End Date & Time: ");
            sb.Append(emailInputDto.ShiftEndDateTime);
            sb.Append("\n");

            // Shift Location
            sb.Append("Shift Location: ");
            sb.Append(emailInputDto.ShiftLocation);
            sb.Append("\n");

            // Shift New Staff Member
            sb.Append("Shift Staff Member (ON): ");
            sb.Append(emailInputDto.ShiftNewStaffMember);
            sb.Append("\n");

            // Shift Old Staff Member
            sb.Append("Shift Staff Member (OFF): ");
            sb.Append(emailInputDto.ShiftOldStaffMember);
            sb.Append("\n");

            return sb.ToString();
        }
    }

    public enum EmailType
    {
        NewUserRegistration,
        PasswordReset,
        TimeAdjustment,
        LeaveRequest,
        TempMaddie
    }
}
