using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.WebAPI.Controllers
{
    [Route("api/[controller]")]     //api/values
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
            .Select(i => (Id: i, Value: $"Value {i}"))
            .ToDictionary(v => v.Id, v => v.Value);

        public ValuesController()
        {

        }

        //[HttpGet]
        //public IEnumerable<string> Get() => _Values.Values;

        //[HttpGet]
        //public ActionResult<string[]> Get() => _Values.Values.ToArray();

        //[HttpGet]
        //public ActionResult<string[]> Get() => Ok(_Values.Values);

        [HttpGet]
        public IActionResult Get() => Ok(_Values.Values);

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (!_Values.ContainsKey(id))
                return NotFound();

            return Ok(_Values[id]);
        }

        [HttpGet("Count")]
        public IActionResult Count() => Ok(_Values.Count);

        [HttpPost]
        [HttpPost("add")]
        public IActionResult Add([FromBody] string value)
        {
            var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;
            _Values[id] = value;

            return CreatedAtAction(nameof(GetById), new { Id = id });
        }

        [HttpPut("{id}")]
        public IActionResult Replace(int id, [FromBody] string value)
        {
            if (!_Values.ContainsKey(id))
                return NotFound();

            _Values[id] = value;

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_Values.ContainsKey(id))
                return NotFound();

            _Values.Remove(id);

            return Ok();
        }
    }
}
