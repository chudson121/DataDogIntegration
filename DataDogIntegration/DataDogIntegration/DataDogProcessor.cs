using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace DataDogIntegration
{
    internal class DataDogProcessor
    {
        private readonly DataDogConfig _config;
        private string AuthToken { get; set; }

        public DataDogProcessor(DataDogConfig datadogConfiguration)
        {
            _config = datadogConfiguration;

        }
        
        public List<Screenboard> GetDashboards()
        {
            string apiPathProjects = $"api/v1/screen";
            var verb = Method.GET;
            
            var response = ConfigureClientWithParameter(verb, apiPathProjects, null);
            var content = response.Content; // raw content as string
            var jsonObj = JsonConvert.DeserializeObject<Rootobject>(content);
            
            return jsonObj.screenboards.ToList();

        }


        public Screenboard GetDashboardById(string boardId)
        {

            string apiPathProjects = $"api/v1/screen/{boardId}";
            var verb = Method.GET;
            
            var response = ConfigureClientWithParameter(verb, apiPathProjects, null);
            var content = response.Content; // raw content as string
            var jsonObj = JsonConvert.DeserializeObject<Screenboard>(content);
            
            return jsonObj;

        }


        public Screenboard AddWidgetDashboard(Screenboard screen, List<Widget> wigetsToAddt)
        {
            string apiPathProjects = $"api/v1/screen/{screen.id}";
            var verb = Method.PUT;

            var wids = screen.widgets.ToList();//keep existing ones
            wids.AddRange(wigetsToAddt);

            //determineIfalready existing

            var widsWithPosition = Widget.CalculatePositionForWidgets(wids.Distinct());

            var screendto = new ScreenboardUpdateDto
            {
                board_title = screen.board_title,
                widgets = widsWithPosition.ToArray()
            };

            var jsonBody = JsonConvert.SerializeObject(screendto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var response = ConfigureClientWithBodyContent(verb, apiPathProjects, JsonConvert.DeserializeObject<ScreenboardUpdateDto>(jsonBody));
            var content = response.Content; // raw content as string
            var jsonObj = JsonConvert.DeserializeObject<Screenboard>(content);

            return jsonObj;

        }

        public void SubmitSingleMetric(DataDogMetricSeries s)
        {
            const string apiPathProjects = "api/v1/series";
            const Method verb = Method.POST;

            var output = JsonConvert.SerializeObject(s);


            ConfigureClientWithBodyContent(verb, apiPathProjects, output);

            //var content = response.Content; // raw content as string
            //return content;




        }

        private IRestResponse ConfigureClientWithBodyContent(Method httpVerb, string resourceMethod, object bodyContent)
        {
            var client = new RestClient(_config.BaseUrl);
            var request = new RestRequest
            {
                Method = httpVerb,
                Resource = $"{resourceMethod}?api_key={_config.api_key}&application_key={_config.app_key}"
            };

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            //request.Parameters.Clear();
            request.JsonSerializer.ContentType = "application/json; charset=utf-8";
            request.RequestFormat = DataFormat.Json; // Or DataFormat.Xml, if you prefer
            
            if (bodyContent != null)
            {
                request.AddParameter("application/json", bodyContent, ParameterType.RequestBody);
            }
       

            
            var response = client.Execute(request);
            return response;

        }
        private IRestResponse ConfigureClientWithParameter(Method httpVerb, string resourceMethod, Dictionary<string, string> payload)
        {
            var client = new RestClient(_config.BaseUrl);
            var request = new RestRequest { Method = httpVerb };
            request.AddParameter("api_key", _config.api_key);
            request.AddParameter("application_key", _config.app_key);
            

            if (payload != null)
            {
                foreach (var kvp in payload)
                {
                    request.AddParameter(kvp.Key, kvp.Value);
                }
            }

            request.Resource = resourceMethod;
            var response = client.Execute(request);
            return response;

        }


    }
}
