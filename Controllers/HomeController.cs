using ImageFilterASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ImageFilterASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            ParseRequestAsync(HttpContext);
            return View();
        }

        private async void ParseRequestAsync(HttpContext httpContext)
        { 
            //HttpListenerRequest
            _logger.LogDebug("Logging info from current request");
            var result = await ReadPipeAsync(httpContext.Request);
            PostRequest request = result.Item1;
            if (httpContext.Request.ContentLength.HasValue)
            {
                request.ContentLength = httpContext.Request.ContentLength.Value;
                _logger.LogInformation($"post: {request.ContentLength}; method: {result.Item2} difference: {request.ContentLength- result.Item2} ");
                
            }
            else
            {
                //something probably went wrong
            }

            //request.display();
            /*for(int i = 0; i < headers.Length; i++)
            {
            _logger.LogInformation($"{i}: {headers[i]}");
            }*/
           

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /*const uint CONTENTINFO = 0;
        const uint IMAGE = 1;
        const uint ENDTEXT = 2;*/
        async Task<Tuple<PostRequest, long>> ReadPipeAsync(HttpRequest request)
        {
            PostRequest pr = new PostRequest(_logger);
            long contentLength = 0;
            string result;
            Int32 position = 0;
            StringBuilder sb = new StringBuilder();
            if (request.ContentLength.HasValue)
            {
               
                char[] buffer;
                var lastPos = 0;
                int index = 0;
                //File f = new File();
                var fs = new FileStream("text.png", FileMode.Create);
                
                PipeReader reader = request.BodyReader;
                var resBuf = reader.ReadAsync().Result.Buffer;
                

                _ = await Task.FromResult(Write(fs, trimRequest(resBuf)));
                bool Write(FileStream fs, byte[] buf)
                {
                    
                    ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(buf);
                    fs.Write(bytes);
                    return true;
                }
                fs.Close();
                

                /* using (System.IO.StreamReader reader = new System.IO.StreamReader(request.Body))
                {
                    do
                    {
                        buffer = new char[512];
                        /*for(int i = 0; i < buffer.Length; i++)
                        {
                            if(buffer[i] == null)
                            {

                            }
                        }
                        position = reader.ReadAsync(buffer, 0, buffer.Length).Result;
                        fileStream.Write();
                        _logger.LogInformation($"position: {position}");
                        if (position != buffer.Length)
                        {
                            Array.Resize<char>(ref buffer, position);
                        }
              
                        var data = new string(buffer);
                        if (data.Contains("/r"))
                        {
                            index++;
                            _logger.LogInformation($"new index: {index}");
                        }
                        sb.Append(data);
                        contentLength += position;
                    } while (position != 0);
                    //result = reader.ReadToEndAsync().Result;
                    
                    _logger.LogInformation(sb.ToString());

                }*/
            }
            
            


            _logger.LogInformation($"full requset: {sb.ToString()}");
            return Tuple.Create(pr, contentLength);
        }
        public byte[] trimRequest(ReadOnlySequence<byte> request)
        {
            var startString = "\r\n\r\n";
            var endString = "\r\n------WebKitFormBoundary";
            byte[] bytes = request.ToArray();
            var str = System.Text.Encoding.Default.GetString(bytes);
            int startPos = str.IndexOf(startString);
            startPos += startString.Length;
            int endPos = str.IndexOf(endString);
            
            return bytes[startPos..endPos];
        }
    }
}
