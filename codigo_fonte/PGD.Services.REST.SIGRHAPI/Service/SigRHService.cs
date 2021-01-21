using Npgsql;
using PGD.Domain.Entities.RH;
using PGD.Domain.Entities.Usuario;
using System;
using System.Collections.Generic;

namespace PGD.Services.REST.SIGRHAPI.Service
{
	public class SigRHService
	{
		public string connString = System.Configuration.ConfigurationManager.ConnectionStrings["SIGRHConnectionString"]?.ToString();
		public bool EhDirigente(string cpf)
		{
			bool retorno = false;

			using (var conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;

					// Regra atual: sao dirigentes os chefes ou substitutos de unidades. 
					cmd.CommandText = $@"SELECT distinct s.id_servidor 
										  FROM comum.responsavel_unidade ru
										  INNER JOIN rh.servidor s on ru.id_servidor = s.id_servidor
										  INNER JOIN comum.pessoa p on s.id_pessoa = p.id_pessoa
										  WHERE ( ru.data_fim is null or ru.data_fim > '{ DateTime.Now.ToString("yyyy-MM-dd") }' )
											AND ru.nivel_responsabilidade in ('C', 'V', 'G')
											AND p.cpf_cnpj = @cpf";
					var parametro = cmd.Parameters.Add("@cpf", NpgsqlTypes.NpgsqlDbType.Bigint);
					parametro.Value = Int64.Parse(cpf);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							return true;
						}
					}
				}
			}

			return retorno;
		}

        public List<Usuario> ObterDirigentesUnidade(int idUnidade)
        {
            var lista = new List<Usuario>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Regra atual: sao dirigentes os chefes ou substitutos de unidades. 
                    cmd.CommandText = $@"SELECT distinct s.id_servidor, p.Nome, u.Email
										    FROM comum.responsavel_unidade ru
                                            INNER JOIN rh.servidor s on ru.id_servidor = s.id_servidor
                                            INNER JOIN comum.pessoa p on s.id_pessoa = p.id_pessoa
                                            INNER JOIN comum.usuario u on u.id_servidor = s.id_servidor
                                            WHERE(ru.data_fim is null or ru.data_fim > '{ DateTime.Now.ToString("yyyy-MM-dd") }')
                                            AND ru.nivel_responsabilidade in ('C', 'V', 'G')
                                            AND ru.id_unidade = @id_unidadde";

                    var parametro = cmd.Parameters.Add("@id_unidadde", NpgsqlTypes.NpgsqlDbType.Bigint);
                    parametro.Value = idUnidade;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuario();                            
                            usuario.Email = reader["email"].ToString();
                            usuario.Nome = reader["nome"].ToString();
                            lista.Add(usuario);
                        }
                    }
                }
            }

            return lista;
        }


            public bool EhSolicitante(string cpf)
		{
			bool retorno = false;
			using (var conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;

					/* São servidores: sem cargo em comissão 
						e aqueles ocupantes dos seguintes 
						cargos: DAS-11, DAS-12, DAS-13, DAS-21, DAS-22, DAS-23, FG-01, FG-02, FG-03,
						FG-04, FG-05, FG-06, FG-07, FGR-01, FGR-02, FGR-03, FPE-11, FPE-12, FPE-13,
						FPE-21, FPE-22, FPE-23, RGA-01, RGA-02, RGA-03, RGA-04, RGA-05.
						Resolvi retirar por excecao.
					 */
					cmd.CommandText = @"select distinct p.cpf_cnpj
										from rh.servidor s
										inner join comum.pessoa p on p.id_pessoa = s.id_pessoa      
										and s.id_servidor not in ( select s1.id_servidor 
													   from rh.servidor s1
														inner join rh.designacao d1 on d1.id_servidor = s1.id_servidor  and d1.fim is null
														inner join funcional.nivel_designacao nd1 on  nd1.id_nivel_designacao = d1.id_nivel_designacao
														where nd1.sigla in ('DAS-14','DAS-15', 'DAS-16', 'FPE-14', 'NES-07','DAS-24','DAS-25','DAS-26','FPE-24')
													 ) 
										and s.data_desligamento is null
										and p.cpf_cnpj = @cpf";

					var parametro = cmd.Parameters.Add("@cpf", NpgsqlTypes.NpgsqlDbType.Bigint);
					parametro.Value = Int64.Parse(cpf);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							return true;
						}
					}
				}
			}
			return retorno;
		}

		#region CSU008_RN050
		public List<Unidade> ObterTodasUnidades()
		{

			var lista = new List<Unidade>();

			using (var conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;

					// Retrieve all rows
					cmd.CommandText = @"select id_unidade,nome,codigo_unidade from comum.unidade order by nome";

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var und = new Unidade();
							und.Codigo = reader["codigo_unidade"].ToString();
							und.IdUnidade = Convert.ToInt32(reader["id_unidade"]);
							und.Nome = reader["nome"].ToString();
							lista.Add(und);
						}
					}
				}
			}

			return lista;

		}

		/// <summary>
		/// Retorna detalhes de uma unidade
		/// </summary>
		/// <param name="id">identificador da unidade</param>
		/// <returns></returns>
		public Unidade ObterUnidadeById(int id)
		{
			Unidade unidade = new Unidade();
			using (var conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;

					 
					cmd.CommandText = @"select id_unidade, nome, codigo_unidade 
									    from comum.unidade where id_unidade = @id";
					var parametro = cmd.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Integer);
					parametro.Value = id;
					using (var reader = cmd.ExecuteReader())
					{
						
						if (reader.Read())
						{
							unidade.Codigo = reader["codigo_unidade"].ToString();
							unidade.IdUnidade = Convert.ToInt32(reader["id_unidade"]);
							unidade.Nome = reader["nome"].ToString();
						}
					}
				}
			}

			return unidade;
		}

		public List<Unidade> ObterUnidadesSubordinadas(int idUnidadePai)
        {

            var lista = new List<Unidade>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Retrieve all rows
                    cmd.CommandText = $"select id_unidade,nome,codigo_unidade from comum.unidade where hierarquia like '%.{idUnidadePai}.%' order by nome";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var und = new Unidade();
                            und.Codigo = reader["codigo_unidade"].ToString();
                            und.IdUnidade = Convert.ToInt32(reader["id_unidade"]);
                            und.Nome = reader["nome"].ToString();
                            lista.Add(und);
                        }
                    }
                }
            }

            return lista;

        }


        public Usuario ObterUsuarioPorParametro(string cpf)
		{
			using (var conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;

                    // Retrieve all rows
                    cmd.CommandText = @" SELECT DISTINCT serv.id_servidor, serv.siape, pess.cpf_cnpj, pess.nome, us.email, serv.id_unidade_lotacao, u.nome AS nome_unidade
                                           FROM rh.servidor serv
                                           JOIN comum.pessoa pess ON pess.id_pessoa = serv.id_pessoa
                                           JOIN comum.unidade u ON u.id_unidade = serv.id_unidade
                                           JOIN comum.usuario us ON us.id_pessoa = pess.id_pessoa
                                          WHERE serv.id_ativo = 1 and pess.cpf_cnpj = @cpf
                                          ORDER BY pess.nome;";

                    var parametro = cmd.Parameters.Add("@cpf", NpgsqlTypes.NpgsqlDbType.Bigint);
					parametro.Value = Int64.Parse(cpf);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var usr = new Usuario();
							usr.IdUsuario = Convert.ToInt32(reader["id_servidor"]);
							usr.Nome = reader["nome"].ToString();
							usr.Matricula = reader["siape"].ToString();
							usr.Unidade = Convert.ToInt32(reader["id_unidade_lotacao"]);
							usr.NomeUnidade = reader["nome_unidade"].ToString();
							usr.CPF = RetornaCpfCorrigido(reader["cpf_cnpj"].ToString());
							usr.Email = reader["email"].ToString();
							return usr;
						}
					}
				}
			}
			return new Usuario();
		}

		public Usuario ObterUsuarioPorNome(string nome)
		{
 
			using (var conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;

					// Retrieve all rows
					cmd.CommandText = @"select id_servidor, siape, cpf_cnpj, nome, email, id_unidade_lotacao, nome_unidade FROM vw_Lista_Servidores WHERE nome = @nome";

					var parametro = cmd.Parameters.Add("@nome", NpgsqlTypes.NpgsqlDbType.Text);
					parametro.Value = nome;

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var usr = new Usuario();
							usr.IdUsuario = Convert.ToInt32(reader["id_servidor"]);
							usr.Nome = reader["nome"].ToString();
							usr.Matricula = reader["siape"].ToString();
							usr.Unidade = Convert.ToInt32(reader["id_unidade_lotacao"]);
							usr.NomeUnidade = reader["nome_unidade"].ToString();
							usr.CPF = RetornaCpfCorrigido(reader["cpf_cnpj"].ToString());
							usr.Email = reader["email"].ToString();
							return usr;
						}
					}
				}
			}
			return new Usuario();
		}

		public List<Usuario> TodosUsuariosDaUnidade(string id, bool incluirSubordinados = false)
		{
			var lista = new List<Usuario>();

			using (var conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;

                    if (incluirSubordinados)
                    {
                        cmd.CommandText = $"select id_servidor, siape, cpf_cnpj, nome, email, id_unidade_lotacao, nome_unidade FROM vw_Lista_Servidores WHERE id_unidade_lotacao in ( select id_unidade from comum.unidade where hierarquia like '%.{id}.%')";
                    }
                    else
                    {
                        cmd.CommandText = @"select id_servidor, siape, cpf_cnpj, nome, email, id_unidade_lotacao, nome_unidade FROM vw_Lista_Servidores WHERE id_unidade_lotacao = @id";
                    }

                    var parametro = cmd.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint);
					parametro.Value = Int64.Parse(id);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var usr = new Usuario();
							usr.IdUsuario = Convert.ToInt32(reader["id_servidor"]);
							usr.Nome = reader["nome"].ToString();
							usr.Matricula = reader["siape"].ToString();
							usr.Unidade = Convert.ToInt32(reader["id_unidade_lotacao"]);
							usr.NomeUnidade = reader["nome_unidade"].ToString();
							usr.CPF = RetornaCpfCorrigido(reader["cpf_cnpj"].ToString());
							usr.Email = reader["email"].ToString();
							lista.Add(usr);
						}
					}
				}
			}

			return lista;
		}
		public List<Usuario> TodosUsuariosDaBase()
		{
			var lista = new List<Usuario>();

			using (var conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;

					// Retrieve all rows
					cmd.CommandText = @"select id_servidor, siape, cpf_cnpj, nome, email, id_unidade_lotacao, nome_unidade FROM vw_Lista_Servidores order by nome";
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var usr = new Usuario();
							usr.IdUsuario = Convert.ToInt32(reader["id_servidor"]);
							usr.Nome = reader["nome"].ToString();
							usr.Matricula = reader["siape"].ToString();
							usr.Unidade = Convert.ToInt32(reader["id_unidade_lotacao"]);
							usr.NomeUnidade = reader["nome_unidade"].ToString();
							usr.CPF = RetornaCpfCorrigido(reader["cpf_cnpj"].ToString());
							usr.Email = reader["email"].ToString();
							lista.Add(usr);
						}
					}
				}
			}

			return lista;
		}
		public string RetornaCpfCorrigido(string cpf)
		{
			var CpfCorrigido = string.Empty;
			if (!String.IsNullOrEmpty(cpf))
			{
				if (cpf.Length < 11)
					CpfCorrigido = cpf.PadLeft(11, '0');
				else

					CpfCorrigido = cpf;
			}
			return CpfCorrigido;
		}
		#endregion


	}

}
