using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trackin.Application.DTOs;
using Trackin.Domain.Enums;
using Trackin.Domain.Interfaces;

namespace Trackin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _userRepository;

        public UsuarioController(IUsuarioRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR, GERENTE")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Nome,
                Email = u.Email,
                Role = u.Role,
                PatioId = u.PatioId
            });

            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(long id)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserId != id && currentUserRole != UsuarioRole.ADMINISTRADOR)
            {
                return Forbid();
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            var userDto = new UserDTO
            {
                Id = user.Id,
                Username = user.Nome,
                Email = user.Email,
                Role = user.Role,
                PatioId = user.PatioId
            };

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult> Delete(long id)
        {
            var deleted = await _userRepository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        private long GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        private UsuarioRole GetCurrentUserRole()
        {
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            return Enum.TryParse<UsuarioRole>(roleClaim, out var role) ? role : UsuarioRole.COMUM;
        }
    }
}