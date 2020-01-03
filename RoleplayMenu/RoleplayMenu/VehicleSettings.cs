using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace RoleplayMenuClient
{
    class VehicleSettings : BaseScript
    {
        private static bool disableSeatShuffle = false;

        public VehicleSettings()
        {
            RegisterScript(this);
            Tick += VehicleSettingsTick;
        }

        public static bool DisableSeatShuffle
        {
            get { return disableSeatShuffle; }
            set { disableSeatShuffle = value; }
        }

        public async Task VehicleSettingsTick()
        {
            await Delay(0);

            if (disableSeatShuffle && Game.PlayerPed.IsInVehicle())
            {
                Debug.WriteLine(Game.PlayerPed.IsDoingDriveBy.ToString() + " " + Game.PlayerPed.IsAiming.ToString());
                if (Game.PlayerPed.SeatIndex == VehicleSeat.RightFront)
                {
                    if (!Game.PlayerPed.IsDoingDriveBy || !Game.PlayerPed.IsAiming || !Game.PlayerPed.IsShooting)
                    {
                        if (GetIsTaskActive(Game.PlayerPed.Handle, 165))
                        {
                            Game.PlayerPed.SetIntoVehicle(Game.PlayerPed.CurrentVehicle, Game.PlayerPed.SeatIndex);
                        }
                    }
                }
            }
        }
    }
}
