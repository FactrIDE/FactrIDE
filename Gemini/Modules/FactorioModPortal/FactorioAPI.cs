using FactrIDE.Properties;
using FactrIDE.Storage.Folders;
using HtmlAgilityPack;
using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xceed.Wpf.Toolkit;

namespace FactrIDE.Gemini.Modules.FactorioModPortal
{
    public class InfoJson
    {
        [JsonProperty("factorio_version")]
        public string FactorioVersion { get; set; }
    }

    public class Release
    {
        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }

        [JsonProperty("file_name")]
        public string Filename { get; set; }

        [JsonProperty("info_json")]
        public InfoJson InfoJson { get; set; }

        [JsonProperty("released_at")]
        public DateTime ReleasedAt { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class ModInfoResponse
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("downloads_count")]
        public int DownloadsCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("releases")]
        public List<Release> Releases { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class PlayerData
    {
        [JsonProperty("service-username")]
        public string ServiceUsername { get; set; }
        [JsonProperty("service-token")]
        public string ServiceToken { get; set; }
    }

    public static class FactorioAPI
    {
        private static RestClient AuthClient { get; } = new RestClient("https://auth.factorio.com");
        private static RestClient ModsClient { get; } = new RestClient("https://mods.factorio.com");

        public static void UpdateToken()
        {
            if (Settings.Default.UseFactorioCredentials)
            {
                var playerDataFile = new FactorioPlayerDataFile();
                if (playerDataFile.Exists)
                {
                    if (playerDataFile.PlayerData != null && !string.IsNullOrEmpty(playerDataFile.PlayerData.ServiceUsername) && !string.IsNullOrEmpty(playerDataFile.PlayerData.ServiceToken))
                    {
                        Settings.Default.Username = playerDataFile.PlayerData.ServiceUsername;
                        Settings.Default.Token = playerDataFile.PlayerData.ServiceToken;
                    }
                    else
                        MessageBox.Show("Error while reading Factorio credentials!\nPlease make sure that you are logged in!", "Error!");
                }
            }
            else
            {
                var username = Settings.Default.Username;
                var password = Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(Settings.Default.Password), null, DataProtectionScope.CurrentUser));

                var request = new RestRequest("api-login");
                request.AddParameter("username", username);
                request.AddParameter("password", password);
                var response = AuthClient.Post(request);
                Settings.Default.Token = JsonConvert.DeserializeObject<string[]>(response.Content)[0];
            }
        }

        public static ModInfoResponse GetModificationInfo(string name)
        {
            var request = new RestRequest("api/mods/{name}");
            request.AddUrlSegment("name", name);
            var response = ModsClient.Get<ModInfoResponse>(request);
            return response.Data;
        }

        public static byte[] GetModification(string download_url)
        {
            var request = new RestRequest("{download_url}");
            request.AddUrlSegment("download_url", download_url);
            request.AddParameter("username", Settings.Default.Username);
            request.AddParameter("token", Settings.Default.Token);
            var response = ModsClient.Get(request);
            return response.RawBytes;
            //return new ZipArchive(new MemoryStream(response.RawBytes));
        }


        // bascally https://github.com/shanemadden/factorio-mod-portal-publish/blob/master/entrypoint.sh
        public static async Task UploadMod(string username, string password, string mod, string tag, byte[] archive)
        {
            var csrf_token = "";
            var upload_token = "";

            var cookieContainer = new CookieContainer();

            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler);
            //client.DefaultRequestHeaders.Add("User-Agent", "PycURL/7.43.0.2 libcurl/7.47.0 OpenSSL/1.0.2g zlib/1.2.8 libidn/1.32 librtmp/2.3");

            // get csrf_token
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://mods.factorio.com/login"))
            {
                request.Headers.TryAddWithoutValidation("Cookie", "cookiejar.txt");

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                csrf_token = doc.GetElementbyId("csrf_token").GetAttributeValue("value", "");
            }

            // login using csrf_token
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://mods.factorio.com/login"))
            {
                request.Headers.TryAddWithoutValidation("Cookie", "cookiejar.txt");
                request.Content = new MultipartFormDataContent
                    {
                        { new StringContent(csrf_token), "csrf_token" },
                        { new StringContent(username), "username" },
                        { new StringContent(password), "password" }
                    };

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // check if logged in
            }

            // get mod releases
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://mods.factorio.com/api/mods/{mod}/full"))
            {
                request.Headers.TryAddWithoutValidation("Cookie", "cookiejar.txt");

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (html.Contains($"\"version\":\"{tag}\""))
                    return;
            }

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://mods.factorio.com/mod/{mod}/downloads/edit"))
            {
                request.Headers.TryAddWithoutValidation("Cookie", "cookiejar.txt");

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // dont even care
                var regex = new Regex("(token:)(.*),");
                var match = regex.Match(html);

                upload_token = match.Groups[2].Value.Replace("\'", "");
            }

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://direct.mods-data.factorio.com/upload/mod/${UPLOAD_TOKEN}"))
            {
                request.Headers.TryAddWithoutValidation("Cookie", "cookiejar.txt");

                var multipartContent = new MultipartFormDataContent();
                var file = new ByteArrayContent(archive);
                file.Headers.Add("Content-Type", "application/x-zip-compressed");
                multipartContent.Add(file, "file", $"{mod}_{tag}.zip");
                request.Content = multipartContent;

                var response = await client.SendAsync(request).ConfigureAwait(false);
            }

            // add last part
        }
    }
}