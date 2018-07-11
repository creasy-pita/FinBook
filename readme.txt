2018-7-11
	添加项目推荐服务调用
		创建 RecommendService IRecommendService
		注入RecommendService
	添加 ProjectQueries 及 webcontroller 调用
			创建类
			实现类
				使用Dapper
				只取表中的部分字段信息 ，所以返回 Task<dynamic>类型
	ProjectController 中使用 ProjectQueries
		注入 ProjectQueries
		添加 GetProjects  (Route("my/{projectId}")), GetProjectDetail 
	
	IRecommendService
	


2018-7-10
PowerShell 使用
	搜索
		Find-Package  "MySql.Data.EntityFrameworkCore"
	安装
		Install-Package MySql.Data.EntityFrameworkCore -Version 8.0.11
	卸载
		Uninstall-Package MySql.Data.EntityFrameworkCore -Version 8.0.11
	注：各版本的命令 不同

EF  migration  



Mediat 
	发送领域事件

	发送领域集成事件
		消息 发送 处理中Handler 类在接收到发送的消息去执行 Handler 方法时 可以选择发送领域集成事件，通过eventbus 广播到跨服务




mysql
	安装
	登录
	创建用户
	创建数据库
		CREATE DATABASE finbook_beta_project CHARACTER SET utf8 COLLATE utf8_general_ci;
	用户授权
		GRANT ALL ON finbook_beta_project.* TO 'finbook_test'@'192.168.5.217';
		GRANT ALL ON *.* TO 'finbook_test'@'%';
	撤回权限
		REVOKE ALL PRIVILEGES FROM 'finbook_test'@'192.168.5.217';
	应用程序 数据库连接串
		connectionString 为 "Server=192.168.11.83;Database=finbook_metadata;Uid=finbook_test;Pwd=root;Encrypt=true"

错误：
	The Entity Framework Core Package Manager Console Tools don't support PowerShell version 2.0
	
	
	问题：
		The 'MySQLNumberTypeMapping' does not support value conversions. Support for value conversions typically requires changes in the database provider.
	原因：
		版本 Microsoft.EntityFrameworkCore.Relational 2.1.1可能 和 Mysql.data.EntityFrameworkCore 一起使用有问题
	解决： 
		版本修改 都使用2.0.1
	
	Unable to cast object of type 'ConcreteTypeMapping' to type 'Microsoft.EntityFrameworkCore.Storage.RelationalTypeMapping'.
		比较 Project.API  User.API 中  
			Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Relational,MySql.Data.EntityFrameworkCore 
		不同
			Microsoft.EntityFrameworkCore 2.1.1 //Project.API
			Microsoft.EntityFrameworkCore 2.0.1 //User.API
			
	问题：Your target project 'Project.API' doesn't match your migrations assembly 'Project.Infrastructure'. Either change your target project or change
	your migrations assembly.
	Change your migrations assembly by using DbContextOptionsBuilder. E.g. options.UseSqlServer(connection, b => b.MigrationsAssembly("Project.API")). By default, the migrations assembly is the assembly containing the DbContext.
	Change your target project to the migrations project by using the Package Manager Console's Default project drop-down list, or by executing "dotnet ef" from the directory containing the migrations project.	
	解决：
		options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")
		, b=>b.MigrationsAssembly(typeof(ProjectContext).Assembly.GetName().Name));
		修改为
		options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")
		,b => b.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));					
		
	问题：Specified key was too long; max key length is 767 bytes
		`Key` varchar(767) NOT NULL,
		Value varchar(767) NOT NULL,
	原因： 
		主键最长字节限制  3072 bytes
	解决1：
		修改Key 的长度为50后 重新 dotnet ef database update 不能成功
		原因： 结构修改后 需要重新  创建迁移
			dotnet ef database migrations

		
2018-7-10
课程笔记
	项目服务实现

	
	
	通过MediatR 完成api 对Command 调用
		webapi层 项目create,view，join 实现
			httppost  Project对象
			创建 ProjectCreateCommand
			MediatR发送command 请求
				MediaR 发布订阅可以查看官网文档
					https://github.com/jbogard/MediatR/wiki
			配置MediatR
				添加用于aspnetcore 单独包
				  MediatR.Extensions.Microsoft.DependencyInjection
				方法：AddMediatR 其实是在 Microsoft.Extension.DependencyInjection.MediatR中，但是是对 IServiceCollection services的扩展，所以需要引入 Microsoft.Extension.DependencyInjection.MediatR
			ProjectController 继承BaseController 可以获取当前登入的用户信息
			
	 CommanHandler 处理方法中 IProjectRepository 实现类 ProjectRepository EF 的实现
		基础层
			添加EF ,实现 IProjectRepository
			ProjectDbContext 实现
				
	跨服务的调用

补充 
	EF 基础类的实现 
	再检视一遍EF 的 Quick Overview的知识结果
			资料： https://docs.microsoft.com/en-us/ef/core/
ProjectController 继承 BaseControoler
			

搭建CQRS框架步骤
	domain
		领域事件：ProjectCreateEvent
			会在 加入 domainEvents列表
	
		
	
	command  handler：create，view,join
	queries-project list,project detail
	
