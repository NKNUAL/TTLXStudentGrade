using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class QuestionsHelper
    {
        #region 单例
        private static readonly object padlock = new object();
        private static QuestionsHelper _helper;

        public static QuestionsHelper Instance
        {
            get
            {
                if (_helper == null)
                {
                    lock (padlock)
                    {
                        if (_helper == null)
                        {
                            _helper = new QuestionsHelper();
                        }
                    }
                }
                return _helper;
            }
        }

        private QuestionsHelper()
        {

        } 
        #endregion

        public string SaveQueImage(string paperId, string queNo, byte[] imageByte, ImageSource imageSource)
        {

            if (imageByte == null) return "";

            string fileUrlName = $"/Data/QueImage/{paperId}/{queNo}";

            string dirpath = System.Web.Hosting.HostingEnvironment.MapPath("~" + fileUrlName);

            if (!Directory.Exists(dirpath)) Directory.CreateDirectory(dirpath);

            string filename = string.Empty;
            switch (imageSource)
            {
                case ImageSource.OptionA:
                    filename = "OptionA.png";
                    break;
                case ImageSource.OptionB:
                    filename = "OptionB.png";
                    break;
                case ImageSource.OptionC:
                    filename = "OptionC.png";
                    break;
                case ImageSource.OptionD:
                    filename = "OptionD.png";
                    break;
                case ImageSource.QueContent:
                    filename = "QueContent.png";
                    break;
            }
            string filepath = Path.Combine(dirpath, filename);


            return BinaryStreamToPicture(imageByte, filepath) ? Path.Combine(fileUrlName, filename) : "";

        }

        private bool BinaryStreamToPicture(byte[] streamByte, string filepath)
        {
            try
            {
                MemoryStream ms = new MemoryStream(streamByte);
                Image img = Image.FromStream(ms);
                img.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
    public enum ImageSource
    {
        QueContent,
        OptionA,
        OptionB,
        OptionC,
        OptionD
    }
}
