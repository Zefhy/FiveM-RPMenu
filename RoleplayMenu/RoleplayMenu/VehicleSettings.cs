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
        private static bool disableSeatShuffle = true;

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

            Debug.WriteLine("Stopping Shuffle 0");
            if (disableSeatShuffle && Game.PlayerPed.IsInVehicle())
            {
                Debug.WriteLine("Stopping Shuffle 1");
                if (Game.PlayerPed.SeatIndex == VehicleSeat.RightFront)
                {
                    Debug.WriteLine("Stopping Shuffle 2");
                    if (GetIsTaskActive(Game.PlayerPed.Handle, 165))
                    {
                        Debug.WriteLine("Stopping Shuffle 3");
                        Game.PlayerPed.SetIntoVehicle(Game.PlayerPed.CurrentVehicle, Game.PlayerPed.SeatIndex);
                    }
                }
            }
        }
    }
}
