using Microsoft.AspNetCore.Mvc;
using PGPInformation.Models;
using System.Diagnostics;

using System;
using System.Collections;
using System.IO;
using System.Text;

using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;
using Org.BouncyCastle.Bcpg;
using NuGet.Protocol;
using Newtonsoft.Json;
using System.Text.Json;

namespace PGPInformation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

         public IActionResult InspectPublicKey()
        {

            return View();
        }

        [HttpPost]
        public IActionResult InspectPublicKey(SubmitViewModel model)
        {   
            // Process input from view to inputStream
            Stream inputStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(model.Input));
            inputStream.Position = 0;
            
            // Decode to ArmoredInputStream or BcpgInputStream
            Stream keyIn = PgpUtilities.GetDecoderStream(inputStream);

            PgpPublicKeyRingBundle pubRings;

            // In case of exceptions such as invalid apparmor, invalid crc and so on
            try
            {
                pubRings = new PgpPublicKeyRingBundle(keyIn);
            }
            catch (Exception ex) 
            {
                // Save exeption message in viewdata for dispaying in view
                ViewData["Message"] = ex.Message;

                // Create empty PgpPublicKeyRingBundle
                List<PgpObject> pgpObjects = new List<PgpObject>();
                pubRings = new PgpPublicKeyRingBundle(pgpObjects);
            }

            // Convert PgpPublicKeyRingBundle to list of PgpPublicKeys

            List<PgpPublicKey> publicKeyList = new List<PgpPublicKey>();

            foreach (PgpPublicKeyRing keyRing in pubRings.GetKeyRings())
            {
                foreach (PgpPublicKey key in keyRing.GetPublicKeys())
                {
                    publicKeyList.Add(key);                    
                }
            }

            // Save in viewdata for use in view
            ViewData["PublicKeyList"] = publicKeyList;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}