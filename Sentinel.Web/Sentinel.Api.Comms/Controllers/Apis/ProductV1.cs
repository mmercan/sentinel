using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace Sentinel.Api.Comms.Controllers
{

    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/Product")]
    //[Route("api/v{version:apiVersion}/Product")]
    [ApiController]
    public class ProductV1Controller : ControllerBase
    {

        ILogger<ProductV1Controller> logger;
        private IMapper mapper;

        public ProductV1Controller(ILogger<ProductV1Controller> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // var repos = repo.GetAll().Select(mapper.Map<ProductInfo,ProductInfoDtoV1>);
                return Ok("");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute GET " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] object model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest("Invalid format");
            }

            try
            {
                // var result = mapper.Map<ProductInfo>(model);
                // repo.Add(result);
                // repo.SaveChanges();
                return Created("", "null");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute POST " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] object model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest("Invalid format");
            }

            try
            {
                // var result = mapper.Map<ProductInfo>(model);
                // repo.Update(result);
                // repo.SaveChanges();
                return Ok("");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute PUT " + ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok("");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute DELETE " + ex.Message);
                return BadRequest();
            }
        }
    }
}
