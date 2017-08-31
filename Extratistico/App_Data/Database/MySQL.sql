SET FOREIGN_KEY_CHECKS=0;

DROP TABLE IF EXISTS `autenticacao`;
CREATE TABLE `autenticacao` (
  `Usuario` varchar(10) NOT NULL,
  `Perfil` int(11) NOT NULL,
  PRIMARY KEY (`Usuario`,`Perfil`),
  KEY `FK_PERFIL_AUT` (`Perfil`),
  CONSTRAINT `autenticacao_ibfk_1` FOREIGN KEY (`Perfil`) REFERENCES `perfil` (`Codigo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `autenticacao_ibfk_2` FOREIGN KEY (`Usuario`) REFERENCES `usuario` (`Username`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `funcionalidade`;
CREATE TABLE `funcionalidade` (
  `Codigo` int(11) NOT NULL,
  `Controller` varchar(40) NOT NULL,
  `Action` varchar(40) NOT NULL,
  `Descricao` varchar(50) NOT NULL,
  `Area` varchar(50) NOT NULL,
  `ControllerDescription` varchar(40) NOT NULL,
  PRIMARY KEY (`Codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `perfil`;
CREATE TABLE `perfil` (
  `Codigo` int(11) NOT NULL,
  `Descricao` varchar(255) NOT NULL,
  PRIMARY KEY (`Codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `permissao`;
CREATE TABLE `permissao` (
  `Perfil` int(11) NOT NULL,
  `Funcionalidade` int(11) NOT NULL,
  PRIMARY KEY (`Perfil`,`Funcionalidade`),
  KEY `FK_FUNC_PERMISSAO` (`Funcionalidade`),
  CONSTRAINT `permissao_ibfk_1` FOREIGN KEY (`Funcionalidade`) REFERENCES `funcionalidade` (`Codigo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `permissao_ibfk_2` FOREIGN KEY (`Perfil`) REFERENCES `perfil` (`Codigo`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `smtp`;
CREATE TABLE `smtp` (
  `Host` varchar(60) NOT NULL,
  `Porta` int(5) NOT NULL,
  `Username` varchar(100) NOT NULL,
  `Password` varchar(25) NOT NULL,
  `SSLEnabled` tinyint(4) NOT NULL,
  PRIMARY KEY (`Host`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE `usuario` (
  `Username` varchar(10) NOT NULL,
  `Nome` varchar(50) NOT NULL,
  `Password` varchar(88) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `Email` varchar(60) NOT NULL,
  PRIMARY KEY (`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
