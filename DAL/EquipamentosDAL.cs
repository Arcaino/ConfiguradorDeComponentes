using Npgsql;
using ConfiguradorDeComponents.Models;
using System;
using System.Collections.Generic;


namespace ConfiguradorDeComponents.DAL
{
    public class EquipamentosDAL
    {
        private NpgsqlConnection _con;

        private void Connection(){
            string constr = "Server=srv-dev;Port=5432;User Id=postgres;Password=geodados;Database=claudio-teste";
            _con = new NpgsqlConnection(constr);
        }

        public List<Equipamentos> ObterEquipamentos(){
            Connection();
            List<Equipamentos> equipamentosLista = new List<Equipamentos>();

            using(NpgsqlCommand command = new NpgsqlCommand("select * from equipamentosView", _con)){
                _con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read()){
                    Equipamentos equipamento = new Equipamentos()
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nome = Convert.ToString(reader["nome"]),
                        Descricao = Convert.ToString(reader["descricao"]),
                        NomeDoTipoDeEquipamento = Convert.ToString(reader["tipo_do_equipamento"]),
                        NumeroDeSerie = Convert.ToInt32(reader["numero_de_serie"]),
                        DataCadastro = Convert.ToString(reader["data_de_cadastro"]),
                    };

                    equipamentosLista.Add(equipamento);
                }
                _con.Close();

                return equipamentosLista;
            }   
        }

        public bool AdicionarEquipamento(Equipamentos equipamentosObj){
            Connection();

            int retornoCasoSucesso;

            using(NpgsqlCommand command = new NpgsqlCommand("INSERT INTO equipamentos (nome, descricao, numeroDeSerie, tipoDoEquipamentoId, dataDeCadastro) VALUES" 
                                                             +"(@Nome, @Descricao, @NumeroDeSerie, @TipoDoEquipamentoId, (SELECT DATE_TRUNC('second', CURRENT_TIMESTAMP::timestamp)))", _con)){
                command.Parameters.AddWithValue("@Nome", equipamentosObj.Nome);
                command.Parameters.AddWithValue("@Descricao", equipamentosObj.Descricao);
                command.Parameters.AddWithValue("@NumeroDeSerie", equipamentosObj.NumeroDeSerie);
                command.Parameters.AddWithValue("@tipoDoEquipamentoId", equipamentosObj.IdDoTipoDeEquipamento);

                _con.Open();

                retornoCasoSucesso = Convert.ToInt32(command.ExecuteScalar());
            }

            _con.Close();

            return retornoCasoSucesso >= 1;
        }

        public bool EditarEquipamento(Equipamentos equipamentosObj){
            Connection();

            int retornoCasoSucesso;

            using(NpgsqlCommand command = new NpgsqlCommand("UPDATE equipamentos SET (nome, descricao, numeroDeSerie, tipoDoEquipamentoId) = "
                                                             +   "(@Nome, @Descricao, @NumeroDeSerie, @TipoDoEquipamentoId) WHERE id = @Id;", _con)){
                command.Parameters.AddWithValue("@Id", equipamentosObj.Id);
                command.Parameters.AddWithValue("@Nome", equipamentosObj.Nome);
                command.Parameters.AddWithValue("@Descricao", equipamentosObj.Descricao);
                command.Parameters.AddWithValue("@NumeroDeSerie", equipamentosObj.NumeroDeSerie);
                command.Parameters.AddWithValue("@tipoDoEquipamentoId", equipamentosObj.IdDoTipoDeEquipamento);

                _con.Open();

                retornoCasoSucesso = Convert.ToInt32(command.ExecuteScalar());
            }

            _con.Close();

            return retornoCasoSucesso >= 1;
        }

        public bool DeletarEquipamento(int id){
            Connection();

            int retornoCasoSucesso;

            using(NpgsqlCommand command = new NpgsqlCommand("select deletarEquipamento(@id);", _con)){
                command.Parameters.AddWithValue("@id", id);

                _con.Open();

                retornoCasoSucesso = command.ExecuteNonQuery();
            }

            _con.Close();

            return retornoCasoSucesso >= 1;
        }
    }
}