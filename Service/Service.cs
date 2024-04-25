using Adm.Domain;
using Adm.Interface;

namespace Adm.Service
{
    public class Service : IService
    {
        private readonly IRepository _Repository;
        private readonly ICrypto _Crypto;
        private readonly IAuth _Auth;

        public Service(IRepository Repository, ICrypto Crypto, IAuth Auth)
        {
            _Repository = Repository;
            _Crypto = Crypto;
            _Auth = Auth;
        }

        public async Task<IResultadoOperacao<List<Administrador>>> GetAdministradoresAsync(IAdministradorDTO query)
        {
            return await _Repository.GetAdministradoresAsync(query);
        }

        public async Task<IResultadoOperacao<IAdministradorDTO>> Create(IAdministradorDTO administrador)
        {
            if (!string.IsNullOrEmpty(administrador.Senha))
            {
                string hash = _Crypto.Encrypt(administrador.Senha);
                administrador.Senha = hash;
            }
            return await _Repository.Create(administrador);
        }

        public async Task<IResultadoOperacao<IAdministradorDTO>> Edit(IAdministradorDTO administrador)
        {
            if (!string.IsNullOrEmpty(administrador.Senha))
            {
                string hash = _Crypto.Encrypt(administrador.Senha);
                administrador.Senha = hash;
            }
            return await _Repository.Edit(administrador);
        }

        public async Task<IResultadoOperacao<IAdministradorDTO>> Delete(IAdministradorDTO administrador)
        {
            return await _Repository.Delete(administrador);
        }
        public async Task<IResultadoOperacao<string>> Login(IAdministradorDTOLogin administrador)
        {
            IAdministradorDTO? query = new Administrador
            {
                Email = administrador.Email
            };
            IResultadoOperacao<List<Administrador>> search = await GetAdministradoresAsync(query);
            if (search.Data is not null && !string.IsNullOrEmpty(administrador.Senha))
            {
                var adm = search.Data.FirstOrDefault(a => a.Senha != null);
                if (adm is not null && adm.Senha is not null)
                {
                    bool compare = _Crypto.Decrypt(administrador.Senha, adm.Senha);
                    if (compare)
                    {
                        string token = _Auth.CreateToken(adm);
                        return new ResultadoOperacao<string> { Data = token, Sucesso = true };
                    }
                }
            }
            return new ResultadoOperacao<string> { Sucesso = false, Erro = "Email ou Senha Incorreta" };
        }
    }
}