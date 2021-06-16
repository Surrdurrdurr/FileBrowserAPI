using FileBrowser.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileBrowserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllFiles()
        {
            return FileBrowserModel.GetAllFiles().ToArray();
        }
    }
}
