using QMTech.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QMTech.Api.Models.Media
{
    /// <summary>
    /// 图片对象
    /// </summary>
    public class PictureModel : BaseQMEntityModel
    {
        public int PicutreId { get; set; }

        public string Url { get; set; }
    }
}