术语

opentid   oath2.0
	openid  
	
client agent resource resource owne identityserver

client
	具体形式可以是 第三方网站，或者 app client
scope 定义了可以访问的不同的resource的范围	
resource： 通常是受保护的资源 

ro: 第一次启动 或 与 identityserver 通信 并获取公钥（如果是 rsa256 形式的非对称秘钥形式），这样不需要借助identityserver 自己可以完成access token验证



auth2 有不同四种授权方式，各种方式有不同的角色参与，会稍有变化 可以认为是
	中的复杂形式 授权码模式，



不同resource  注册授权认证时可以定义不同的grantype

openid 第三方client 使用 resource 时需要一个resource owner 的 输入用户密码等信息进行授权
		但是oath2.0 提供一种 密码模式 可以client 使用用户信息进行授权
		
有identityserver 来配置 client(client中的scope) ,apiresource, user,role...

几种方式之间的 区别??
			AuthenticationScheme 与  granttype 的区别？？？？
				GrantTypes
					Implicit ClientCredentials	 ResourceOwnerPassword Code... 也可以自定义
			AuthenticationScheme
			 options.DefaultScheme =  Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
			 options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme
			 Microsoft.AspNetCore.Authentication.Facebook.FacebookDefaults.AuthenticationScheme
			 
			 
			 
			 options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
			 
			 options.DefaultAuthenticateScheme = "";options.DefaultChallengeScheme = "";options.DefaultSignInScheme = ""; 的区别？？	
				具体 
					services.AddAuthentication(options => { options.DefaultAuthenticateScheme = "";options.DefaultChallengeScheme = "";options.DefaultSignInScheme = ""; });

					
identityserver.sample 中7个项目的命名方式，	后一个项目是对前一个项目的追加  

openid和 oath 中scopes的不同 openid 针对对身份信息进行保护，oath 一般使用  apiresource 比如
	Similar to OAuth 2.0, OpenID Connect also uses the scopes concept. Again, scopes represent something you want to protect and that clients want to access. In contrast to OAuth, scopes in OIDC don’t represent APIs, but identity data like user id, name or email address.
	http://docs.identityserver.io/en/release/quickstarts/3_interactive_login.html	下得 Adding support for OpenID Connect Identity Scopes 一节			

secret
		用来验证客户端，与jwttoken的 解密秘钥没有关系
					
TBD
	openid + consent + ef + profileservice
	
			 
				

