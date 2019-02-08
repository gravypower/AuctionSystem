﻿namespace AuctionSystem.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.User;
    using Services.Interfaces;
    using Services.Models.AuctionUser;

    public class UsersController : AdminController
    {
        private readonly IUserService userService;
        private readonly UserManager<AuctionUser> userManager;

        public UsersController(IUserService userService, UserManager<AuctionUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var users = this.userService.GetAllUsers<UserListingServiceModel>()
                .Select(Mapper.Map<UserListingViewModel>)
                .ToPaginatedList(page, WebConstants.UsersCountPerPage);
            
            var adminIds = (await this.userManager
                    .GetUsersInRoleAsync(WebConstants.AdministratorRole))
                .Select(r => r.Id)
                .ToHashSet();
            
            foreach (var user in users)
            {
                var currentUserRoles = new List<string>();
                var nonCurrentRoles = new List<string>();

                if (adminIds.Contains(user.Id))
                {
                    currentUserRoles.Add(WebConstants.AdministratorRole);
                }
                else
                {
                    nonCurrentRoles.Add(WebConstants.AdministratorRole);
                }

                user.CurrentRoles = currentUserRoles;
                user.NonCurrentRoles = nonCurrentRoles;
            }

            return this.View(users);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToRole(string userEmail, string role)
        {
            if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(userEmail))
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                return this.RedirectToAction(nameof(this.Index));
            }

            var user = await this.userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                this.ShowErrorMessage(NotificationMessages.UserNotFound);
                return this.RedirectToAction(nameof(this.Index));
            }

            var identityResult = await this.userManager.AddToRoleAsync(user, role);

            var success = identityResult.Succeeded;
            if (success)
            {
                this.ShowSuccessMessage(
                    string.Format(NotificationMessages.UserAddedToRole, userEmail, role));
            }
            else
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
