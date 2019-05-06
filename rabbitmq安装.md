1 
https://blog.csdn.net/qq_26718271/article/details/76577356#

安装 erlang 和 rabbitmq


2 版本选择
https://www.rabbitmq.com/which-erlang.html

git上各个版本信息 包括源码及 各系统编译后的软件包 ： https://github.com/rabbitmq/rabbitmq-server/releases

3 rabbitmq 3.7.14  erlang 21.03



4 问题
The filename, directory name, or volume label syntax is incorrect.

解决：
	第一步
		Remove RabbitMQ and Erlang
		Remove registry entries under HKLM/SOFTWARE/Ericsson/Erlang/ErlSrv.
		Remove all .erlang.cookie (possibly in %HOMEDRIVE%%HOMEPATH% and %SYSTEMROOT%)
		Install Erlang then RabbitMQ WITH ADMIN USER.
		Make sure in system environment, ERLANG_HOME with C:\Program Files\erlyour version number exist. If not, create.
		Run rabbitmq-plugins enable rabbitmq_management from RabbitMQ sbin folder	
	第二步 重启
	第3步
	
	https://stackoverflow.com/questions/38900125/windows-could-not-start-the-rabbitmq-service-on-local-computer
	第4步 参考
		https://github.com/rabbitmq/rabbitmq-server/issues/625
		
具体原因不清楚，安装 上面步骤 可以解决