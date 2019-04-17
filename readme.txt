2019年4月16日
	问题
		Use.API 中的BaseController 中  User.Claims 没有值
	具体描述：
		PostMan中
		通过 http://localhost:50255/connect/token 获取access_token 值
		patch :http://localhost:{{host}}/api/user/ 时，把access_token 放入environments的{{token}}中
		调用后台发现User.Claims
	原因： environments 分类别维护，patch :http://localhost:{{host}}/api/user/ 的请求没有选中这个类别，所以就不能读取到这个类别下边的{{token}}参数  

	eventbus CAP+ mysql + rabbitmq 的方式
		nuget 包
			dotnetcore.cap
			dotnetcore.mysql .rabbitmq
		安装rabbitmq

	开源组件 CAP 资料 ：https://www.cnblogs.com/savorboard/p/cap.html

	RabbitMQ 的docker 方式安装
	1  docker 国内镜像 安装 
		https://blog.csdn.net/LaySolitary/article/details/82623913
		启动docker ： systemctl start docker
	2 docker rabbitmq
		//docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 registry.docker-cn.com/rabbitmq:3-management
		1 docker pull daocloud.io/library/rabbitmq:3.5.3-management
		2 docker run -d -p 5672:5672 -p 15672:15672 --name rabbit -e RABBITMQ_DEFAULT_USER=kail  -e RABBITMQ_DEFAULT_PASS=1234 daocloud.io/library/rabbitmq:3.5.3-management
		3 访问 http://192.168.161.132:15672
		参考：https://www.cnblogs.com/yufeng218/p/9452621.html
		https://www.kancloud.cn/kailbin/rabbitmq/471964
		注：
		1 官方镜像特别慢，
			修改docker镜像地址
				在docker pull 镜像时 加入registry.docker-cn.com
				参考：https://www.docker-cn.com/registry-mirror
				参考： https://www.kancloud.cn/kailbin/rabbitmq/471964	
		2 Linux软件源慢   ：镜像修改，整体修改，而不是针对单个软件包的镜像地址修改方式
		https://blog.csdn.net/QimaoRyan/article/details/78169223


2019年4月14日

