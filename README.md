# Sistema de Gestión de Almacén

Una aplicación completa de gestión de almacén desarrollada en C# con Windows Forms que permite administrar clientes, productos, pedidos, ventas y control de stock. **Ahora incluye sistema de autenticación de usuarios con diferentes roles y permisos.**

## Características

### 🔐 **Sistema de Usuarios y Autenticación** *(NUEVO)*
- Login seguro con usuario y contraseña
- Tres tipos de usuario: Administrador, Empleado, Vendedor
- Control de acceso basado en roles
- Gestión completa de usuarios (solo para administradores)
- Seguimiento de último acceso
- Activación/desactivación de usuarios

### 🏢 **Gestión de Clientes**
- Agregar, editar y eliminar clientes
- Búsqueda de clientes por nombre, apellido o email
- Información completa: nombre, apellido, email, teléfono, dirección
- Fecha de registro automática

### 📦 **Gestión de Productos**
- Administración completa de productos
- Códigos únicos de producto
- Control de categorías
- Manejo de precios y stock
- Alertas de stock bajo
- Búsqueda por código, nombre o descripción

### 📋 **Gestión de Pedidos**
- Crear pedidos para clientes
- Estados de pedido: Pendiente, Procesando, Completado, Cancelado
- Agregar múltiples productos por pedido
- Cálculo automático de totales
- Cambio de estado de pedidos
- Actualización automática de stock al completar pedidos

### 💰 **Gestión de Ventas**
- Registro de ventas directas
- Soporte para clientes ocasionales
- Múltiples formas de pago: Efectivo, Tarjeta, Transferencia, Cheque
- Control de stock en tiempo real
- Cálculo automático de totales

### 📊 **Control de Stock**
- Vista completa del inventario
- Ajustes de stock (entrada/salida)
- Alertas de productos con bajo stock
- Estadísticas del inventario
- Valor total del inventario
- Reportes detallados

## Estructura del Proyecto

```
Almacen/
├── Models/                 # Modelos de datos
│   ├── Cliente.cs
│   ├── Producto.cs
│   ├── Pedido.cs
│   ├── Venta.cs
│   ├── DetallePedido.cs
│   └── DetalleVenta.cs
├── Data/                   # Capa de acceso a datos
│   └── DataManager.cs
├── Forms/                  # Formularios de la aplicación
│   ├── ClientesForm.cs
│   ├── ProductosForm.cs
│   ├── PedidosForm.cs
│   ├── VentasForm.cs
│   └── StockForm.cs
├── MainForm.cs            # Formulario principal
└── Program.cs             # Punto de entrada
```

## Requisitos del Sistema

- **Sistema Operativo**: Windows 7 o superior
- **.NET Framework**: 4.7.2 o superior
- **IDE Recomendado**: Visual Studio 2017 o superior

## Instalación y Configuración

### Opción 1: Visual Studio

1. **Abrir el proyecto**:
   ```
   Abrir Visual Studio → File → Open → Project/Solution
   Seleccionar: Almacen.sln
   ```

2. **Restaurar dependencias** (si es necesario):
   ```
   Build → Rebuild Solution
   ```

3. **Ejecutar la aplicación**:
   ```
   Presionar F5 o Debug → Start Debugging
   ```

### Opción 2: Línea de comandos (MSBuild)

1. **Compilar el proyecto**:
   ```cmd
   msbuild Almacen.sln /p:Configuration=Release
   ```

2. **Ejecutar la aplicación**:
   ```cmd
   cd Almacen\bin\Release
   Almacen.exe
   ```

### Opción 3: Mono (Linux/macOS)

1. **Instalar Mono**:
   ```bash
   # Ubuntu/Debian
   sudo apt-get install mono-complete
   
   # macOS
   brew install mono
   ```

2. **Compilar**:
   ```bash
   msbuild Almacen.sln
   ```

3. **Ejecutar**:
   ```bash
   mono Almacen/bin/Debug/Almacen.exe
   ```

## Uso de la Aplicación

### Login Inicial

Al iniciar la aplicación, primero se mostrará la pantalla de login. Use las siguientes credenciales por defecto:

- **Administrador**: usuario: `admin`, contraseña: `admin123`
- **Empleado**: usuario: `empleado`, contraseña: `emp123`  
- **Vendedor**: usuario: `vendedor`, contraseña: `vend123`

### Pantalla Principal

Después del login exitoso, verás el menú principal con 5 módulos:

- **GESTIÓN DE CLIENTES** (Azul)
- **GESTIÓN DE PRODUCTOS** (Verde)
- **GESTIÓN DE PEDIDOS** (Naranja)
- **GESTIÓN DE VENTAS** (Rojo)
- **CONTROL DE STOCK** (Morado)

### Flujo de Trabajo Recomendado

1. **Configuración inicial**:
   - Agregar clientes en el módulo de Clientes
   - Agregar productos en el módulo de Productos

2. **Operaciones diarias**:
   - Crear pedidos para clientes
   - Procesar ventas directas
   - Ajustar stock cuando sea necesario
   - Revisar alertas de bajo stock

3. **Seguimiento**:
   - Cambiar estados de pedidos
   - Generar reportes de inventario
   - Revisar estadísticas de ventas

## Características Técnicas

### Modelos de Datos

- **Cliente**: Información personal y de contacto
- **Producto**: Código, nombre, precio, stock, categoría
- **Pedido**: Encabezado con estado y detalles
- **Venta**: Transacción completa con detalles
- **DetallePedido/DetalleVenta**: Líneas de productos

### Gestión de Datos

- **Almacenamiento en memoria** para demostración
- **Singleton Pattern** para el DataManager
- **Datos de prueba** incluidos automáticamente
- **Validaciones** en todas las operaciones

### Interfaz de Usuario

- **Diseño intuitivo** con código de colores
- **Búsqueda en tiempo real** en todas las listas
- **Validaciones de entrada** de datos
- **Mensajes informativos** para el usuario

## Datos de Prueba

La aplicación incluye datos de ejemplo:

### Clientes
- Juan Pérez (juan.perez@email.com)
- María González (maria.gonzalez@email.com)

### Productos
- Laptop HP (PROD001) - $850.00
- Mouse Inalámbrico (PROD002) - $25.50
- Teclado Mecánico (PROD003) - $75.00 (Bajo stock)

## Extensibilidad

### Para agregar una base de datos:

1. Instalar Entity Framework:
   ```
   Install-Package EntityFramework
   ```

2. Crear contexto de datos:
   ```csharp
   public class AlmacenContext : DbContext
   {
       public DbSet<Cliente> Clientes { get; set; }
       public DbSet<Producto> Productos { get; set; }
       // ... otros DbSet
   }
   ```

3. Reemplazar DataManager con repositorios:
   ```csharp
   public class ClienteRepository
   {
       private AlmacenContext context;
       // Implementar métodos CRUD
   }
   ```

### Para agregar autenticación:

1. Crear modelo de Usuario
2. Agregar LoginForm
3. Implementar roles y permisos

## Licencia

Este proyecto es de código abierto y está disponible bajo la licencia MIT.

## Contacto

Para soporte o consultas, por favor contacta al desarrollador.

---

**¡Disfruta usando el Sistema de Gestión de Almacén!** 🚀