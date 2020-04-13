﻿namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Application.Users.Commands.CreateUser;
    using Application.Users.Commands.LoginUser;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SwaggerExamples;
    using Swashbuckle.AspNetCore.Annotations;

    public class IdentityController : BaseController
    {
        //TODO: Implement refresh token

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(nameof(Register))]
        [SwaggerResponse(StatusCodes.Status200OK, "The user was created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The user data is invalid", typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Register(CreateUserCommand model)
        {
            await this.Mediator.Send(model);
            return this.Ok();
        }

        /// <summary>
        /// Verifies user credentials and generates JWT token
        /// </summary>
        [HttpPost]
        [Route(nameof(Login))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Jwt token successfully generated",
            typeof(LoginUserResponseModel))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "The user credentials are invalid",
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Login(LoginUserCommand model)
        {
            var result = await this.Mediator.Send(model);
            return this.Ok(result);
        }
    }
}
