namespace SmartHouseWebLib.Migrations
{
    using SmartHouseWebLib.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SmartHouseWebLib.Models.SmartHouseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SmartHouseWebLib.Models.SmartHouseContext context)
        {
            var houseName = "Vinodolska House";
            //House house = null;

            //if (!context.House.Any())
            //{
            //    house = new House()
            //    {
            //        Name = "Vinodolska House",
            //        City = "Split",
            //        Address = "Vinodolska",
            //        ImageUrl = null
            //    };

            //    context.House.AddOrUpdate(x => x.Name, house);
            //}
            //else
            //{
            //    house = context.House.FirstOrDefault(x => x.Name == houseName);
            //}            

            //context.Users.AddOrUpdate(
            //  p => p.UserName,
            //  new ApplicationUser { Id = "c0b9b1a1-b3dd-4d9f-ad69-205432e0d542", Name = "Filip", Surname = "Krišto", UserName = "filipkristo@windowslive.com", Email = "filipkristo@windowslive.com", LockoutEnabled = true, PasswordHash = "AM9ApoCggHN7IU8gbEMz2qxM5DjC8/wg5sEDX5yJZB3ifLCn3Qalrnz+H4xvGynVFQ==", SecurityStamp = "b019e3b3-9705-4304-8afc-12923dc93f9e", House = house }
            //);

            //context.SaveChanges();

            var house = context.House.FirstOrDefault(x => x.Name == houseName);

            var room = new Room()
            {
                HouseId = house.Id,
                Name = "Living room",
            };

            context.Room.AddOrUpdate(
                p => p.Name,
                room);

            context.SaveChanges();
        }
    }
}
