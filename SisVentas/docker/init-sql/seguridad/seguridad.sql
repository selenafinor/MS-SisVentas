-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: localhost    Database: db_security
-- ------------------------------------------------------
-- Server version	9.7.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ 'fed30251-605e-11f1-acbc-1ea2159c5b27:1-464';

--
-- Table structure for table `Access`
--

DROP TABLE IF EXISTS `Access`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Access` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Fullname` varchar(255) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(50) NOT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Access`
--

LOCK TABLES `Access` WRITE;
/*!40000 ALTER TABLE `Access` DISABLE KEYS */;
INSERT INTO `Access` VALUES (1,'Ivan Cuadros Altamirano','aforo255','123456');
/*!40000 ALTER TABLE `Access` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Permiso`
--

DROP TABLE IF EXISTS `Permiso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Permiso` (
  `ID_Permiso` int NOT NULL AUTO_INCREMENT,
  `Nombre_Permiso` varchar(50) NOT NULL,
  `Descripcion` varchar(255) DEFAULT NULL,
  `Fecha_Creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID_Permiso`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Permiso`
--

LOCK TABLES `Permiso` WRITE;
/*!40000 ALTER TABLE `Permiso` DISABLE KEYS */;
INSERT INTO `Permiso` VALUES (16,'gestionar_usuarios','Gestionar usuarios del sistema','2026-06-13 19:12:04'),(17,'gestionar_roles','Gestionar roles del sistema','2026-06-13 19:12:15'),(18,'gestionar_permisos','Gestionar permisos del sistema','2026-06-13 19:12:23'),(19,'ver_reportes','Ver reportes del sistema','2026-06-13 19:12:40'),(20,'gestionar_compras','Gestionar compras','2026-06-13 19:12:54'),(22,'gestionar_ventas','Gestionar ventas','2026-06-13 19:13:23'),(23,'gestionar_inventario','Gestionar inventario','2026-06-13 19:14:33');
/*!40000 ALTER TABLE `Permiso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Rol`
--

DROP TABLE IF EXISTS `Rol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Rol` (
  `ID_Rol` int NOT NULL AUTO_INCREMENT,
  `Nombre_Rol` varchar(50) NOT NULL,
  `Descripcion` varchar(255) DEFAULT NULL,
  `Fecha_Creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `Estado` varchar(20) DEFAULT 'activo',
  PRIMARY KEY (`ID_Rol`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Rol`
--

LOCK TABLES `Rol` WRITE;
/*!40000 ALTER TABLE `Rol` DISABLE KEYS */;
INSERT INTO `Rol` VALUES (1,'Administrador','Rol con privilegios completos.','2026-06-09 00:00:00','activo'),(2,'Encargado ventas','Rol encargado de gestionar las ventas.','2026-06-09 00:00:00','activo'),(3,'Gerente','Rol de gerencia','2026-06-13 18:44:18','activo');
/*!40000 ALTER TABLE `Rol` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Rol_Permiso`
--

DROP TABLE IF EXISTS `Rol_Permiso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Rol_Permiso` (
  `ID_Rol_Permiso` int NOT NULL AUTO_INCREMENT,
  `ID_Rol` int NOT NULL,
  `ID_Permiso` int NOT NULL,
  PRIMARY KEY (`ID_Rol_Permiso`),
  KEY `rol_permiso_ibfk_1` (`ID_Rol`),
  KEY `rol_permiso_ibfk_2` (`ID_Permiso`),
  CONSTRAINT `rol_permiso_ibfk_1` FOREIGN KEY (`ID_Rol`) REFERENCES `Rol` (`ID_Rol`) ON DELETE CASCADE,
  CONSTRAINT `rol_permiso_ibfk_2` FOREIGN KEY (`ID_Permiso`) REFERENCES `Permiso` (`ID_Permiso`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Rol_Permiso`
--

LOCK TABLES `Rol_Permiso` WRITE;
/*!40000 ALTER TABLE `Rol_Permiso` DISABLE KEYS */;
INSERT INTO `Rol_Permiso` VALUES (24,1,16),(25,1,17),(26,1,18),(27,1,19),(28,1,20),(29,1,22),(30,1,23),(32,2,22),(35,3,20),(36,3,22),(37,3,23),(38,3,19);
/*!40000 ALTER TABLE `Rol_Permiso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Rol_Permiso_Usuario`
--

DROP TABLE IF EXISTS `Rol_Permiso_Usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Rol_Permiso_Usuario` (
  `ID_Usuario_Rol_Permiso` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ID_Rol_Permiso` int NOT NULL,
  PRIMARY KEY (`ID_Usuario_Rol_Permiso`),
  KEY `rol_permiso_usuario_ibfk_1` (`UserId`),
  KEY `rol_permiso_usuario_ibfk_2` (`ID_Rol_Permiso`),
  CONSTRAINT `rol_permiso_usuario_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `Usuario` (`UserId`) ON DELETE CASCADE,
  CONSTRAINT `rol_permiso_usuario_ibfk_2` FOREIGN KEY (`ID_Rol_Permiso`) REFERENCES `Rol_Permiso` (`ID_Rol_Permiso`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=78 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Rol_Permiso_Usuario`
--

LOCK TABLES `Rol_Permiso_Usuario` WRITE;
/*!40000 ALTER TABLE `Rol_Permiso_Usuario` DISABLE KEYS */;
INSERT INTO `Rol_Permiso_Usuario` VALUES (49,1,24),(50,1,25),(51,1,26),(52,1,27),(53,1,28),(54,1,29),(55,1,30),(56,7,24),(57,7,25),(58,7,26),(59,7,27),(60,7,28),(61,7,29),(62,7,30),(63,8,24),(64,8,25),(65,8,26),(66,8,27),(67,8,28),(68,8,29),(69,8,30),(70,9,35),(71,9,36),(72,9,37),(73,9,38),(75,10,32);
/*!40000 ALTER TABLE `Rol_Permiso_Usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Usuario`
--

DROP TABLE IF EXISTS `Usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Usuario` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Fullname` varchar(255) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(50) NOT NULL,
  `Correo` varchar(100) DEFAULT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `Estado` varchar(20) DEFAULT 'activo',
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Usuario`
--

LOCK TABLES `Usuario` WRITE;
/*!40000 ALTER TABLE `Usuario` DISABLE KEYS */;
INSERT INTO `Usuario` VALUES (1,'Edwin Calle','edwincalle','ect*123','calle@tecnoweb.net',NULL,'activo'),(7,'Liliana Juarez Paniagua','lili','ect*123','lili@tecnoweb.net','78945621','activo'),(8,'Selena Flores Olivares','admin','ect*123','selena@tecnoweb.net','78956421','activo'),(9,'Renato Llanos Vaca','Renato','ect*123','renato@tecnoweb.net','78945612','activo'),(10,'Claudia Guzman Medina','Claudia','ect*123','claudia@tecnoweb.net','78963222','activo');
/*!40000 ALTER TABLE `Usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'db_security'
--
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-07-05 13:45:23
