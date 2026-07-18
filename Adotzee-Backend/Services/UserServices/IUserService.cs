using Adotzee_Backend.DTOs.UserDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.UserServices
{
    public interface IUserService
    {
        /* ---------- USERS ---------- */
        Task<ApiResponse<List<UserDTO>>> GetUsers();
        Task<ApiResponse<UserDTO?>> GetUserById(int id);

        /* ---------- AUTH ---------- */
        Task<ApiResponse<LoginResponseDTO>> Login(LoginDTO loginDTO);
        Task<ApiResponse<bool>> Register(RegisterDTO registerDTO);

        /* ---------- USER MANAGEMENT ---------- */
        Task<ApiResponse<bool>> DeleteUser(int id);
        Task<ApiResponse<bool>> ToggleBlockUser(int id);
    }
}