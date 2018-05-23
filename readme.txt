2018-5-23
identity 自定义validtor
	自定义 SmsAuthCodeValidator, 实现 IExtensionGrantValidator
	
	创建 验证码验证服务接口IAuthCodeService，TestAuthCodeService,user服务接口 IUserService
identity 与 userservice 互通
	指定 userserviceUrl = "http:localhost"
	创建 HttpClient
	UserService中 发起HttpPost 使用user.api 对phone进行验证
集成identityserver4到 user.identity	
	创建 client, apiresource,userresource对象
		GrantType ="sms_auth_code"
		new apiresource{"user_api","user service"}	
	user.identity 引入 identityserver4
            services.AddIdentityServer()
                    .AddExtensionGrantValidator<SmsAuthCodeValidator>()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryApiResources(Config.GetResource())
                    .AddInMemoryClients(Config.GetClient())
                    .AddInMemoryIdentityResources(Config.GetIdentityResources());		
	

	
	startup configservice中注册 内存式的 client, apiresource,userresource
	
	postman  访问 user.identity  下 /connect/token 获取 token
		调用SmsAuthCodeValidator 的验证方法
			验证方法中 发起HttpPost 使用user.api 对phone进行验证

2018-5-22
加入 api 网关  gataway.api
	1 创建webapi 项目  gataway.api
	2 引入 Ocelot 包 
	3 添加网关配置文件 Ocelot.json，配置路由
		{
			"ReRoutes": [
				{
					"DownstreamPathTemplate": "/api/user/{all}",
					"DownstreamScheme": "http",
					"DownstreamHostAndPorts": [
							{
								"Host": "localhost",
								"Port": 56688
							}
						],
					"UpstreamPathTemplate": "/user/{all}",
					"UpstreamHttpMethod": [ "Get"]
				}

			]
		}		
	
	4 program 中读取配置
	5 startup 中注入服务 ， 加入中间件


2018-5-21
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
    


