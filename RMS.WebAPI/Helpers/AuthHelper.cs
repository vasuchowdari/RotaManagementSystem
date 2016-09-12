using System.Web;
using System.Web.Security;

namespace RMS.WebAPI.Helpers
{
    public static class AuthHelper
    {
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public static void SetAuthCookie(long userId)
        {
            FormsAuthentication.SetAuthCookie(userId.ToString(), true);
        }

        public static long GetCurrentUserId()
        {
            var authTicket = GetAuthTicket();

            return long.Parse(authTicket.Name);
        }

        private static FormsAuthenticationTicket GetAuthTicket()
        {
            return Decrypt(GetHttpCookie());
        }

        private static HttpCookie GetHttpCookie()
        {
            return HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        }

        private static FormsAuthenticationTicket Decrypt(HttpCookie httpCookie)
        {
            return FormsAuthentication.Decrypt(httpCookie.Value);
        }
    }
}