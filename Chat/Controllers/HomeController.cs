using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chat.Domain.Model;
using Chat.Data;

namespace Chat.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return Redirect("/Account/Logon");
            }
            else
            {
                var authorRepository = new AuthorRepository();
                var author = authorRepository.GetAllFilteredBy(x => x.Login.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)).Single();
                return View(author);
            }
        }

        public ActionResult About()
        {
            ViewBag.Title = ".Net Magazine Chat";
            return View();
        }

        [OutputCache(Duration=0)]
        public JsonResult GetWallMessages()
        {
            var messageRepository = new MessageRepository();
            var messages = messageRepository.GetAllFilteredByAndOrderedBy(
                x => x.ParentMessage == null
                , x => - x.CreatedOn.Ticks);
            return new JsonResult() { Data = new { Messages = messages }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
