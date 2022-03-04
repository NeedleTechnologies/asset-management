using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asset_management.Entities;
using asset_management.Models;
using asset_management.Services;
using AssetManagement.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace AssetManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private static readonly string[] Summaries = new[] {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        private readonly ILogger<DocumentsController> _logger;
        private readonly ApplicationDbContext _dbcontext;
        private readonly AppServices _appServices;

        public DocumentsController(ILogger<DocumentsController> logger, ApplicationDbContext dbContext, AppServices appServices)
        {
            _logger = logger;
            _dbcontext = dbContext;
            _appServices = appServices;
        }

        [ProducesResponseType(typeof(ApiResponse<List<FolderResponseModel>>), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody] AuthModel model)
        {
            return Ok(await _appServices.GenerateToken(model));
        }

        [ProducesResponseType(typeof(ApiResponse<List<FolderResponseModel>>), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [HttpGet("get-folders")]
        [Authorize]
        public IActionResult GetFolders()
        {
            var resp = _dbcontext.DocumentFolders.Select(x => new FolderResponseModel
            {
                folderName = x.folderName,
                folderId = x.folderUniqueKey
            });
            return Ok(new ApiResponse
            {
                data = resp,
                status = true,
                message = "Folder retrieved successfully"
            });
        }

        [ProducesResponseType(typeof(ApiResponse<List<FolderResponseModel>>), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [HttpPost("create-folder")]
        [Authorize]
        public async Task<IActionResult> CreateFolders([FromBody] CreateFolderRequestModel model)
        {
            if (_dbcontext.DocumentFolders.Any(x => x.folderName.ToLower() == model.FolderName.ToLower()))
            {
                return BadRequest(new ApiResponse
                {
                    status = false,
                    message = "Folder name in existence"
                });
            }
            var folder = new DocumentFolder
            {
                folderName = model.FolderName
            };

            var dataSaved = await _dbcontext.DocumentFolders.AddAsync(folder);
            int savedCount = await _dbcontext.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                data = dataSaved.Entity,
                status = true,
                message = "Folder retrieved successfully"
            });
        }

        [ProducesResponseType(typeof(ApiResponse<List<FolderResponseModel>>), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel model)
        {
            if (_dbcontext.ApplicationUser.Any(x => x.UserName.ToLower() == model.Username.ToLower()))
            {
                return BadRequest(new ApiResponse
                {
                    status = false,
                    message = "User name in existence"
                });
            }

            var user = new ApplicationUser
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                UserName = model.Username
            };

            var dataResp = await _appServices.Register(user, model.Password, model.Role);
            if (dataResp.status)
            {
                return Ok(new ApiResponse
                {
                    data = model,
                    status = true,
                    message = "User Created Successfully"
                });
            }
            else
            {
                return BadRequest(new ApiResponse
                {
                    status = false,
                    message = dataResp.message,
                    data = dataResp.response
                });
            }
        }
    }
}