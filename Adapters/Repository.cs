using Adm.Domain;
using Adm.Interface;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace Adm.Infrastructure
{
    public class Repository : IRepository
    {
        private readonly AdministradorContext _context;

        public Repository(AdministradorContext context)
        {
            _context = context;
        }
        public async Task<IResultadoOperacao<List<Administrador>>> GetAdministradoresAsync(IAdministradorDTO query)
        {

            var queryable = _context.Administradors.AsQueryable();

            if (query.Id != null)
            {
                queryable = queryable.Where(a => a.Id == query.Id);
            }

            if (!string.IsNullOrEmpty(query.Nome))
            {
                queryable = queryable.Where(a => a.Nome == query.Nome);
            }

            if (!string.IsNullOrEmpty(query.Email))
            {
                queryable = queryable.Where(a => a.Email == query.Email);
            }

            if (query.Level != null)
            {
                queryable = queryable.Where(a => a.Level == query.Level);
            }

            try
            {
                List<Administrador> administradores = await queryable.ToListAsync();

                if (administradores.Count == 0)
                {
                    return new ResultadoOperacao<List<Administrador>> { Sucesso = false, Erro = "Nenhum Administrador encontrado" };
                }
                return new ResultadoOperacao<List<Administrador>> { Data = administradores, Sucesso = true };
            }
            catch (DbUpdateException)
            {
                return new ResultadoOperacao<List<Administrador>> { Sucesso = false, Erro = "Erro ao buscar Administradores" };
            }
        }

        public async Task<IResultadoOperacao<IAdministradorDTO>> Create(IAdministradorDTO administrador)
        {
            try
            {
                _context.Add(administrador);
                await _context.SaveChangesAsync();
                IAdministradorDTO? administradorNovo = await _context.Administradors.FirstOrDefaultAsync(a => a.Email == administrador.Email);

                if (administradorNovo == null)
                {
                    return new ResultadoOperacao<IAdministradorDTO> { Sucesso = false, Erro = "Erro ao salvar Administrador" };
                }
                return new ResultadoOperacao<IAdministradorDTO> { Data = administradorNovo, Sucesso = true };
            }
            catch (DbUpdateException)
            {
                if (!AdministradorExists(administrador.Email))
                {
                    return new ResultadoOperacao<IAdministradorDTO> { Sucesso = false, Erro = "Erro ao salvar administrador" };
                }
                return new ResultadoOperacao<IAdministradorDTO> { Sucesso = false, Erro = "Administrador já existe" };
            }
        }

        public async Task<ResultadoOperacao<IAdministradorDTO>> Edit(IAdministradorDTO administrador)
        {
            try
            {
                IAdministradorDTO? adm = _context.Administradors.Find(administrador.Id);
                if (adm != null)
                {
                    if (administrador.Email != null)
                    {
                        adm.Email = administrador.Email;
                    }
                    if (administrador.Nome != null)
                    {
                        adm.Nome = administrador.Nome;
                    }
                    if (administrador.Senha != null)
                    {
                        adm.Senha = administrador.Senha;
                    }
                    if (administrador.Level != null)
                    {
                        adm.Level = administrador.Level;
                    }
                }
                await _context.SaveChangesAsync();
                IAdministradorDTO? administradorNovo = _context.Administradors.Find(administrador.Id);
                if (administradorNovo == null)
                {
                    return new ResultadoOperacao<IAdministradorDTO> { Sucesso = false, Erro = "Administrador não existe" };
                }
                return new ResultadoOperacao<IAdministradorDTO> { Data = adm, Sucesso = true };
            }
            catch (DbUpdateException)
            {
                return new ResultadoOperacao<IAdministradorDTO> { Sucesso = false, Erro = "Erro ao editar administrador" };
            }
        }
        public async Task<ResultadoOperacao<IAdministradorDTO>> Delete(IAdministradorDTO administrador)
        {
            try
            {
                Administrador? buscaAdministrador = await _context.Administradors.FindAsync(administrador.Id);
                if (buscaAdministrador == null)
                {
                    return new ResultadoOperacao<IAdministradorDTO> { Sucesso = false, Erro = "Administrador não existe" };

                }
                _context.Administradors.Remove(buscaAdministrador);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<IAdministradorDTO> { Data = buscaAdministrador, Sucesso = true };
            }
            catch (DbUpdateException)
            {
                if (!AdministradorExists(administrador.Email))
                {
                    return new ResultadoOperacao<IAdministradorDTO> { Sucesso = false, Erro = "Administrador não existe" };
                }
                return new ResultadoOperacao<IAdministradorDTO> { Sucesso = false, Erro = "Error ao deletar Administrador" };
            }
        }
        private bool AdministradorExists(string? email)
        {
            return _context.Administradors.Any(e => e.Email == email);
        }
    }
}