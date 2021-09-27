CREATE TABLE tipoEquipamento(
	id		serial	PRIMARY KEY,
	nome	varchar(10)
);

INSERT INTO tipoEquipamento (nome) VALUES ('Tensão');
INSERT INTO tipoEquipamento (nome) VALUES ('Corrente');
INSERT INTO tipoEquipamento (nome) VALUES ('Óleo');

CREATE TABLE equipamentos(
	id						serial	PRIMARY KEY,
	nome					varchar(50),
	numeroDeSerie			int,
	tipoDoEquipamentoId		int,
	descricao				varchar(200),
	dataDeCadastro			text,
	FOREIGN KEY (tipoDoEquipamentoId) REFERENCES tipoEquipamento (id)
);

INSERT INTO equipamentos (nome, numeroDeSerie, tipoDoEquipamentoId, descricao, dataDeCadastro) VALUES ('Pareador', 2, 2, 'Responsável por parear dois componentes', '2021-09-27 08:25');
INSERT INTO equipamentos (nome, numeroDeSerie, tipoDoEquipamentoId, descricao, dataDeCadastro) VALUES ('Roteador', 11, 1, 'Compartilhamento de redes', '2021-09-27 11:59');
INSERT INTO equipamentos (nome, numeroDeSerie, tipoDoEquipamentoId, descricao, dataDeCadastro) VALUES ('Regulador', 22, 3, 'Manipular nível dos componentes', '2021-09-27 12:22');

CREATE OR REPLACE FUNCTION deletarEquipamento(idAlarme int) RETURNS void AS $$
    BEGIN
		DELETE FROM alarmes WHERE equipamentoRelacionadoId = idAlarme;
		DELETE FROM equipamentos WHERE id = idAlarme;
    END;
$$ LANGUAGE plpgsql;

CREATE VIEW equipamentosView AS
SELECT
	e.id,
	e.nome,
	e.descricao,
	te.nome as "tipo_do_equipamento",
	e.numeroDeSerie as "numero_de_serie",
	e.dataDeCadastro as "data_de_cadastro"
FROM
	equipamentos e
INNER JOIN tipoEquipamento te 
    ON e.tipoDoEquipamentoId = te.id
ORDER BY e.id;

CREATE TABLE classificacaoAlarme(
	id		serial	PRIMARY KEY,
	nome	varchar(10)
);

INSERT INTO classificacaoAlarme (nome) VALUES ('Alto');
INSERT INTO classificacaoAlarme (nome) VALUES ('Médio');
INSERT INTO classificacaoAlarme (nome) VALUES ('Baixo');

CREATE TABLE alarmes(
	id							serial	PRIMARY KEY,
	descricao					varchar(200),
	classificacaoId				int,
	equipamentoRelacionadoId	int,
	dataDeCadastro				text,
	status						bool,
	dataEntrada					text,
	dataSaida					text,
	vezesAtuadas				int,
	FOREIGN KEY (classificacaoId) 			REFERENCES classificacaoAlarme (id),
	FOREIGN KEY (equipamentoRelacionadoId) 	REFERENCES equipamentos (id)
);

INSERT INTO alarmes(descricao, classificacaoId, equipamentoRelacionadoId, dataDeCadastro, status, dataEntrada, dataSaida, vezesAtuadas) 
VALUES ('Verificar se o pareamento está ativo', 2, 1, '2021-10-09 09:20', false, '', '', 0);
INSERT INTO alarmes(descricao, classificacaoId, equipamentoRelacionadoId, dataDeCadastro, status, dataEntrada, dataSaida, vezesAtuadas) 
VALUES ('Informar se a rede desestabilizou', 1, 2, '2021-10-10 07:22', false, '', '', 0);
INSERT INTO alarmes(descricao, classificacaoId, equipamentoRelacionadoId, dataDeCadastro, status, dataEntrada, dataSaida, vezesAtuadas) 
VALUES ('Checar se os componentes estão nivelados', 3, 3, '2021-10-09 10:32', false, '', '', 0);
INSERT INTO alarmes(descricao, classificacaoId, equipamentoRelacionadoId, dataDeCadastro, status, dataEntrada, dataSaida, vezesAtuadas) 
VALUES ('Alertar o desligamento repentino', 1, 3, '2021-10-09 10:35', false, '', '', 0);
INSERT INTO alarmes(descricao, classificacaoId, equipamentoRelacionadoId, dataDeCadastro, status, dataEntrada, dataSaida, vezesAtuadas) 
VALUES ('Encaminhamento divergente de informações', 3, 2, '2021-10-12 09:44', false, '', '', 0);

CREATE VIEW alarmesView AS
SELECT
	a.id,
	e.nome as "nome_equipamento_relacionado",
	a.descricao,
	ca.nome as "nome_classificacao_alarme",
	a.dataDeCadastro as "data_de_cadastro"
FROM
	alarmes a
INNER JOIN classificacaoAlarme ca
    ON a.classificacaoId = ca.id
INNER JOIN equipamentos e
    ON a.equipamentoRelacionadoId = e.id
ORDER BY a.id;

CREATE VIEW alarmesParaAtuarView AS
SELECT
	a.id,
	a.descricao,
	e.nome as "nome_equipamento_relacionado",
	e.descricao as "descricao_equipamento_relacionado",
	a.status,
	a.dataEntrada,
	a.dataSaida
FROM
	alarmes a
INNER JOIN classificacaoAlarme ca
    ON a.classificacaoId = ca.id
INNER JOIN equipamentos e
    ON a.equipamentoRelacionadoId = e.id
ORDER BY a.id;

CREATE OR REPLACE FUNCTION atuarAlarme(statusAtual bool, idAlarme int) RETURNS void AS $$
    BEGIN
		UPDATE alarmes SET (datasaida, vezesAtuadas) = ((select dataEntrada from alarmes where id = idAlarme), vezesAtuadas+1) WHERE id = idAlarme;
		UPDATE alarmes SET (status, dataEntrada) = (statusAtual, (SELECT DATE_TRUNC('second', CURRENT_TIMESTAMP::timestamp))) WHERE id = idAlarme;
    END;
$$ LANGUAGE plpgsql;

CREATE VIEW alarmesAtuadosView AS
SELECT
	a.id,
	e.nome as "nome_equipamento_relacionado",
	a.descricao,
	ca.nome as "nome_classificacao_alarme",
	a.dataDeCadastro as "data_de_cadastro",
	a.vezesAtuadas
FROM
	alarmes a
INNER JOIN classificacaoAlarme ca
    ON a.classificacaoId = ca.id
INNER JOIN equipamentos e
    ON a.equipamentoRelacionadoId = e.id
WHERE a.status = true;

CREATE VIEW alarmesMaisAtuadosView AS
SELECT
	a.id,
	e.nome as "nome_equipamento_relacionado",
	a.descricao,
	ca.nome as "nome_classificacao_alarme",
	a.vezesAtuadas
FROM
	alarmes a
INNER JOIN classificacaoAlarme ca
    ON a.classificacaoId = ca.id
INNER JOIN equipamentos e
    ON a.equipamentoRelacionadoId = e.id
ORDER BY vezesatuadas DESC LIMIT 3;