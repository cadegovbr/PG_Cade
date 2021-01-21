using Npgsql;
using PGD.Domain.Entities.RH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PGD.Services.REST.SIGRHAPI.Service
{
    public class SistemasComumService
    {
        public List<Feriado> ObterFeriados(string dataAPartirdeYYYYMMDD)
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["SistemasComumConnectionString"].ToString();
            List<Feriado> retorno = new List<Feriado>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = @"SELECT data_feriado, to_char(coalesce(duracao,'08:00'), 'HH:mi') as duracaoHH, descricao
                            FROM comum.feriado
                            where data_feriado >= @dtAPartirDe and id_municipio is null 
                            and id_unidade_federativa is null and id_localidade is null
                            order by data_feriado ";
                    var parametro = cmd.Parameters.Add("@dtAPartirDe", NpgsqlTypes.NpgsqlDbType.Date);
                    parametro.Value = DateTime.ParseExact(dataAPartirdeYYYYMMDD, "yyyyMMdd", CultureInfo.InvariantCulture);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno.Add(new Feriado { data_feriado = reader.GetDateTime(0),
                                                      duracao = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                                      descricao = reader.GetString(2) });
                        }
                    }
                }
            }

            return retorno;
        }


        public bool VefificaSeFeriado(string dataYYYYMMDD)
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["SistemasComumConnectionString"].ToString();
            bool retorno = false;

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = @"SELECT data_feriado
                                    FROM comum.feriado
                                    where data_feriado = @dtVerifica and id_municipio is null 
                                          and id_unidade_federativa is null and id_localidade is null";
                    var parametro = cmd.Parameters.Add("@dtVerifica", NpgsqlTypes.NpgsqlDbType.Date);
                    parametro.Value = DateTime.ParseExact(dataYYYYMMDD, "yyyyMMdd", CultureInfo.InvariantCulture);

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
    }
}