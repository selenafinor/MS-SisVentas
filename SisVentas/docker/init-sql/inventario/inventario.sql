-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: localhost    Database: db_inventario
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
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20260610133626_InitialCreate','5.0.3'),('20260612012245_AddFotoToArticulo','5.0.3'),('20260620193608_AddGesPrecio','5.0.3'),('20260621164807_AddSoloRegistroToIngreso','5.0.17');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `almacen`
--

DROP TABLE IF EXISTS `almacen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `almacen` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Descripcion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Direccion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CantidadMax` int DEFAULT NULL,
  `Estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `almacen`
--

LOCK TABLES `almacen` WRITE;
/*!40000 ALTER TABLE `almacen` DISABLE KEYS */;
INSERT INTO `almacen` VALUES (1,'Almacen Central','Almacen principal','Av. Principal 123',2000,'activo'),(2,'almacen 2',NULL,NULL,0,'activo');
/*!40000 ALTER TABLE `almacen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `articulo`
--

DROP TABLE IF EXISTS `articulo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `articulo` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Descripcion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Precio` decimal(65,30) NOT NULL,
  `Estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Id_Marca` int DEFAULT NULL,
  `Id_Categoria` int DEFAULT NULL,
  `Id_UnidadMedida` int DEFAULT NULL,
  `Foto` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `Id_Marca` (`Id_Marca`),
  KEY `Id_Categoria` (`Id_Categoria`),
  KEY `Id_UnidadMedida` (`Id_UnidadMedida`),
  CONSTRAINT `articulo_ibfk_1` FOREIGN KEY (`Id_Marca`) REFERENCES `marca` (`Id`),
  CONSTRAINT `articulo_ibfk_2` FOREIGN KEY (`Id_Categoria`) REFERENCES `categoria` (`Id`),
  CONSTRAINT `articulo_ibfk_3` FOREIGN KEY (`Id_UnidadMedida`) REFERENCES `unidad_medida` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `articulo`
--

