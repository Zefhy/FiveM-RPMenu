using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MenuAPI;

namespace RoleplayMenuClient
{
    class TaxiHandler : BaseScript
    {
        private static Taxi currentTaxi;

        public TaxiHandler()
        {
            EventHandlers["RoleplayMenu:SendTaxiToPlayer"] += new Action(SendTaxiToPlayer);
        }    

        public static async Task UpdatePlayerDestinations()
        {
            await Delay(0);
            MenuHandler.destinationsMenu.ClearMenuItems();
            MenuItem taxiItem = new MenuItem("Waypoint", "Your current waypoint is set as your destination.")
            {
                Enabled = true,
            };
            MenuHandler.destinationsMenu.AddMenuItem(taxiItem);

            foreach (int p in GetActivePlayers())
            {
                MenuItem item = new MenuItem(GetPlayerName(p), GetPlayerName(p) + "'s position is set as your destination.")
                {
                    Enabled = true,
                };
                MenuHandler.destinationsMenu.AddMenuItem(item);
            }
        }

        public async void SendTaxiToPlayer()
        {
            Vehicle taxiVehicle = await World.CreateVehicle(VehicleHash.Taxi, World.GetNextPositionOnStreet(Game.Player.Character.Position + 150f), 0);
            Ped taxiDriver = await World.CreatePed(PedHash.Manuel, World.GetNextPositionOnStreet(Game.Player.Character.Position + 150f), 0);
            currentTaxi = new Taxi(taxiVehicle, taxiDriver, Game.Player);
            currentTaxi.Initialize();
        }

        public static Taxi GetCurrentTaxi()
        {
            return currentTaxi;
        }

    }
}
