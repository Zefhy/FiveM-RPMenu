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
        Menu miscSettingsMenu;

        public MenuHandler()
        {
            // Menus
            menu = new Menu("Roleplay Menu");
            destinationsMenu = new Menu("Destinations");
            miscSettingsMenu = new Menu("Misc Settings");

            // Create menu controller
            if (menuController == null)
            {
                menuController = new MenuController();
                MenuController.AddMenu(menu);
                MenuController.AddMenu(destinationsMenu);
                MenuController.AddMenu(miscSettingsMenu);
                MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;
                MenuController.MenuToggleKey = Control.MultiplayerInfo;
                MenuController.EnableMenuToggleKeyOnController = false;
                MenuController.MainMenu = menu;
                MenuController.AddSubmenu(menu, miscSettingsMenu);
            }

            // Main menu items
            MenuItem taxiItem = new MenuItem("Call Taxi", "Dispatches a taxi to your current location")
            {
                Enabled = true,
                LeftIcon = MenuItem.Icon.CAR
            };
            menu.AddMenuItem(taxiItem);

            MenuItem miscSettingsItem = new MenuItem("Miscellaneous Settings", "Configure various miscellaneous settings.")
            {
                Enabled = true,
            };
            menu.AddMenuItem(miscSettingsItem);

            // Misc Settings items
            MenuCheckboxItem seatShuffleItem = new MenuCheckboxItem("Disable passenger seat shuffling",
                "Disables the automatic seat shuffling the passenger performs if the driver seat is empty", true)
            {
                Enabled = true,
                Checked = false
            };
            miscSettingsMenu.AddMenuItem(seatShuffleItem);

            // Main menu events
            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item == taxiItem)
                {
                    UIHandler.ShowNotificationWithIcon("CHAR_TAXI", "CHAR_TAXI", "A taxi will be dispatched to your location shortly.", "Downtown Cab & Co.");
                    menu.CloseMenu();
                    TriggerEvent("RoleplayMenu:SendTaxiToPlayer");
                } 
                else if (_item == miscSettingsItem)
                {
                    menu.CloseMenu();
                    miscSettingsMenu.OpenMenu();
                }
            };

            // Misc settings menu events

            miscSettingsMenu.OnCheckboxChange += (_sender, _item, _index, _checked) =>
            {
                Debug.WriteLine("Shitty");
                if (_item == seatShuffleItem)
                {
                    Debug.WriteLine("Selected item");
                    VehicleSettings.DisableSeatShuffle = seatShuffleItem.Checked;
                }
            };

            miscSettingsMenu.OnMenuClose += (_sender) =>
            {
                miscSettingsMenu.ParentMenu.OpenMenu();
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

