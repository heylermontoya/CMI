using MediatR;
using CMI.Application.DTOs;
using CMI.Domain.QueryFilters;

namespace CMI.Application.Feature.product.Queries
{
    public record GetListProductoQuery(
        IEnumerable<FieldFilter>? FieldFilter
    ) : IRequest<List<ProductoDto>>;
}
