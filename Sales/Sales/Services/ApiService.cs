namespace Sales.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Common.Models;
    using Newtonsoft.Json;

    public class ApiService
    {
        /// <summary>
        /// Generic method to return a list of any object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlBase"></param>
        /// <param name="prefix"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                //consume RESTful service
                //1 Create variable to make the communication 
                var client = new HttpClient();
                //2 Load address
                client.BaseAddress = new Uri(urlBase);
                //3 concatenate prefix and controller
                var url = $"{prefix}{controller}";
                //exec the connection
                var response = await client.GetAsync(url);
                //receive a response
                var answer = await response.Content.ReadAsStringAsync();
                //Here answer may contain the Json or the exception so we need to ask if the action was successful
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }

                //when there's a json string and want to convert it to object (deserialize)
                var list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };               
            }
        }
    }
}
