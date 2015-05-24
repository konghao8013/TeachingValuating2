using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    [Serializable]
    public class UploadFileType
    {
        public string Key { set; get; }
        public string Url { set; get; }
        public string Name { set; get; }
        public long Size { set; get; }
        public DateTime Date { set; get; }
    }
}
