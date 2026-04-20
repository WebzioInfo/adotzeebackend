using Adotzee_Backend.DTOs.UserDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Repository.UserRepositories;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Adotzee_Backend.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _config;

        public UserService(
            IUserRepository userRepo,
            IMapper mapper,
            ILogger<UserService> logger,
            IConfiguration configuration)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
            _config = configuration;
        }

        /* ---------------- USERS ---------------- */

        public async Task<ApiResponse<List<UserDTO>>> GetUsers()
        {
            var users = await _userRepo.GetAllUsers();
            return ApiResponse<List<UserDTO>>
                .SuccessResponse(_mapper.Map<List<UserDTO>>(users));
        }

        public async Task<ApiResponse<UserDTO?>> GetUserById(int id)
        {
            var user = await _userRepo.GetUser(id);

            if (user == null)
                return ApiResponse<UserDTO?>
                    .FailResponse("User not found");

            return ApiResponse<UserDTO?>
                .SuccessResponse(_mapper.Map<UserDTO>(user));
        }

        /* ---------------- LOGIN ---------------- */

        public async Task<ApiResponse<LoginResponseDTO>> Login(LoginDTO dto)
        {
            try
            {
                var user = await _userRepo.GetByEmail(dto.Email);

                if (user == null)
                    return ApiResponse<LoginResponseDTO>
                        .FailResponse("User not found");

                if (user.IsBlocked)
                    return ApiResponse<LoginResponseDTO>
                        .FailResponse("User blocked");

                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                    return ApiResponse<LoginResponseDTO>
                        .FailResponse("Invalid password");

                var token = GenerateToken(user);

                return ApiResponse<LoginResponseDTO>.SuccessResponse(
                    new LoginResponseDTO
                    {
                        Token = token,
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name,
                        Role = user.Role,
                    },
                    "Login successful"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed");
                return ApiResponse<LoginResponseDTO>
                    .FailResponse("Login error");
            }
        }

        /* ---------------- REGISTER ---------------- */

        public async Task<ApiResponse<bool>> Register(RegisterDTO dto)
        {
            try
            {
                var existing = await _userRepo.GetByEmail(dto.Email);

                if (existing != null)
                    return ApiResponse<bool>
                        .FailResponse("User already exists");

                if (dto.Password == null) return ApiResponse<bool>.FailResponse("Password is required");
                dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = _mapper.Map<User>(dto);
                await _userRepo.Add(user);

                return ApiResponse<bool>
                    .SuccessResponse(true, "Registration successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed");
                return ApiResponse<bool>
                    .FailResponse("Registration failed");
            }
        }

        /* ---------------- DELETE ---------------- */

        public async Task<ApiResponse<bool>> DeleteUser(int id)
        {
            var deleted = await _userRepo.Delete(id);

            if (!deleted)
                return ApiResponse<bool>.FailResponse("User not found");

            return ApiResponse<bool>
                .SuccessResponse(true, "User deleted");
        }

        /* ---------------- BLOCK ---------------- */

        public async Task<ApiResponse<bool>> ToggleBlockUser(int id)
        {
            var status = await _userRepo.ToggleBlock(id);

            if (status == null)
                return ApiResponse<bool>
                    .FailResponse("User not found");

            return ApiResponse<bool>
                .SuccessResponse(status.Value,
                    status.Value ? "User blocked" : "User unblocked");
        }

        /* ---------------- TOKEN ---------------- */

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"))
            );

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? "User"),
                new Claim(ClaimTypes.Role, user.Role ?? "Guest")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
