using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataDogIntegration
{
    public class Widget
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int board_id { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int title_size { get; set; }

        public bool title { get; set; }
        public string title_align { get; set; }
        public string text_align { get; set; }
        public string title_text { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int height { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int width { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore)]
        public object[] group_by { get; set; }

        public string timeframe { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int y { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int x { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string text_size { get; set; }

        public string grouping { get; set; }
        public string type { get; set; }
        public string check { get; set; }
        public string group { get; set; }
        public string[] tags { get; set; }

        public static Widget CreatePingdomCheckWidget(string widgettitle, string checkName, string checkId)
        {
            var wid = new Widget
            {
                title_text = widgettitle,
                @group = $"check:{checkName.ToLower().Replace(" ", "_").Replace("(","").Replace(")","")},id:{checkId}",
                tags = new[]
                {
                    "*"
                },

                //x = 15,
                //y = 1,
                type = "check_status",
                check = "pingdom.status",
                grouping = "check",
                timeframe = "10m",
                height = 5,
                width = 10,
                title = true,
                text_size = "auto",
                text_align = "center",
                title_size = 12,
                title_align = "center"
            };
            return wid;
        }

        public static List<Widget> CalculatePositionForWidgets(IEnumerable<Widget> wids)
        {
            int rows = 0;
            int columns = 0;
            int maxColumns = 10;
            int widgetWidthPlusPading = 12;
            int widgetHeightPlusPadding = 8;

            var returnUpdatedWidgets = new List<Widget>();
            foreach (var widget in wids)
            {
                widget.x = columns * widgetWidthPlusPading;
                widget.y = rows * widgetHeightPlusPadding;
                columns++;
                if (columns >= maxColumns)
                {
                    rows++;
                    columns = 0;
                }

                returnUpdatedWidgets.Add(widget);
            }

            return returnUpdatedWidgets;
        }

    }
}