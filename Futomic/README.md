# ⚽ Futomic – Gestión de Reservas de Campos y Equipos de Fútbol

**Futomic** es una aplicación web diseñada para facilitar la gestión de equipos de fútbol amateur, reservas de campos y seguimiento de resultados y ranking. La app permite a los usuarios:

- Crear y unirse a equipos.  
- Reservar campos disponibles.  
- Registrar partidos y resultados.  
- Visualizar estadísticas y ranking de manera profesional.  

---

## 🛠 Tecnologías utilizadas

- **Backend:** ASP.NET Core MVC 8  
- **Base de datos** Entity Framework Core con SQL Server  
- **Frontend:** Razor Views + Bootstrap 5  
- **Autenticación y Autorización:** ASP.NET Core Identity (modificado) 
- **Generación de PDFs:** QuestPDF  
- **Mapas y geolocalización:** Google Maps API  
- **Control de dependencias:** Dependency Injection  
- **Lenguaje:** C# 

---

## 🚀 Funcionalidades principales

### 1️⃣ Gestión de usuarios y equipos
- Registro y autenticación de usuarios.  
- Creación de equipos con información básica: nombre, logo, nivel y capitán.  
- Unirse a un equipo existente (un usuario solo puede pertenecer a un equipo).  
- Edición de equipos por parte del administrador (logo, nivel y nombre).  
- Eliminación de equipos incluso si tienen reservas o partidos registrados.  
- Vista **“Mi Equipo”** con información completa: jugadores, últimos partidos y estadísticas.

---

### 2️⃣ Sistema de ranking y resultados
- Visualización de ranking de equipos filtrable por nivel.  
- Registro de partidos entre equipos del mismo nivel.  
- Estadísticas de equipos: puntos, racha y últimos partidos.  
- Visualización de resultados de partidos.  
- Edición y eliminación de resultados por parte de administradores.

---

### 3️⃣ Gestión de campos y reservas
- Registro de campos con nombre, ubicación, email de contacto y PlusCode.  
- Búsqueda de campos por ubicación o PlusCode.  
(Faltan añadir un sistema robusto de buscar campos con el mapa directamente, he tenido que añadir a mano, con poner el pluscode, localidad/municipio o directamente Madrid aparecen)
- Visualización de mapas con marcadores usando **Google Maps API**.  
- Calendario de disponibilidad de campos con slots horarios.  
- Reserva de campos con selección de fecha, hora y duración.  
- Cálculo automático de precio según duración.  
- Selección de método de pago: Efectivo, Tarjeta o Bizum.  
- Confirmación de reservas y generación de **ticket PDF descargable** con QuestPDF.  
- Vista **“Mis Reservas”** para consultar reservas activas.

---

### 4️⃣ Seguridad y validaciones
- Control de acceso por roles: **Admin / Usuario**.  
- Validaciones de modelos para campos obligatorios y formato de datos.  
- Prevención de reservas duplicadas y validación de disponibilidad de horarios.  
- Validaciones de lógica de negocio:
  - Un usuario solo puede pertenecer a un equipo.  
  - No se pueden registrar partidos entre niveles distintos.  
  - Un usuario solo puede reservar campos si pertenece a un equipo.

---

## 📌 Notas adicionales
- Los administradores pueden gestionar equipos, partidos y rankings desde vistas protegidas por roles.
- Los horarios disponibles se calculan dinámicamente según reservas existentes y horario de apertura del campo.

---

## 🔮 Mejoras futuras posibles
- Integrar **notificaciones por email** para confirmación de reservas.  
- Implementar **historial completo de partidos y reservas canceladas**.  
- Sistema de **chat interno** entre miembros del equipo y administradores de campo.  
- Optimización de **búsquedas y mapas** para grandes cantidades de campos. 
- Gestión de campos por usuarios administradores propios.
- Implementar Api y Tests

---

## 👥 Usuarios de prueba
- **Usuario:** sergio@gmail.com | Contraseña: Admin123!  
- **Usuario:** luis@gmail.com | Contraseña: Admin123!  (tiene lo mismo que user de sergio pero tiene reservas activas)
- **Administrador:** admin@futomic.com | Contraseña: Admin123!  

> Te recomiendo crear un usuario propio para probar todo desde cero y explorar todas las funcionalidades de la aplicación, excepto las funciones de Admin ( admin@futomic.com | Contraseña: Admin123! ).

---



