using CMI.Domain.QueryFilters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CMI.Application.DTOs;
using CMI.Application.Feature.product.Commands;
using CMI.Application.Feature.product.Queries;

namespace CMI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(CreateProductCommand command)
        {
            ProductDto ProductDto = await mediator.Send(command);

            return new CreatedResult($"Product/{ProductDto.Id}", ProductDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(UpdateProductCommand command)
        {
            ProductDto ProductDto = await mediator.Send(command);

            return new OkObjectResult(ProductDto);
        }

        [HttpGet("productById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            ProductDto ProductDto = await mediator.Send(
                 new GetProductByIdQuery(id)
             );

            return new OkObjectResult(ProductDto);
        }

        [HttpPost("list")]
        public async Task<IActionResult> ObtainListProductAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<ProductDto> listProductDto = await mediator.Send(
                new GetListProductQuery(fieldFilter)
            );

            return new OkObjectResult(listProductDto);
        }

        [HttpDelete("productById/{id}")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            await mediator.Send(
                 new DeleteProductByIdCommand(id)
             );

            return new NoContentResult();
        }
    }
}
