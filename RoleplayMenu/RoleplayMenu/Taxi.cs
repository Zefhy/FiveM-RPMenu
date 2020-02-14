using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace RoleplayMenuClient 
{
    class Taxi : BaseScript
    {
        private Vehicle TaxiVehicle;
        private Ped TaxiDriver;
        private Player Owner;
        private Vector3 Destination;
        private const int DRIVING_STYLE_NORMAL = 8388614;
        private const int DRIVING_STYLE_HURRY = (int)DrivingStyle.Rushed;
        bool isActive;

        public Taxi(Vehicle taxiVehicle, Ped taxiDriver, Player owner)
        {
            RegisterScript(this);
            TaxiVehicle = taxiVehicle;
            TaxiDriver = taxiDriver;
            TaxiDriver.SetIntoVehicle(TaxiVehicle, VehicleSeat.Driver);

            TaxiVehicle.AttachBlip();
            TaxiVehicle.AttachedBlip.Sprite = BlipSprite.Cab;

            isActive = true;

            Owner = owner;

            Debug.WriteLine("[RoleplayMenu] Taxi Created");

            Tick += TaxiTick;
        }

        public Vehicle getTaxiVehicle()
        {
            return TaxiVehicle;
        }

        public Ped getTaxidriver()
        {
            return TaxiDriver;
        }

        public async Task TaxiTick()
        {
            if (TaxiVehicle == null || TaxiDriver == null) return;

            await Delay(0);
            if (Game.PlayerPed.IsInRangeOf(TaxiVehicle.Position, 7f) && !Game.PlayerPed.IsInVehicle())
            {
                Screen.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to enter this taxi.");
                if (Game.IsControlJustPressed(1, (Control)38))
                {
                    Game.PlayerPed.Task.EnterVehicle(TaxiVehicle, VehicleSeat.RightRear);
                    await TaxiHandler.UpdatePlayerDestinations();
                    while (!Game.PlayerPed.IsInVehicle())
                    {
                        await Delay(0);
                    }
                    MenuHandler.destinationsMenu.Visible = true;
                }
                else
                {
                    MenuHandler.destinationsMenu.Visible = false;
                }
            }

            if (TaxiVehicle.IsInRangeOf(Destination, 10f))
            {
                TaxiVehicle.IsHandbrakeForcedOn = true;
                while (TaxiVehicle.Speed > 0)
                {
                    await Delay(0);
                }
                await Delay(1000);
                Game.PlayerPed.Task.LeaveVehicle();
                await Delay(2000);
                Debug.WriteLine("[Taxi Arrived]");
                TaxiVehicle.IsHandbrakeForcedOn = false;
                isActive = false;
            }

            if ((TaxiDriver.IsDead ||
                TaxiVehicle.IsDead ||
                TaxiDriver.IsFleeing) && 
                !TaxiVehicle.IsInRangeOf(Destination, 10f))
            {
                Debug.WriteLine("[Taxi Disturbed]");
                isActive = false;
            }

            if (!isActive)
            {
                MarkAsNoLongerNeeded();
            }

            if (Game.PlayerPed.CurrentVehicle == TaxiVehicle)
            {
                Screen.DisplayHelpTextThisFrame("Press ~INPUT_VEH_HEADLIGHT~ to hurry up.");
                if (Game.IsControlJustPressed(1, (Control)74))
                {
                    TaskVehicleGotoNavmesh(TaxiDriver.Handle, TaxiVehicle.Handle, Destination.X, Destination.Y, Destination.Z, 200f, 156, 5.0f);
                }
            }
        }

        public void DriveToDestination(Vector3 destination)
        {
            Destination = World.GetNextPositionOnStreet(destination);
            TaxiDriver.Task.DriveTo(TaxiVehicle, Destination, 10f, 30f, DRIVING_STYLE_NORMAL);
        }

        public void MarkAsNoLongerNeeded()
        {
            Tick -= TaxiTick;
            TaxiVehicle.AttachedBlip.Delete();
            TaxiVehicle.MarkAsNoLongerNeeded();
            TaxiDriver.MarkAsNoLongerNeeded();
            TaxiDriver.Task.CruiseWithVehicle(TaxiVehicle, 20f, DRIVING_STYLE_NORMAL);
            MenuHandler.destinationsMenu.Visible = false;
        }

        public void Initialize()
        {
            Debug.WriteLine("[RoleplayMenu] Taxi driver tasked wth driving to player...");
            TaxiDriver.Task.DriveTo(TaxiVehicle, Owner.Character.Position, 10f, 20f, DRIVING_STYLE_NORMAL);
        }
    }
}
