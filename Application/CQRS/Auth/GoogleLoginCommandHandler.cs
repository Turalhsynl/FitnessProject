using Application.Abstractions;
using Application.CQRS.Auth.ResponseDto;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Repository.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.CQRS.Auth;

public class GoogleLoginCommandHandler
{
    public class GoogleLoginCommand : IRequest<AuthResultDto>
    {
        public string Code { get; set; } = string.Empty;
    }

    public class GoogleLoginHandler : IRequestHandler<GoogleLoginCommand, AuthResultDto>
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public GoogleLoginHandler(IGoogleAuthService googleAuthService, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _googleAuthService = googleAuthService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResultDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var googleUser = await _googleAuthService.GetUserInfoAsync(request.Code);

            if (string.IsNullOrEmpty(googleUser.Email))
                throw new Exception("Google istifadəçi email məlumatı alınmadı.");

            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(googleUser.Email);

            if (user == null)
            {
                user = new User
                {
                    Firstname = googleUser.Given_name,
                    Lastname = googleUser.Family_name,
                    Email = googleUser.Email,
                    UserRole = UserRoles.User,
                    CreatedDate = DateTime.UtcNow,
                    Gender = "male",
                    Age = 21,
                    Password = "1234"

                };

                await _unitOfWork.UserRepository.RegisterAsync(user);
                await _unitOfWork.SaveChangeAsync();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var jwtToken = TokenService.CreateToken(claims, _configuration);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var refreshTokenString = TokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshTokenString,
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            await _unitOfWork.RefreshTokenRepository.SaveRefreshToken(refreshTokenEntity);
            await _unitOfWork.SaveChangeAsync();

            return new AuthResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString
            };
        }
    }
}
