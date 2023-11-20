using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zero_hunger.EF;

namespace zero_hunger.Controllers
{
    public class DistributionController : Controller
    {
        // GET: Distribution
        public ActionResult Index()
        {
            var db = new ZeroHungerContext();
            var distributiedList = db.Distributions
                .Include("CollectRequest")
                .ToList();
            return View(distributiedList);
        }
    }
}