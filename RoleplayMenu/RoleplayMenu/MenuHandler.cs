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
    public class MenuHandler : BaseScript
    {
        Menu menu;
        private MenuController menuController;
        public static Menu destinationsMenu;

        public MenuHandler()
        {
            // Menus
            menu = new Menu("Roleplay Menu");
            destinationsMenu = new Menu("Destinations");

            // Create menu controller
            if (menuController == null)
            {
                menuController = new MenuController();
                MenuController.AddMenu(menu);
                MenuController.AddMenu(destinationsMenu);
                MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;
                MenuController.MenuToggleKey = Control.MultiplayerInfo;
                MenuController.EnableMenuToggleKeyOnController = false;
                MenuController.MainMenu = menu;
            }

            // Main menu items
            MenuItem taxiItem = new MenuItem("Call Taxi", "Dispatches a taxi to your current location")
            {
                Enabled = true,
                LeftIcon = MenuItem.Icon.CAR
            };
            menu.AddMenuItem(taxiItem);

            // Main menu events
            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item == taxiItem)
                {
                    UIHandler.ShowQuickNotification("A taxi will be dispatched to your location shortly.");
                    TriggerEvent("RoleplayMenu:SendTaxiToPlayer");
                }
            };

            // Destination menu events
            destinationsMenu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_index == 0)
                {
                    if (IsWaypointActive())
                    {
                        TaxiHandler.GetCurrentTaxi().DriveToDestination(World.GetNextPositionOnStreet(World.WaypointPosition));
                    }
                }
                else
                {
                    foreach (int p in GetActivePlayers())
                    {
                        if (_item.Text == GetPlayerName(p))
                        {
                            TaxiHandler.GetCurrentTaxi().DriveToDestination(World.GetNextPositionOnStreet(GetEntityCoords(GetPlayerPed(p), true)));
                        }
                    }
                }
            };
        }
    }
   }

