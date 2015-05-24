using System;

namespace ALOS.Expand
{
    public class KeyValue
    {
        [MapName("key")]
        public String Key { set; get; }
        [MapName("value")]
        public string Value { set; get; }
        public Object Tag { set; get; }
        public Object Tag2 { set; get; }
        [MapName("id")]
        public Int32 Id { set; get; }
    }
}
