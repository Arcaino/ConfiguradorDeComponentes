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
	dataDeCadastro			timestamp,
	FOREIGN KEY (tipoDoEquipamentoId) REFERENCES tipoEquipamento (id)
);

CREATE VIEW equipamentosView AS
SELECT
	e.id,
	e.nome,
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
	descricao					varchar(100),
	classificacaoId				int,
	equipamentoRelacionadoId	int,
	dataDeCadastro				timestamp,
	status						bool,
	FOREIGN KEY (classificacaoId) 			REFERENCES classificacaoAlarme (id),
	FOREIGN KEY (equipamentoRelacionadoId) 	REFERENCES equipamentos (id)
);

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