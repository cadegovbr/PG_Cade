using System;
using System.Collections.Generic;

namespace PGD.Domain.Entities.Usuario
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Matricula { get; set; }
        public string Cpf { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public string Email{ get; set; }
        public int Unidade { get; set; }
        public string NomeUnidade { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Administrador { get; set; }
        public bool Inativo { get; set; }
        public virtual ICollection<UsuarioPerfilUnidade> UsuariosPerfisUnidades { get; set; }
    }
}
