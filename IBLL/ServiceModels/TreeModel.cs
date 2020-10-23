using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBLL.ServiceModels
{
    public class TreeModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public Attributes attributes { get; set; }
        public List<Children> children { get; set; }
    }
    public class Children
    {
        public string id { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        public Attributes attributes { get; set; }
        public List<Children> children { get; set; }
    }

    public class Attributes
    {
        public int Level { get; set; }
    }

    public class TreeArgs
    {
        public string id { get; set; }
        public string text { get; set; }
        public int Level { get; set; }
    }
}