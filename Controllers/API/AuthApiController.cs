using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;    // Para las llaves de seguridad
using System.IdentityModel.Tokens.Jwt;   // Para crear el token
using System.Security.Claims;            // Para los datos dentro del token (Claims)
using System.Text;             
using Microsoft.AspNetCore.Authentication.JwtBearer;     
using Thoth.Web.Repositories;            // Tus repositorios
using Thoth.Web.Models.ViewModels;       // Tu LoginViewModel
using Microsoft.Extensions.Configuration; // Para leer appsettings
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Thoth.Web.Controllers.Api
{
    [Route("api/[controller]")] // La URL será: /api/AuthApi
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthApiController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IConfiguration _config; // Necesario para leer "Jwt:Key" de appsettings

        public AuthApiController(IUsuarioRepository usuarioRepo, IConfiguration config)
        {
            _usuarioRepo = usuarioRepo;
            _config = config;
        }

        [HttpPost("Login")] // POST: api/AuthApi/Login
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            // 1. Validar credenciales
            var usuario = await _usuarioRepo.ValidateUserAsync(model.Email, model.Password);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Email o contraseña incorrectos" });
            }

            // 2. Verificar si está activo
            if (!usuario.EstaActivo)
            {
                return Unauthorized(new { message = "Tu cuenta está desactivada. Contacta a RRHH." });
            }

            // 3. Generar el Token
            var tokenString = GenerarTokenJWT(usuario);

            // 4. Devolver respuesta JSON
            return Ok(new 
            { 
                token = tokenString,
            });
        }

        private string GenerarTokenJWT(Thoth.Web.Models.Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]); // Lee la clave secreta

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // Guardamos ID, Email y Rol DENTRO del token
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
                }),
                Expires = DateTime.UtcNow.AddDays(30), // Duración del token
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}