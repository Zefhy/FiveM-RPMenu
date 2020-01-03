using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace RoleplayMenuClient
{
    public class UIHandler : BaseScript
    {
        public static void ShowQuickNotification(string text)
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString(text);
            DrawNotification(false, true);
        }
    }
}
