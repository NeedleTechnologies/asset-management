using System.IO;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IWebHostEnvironment _env;
        private readonly AppServices _appServices;

        public DocumentsController(ILogger<DocumentsController> logger, ApplicationDbContext dbContext, AppServices appServices, IWebHostEnvironment env)
        {
            _logger = logger;
            _dbcontext = dbContext;
            _appServices = appServices;
            _env = env;
        }

        [ProducesResponseType(typeof(ServiceResponse), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody] AuthModel model)
        {
            var resp = await _appServices.GenerateToken(model);
            if(resp.status){
                var activity = new Activity
                {
                    description = $"{model.username} logs in",
                    userName = model.username,
                };
                await _dbcontext.Activities.AddAsync(activity);
                await _dbcontext.SaveChangesAsync();
                return Ok(resp);
            }
            return BadRequest(resp);
        }

        [ProducesResponseType(typeof(ApiResponse<List<FolderResponseModel>>), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [HttpGet("get-folders")]
        [Authorize]
        public async Task<IActionResult> GetFolders()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _dbcontext.ApplicationUser.FirstOrDefaultAsync(x => x.Id == userId);

            var activity = new Activity
            {
                description = $"{user.UserName} fetch all folders",
                userName = user.UserName,
            };
            await _dbcontext.Activities.AddAsync(activity);
            await _dbcontext.SaveChangesAsync();

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
        [Authorize (Roles ="Admin")]
        public async Task<IActionResult> CreateFolders([FromBody] CreateFolderRequestModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _dbcontext.ApplicationUser.FirstOrDefaultAsync(x => x.Id == userId);

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

            var activity = new Activity
            {
                description = $"{user.UserName} create new folder {model.FolderName}",
                userName = user.UserName,
            };
            await _dbcontext.Activities.AddAsync(activity);
            var dataSaved = await _dbcontext.DocumentFolders.AddAsync(folder);
            int savedCount = await _dbcontext.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                data = dataSaved.Entity,
                status = true,
                message = "Folder retrieved successfully"
            });
        }

        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [HttpPost("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var authUser = await _dbcontext.ApplicationUser.FirstOrDefaultAsync(x => x.Id == userId);

            if (_dbcontext.ApplicationUser.Any(x => x.UserName.ToLower() == model.Username.ToLower()))
            {
                return BadRequest(new ApiResponse
                {
                    status = false,
                    message = "User name in existence"
                });
            }

            if (_dbcontext.ApplicationUser.Any(x => x.Email.ToLower() == model.Email.ToLower()))
            {
                return BadRequest(new ApiResponse
                {
                    status = false,
                    message = "Email in existence"
                });
            }

            var user = new ApplicationUser
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                UserName = model.Username,
                Email = model.Email
            };

            var dataResp = await _appServices.Register(user, model.Password, "user");

            if (dataResp.status)
            {
                var activity = new Activity
                {
                    description = $"{authUser.UserName} created new user {model.Username}",
                    userName = authUser.UserName,
                };
                await _dbcontext.Activities.AddAsync(activity);
                await _dbcontext.SaveChangesAsync();
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

        [ProducesResponseType(typeof(ServiceResponse), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [HttpPost("upload-documents/{folderId}")]
        [Authorize]
        public async Task<IActionResult> UploadDocument(string folderId, [FromForm] List<IFormFile> files)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _dbcontext.ApplicationUser.FirstOrDefaultAsync(x => x.Id == userId);

            var resp = await _appServices.UploadDocument(folderId, Request.Form.Files, _env.WebRootPath, user.UserName);
            if (resp.status)
            {
                var activity = new Activity
                {
                    description = $"{user.UserName} uploaded {Request.Form.Files.Count} documents",
                    userName = user.UserName,
                };

                await _dbcontext.Activities.AddAsync(activity);
                await _dbcontext.SaveChangesAsync();
                return Ok(resp);
            }
            return BadRequest(resp);
        }

        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(NotFoundObjectResult), 404)]
        [HttpGet("get-document/{uniqueDocumentName}")]
        [Authorize]
        public async Task<IActionResult> GetDocument(string uniqueDocumentName){
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _dbcontext.ApplicationUser.FirstOrDefaultAsync(x=>x.Id == userId) ;

            var document = await _dbcontext.Documents.Include(y=> y.documentFolder).FirstOrDefaultAsync(x => x.uniqueDocumentName == uniqueDocumentName);
            if(document is null){
                return NotFound(new ApiResponse
                {
                    status = false,
                    message = "File not found"
                });
            }

            var activity = new Activity
            {
                description = $"{user.UserName} downloaded {uniqueDocumentName}",
                userName = user.UserName,
            };
            
            await _dbcontext.Activities.AddAsync(activity);
            await _dbcontext.SaveChangesAsync();

            var path = Path.Combine(_env.WebRootPath, $"Uploads/{document.documentFolder.folderName}/") + document.uniqueDocumentName;
            var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
            return File(fileBytes, document.documentType, document.documentName);
        }

        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(NotFoundObjectResult), 404)]
        [HttpGet("get-folder-documents/{folderId}")]
        [Authorize]
        public async Task<IActionResult> GetDocumentByFolderId(string folderId){
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _dbcontext.ApplicationUser.FirstOrDefaultAsync(x=>x.Id == userId) ;

            var folder = await _dbcontext.DocumentFolders.FirstOrDefaultAsync(x => x.folderUniqueKey == folderId);
            if(folder is null){
                return BadRequest(new ApiResponse
                {
                    status = false,
                    message = "Folder not found"
                });
            }

            var resp = await _appServices.GetDocuments(folder);
            if(resp.status){
                var activity = new Activity
            {
                description = $"{user.UserName} fetches files in {folder.folderName}",
                userName = user.UserName,
            };
            
            await _dbcontext.Activities.AddAsync(activity);
            await _dbcontext.SaveChangesAsync();
                return Ok(resp);
            }
            return BadRequest(resp);
        }
    }
}