using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JupiterClient
{
    public class JupiterMessage<T>
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

    }

    public class AutoCreateResult
    {
        [JsonProperty(PropertyName = "guid")]
        public Guid Guid { get; set; }

        [JsonProperty(PropertyName = "authorization")]
        public string Authorization { get; set; }
    }
}