LOCK TABLES `articulo` WRITE;
/*!40000 ALTER TABLE `articulo` DISABLE KEYS */;
INSERT INTO `articulo` VALUES (1,'Televisor LED 32\"','',0.050000000000000000000000000000,'activo',1,3,1,'/uploads/articulo_1_images (8).jpg'),(2,'Televisor LED 43\"','',0.800000000000000000000000000000,'activo',2,3,1,'/uploads/articulo_2_televisor_lg_43_pulgadas_samrt_tv_4k_nancell_ur8750.webp'),(3,'Televisor LED 55\"','',0.850000000000000000000000000000,'activo',3,3,1,'/uploads/articulo_3_serie_x85j_1500x1500px_55inch_01_b_mainimage.webp'),(4,'Reproductor DVD','',0.650000000000000000000000000000,'activo',4,3,1,'/uploads/articulo_4_816R4Lb7DHL._AC_UF1000,1000_QL80_.jpg'),(5,'Sistema de Sonido','',0.700000000000000000000000000000,'activo',3,3,1,'/uploads/articulo_5_90586.webp'),(6,'Resistencia 1k Ohm','',0.050000000000000000000000000000,'activo',8,2,1,'/uploads/articulo_6_image_1024.jpg'),(7,'Condensador 100uF','',0.080000000000000000000000000000,'activo',8,2,1,'/uploads/articulo_7_images (5).jpg'),(8,'Transistor BC547','',0.100000000000000000000000000000,'activo',8,2,1,'/uploads/articulo_8_D_NQ_NP_611395-MLM78252815515_082024-O.webp'),(9,'Diodo 1N4007','',0.020000000000000000000000000000,'activo',8,2,1,'/uploads/articulo_9_images (1).png'),(10,'LED Rojo 5mm','',0.050000000000000000000000000000,'activo',8,2,1,'/uploads/articulo_10_diodo-led-5mm-rojo.jpg'),(11,'Cargador Universal','',0.350000000000000000000000000000,'activo',8,4,1,'/uploads/articulo_11_IGG319895grid_Mesadetrabajo1_17515316749_1.webp'),(12,'Fuente de Poder 500W','',0.600000000000000000000000000000,'activo',8,4,1,'/uploads/articulo_12_81seo5K3+tL._AC_UF894,1000_QL80_.jpg'),(13,'Bateria 9V','',0.200000000000000000000000000000,'activo',5,4,1,'/uploads/articulo_13_1422-philips-6lr61p1b-10-pila-alcalina-9v.webp'),(14,'Bateria AA x4','',0.250000000000000000000000000000,'activo',5,4,1,'/uploads/articulo_14_31104.png'),(15,'Soldador 30W','',0.450000000000000000000000000000,'activo',6,5,1,'/uploads/articulo_15_613faLtkmUL._AC_UF894,1000_QL80_.jpg'),(16,'Multimetro Digital','',0.550000000000000000000000000000,'activo',8,5,1,'/uploads/articulo_16_3.jpg'),(17,'Cautin de Punta Fina','',0.400000000000000000000000000000,'activo',6,5,1,'/uploads/articulo_17_D_756579-MLM90224001875_082025-C.jpg'),(18,'Pantalla LCD Samsung 32\"','',0.750000000000000000000000000000,'activo',1,6,1,'/uploads/articulo_18_pantalla repuesto sansung.jpg'),(19,'Tarjeta Main LG','',0.700000000000000000000000000000,'activo',2,6,1,'/uploads/articulo_19_16_1_1.jpg'),(20,'Control Remoto Universal','',0.150000000000000000000000000000,'activo',8,6,1,'/uploads/articulo_20_dd61318a4c9c3bd8691dee0d28a6.jpg'),(21,'Router WiFi ','',0.400000000000000000000000000000,'activo',7,1,1,'/uploads/articulo_21_images (7).jpg'),(22,'Cable HDMI 1.8m','',0.200000000000000000000000000000,'activo',8,1,1,'/uploads/articulo_22_2l-7d02h21.cables.hdmi-cables.45.jpg'),(23,'Regleta 6 Tomas','',0.250000000000000000000000000000,'activo',8,1,1,'/uploads/articulo_23_images (6).jpg'),(24,'cable tipo C',NULL,0.200000000000000000000000000000,'activo',1,1,1,NULL);
/*!40000 ALTER TABLE `articulo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `articulo_almacen`
--

DROP TABLE IF EXISTS `articulo_almacen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `articulo_almacen` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Stock` int DEFAULT NULL,
  `StockMin` int DEFAULT NULL,
  `StockMax` int DEFAULT NULL,
  `Id_Articulo` int DEFAULT NULL,
  `Id_Almacen` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Id_Articulo` (`Id_Articulo`),
  KEY `Id_Almacen` (`Id_Almacen`),
  CONSTRAINT `articulo_almacen_ibfk_1` FOREIGN KEY (`Id_Articulo`) REFERENCES `articulo` (`Id`),
  CONSTRAINT `articulo_almacen_ibfk_2` FOREIGN KEY (`Id_Almacen`) REFERENCES `almacen` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `articulo_almacen`
--

LOCK TABLES `articulo_almacen` WRITE;
/*!40000 ALTER TABLE `articulo_almacen` DISABLE KEYS */;
INSERT INTO `articulo_almacen` VALUES (1,285,20,200,1,1),(2,49,20,50,2,1),(3,42,20,100,3,1),(4,93,20,200,4,1),(5,9,2,10,5,1),(6,88,20,200,6,1),(7,47,20,50,7,1),(8,41,20,100,8,1),(9,197,20,200,9,1),(10,71,20,200,10,1),(11,99,20,100,11,1),(12,23,20,50,12,1),(13,191,10,200,13,1),(14,195,50,200,14,1),(15,29,10,30,15,1),(16,18,10,50,16,1),(17,21,10,30,17,1),(18,10,2,15,18,1),(19,6,2,10,19,1),(20,46,15,50,20,1),(21,20,3,15,21,1),(22,97,20,100,22,1),(23,40,20,50,23,1),(24,6,6,15,24,1),(25,1,2,15,21,2),(26,10,0,0,9,2),(27,100,0,0,1,2);
/*!40000 ALTER TABLE `articulo_almacen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categoria`
--

DROP TABLE IF EXISTS `categoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categoria` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Descripcion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categoria`
--

LOCK TABLES `categoria` WRITE;
/*!40000 ALTER TABLE `categoria` DISABLE KEYS */;
INSERT INTO `categoria` VALUES (1,'Accesorio de Montaje',NULL,NULL),(2,'Componente Electrnico',NULL,NULL),(3,'Equipo Electrnico',NULL,NULL),(4,'Fuente de Energa',NULL,NULL),(5,'Herramienta y Accesorio',NULL,NULL),(6,'Repuesto de Aparato',NULL,NULL);
/*!40000 ALTER TABLE `categoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalle_egreso`
--

DROP TABLE IF EXISTS `detalle_egreso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalle_egreso` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Cantidad` int DEFAULT NULL,
  `Observacion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Id_Egreso` int DEFAULT NULL,
  `Id_ArticuloAlmacen` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Id_Egreso` (`Id_Egreso`),
  KEY `Id_ArticuloAlmacen` (`Id_ArticuloAlmacen`),
  CONSTRAINT `detalle_egreso_ibfk_1` FOREIGN KEY (`Id_Egreso`) REFERENCES `nota_egreso` (`Id`),
  CONSTRAINT `detalle_egreso_ibfk_2` FOREIGN KEY (`Id_ArticuloAlmacen`) REFERENCES `articulo_almacen` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalle_egreso`
--

LOCK TABLES `detalle_egreso` WRITE;
/*!40000 ALTER TABLE `detalle_egreso` DISABLE KEYS */;
INSERT INTO `detalle_egreso` VALUES (1,1,'',2,21),(2,1,'Venta #8',3,1),(3,1,'Venta #9',4,6),(4,1,'Venta #10',5,10),(5,1,'Venta #11',6,1),(6,1,'Venta #11',7,4),(7,1,'Venta #11',8,7),(8,1,'Venta #12',9,10),(9,1,'Venta #13',10,8),(10,1,'Venta #14',11,13),(11,1,'Venta #15',12,1),(12,1,'Venta #15',12,4),(13,1,'Venta #15',12,5),(14,1,'Venta #16',13,3),(15,1,'Venta #16',13,4);
/*!40000 ALTER TABLE `detalle_egreso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalle_ingreso`
--

DROP TABLE IF EXISTS `detalle_ingreso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalle_ingreso` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Cantidad` int DEFAULT NULL,
  `PrecioCompra` decimal(65,30) NOT NULL,
  `Observacion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Id_Ingreso` int DEFAULT NULL,
  `Id_ArticuloAlmacen` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Id_Ingreso` (`Id_Ingreso`),
  KEY `Id_ArticuloAlmacen` (`Id_ArticuloAlmacen`),
  CONSTRAINT `detalle_ingreso_ibfk_1` FOREIGN KEY (`Id_Ingreso`) REFERENCES `nota_ingreso` (`Id`),
  CONSTRAINT `detalle_ingreso_ibfk_2` FOREIGN KEY (`Id_ArticuloAlmacen`) REFERENCES `articulo_almacen` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalle_ingreso`
--

LOCK TABLES `detalle_ingreso` WRITE;
/*!40000 ALTER TABLE `detalle_ingreso` DISABLE KEYS */;
INSERT INTO `detalle_ingreso` VALUES (1,1,1.000000000000000000000000000000,'',1,19),(2,1,0.100000000000000000000000000000,'',2,21),(3,2,0.000000000000000000000000000000,'',3,21),(4,10,5.500000000000000000000000000000,'Compra #1',8,21),(5,100,0.020000000000000000000000000000,'Compra #2',9,9),(6,10,0.020000000000000000000000000000,'Compra #3',10,9),(7,100,0.050000000000000000000000000000,'Compra #4',11,1);
/*!40000 ALTER TABLE `detalle_ingreso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalle_traspaso`
--

DROP TABLE IF EXISTS `detalle_traspaso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalle_traspaso` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Cantidad` int DEFAULT NULL,
  `Id_Traspaso` int DEFAULT NULL,
  `Id_ArticuloAlmacen` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Id_Traspaso` (`Id_Traspaso`),
  KEY `Id_ArticuloAlmacen` (`Id_ArticuloAlmacen`),
  CONSTRAINT `detalle_traspaso_ibfk_1` FOREIGN KEY (`Id_Traspaso`) REFERENCES `traspaso` (`Id`),
  CONSTRAINT `detalle_traspaso_ibfk_2` FOREIGN KEY (`Id_ArticuloAlmacen`) REFERENCES `articulo_almacen` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalle_traspaso`
--

LOCK TABLES `detalle_traspaso` WRITE;
/*!40000 ALTER TABLE `detalle_traspaso` DISABLE KEYS */;
INSERT INTO `detalle_traspaso` VALUES (1,5,1,14),(2,10,1,7),(3,6,2,21),(4,1,3,21);
/*!40000 ALTER TABLE `detalle_traspaso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ges_precio`
--

DROP TABLE IF EXISTS `ges_precio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ges_precio` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Id_Articulo` int NOT NULL,
  `PrecioCompra` decimal(65,30) NOT NULL,
  `PrecioVenta` decimal(65,30) NOT NULL,
  `Fecha` datetime(6) NOT NULL,
  `MetodoInventario` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_ges_precio_Id_Articulo` (`Id_Articulo`),
  CONSTRAINT `FK_ges_precio_articulo_Id_Articulo` FOREIGN KEY (`Id_Articulo`) REFERENCES `articulo` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ges_precio`
--

LOCK TABLES `ges_precio` WRITE;
/*!40000 ALTER TABLE `ges_precio` DISABLE KEYS */;
INSERT INTO `ges_precio` VALUES (5,1,0.050000000000000000000000000000,0.370000000000000000000000000000,'2026-06-20 00:00:00.000000','PROMEDIO'),(6,1,0.050000000000000000000000000000,0.260000000000000000000000000000,'2026-06-20 00:00:00.000000','PROMEDIO'),(7,9,0.020000000000000000000000000000,0.034771573604060913705583756300,'2026-06-26 00:00:00.000000','PROMEDIO'),(8,9,0.020000000000000000000000000000,0.020000000000000000000000000000,'2026-06-26 00:00:00.000000','PROMEDIO'),(9,1,0.050000000000000000000000000000,0.050000000000000000000000000000,'2026-06-26 00:00:00.000000','PROMEDIO');
/*!40000 ALTER TABLE `ges_precio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `marca`
--

DROP TABLE IF EXISTS `marca`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marca` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Descripcion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `marca`
--

LOCK TABLES `marca` WRITE;
/*!40000 ALTER TABLE `marca` DISABLE KEYS */;
INSERT INTO `marca` VALUES (1,'Samsung',NULL,NULL),(2,'LG',NULL,NULL),(3,'Sony',NULL,NULL),(4,'Panasonic',NULL,NULL),(5,'Philips',NULL,NULL),(6,'Bosch',NULL,NULL),(7,'TP-Link',NULL,NULL),(8,'Genrico',NULL,NULL);
/*!40000 ALTER TABLE `marca` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nota_egreso`
--

DROP TABLE IF EXISTS `nota_egreso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nota_egreso` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Fecha` datetime(6) NOT NULL,
  `Glosa` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Motivo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Id_Usuario` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nota_egreso`
--

LOCK TABLES `nota_egreso` WRITE;
/*!40000 ALTER TABLE `nota_egreso` DISABLE KEYS */;
INSERT INTO `nota_egreso` VALUES (1,'2026-06-11 00:00:00.000000','articulo dado de baja por que esta dañado','articulo defectuoso','activo',0),(2,'2026-06-11 00:00:00.000000','producto dañado','merma o deterioro','activo',0),(3,'2026-06-14 00:00:00.000000','Egreso por venta #8','venta','activo',8),(4,'2026-06-14 00:00:00.000000','Egreso por venta #9','venta','activo',8),(5,'2026-06-18 00:00:00.000000','Egreso por venta #10','venta','activo',8),(6,'2026-06-18 00:00:00.000000','Egreso por venta #11','venta','activo',8),(7,'2026-06-18 00:00:00.000000','Egreso por venta #11','venta','activo',8),(8,'2026-06-18 00:00:00.000000','Egreso por venta #11','venta','activo',8),(9,'2026-06-18 00:00:00.000000','Egreso por venta #12','venta','activo',8),(10,'2026-06-18 00:00:00.000000','Egreso por venta #13','venta','activo',8),(11,'2026-06-18 00:00:00.000000','Egreso por venta #14','venta','activo',8),(12,'2026-06-19 00:00:00.000000','Egreso por venta #15','venta','activo',8),(13,'2026-06-19 00:00:00.000000','Egreso por venta #16','venta','activo',8);
/*!40000 ALTER TABLE `nota_egreso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nota_ingreso`
--

DROP TABLE IF EXISTS `nota_ingreso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nota_ingreso` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Fecha` datetime(6) NOT NULL,
  `Glosa` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Motivo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Id_Usuario` int DEFAULT NULL,
  `SoloRegistro` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nota_ingreso`
--

LOCK TABLES `nota_ingreso` WRITE;
/*!40000 ALTER TABLE `nota_ingreso` DISABLE KEYS */;
INSERT INTO `nota_ingreso` VALUES (1,'2026-06-10 00:00:00.000000','compra de  repuestos ','compra','activo',0,0),(2,'2026-06-11 00:00:00.000000','compra','compra','activo',0,0),(3,'2026-06-11 00:00:00.000000','compra','compra','activo',0,0),(4,'2026-06-18 00:00:00.000000','Devolución por cancelación de venta #2','devolucion','activo',1,0),(5,'2026-06-18 00:00:00.000000','Devolución por cancelación de venta #3','devolucion','activo',8,0),(6,'2026-06-18 00:00:00.000000','Devolución por cancelación de venta #4','devolucion','activo',8,0),(7,'2026-06-18 00:00:00.000000','Devolución por cancelación de venta #1','devolucion','activo',1,0),(8,'2026-06-19 00:00:00.000000','Ingreso por compra #1','compra','activo',1,0),(9,'2026-06-26 00:00:00.000000','Ingreso por compra #2','compra','activo',8,1),(10,'2026-06-26 00:00:00.000000','Ingreso por compra #3','compra','activo',8,1),(11,'2026-06-26 00:00:00.000000','Ingreso por compra #4','compra','activo',8,1);
/*!40000 ALTER TABLE `nota_ingreso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `traspaso`
--

DROP TABLE IF EXISTS `traspaso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `traspaso` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Fecha` datetime(6) NOT NULL,
  `Glosa` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Id_Usuario` int DEFAULT NULL,
  `Id_AlmacenOrigen` int DEFAULT NULL,
  `Id_AlmacenDestino` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Id_AlmacenOrigen` (`Id_AlmacenOrigen`),
  KEY `Id_AlmacenDestino` (`Id_AlmacenDestino`),
  CONSTRAINT `traspaso_ibfk_1` FOREIGN KEY (`Id_AlmacenOrigen`) REFERENCES `almacen` (`Id`),
  CONSTRAINT `traspaso_ibfk_2` FOREIGN KEY (`Id_AlmacenDestino`) REFERENCES `almacen` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `traspaso`
--

LOCK TABLES `traspaso` WRITE;
/*!40000 ALTER TABLE `traspaso` DISABLE KEYS */;
INSERT INTO `traspaso` VALUES (1,'2026-06-11 00:00:00.000000','abastecimiento de almacen','activo',0,1,2),(2,'2026-06-12 00:00:00.000000','abasteciomiento','activo',0,1,2),(3,'2026-06-12 00:00:00.000000','abastecimiento','activo',0,1,2);
/*!40000 ALTER TABLE `traspaso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unidad_medida`
--

DROP TABLE IF EXISTS `unidad_medida`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `unidad_medida` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Abreviatura` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unidad_medida`
--

LOCK TABLES `unidad_medida` WRITE;
/*!40000 ALTER TABLE `unidad_medida` DISABLE KEYS */;
INSERT INTO `unidad_medida` VALUES (1,'Unidad',NULL,NULL),(2,'Par',NULL,NULL),(3,'Caja',NULL,NULL),(4,'Metro',NULL,NULL);
/*!40000 ALTER TABLE `unidad_medida` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'db_inventario'
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

-- Dump completed on 2026-07-05 13:47:53
