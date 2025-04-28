using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(IRestaurantsRepository restaurantsRepository, 
    ILogger<GetRestaurantByIdQueryHandler> logger, IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
{
    public async Task<RestaurantDto?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting a restaurant by {request}");

        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);

        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

        return restaurantDto;
    }
}
