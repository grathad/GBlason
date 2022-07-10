using Ebnf;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace GBlasonWebAPI.Controllers
{
    [ApiController]
    [Route("api/ebnf")]
    public class EbnfController : ControllerBase
    {
        private static readonly Parser parsedEbnf = new Parser();

        private static Stream? rawEbnf;
        private string rawEbnfLoadingError = string.Empty;

        private const string EBNF_FILE_NAME = "GBlasonWebAPI.Resources.GrammarDefinition.ebnf";

        private readonly ILogger<EbnfController> _logger;

        public EbnfController(ILogger<EbnfController> logger)
        {
            _logger = logger;
            var assembly = Assembly.GetExecutingAssembly();
            try
            {
                rawEbnf = assembly.GetManifestResourceStream(EBNF_FILE_NAME);
            }
            catch (Exception e)
            {
                rawEbnfLoadingError = e.Message;
            }
        }

        [HttpGet("raw")]
        public string Raw()
        {
            if(rawEbnf == null)
            {
                return $"Error the EBNF Grammar can't be loaded {rawEbnfLoadingError}";
            }
            else
            {
                using(StreamReader reader = new StreamReader(rawEbnf))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        //[HttpGet(Name = "ParsedEbnf")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}