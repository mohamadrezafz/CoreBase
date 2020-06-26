using CoreBase.Common.Constant;
using Newtonsoft.Json;
using Serilog;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBase.Common.RestfulHelper
{
   

    public static class RestfulHelper
    {
        public static ServiceResult<TResult> Get<TResult>(
            GetRestfulRequest getRestful
        )
        {
            try
            {
                var request = CreateRequest(getRestful);
                var response = request.Item1.GetAsync(request.Item2.ToString()).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                (LogTemplates.Response,
                request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                response.StatusCode,
                result.SerializeToJson()
                );

                TResult data;
                if (result.IsNullOrWhiteSpace() || response.StatusCode != HttpStatusCode.OK)
                    data = default;
                else
                    data = JsonConvert.DeserializeObject<TResult>(
                        result,
                        new JsonSerializerSettings
                        {
                            Error = (s, a) => { a.ErrorContext.Handled = true; }
                        }
                    );

                return new ServiceResult<TResult>
                {
                    Data = data,
                    HttpStatus = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                };

            }
            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                    Error = ex.Message,

                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                    Error = ex.Message
                };
            }

        }

        public static async Task<ServiceResult<TResult>> GetAsync<TResult>(
            GetRestfulRequest getRestful
        )
        {
            try
            {
                var request = CreateRequest(getRestful);
                var response = await request.Item1.GetAsync(request.Item2.ToString());
                var result = await response.Content.ReadAsStringAsync();

                Log.Information
                (LogTemplates.Response,
                request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                response.StatusCode,
                result.SerializeToJson()
                );

                TResult data;
                if (result.IsNullOrWhiteSpace() || response.StatusCode != HttpStatusCode.OK)
                    data = default;
                else
                    data = JsonConvert.DeserializeObject<TResult>(
                        result,
                        new JsonSerializerSettings
                        {
                            Error = (s, a) => { a.ErrorContext.Handled = true; }
                        }
                    );

                return new ServiceResult<TResult>
                {
                    Data = data,
                    HttpStatus = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                };
            }
            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                    Error = ex.Message,
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                    Error = ex.Message,

                };
            }


        }

        public static ServiceResult Get(
            GetRestfulRequest getRestful
        )
        {
            try
            {
                var request = CreateRequest(getRestful);
                var response = request.Item1.GetAsync(request.Item2.ToString()).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                (LogTemplates.Response,
                 request.Item1.BaseAddress.Scheme +request.Item1.BaseAddress.Host +request.Item1.BaseAddress.PathAndQuery,
                response.StatusCode
                );

                return new ServiceResult
                {
                    Data = result,
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                };

            }
            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    Error = ex,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }

        }

        public static async Task<ServiceResult> GetAsync(
            GetRestfulRequest getRestful
        )
        {
            try
            {
                var request = CreateRequest(getRestful);
                var response = await request.Item1.GetAsync(request.Item2.ToString());
                var result = await response.Content.ReadAsStringAsync();

                Log.Information
                (LogTemplates.Response,
                request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                response.StatusCode,
                result.SerializeToJson()
                );

                return new ServiceResult
                {
                    Data = result,
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                };
            }
            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    Error = ex,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }



        }

        public static ServiceResult<TResult> Post<TResult>(
            PostRestfulRequest postRestful
        )
        {
            try
            {
                var request = CreateRequest(postRestful);
                var response = request.Item1.PostAsync(request.Item2.ToString(), request.Item3).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                (LogTemplates.Response,
                request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                response.StatusCode,
                result.SerializeToJson()
                );

                var error = string.Empty;

                TResult data;
                if (string.IsNullOrEmpty(result))
                    data = default;
                else
                    data = JsonConvert.DeserializeObject<TResult>(result,
                        new JsonSerializerSettings { Error = (s, a) => { a.ErrorContext.Handled = true; } });

                return new ServiceResult<TResult>
                {
                    Data = data,
                    HttpStatus = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    Error = error,
                };

            }

            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }

        }

        public static async Task<ServiceResult<TResult>> PostAsync<TResult>(
            PostRestfulRequest postRestful
        )
        {
            try
            {
                var request = CreateRequest(postRestful);
                var response = await request.Item1.PostAsync(request.Item2.ToString(), request.Item3);
                var result = await response.Content.ReadAsStringAsync();

                Log.Information
                (LogTemplates.Response,
                request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                response.StatusCode,
                result.SerializeToJson()
                );

                var error = string.Empty;

                TResult data;
                if (string.IsNullOrEmpty(result))
                    data = default;
                else
                    data = JsonConvert.DeserializeObject<TResult>(result,
                      new JsonSerializerSettings { Error = (s, a) => { a.ErrorContext.Handled = true; } });

                return new ServiceResult<TResult>
                {
                    Data = data,
                    HttpStatus = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    Error = error,
                };

            }

            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }




        }

        public static ServiceResult Post(
            PostRestfulRequest postRestful
        )
        {
            try
            {
                var request = CreateRequest(postRestful);
                var response = request.Item1.PostAsync(request.Item2.ToString(), request.Item3).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                (LogTemplates.Response,
                request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                response.StatusCode,
                result.SerializeToJson()
                );


                return new ServiceResult
                {
                    Data = result,
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                };

            }

            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    Error = ex,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }


        }

        public static async Task<ServiceResult> PostAsync(
            PostRestfulRequest postRestful
        )
        {
            try
            {
                var request = CreateRequest(postRestful);
                var response = await request.Item1.PostAsync(request.Item2.ToString(), request.Item3);
                var result = await response.Content.ReadAsStringAsync();


                Log.Information
                (LogTemplates.Response,
                request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                response.StatusCode,
                result.SerializeToJson()
                );


                return new ServiceResult
                {
                    Data = result,
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                };
            }

            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    Error = ex,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }


        }

        public static ServiceResult<TResult> Put<TResult>(
            PutRestfulRequest putRestful
        )
        {
            try
            {
                var request = CreateRequest(putRestful);
                var response = request.Item1.PutAsync(request.Item2.ToString(), request.Item3).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                 (LogTemplates.Response,
                 request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                 response.StatusCode,
                 result.SerializeToJson()
                 );


                var error = string.Empty;

                TResult data;
                if (string.IsNullOrEmpty(result))
                    data = default;
                else
                    data = JsonConvert.DeserializeObject<TResult>(result,
                        new JsonSerializerSettings { Error = (s, a) => { a.ErrorContext.Handled = true; } });

                return new ServiceResult<TResult>
                {
                    Data = data,
                    HttpStatus = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    Error = error,
                };

            }

            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }


        }

        public static async Task<ServiceResult<TResult>> PutAsync<TResult>(
            PutRestfulRequest putRestful
        )
        {
            try
            {

                var request = CreateRequest(putRestful);
                var response = await request.Item1.PutAsync(request.Item2.ToString(), request.Item3);
                var result = await response.Content.ReadAsStringAsync();

                Log.Information
                 (LogTemplates.Response,
                 request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                 response.StatusCode,
                 result.SerializeToJson()
                 );

                var error = string.Empty;

                TResult data;
                if (string.IsNullOrEmpty(result))
                    data = default;
                else
                    data = JsonConvert.DeserializeObject<TResult>(result,
                        new JsonSerializerSettings { Error = (s, a) => { a.ErrorContext.Handled = true; } });

                return new ServiceResult<TResult>
                {
                    Data = data,
                    HttpStatus = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    Error = error,
                };
            }
            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult<TResult>
                {
                    Data = default,
                    HttpStatus = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }

        }

        public static ServiceResult Put(
             PutRestfulRequest putRestful
        )
        {
            try
            {
                var request = CreateRequest(putRestful);
                var response = request.Item1.PutAsync(request.Item2.ToString(), request.Item3).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                 (LogTemplates.Response,
                 request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                 response.StatusCode,
                 result.SerializeToJson()
                 );

                return new ServiceResult
                {
                    Data = result,
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                };
            }
            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    Error = ex,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }



        }

        public static async Task<ServiceResult> PutAsync(
             PutRestfulRequest putRestful
        )
        {
            try
            {

                var request = CreateRequest(putRestful);
                var response = await request.Item1.PutAsync(request.Item2.ToString(), request.Item3);
                var result = await response.Content.ReadAsStringAsync();


                Log.Information
                 (LogTemplates.Response,
                  request.Item1.BaseAddress.Scheme + request.Item1.BaseAddress.Host + request.Item1.BaseAddress.PathAndQuery,
                 response.StatusCode,
                 result.SerializeToJson()
                 );

                return new ServiceResult
                {
                    Data = result,
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                };

            }


            catch (TimeoutException ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    ReasonPhrase = "",
                };
            }
            catch (Exception ex)
            {
                Log.Error(
                ex,
                LogTemplates.Exception
                );
                return new ServiceResult
                {
                    Data = default,
                    Error = ex,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "",
                };
            }
        }


        private static Tuple<HttpClient, StringBuilder, HttpContent> CreateRequest(PostRestfulRequest postRestful)
        {
            var client = new HttpClient { BaseAddress = new Uri(postRestful.Url) };
            if (Math.Abs(postRestful.Timeout.TotalSeconds) > 0)
            {
                client.Timeout = postRestful.Timeout;
            }
            if (postRestful.BypassCertificate)
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            if (postRestful.Headers != null)
                foreach (var header in postRestful.Headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

            var param = new StringBuilder();
            if (postRestful.Parameters != null)
            {
                param.Append("?");
                param.Append(string.Join("&", postRestful.Parameters.Select(par => $"{par.Key}={par.Value}")));
            }
            HttpContent serviceContent;
            switch (postRestful.Format)
            {
                case InputFormat.Json:
                    serviceContent = new StringContent(postRestful.Content, Encoding.UTF8, "application/json");
                    break;
                case InputFormat.FromUrlEncoded:
                    serviceContent = new FormUrlEncodedContent(postRestful.Content);
                    break;
                case InputFormat.FromData:
                    serviceContent = new FormUrlEncodedContent(postRestful.Content);
                    break;
                case InputFormat.Xml:
                    serviceContent = new FormUrlEncodedContent(postRestful.Content);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(postRestful.Format), postRestful.Format, null);
            }

            Log.Information
            (LogTemplates.Request,
            client.BaseAddress.Scheme + client.BaseAddress.Host + client.BaseAddress.PathAndQuery,
            postRestful.Content,
            JsonConvert.SerializeObject(postRestful.Headers)
            );

            return Tuple.Create(client, param, serviceContent);

        }

        public static Tuple<HttpClient, StringBuilder> CreateRequest(GetRestfulRequest getRestful)
        {
            var client = new HttpClient { BaseAddress = new Uri(getRestful.Url) };
            if (Math.Abs(getRestful.Timeout.TotalSeconds) > 0)
            {
                client.Timeout = getRestful.Timeout;
            }
            if (getRestful.BypassCertificate)
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            if (getRestful.Headers != null)
                getRestful.Headers.ForEach(h =>
                {
                    client.DefaultRequestHeaders.Add(h.Key, h.Value);
                });
            var param = new StringBuilder();
            if (getRestful.Parameters != null)
            {
                param.Append("?");
                param.Append(string.Join("&", getRestful.Parameters.Select(par => $"{par.Key}={par.Value}")));
            }

            Log.Information
            (LogTemplates.Request,
            client.BaseAddress.Scheme + client.BaseAddress.Host + param.ToString() + client.BaseAddress.PathAndQuery,
            "",
            JsonConvert.SerializeObject(getRestful.Headers)
            );

            return Tuple.Create(client, param);

        }
    }
}
