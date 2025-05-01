using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class AccountService(
    UserManager<User> userManager, 
    SignInManager<User> signInManager,
    IMapper mapper,
    IUserPersist userPersist) : IAccountService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IMapper _mapper = mapper;
    private readonly IUserPersist _userPersist = userPersist;

    public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
    {
        try
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(user => user.UserName.ToLower() == userUpdateDto.UserName.ToLower());
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }
        catch (Exception ex) 
        {
            throw new Exception($"Erro ao verificar senha. Erro: {ex.Message}");
        }
    }

    public async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
    {
        try
        {
            var user = _mapper.Map<User>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
                return _mapper.Map<UserUpdateDto>(user);

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao criar conta. Erro: {ex.Message}");
        }
    }

    public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
    {
        try
        {
            var user = await _userPersist.GetUserByUserNameAsync(userName);
            if (user == null) return null;
            return _mapper.Map<UserUpdateDto>(user);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao recuperar usuário. Erro: {ex.Message}");
        }
    }

    public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
    {
        try
        {
            var user = await _userPersist.GetUserByUserNameAsync(userUpdateDto.UserName);
            if (user == null) return null;

            _mapper.Map(userUpdateDto, user);

            if (userUpdateDto.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
            }

            _userPersist.Update(user);
            if (await _userPersist.SaveChangesAsync())
            {
                var userToReturn = await _userPersist.GetUserByUserNameAsync(user.UserName);
                return _mapper.Map<UserUpdateDto>(userToReturn);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao atualizar conta. Erro: {ex.Message}");
        }
    }

    public async Task<bool> UserExists(string userName)
    {
        try
        {
            return await _userManager.Users.AnyAsync(user => user.UserName.ToLower() == userName.ToLower());
        }
        catch (Exception ex) 
        {
            throw new Exception($"Erro ao verificar se usuário existe. Erro: {ex.Message}");
        }
    }
}