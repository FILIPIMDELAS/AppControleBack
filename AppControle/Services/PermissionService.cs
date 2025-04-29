using AppControle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace AppControle.Services
{
    public class PermissionService
    { 
        private readonly AcessControlContext _context;
        public PermissionService(AcessControlContext context)
        {
            _context = context;
        }

        public async Task<bool> Verifypermission(int id, string namePermission)
        {
            var PermissionList = await _context.UserPermission.ToListAsync();
            int PermissionId = 0;

            foreach (var Permission in PermissionList)
            {
                if (Permission.NamePermission == namePermission)
                {
                    PermissionId = Permission.Id;
                }
            }

            var relPermUser = await _context.RelPermUser.ToListAsync();

            foreach(var RelPermUser in relPermUser)
            {
                if(RelPermUser.UserId == id && RelPermUser.UserPermissionId == PermissionId)
                {
                     return true;
                }
            }

            return false;
        }
    }
}

