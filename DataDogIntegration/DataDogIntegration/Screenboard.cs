using System;

namespace DataDogIntegration
{
    public class Screenboard
    {
        public string board_title { get; set; }
        public bool read_only { get; set; }
        public bool isIntegration { get; set; }
        public string board_bgtype { get; set; }
        public DateTime created { get; set; }
        public string original_title { get; set; }
        public DateTime modified { get; set; }
        public bool disableEditing { get; set; }
        public int height { get; set; }
        public string width { get; set; }
        public object[] template_variables { get; set; }
        public Widget[] widgets { get; set; }
        public int id { get; set; }
        public bool title_edited { get; set; }
        public bool isShared { get; set; }
    }
}