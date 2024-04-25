using Adm.Domain;
using Adm.Infrastructure;
using Adm.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Reflection;

namespace Adm.Adapters
{
    [ApiController]
    [Route("Adm")]
    public class AdministradorController : Controller
    {
        private readonly AdministradorContext _context;
        private readonly IService _service;

        public AdministradorController(AdministradorContext context, IService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Administrador
        [HttpGet]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Index([FromQuery] Administrador administrador)
        {
            try
            {
                IAdministradorDTO dto = administrador;
                IResultadoOperacao<List<Administrador>> result = await _service.GetAdministradoresAsync(dto);
                result.Link.Add(new Link { Rel = "self", Href = "/admin", Method = "GET", Query = administrador });
                if (!result.Sucesso)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao recuperar os administradores");
            }
        }

        // POST: Create Administrador
        [HttpPost]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Administrador administrador)
        {
            try
            {
                IAdministradorDTO dto = administrador;
                IResultadoOperacao<IAdministradorDTO> result = await _service.Create(dto);
                result.Link.Add(new Link { Rel = "create_admin", Href = "/admin", Method = "POST" });
                if (!result.Sucesso)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return BadRequest("MERDA");
            }
        }

        // PUT: Edit Administrador
        [HttpPut("{Id}")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit(int Id, [FromBody] Administrador administrador)
        {
            try
            {
                administrador.Id = Id;
                IAdministradorDTO dto = administrador;
                IResultadoOperacao<IAdministradorDTO> result = await _service.Edit(dto);
                result.Link.Add(new Link { Rel = "update_admin", Href = $"/admin/{Id}", Method = "PUT" });
                if (!result.Sucesso)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao recuperar os administradores");
            }
        }

        // POST: Delete Administrador
        [HttpDelete("{Id}")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteConfirmed(int Id, [FromBody] Administrador administrador)
        {
            if (administrador.Email == null)
            {
                return BadRequest("Email não pode ser nulo!");
            }

            administrador.Id = Id;

            try
            {
                IAdministradorDTO dto = administrador;
                IResultadoOperacao<IAdministradorDTO> result = await _service.Delete(dto);
                result.Link.Add(new Link { Rel = "update_admin", Href = $"/admin/{Id}", Method = "Delete" });
                if (!result.Sucesso)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao recuperar os administradores");
            }
        }

        [HttpPost("Login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] AdministradorLogin administrador)
        {
            try
            {
                IAdministradorDTOLogin dto = administrador;
                IResultadoOperacao<string> result = await _service.Login(dto);
                result.Link.Add(new Link { Rel = "login_admin", Href = "/admin/login", Method = "POST" });
                if (!result.Sucesso)
                {
                    return BadRequest(result);
                }
                if (result.Data != null)
                {
                    HttpContext.Session.SetString("AuthToken", result.Data);
                }

                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Falha ao Logar");
            }
        }
    }
}
