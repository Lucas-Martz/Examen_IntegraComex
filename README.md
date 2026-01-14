# Examen IntegraComex — ASP.NET MVC 5 + EF6 + SQL Server

Aplicación web en ASP.NET MVC 5 (.NET Framework) que implementa CRUD de Clientes con:
- Persistencia con Entity Framework 6
- Borrado lógico (Desactivar / Reactivar) vía campo `Activo`
- Autocompletado de Razón Social por CUIT mediante AJAX (consumiendo un WebMethod externo)

---

## Requisitos

- Visual Studio 2022 (o compatible con .NET Framework)
- .NET Framework 4.8 (recomendado)
- SQL Server (Express / Developer / Local)
- (Opcional) SQL Server Management Studio (SSMS)

---

## Instalación / Ejecución (paso a paso)

### 1) Clonar el repositorio
```bash
git clone <URL_DEL_REPO>
cd <CARPETA_DEL_REPO>
