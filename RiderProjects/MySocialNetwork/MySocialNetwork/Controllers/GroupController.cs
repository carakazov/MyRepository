using System.Collections.Generic;
using System.Web.Mvc;
using MySocialNetwork.DTO;
using MySocialNetwork.Services;

namespace MySocialNetwork.Controllers
{
    public class GroupController : Controller
    {
        private GroupService groupService = new GroupService();
        private UserService userService = new UserService();
        public ActionResult GroupsList(int userId)
        {
            List<GroupDto> groups = groupService.GetGroups(userId);
            return PartialView(groups);
        }

        public ActionResult CreateGroup()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateGroup(GroupCreationDto newGroup, int userId)
        {
            UserDto currentUser = (UserDto) Session["session"];
            newGroup.CreatorId = currentUser.Id;
            groupService.AddNewGroup(newGroup);
            return RedirectToAction("GroupsList", new {userId = userId});
        }

        public ActionResult GroupPage(string login)
        {
            UserDto userDto = userService.LogIn(login);
            return View(userDto);
        }
    }
}