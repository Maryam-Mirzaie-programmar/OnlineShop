using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Datalayer;

namespace MyEshop
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            using (OnlineShopDBEntities db = new OnlineShopDBEntities())
            {
                return db.Roles.Select(r => r.RoleName).ToArray();
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using(OnlineShopDBEntities db = new OnlineShopDBEntities())
            {
                return db.Users.Where(u => u.Username == username).Select(u => u.Role.RoleName).ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (OnlineShopDBEntities db = new OnlineShopDBEntities())
            {
                return db.Users.Where(u => u.Role.RoleName == roleName).Select(u => u.Username).ToArray();
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (OnlineShopDBEntities db = new OnlineShopDBEntities())
            {
                return db.Users.Any(u => u.Username == username && u.Role.RoleName == roleName);
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            using (OnlineShopDBEntities db = new OnlineShopDBEntities())
            {
                return db.Roles.Any(r => r.RoleName == roleName);
            }
        }
    }
}