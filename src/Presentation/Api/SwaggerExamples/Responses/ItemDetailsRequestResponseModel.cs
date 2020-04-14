﻿namespace Api.SwaggerExamples.Responses
{
    using System;
    using System.Collections.Generic;
    using Application.Common.Models;
    using Application.Items.Queries.Details.Models;
    using Application.Pictures;
    using Swashbuckle.AspNetCore.Filters;

    public class ItemDetailsRequestResponseModel : IExamplesProvider<Response<ItemDetailsResponseModel>>
    {
        public Response<ItemDetailsResponseModel> GetExamples()
            => new Response<ItemDetailsResponseModel>(new ItemDetailsResponseModel
            {
                Id = Guid.Parse("46B33009-243D-4765-872E-08D7DFB08A87"),
                Title = "Test Title_1",
                Description = "Test Description_1",
                StartingPrice = 346.00m,
                MinIncrease = 5.00m,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(10),
                UserUserName = "test@test.com",
                SubCategoryName = "Antiques",
                Pictures = new List<PictureResponseModel>
                {
                    new PictureResponseModel { Id = Guid.NewGuid(), Url = "Some example url here" }
                },
            });
    }
}
