using Microsoft.AspNetCore.Mvc;
using SentimentSense.API.Repositories.Interfaces;
using SentimentSense.Models;

namespace SentimentSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MlModelController : ControllerBase
    {
        public readonly IMongoRepository<MlModel> _MlModelRepository;

        public MlModelController(IMongoRepository<MlModel> mongoRepository)
        {
            _MlModelRepository = mongoRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var models = await _MlModelRepository.FindAll();

            if (!models.Any())
            {
                return NoContent();
            }

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var model = await _MlModelRepository.FindById(id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] MlModel model)
        {
            model = await _MlModelRepository.InsertOne(model);
            return Created(model.Id, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, MlModel model)
        {
            model = await _MlModelRepository.ReplaceOne(id, model);

            if (model.Id != id)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteResult = await _MlModelRepository.DeleteById(id);

            if (deleteResult.DeletedCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}