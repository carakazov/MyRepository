using System.Collections.Generic;
using System.Web.Mvc;
using MySocialNetwork.DTO;
using MySocialNetwork.Models.SocialNetwork;
using MySocialNetwork.Services;

namespace MySocialNetwork.Controllers
{
    public class WallController : Controller
    {
        private PageService pageService = new PageService();
        public ActionResult CreateWall()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateWall(AlbumCreateDto newAlbumCreate, int ownerId)
        {
            if (ModelState.IsValid)
            {
                pageService.CreateWall(newAlbumCreate, ownerId);
                return RedirectToAction("AllDone", "Page");
            }

            return View();
        }

        public ActionResult AlbumList(int userId, ContentTypes type)
        {
            List<AlbumDto> albumDto = pageService.GetAlbums(userId, type);
            return View(albumDto);
        }

        public ActionResult OpenAlbum(int wallId)
        {
            AlbumDto albumDto = pageService.GetAlbum(wallId);
            return View("Album", albumDto);
        }
    }
}