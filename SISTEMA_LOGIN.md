# Sistema de Login de Usuarios - Almacén

## Resumen
Se ha implementado un sistema completo de autenticación y gestión de usuarios para la aplicación de gestión de almacén. El sistema incluye login, gestión de usuarios con diferentes roles y controles de acceso.

## Características Implementadas

### 1. Modelo de Usuario (`Models/Usuario.cs`)
- **Propiedades del Usuario:**
  - ID único
  - Nombre de usuario
  - Contraseña
  - Nombre y apellido completos
  - Email
  - Tipo de usuario (Administrador, Empleado, Vendedor)
  - Estado activo/inactivo
  - Fecha de creación
  - Último acceso

- **Tipos de Usuario:**
  - **Administrador**: Acceso completo al sistema, incluyendo gestión de usuarios
  - **Empleado**: Acceso a operaciones generales del almacén
  - **Vendedor**: Enfocado en ventas y atención al cliente

### 2. Formulario de Login (`Forms/LoginForm.cs`)
- **Funcionalidades:**
  - Autenticación de usuario con nombre de usuario y contraseña
  - Validación de credenciales
  - Actualización de último acceso
  - Interfaz intuitiva y profesional
  - Manejo de errores de autenticación

- **Credenciales por Defecto:**
  - **Administrador**: usuario: `admin`, contraseña: `admin123`
  - **Empleado**: usuario: `empleado`, contraseña: `emp123`
  - **Vendedor**: usuario: `vendedor`, contraseña: `vend123`

### 3. Gestión de Usuarios (`Forms/UsuariosForm.cs`)
- **Solo disponible para Administradores**
- **Funcionalidades:**
  - Listar todos los usuarios del sistema
  - Buscar usuarios por nombre o usuario
  - Agregar nuevos usuarios
  - Editar usuarios existentes
  - Activar/desactivar usuarios
  - Eliminar usuarios (con confirmación)

### 4. Formulario de Usuario (`Forms/UsuarioForm.cs`)
- **Funcionalidades:**
  - Crear nuevos usuarios
  - Editar usuarios existentes
  - Validación de datos de entrada
  - Verificación de contraseñas
  - Validación de email
  - Verificación de unicidad de nombre de usuario

### 5. Modificaciones al Sistema Principal

#### DataManager (`Data/DataManager.cs`)
- **Nuevos Métodos:**
  - `AutenticarUsuario()`: Valida credenciales de login
  - `AgregarUsuario()`: Agrega nuevos usuarios al sistema
  - `ActualizarUsuario()`: Actualiza información de usuarios
  - `EliminarUsuario()`: Elimina usuarios del sistema
  - `BuscarUsuarios()`: Busca usuarios por texto
  - `ExisteNombreUsuario()`: Verifica unicidad de nombres de usuario

#### Program.cs
- **Modificaciones:**
  - Muestra el formulario de login antes del formulario principal
  - Solo permite acceso al sistema con autenticación exitosa
  - Pasa el usuario autenticado al formulario principal

#### MainForm.cs
- **Nuevas Características:**
  - Muestra información del usuario logueado
  - Botón de cerrar sesión (logout)
  - Botón de gestión de usuarios (solo para administradores)
  - Control de acceso basado en roles

## Flujo de Uso

### 1. Inicio de la Aplicación
1. Al ejecutar la aplicación, se muestra el formulario de login
2. El usuario ingresa sus credenciales
3. El sistema valida las credenciales
4. Si son correctas, se abre el formulario principal
5. Si son incorrectas, se muestra un mensaje de error

### 2. Gestión de Usuarios (Solo Administradores)
1. Desde el menú principal, hacer clic en "Gestión de Usuarios"
2. Se abre la ventana de gestión de usuarios
3. Opciones disponibles:
   - Ver lista de usuarios
   - Buscar usuarios
   - Agregar nuevo usuario
   - Editar usuario existente
   - Eliminar usuario

### 3. Cerrar Sesión
1. Hacer clic en "Cerrar Sesión" en el formulario principal
2. Se cierra la sesión actual
3. Se vuelve al formulario de login

## Seguridad Implementada

### 1. Autenticación
- Validación de credenciales contra la base de datos
- Contraseñas almacenadas como texto plano (para simplicidad de demostración)
- Control de usuarios activos/inactivos

### 2. Control de Acceso
- Verificación de roles de usuario
- Restricción de funciones administrativas
- Validación de permisos antes de mostrar opciones

### 3. Validaciones
- Verificación de unicidad de nombres de usuario
- Validación de formato de email
- Confirmación de contraseñas
- Validación de campos obligatorios

## Instalación y Ejecución

### Compilación
```bash
cd Almacen
mcs -target:winexe -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:Almacen.exe *.cs Models/*.cs Forms/*.cs Data/*.cs Properties/*.cs
```

### Ejecución
```bash
mono Almacen.exe
```

## Archivos Modificados/Creados

### Archivos Nuevos:
- `Models/Usuario.cs` - Modelo de datos de usuario
- `Forms/LoginForm.cs` - Formulario de inicio de sesión
- `Forms/UsuariosForm.cs` - Gestión de usuarios
- `Forms/UsuarioForm.cs` - Formulario individual de usuario

### Archivos Modificados:
- `Program.cs` - Punto de entrada con login
- `MainForm.cs` - Formulario principal con información de usuario
- `Data/DataManager.cs` - Gestión de datos de usuarios

## Mejoras Futuras Sugeridas

### 1. Seguridad
- Implementar hash de contraseñas (SHA256, bcrypt)
- Agregar políticas de contraseñas
- Implementar bloqueo por intentos fallidos
- Agregar logs de actividad de usuarios

### 2. Funcionalidades
- Recuperación de contraseñas
- Cambio de contraseña por el usuario
- Perfiles de usuario más detallados
- Permisos granulares por módulo

### 3. Persistencia
- Migrar a base de datos SQL Server/SQLite
- Implementar Entity Framework
- Backup automático de usuarios

### 4. Interfaz
- Mejorar diseño visual
- Agregar temas
- Implementar responsive design
- Agregar iconos y mejores gráficos

## Conclusión

El sistema de login implementado proporciona una base sólida para la autenticación y gestión de usuarios en la aplicación de almacén. Incluye las funcionalidades esenciales para un sistema de usuarios básico pero funcional, con controles de acceso por roles y una interfaz intuitiva.

El sistema está listo para uso inmediato y puede ser extendido fácilmente con las mejoras sugeridas según las necesidades específicas del negocio.