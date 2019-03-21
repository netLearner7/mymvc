using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Heavy.Web.Controllers
{
    
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserController(UserManager<IdentityUser> userManager )
        {
            this.userManager = userManager;
        }

        public async Task< IActionResult> Index()
        {
            var user = await userManager.Users.ToListAsync() ;

            return View(user);
        }

        public IActionResult AddUser() {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateViewModel userCreateViewModel) {
            if (!ModelState.IsValid)
            {
                return View(userCreateViewModel);
            }

            var user = new IdentityUser()
            {
                UserName=userCreateViewModel.UserName,
                Email=userCreateViewModel.Email
            };

            var result = await userManager.CreateAsync(user, userCreateViewModel.PassWord);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (IdentityError item in result.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }
            return View(userCreateViewModel);

        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id) {

            var user =await userManager.FindByIdAsync(id);
            if (user != null) {
                var logo =await userManager.DeleteAsync(user);
                if (logo.Succeeded) {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "删除时出现一个错误！");
            }
            ModelState.AddModelError(string.Empty, "找不到此用户!");
            return View(nameof(Index), await userManager.Users.ToListAsync());
        }

        public async Task< IActionResult> Edit(string id) {

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return View("Index");
            var result = new EditViewModel() {
                Id=user.Id,
                UserName=user.UserName,
                PassWord=user.PasswordHash,
                Email=user.Email
            };          
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel editViewModel) {
            var user = await userManager.FindByIdAsync(editViewModel.Id);

            if (user == null)
            {
                ModelState.AddModelError("", "用户不存在！");
                return View(editViewModel);
            }

            user.UserName = editViewModel.UserName;
            user.PasswordHash = editViewModel.PassWord;
            user.Email = editViewModel.Email;

            var logo =await userManager.UpdateAsync(user);

            if (logo.Succeeded) {

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "更新到数据库失败！");
            return View(editViewModel);




        }

    }
}