using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using University.BL.DTOs;

namespace University.BL.Services.Implements
{
    public class ApiService
    {
        //  Nugget: Xam.Plugin.Connectivity (Android,iOS, BL)
        /// <summary>
        /// CheckConnection
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com/");

            if (!isReachable)
                return false;

            return true;
        }

        public enum Method
        {
            Get,
            Post,
            Put,
            Delete
        }

        /// <summary>
        /// RequestAPI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlBase"></param>
        /// <param name="prefix"></param>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public async Task<ResponseDTO> RequestAPI<T>(string urlBase,
            string prefix,
            object data,
            Method method,
            bool auth=false)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                HttpContent content = null;
                if (data != null)
                    content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (method == Method.Get)
                    response = await client.GetAsync(prefix);
                else if (method == Method.Post)
                    response = await client.PostAsync(prefix, content);
                else if (method == Method.Put)
                    response = await client.PutAsync(prefix, content);
                else
                    response = await client.DeleteAsync(prefix);

                string responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (auth)
                    {
                        var responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(responseText);
                        if (responseDTO.Data != null)
                            responseDTO.Data = JsonConvert.DeserializeObject<T>(responseDTO.Data.ToString());
                        return responseDTO;

                    }
                    else
                        return new ResponseDTO { Code = (int)response.StatusCode, Data = JsonConvert.DeserializeObject<T>(responseText) };
                }
                else
                    return new ResponseDTO { Code = (int)response.StatusCode, Message = responseText };
            }
            catch (Exception ex) { return new ResponseDTO { Code = 500, Message = ex.Message }; }
        }
    }
}
