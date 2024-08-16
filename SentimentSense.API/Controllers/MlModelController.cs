using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SentimentSense.API.Repositories.Interfaces;
using SentimentSense.Models;

namespace SentimentSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MlModelController : ControllerBase
    {
        public readonly IMongoRepository<MlModel> _MongoRepository;

        public MlModelController(IMongoRepository<MlModel> mongoRepository)
        {
            _MongoRepository = mongoRepository;
        }

        [HttpGet("")] public async Task<IActionResult> Get()
        {
            var models = await _MongoRepository.FindAll();

            if (!models.Any())
            {
                return NoContent();
            }
            
            return Ok(models);
        }
        
        [HttpGet("{id}")] public async Task<IActionResult> Get(string id)
        {
            var model = await _MongoRepository.FindById(id);

            if (model == null)
            {
                return NotFound();
            }
            
            return Ok(model);
        }
        
        [HttpPost("")]
        public async Task<IActionResult> Create(MlModel model)
        {
            model = await _MongoRepository.InsertOne(model);
            return Created(model.Id, model);
        }

        // Fix
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, MlModel model)
        {
            model = await _MongoRepository.ReplaceOne(id, model);

           

            return NoContent();
        }
        
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _MongoRepository.DeleteById(id);
            return NoContent();
        }
    }
}
