using Npgsql;
using ConfiguradorDeComponents.Models;
using System;
using System.Collections.Generic;


namespace ConfiguradorDeComponents.DAL
{
    public class AlarmesDAL
    {
        private NpgsqlConnection _con;

        private void Connection(){
            string constr = "Server=srv-testes;Port=5432;User Id=postgres;Password=geodados;Database=ConfigComponents";
            _con = new NpgsqlConnection(constr);
        }

        public List<Alarmes> ObterAlarmes(){
            Connection();
            List<Alarmes> alarmesLista = new List<Alarmes>();

            using(NpgsqlCommand command = new NpgsqlCommand("select * from alarmesView", _con)){
                _con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read()){
                    Alarmes alarme = new Alarmes()
                    {
                        IdAlarme = Convert.ToInt32(reader["id"]),
                        DescricaoAlarme = Convert.ToString(reader["descricao"]),
                        NomeClassificacaoAlarme = Convert.ToString(reader["nome_classificacao_alarme"]),
                        Nome = Convert.ToString(reader["nome_equipamento_relacionado"]),
                        DataDeCadastroAlarme = Convert.ToString(reader["data_de_cadastro"]),
                    };

                    alarmesLista.Add(alarme);
                }
                _con.Close();

                return alarmesLista;
            }   
        }

        public bool AdicionarAlarme(Alarmes alarmeObj){
            Connection();

            int retornoCasoSucesso;

            using(NpgsqlCommand command = new NpgsqlCommand("INSERT INTO alarmes (descricao, classificacaoId, equipamentoRelacionadoId, dataDeCadastro) VALUES" 
                                                             +"(@Descricao, @ClassificacaoId, @EquipamentoRelacionadoId, current_timestamp)", _con)){
                command.Parameters.AddWithValue("@Descricao", alarmeObj.DescricaoAlarme);
                command.Parameters.AddWithValue("@ClassificacaoId", alarmeObj.IdClassificacaoDoAlarme);
                command.Parameters.AddWithValue("@EquipamentoRelacionadoId", alarmeObj.Id);

                _con.Open();

                retornoCasoSucesso = Convert.ToInt32(command.ExecuteScalar());
            }

            _con.Close();

            return retornoCasoSucesso >= 1;
        }

        public bool EditarAlarme(Alarmes alarmeObj){
            Connection();

            int retornoCasoSucesso;

            using(NpgsqlCommand command = new NpgsqlCommand("UPDATE alarmes SET (descricao, classificacaoId, equipamentoRelacionadoId) = "
                                                             +   "(@Descricao, @ClassificacaoId, @EquipamentoRelacionadoId) WHERE id = @Id;", _con)){
                command.Parameters.AddWithValue("@Id", alarmeObj.IdAlarme);
                command.Parameters.AddWithValue("@Descricao", alarmeObj.DescricaoAlarme);
                command.Parameters.AddWithValue("@ClassificacaoId", alarmeObj.IdClassificacaoDoAlarme);
                command.Parameters.AddWithValue("@EquipamentoRelacionadoId", alarmeObj.Id);

                _con.Open();

                retornoCasoSucesso = Convert.ToInt32(command.ExecuteScalar());
            }

            _con.Close();

            return retornoCasoSucesso >= 1;
        }

        public bool DeletarAlarme(int id){
            Connection();

            int retornoCasoSucesso;

            using(NpgsqlCommand command = new NpgsqlCommand("DELETE FROM alarmes WHERE id = @id;", _con)){
                command.Parameters.AddWithValue("@id", id);

                _con.Open();

                retornoCasoSucesso = Convert.ToInt32(command.ExecuteScalar());
            }

            _con.Close();

            return retornoCasoSucesso >= 1;
        }
    }
}