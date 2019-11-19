
## MODELOS
- Models > Add Class
- En el modelo, agregar atributos y referencias
- Agregar al ApplicationDbContext.cs

``
dotnet ef migrations add <MigrationName> (se demora, a veces es necesario cerrar y abrir Rider)
dotnet ef database update
dotnet aspnet-codegenerator controller -name <ControllerName> -m <ModelName> -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
```

INDEX
Agregar en Home > Index el link al modelo`

PARA ELIMINAR UNA MIGRACIÓN
`dotnet ef database update <previous-migration-name>`
`dotnet ef migrations remove`

```
var type = typeof(IMyInterface);
var types = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => type.IsAssignableFrom(p));
```
