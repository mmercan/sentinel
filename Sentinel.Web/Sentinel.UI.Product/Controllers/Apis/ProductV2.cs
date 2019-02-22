    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    namespace Sentinel.UI.Product.Controllers
    {
        [ApiVersion("2.0")]
        [Route("api/Product")]
        //[Route("api/v{version:apiVersion}/Product")]
        [ApiController]
        public class ProductV2Controller : ControllerBase
        {
            [HttpGet]
            public string Get() => "Hello world v2!";
        }
    }
