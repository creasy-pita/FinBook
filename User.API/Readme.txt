
准备  ResilientHttpClient  
在startup中将 ResilientHttpClient注入容器，在userservice 的httpclient 注入 ResilientHttpClient
	预先定义 IHttpClient
	userservice 使用 IHttpClient



dbcontext 和 model 准备
    1 创建appuserdbcontext,appuser
        data/appuserdbcontext,model/appuser
数据库准备
    mysql 中创建finbook_metadata 数据库
    创建 finbook_test 用户 和授权
        CREATE USER 'finbook_test'@'%' IDENTIFIED BY 'root';
        GRANT ALL PRIVILEGES ON *.* TO 'finbook_test'@'%' WITH GRANT OPTION;
    设置 connectionString 为 "Server=192.168.11.83;Database=finbook_metadata;Uid=finbook_test;Pwd=root;Encrypt=true"
数据库迁移和初始化
    dotnet ef cli 安装

	创建首次迁移代码
		dotnet ef migrations add Initdb -c AppUserDbContext -o Data/migrations
		dotnet ef database update -c AppUserDbContext
    finbook_metadata 库 初始 ef表
        CREATE TABLE `__EFMigrationsHistory` 
                ( 
                    `MigrationId` nvarchar(150) NOT NULL, 
                    `ProductVersion` nvarchar(32) NOT NULL, 
                    PRIMARY KEY (`MigrationId`) 
                );
    


