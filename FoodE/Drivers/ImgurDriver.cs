using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FoodE.Drivers
{
    public class ImgurDriver
    {
        private const string urlEndpoint = "https://api.imgur.com/3/image";
        public async Task<string> GenerateUrl(string content)
        {
            try
            {
                var client = new ImgurClient("0c2d8f7da822786", "dd86c8de3ca74532ce66ea22aa282f137d766eab");
                var endpoint = new ImageEndpoint(client);
                //IImage image = await endpoint.UploadImageBinaryAsync(Convert.FromBase64String(content));
                var image = await endpoint.UploadImageStreamAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
                return image == null ? null : image.Link;
            }
            //catch (ImgurException imgurEx)
            //{
            //    Debug.Write("An error occurred uploading an image to Imgur.");
            //    Debug.Write(imgurEx.Message);
            //    throw;
            //}
            catch (Exception ex)
            {
                return "content: " + content;
            }
        }
    }
}