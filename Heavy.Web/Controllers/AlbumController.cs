using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Heavy.Web.Data;
using Heavy.Web.Models;
using Heavy.Web.Services;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Heavy.Web.Controllers
{

    [Authorize(Policy = "音乐编辑1")]
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly HtmlEncoder htmlEncoder;
        private readonly IMemoryCache memoryCache;

        public AlbumController(IAlbumService albumService,HtmlEncoder htmlEncoder,IMemoryCache memoryCache)
        {
            _albumService = albumService;
            this.htmlEncoder = htmlEncoder;
            this.memoryCache = memoryCache;
        }

        // GET: Album

        public async Task<ActionResult> Index()
        {
            if (!memoryCache.TryGetValue(CacheEntryConstants.AlbumsOfToday,out List<Album> AlbumList))
            {
                AlbumList = await _albumService.GetAllAsync();

                var CacheOption = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(10000))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .RegisterPostEvictionCallback(huidaio,this);

                memoryCache.Set(CacheEntryConstants.AlbumsOfToday, AlbumList, CacheOption);

            }

            return View(AlbumList);
        }

        //缓存的回调函数
        private void huidaio(object key, object value, EvictionReason reason, object state)
        {
        }

        // GET: Album/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var album = await _albumService.GetByIdAsync(id);
            if (album == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            var newModel = new AlbumCreateViewModel();
            return View(newModel);
        }

        // POST: Album/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AlbumCreateViewModel albumCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Model is not valid");
                return View();

            }
            try
            {
                var newModel = await _albumService.AddAsync(new Album
                {
                    Artist = htmlEncoder.Encode( albumCreateViewModel.Artist),
                    Title = albumCreateViewModel.Title,
                    CoverUrl = albumCreateViewModel.CoverUrl,
                    Price = albumCreateViewModel.Price,
                    ReleaseDate = albumCreateViewModel.ReleaseDate
                });
                return RedirectToAction(nameof(Details), new { id = newModel.Id });
            }
            catch
            {
                return View(albumCreateViewModel);
            }
        }

        // GET: Album/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _albumService.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var editModel = new AlbumUpdateViewModel
            {
                Artist = model.Artist,
                Title = model.Title,
                CoverUrl = model.CoverUrl,
                Price = model.Price,
                ReleaseDate = model.ReleaseDate
            };
            return View(editModel);
        }

        // POST: Album/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AlbumUpdateViewModel albumUpdateViewModel)
        {
            var model = await _albumService.GetByIdAsync(id);
            if (model == null)
            {
                // return NotFound();
                return View(albumUpdateViewModel);
            }

            try
            {
                model.Artist = albumUpdateViewModel.Artist;
                model.Title = albumUpdateViewModel.Title;
                model.CoverUrl = albumUpdateViewModel.CoverUrl;
                model.ReleaseDate = albumUpdateViewModel.ReleaseDate;
                model.Price = albumUpdateViewModel.Price;

                await _albumService.UpdateAsync(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(albumUpdateViewModel);
            }
        }

        // GET: Album/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _albumService.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST: Album/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var model = await _albumService.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
                // return NotFound();
            }

            try
            {
                await _albumService.DeleteAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}