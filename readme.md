#说明

Repos选择发送邮件功能

#部署
1. 创建数据库（名称为leansoftX.Repos），打开DataBase.sql文件复制到查询分析器，创建数据库表
2. 在部署电脑（服务器）上安装.net core3.0,详见 https://dotnet.microsoft.com/download/linux-package-manager/centos7/sdk-current
3. 在部署电脑（服务器）的iis上创建站点，并配置好端口，比如666
4. 在部署电脑（服务器）上使用localhost:666访问页面

#操作说明
1. 左侧点击选中，点击“>”移动到右边，同理“<”可以左移
2. 点击“>>”全部移动到右边，点击“<<”全部移动到左边
3. 点击“Sent”发送邮件，发送的文本会在文本框中显示

#源码结构

**开发使用.net core mvc模式**
文件夹 | 用途
---|---
wwwroot | 资源文件
Controllers | MVC控制器
Dto | 数据传输对象
Views | 视图
Lib | 公用类