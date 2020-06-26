using System.IO;
using System.Threading.Tasks;
using CoreBase.Common.Constant;
using Microsoft.AspNetCore.Http;
using Serilog;
using ServiceStack.Text;

namespace CoreBase.Common.Middleware
{
    public class RequestResponseLog
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private const int ReadChunkBufferLength = 4096;


        public RequestResponseLog(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            Stream originalBody = context.Response.Body;

            using (MemoryStream newResponseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = newResponseBody;

                await _next(context);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalBody);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                LogResponse(context, newResponseBody);
            }
        }

        private void LogResponse(HttpContext context, MemoryStream newResponseBody)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            Log.Information
                (LogTemplates.Response,
                request.Scheme.ToString() + 
                request.Host.ToString() + 
                request.Path.ToString() + 
                request.QueryString.ToString(),
                response.StatusCode,
                ReadStreamInChunks(newResponseBody)
                );

        }

        private async Task LogRequest(HttpContext context)
        {
            HttpRequest request = context.Request;

            Log.Information
                (LogTemplates.Request,
                request.Scheme.ToString() + request.Host.ToString() + request.Path.ToString() + request.QueryString.ToString(),
                await GetRequestBody(request),
                request.Headers
                );

        }

        public async Task<string> GetRequestBody(HttpRequest request)
        {
            request.EnableBuffering(); // instead of request.EnableRewind()
            using var requestStream = _recyclableMemoryStreamManager.GetStream();


            await request.Body.CopyToAsync(requestStream);
            request.Body.Seek(0, SeekOrigin.Begin);
            return ReadStreamInChunks(requestStream);

        }

        private static string ReadStreamInChunks(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string result;
            using (var textWriter = new StringWriter())
            using (var reader = new StreamReader(stream))
            {
                var readChunk = new char[ReadChunkBufferLength];
                int readChunkLength;
                //do while: is useful for the last iteration in case readChunkLength < chunkLength
                do
                {
                    readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                    textWriter.Write(readChunk, 0, readChunkLength);
                } while (readChunkLength > 0);

                result = textWriter.ToString();
            }

            return result;
        }

    }
}
