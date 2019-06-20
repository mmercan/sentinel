using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sentinel.Model.Product;
using Sentinel.Repos.Repositories;
using Sentinel.Model.Product.Dto;
using Sentinel.Model;
using EasyNetQ;
using EasyNetQ.Topology;
// using Mercan.Common.Mongo;
//using EasyNetQMessages;
// using EasyNetQMessages.Polymorphic;


namespace Sentinel.Api.Product.Controllers
{
    // [Authorize(AuthenticationSchemes = "azure")]
    [ApiVersion("2.0")]
    [Route("api/Product")]
    //[Route("api/v{version:apiVersion}/Product")]
    [ApiController]
    public class ProductV2Controller : ControllerBase
    {
        ILogger<ProductV2Controller> logger;
        private readonly IMapper mapper;
        // private readonly MangoBaseRepo<ProductInfoDtoV2> productInfoDtoV2base;
        private readonly IBus bus;
        private ProductRepo productRepo;

        public ProductV2Controller(ILogger<ProductV2Controller> logger,
        ProductRepo productRepo, IMapper mapper, IBus rabbitMQBus)
        {
            this.logger = logger;
            this.productRepo = productRepo;
            this.mapper = mapper;
            //   this.productInfoDtoV2base = mongobase;
            this.bus = rabbitMQBus;
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

                bus.Publish(repos.FirstOrDefault(), "product.newproduct");
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
                //productRepo.Add(result);
                //productRepo.SaveChanges();

                // using (var bus = RabbitHutch.CreateBus("host=sentinel-service-rabbitmq;username=rabbitmq;password=rabbitmq"))
                // {
                bus.Publish(model, "product.newproduct");
                // bus.Publish(new ProductInfoDtoV2 { }, "product.newproduct");

                // var response = bus.Request<ProductInfoDtoV2, MessageResponse>(model);
                // logger.LogCritical("ProductInfoDtoV2 response " + response.Message);
                //   var exchange =  bus.Advanced.ExchangeDeclare("Sentinel.Model.Product.ProductInfo, Sentinel.Model", ExchangeType.Topic, passive: false, durable: true, autoDelete: false);
                //   var topic =  bus.Advanced.QueueDeclare("Sentinel.Model.Product.ProductInfo, Sentinel.Model_product",passive:false,durable:true)
                //     bus.Advanced.Bind()
                // bus.Subscribe<ProductInfo>("product", Handler, x =>{ x.WithDurable(true); x.WithTopic("product.*");});
                // }


                return Created("", null);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute POST " + ex.Message);
                return BadRequest("Failed to execute POST " + ex.Message);
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
