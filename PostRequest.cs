using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageFilterASP
{
    public class PostRequest
    {
        private long contentLength;
        private string contentInfo;  
        private Image img;
        private string imgSrt;
        private FileStream file;
        private string weirdTextAtTheEnd;
        private readonly ILogger _logger;

        public string ContentInfo { get => contentInfo; set => contentInfo = value; }
        public Image Img { get => img; set => img = value; }
        public string WeirdTextAtTheEnd { get => weirdTextAtTheEnd; set => weirdTextAtTheEnd = value; }
        public long ContentLength { get => contentLength; set => contentLength = value; }
        public string ImgSrt { get => imgSrt; set => imgSrt = value; }

        public PostRequest(ILogger _logger)
        {
            this._logger = _logger;
        }

        public PostRequest(string contentInfo, string imgText, string weirdTextAtTheEnd, ILogger logger)
        {
            this._logger = logger;
            this.ContentInfo = contentInfo;
            this.WeirdTextAtTheEnd = weirdTextAtTheEnd;
            this.Img = new Image(imgText);
        }
        public void display()
        {
            _logger.LogInformation($"content length: {this.contentLength}");
            _logger.LogInformation($"content info: {this.contentInfo}");
            _logger.LogInformation($"image: {this.imgSrt}");
            _logger.LogInformation($"weirdTextAtTheEnd: {this.weirdTextAtTheEnd}");
        }
        
    }
}
