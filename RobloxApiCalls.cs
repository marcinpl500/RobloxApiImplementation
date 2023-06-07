using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft;
using DeepEqual;
using DeepEqual.Syntax;
using DeepEqual.Formatting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using RobloxApiImplementation.CallsObjects;

namespace RobloxApiImplementation
{
    interface IRobloxCalls
    {

    }

    /// <summary>
    /// Class <c>RobloxGroupCalls</c> Handles calls to groups API, <see href="https://groups.roblox.com"/>, docs: <seealso href="https://groups.roblox.com/docs#!/Groups/"/>
    /// </summary>
    
    public class RobloxGroupCalls : IRobloxCalls
    {
        private Cookie CreateRobloxCookie(string Value, string Domain)
        {
            Cookie SecureCookie = new Cookie();
            SecureCookie.Name = ".ROBLOSECURITY";
            SecureCookie.Value = Value;
            SecureCookie.Domain = Domain;
            SecureCookie.Path = "/";
            SecureCookie.Secure = true;
            SecureCookie.HttpOnly = true;
            return SecureCookie;
        }
        async Task<string> XcrsfToken(string RobloxCookie, long GroupId, long UserId) //, DateTime CookieExpiration
        {
            Cookie SecureCookie = CreateRobloxCookie(RobloxCookie, "groups.roblox.com");
            //SecureCookie.Expires = CookieExpiration;
            HttpClientHandler RequestHandler = new HttpClientHandler();
            HttpClient RequestClient = new HttpClient(RequestHandler);
            CookieContainer CookieContainer = new CookieContainer();
            CookieContainer.Add(SecureCookie);
            HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Patch, $"https://groups.roblox.com/v1/groups/{GroupId}/users/{UserId}");
            RequestMessage.Headers.Add("x-csrf-token", "fetch");
            HttpResponseMessage ResponseMessage = await RequestClient.SendAsync(RequestMessage);
            if (ResponseMessage.Headers.Contains("x-csrf-token"))
            {
                return ResponseMessage.Headers.GetValues("x-csrf-token").ToString();
            }
            else
            {
                
                return "";
            }
        }

