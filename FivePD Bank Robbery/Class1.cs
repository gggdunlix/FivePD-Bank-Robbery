using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;



namespace BankRobberyCallout
{
    [CalloutProperties("Bank Robbery", "GGGDunlix", "0.0.1")]
    public class BankRobbery : Callout
    {
        Ped driver, suspect2, suspect3;
        private Vehicle getaway;

        private Vector3[] coordinates = {
            new Vector3(-2957f,481f,15.71f),
            new Vector3(-105f,6476f,31.63f),
            new Vector3(146f,-1045f,29.38f),
            new Vector3(-1212f,-336f,37.79f),
            new Vector3(310f,-283f,53f),
        };

        public BankRobbery()
        {
            Vector3 location = coordinates.OrderBy(x => World.GetDistance(x, Game.PlayerPed.Position)).Skip(1).First();

            InitInfo(location);
            ShortName = "Bank Robbery";
            CalloutDescription = "Three suspects with weapons are robbing a bank. Respond in Code 3.";
            ResponseCode = 3;
            StartDistance = 60f;
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();

            var cars = new[]
           {
               VehicleHash.Burrito,
               VehicleHash.Burrito2,
               VehicleHash.Burrito3,
               VehicleHash.Burrito4,
               VehicleHash.Burrito5,
               VehicleHash.GBurrito,
               VehicleHash.GBurrito2,
           };

            driver = await SpawnPed(PedHash.PestContDriver, Location);
            suspect2 = await SpawnPed(PedHash.EdToh, Location);
            suspect3 = await SpawnPed(PedHash.EdToh, Location);
            getaway = await SpawnVehicle(cars[RandomUtils.Random.Next(cars.Length)], World.GetNextPositionOnSidewalk(Location));
            driver.AlwaysKeepTask = true;
            driver.BlockPermanentEvents = true;
            suspect2.AlwaysKeepTask = true;
            suspect2.BlockPermanentEvents = true;
            suspect3.AlwaysKeepTask = true;
            suspect3.BlockPermanentEvents = true;

        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);
            suspect2.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
            suspect3.Weapons.Give(WeaponHash.PistolMk2, 9999, true, true);
            driver.AttachBlip();
            suspect2.AttachBlip();
            suspect3.AttachBlip();
            driver.SetIntoVehicle(getaway, VehicleSeat.Driver);
            suspect2.SetIntoVehicle(getaway, VehicleSeat.Any);
            suspect3.SetIntoVehicle(getaway, VehicleSeat.Any);
            getaway.AttachBlip();
            driver.Accuracy = 2;
            suspect2.Accuracy = 2;
            suspect3.Accuracy = 2;
            suspect2.ShootRate = 1000;
            suspect3.ShootRate = 1000;
            driver.Armor = 6000;
            suspect2.Armor = 6000;
            suspect3.Armor = 6969;
            driver.Task.FleeFrom(Game.PlayerPed);
            suspect2.Task.ShootAt(Game.PlayerPed);
            suspect3.Task.ShootAt(Game.PlayerPed);
            driver.DrivingStyle = DrivingStyle.IgnoreRoads;

        }
    }


}
