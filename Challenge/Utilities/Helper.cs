using System.Net;

namespace Challenge.Utilities
{
    public static class Helper
    {
        /// <summary>Generate profile image as an asynchronous operation.</summary>
        /// <param name="fullName">The full name.</param>
        /// <returns>A Task&lt;System.String&gt; representing the asynchronous operation.</returns>
        public async static Task<string> GenerateProfileImageAsync(string fullName)
        {
            var apiBaseUrl = "https://ui-avatars.com/api/";
            var fullNameEncoded = WebUtility.UrlEncode(fullName);
            var apiUrl = $"{apiBaseUrl}?name={fullNameEncoded}&format=svg";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var svgData = await response.Content.ReadAsStringAsync();
                    return svgData;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
