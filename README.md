# 🏭 ProdLogApp

**ProdLogApp** es una aplicación de escritorio desarrollada en **C# (.NET WPF)** para la gestión de **producción industrial** en entornos locales (*on-premise*).  
Permite registrar partes diarios de producción, gestionar operarios, productos, categorías y puestos de trabajo, con soporte multiusuario y perfiles diferenciados (Gerente / Operario).

---

## ⚙️ Características principales

- **Login** con control de roles (Gerente / Operario)
- **Gestión completa (ABM)** de:
  - Usuarios  
  - Puestos  
  - Categorías  
  - Productos  
- **Registro de Producción:**
  - Ingreso de partes diarios por operario
  - Validaciones de horario y cantidad
  - Cálculo automático de duración
- **Filtros y Prompts:**
  - Ventanas emergentes para seleccionar productos, puestos y categorías
  - Búsqueda por nombre, código o categoría
- **Despliegue local (on-premise):**
  - Base de datos MySQL alojada en una PC de la red
  - Aplicación cliente conectada a través de la LAN

---

## 🧩 Arquitectura

- **Frontend / UI:** WPF (.NET 6 o superior)
- **Patrón:** MVP (Model–View–Presenter)
- **Base de datos:** MySQL 8.0
- **Conector:** [MySqlConnector](https://mysqlconnector.net/)
- **Estructura de capas:**
  - `Models` → entidades de dominio  
  - `Interfaces` → contratos de vistas  
  - `Presenters` → lógica de presentación  
  - `Servicios` → acceso a datos  
  - `Views` → interfaz gráfica (ventanas WPF)

📁 **Estructura del proyecto:**
ProdLogApp/
├── Interfaces/ # Contratos de vistas (IMenuGerenteVista, etc.)
├── Models/ # Entidades de dominio (Usuario, Producto, etc.)
├── Presenters/ # Lógica de presentación (MVP)
│ └── Prompts_PopUps/ # Presenters de ventanas de selección
├── Servicios/ # Acceso a datos (MySQL)
├── Views/ # Ventanas WPF (.xaml y .xaml.cs)
├── App.config # Configuración de conexión
└── README.md

pgsql
Copy code

---

## 🗄️ Base de datos

**Nombre:** `ProdLog_BD`  
**Motor:** MySQL 8.0+  
**Codificación:** `utf8mb4_0900_ai_ci`

📘 **Tablas principales:**
| Tabla | Descripción |
|-------|--------------|
| `Usuario` | Usuarios del sistema (gerente / operario) |
| `Categoria` | Clasificación de productos |
| `Producto` | Artículos o piezas fabricadas |
| `Puesto` | Puestos o estaciones de trabajo |
| `Parte` | Cabecera de parte diario |
| `Producciones` | Detalle de producción (horas y cantidad) |

🧱 **Script DDL principal:**  
(se puede colocar en `/Docs/ProdLog_BD.sql`)

```sql
CREATE DATABASE IF NOT EXISTS `ProdLog_BD`
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE `ProdLog_BD`;

CREATE TABLE `Categoria` (
  `CategoriaId` INT NOT NULL AUTO_INCREMENT,
  `CategoriaNombre` VARCHAR(100) NOT NULL,
  `Activo` TINYINT(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`CategoriaId`),
  UNIQUE KEY `UQ_CategoriaNombre` (`CategoriaNombre`)
);

CREATE TABLE `Usuario` (
  `UsId` INT NOT NULL AUTO_INCREMENT,
  `UsNombre` VARCHAR(100) NOT NULL,
  `UsDNI` CHAR(8) NOT NULL,
  `UsFechaIngreso` DATE DEFAULT CURRENT_DATE,
  `UsGerente` TINYINT(1) NOT NULL DEFAULT 0,
  `UsPass` VARCHAR(255),
  `UsActivo` TINYINT(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`UsId`),
  UNIQUE KEY `UQ_UsDNI` (`UsDNI`)
);

CREATE TABLE `Puesto` (
  `PuestoId` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(100) NOT NULL,
  `Activo` TINYINT(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`PuestoId`),
  UNIQUE KEY `UQ_PuestoNombre` (`Nombre`)
);

CREATE TABLE `Producto` (
  `ProductoId` INT NOT NULL AUTO_INCREMENT,
  `ProductoNombre` VARCHAR(150) NOT NULL,
  `CategoriaId` INT NOT NULL,
  `Activo` TINYINT(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`ProductoId`),
  UNIQUE KEY `UQ_ProductoNombre` (`ProductoNombre`),
  CONSTRAINT `FK_Producto_Categoria`
    FOREIGN KEY (`CategoriaId`) REFERENCES `Categoria` (`CategoriaId`)
    ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE `Parte` (
  `Parte_Id` INT NOT NULL AUTO_INCREMENT,
  `Parte_Fecha` DATE NOT NULL DEFAULT CURRENT_DATE,
  `Usuario_Id` INT NOT NULL,
  PRIMARY KEY (`Parte_Id`),
  CONSTRAINT `FK_Parte_Usuario`
    FOREIGN KEY (`Usuario_Id`) REFERENCES `Usuario` (`UsId`)
    ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE `Producciones` (
  `Produccion_Id` INT NOT NULL AUTO_INCREMENT,
  `Produccion_HoraInicio` TIME NOT NULL,
  `Produccion_HoraFin` TIME NOT NULL,
  `Produccion_Cantidad` INT NOT NULL DEFAULT 0,
  `Producto_Id` INT NOT NULL,
  `Puesto_Id` INT NOT NULL,
  `Parte_Id` INT NOT NULL,
  PRIMARY KEY (`Produccion_Id`),
  CONSTRAINT `FK_Producciones_Producto`
    FOREIGN KEY (`Producto_Id`) REFERENCES `Producto` (`ProductoId`)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Producciones_Puesto`
    FOREIGN KEY (`Puesto_Id`) REFERENCES `Puesto` (`PuestoId`)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Producciones_Parte`
    FOREIGN KEY (`Parte_Id`) REFERENCES `Parte` (`Parte_Id`)
    ON DELETE CASCADE ON UPDATE CASCADE
);
🧰 Instalación y configuración
1️⃣ Requisitos
Windows 10 / 11

.NET Desktop Runtime 6.0 o superior

MySQL Server 8.0

2️⃣ Configurar base de datos
Ejecutar el script ProdLog_BD.sql desde MySQL Workbench o terminal:

bash
Copy code
mysql -u root -p < ProdLog_BD.sql
Crear usuario de conexión:

sql
Copy code
CREATE USER 'prodlog_user'@'%' IDENTIFIED BY 'Madersa123';
GRANT SELECT, INSERT, UPDATE, DELETE ON ProdLog_BD.* TO 'prodlog_user'@'%';
FLUSH PRIVILEGES;
3️⃣ Configurar la conexión en clientes
Crear archivo:

makefile
Copy code
C:\Users\<usuario>\AppData\Roaming\ProdLogApp\connection.txt
con el contenido:

ini
Copy code
Server=192.168.0.50;Port=3306;Database=ProdLog_BD;Uid=prodlog_user;Pwd=Madersa123;SslMode=None;
⚠️ Reemplazar 192.168.0.50 por la IP del servidor MySQL (debe ser fija o reservada).

🚀 Ejecución
Compilar el proyecto con Visual Studio 2022 o superior.

Ejecutar ProdLogApp.exe.

Iniciar sesión con un usuario activo.

👤 Roles del sistema
Rol	Descripción
Gerente	Administra usuarios, productos, categorías, puestos y controla la producción.
Operario	Carga partes de producción diarias desde su puesto asignado.

🧪 Validaciones implementadas
Verificación de DNI único en alta de usuarios.

Validaciones de campos obligatorios (nombre, DNI, horarios, cantidades).

Control de inicio/fin de jornada (HoraInicio < HoraFin).

Restricción de eliminación mediante flag Activo en entidades maestras.

Hash de contraseñas en el campo UsPass.

🧱 Patrón y estructura
El sistema sigue el patrón MVP (Model–View–Presenter):

Model: datos de negocio (Models)

View: interfaz visual (WPF .xaml.cs)

Presenter: intermediario entre vista y servicios, sin acceso directo a UI o BD.

🛠️ Próximas mejoras
Panel de reportes y dashboard con gráficos (por fecha, producto, operario).

Exportación de datos a Excel / PDF.

Control de auditoría y bitácora de actividad.

Configuración centralizada de la conexión desde el menú Gerente.

👨‍💻 Autor
ProdLogApp
Proyecto académico desarrollado por [Tobias Engler, Zhoryx]

Año: 2025
