using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chat.Domain.Model;
using Chat.Data;

namespace Chat.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewBag.Title = "Social News";
        }
    }
}
