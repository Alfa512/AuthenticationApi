using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


namespace AuthenticationApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class InfoController : ControllerBase
{
    //JsonSerializer serializer;
    
    private readonly ILogger<InfoController> _logger;
    public InfoController(ILogger<InfoController> logger)
    {
        _logger = logger;
        //serializer = new JsonSerializer();
        //serializer.Converters.Add(new JavaScriptDateTimeConverter());
        //serializer.NullValueHandling = NullValueHandling.Ignore;
    }

    [HttpGet]
    [Route("/ip-address")]
    public async Task<string> GetIpAddress()
    {
        //JsonWriter writer = new JsonTextWriter();
        var remoteIpAddress = this.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
        _logger.LogInformation($"RemoteIpAddress: {remoteIpAddress}");
        try
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                CheckAdditionalContent = false,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                FloatFormatHandling = FloatFormatHandling.String,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.None
            };
            settings.Converters.Add(new IPAddressConverter());
            settings.Converters.Add(new IPEndPointConverter());
            settings.Formatting = Formatting.Indented;
            _logger.LogWarning(JsonConvert.SerializeObject(this.HttpContext.Request.HttpContext.Connection, settings));
            _logger.LogWarning(JsonConvert.SerializeObject(this.HttpContext.Request.HttpContext.Items));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
        
        if (remoteIpAddress != null)
            return await Task.FromResult(remoteIpAddress.ToString());
        return string.Empty;
    }

    class IPAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddress));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return IPAddress.Parse((string)reader.Value);
        }
    }

    class IPEndPointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPEndPoint));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IPEndPoint ep = (IPEndPoint)value;
            JObject jo = new JObject();
            jo.Add("Address", JToken.FromObject(ep.Address, serializer));
            jo.Add("Port", ep.Port);
            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            IPAddress address = jo["Address"].ToObject<IPAddress>(serializer);
            int port = (int)jo["Port"];
            return new IPEndPoint(address, port);
        }
    }
}