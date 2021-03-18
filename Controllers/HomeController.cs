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
            _logger.LogDebug("Logging info from current request");
            PostRequest request = await ReadPipeAsync(httpContext.Request.BodyReader);
            request.display();
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
        const uint CONTENTINFO = 0;
        const uint IMAGE = 1;
        const uint ENDTEXT = 2;
        async Task<PostRequest> ReadPipeAsync(PipeReader reader)
        {
            PostRequest pr = new PostRequest(_logger);
            string image = "";
            uint index = 0;
            while (true)
            {
                ReadResult result = await reader.ReadAsync();


                ReadOnlySequence<byte> buffer = result.Buffer;
                SequencePosition? position = null;


                do
                {
                    // Look for a EOL in the buffer
                    position = buffer.PositionOf((byte)'\n');


                    if (position != null)
                    {
                        string currLine = ProcessLine(buffer.Slice(0, position.Value), index == IMAGE);
                        Console.WriteLine();
                        if (currLine == null || currLine == "\r")
                        {
                            index++;
                        }
                        else
                        {
                            if (!currLine.StartsWith("------WebKitFormBoundary"))
                            {
                                switch (index)
                                {
                                    case CONTENTINFO:
                                        pr.ContentInfo += currLine;
                                        break;
                                    case IMAGE:
                                        //pr.Img = new Image("null");
                                        image += currLine;
                                        break;
                                    case ENDTEXT:
                                        pr.WeirdTextAtTheEnd += currLine;
                                        break;
                                }
                            }
                        }
                        
                        // Process the line


                        // Skip the line + the \n character (basically position)
                        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                    }
                }
                while (position != null);


                // Tell the PipeReader how much of the buffer we have consumed
                reader.AdvanceTo(buffer.Start, buffer.End);


                // Stop reading if there's no more data coming
                if (result.IsCompleted)
                {
                    break;
                }
            }


            // Mark the PipeReader as complete
            reader.Complete();
            return pr;
        }

        private string ProcessLine(ReadOnlySequence<byte> readOnlySequence, Boolean isImage)
        {
            var sb = new StringBuilder();
            var processed = 0L;
            var total = readOnlySequence.Length;
            if (isImage)
            {
                byte[] bytearr = readOnlySequence.ToArray<Byte>();
                byte[] betterbytearr = new byte[bytearr.Length + 1];
                bytearr.CopyTo(betterbytearr, 0);
                betterbytearr[bytearr.Length] = 10;
                string base64String = Convert.ToBase64String(betterbytearr, 0, betterbytearr.Length);
                return base64String.Replace("=", "");
            }
            else
            {
                Decoder decoder = Encoding.UTF8.GetDecoder();


                foreach (var i in readOnlySequence)
                {
                    processed += i.Length;
                    var isLast = processed == total;
                    var span = i.Span;
                    var charCount = decoder.GetCharCount(span, isLast);
                    Span<char> buffer = stackalloc char[charCount];
                    decoder.GetChars(span, buffer, isLast);
                    sb.Append(buffer);
                }
                return sb.ToString();
            }
        }
    }
}
