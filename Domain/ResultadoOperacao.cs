using Adm.Interface;

namespace Adm.Domain
{
    public class ResultadoOperacao<T> : IResultadoOperacao<T>
    {
        public bool Sucesso { get; set; }
        public string? Erro { get; set; }
        public T? Data { get; set; }
        public List<Link> Link { get; set; }

        public ResultadoOperacao()
        {
            Link = [];
        }
    }

    public class Link : ILink
    {
        public required string Rel { get; set; }
        public required string Href { get; set; }
        public required string Method { get; set; }

        public IAdministradorDTO? Query { get; set; }

    }
}
