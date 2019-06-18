using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Sentinel.Repos.Repositories;
using Sentinel.Model.Product;
// using Mercan.Common.Filters;
using Sentinel.Model.Product.Dto;

namespace Sentinel.Api.Product.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/Product")]
    //[Route("api/v{version:apiVersion}/Product")]
    [ApiController]
    // [ValidateModel]
    public class ProductV1Controller : ControllerBase
    {
        ILogger<ProductV1Controller> logger;
        private IMapper mapper;
        private ProductRepo repo;

        public ProductV1Controller(ILogger<ProductV1Controller> logger, IMapper mapper, ProductRepo repo)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.repo = repo;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(List<ProductInfoDtoV1>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 400)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 401)]
        [ApiExplorerSettings(GroupName = @"Products V1")]
        public ActionResult<List<ProductInfoDtoV1>> Get()
        {
            try
            {
                var repos = repo.GetAll().Select(mapper.Map<ProductInfo, ProductInfoDtoV1>);
                return Ok(repos);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute GET " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductInfoDtoV1), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 400)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 401)]
        public IActionResult Post([FromBody] ProductInfoDtoV1 model)
        {
            if (!this.ModelState.IsValid)
            {
                var error = new Dictionary<string, string>();
                error.Add("code", "422");
                //  Microsoft.AspNetCore.Http.StatusCodes

                return BadRequest("Invalid format");
            }

            try
            {
                var mappedModel = mapper.Map<ProductInfo>(model);
                var result = repo.Add(mappedModel);
                repo.SaveChanges();
                var mappedResult = mapper.Map<ProductInfoDtoV1>(result);
                return Created("", mappedResult);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute POST " + ex.Message);
                return BadRequest("Save Failed");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] ProductInfoDtoV1 model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest("Invalid format");
            }

            try
            {
                var mappedModel = mapper.Map<ProductInfo>(model);
                repo.Update(mappedModel);
                repo.SaveChanges();
                return Ok();
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
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute DELETE " + ex.Message);
                return BadRequest();
            }
        }
    }
}
