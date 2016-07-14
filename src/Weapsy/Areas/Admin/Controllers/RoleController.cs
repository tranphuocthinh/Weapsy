﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weapsy.Core.Dispatcher;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Services.Identity;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IIdentityService _identityService;

        public RoleController(RoleManager<IdentityRole> roleManager,
            IIdentityService identityService,
            IContextService contextService)
            : base(contextService)
        {
            _roleManager = roleManager;
            _identityService = identityService;
        }

        public async Task<IActionResult> Index()
        {
            var model = _roleManager.Roles.ToList();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> Save(IdentityRole role)
        {
            await _identityService.CreateRole(role.Name);
            return Ok(string.Empty);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _roleManager.FindByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Update(IdentityRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return NotFound();

            role.Name = model.Name;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
                return Ok(string.Empty);

            throw new Exception(GetErrorMessage(result));
        }

        private string GetErrorMessage(IdentityResult result)
        {
            var builder = new StringBuilder();

            foreach (var error in result.Errors)
                builder.AppendLine(error.Description);

            return builder.ToString();
        }
    }
}
