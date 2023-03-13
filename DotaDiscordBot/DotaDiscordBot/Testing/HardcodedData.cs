using DotaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.Testing
{
    public static class HardcodedData
    {
        public static List<Player> GetHardcodedUsersData => new List<Player>
        {
        new Player()
            {
                Name = "Po4vara",

                Roles = new List<Role> { Role.Offlane, Role.SoftSupport, Role.HardSupport }
            },
            //new Player()
            //{
            //    Name = "Wakman",
            //
            //    Roles = new List<Role> { Role.Mid, Role.Carry, Role.Hard, Role.SoftSupport, Role.HardSupport }
            //},
            new Player()
            {
                Name = "Oleg XSQ",

                Roles = new List<Role> { Role.Mid, Role.Carry }
            },
            //new Player()
            //{
            //    Name = "Grandi show",
            //
            //    Roles = new List<Role> { Role.SoftSupport, Role.HardSupport }
            //},
            new Player()
            {
                Name = "Dencolog",

                Roles = new List<Role> { Role.Mid, Role.Carry }
            },
            new Player()
            {
                Name = "Barabola",

                Roles = new List<Role> { Role.Mid }
            },
            //new Player()
            //{
            //    Name = "Mitric",
            //
            //    Roles = new List<Role> { Role.Mid, Role.Carry, Role.SoftSupport, Role.HardSupport, Role.Hard, Role.SoftSupport }
            //},
            new Player()
            {
                Name = "Dmutro",

                Roles = new List<Role> { Role.Offlane, Role.SoftSupport, Role.HardSupport }
            },
            new Player()
            {
                Name = "Despot",

                Roles = new List<Role> { Role.Offlane, Role.SoftSupport, Role.HardSupport }
            },
            new Player()
            {
                Name = "Wackman",

                Roles = new List<Role> { Role.HardSupport, Role.SoftSupport }
            },
            //new Player()
            //{
            //    Name = "Agent crip",
            //
            //    Roles = new List<Role> { Role.Hard, Role.SoftSupport, Role.HardSupport }
            //},
            //new Player()
            //{
            //    Name = "Matu",
            //
            //    Roles = new List<Role> { Role.Mid, Role.Carry, Role.Hard }
            //},


            new Player()
            {
                Name = "Gambit",
                Roles = new List<Role> { Role.Carry, Role.Offlane, Role.HardSupport, Role.SoftSupport }
            },
            new Player()
            {
                Name = "Mitrik",

                Roles = new List<Role> { Role.Offlane, Role.SoftSupport }
            },
            new Player()
            {
                Name = "Maskarpone",

                Roles = new List<Role> { Role.Mid, Role.Carry }
            },
        };
    }
}
