using Adm.Domain;

namespace Adm.Interface
{
    public interface IResultadoOperacao<T>
    {
        T? Data { get; set; }
        string? Erro { get; set; }
        bool Sucesso { get; set; }
        List<Link> Link { get; set; }
    }

    public interface ILink
    {
        string Rel { get; set; }
        string Href { get; set; }
        string Method { get; set; }
        IAdministradorDTO? Query { get; set; }
    }
}
