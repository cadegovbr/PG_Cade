using Newtonsoft.Json;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PGD.Domain.Services
{
    public class PerfilService : IPerfilService
    {
        //Retorna os perfis de um Usuário através do seu cpf (pressuõe-se que há um relacionamento entre Perfil x Usuario)
        public IEnumerable<Enums.Perfil> ObterPerfis(Usuario objUsuario)
        {
            var lista = new List<Enums.Perfil>();

            if (this.EhDirigente(objUsuario.Cpf))
                lista.Add(Enums.Perfil.Dirigente);

            if (this.EhSolicitante(objUsuario.Cpf))
                lista.Add(Enums.Perfil.Solicitante);

            //Se não for nem dirigente nem solicitante, retorna como consulta
            if (lista.Count == 0)
                lista.Add(Enums.Perfil.Consulta);

            return lista;
        }

        public bool EhDirigente(string cpf)
        {
            bool retorno = false;

           // using (var conn = new NpgsqlConnection(connString))
           // {
           //     conn.Open();
           //     using (var cmd = new NpgsqlCommand())
           //     {
           //         cmd.Connection = conn;

           //         // Regra atual: sao dirigentes os chefes ou substitutos de unidades. 
           //         cmd.CommandText = $@"SELECT distinct s.id_servidor 
										 // FROM comum.responsavel_unidade ru
										 // INNER JOIN rh.servidor s on ru.id_servidor = s.id_servidor
										 // INNER JOIN comum.pessoa p on s.id_pessoa = p.id_pessoa
										 // WHERE ( ru.data_fim is null or ru.data_fim > '{ DateTime.Now.ToString("yyyy-MM-dd") }' )
											//AND ru.nivel_responsabilidade in ('C', 'V', 'G')
											//AND p.cpf_cnpj = @cpf";
           //         var parametro = cmd.Parameters.Add("@cpf", NpgsqlTypes.NpgsqlDbType.Bigint);
           //         parametro.Value = Int64.Parse(cpf);

           //         using (var reader = cmd.ExecuteReader())
           //         {
           //             while (reader.Read())
           //             {
           //                 return true;
           //             }
           //         }
           //     }
           // }

            return retorno;
        }

        public bool EhSolicitante(string cpf)
        {
            bool retorno = false;
      //      using (var conn = new NpgsqlConnection(connString))
      //      {
      //          conn.Open();
      //          using (var cmd = new NpgsqlCommand())
      //          {
      //              cmd.Connection = conn;

      //              /* São servidores: sem cargo em comissão 
						//e aqueles ocupantes dos seguintes 
						//cargos: DAS-11, DAS-12, DAS-13, DAS-21, DAS-22, DAS-23, FG-01, FG-02, FG-03,
						//FG-04, FG-05, FG-06, FG-07, FGR-01, FGR-02, FGR-03, FPE-11, FPE-12, FPE-13,
						//FPE-21, FPE-22, FPE-23, RGA-01, RGA-02, RGA-03, RGA-04, RGA-05.
						//Resolvi retirar por excecao.
					 //*/
      //              cmd.CommandText = @"select distinct p.cpf_cnpj
						//				from rh.servidor s
						//				inner join comum.pessoa p on p.id_pessoa = s.id_pessoa      
						//				and s.id_servidor not in ( select s1.id_servidor 
						//							   from rh.servidor s1
						//								inner join rh.designacao d1 on d1.id_servidor = s1.id_servidor  and d1.fim is null
						//								inner join funcional.nivel_designacao nd1 on  nd1.id_nivel_designacao = d1.id_nivel_designacao
						//								where nd1.sigla in ('DAS-14','DAS-15', 'DAS-16', 'FPE-14', 'NES-07','DAS-24','DAS-25','DAS-26','FPE-24')
						//							 ) 
						//				and s.data_desligamento is null
						//				and p.cpf_cnpj = @cpf";

      //              var parametro = cmd.Parameters.Add("@cpf", NpgsqlTypes.NpgsqlDbType.Bigint);
      //              parametro.Value = Int64.Parse(cpf);

      //              using (var reader = cmd.ExecuteReader())
      //              {
      //                  while (reader.Read())
      //                  {
      //                      return true;
      //                  }
      //              }
      //          }
      //      }
            return retorno;
        }

        IEnumerable<Entities.Perfil> IPerfilService.ObterPerfis(Usuario objUsuario)
        {
            throw new NotImplementedException();
        }

        public Entities.Perfil Adicionar(Entities.Perfil obj)
        {
            throw new NotImplementedException();
        }

        public Entities.Perfil ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Perfil> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public Entities.Perfil Atualizar(Entities.Perfil obj)
        {
            throw new NotImplementedException();
        }

        public Entities.Perfil Remover(Entities.Perfil obj)
        {
            throw new NotImplementedException();
        }
    }
}
