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
            string constr = "Server=localhost;Port=5432;User Id=postgres;Password=geodados;Database=ConfigComponents";
            _con = new NpgsqlConnection(constr);
        }

        public List<EquipamentosRelacionados> ObterEquipamentosRelacionados(){
            Connection();
            List<EquipamentosRelacionados> equipamentosRelacionadosLista = new List<EquipamentosRelacionados>();

            using(NpgsqlCommand command = new NpgsqlCommand("select id, nome from equipamentos", _con)){
                _con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read()){
                    EquipamentosRelacionados equipamentoRelacionado = new EquipamentosRelacionados()
                    {
                        IdDoEquipamentoRelacionado = Convert.ToInt32(reader["id"]),
                        NomeDoEquipamentoRelacionado = Convert.ToString(reader["nome"]),
                    };

                    equipamentosRelacionadosLista.Add(equipamentoRelacionado);
                }
                _con.Close();

                return equipamentosRelacionadosLista;
            }
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

            using(NpgsqlCommand command = new NpgsqlCommand("INSERT INTO alarmes (descricao, classificacaoId, equipamentoRelacionadoId, dataDeCadastro, status, dataEntrada, dataSaida, vezesAtuadas) VALUES" 
                                                             +"(@Descricao, @ClassificacaoId, @EquipamentoRelacionadoId, (SELECT DATE_TRUNC('second', CURRENT_TIMESTAMP::timestamp)), false, '', '', 0)", _con)){

                command.Parameters.AddWithValue("@Descricao", alarmeObj.DescricaoAlarme);
                command.Parameters.AddWithValue("@ClassificacaoId", alarmeObj.IdClassificacaoDoAlarme);
                command.Parameters.AddWithValue("@EquipamentoRelacionadoId", alarmeObj.IdDoEquipamentoRelacionado);

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
                command.Parameters.AddWithValue("@Id", alarmeObj.Id);
                command.Parameters.AddWithValue("@Descricao", alarmeObj.DescricaoAlarme);
                command.Parameters.AddWithValue("@ClassificacaoId", alarmeObj.IdClassificacaoDoAlarme);
                command.Parameters.AddWithValue("@EquipamentoRelacionadoId", alarmeObj.IdDoEquipamentoRelacionado);

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

        public List<Alarmes> ObterAlarmesParaAtuar(){
            Connection();
            List<Alarmes> alarmesLista = new List<Alarmes>();            

            using(NpgsqlCommand command = new NpgsqlCommand("select * from alarmesParaAtuarView", _con)){
                _con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read()){
                    Alarmes alarme = new Alarmes()
                    {
                        IdAlarme = Convert.ToInt32(reader["id"]),
                        DescricaoAlarme = Convert.ToString(reader["descricao"]),
                        NomeDoEquipamentoRelacionado = Convert.ToString(reader["nome_equipamento_relacionado"]),
                        Descricao = Convert.ToString(reader["descricao_equipamento_relacionado"]),
                        Status = Convert.ToBoolean(reader["status"]),
                        DataEntrada = Convert.ToString(reader["dataEntrada"]),
                        DataSaida = Convert.ToString(reader["dataSaida"])
                    };

                    alarmesLista.Add(alarme);
                }
                _con.Close();

                return alarmesLista;
            }   
        }

        public bool AtuarAlarme(int id, bool statusDoAlarme){
            Connection();

            int retornoCasoSucesso;

            using(NpgsqlCommand command = new NpgsqlCommand("SELECT atuarAlarme(@Status, @Id);", _con)){

                command.Parameters.AddWithValue("@Status", statusDoAlarme);                                     
                command.Parameters.AddWithValue("@Id", id);

                _con.Open();

                retornoCasoSucesso = command.ExecuteNonQuery();
            }

            _con.Close();

            return retornoCasoSucesso >= 1;
        }

        public List<Alarmes> ObterAlarmesAtuados(){
            Connection();
            List<Alarmes> alarmesMaisAtuadosLista = new List<Alarmes>();

            using(NpgsqlCommand command = new NpgsqlCommand("select * from alarmesAtuadosView;", _con)){
                _con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read()){
                    Alarmes alarmeMaisAtuado = new Alarmes()
                    {
                        IdAlarme = Convert.ToInt32(reader["id"]),
                        DescricaoAlarme = Convert.ToString(reader["descricao"]),
                        NomeClassificacaoAlarme = Convert.ToString(reader["nome_classificacao_alarme"]),
                        Nome = Convert.ToString(reader["nome_equipamento_relacionado"]),
                        DataDeCadastroAlarme = Convert.ToString(reader["data_de_cadastro"]),
                    };

                    alarmesMaisAtuadosLista.Add(alarmeMaisAtuado);
                }
                _con.Close();

                return alarmesMaisAtuadosLista;
            }
        }

        public List<Alarmes> ObterAlarmesMaisAtuados(){
            Connection();
            List<Alarmes> alarmesMaisAtuadosLista = new List<Alarmes>();

            using(NpgsqlCommand command = new NpgsqlCommand("select * from alarmesMaisAtuadosView", _con)){
                _con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read()){
                    Alarmes alarmeMaisAtuado = new Alarmes()
                    {
                        IdAlarme = Convert.ToInt32(reader["id"]),
                        DescricaoAlarme = Convert.ToString(reader["descricao"]),
                        NomeClassificacaoAlarme = Convert.ToString(reader["nome_classificacao_alarme"]),
                        Nome = Convert.ToString(reader["nome_equipamento_relacionado"]),
                        VezesAtuadas = Convert.ToInt32(reader["vezesAtuadas"]),
                    };

                    alarmesMaisAtuadosLista.Add(alarmeMaisAtuado);
                }
                _con.Close();

                return alarmesMaisAtuadosLista;
            }
        }
    }
}