推荐服务
	获取推荐列表，领域集成事件

应用层	
	commandHandler
		处理command 和业务持久化
	command
领域层
	领域事件：
		ProjectCreateEvent ：


MediatR	的使用
	一对一 发送消息请求， 直接返回response
		相关类
			IRequest IRequestHandler
		消息	
			定义代码
				public class Ping : IRequest<string> { }
		消息处理者
			会定义处理方法，会传入消息对象， <Ping, string> 代表输入消息对象Ping, 返回string
			处理者代码
				public class PingHandler : IRequestHandler<Ping, string> {
					public Task<string> Handle(Ping request, CancellationToken cancellationToken) {
						return Task.FromResult("Pong");
					}
				}			
		发送消息
			会执行之前定义的处理方法
			执行代码
				var response = await mediator.Send(new Ping());
				Debug.WriteLine(response); // "Pong"
		
		
		发送消息后不需要返回值情形：
	一对多 ：发布 订阅，不会有返回信息
			相关类 INotification 


2018-7-6

其他类给当前类扩展方法
	public static ILogFactory AddLog4net(this ILogFactory logFactory);


2018-7-5
	使用polly
		超时与重试（Timeout and Retry)
		限流（Rate Limiting)
		熔断器（Circuit Breaker)
		舱壁隔离(Bulkhead Isolation)
		回退(Fallback)	
	准备  ResilientHttpClient ResilientHttpClientFactory 
		在 ResilientHttpClientFactory 中准备 polly 的定义处理规则
		在startup中将 ResilientHttpClient注入容器，在userservice 的httpclient 注入 ResilientHttpClient
		预先定义 IHttpClient
		userservice 使用 IHttpClient
		
	错误处理：
		ResilientHttpClient 从 eShopOnContainers项目参考
			其中requestMessage.Content 从使用 StringContent 修改为 FormUrlEncodedContent
		dnsresult.First().HostName  返回值  ：localhost. 会多一个 [.]
			修改
				var result = await _dns.ResolveServiceAsync("service.consul", _options.Value.ServiceName);
				var addressList = result.First().AddressList;
				var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName.TrimEnd('.');		
	参考资料：
		https://www.cnblogs.com/jesse2013/p/polly-docs.html
2018-5-30
	微服务之间的发现
		微服务user.identity使用 consul  dnsclient  来发现微服务user.api 
		user.identity 通过dnsclient 传递servicename通过consul端口 发现 之前注册在consul服务中的user.api 
	参考资料：
		注册服务发现：
			http://michaco.net/blog/ServiceDiscoveryAndHealthChecksInAspNetCoreWithConsul?tag=Microservices
			
	错误汇总
	
	
		_dns.ResolveServiceAsync("service.consul", _options.Value.ServiceName)没有返回列表
			检查方式：
				跟之前简单注册可以成功的方式进行对比， 
				采用排除法 调整 端口，host 为固定的值，都没有成功，最好调整ServiceName 为固定值是发现 带 【.】 不行
				
		
			当 _options.Value.ServiceName ="User.API" 没有返回列表
			当 _options.Value.ServiceName ="UserAPI" 有返回列表
	经验：
		当有一个成功实例时
		参考再做时如果有问题，要全面考虑涉及的关键变量值是否相同，再是所有变量值是否一致
2018-5-24


Ocelot 集成consul服务发现
	下载和安装consul 
	开启consul   默认ui  和 consul服务发现监听端口 8500； 命令：consul agent -dev
	创建api1,api2 并使用consul服务; 
		api1 创建 HealthCheckController
		引入 Consul 包
		api1 startup config中 注册到consul服务
		
	创建Ocelot  gateway.api
		创建Ocelot.json
		创建webhost 时加入Ocelot.json
		依赖注入和加入中间件
	consul  ui
		默认 http://localhsot:8500
	参考资料：
		Consul 
			https://www.consul.io/intro/getting-started/services.html
		Ocelot 配置
			configurtion 模板
				http://ocelot.readthedocs.io/en/latest/features/configuration.html
			servicediscovery
				http://ocelot.readthedocs.io/en/latest/features/servicediscovery.html
	错误汇总
		测试 gateway.api 访问api1的资源时报错
			原因：创建webhost 时加入Ocelot.json 这一步未做
				时间花费在 对json 文件内容的检查 而没有 跳出 去查看真实原因
	
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

gateway.api 集成identity
	
错误汇总
	拿到访问令牌 postman 去访问 user.api的资源时提示错误
	Unable to obtain configuration from: 'https://localhost:50255/.well-known/openid-configuration'.
		解决： gateway.api 注册identityservier时 修改https 为 http 
					services.AddAuthentication()
						.AddIdentityServerAuthentication(authenticationProviderKey, options =>
						{
							//options.Authority = "https://localhost:50255";
							options.Authority = "http://localhost:50255";
							options.ApiName = "gateway_api";
							options.SupportedTokens = SupportedTokens.Both;
							options.ApiSecret = "secret";
							options.RequireHttpsMetadata = false;
						});		

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
    


