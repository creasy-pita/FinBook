project.api 迁移使用如下命令
dotnet ef database update --project Project.API --startup-project Project.API -c ProjectContext
dotnet ef migrations add Initdb --project Project.Infrastructure --startup-project Project.API -c ProjectContext -o Migrations