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
    public class ProductoController(IMediator mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateProductotAsync(CreateProductoCommand command)
        {
            ProductoDto ProductoDto = await mediator.Send(command);

            return new CreatedResult($"Producto/{ProductoDto.Id}", ProductoDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductoAsync(UpdateProductoCommand command)
        {
            ProductoDto ProductoDto = await mediator.Send(command);

            return new OkObjectResult(ProductoDto);
        }

        [HttpGet("ProductoById/{id}")]
        public async Task<IActionResult> GetProductoById(int id)
        {
            ProductoDto ProductoDto = await mediator.Send(
                 new GetProductoByIdQuery(id)
             );

            return new OkObjectResult(ProductoDto);
        }

        [HttpPost("list")]
        public async Task<IActionResult> ObtainListProductoAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<ProductoDto> listProductoDto = await mediator.Send(
                new GetListProductoQuery(fieldFilter)
            );

            return new OkObjectResult(listProductoDto);
        }

        [HttpDelete("ProductoById/{id}")]
        public async Task<IActionResult> DeleteProductoById(int id)
        {
            await mediator.Send(
                 new DeleteProductoByIdCommand(id)
             );

            return new NoContentResult();
        }
    }
}
