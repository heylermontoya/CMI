using MediatR;
using CMI.Application.DTOs;
using CMI.Domain.Enums;
using CMI.Domain.Helpers;
using CMI.Domain.Ports;
using CMI.Domain.QueryFilters;

namespace CMI.Application.Feature.product.Queries
{
    public class GetListProductQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<GetListProductQuery, List<ProductDto>>
    {
        public async Task<List<ProductDto>> Handle(
            GetListProductQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<ProductDto> properties =
                await queryWrapper
                    .QueryAsync<ProductDto>(
                        ItemsMessageConstants.GetProducts
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );

            return properties.ToList();
        }
    }
}