dotnet ef migrations add Initdb --project Project.Infrastructure --startup-project Project.API -c ProjectContext -o Migrations
dotnet ef database update --project Project.Infrastructure --startup-project Project.API -c ProjectContext
	dotnet ef migrations add Initdb --project Project.API --startup-project Project.API -c ProjectContext -o Migrations
	dotnet ef database update -c ProjectContext --project Project.API --startup-project Project.API 


	mysql  efcore  code first  migrate
	创建 AppUserDbContext, model
	startup  中 services.AddDbContext<AppUserDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
	nuget 安装 mysql.data.entityframeworkcore\8.0.15
	dotnet ef migrations add Initdb --project MysqlEFMigrate --startup-project MysqlEFMigrate -c AppUserDbContext -o Data/migrations
	dotnet ef database update -c AppUserDbContext --project MysqlEFMigrate --startup-project MysqlEFMigrate
	
	参考示例见： git  https://github.com/creasy-pita/aspnetcoreDemo/tree/master/ef/MysqlEFMigrate
	
	说明： 
		1 以上方法 ，不需要先常见数据库和 __EFMigrationsHistory 表 ，这些迁移时自动生成
		2 --startup-project  指定 startup所在项目 --project 指定 dbcontext所在项目
		3 有迁移提示 【 The 'MySQLNumberTypeMapping' does not support value conversions.】 问题   网上有说使用 pemo.efcore 的解决方式，但没有试验成功。 但以上的迁移方法方法确认可行
		参考资料 https://stackoverflow.com/questions/50442930/microsoft-entityframeworkcore-2-1-rc-with-mysql-data-entityframeworkcore
		
	问题：TestEFMigration 项目的问题
		System.InvalidOperationException: The current CSharpHelper cannot scaffold literals of type 'Microsoft.EntityFrameworkCore.Metadata.Internal.DirectConstructorBinding'. Configure your services to use one that can.
	   at Microsoft.EntityFrameworkCore.Design.Internal.CSharpHelper.UnknownLiteral(Object value)
	   at Microsoft.EntityFrameworkCore.Migrations.Design.CSharpSnapshotGenerator.GenerateAnnotation(IAnnotation annotation, IndentedStringBuilder stringBuilder)
	   at Microsoft.EntityFrameworkCore.Migrations.Design.CSharpSnapshotGenerator.GenerateEntityTypeAnnotations(String builderName, IEntityType entityType, IndentedStringBuilder stringBuilder)
	   at Microsoft.EntityFrameworkCore.Migrations.Design.CSharpSnapshotGenerator.GenerateEntityType(String builderName, IEntityType entityType, IndentedStringBuilder stringBuilder)
	   at Microsoft.EntityFrameworkCore.Migrations.Design.CSharpSnapshotGenerator.GenerateEntityTypes(String builderName, IReadOnlyList`1 entityTypes, IndentedStringBuilder stringBuilder)
	   at Microsoft.EntityFrameworkCore.Migrations.Design.CSharpSnapshotGenerator.Generate(String builderName, IModel model, IndentedStringBuilder stringBuilder)
	   at Microsoft.EntityFrameworkCore.Migrations.Design.CSharpMigrationsGenerator.GenerateMetadata(String migrationNamespace, Type contextType, String migrationName, String migrationId, IModel targetModel)
	   at Microsoft.EntityFrameworkCore.Migrations.Design.MigrationsScaffolder.ScaffoldMigration(String migrationName, String rootNamespace, String subNamespace)
	   at Microsoft.EntityFrameworkCore.Design.Internal.MigrationsOperations.AddMigration(String name, String outputDir, String contextType)
	   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.AddMigrationImpl(String name, String outputDir, String contextType)
	   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.<>c__DisplayClass3_0`1.<Execute>b__0()
	   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.Execute(Action action)
		The current CSharpHelper cannot scaffold literals of type 'Microsoft.EntityFrameworkCore.Metadata.Internal.DirectConstructorBinding'. Configure your services to use one that can.
		解决：Microsoft.Aspnetcore.All  为 2.0.0 ， microsoft.entityframeworkcore.design 2.0.1	升级为 2.10
		原因： 可能各包完成迁移时不匹配的版本产生配合的问题，不匹配版本在生成编译时时能通过的（类型，方法等定义上没有问题）
		参考资料： https://stackoverflow.com/questions/50763221/migrations-error-cannot-scaffold-literals-of-type-directconstructorbinding
	问题： TestEFMigration 项目的问题
		System.ArgumentException: Option not supported.
		Parameter name: encrypt
		   at MySql.Data.MySqlClient.MySqlBaseConnectionStringBuilder.GetOption(String key)
		   at MySql.Data.MySqlClient.MySqlConnectionStringBuilder.set_Item(String keyword, Object value)
		   at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
		   at MySql.Data.MySqlClient.MySqlBaseConnectionStringBuilder..ctor(String connStr, Boolean isXProtocol)
		   at MySql.Data.MySqlClient.MySqlConnection.set_ConnectionString(String value)
		   at MySql.Data.EntityFrameworkCore.MySQLServerConnection.CreateDbConnection()
		   at Microsoft.EntityFrameworkCore.Internal.LazyRef`1.get_Value()
		   at MySql.Data.EntityFrameworkCore.MySQLDatabaseCreator.Exists()
		   at Microsoft.EntityFrameworkCore.Migrations.HistoryRepository.Exists()
		   at Microsoft.EntityFrameworkCore.Migrations.Internal.Migrator.Migrate(String targetMigration)
		   at Microsoft.EntityFrameworkCore.Design.Internal.MigrationsOperations.UpdateDatabase(String targetMigration, String contextType)
		   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.Execute(Action action)
		Option not supported.
		Parameter name: encrypt
		原因：connectstring 中加入了 Encrypt=true，数据库可能需要开启某个安全项的设置才可以
		解决： 去除 Encrypt=true
	
	问题 
		Project.API   项目于 在 dotnet ef database update --project Project.API --startup-project Project.API -c ProjectContext
Build failed.  提示	build fialed
		原因： 生成项目于失败，需要先解决 项目生成失败的问题
		
2018-8-3
TBD  	rabbitmq 消息处理的延时时间的设置，现在有大概5m的 delay
test conflict1 
2018-8-1
test conflict2
	mongodb 
	下载  window .msi  
		下载地址
	安装
	环境变量设置
		Path append   <install directory>\bin;
	使用 mongo  cmd 命令创建 dbdata |log  directory
		md "F:\dbdata\mongodb\data\db" "F:\dbdata\mongodb\log"
	exe 方式打开 mongodb
		"d:\Program Files\MongoDB\Server\4.0\bin\mongod.exe" --dbpath="F:\dbdata\mongodb\data\db"
	window services 
		Start MongoDB Community Edition as a Windows Service
		Stop MongoDB Community Edition as a Windows Service
		Remove MongoDB Community Edition as a Windows Service
		方式启动，停止 和 从windows services 删除  m

2018-7-30
问题：
	配置正确，但CAP ，rabbitmq 就是不work问题问题 
	
解决步骤
	1 重装 erlang 和 rabbitmq 3.7.7
	2 不打开 consul 服务发现
		注：打开consul 服务也正常， 所以之前出现问题是  原因不会是端口冲突
	3 先使用
	Server=localhost;Database=finbook_metadata;Uid=root;Pwd=root;Encrypt=true;
		表现：
			程序控制台 输出报错误日志 InvalidOperationException: Value 'Prefered' not supported for option 'MySqlSslMode'.
			打开 dashboard 时 提示 InvalidOperationException: Value 'Prefered' not supported for option 'MySqlSslMode'.		
			cap.published ， received  表没有正常生成
			cap.published 可以插入，但是 received 记录不能生成

		说明：需要设置 SslMode ，暂时不设置， 修改为按4 操作
	4 修改为Server=localhost;Database=finbook_metadata;Uid=root;Pwd=root;
		表现：
			cap.published 可以插入，过大概3-5分钟时间received可以显示记录
			打开 dashboard 可以看到 publisher  subscriber received list 有信息
			
记录下 ：可以work 时的心情
	信息爆n个棚,难以平复, 大货已解，心以无碍，犹如重生，瞭望世界，宇宙问题皆可克之
	妳喜欢折腾的品质多少影响了我，让我consist on resolving this huge problem and finally work it out
初步结论 
	配置正确，但CAP ，rabbitmq 就是不work问题 判断为数据库连接字符串不同，连接方式不同，有些连接方式会出现错误导致
	
问题重现
	方式1
		1 Server=localhost;Database=finbook_metadata;Uid=root;Pwd=root;Encrypt=true
		2 不开启 DashBord 和 ServerDictionary
		3 启动web程序
			现象： 没有创建表，程序控制台也没有报错，造成一种 是 明明配置正确，数据连接也没什么问题 ，CAP ，rabbitmq 就是不work
		5 于是手动创建表，
		6 使用注册的publisher 发布一个消息后， 
			现象：可以发布消息，创建 published记录，但不能被接收和消费
	方式2
		1 Server=localhost;Database=finbook_metadata;Uid=root;Pwd=root;Encrypt=true
		2 开启 DashBord 和 ServerDictionary
		3 启动web程序
			现象：
				程序控制台 输出报错误日志 InvalidOperationException: Value 'Prefered' not supported for option 'MySqlSslMode'.
				没有自动创建表
		5 于是手动创建表
		6 使用注册的publisher 发布一个消息后， 
			说明：可以发布消息，创建 published记录，但不能被接收和消费
	方式3
		1 Server=localhost;Database=finbook_metadata;Uid=root;Pwd=root;
		2 开启 DashBord 和 ServerDictionary
		3 启动web程序
			现象：
				程序控制台 没有报错
				cap.published ， received  表正常生成
		4 使用注册的publisher 发布一个消息后， 
			现象：
				可以发布消息，创建 published记录，能正常接收和消费
进一步结论
	使用注册的publisher 发布一个消息 对数据库访问时，对连接方式要求宽松 可以使用：Server=localhost;Database=finbook_metadata;Uid=root;Pwd=root;Encrypt=true，
	而使用Dashboard, Discovery 时，当中去检查、创建数据库表，使用数据库表时 要求的连接方式严格，要使用Server=localhost;Database=finbook_metadata;Uid=root;Pwd=root;才可以
	
	具体 要看官网有没有这方面的资料， 或查看源码去分析


2018年7月28日

说明：
	cap use discovery 中的 currentnodeport  要使用当前webapi 运行的端口
recommend api  获取 通讯录服务的好友列表
	contact api 注册到服务发现
	通讯录添加 通过userId 获取好友列表的action   httpget 方式

	recommend api 实现  获取 通讯录服务的好友列表 的 contactservice
	ProjectCreatedIntegrationEventHandler 中 使用contactservice 完成 项目推荐发送给用户 的好友列表


群里的小伙伴 jesse老师的 CAP部分，有没有完全跑通的， 可以分享一个 能完整运行的示例    
我这边试了尝试了好几天  使用CAP 不能自动创建 cap.published cap.received 两张表， 发布消息时cap.published 会生成记录，但不能被订阅


使用CAP的问题 问了群里的伙伴没有进展 想求助下老师 参考老师的视频部署的，好几天了跑不通
问题简述
	使用CAP 不能自动创建 cap.published cap.received 两张表， 发布消息时cap.published 会生成记录，但不能被订阅
	听老师讲要架一个本地的 rabbitmq 环境

其他 
	jesse 老师 能否 就cap这块 分享一个 能完整运行的示例
软件环境
	系统环境 
		win7 64位
	vs      
		vs2017
	cap版本
		DotnetCore.CAP  2.2.0
		DotnetCore.CAP.RabbitMQ  2.2.0
		DotnetCore.CAP.MySql  2.2.0
	rabbitmq 版本
		rabbit-server 3.7.7   window
		erlang 20.3  window
	mysql EntityFrameworkCore
		MySql.Data.EntityFrameworkCore 6.10.6
	NetCore版本
		netcore2.0
	期待现象
		程序启动完成后，访问 localhost: http://localhost:49349/api/publish 后
			应该会自动创建 cap.published cap.received 两张表
			cap.published 表会生成记录,过段时间后 状态会从 'Scheduled'变为其他状态
			cap.received  表会生成记录
	实际现象
		没有自动创建 cap.published cap.received 两张表，而是自己手动创建
		cap.published 表会生成记录,过段时间后 状态没有改变
		cap.received  表没有生成记录
	关键代码段

		startup.cs
			public void ConfigureServices(IServiceCollection services)
			{
				services.AddMvc();
				services.AddOptions();
				services.AddScoped<Service1>();
				services.AddDbContext<MYDbContext>(options =>
	options.UseMySQL("Server=localhost;Database=finbook_beta_user;Uid=root;Pwd=root;Encrypt=true"));
				services.AddCap(options =>
				{
					options
						.UseEntityFramework<MYDbContext>()
						.UseRabbitMQ("localhost");
				});

			}

			public void Configure(IApplicationBuilder app, IHostingEnvironment env)
			{
				if (env.IsDevelopment())
				{
					app.UseDeveloperExceptionPage();
				}
				app.UseMvc();
				app.UseCap();
			}

		发布者 publishcontroller.cs


			[Route("api/[controller]")]
			public class PublishController : Controller
			{

				private readonly ICapPublisher _publisher;
				private MYDbContext _dbContext;
				public PublishController(ICapPublisher publisher, MYDbContext dbContext)
				{
					_dbContext = dbContext;
					_publisher = publisher;
				}
				// GET api/values
				[HttpGet]
				public  IActionResult Get()
				{
					using (var trans = _dbContext.Database.BeginTransaction())
					{
						_publisher.Publish("xxx.services.account.check", new Person { Name = "Foo", Age = 11 });
						trans.Commit();
					}
					return Ok();
				}
			}

		订阅者 Service1.cs
				public class Service1: ICapSubscribe
				{
					[CapSubscribe("xxx.services.account.check")]
					public void BarMessageProcessor(Person p)
					{
						string s = "ddd";
					}
				}







会不会与 rabbitmq  运行的当前用户有关 ljq  而不是  system用户

CREATE DATABASE IF NOT EXISTS finbook_beta_contact DEFAULT CHARSET utf8 COLLATE utf8_general_ci;
CREATE DATABASE IF NOT EXISTS finbook_beta_user DEFAULT CHARSET utf8 COLLATE utf8_general_ci;
2018-7-27
1 cap ，mysql efcore 使用时  Microsoft.EntityFrameworkCore.Relational 2.1.0的问题
	需要官方修复
	先使用较早版本 Microsoft.EntityFrameworkCore.Relational 2.0.1

推荐服务
	未完成
	
	任务130： 推荐服务实现 - 访问联系人服务获取好友信息
		使用通讯录查找所有好友，给这些好友添加 推荐记录
	18:07 
	任务131： 推荐服务实现 - 调试推荐服务
	21:00 
	任务132： 推荐服务实现 - 推荐服务加入网关	
	

2018-7-25

挂起问题

	按资料注册，rabbitmq 不能正常使用 
	CAP 不能正常使用
		视频使用版本
			cap 2.2.0
			mysql.entityframeworkcore   6.10.6
			rabbitmq 3.7.4
			
		
		
	待解决方式：
		查看rabbitmq 是否有什么遗漏设置

任务
	服务间交互
		中间件方案：
			MediatR， RawRabbit，RabbitMQ，EventBusOnEshopOnContainers(是对 RabbitMQ 的封装)
	RabbitMQ使用介绍
		查看是否成功安装
			rabbitmq-service
		开启 management - ui
			rabbitmq-plugins enable rabbitmq_management


		start rabbitmq service
			rabbitmq-service start
		查看服务状态
			rabbitmq-service status

	UserAPI 集成CAP
	
	UserAPI CAP 事件发送实现
		创建 事件 UserProfileChangedEvent
		使用 
	ContactAPI集成CAP
		cap 引入中间件 注册 RabbitMQ mysql地址等
		创建mysql database finbook_beta_contact
		创建 UserProfileChangedEvent, UserProfileChangedEventHandler:ICapSubscribe
		
	contactapi cap事件接收
		
错误：
	1 http://localhost:50280/cap/published
		ERR_CONNECTION_REFUSED
	

	2 没有自动创建 cap 相关表 ，需要看cap源码 create table部分
		资料：https://github.com/dotnetcore/CAP/search?utf8=%E2%9C%93&q=create+table++mysql&type=
	
			DROP TABLE IF EXISTS `cap.queue`;
		CREATE TABLE IF NOT EXISTS `cap.received` (
		  `Id` int(127) NOT NULL AUTO_INCREMENT,
		  `Name` varchar(400) NOT NULL,
		  `Group` varchar(200) DEFAULT NULL,
		  `Content` longtext,
		  `Retries` int(11) DEFAULT NULL,
		  `Added` datetime NOT NULL,
		  `ExpiresAt` datetime DEFAULT NULL,
		  `StatusName` varchar(50) NOT NULL,
		  PRIMARY KEY (`Id`)
		) ENGINE=InnoDB DEFAULT CHARSET=utf8;
		CREATE TABLE IF NOT EXISTS `cap.published` (
		  `Id` int(127) NOT NULL AUTO_INCREMENT,
		  `Name` varchar(200) NOT NULL,
		  `Content` longtext,
		  `Retries` int(11) DEFAULT NULL,
		  `Added` datetime NOT NULL,
		  `ExpiresAt` datetime DEFAULT NULL,
		  `StatusName` varchar(40) NOT NULL,
		  PRIMARY KEY (`Id`)
		) ENGINE=InnoDB DEFAULT CHARSET=utf8;
	使用select * from `cap.published`查询

拓展
	ocelot 网关 的路由 原理
	mvc路由原理
	相互的比较
	
通讯录中冗余了用户的信息，用户服务中用户信息更新时同时 通知通讯录服务更新相关用户信息
	使用cap
		eventbus 使用 rabbitmq
			cap
			cap.mysql
			cap.rabbitmq
			
	rabbitmq 的安装 和使用
		系统环境
			可以本机
			也可以 远程docker方式安装
	

2018-7-24



TBD 
	BaseController User.Claims 获取时 注意 没有传相应 名称的 claims 情况 
contactapi 完整运行
	postman  
		通过gateway 转发到各api形式访问
		直接各api 形式访问
	使用phone添加A,B用户， 
	A用户 check-or-create 来获取 token ,不存在此用户时会创建
	用户更新信息
	A 查看好友列表，发现没有B   api/Contact
	A 申请 添加B 为好友 post:apply-request/{userId}   
	B 获取好友申请列表  HttpGet :api/Contact/apply-request
	B 通过A的好友申请   HttpPut :api/Contact/apply-request
	A 好友列表 可查看到B 
	A.phone 135555554444 B.phone 15924852562
	
		token 的值会在有加入认证框架的webapi中 自动放入header
错误记录：
	访问user.api 获取用户基本信息 (http://localhost/user/baseinfo/1) 时 BaseController User.Claims 找不到用户的基本信息
		可能原因：
			访问 待用户信息的 token 没有传入
			user.api 没有去获取
				获取方式
					引入认证中间件
			获取方式问题
				语句：
					identity.UserId = Convert.ToInt32(User.Claims.FirstOrDefault());
				应修改为：
					identity.UserId = Convert.ToInt32(User.Claims.FirstOrDefault(c=>c.Type == "sub").Value?? "";
	网关路由
		http://localhost/user 与 http://localhost/user/ 的区别
		/contact/apply-request/{catchAll} 与 /contact/apply-request 的区别
		
		
	MultipartReaderStream	
		IOException: Unexpected end of Stream, the content may have already been read by another component.	Microsoft.AspNetCore.WebUtilities.MultipartReaderStream+<ReadAsync>d__36.MoveNext()
		
		原因 
			webapi 接口定义 如下：
				[HttpPost]
				[Route("apply-request/{userId}")]
				public async Task<IActionResult> AddApplyRequest(int userId, CancellationToken cancellationToken)
			需要从 userId 会从 url 中读取，而不是 form body中读取
			另一边 postman 的post 形式时 form body中输入过key,value = userId,3
			+ url 为http://localhost/contact/apply-request/3 时 
		    aspnet 会认为多种方式传递userId值  所以报异常提醒
		修改方式
			去掉form body中的 key,value = userId,3
	
	
contactapi 调用 userservice 与 userapi 通信GetBaseUserInfo时 ，使用 传递服务名到consul dns 来获取
	可以修改为 由ocelot 网关来转发
		步骤 
			userservice 调用userapi 的地址 使用注册在网关的地址
			ocelot.json 配置中增加 转发路由
			


userapi  写入用户相关信息的claims 的方式
	HttpPost 方式 controller  action 中的参数会获取Form中的对象
	HttpGet 方式 controller  action 中的参数会获取url 中的参数，需要路由中配置参数名称
		比如：
			[Route("check-or-create")]
			[HttpPost]
			public async Task<IActionResult> CheckOrCreate(string phone)	
		与
			/// <summary>
			/// 获取指定userId的用户信息
			/// </summary>
			/// <returns></returns>
			[HttpGet]
			[Route("baseinfo/{id}")]
			public async Task<IActionResult> GetBaseUserInfo(int userId)

2018-7-23
contact api 写入用户相关信息的claims 的方式
	ProfileService
		var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
		if (!int.TryParse(subjectId, out int intUserId))
		{
			throw new ArgumentNullException(nameof(context.Subject));
		}
		context.IssuedClaims = context.Subject.Claims.ToList();
	SmsAuthCodeValidator  
		验证方法中 获取用户信息 写入
		代码
			context.Result = new GrantValidationResult(user.UserId.ToString(), GrantType,claims);


通讯录服务获取 认证服务回传的带用户信息的claims
	方式
	contact.api 
	1 identity.api 的 config.cs 里面添加 API Resource 叫 Contact_API，并且加到client的 AllowedScope中
	2 在 ocelot 的配置中添加一个 .wellknow 模糊匹配转发
	3 配置 identity server 4 endpoints 模糊匹配转发
	4 在contact api 中添加jwt authendication
		//AddAuthentication 则 identity 会帮我们装载 认证token中的claims 到 System.Security.Claims.ClaimsPrincipal User
	5 在 basecontroller 中 从 clams 获取当前用户信息 profile claims
	

claims 加入 user 信息 
	
调试 通讯录服务的 get: api/contacts

	consul 服务 发现 
		服务端注册
	appsettings.json file
		set the ServiceDiscovery node 
			like follow:
			  "ServiceDiscovery": {
				"UserServiceName": "UserAPI", //会使用UserAPI,这里配置UserAPI在consul dns 中的名称UserAPI
				"Consul": {
				  "HttpEndpoint": "http://127.0.0.1:8500",
				  "DnsEndpoint": {
					"Address": "127.0.0.1",
					"Port": 8600
				  }
				}			
	startup
		configservice
		config the discoveryservice (consulservice ) host address
	
		injection IDnsQuery  when api call the other webapi service
		客户端注册 即 api service 注册到 consul 中
				var registration = new AgentServiceRegistration()
                {
                    Check = httpCheck,
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceDisvoveryOptions.Value.ServiceName,
                    Port = address.Port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
				
	错误记录：
		注入相关服务
		包括 IHttpClient ResilientHttpClient ResilientHttpClientFactory IDnsQuery AppSettings ServiceDisvoveryOptions IConsulClient
2018-7-22

用户api 和 用户认证 服务 
	认证时 claims增加用户的基本信息
TBD 
	笔记本中
		mongo 镜像安装和使用
		mysql 各个数据库的准备检查
		通讯录服务 contact api 调试

通讯录服务 contact api 加入到ocelot 网关
get: api/contacts
put:api/contacts/tag
get:api/contacts/apply-request
post:api/contacts/apply-request
put:api/contacts/apply-request

通讯录服务 用户给好友打标签 用户获取好友列表
	
	重点
		controller 层 使用viewmodel (通常是对界面上的输入组合来定义一个 viewmodel),dto(通常是后端临时对象，非领域层的对象定义)

快捷键
	引入命名空间 
		alt + enter
		shift + alt + f10

通讯录服务 用户通过好友申请 实现 完善 
	//获取 当前上下文的用户  和申请人的  信息
	//当前上下文的用户id 添加好友
	//对方好友也要添加当前用户作为好友
通讯录服务 用户好友申请 添加和 申请 通过实现 
	mongo更新用户
		使用linq表达是定义 where 条件
		使用FilterDefinitiion 和 UpdateDefinition 来定义更新的条件和 赋值部分
	
通讯录服务 用户好友申请 和用户 信息的更新 实现 Mongodb 的查询和更新的基本用法
	

	mongodb 组合关系的表的更新
		场景：
			用户id=3 更改信息 此用户有5个好友，则其好友对应通讯录中该用户的信息也同时更新
		步骤和实现
						//查找该用户的通讯录，如果没有通讯录则说明没有好友直接返回true
						//取出该用户的 所有联系人的id
						//有通讯录，用mongodb的关联内部查询方式匹配
						//定义 filterdifinition 用and 条件连接符
							//所有联系人的contactBook 
							//contackbook 中的 contact.UserId==userInfo.UserId 
						//定义 updatedefinition
		
关键
	mongo 数据集合的查询方法
	
		FindAsync(r=>r.UserId = userInfo.UserId) 报错 
		原因： 没有引入 Mongo.Driver空间
		
		组合 表的查询和更新方式
		
		CancellitionToken 用于 异步方法中 主动的中断未完成的异步方法

错误处理：
	User.Identity nuget configuration is invalid.
	restart the  visual studio

		
2018-7-21


通讯录服务
	目录结构创建
	类创建
		model,data,repository,service,dto,ContactController
	service
		userservice 实现
			使用consul 服务发现调用用户api 获取用户基本信息
	repository dbContext 采用mongo实现
		mongo 
			镜像安装
			客户端安装
			dbContext mongo的连接字符串， 组件引入 
		

未完成任务
	任务68： FluentAPI写UserController测试用例（上）
	14:51 
	任务69： FluentAPI写UserController测试用例（下）
	17:56 
	任务70： GitLab CI完整部署UserAPI到线上测试环境-上
	22:18 
	任务71： GitLab CI完整部署UserAPI到线上测试环境-中
	22:06 
	任务72： GitLab CI完整部署UserAPI到线上测试环境-下
	24:16 
	任务73： GitLab CI完整部署UserAPI到线上测试环境-Debug

2018-7-20
TBD  mysql 数据库访问连接 使用root用户可以， 不使用时 会报错问题解决

省厅数据量大 ，
取待报送的数据 
select 


任务64： 全局异常处理与日志记录
	自定义用户异常类
	usercontroller 方法执行异常时 抛出用户异常
	自定义全局filter  继承 ExceptionFilter
		实现异常处理方法 ，根据 异常上下文中的信息来判断是什么异常，有区别的处理
		处理中需要 定义 response 的内容及httpstatus

2018-7-19

项目推荐服务实现 的部分开发
	推荐服务api 开发
		创建 Recommend  EF ，数据库
	IntegrationEventHandlers handler 实现
		获取fromuser 信息
		通过 Event传入项目的用户id,及项目其他基本信息  
		使用  consul 服务发现 找到User 服务地址，获取用户基本信息
		使用通讯录查找所有好友，给这些好友添加 推荐记录
			通讯录api服务 注册到consul
			
错误：
		迁移数据库时报错：The 'MySQLNumberTypeMapping' does not support value conversions. Support for value conversions typically requires changes in the database provider.
		原因：mysql ef 相关的包对 enum 类型字段的转化问题
		解决
			还没有找到解决方法， 可以从版本问题入手
			或者 升级  net core 2.0 为netcore 2.1  
			或者 先删除DotNetCore.CAP包，等迁移后在引入 可能原因： DotNetCore.CAP对 依赖包与 mysql ef 相关的包及其依赖的包 的配合问题
			或者 Microsoft.EntityFrameworkCore.Relational 2.1.1 修改为 Microsoft.EntityFrameworkCore.Relational 2.0.1

2018-7-18
				包括Rcommend model层
				引入 DotNetCore.CAP.RabbitMQ
					先部署RabbitMQ 服务端（可延后，客户端先实现 等调试时再加入）
错误汇总
	DotNetCore.CAP.RabbitMQ  没有 
			            services.AddCap(options =>
                options.UseEntityFramework<ProjectContext>()
                    
                );
	解决：
		使用DotNetCore.CAP.Mysql

2018-7-13

	

	项目推荐服务实现  
			此项目简单实现 不会像 Project分 Project.API Project.Domain Project.Infrastructure 
			
		需求
			创建项目时发送集成事件，通过DotNetCore.CAP.RabbitMQ 发送集成事件
			推荐服务中加入 根据创建项目的projectid 及project userid 去查找 一度好友列表，并提前创建推荐信息。
				好友用户进入机会 标签时会看到推荐信息
		总体分三步
			接收其他领域服务发送的 IntegrationEvent 并使用Cap 接收并处理
				创建 IntegrationEvents 文件夹， 用于定义 外部相关集成事件 比如 ProjectCreatedIntegrationEvent
				创建 IntegrationEventHandlers 处理集成事件，如 ProjectCreatedIntegrationEventHandler
				实现 ProjectCreatedEventHandler 来处理 ProjectCreatedIntegrationEvent
				需要同步实现Project服务中的 ProjectViewedDomainEventHandler 并使用 DotNetCore.CAP.RabbitMQ 发送 IntegrationEvent
			recommend EFcore 实现 通过ef Context 插入数据库
				
				创建 Data  文件夹
					创建 RecommendDbContext
				创建Model
					
					
				引入 DotNetCore.CAP.RabbitMQ
					先部署RabbitMQ 服务端
				引入 mysql.data.entityframeworkcore
				RecommendContext
				数据库 代码段迁移到 mysql端
			编写RecommendAPI  controller, 通过UserIdentity中的UserId 获取 推荐列表
		
	
		创建model层
			创建ProjectRecommend
		创建 data层
			ProjectRecommendContext
			ProjectRecommend
		创建 controller
		
		创建IntegrationEventHandler  
			接收其他领域服务发送的IntegrationEvent  并处理
			需要加入 DotNetCore.CAP.RabbitMQ
		创建IntegrationEvent 
		
		Service
			IUserService，UserService  推荐服务接收到项目创建事件后 会根据项目创建人userId 使用UserAPI获取好友列表  这样用户进入推荐项目页面时 就能获取的推荐项目列表；
			
				需要Consul DNS 获取UserAPI的地址 来调用
				需要加入 ConsulClient ,但是推荐服务不会注册到Consul让其他服务访问。 与其他服务的通信都是通过 发布订阅 及EventBus来实现
		
		
		
		

	创建EventHandler
	实现EventHandler
	

		Command
			CreateProjectCommand:IRequest
		CommanHandler
			CreateProjectCommandHandler:IRequestHandler
		Event	(DomainEvent)
			ProjectCreatedEvent :INotification
		EventHandler (DomainEventHandler)
			ProjectViewedDomainEventHandler:INotificationHa
		DomainIntegrateEvent
		
		DomainIntegrateEventHandler




	调试ProjectCreateCommand ProjectController: CreateProject
		汇总
			ProjectRepository 的注入
				可以使用  简单的  services.AddScope<IProjectRepository, ProjectRepository>();
				或者   services.AddScope<IProjectRepository, ProjectRepository>( sp => {return new ProjectRepository(services.GetRequiredService(ProjectContext)) ;}); 	
	调试 ProjectQueries
				
08:31:35	SELECT projects.Id ,projects.Company ,projects.Avatar ,projects.ProvinceId ,projects.FinStage ,projects.FinMoney ,projects.Valution ,projects.FinPercentage ,projects.Introduction ,projects.UserId ,projects.Income ,projects.Revenue ,projects.UserName ,projects.BrokerageOption ,projectvisiblerules.Tags ,projectvisiblerules.Visible FROM  projects INNER JOIN projectvisiblerulesprojects  ON projects.Id = projectvisiblerules.ProjectId WHERE projects.id =1 and projects.UserId = 1 LIMIT 0, 1000	Error Code: 1146. Table 'finbook_beta_project.projectvisiblerulesprojects' doesn't exist	0.000 sec
				
				
2018-7-12
	调试Command
		传入简单的project 属性 Instruction，company信息， 调试CreateProjectCommand  
		ViewProject
		JoinProject 时 增加如果为自己的项目则 提示不能加入/查看自己的项目

	获取推荐列表， 加领域集成事件 （未做）

2018-7-11
	添加项目推荐服务调用 
		创建 RecommendService IRecommendService
		注入RecommendService
		RecommendService 虚拟实现
	添加 ProjectQueries 及 webcontroller 调用
			创建类
			实现类
				使用Dapper
				只取表中的部分字段信息 ，所以返回 Task<dynamic>类型
	ProjectController 中使用 ProjectQueries
		注入 ProjectQueries
		添加 GetProjects  (Route("my/{projectId}")), GetProjectDetail 
	
	IRecommendService
	
	
	注册网关 服务发现注册 添加认证服务 
		网关注册中 allowedscope:["project_api"] : 表示携带token 的scope 中需要带有 project_api 


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
	解决： 升级PowerShell
	
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
已过期 有最新方式迁移myssql efcore (使用 code first 方式)
【【dbcontext 和 model 准备
    1 创建appuserdbcontext,appuser
        data/appuserdbcontext,model/appuser
数据库准备
    mysql 中创建 finbook_metadata 数据库
    创建 finbook_test 用户 和授权
        CREATE USER 'finbook_test'@'%' IDENTIFIED BY 'root';
        GRANT ALL PRIVILEGES ON *.* TO 'finbook_test'@'%' WITH GRANT OPTION;
    设置 connectionString 为 "Server=192.168.11.83;Database=finbook_metadata;Uid=finbook_test;Pwd=root;Encrypt=true"
数据库迁移和初始化
    dotnet ef cli 安装

	创建首次迁移代码
		dotnet ef migrations add Initdb -c AppUserDbContext -o Data/migrations
		//dotnet ef migrations add Initdb --project TestEFMigration --startup-project TestEFMigration -c RecommendDbContext -o Data/Migrations
		dotnet ef database update -c AppUserDbContext
		//dotnet ef database update -c AppUserDbContext --project MysqlEFMigrate --startup-project MysqlEFMigrate
    finbook_metadata 库 初始 ef表
CREATE TABLE `__EFMigrationsHistory` 
		( 
			`MigrationId` nvarchar(150) NOT NULL, 
			`ProductVersion` nvarchar(32) NOT NULL, 
			PRIMARY KEY (`MigrationId`) 
		);
】】    


