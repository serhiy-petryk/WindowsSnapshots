using System.Security.Principal;

namespace Helpers
{
    public static class Others
    {
        public static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /*public static string GetDriverLetter()
        {
            string driveLetter = null;
            if (File.Exists("C:\\" + Files[0]))
                driveLetter = "C:\\";
            else if (File.Exists("J:\\" + Files[0]))
                driveLetter = "J:\\";
            else
            {
                MessageBox.Show($"Can't find file {Files[0]}");
            }

            return null;
        }*/
    }
}
