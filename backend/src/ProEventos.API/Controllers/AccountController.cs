using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService accountService, ITokenService tokenService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;
    private readonly ITokenService _tokenService = tokenService;

    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var userName = User.GetUserName();
            var user = await _accountService.GetUserByUserNameAsync(userName);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar usuário. Erro: {ex.Message}");
        }
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        try
        {
            if (await _accountService.UserExists(userDto.UserName))
                return BadRequest("Usuário já existe.");

            var user = await _accountService.CreateAccountAsync(userDto);
            if (user != null)
                return Ok(new
                {
                    user.UserName,
                    user.PrimeiroNome,
                    Token = _tokenService.CreateToken(user).Result
                });

            return BadRequest("Usuário não criado, tente novamente mais tarde!");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar registrar usuário. Erro: {ex.Message}");
        }
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        try
        {
            var user = await _accountService.GetUserByUserNameAsync(userLoginDto.UserName);
            if (user == null) return Unauthorized("Usuário não encontrado.");

            var result = await _accountService.CheckUserPasswordAsync(user, userLoginDto.Password);
            if (!result.Succeeded) return Unauthorized("Senha inválida.");

            return Ok(new
            {
                user.UserName,
                user.PrimeiroNome,
                Token = _tokenService.CreateToken(user).Result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar fazer login. Erro: {ex.Message}");
        }
    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
    {
        try
        {
            if (userUpdateDto.UserName != User.GetUserName())
                return Unauthorized("Usuário inválido");

            var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
            if (user == null) return Unauthorized("Usuário inválido.");

            var userReturn = await _accountService.UpdateAccount(userUpdateDto);
            if (userReturn == null)
                return NoContent();

            return Ok(new
            {
                userReturn.UserName,
                userReturn.PrimeiroNome,
                Token = _tokenService.CreateToken(userReturn).Result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar usuário. Erro: {ex.Message}");
        }
    }
}