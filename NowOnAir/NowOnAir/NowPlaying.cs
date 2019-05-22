using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NowOnAir
{
    // C# object based on http://radiobox2.omroep.nl/data/radiobox2/nowonair/2.json


    // root object
    public class NowPlaying
    {
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string startdatetime { get; set; }
        public string stopdatetime { get; set; }
        public string date { get; set; }
        public Songfile songfile { get; set; }
        public string channel { get; set; }
    }


    public class Songfile
    {
        public string id { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public string dalet_id { get; set; }
        public string song_id { get; set; }
        public string hidden { get; set; }
        public string last_updated { get; set; }
        public string buma_id { get; set; }
        public string rb1id { get; set; }

        // TODO: songversion
        // TODO: _references
        // TODO: _references_ssl
    }

}
