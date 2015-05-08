using System.Web.Http;

namespace Blackbird.Services.Controllers
{
    public class HomeController : ApiController
    {
        public string GetHome()
        {
            return "Hello this is the DBBahn Web Api";
        }
    }
}