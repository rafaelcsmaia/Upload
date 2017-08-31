
CREATE TABLE [AUTENTICACAO] (
[Usuario] varchar(10) NOT NULL ,
[Perfil] int NOT NULL 
);

CREATE TABLE [FUNCIONALIDADE] (
[Codigo] int NOT NULL ,
[Controller] varchar(40) NOT NULL ,
[Action] varchar(40) NOT NULL ,
[Descricao] varchar(50) NOT NULL ,
[Area] varchar(50) NOT NULL ,
[ControllerDescription] varchar(40) NOT NULL 
);

CREATE TABLE [PERFIL] (
[Codigo] int NOT NULL ,
[Descricao] varchar(255) NOT NULL 
);

CREATE TABLE [PERMISSAO] (
[Perfil] int NOT NULL ,
[Funcionalidade] int NOT NULL 
);

CREATE TABLE [SMTP] (
[Host] varchar(60) NOT NULL ,
[Porta] smallint NOT NULL ,
[Username] varchar(100) NOT NULL ,
[Password] varchar(25) NOT NULL ,
[SSLEnabled] smallint NOT NULL 
);

CREATE TABLE [USUARIO] (
[Username] varchar(10) NOT NULL ,
[Nome] varchar(50) NOT NULL ,
[Password] varchar(88) NOT NULL ,
[Status] tinyint NOT NULL ,
[Email] varchar(60) NOT NULL 
);

ALTER TABLE [AUTENTICACAO] ADD PRIMARY KEY ([Usuario], [Perfil]);

ALTER TABLE [FUNCIONALIDADE] ADD PRIMARY KEY ([Codigo]);

ALTER TABLE [PERFIL] ADD PRIMARY KEY ([Codigo]);

ALTER TABLE [PERMISSAO] ADD PRIMARY KEY ([Perfil], [Funcionalidade]);

ALTER TABLE [SMTP] ADD PRIMARY KEY ([Host]);

ALTER TABLE [USUARIO] ADD PRIMARY KEY ([Username]);

ALTER TABLE [AUTENTICACAO] ADD FOREIGN KEY ([Perfil]) REFERENCES [PERFIL] ([Codigo]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE [AUTENTICACAO] ADD FOREIGN KEY ([Usuario]) REFERENCES [USUARIO] ([Username]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE [PERMISSAO] ADD FOREIGN KEY ([Funcionalidade]) REFERENCES [FUNCIONALIDADE] ([Codigo]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE [PERMISSAO] ADD FOREIGN KEY ([Perfil]) REFERENCES [PERFIL] ([Codigo]) ON DELETE CASCADE ON UPDATE CASCADE;