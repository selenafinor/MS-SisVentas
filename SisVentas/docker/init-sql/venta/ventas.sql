-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: localhost    Database: db_ventas
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
-- Table structure for table `cliente`
--

DROP TABLE IF EXISTS `cliente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cliente` (
  `id_cliente` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `paterno` varchar(100) DEFAULT NULL,
  `materno` varchar(100) DEFAULT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `correo` varchar(150) DEFAULT NULL,
  `nit` varchar(20) DEFAULT NULL,
  `direccion` varchar(200) DEFAULT NULL,
  `estado` varchar(20) NOT NULL DEFAULT 'activo',
  PRIMARY KEY (`id_cliente`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cliente`
--

LOCK TABLES `cliente` WRITE;
/*!40000 ALTER TABLE `cliente` DISABLE KEYS */;
INSERT INTO `cliente` VALUES (1,'Carlos','Mamani','Quispe','77712345','carlos.mamani@gmail.com','1234567','Av. América, Cochabamba','activo'),(2,'María','López','Flores','77723456','maria.lopez@gmail.com','2345678','Calle Sucre, Cochabamba','activo'),(3,'Juan','Pérez','Vargas','77734567','juan.perez@gmail.com','3456789','Av. Heroínas, Cochabamba','activo'),(4,'Ana','Flores','Condori','77745678','ana.flores@gmail.com','4567890','Av. Blanco Galindo, Cochabamba','activo'),(5,'Luis','Condori','Mamani','77756789','luis.condori@gmail.com','5678901','Calle Jordán, Cochabamba','activo'),(6,'Rosa','Vargas','López','77767890','rosa.vargas@gmail.com','6789012','Av. Ayacucho, Cochabamba','activo'),(7,'Pedro','Quispe','Flores','77778901','pedro.quispe@gmail.com','7890123','Calle Baptista, Cochabamba','activo'),(8,'Sandra','Torrez','Pérez','77789012','sandra.torrez@gmail.com','8901234','Av. Villarroel, Cochabamba','activo');
/*!40000 ALTER TABLE `cliente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalle_venta`
--

DROP TABLE IF EXISTS `detalle_venta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalle_venta` (
  `id_detalle_venta` int NOT NULL AUTO_INCREMENT,
  `cantidad` decimal(10,2) NOT NULL,
  `precio_uni` decimal(12,2) NOT NULL,
  `precio_subtotal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `id_producto` int DEFAULT NULL,
  `nombre_producto` varchar(200) DEFAULT NULL,
  `id_almacen` int DEFAULT NULL,
  `nombre_almacen` varchar(200) DEFAULT NULL,
  `id_venta` int NOT NULL,
  PRIMARY KEY (`id_detalle_venta`),
  KEY `fk_detalle_venta` (`id_venta`),
  CONSTRAINT `fk_detalle_venta` FOREIGN KEY (`id_venta`) REFERENCES `nota_venta` (`id_venta`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalle_venta`
--

LOCK TABLES `detalle_venta` WRITE;
/*!40000 ALTER TABLE `detalle_venta` DISABLE KEYS */;
INSERT INTO `detalle_venta` VALUES (1,1.00,0.75,0.75,1,'Televisor LED 32',1,'Almacen Central',8),(2,1.00,0.05,0.05,6,'Resistencia 1k Ohm',1,'Almacen Central',9),(3,1.00,0.05,0.05,10,'LED Rojo 5mm',1,'Almacen Central',10),(4,1.00,0.75,0.75,1,'Televisor LED 32\"',1,'Almacen Central',11),(5,1.00,0.65,0.65,4,'Reproductor DVD',1,'Almacen Central',11),(6,1.00,0.08,0.08,7,'Condensador 100uF',1,'Almacen Central',11),(7,1.00,0.05,0.05,10,'LED Rojo 5mm',1,'Almacen Central',12),(8,1.00,0.10,0.10,8,'Transistor BC547',1,'Almacen Central',13),(9,1.00,0.20,0.20,13,'Bateria 9V',1,'Almacen Central',14),(10,1.00,0.75,0.75,1,'Televisor LED 32\"',1,'Almacen Central',15),(11,1.00,0.65,0.65,4,'Reproductor DVD',1,'Almacen Central',15),(12,1.00,0.70,0.70,5,'Sistema de Sonido',1,'Almacen Central',15),(13,1.00,0.85,0.85,3,'Televisor LED 55\"',1,'Almacen Central',16),(14,1.00,0.65,0.65,4,'Reproductor DVD',1,'Almacen Central',16);
/*!40000 ALTER TABLE `detalle_venta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nota_venta`
--

DROP TABLE IF EXISTS `nota_venta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nota_venta` (
  `id_venta` int NOT NULL AUTO_INCREMENT,
  `fecha_venta` date NOT NULL,
  `hora_venta` time NOT NULL,
  `monto_total` decimal(12,2) NOT NULL DEFAULT '0.00',
  `glosa` text,
  `estado` varchar(20) NOT NULL DEFAULT 'activo',
  `tipo_pago` varchar(20) NOT NULL DEFAULT 'contado',
  `pago_confirmado` tinyint(1) NOT NULL DEFAULT '0',
  `id_transaccion_qr` varchar(200) DEFAULT NULL,
  `id_cliente` int DEFAULT NULL,
  `id_usuario` int DEFAULT NULL,
  PRIMARY KEY (`id_venta`),
  KEY `fk_venta_cliente` (`id_cliente`),
  CONSTRAINT `fk_venta_cliente` FOREIGN KEY (`id_cliente`) REFERENCES `cliente` (`id_cliente`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nota_venta`
--

LOCK TABLES `nota_venta` WRITE;
/*!40000 ALTER TABLE `nota_venta` DISABLE KEYS */;
INSERT INTO `nota_venta` VALUES (1,'2026-06-13','17:21:26',0.00,'Venta de prueba','cancelado','contado',0,NULL,1,1),(2,'2026-06-13','17:21:30',0.00,'Venta de prueba','cancelado','contado',0,NULL,1,1),(3,'2026-06-14','21:04:56',0.00,'','cancelado','contado',0,NULL,5,8),(4,'2026-06-14','21:06:46',0.00,'','cancelado','contado',0,NULL,4,8),(5,'2026-06-14','21:16:49',0.00,'','pagado','contado',1,NULL,4,1),(6,'2026-06-14','21:31:14',0.00,'','pagado','contado',1,NULL,5,1),(7,'2026-06-14','21:36:46',0.00,'','pagado','contado',1,NULL,1,8),(8,'2026-06-14','21:47:43',0.75,'','pagado','contado',1,NULL,7,8),(9,'2026-06-14','21:52:09',0.05,'','pagado','contado',1,NULL,5,8),(10,'2026-06-18','14:35:19',0.05,'','pagado','contado',1,NULL,4,8),(11,'2026-06-18','14:39:32',1.48,'','pagado','contado',1,NULL,5,8),(12,'2026-06-18','15:58:40',0.05,'','pagado','qr',1,'VENTA-12-20260618-155840-1781816748',8,8),(13,'2026-06-18','16:48:00',0.10,'','pagado','qr',1,'VENTA-13-20260618-164800',8,8),(14,'2026-06-18','18:11:08',0.20,'','pagado','qr',1,'VENTA-14-20260618-181108-1781820668',5,8),(15,'2026-06-19','16:55:22',2.10,'','pagado','contado',1,NULL,1,8),(16,'2026-06-19','17:19:55',1.50,'','pagado','contado',1,NULL,4,8);
/*!40000 ALTER TABLE `nota_venta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'db_ventas'
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

-- Dump completed on 2026-07-05 13:46:58
