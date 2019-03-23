using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
    [Authorize(Roles = "zyz2")]
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        public async Task<IActionResult> Index()
        {
            var model = await roleManager.Roles.ToListAsync();
            return View(model);
        }

        public IActionResult AddRole()
        {

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddRole(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(roleViewModel);
            }

            var role = new IdentityRole()
            {
                Name = roleViewModel.name
            };

            var logo = await roleManager.CreateAsync(role);

            if (logo.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var item in logo.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }

            return View(roleViewModel);


        }


        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return View();
            }

            var roleViewModel = new EditRoleViewModel()
            {
                Id = id,
                name = role.Name,
                users = new List<string>()
            };

            var user = await userManager.Users.ToListAsync();

            foreach (var item in user)
            {
                if (await userManager.IsInRoleAsync(item, role.Name))
                {
                    roleViewModel.users.Add(item.UserName);
                }
            }

            return View(roleViewModel);

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {

            var role = await roleManager.FindByIdAsync(editRoleViewModel.Id);

            if (role != null)
            {

                role.Name = editRoleViewModel.name;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "更新失败");

                return View(editRoleViewModel);

            }

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteRole(string Id)
        {

            var role = await roleManager.FindByIdAsync(Id);

            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "删除失败！");

            }
            ModelState.AddModelError(string.Empty, "没有找到这个用户！");

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> AddUserToRole(string Id)
        {

            var role = await roleManager.FindByIdAsync(Id);

            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var vm = new AddOrRemoveUserToRoleVM()
            {
                RoleId = Id
            };

            var UserItems = await userManager.Users.ToListAsync();

            foreach (var item in UserItems)
            {
                if (!await userManager.IsInRoleAsync(item, role.Name))
                {
                    vm.identityUsers.Add(item);
                }
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(AddOrRemoveUserToRoleVM addUserToRoleVM)
        {

            var role = await roleManager.FindByIdAsync(addUserToRoleVM.RoleId);
            var user = await userManager.FindByIdAsync(addUserToRoleVM.UserId);

            if (role != null && user != null)
            {

                var result = await userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(EditRole), new { id = role.Id });
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(addUserToRoleVM);
            }

            ModelState.AddModelError("", "没有找到这个用户");
            return View(addUserToRoleVM);

        }

        public async Task<IActionResult> RemoveUserFromRole(string Id)
        {

            var role = await roleManager.FindByIdAsync(Id);
            var userList = new List<IdentityUser>();

            foreach (var item in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(item, role.Name))
                {
                    userList.Add(item);
                }
            }

            var vm = new AddOrRemoveUserToRoleVM()
            {
                RoleId = Id,
                identityUsers = userList
            };

            return View(vm);


        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(AddOrRemoveUserToRoleVM addOrRemoveUserToRoleVM)
        {
            var role = await roleManager.FindByIdAsync(addOrRemoveUserToRoleVM.RoleId);
            var user = await userManager.FindByIdAsync(addOrRemoveUserToRoleVM.UserId);

            if (role != null && user != null && (await gengxin(user, role.Name)).Succeeded)
            {
                //var result=await userManager.RemoveFromRoleAsync(user,role.Name);              

                //if (result.Succeeded)
                //{
                return RedirectToAction(nameof(EditRole), new { id = role.Id });
                //}

                //foreach (var item in result.Errors)
                //{
                //    ModelState.AddModelError("", item.Description);
                //}

                //return View(addOrRemoveUserToRoleVM);

            }



            ModelState.AddModelError("", "更新失败，请重试！");
            return View(addOrRemoveUserToRoleVM);
        }


        public async Task<IdentityResult> gengxin(ApplicationUser user, string role)
        {

            return await userManager.RemoveFromRoleAsync(user, role);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> hhh([Bind("name")]string name)
        {
            var obj = await roleManager.FindByNameAsync(name);
            if (obj != null)
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}