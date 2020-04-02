using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AliyunSMS
{
    public static class AliyunSms
    {
        public static string SendSms(string phonenumber,string templateCode, object parameters,string signName="滇峰")
        {
            string accessKeyId = "LTAI4FuyZFfh5R1z5NTuynpL";//你的accessKeyId，参考本文档步骤2
            string accessKeySecret = "KRFmpYlyAWdP3XiEdm5bhag3wFkelv";//你的accessKeySecret，参考本文档步骤2

            Dictionary<string, string> smsDict = new Dictionary<string, string>
                {
                    { "PhoneNumbers", phonenumber },
                    { "SignName", signName },
                    { "TemplateCode", templateCode },
                    { "TemplateParam", JsonConvert.SerializeObject(parameters) },
                    { "RegionId", "cn-hangzhou" },
                    { "Action", "SendSms" },
                    { "Version", "2017-05-25" }
                };
            string domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
            SignatureHelper singnature = new SignatureHelper();
            return singnature.Request(accessKeyId, accessKeySecret, domain, smsDict);
        }

        public class SignatureHelper
        {
            private const string ISO8601_DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:ss'Z'";
            private const string ENCODING_UTF8 = "UTF-8";

            public static string PercentEncode(string value)
            {
                StringBuilder stringBuilder = new StringBuilder();
                string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
                byte[] bytes = Encoding.GetEncoding(ENCODING_UTF8).GetBytes(value);
                foreach (char c in bytes)
                {
                    if (text.IndexOf(c) >= 0)
                    {
                        stringBuilder.Append(c);
                    }
                    else
                    {
                        stringBuilder.Append("%").Append(
                            string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c));
                    }
                }
                return stringBuilder.ToString();
            }

            public static string FormatIso8601Date(DateTime date)
            {
                return date.ToUniversalTime().ToString(ISO8601_DATE_FORMAT, CultureInfo.CreateSpecificCulture("en-US"));
            }

            private static IDictionary<string, string> SortDictionary(Dictionary<string, string> dic)
            {
                IDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dic, StringComparer.Ordinal);
                return sortedDictionary;
            }

            public static string SignString(string source, string accessSecret)
            {
                using (HMACSHA1 algorithm = new HMACSHA1())
                {
                    algorithm.Key = Encoding.UTF8.GetBytes(accessSecret.ToCharArray());
                    return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(source.ToCharArray())));
                }
            }

            public async Task<string> HttpGet(string url)
            {
                string responseBody = string.Empty;
                using (HttpClient http = new HttpClient())
                {
                    try
                    {
                        http.DefaultRequestHeaders.Add("x-sdk-client", "Net/2.0.0");
                        HttpResponseMessage response = await http.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        responseBody = await response.Content.ReadAsStringAsync();
                        response.Dispose();
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("\nException !");
                        Console.WriteLine("Message :{0} ", e.Message);
                    }
                }
                return responseBody;
            }

            public string Request(string accessKeyId, string accessKeySecret, string domain, Dictionary<string, string> paramsDict, bool security = false)
            {

                string result = string.Empty;
                Dictionary<string, string> apiParams = new Dictionary<string, string>
                    {
                        { "SignatureMethod", "HMAC-SHA1" },
                        { "SignatureNonce", Guid.NewGuid().ToString() },
                        { "SignatureVersion", "1.0" },
                        { "AccessKeyId", accessKeyId },
                        { "Timestamp", FormatIso8601Date(DateTime.Now) },
                        { "Format", "JSON" }
                    };

                foreach (KeyValuePair<string, string> param in paramsDict)
                {
                    if (!apiParams.ContainsKey(param.Key))
                    {
                        apiParams.Add(param.Key, param.Value);
                    }
                }
                IDictionary<string, string> sortedDictionary = SortDictionary(apiParams);
                string sortedQueryStringTmp = "";
                foreach (KeyValuePair<string, string> param in sortedDictionary)
                {
                    sortedQueryStringTmp += "&" + PercentEncode(param.Key) + "=" + PercentEncode(param.Value);
                }
                string stringToSign = "GET&%2F&" + PercentEncode(sortedQueryStringTmp.Substring(1));
                string sign = SignString(stringToSign, accessKeySecret + "&");
                string signature = PercentEncode(sign);
                string url = (security ? "https" : "http") + $"://{domain}/?Signature={signature}{sortedQueryStringTmp}";
                try
                {
                    result = HttpGet(url).Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return result;

            }
        }
    }

}
