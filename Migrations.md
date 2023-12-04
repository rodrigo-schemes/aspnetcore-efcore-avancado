## Migrations
##### Adiciona Migrations
    dotnet ef migrations add PrimeiraMigracao -p .\CursoEFCore\CursoEFCore.csproj
##### Gerar Script
    dotnet ef migrations script -p .\CursoEFCore\CursoEFCore.csproj -o .\CursoEFCore\PrimeiraMigracao.SQL
##### Gerar Script Idempotente
    dotnet ef migrations script -p .\CursoEFCore\CursoEFCore.csproj -o .\CursoEFCore\Idempotente.SQL -i
##### Aplicar a Migration
    dotnet ef database update -p .\CursoEFCore\CursoEFCore.csproj -v
##### Rollback de Migration
    dotnet ef database update [Migração Ponto de Retorno] -p .\CursoEFCore\CursoEFCore.csproj
##### Remover Migration
    dotnet ef migrations remove -p .\CursoEFCore\CursoEFCore.csproj
##### Aplicar Migration via código
    db.Database.Migrate();
##### Verificar Migrations Pendentes
    db.Database.GetPendingMigration();
##### Scaffold (Engenharia Reversa)
    dotnet ef dbcontext scaffold [string de conexao] MIcrosoft.EntityFramework.SqlServer




