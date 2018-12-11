using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sentinel.Web.Dto.Product;
using Sentinel.Web.Model.Product;
using Sentinel.Web.Repos.Repositories;


namespace Sentinel.Web.Api.Product.Controllers
{
    [Authorize(AuthenticationSchemes = "azure")]
    [ApiVersion("2.0")]
    [Route("api/Product")]
    //[Route("api/v{version:apiVersion}/Product")]
    [ApiController]
    public class ProductV2Controller : ControllerBase
    {
        ILogger<ProductV2Controller> logger;
        private readonly IMapper mapper;
        private ProductRepo productRepo;

        public ProductV2Controller(ILogger<ProductV2Controller> logger, ProductRepo productRepo, IMapper mapper)
        {
            this.logger = logger;
            this.productRepo = productRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductInfoDtoV2>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 400)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 401)]
        public IActionResult Get()
        {
            try
            {
                var repos = productRepo.GetAll().Select(mapper.Map<ProductInfo, ProductInfoDtoV2>);
                //var result = mapper.Map<List<ProductInfoDtoV2>>(repos);
                return Ok(repos);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute GET" + ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<ProductInfoDtoV2>), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 400)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 401)]
        public IActionResult Post([FromBody] ProductInfoDtoV2 model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest("Invalid format");
            }

            try
            {
                var result = mapper.Map<ProductInfo>(model);
                productRepo.Add(result);
                productRepo.SaveChanges();
                return Created("", null);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute POST " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] ProductInfoDtoV2 model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest("Invalid format");
            }

            try
            {

                var result = mapper.Map<ProductInfo>(model);
                productRepo.Update(result);
                productRepo.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                logger.LogError("Failed to execute PUT");
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                logger.LogError("Failed to execute DELETE");
                return BadRequest();
            }
        }
    }
}
