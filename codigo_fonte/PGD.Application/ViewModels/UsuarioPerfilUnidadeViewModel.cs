using PGD.Domain.Enums;
using System;

namespace PGD.Application.ViewModels
{
    public class UsuarioPerfilUnidadeViewModel
    {
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public string CpfUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string MatriculaUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdUnidade { get; set; }
        public string NomeUnidade { get; set; }
        public string SiglaUnidade { get; set; }
        public string NomePerfil { get; set; }
        public Perfil PerfilEnum => (Perfil)IdPerfil;
    }
}
