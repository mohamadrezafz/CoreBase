using CoreBase.Common.Constant;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreBase.Common.RestfulHelper
{
    public static class RestfulHelper
    {
        public static ServiceResult<TResult> Get<TResult>(
            string url,
            List<KeyValuePair<string, string>> parameters = null,
            List<KeyValuePair<string, string>> headers = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    headers.ForEach(h =>
                    {
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    });
                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }
                Log.Information
                (LogTemplates.Request,
                client.BaseAddress.Scheme + client.BaseAddress.Host + param.ToString() + client.BaseAddress.PathAndQuery,
                "",
                JsonConvert.SerializeObject(headers)
                );


                var response = client.GetAsync(param.ToString()).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                (LogTemplates.Response,
                client.BaseAddress.Scheme + 
                client.BaseAddress.Host + 
                client.BaseAddress.PathAndQuery,
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
            string url,
            List<KeyValuePair<string, string>> parameters = null,
            List<KeyValuePair<string, string>> headers = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    headers.ForEach(h =>
                    {
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    });
                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }

                Log.Information
                (LogTemplates.Request,
                client.BaseAddress.Scheme + client.BaseAddress.Host + param.ToString() + client.BaseAddress.PathAndQuery,
                "",
                JsonConvert.SerializeObject(headers)
                );

                var response = await client.GetAsync(param.ToString());
                var result = await response.Content.ReadAsStringAsync();

                Log.Information
                (LogTemplates.Response,
                client.BaseAddress.Scheme + 
                client.BaseAddress.Host +
                client.BaseAddress.PathAndQuery,
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
            string url,
            List<KeyValuePair<string, string>> parameters = null,
            List<KeyValuePair<string, string>> headers = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    headers.ForEach(h =>
                    {
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    });
                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }

                Log.Information
                (LogTemplates.Request + client.BaseAddress.Scheme + client.BaseAddress.Host + param.ToString() + client.BaseAddress.PathAndQuery,
                "",
                JsonConvert.SerializeObject(headers)
                );

                var response = client.GetAsync(param.ToString()).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                (LogTemplates.Response,
                client.BaseAddress.Scheme + 
                client.BaseAddress.Host +
                client.BaseAddress.PathAndQuery,
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
            string url,
            List<KeyValuePair<string, string>> parameters = null,
            List<KeyValuePair<string, string>> headers = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {

                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    headers.ForEach(h =>
                    {
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    });
                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }

                Log.Information
                (LogTemplates.Request ,
                client.BaseAddress.Scheme +
                client.BaseAddress.Host +
                param.ToString() + 
                client.BaseAddress.PathAndQuery,
                "",
                JsonConvert.SerializeObject(headers)
                );

                var response = await client.GetAsync(param.ToString());
                var result = await response.Content.ReadAsStringAsync();

                Log.Information
                (LogTemplates.Response,
                client.BaseAddress.Scheme +
                client.BaseAddress.Host +
                client.BaseAddress.PathAndQuery,
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
            string url,
            InputFormat format,
            List<KeyValuePair<string, string>> headers = null,
            List<KeyValuePair<string, string>> parameters = null,
            dynamic content = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(url) })
                {
                    if (Math.Abs(timeout.TotalSeconds) > 0)
                    {
                        client.Timeout = timeout;
                    }
                    if (bypassCertificate)
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    if (headers != null)
                        headers.ForEach(h =>
                        {
                            client.DefaultRequestHeaders.Add(h.Key, h.Value);
                        });

                    var param = new StringBuilder();
                    if (parameters != null)
                    {
                        param.Append("?");
                        param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                    }
                    HttpContent serviceContent;

                    switch (format)
                    {
                        case InputFormat.Json:
                            serviceContent = new StringContent(content, Encoding.UTF8, "application/json");
                            break;
                        case InputFormat.FromUrlEncoded:
                            serviceContent = new FormUrlEncodedContent(content);
                            break;
                        case InputFormat.FromData:
                            serviceContent = new FormUrlEncodedContent(content);
                            break;
                        case InputFormat.Xml:
                            serviceContent = new FormUrlEncodedContent(content);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(format), format, null);
                    }
                    Log.Information
                    (LogTemplates.Request,
                    client.BaseAddress.Scheme +
                    client.BaseAddress.Host +
                    param.ToString() +
                    client.BaseAddress.PathAndQuery,
                    content,
                    JsonConvert.SerializeObject(headers)
                    );

                    var response = client.PostAsync(param.ToString(), serviceContent).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    Log.Information
                    (LogTemplates.Response,
                    client.BaseAddress.Scheme +
                    client.BaseAddress.Host +
                    client.BaseAddress.PathAndQuery,
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
            string url,
            InputFormat format,
            List<KeyValuePair<string, string>> headers = null,
            List<KeyValuePair<string, string>> parameters = null,
            dynamic content = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    headers.ForEach(h =>
                    {
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    });

                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }
                HttpContent serviceContent;

                switch (format)
                {
                    case InputFormat.Json:
                        serviceContent = new StringContent(content, Encoding.UTF8, "application/json");
                        break;
                    case InputFormat.FromUrlEncoded:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.FromData:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.Xml:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }

                Log.Information
                (LogTemplates.Request,
                client.BaseAddress.Scheme + 
                client.BaseAddress.Host + 
                param.ToString() +
                client.BaseAddress.PathAndQuery,
                content,
                JsonConvert.SerializeObject(headers)
                );

                var response = await client.PostAsync(param.ToString(), serviceContent);
                var result = await response.Content.ReadAsStringAsync();

                Log.Information
                (LogTemplates.Response,
                client.BaseAddress.Scheme +
                client.BaseAddress.Host +
                client.BaseAddress.PathAndQuery,
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
            string url,
            InputFormat format,
            List<KeyValuePair<string, string>> parameters = null,
            List<KeyValuePair<string, string>> headers = null,
            dynamic content = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }
                HttpContent serviceContent;
                switch (format)
                {
                    case InputFormat.Json:
                        serviceContent = new StringContent(content, Encoding.UTF8, "application/json");
                        break;
                    case InputFormat.FromUrlEncoded:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.FromData:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.Xml:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }

                Log.Information
                (LogTemplates.Request,
                client.BaseAddress.Scheme + 
                client.BaseAddress.Host +
                param.ToString() +
                client.BaseAddress.PathAndQuery,
                content,
                JsonConvert.SerializeObject(headers)
                );

                var response = client.PostAsync(param.ToString(), serviceContent).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                (LogTemplates.Response,
                client.BaseAddress.Scheme +
                client.BaseAddress.Host +
                client.BaseAddress.PathAndQuery,
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
            string url,
            InputFormat format,
            List<KeyValuePair<string, string>> parameters = null,
            List<KeyValuePair<string, string>> headers = null,
            dynamic content = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }
                HttpContent serviceContent;
                switch (format)
                {
                    case InputFormat.Json:
                        serviceContent = new StringContent(content, Encoding.UTF8, "application/json");
                        break;
                    case InputFormat.FromUrlEncoded:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.FromData:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.Xml:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }

                Log.Information
                (LogTemplates.Request,
                client.BaseAddress.Scheme + 
                client.BaseAddress.Host + 
                param.ToString() + 
                client.BaseAddress.PathAndQuery,
                content,
                JsonConvert.SerializeObject(headers)
                );


                var response = await client.PostAsync(param.ToString(), serviceContent);
                var result = await response.Content.ReadAsStringAsync();


                Log.Information
                (LogTemplates.Response,
                client.BaseAddress.Scheme +
                client.BaseAddress.Host +
                client.BaseAddress.PathAndQuery,
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
            string url,
            InputFormat format,
            List<KeyValuePair<string, string>> headers = null,
            List<KeyValuePair<string, string>> parameters = null,
            dynamic content = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(url) })
                {
                    if (Math.Abs(timeout.TotalSeconds) > 0)
                    {
                        client.Timeout = timeout;
                    }
                    if (bypassCertificate)
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    if (headers != null)
                        headers.ForEach(h =>
                        {
                            client.DefaultRequestHeaders.Add(h.Key, h.Value);
                        });

                    var param = new StringBuilder();
                    if (parameters != null)
                    {
                        param.Append("?");
                        param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                    }
                    HttpContent serviceContent;

                    switch (format)
                    {
                        case InputFormat.Json:
                            serviceContent = new StringContent(content, Encoding.UTF8, "application/json");
                            break;
                        case InputFormat.FromUrlEncoded:
                            serviceContent = new FormUrlEncodedContent(content);
                            break;
                        case InputFormat.FromData:
                            serviceContent = new FormUrlEncodedContent(content);
                            break;
                        case InputFormat.Xml:
                            serviceContent = new FormUrlEncodedContent(content);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(format), format, null);
                    }

                    Log.Information
                    (LogTemplates.Request,
                    client.BaseAddress.Scheme +
                    client.BaseAddress.Host +
                    param.ToString() +
                    client.BaseAddress.PathAndQuery,
                    content,
                    JsonConvert.SerializeObject(headers)
                    );

                    var response = client.PutAsync(param.ToString(), serviceContent).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    Log.Information
                     (LogTemplates.Response,
                     client.BaseAddress.Scheme + 
                     client.BaseAddress.Host +
                     client.BaseAddress.PathAndQuery,
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
            string url,
            InputFormat format,
            List<KeyValuePair<string, string>> headers = null,
            List<KeyValuePair<string, string>> parameters = null,
            dynamic content = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    headers.ForEach(h =>
                    {
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    });

                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }
                HttpContent serviceContent;

                switch (format)
                {
                    case InputFormat.Json:
                        serviceContent = new StringContent(content, Encoding.UTF8, "application/json");
                        break;
                    case InputFormat.FromUrlEncoded:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.FromData:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.Xml:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }

                Log.Information
                (LogTemplates.Request,
                client.BaseAddress.Scheme  +
                client.BaseAddress.Host + 
                param.ToString() + 
                client.BaseAddress.PathAndQuery,
                content,
                JsonConvert.SerializeObject(headers)
                );


                var response = await client.PutAsync(param.ToString(), serviceContent);
                var result = await response.Content.ReadAsStringAsync();



                Log.Information
                 (LogTemplates.Response,
                 client.BaseAddress.Scheme + 
                 client.BaseAddress.Host +
                 client.BaseAddress.PathAndQuery,
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
            string url,
            InputFormat format,
            List<KeyValuePair<string, string>> parameters = null,
            List<KeyValuePair<string, string>> headers = null,
            dynamic content = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }
                HttpContent serviceContent;
                switch (format)
                {
                    case InputFormat.Json:
                        serviceContent = new StringContent(content, Encoding.UTF8, "application/json");
                        break;
                    case InputFormat.FromUrlEncoded:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.FromData:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.Xml:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }

                Log.Information
                (LogTemplates.Request,
                client.BaseAddress.Scheme + 
                client.BaseAddress.Host + 
                param.ToString() + 
                client.BaseAddress.PathAndQuery,
                content,
                JsonConvert.SerializeObject(headers)
                );

                var response = client.PutAsync(param.ToString(), serviceContent).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Log.Information
                 (LogTemplates.Response,
                 client.BaseAddress.Scheme +
                 client.BaseAddress.Host +
                 client.BaseAddress.PathAndQuery,
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
            string url,
            InputFormat format,
            List<KeyValuePair<string, string>> parameters = null,
            List<KeyValuePair<string, string>> headers = null,
            dynamic content = null,
            bool bypassCertificate = false,
            TimeSpan timeout = new TimeSpan()
        )
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(url) };
                if (Math.Abs(timeout.TotalSeconds) > 0)
                {
                    client.Timeout = timeout;
                }
                if (bypassCertificate)
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (headers != null)
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                var param = new StringBuilder();
                if (parameters != null)
                {
                    param.Append("?");
                    param.Append(string.Join("&", parameters.Select(par => $"{par.Key}={par.Value}")));
                }
                HttpContent serviceContent;
                switch (format)
                {
                    case InputFormat.Json:
                        serviceContent = new StringContent(content, Encoding.UTF8, "application/json");
                        break;
                    case InputFormat.FromUrlEncoded:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.FromData:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    case InputFormat.Xml:
                        serviceContent = new FormUrlEncodedContent(content);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }

                Log.Information
                (LogTemplates.Request,
                client.BaseAddress.Scheme + 
                client.BaseAddress.Host + 
                param.ToString() + 
                client.BaseAddress.PathAndQuery,
                content,
                JsonConvert.SerializeObject(headers)
                );

                var response = await client.PutAsync(param.ToString(), serviceContent);
                var result = await response.Content.ReadAsStringAsync();


                Log.Information
                 (LogTemplates.Response,
                 client.BaseAddress.Scheme +
                 client.BaseAddress.Host +
                 client.BaseAddress.PathAndQuery,
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
    }
}
