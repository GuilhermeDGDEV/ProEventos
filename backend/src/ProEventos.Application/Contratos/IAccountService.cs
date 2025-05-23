using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos;

public interface IAccountService
{
    Task<bool> UserExists(string userName);
    Task<UserUpdateDto> GetUserByUserNameAsync(string userName);
    Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password);
    Task<UserUpdateDto> CreateAccountAsync(UserDto userDto);
    Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto);
}