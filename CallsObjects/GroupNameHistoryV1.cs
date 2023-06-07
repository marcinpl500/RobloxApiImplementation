using System;
using System.Collections.Generic;
using System.Text;

namespace RobloxApiImplementation.CallsObjects.GroupNameHistoryV1
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public string name { get; set; }
        public DateTime created { get; set; }
    }

    public class Root
    {
        public string previousPageCursor { get; set; }
        public string nextPageCursor { get; set; }
        public List<Datum> data { get; set; }
    }


}