        public async Task<CallsObjects.GroupInfoV1.Root> GetGroupInformationV1(int GroupId)
        {
            HttpClient RequestClient = new HttpClient();
            string JSON;
            HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://groups.roblox.com/v1/groups/{GroupId}");
            HttpResponseMessage ResponseMessage = await RequestClient.SendAsync(RequestMessage);
            var contentStream = await ResponseMessage.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(contentStream))
            {
                JSON = await reader.ReadToEndAsync();
            }
            CallsObjects.GroupInfoV1.Root Deserialized = JsonConvert.DeserializeObject<CallsObjects.GroupInfoV1.Root>(JSON);
            return Deserialized;
        }

        public async Task<CallsObjects.GetAuditLogV1.Root> GetAuditLogV1(string RobloxCookie, long GroupId, string ActionType= "", long UserId= 0, string SortOrder = "Asc", int Limit = 10, string Cursor = "")
        { 
            string Address = $"https://groups.roblox.com/v1/groups/{GroupId}/audit-log?";
            if (ActionType!= "")
            {
                Address = Address + $"actionType={ActionType}";
            }
            if(UserId != 0)
            {
                Address = Address + $"&userId={UserId}";
            }
            Address = Address + $"&sortOrder={SortOrder}&limit={Limit}";
           // Address = Address + $"&limit={Limit}";
            if(Cursor != "")
            {
                Address = Address + $"&cursor={Cursor}";
            }
           Console.WriteLine(Address); 
            string JSON;
            Cookie SecureCookie = CreateRobloxCookie(RobloxCookie, "groups.roblox.com");
            CookieContainer Container = new CookieContainer();
            Container.Add(SecureCookie);
            HttpClientHandler Handler = new HttpClientHandler();
            Handler.CookieContainer = Container;
            HttpClient RequestClient = new HttpClient(Handler);
            HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Get, Address);
            HttpResponseMessage ResponseMessage = await RequestClient.SendAsync(RequestMessage);
            var contentStream = await ResponseMessage.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(contentStream))
            {
                JSON = await reader.ReadToEndAsync();
            }
            var Deserialized = JsonConvert.DeserializeObject<CallsObjects.GetAuditLogV1.Root>(JSON);
            return Deserialized;
        }

        public async Task<CallsObjects.GroupNameHistoryV1.Root> GetGroupNameHistoryV1(long GroupId, string SortOrder = "Asc", int Limit = 10, string Cursor = "")
        {
            //https://groups.roblox.com/v1/groups/sad/name-history?sortOrder=Asc&limit=10&cursor=asd
            string Address = $"https://groups.roblox.com/v1/groups/{GroupId}/name-history?";
            Address = Address + $"sortOrder={SortOrder}&limit={Limit}";
            if(Cursor != "")
            {
                Address = Address + $"&cursor={Cursor}";
            }
            string JSON;;
            HttpClient RequestClient = new HttpClient();
            HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Get, Address);
            HttpResponseMessage ResponseMessage = await RequestClient.SendAsync(RequestMessage);
            var contentStream = await ResponseMessage.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(contentStream))
            {
                JSON = await reader.ReadToEndAsync();
            }
            return JsonConvert.DeserializeObject<CallsObjects.GroupNameHistoryV1.Root>(JSON);
        }

       /// <summary>
       /// Calls Roblox's group API to change user's role based on its ID. Needs .ROBLOSECURITY cookie value for authorization.
       /// </summary>
       /// <param name="GroupID"></param>
       /// <param name="UserID"></param>
       /// <param name="RankID"></param>
       /// <param name="RobloxCookie"></param>
       /// <returns></returns>
      public  async Task<bool> ChangeRankInGroup(int GroupID, uint UserID, int RankID, string RobloxCookie) //DateTime CookieExpiration
        {
            Dictionary<HttpStatusCode, string> returnDic = new Dictionary<HttpStatusCode, string>();
            Uri changerankaddress = new Uri($"https://groups.roblox.com/v1/groups/{GroupID}/users/{UserID}");
            Dictionary<string, int> DataToChange = new Dictionary<string, int>();
            DataToChange.Add("roleId", RankID);
            StringContent RequestBody = new StringContent(JsonConvert.SerializeObject(DataToChange), Encoding.UTF8, "application/json");
            Cookie SecurityCookie = new Cookie();
            SecurityCookie.Name = ".ROBLOSECURITY";
            SecurityCookie.Value = RobloxCookie;
            Cookie TokenCookie = new Cookie();
            TokenCookie.Name = "x-csrf-token";
            string Token = await XcrsfToken(RobloxCookie, GroupID, UserID);
            if (Token == "")
            {
                while (Token != "")
                {
                     Token = await XcrsfToken(RobloxCookie, GroupID, UserID);
                   
                }
            }
            TokenCookie.Value = Token;
            CookieCollection Cookies = new CookieCollection();
            Cookies.Add(TokenCookie);
            Cookies.Add(SecurityCookie);
            CookieContainer container = new CookieContainer();
            container.Add(changerankaddress, Cookies);
            var handler = new HttpClientHandler();
            handler.CookieContainer = container;
            var changeRankRequest = new HttpRequestMessage(HttpMethod.Patch, changerankaddress)
            {
                Content = RequestBody,


            };

            HttpClient h = new HttpClient(handler);
            var result = await h.SendAsync(changeRankRequest);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("Successfull");
                return false;
            }
            else
            {
                Console.WriteLine($"Unsuccessfull, Code:{result.StatusCode.ToString()}");
                return true;
            }
        }


    }

    /// <summary>
    /// Class <c>RobloxUserCalls</c> Handles User Calls, <see href="https://accountsettings.roblox.com"/>, docs: <seealso href="https://accountsettings.roblox.com/docs"/>
    /// </summary>
    class RobloxUserCalls : IRobloxCalls
    {
        async Task<string> XcrsfToken(string RobloxCookie) //, DateTime CookieExpiration
        {
            Cookie SecureCookie = new Cookie(".ROBLOSECURITY", RobloxCookie);
            SecureCookie.Domain = "accountsettings.roblox.com";
            SecureCookie.Path = "/";
            SecureCookie.Secure = true;
            SecureCookie.HttpOnly = true;
            //SecureCookie.Expires = CookieExpiration;
            HttpClientHandler RequestHandler = new HttpClientHandler();
            HttpClient RequestClient = new HttpClient(RequestHandler);
            CookieContainer CookieContainer = new CookieContainer();
            CookieContainer.Add(SecureCookie);
            HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Patch, $"https://accountsettings.roblox.com/v1/users/s/unblock");
            RequestMessage.Headers.Add("x-csrf-token", "fetch");
            HttpResponseMessage ResponseMessage = await RequestClient.SendAsync(RequestMessage);
            if (ResponseMessage.Headers.Contains("x-csrf-token"))
            {
                return ResponseMessage.Headers.GetValues("x-csrf-token").ToString();
            }
            else
            {
                return "";
            }
        }
    }

    /// <summary>
    /// Class <c>RobloxGameCalls</c> Handles Calls to game API, <see href="https://games.roblox.com"/>, docs: <seealso href="https://games.roblox.com/docs/index.html"/> 
    /// </summary>
    class RobloxGameCalls : IRobloxCalls
    {
        async Task<string> XcrsfToken(string RobloxCookie) //, DateTime CookieExpiration
        {
            Cookie SecureCookie = new Cookie(".ROBLOSECURITY", RobloxCookie);
            SecureCookie.Domain = "accountsettings.roblox.com";
            SecureCookie.Path = "/";
            SecureCookie.Secure = true;
            SecureCookie.HttpOnly = true;
            //SecureCookie.Expires = CookieExpiration;
            HttpClientHandler RequestHandler = new HttpClientHandler();
            HttpClient RequestClient = new HttpClient(RequestHandler);
            CookieContainer CookieContainer = new CookieContainer();
            CookieContainer.Add(SecureCookie);
            HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Patch, $"https://accountsettings.roblox.com/v1/users/s/unblock");
            RequestMessage.Headers.Add("x-csrf-token", "fetch");
            HttpResponseMessage ResponseMessage = await RequestClient.SendAsync(RequestMessage);
            if (ResponseMessage.Headers.Contains("x-csrf-token"))
            {
                return ResponseMessage.Headers.GetValues("x-csrf-token").ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
