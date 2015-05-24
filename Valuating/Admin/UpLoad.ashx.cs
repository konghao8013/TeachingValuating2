using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALOS.Model;
using Valuating.Controllers;

namespace Valuating.Admin
{
    /// <summary>
    /// UpLoad 的摘要说明
    /// </summary>
    public class UpLoad : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (VT.Users == null)
            {
                return;
            }
            
            var list = new List<UploadFileType>();
           
            if (context.Request.Files != null&&context.Request.Files.Count>0)
            {
                
                var keys = context.Request.Files.Keys;
                var length = keys.Count;
                for (int i = 0; i < length; i++)
                {
                    var file = context.Request.Files[keys[i]];
                    var fileModel = new UploadFileType();
                    fileModel.Key = keys[i];
                    if (file.InputStream.Length > 0)
                    {
                        fileModel.Size = file.InputStream.Length;
                        fileModel.Name = file.FileName.Split('/').Last();
                        fileModel.Date = DateTime.Now;
                        var path = "/resource/upload/" + Guid.NewGuid().ToString("N") + "." +
                             file.FileName.Split('.').Last();

                        fileModel.Url = path;
                        file.SaveAs(context.Server.MapPath("~" + path));
                    }
                  
                    list.Add(fileModel);
                   
                
                }
            }
          
            context.Response.Write(list.Serialize());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}