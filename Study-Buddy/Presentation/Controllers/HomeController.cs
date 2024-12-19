using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        Dictionary<int, string> rooms;
        public HomeController()
        {
            rooms = new Dictionary<int, string>
            {
                {1,  "Let's Learn Python"},
                {2,  "Design With Me"},
                {3,  "Frontend Developers"}
            };
        }
        public IActionResult Index()
        {

            ViewBag.Rooms = rooms;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            KeyValuePair<int, string>? result = null;
            foreach (var room in rooms)
            {
                if (room.Key == id)
                {
                    result = new KeyValuePair<int, string>(room.Key, room.Value);
                    break;
                }

            }
            ViewBag.Room = result;
            return View();
        }
    }
}
