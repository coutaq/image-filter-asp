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
            var req = httpContext.Request;
            
            if (httpContext.Request.ContentLength.HasValue)
            {
                _logger.LogInformation($"content lenghth: {httpContext.Request.ContentLength.Value}");
                //_logger.LogInformation($"post: {request.ContentLength}; method: {result.Item2} difference: {request.ContentLength- result.Item2} ");
                
            }
            else
            {
                //something probably went wrong
            }
           // try
            //{

                var result = await ReadPipeAsync(req);
                PostRequest request = result.Item1;
                
            //}
            //catch
            //{
                _logger.LogInformation("ERROR");
            //}
            

            

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
            _logger.LogInformation("Accepting request 123");
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
                var sw = new StreamWriter(Console.OpenStandardOutput());
                sw.AutoFlush = true;
                Console.SetOut(sw);
                PipeReader reader = request.BodyReader;
                PostStream ps = new PostStream();
                var resBuf = reader.AsStream();
                //resBuf.CopyTo(fs);
                resBuf.CopyTo(ps);
                

                //_ = await Task.FromResult(Write(fs, trimRequest(resBuf)));
                //
                bool Write(FileStream fs, byte[] buf)
                { 
                    //ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(buf);
                    //fs.Write(bytes);
                    return true;
                }
                fs.Close();
            }
            
            


            _logger.LogInformation($"full requset: {sb.ToString()}");
            return Tuple.Create(pr, contentLength);
        }
        public byte[] trimRequest(ReadOnlySequence<byte> request)
        {


            var startString = "\r\n\r\n";
            var endString = "\r\n-------";
            byte[] bytes = new byte[99999999];
            //bytes = request.ToArray();

            var decoder = Encoding.UTF8.GetDecoder();
            var sb = new StringBuilder();
            var processed = 0L;
            var total = request.Length;
            foreach (var i in request)
            {
                processed += i.Length;
                var isLast = processed == total;
                var span = i.Span;
                var charCount = decoder.GetCharCount(span, isLast);
                Span<char> buffer = stackalloc char[charCount];
                decoder.GetChars(span, buffer, isLast);
                sb.Append(buffer);
            }

            //var str = System.Text.Encoding.Default.GetString(bytes);
            var str = sb.ToString();
            int startPos = str.IndexOf(startString);
             _logger.LogInformation(str+"//1end-of-file");
            startPos += startString.Length;
            int endPos = str.IndexOf(endString);
            
            return bytes[startPos..endPos];
        }
    }
}
