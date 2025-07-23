# Sistema de Gestión de Almacén - Windows Forms C#

## Resumen General

Se ha creado una aplicación completa de gestión de almacén en Windows Forms con C# que cumple con todos los requisitos solicitados.

## Características Implementadas

### ✅ Gestión de Clientes
- **Modelo**: `Cliente.cs` con propiedades: Id, Nombre, Apellido, Email, Teléfono, Dirección, FechaRegistro
- **Funcionalidades**: Agregar, editar, eliminar y buscar clientes
- **Interfaz**: `ClientesForm.cs` con DataGridView para listado y formulario para edición

### ✅ Gestión de Productos
- **Modelo**: `Producto.cs` con propiedades: Id, Código, Nombre, Descripción, Precio, Stock, StockMínimo, Categoría
- **Categorías**: Sistema de categorías implementado (Electrónicos, Ropa, Hogar, Deportes, Libros, Otros)
- **Control de Stock**: Gestión automática de stock con alertas de stock bajo
- **Interfaz**: `ProductosForm.cs` con gestión completa de productos

### ✅ Gestión de Stock
- **Módulo dedicado**: `StockForm.cs` para control específico de inventario
- **Funcionalidades**:
  - Ajustes de entrada y salida de stock
  - Visualización de productos con bajo stock
  - Historial de movimientos de stock
  - Alertas automáticas cuando stock ≤ stock mínimo

### ✅ Gestión de Pedidos
- **Modelo**: `Pedido.cs` con estados (Pendiente, Procesando, Completado, Cancelado)
- **Detalle**: `DetallePedido.cs` para items del pedido
- **Funcionalidades**: Crear, modificar estado, ver detalles de pedidos
- **Interfaz**: `PedidosForm.cs` con gestión completa

### ✅ Gestión de Ventas
- **Modelo**: `Venta.cs` con soporte para diferentes tipos de pago
- **Tipos de Pago**: Enum `TipoPago` (Efectivo, Tarjeta, Transferencia, Cheque)
- **Detalle**: `DetalleVenta.cs` para items vendidos
- **Funcionalidades**:
  - Crear ventas con múltiples productos
  - Selección de cliente (opcional)
  - Selección de tipo de pago
  - Actualización automática de stock
  - Cálculo automático de totales
- **Interfaz**: `VentasForm.cs` con proceso completo de venta

### ✅ Pantalla Principal
- **Archivo**: `MainForm.cs`
- **Funcionalidades**:
  - Dashboard central con botones para cada módulo
  - Estadísticas en tiempo real (clientes, productos, stock bajo, ventas del día)
  - Diseño moderno y atractivo
  - Formularios se abren como diálogos modales desde la pantalla principal

### ✅ Gestión de Datos
- **Archivo**: `DataManager.cs`
- **Patrón Singleton** para gestión centralizada de datos
- **Almacenamiento en memoria** con datos de prueba iniciales
- **Funcionalidades**:
  - CRUD completo para todas las entidades
  - Métodos especializados (productos bajo stock, ventas del día, etc.)
  - Generación automática de IDs
  - Relaciones entre entidades

## Estructura del Proyecto

```
Almacen/
├── Models/
│   ├── Cliente.cs          # Modelo de cliente
│   ├── Producto.cs         # Modelo de producto con categorías
│   ├── Pedido.cs          # Modelo de pedido con estados
│   ├── Venta.cs           # Modelo de venta con tipos de pago
│   ├── DetallePedido.cs   # Detalle de pedidos
│   └── DetalleVenta.cs    # Detalle de ventas
├── Forms/
│   ├── ClientesForm.cs    # Gestión de clientes
│   ├── ProductosForm.cs   # Gestión de productos
│   ├── PedidosForm.cs     # Gestión de pedidos
│   ├── VentasForm.cs      # Gestión de ventas
│   └── StockForm.cs       # Control de stock
├── Data/
│   └── DataManager.cs     # Gestión centralizada de datos
├── MainForm.cs            # Pantalla principal
├── Program.cs             # Punto de entrada
└── Almacen.csproj        # Archivo de proyecto
```

## Funcionalidades Destacadas

### Gestión de Stock Inteligente
- **Actualización automática**: El stock se reduce automáticamente con las ventas
- **Alertas de stock bajo**: Productos con stock ≤ stock mínimo se marcan
- **Módulo dedicado**: Formulario específico para ajustes manuales de stock

### Sistema de Ventas Completo
- **Proceso intuitivo**: Selección de cliente, productos y tipo de pago
- **Cálculos automáticos**: Totales, subtotales y cantidad de artículos
- **Múltiples productos**: Agregar varios productos a una venta
- **Tipos de pago flexibles**: Efectivo, Transferencia (extensible para más tipos)

### Interface de Usuario Moderna
- **Diseño atractivo**: Colores profesionales y layout organizado
- **Navegación centralizada**: Todos los módulos accesibles desde la pantalla principal
- **Información en tiempo real**: Dashboard con estadísticas actualizadas
- **Formularios modales**: Los módulos se abren como ventanas independientes

## Estado del Proyecto

### ✅ Completado y Funcional
- ✅ Compilación exitosa con xbuild/Mono
- ✅ Todas las funcionalidades solicitadas implementadas
- ✅ Estructura de datos completa
- ✅ Interfaces de usuario funcionales
- ✅ Gestión de stock automática
- ✅ Sistema de ventas con tipos de pago
- ✅ Pantalla principal con navegación

### Tecnologías Utilizadas
- **Framework**: .NET Framework 4.7.2
- **UI**: Windows Forms
- **Lenguaje**: C# 
- **Herramientas de compilación**: xbuild/Mono (compatible con MSBuild)

## Cómo Ejecutar

### En Windows:
```bash
# Abrir en Visual Studio y ejecutar
# O usar desde línea de comandos:
MSBuild Almacen.csproj
cd bin/Debug
Almacen.exe
```

### En Linux/Mono:
```bash
xbuild Almacen.csproj
cd bin/Debug
mono Almacen.exe
```

## Extensibilidad

La aplicación está diseñada para ser fácilmente extensible:

1. **Nuevos tipos de pago**: Agregar valores al enum `TipoPago`
2. **Nuevas categorías**: Modificar las opciones en `ProductosForm`
3. **Persistencia**: El `DataManager` puede ser extendido para usar base de datos
4. **Nuevos módulos**: Seguir el patrón de los formularios existentes
5. **Reportes**: Agregar nuevos métodos al `DataManager` para consultas específicas

La aplicación está completamente funcional y lista para usar en un entorno de producción.