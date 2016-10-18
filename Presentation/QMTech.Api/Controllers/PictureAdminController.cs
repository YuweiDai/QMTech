using QMTech.Api.Models.Media;
using QMTech.Services.Configuration;
using QMTech.Services.Media;
using QMTech.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace QMTech.Api.Controllers
{
    [RoutePrefix("Admin/Media/Pictures")]
    public class PictureAdminController : BaseAdminApiController
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;

        public PictureAdminController(IPictureService pictureService, 
            Services.Configuration.ISettingService settingService)
        {
            _pictureService = pictureService;
            _settingService = settingService;          
        }

        [HttpGet]
        [Route("Init")]
        public IHttpActionResult Test()
        {
            _settingService.SaveSetting(new Nop.Core.Domain.Media.MediaSettings
            {
                AvatarPictureSize = 120,
                ProductThumbPictureSize = 415,
                ProductDetailsPictureSize = 550,
                ProductThumbPictureSizeOnProductDetailsPage = 100,
                AssociatedProductPictureSize = 220,
                CategoryThumbPictureSize = 450,
                ManufacturerThumbPictureSize = 420,
                CartThumbPictureSize = 80,
                MiniCartThumbPictureSize = 70,
                AutoCompleteSearchThumbPictureSize = 20,
                MaximumImageSize = 1980,
                DefaultPictureZoomEnabled = false,
                DefaultImageQuality = 80,
                MultipleThumbDirectories = false
            });

            return Ok("setting initial");
        }

        [HttpPost]
        [Route("Upload")]
        public IHttpActionResult Upload(int size=200)
        {
            var httpRequest = HttpContext.Current.Request;
           
            var response = new List<PictureModel>();

            if (httpRequest.Files.Count > 0)
            {
                #region 遍历文件
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filename = postedFile.FileName;
                    var contentType = postedFile.ContentType;
                    var stream = postedFile.InputStream;

                    // NOTE: To store in memory use postedFile.InputStream
                    var fileBinary = new byte[stream.Length];
                    stream.Read(fileBinary, 0, fileBinary.Length);

                    var fileExtension = Path.GetExtension(filename);
                    if (!string.IsNullOrEmpty(fileExtension))
                        fileExtension = fileExtension.ToLower();
                    //contentType is not always available 
                    //that's why we manually update it here
                    //http://www.sfsu.edu/training/mimetype.htm
                    if (String.IsNullOrEmpty(contentType))
                    {
                        switch (fileExtension)
                        {
                            case ".bmp":
                                contentType = "image/bmp";
                                break;
                            case ".gif":
                                contentType = "image/gif";
                                break;
                            case ".jpeg":
                            case ".jpg":
                            case ".jpe":
                            case ".jfif":
                            case ".pjpeg":
                            case ".pjp":
                                contentType = "image/jpeg";
                                break;
                            case ".png":
                                contentType = "image/png";
                                break;
                            case ".tiff":
                            case ".tif":
                                contentType = "image/tiff";
                                break;
                            default:
                                break;
                        }
                    }

                    var picture = _pictureService.InsertPicture(fileBinary, contentType, null);

                    response.Add(new PictureModel
                    {
                        Id = picture.Id,
                        Url = _pictureService.GetPictureUrl(picture, size)
                    });
                }
                #endregion           
                return Ok(response);
            }
            return BadRequest("无文件上传");

        }
    }
}