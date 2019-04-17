consul 安装
consul 资料
	开始入门： https://www.consul.io/intro/index.html
	
问题
	- consul agent -dev 命令提示: ==> Starting Consul agent...
	==> Error starting agent: Failed to start Consul server: 
	Failed to start RPC layer: listen tcp 127.0.0.1:8300: bind: An attempt was made to access a socket in a way forbidden by its access permissions.

	参考资料
		https://appuals.com/fix-an-attempt-was-made-to-access-a-socket-in-a-way-forbidden-by-its-access-permissions/

问题：An attempt was made to access a socket in a way forbidden by its access permissions.
	常见解决方案
	1 VPN client  的连接 阻止
	2 application is attempt open a port that is already used
		使用  netstat 命令  netstat -an [-p TCP]
	3 Windows 10 security feature
	4 Another process is listening on the desired port

