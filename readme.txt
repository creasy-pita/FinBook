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
    


