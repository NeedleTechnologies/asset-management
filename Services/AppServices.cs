using System.Net;
using System.Threading.Tasks;
using asset_management.Entities;
using asset_management.Models;
using AssetManagement.Database;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace asset_management.Services
{
    public class AppServices
    {
        private readonly ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _config;
        private RoleManager<IdentityRole> _roleManager;
        public AppServices(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }

        private async Task<ServiceResponse> CreateAccessToken(ApplicationUser User)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                List<string> role = (List<string>)await _userManager.GetRolesAsync(User);

                var claims = new[] {
                    new Claim (JwtRegisteredClaimNames.Sub, User.Id),
                    new Claim (ClaimTypes.NameIdentifier, User.Id),
                    new Claim (ClaimTypes.Role, role.FirstOrDefault ()),
                    new Claim (ClaimTypes.AuthorizationDecision, role.FirstOrDefault ()),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                    new Claim (JwtRegisteredClaimNames.Iat, new DateTimeOffset (now).ToUnixTimeSeconds ().ToString ())
                };

                int tokenExpiration = _config.GetValue<int>("Auth:Jwt:TokenExpiration");
                var IsserSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _config["Auth:Jwt:Issuer"],
                    audience: _config["Auth:Jwt:Audience"],
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromHours(tokenExpiration)),
                    signingCredentials: new SigningCredentials(IsserSignKey, SecurityAlgorithms.HmacSha256)
                );

                var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

                var response = new AuthResponseModel()
                {
                    token = encodeToken,
                    Name = $"{User.firstName} {User.lastName}",
                };

                return new ServiceResponse { response = response, status = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { response = ex, status = false };
            }
        }

        public async Task<ServiceResponse> GenerateToken(AuthModel model)
        {
            try
            {
                ApplicationUser User = (from user in _dbContext.ApplicationUser where user.UserName.ToLower() == model.username.ToLower() select user).FirstOrDefault();

                if (User == null)
                {
                    return new ServiceResponse { status = false, response = "User record not found" };
                }

                var role = (await _userManager.GetRolesAsync(User)).FirstOrDefault();

                var status = await _userManager.CheckPasswordAsync(User, model.password);

                if (!status)
                {
                    return new ServiceResponse { status = false, response = "Incorrect password supplied" };
                }

                return await CreateAccessToken(User);

            }
            catch (Exception ex)
            {
                return new ServiceResponse { status = false, response = new UnauthorizedResult(), message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Register(ApplicationUser User, string Password, string Role)
        {
            User.Email = "needlesupport@needletech.ng";

            var result = await _userManager.CreateAsync(User, Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role));
                }
                await _userManager.AddToRoleAsync(User, Role);
                return new ServiceResponse { status = true, response = User };
            }
            else
            {
                var error = result.Errors.Select(x => x.Description).ToList();
                return new ServiceResponse { status = false, response = error[0] };
            }
        }

        public async Task<ServiceResponse> UploadDocument(string folderId, IFormFileCollection files, string rootPath, string userName)
        {
            var folder = await _dbContext.DocumentFolders.FirstOrDefaultAsync(x => x.folderUniqueKey == folderId);

            if (folder is null)
            {
                return new ServiceResponse
                {
                    status = false,
                    message = "Folder not found"
                };
            }
            else
            {
                var documents = new List<Document>();
                foreach (var file in files)
                {
                    string uniqueName = Guid.NewGuid().ToString("N") + "-" + Guid.NewGuid().ToString("N") + filetype(file.ContentType);
                    var document = new Document
                    {
                        folderId = folder.Id,
                        documentName = file.FileName,
                        documentType = file.ContentType,
                        uniqueDocumentName = uniqueName,
                        uploadedBy = userName
                    };
                    var directory = Path.Combine(rootPath, $"Uploads/{folder.folderName}");
                    if(!Directory.Exists(directory)){
                        Directory.CreateDirectory(directory);
                    }
                    string path = Path.Combine(rootPath, $"Uploads/{folder.folderName}/") + uniqueName;
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        var bytes = memoryStream.ToArray();
                        await File.WriteAllBytesAsync(path, bytes);
                    }

                    documents.Add(document);
                }

                await _dbContext.Documents.AddRangeAsync(documents);
                await _dbContext.SaveChangesAsync();

                var respDocuments = documents.Select(x => new { 
                    documentName = x.documentName,
                    uniqueName = x.uniqueDocumentName,
                    id = x.Id
                });

                return new ServiceResponse
                {
                    status = true,
                    response = respDocuments,
                    message = "Documents Uploaded Successfully"
                };
            }
        }

        public async Task<ApiResponse> GetDocuments(string folderId){
            var folder = await _dbContext.DocumentFolders.FirstOrDefaultAsync(x => x.folderUniqueKey == folderId);
            if(folder is null){
                return new ApiResponse
                {
                    status = false,
                    message = "Folder not found"
                };
            }
            else{
                var documents = await _dbContext.Documents.Where(x => x.folderId == folder.Id).ToListAsync();
                var resp = new List<GetDocumentsResponseModel>();
                documents.ForEach(x => {
                    var docuResponse = new GetDocumentsResponseModel {
                        uniqueDocumentName = x.uniqueDocumentName,
                        dateUploaded = x.dateCreated,
                        folderName = folder.folderName,
                        documentName = x.documentName,
                        uploadedBy = x.uploadedBy
                     };

                    resp.Add(docuResponse);
                });

                return new ApiResponse
                {
                    data = resp,
                    status = true,
                    message = "Documents retrieved successfully"
                };
            }

        }

        private string filetype (string cols) {
            switch (cols) {
                case "application/pdf":
                    return ".pdf";
                case "text/plain":
                    return ".txt";
                case "image/png":
                    return ".png";
                case "image/gif":
                    return ".gif";
                case "image/jpeg":
                    return ".jpg";
                case "application/msword":
                    return ".doc";
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return ".docx";
                default:
                    return ".docx";
            }
        }
    }
}