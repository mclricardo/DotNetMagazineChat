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
                var usuarioRepository = new UsuarioRepository();
                var usuario = usuarioRepository.GetAllFilteredBy(x => x.Login.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)).Single();
                return View(usuario);
            }
        }

        public ActionResult About()
        {
            ViewBag.Title = ".Net Magazine Chat";
            return View();
        }

        [OutputCache(Duration=0)]
        public JsonResult PegarComentariosDoMural()
        {
            var comentarioRepository = new ComentarioRepository();
            var comentarios = comentarioRepository.GetAllFilteredByAndOrderedBy(
                x => x.ComentarioPai == null
                , x => - x.CriadoEm.Ticks);
            return new JsonResult() { Data = new { Comentarios = comentarios }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
