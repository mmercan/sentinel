using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;


namespace Sentinel.Api.Billing3.Helpers
{
    public static class HealthCheckSwaggerOptions
    {
        public static void AddHealthCheckSwaggerOptions(this SwaggerOptions options)
        {


            var HealthReportResult = new OpenApiSchema
            {
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    {"name",new OpenApiSchema{Type="string"}},
                    {"status",new OpenApiSchema{Type="string"}},
                    {"duration",new OpenApiSchema{Type="string"}},
                    {"description",new OpenApiSchema{Type="string"}},
                    {"type",new OpenApiSchema{Type="string"}},
                    {"data",new OpenApiSchema{Type="object"}},
                    {"exception",new OpenApiSchema{Type="string"}}
                },
                Reference = new OpenApiReference
                {
                    Id = "HealthReportResult",
                    Type = ReferenceType.Schema
                }
            };

            var HealthReport = new OpenApiSchema
            {
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    {"status",new OpenApiSchema{Type="string"}},
                    {"duration",new OpenApiSchema{Type="string"}},
                    {"results",new OpenApiSchema{Type="array",Items=HealthReportResult}}
                },
                Reference = new OpenApiReference
                {
                    Id = "HealthReport",
                    Type = ReferenceType.Schema
                }
            };

            var Security = new List<IDictionary<string, IEnumerable<string>>>();
            Security.Add(new Dictionary<string, IEnumerable<string>>
            {
                { "BearerAuth", new List<string>()}
            });

            var IsAliveAndWellPath = new OpenApiPathItem();
            IsAliveAndWellPath.AddOperation(OperationType.Get, new OpenApiOperation
            {
                Description = "Application Health Api",
                // OperationId = "findPets",
                Parameters = new List<OpenApiParameter>
                {
                    new OpenApiParameter
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Description = "access token",
                        Required = false,

                        Schema = new OpenApiSchema{Type = "string"}
                    }
                },
                Responses = new OpenApiResponses
                {
                    ["200"] = new OpenApiResponse
                    {
                        Description = "Healthy",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = HealthReport
                            },
                            // ["application/xml"] = new OpenApiMediaType
                            // {
                            //     Schema = new OpenApiSchema
                            //     {
                            //         Type = "object",
                            //         Items = HealthReport
                            //     }
                            // }
                        }
                    },
                    ["4XX"] = new OpenApiResponse
                    {
                        Description = "unexpected client error",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["text/html"] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    Type = "string",
                                    // Items = HealthReport
                                }
                            }
                        }
                    },
                    ["503"] = new OpenApiResponse
                    {
                        Description = "Unhealthy",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = HealthReport
                            }
                        }
                    }
                }
            });

            var IsAlivePath = new OpenApiPathItem();
            IsAlivePath.AddOperation(OperationType.Get, new OpenApiOperation { });

            options.PreSerializeFilters.Add((doc, req) =>
            {
                doc.Components.Schemas.Add("HealthReport", HealthReport);
                doc.Components.Schemas.Add("HealthReportResult", HealthReportResult);

                doc.Paths.Add("/Health/IsAliveAndWell", IsAliveAndWellPath);
                doc.Paths.Add("/Health/IsAlive", IsAlivePath);
            });





            //     // Responses = new Dictionary<string, OpenApiResponse>{
            //     //             {"200",new OpenApiResponse{Description="Success", Schema = new OpenApiSchema{Items=HealthReport}}},
            //     //             {"503",new OpenApiResponse{Description="Failed"}}
            //     //     },
            //     //  Security = Security,
            //     //     Parameters = new List<IParameter>{
            //     //         new NonBodyParameter
            //     //         {
            //     //             Name = "Authorization",
            //     //             In = "header",
            //     //             Description = "access token",
            //     //             Required = true,
            //     //             Type = "string",
            //     //             Default = "Bearer "
            //     //         }
            //     //    }
            // })

            // });

            // doc.Paths.Add("/Health/IsAlive", new PathItem
            // {
            //     Get = new Operation
            //     {
            //         Tags = new List<string> { "HealthCheck" },
            //         Produces = new string[] { "application/json" },
            //         Responses = new Dictionary<string, Response>{
            //                     {"200",new Response{Description="Success",
            //                     Schema = new Schema{}}},
            //                     {"503",new Response{Description="Failed"}}
            //                 },
            //         Security = Security,
            //     }
            // });



        }
    }
}