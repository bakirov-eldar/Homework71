﻿using Microsoft.AspNetCore.Identity;

namespace ToDoListWebApp.Models;

public class AdminInitializer
{
    public static async Task SeedAdminUser(
          RoleManager<IdentityRole<int>> _roleManager,
          UserManager<User> _userManager)
    {
        string adminEmail = "admin@admin.com";
        string adminPassword = "password";
        string firstName = "admin";
        string lastName = "admin";

        var roles = new[] { "admin", "user" };

        foreach (var role in roles)
        {
            if (await _roleManager.FindByNameAsync(role) is null)
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
        }
        if (await _userManager.FindByNameAsync(adminEmail) == null)
        {
            User admin = new User { Email = adminEmail, UserName = adminEmail, Firstname = firstName, Lastname = lastName };
            IdentityResult result = await _userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(admin, "admin");
        }
    }
}
