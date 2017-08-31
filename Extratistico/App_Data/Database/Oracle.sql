CREATE TABLE autenticacao (
Usuario VARCHAR2(10 BYTE) NOT NULL ,
Perfil NUMBER(11) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

CREATE TABLE funcionalidade (
Codigo NUMBER(11) NOT NULL ,
Controller VARCHAR2(40 BYTE) NOT NULL ,
Action VARCHAR2(40 BYTE) NOT NULL ,
Descricao VARCHAR2(50 BYTE) NOT NULL ,
Area VARCHAR2(50 BYTE) NOT NULL ,
ControllerDescription VARCHAR2(40 BYTE) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

CREATE TABLE perfil (
Codigo NUMBER(11) NOT NULL ,
Descricao VARCHAR2(255 BYTE) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

CREATE TABLE permissao (
Perfil NUMBER(11) NOT NULL ,
funcionalidade NUMBER(11) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

CREATE TABLE smtp (
Host VARCHAR2(60 BYTE) NOT NULL ,
Porta NUMBER(5) NOT NULL ,
Username VARCHAR2(100 BYTE) NOT NULL ,
Password VARCHAR2(25 BYTE) NOT NULL ,
SSLEnabled NUMBER(4) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

CREATE TABLE usuario (
Username VARCHAR2(10 BYTE) NOT NULL ,
Nome VARCHAR2(50 BYTE) NOT NULL ,
Password VARCHAR2(88 BYTE) NOT NULL ,
Status NUMBER(1) NOT NULL ,
Email VARCHAR2(60 BYTE) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

-- ----------------------------
-- Indexes structure for table autenticacao
-- ----------------------------

-- ----------------------------
-- Checks structure for table autenticacao
-- ----------------------------
ALTER TABLE autenticacao ADD CHECK (Usuario IS NOT NULL);
ALTER TABLE autenticacao ADD CHECK (Perfil IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table autenticacao
-- ----------------------------
ALTER TABLE autenticacao ADD PRIMARY KEY (Usuario, Perfil);

-- ----------------------------
-- Indexes structure for table funcionalidade
-- ----------------------------

-- ----------------------------
-- Checks structure for table funcionalidade
-- ----------------------------
ALTER TABLE funcionalidade ADD CHECK (Codigo IS NOT NULL);
ALTER TABLE funcionalidade ADD CHECK (Controller IS NOT NULL);
ALTER TABLE funcionalidade ADD CHECK (Action IS NOT NULL);
ALTER TABLE funcionalidade ADD CHECK (Descricao IS NOT NULL);
ALTER TABLE funcionalidade ADD CHECK (Area IS NOT NULL);
ALTER TABLE funcionalidade ADD CHECK (ControllerDescription IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table funcionalidade
-- ----------------------------
ALTER TABLE funcionalidade ADD PRIMARY KEY (Codigo);

-- ----------------------------
-- Indexes structure for table perfil
-- ----------------------------

-- ----------------------------
-- Checks structure for table perfil
-- ----------------------------
ALTER TABLE perfil ADD CHECK (Codigo IS NOT NULL);
ALTER TABLE perfil ADD CHECK (Descricao IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table perfil
-- ----------------------------
ALTER TABLE perfil ADD PRIMARY KEY (Codigo);

-- ----------------------------
-- Indexes structure for table permissao
-- ----------------------------

-- ----------------------------
-- Checks structure for table permissao
-- ----------------------------
ALTER TABLE permissao ADD CHECK (Perfil IS NOT NULL);
ALTER TABLE permissao ADD CHECK (funcionalidade IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table permissao
-- ----------------------------
ALTER TABLE permissao ADD PRIMARY KEY (Perfil, funcionalidade);

-- ----------------------------
-- Indexes structure for table smtp
-- ----------------------------

-- ----------------------------
-- Checks structure for table smtp
-- ----------------------------
ALTER TABLE smtp ADD CHECK (Host IS NOT NULL);
ALTER TABLE smtp ADD CHECK (Porta IS NOT NULL);
ALTER TABLE smtp ADD CHECK (Username IS NOT NULL);
ALTER TABLE smtp ADD CHECK (Password IS NOT NULL);
ALTER TABLE smtp ADD CHECK (SSLEnabled IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table smtp
-- ----------------------------
ALTER TABLE smtp ADD PRIMARY KEY (Host);

-- ----------------------------
-- Indexes structure for table usuario
-- ----------------------------

-- ----------------------------
-- Checks structure for table usuario
-- ----------------------------
ALTER TABLE usuario ADD CHECK (Username IS NOT NULL);
ALTER TABLE usuario ADD CHECK (Nome IS NOT NULL);
ALTER TABLE usuario ADD CHECK (Password IS NOT NULL);
ALTER TABLE usuario ADD CHECK (Status IS NOT NULL);
ALTER TABLE usuario ADD CHECK (Email IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table usuario
-- ----------------------------
ALTER TABLE usuario ADD PRIMARY KEY (Username);

-- ----------------------------
-- Foreign Key structure for table autenticacao
-- ----------------------------
ALTER TABLE autenticacao ADD FOREIGN KEY (Perfil) REFERENCES perfil (Codigo) ON DELETE CASCADE;
ALTER TABLE autenticacao ADD FOREIGN KEY (Usuario) REFERENCES usuario (Username) ON DELETE CASCADE;

-- ----------------------------
-- Foreign Key structure for table permissao
-- ----------------------------
ALTER TABLE permissao ADD FOREIGN KEY (funcionalidade) REFERENCES funcionalidade (Codigo) ON DELETE CASCADE;
ALTER TABLE permissao ADD FOREIGN KEY (Perfil) REFERENCES perfil (Codigo) ON DELETE CASCADE;