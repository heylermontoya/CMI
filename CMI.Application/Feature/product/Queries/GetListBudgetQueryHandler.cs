using MediatR;
using CMI.Application.DTOs;
using CMI.Domain.Enums;
using CMI.Domain.Helpers;
using CMI.Domain.Ports;
using CMI.Domain.QueryFilters;

namespace CMI.Application.Feature.product.Queries
{
    public class GetListProductoQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<GetListProductoQuery, List<ProductoDto>>
    {
        public async Task<List<ProductoDto>> Handle(
            GetListProductoQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<ProductoDto> properties =
                await queryWrapper
                    .QueryAsync<ProductoDto>(
                        ItemsMessageConstants.GetProductos
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );

            return properties.ToList();
        }
    }
}
