# KMS Activator
### 程序简介 Introduction
&emsp;&emsp;如你所见，这是一个激活Windows和Office的小工具。Anwaert KMS Activator旨在让用户点击“一键激活”按钮即可激活当前的Windows系统或Office软件，无需关心具体的实现过程。该程序是完全开源的，开放的，完全安全和完全可重新编辑。
<br/>
&nbsp;&nbsp;&nbsp;&nbsp;As you can see, this is a small tool for activating Windows and Office. Anawaert KMS Activator is designed to allow users to activate the current Windows system or Office software with the click of a "one-click activation" button, without having to care about the specific implementation process. The program is completely open source, open, secure and re-editable.
***
### 程序原理 Main Functions
&emsp;&emsp;在现代Windows系统上，Education、Professional和Enterprise的官方发行版都提供了批量激活选项，可以在C:\Window\system32\slmgr.vbs获得许可。另一方面，使用位于Office核心安装目录下的OSPP.vbs可为Office提供批量激活。因此，只需要隐式调用这两个Vbscript文件并传递正确的命令就可以激活Windows/Office。
<br/>
&emsp;&emsp;当要执行Windows激活时，使用注册表判断当前的Windows版本,通过使用C:\Window\System32\cscript.exe并设置适当的参数后，运行slmgr.vbs即可。当要激活Office时，首先需要确定Office的版本，然后利用注册表和文件系统中的信息找到正确的Office核心安装路径，最后使用cscript.exe运行OSPP.vbs激活Office。
<br/>
&nbsp;&nbsp;&nbsp;&nbsp;On modern Windows systems, official releases of Education, Professional, and Enterprise each provide bulk activation options that can be licensed at C:\Window\system32\slmgr.vbs. Modern Office software, on the other hand, provides batch activation through OSPP.vbs license located in the Office core installation directory. Therefore, you only need to implicitly call the two Vbscript files and pass the correct command to activate Windows/Office.
<br/>
&nbsp;&nbsp;&nbsp;&nbsp;When performing the activation of Windows, you can run slmgr.vbs (in the background) by using C:\Window\system32 \cscript.exe and setting the appropriate parameters after judging the current Windows version through the registry. To activate Office, you first need to determine the version of Office, then use information in the registry and file system to find the correct Office core installation path, and finally use cscript.exe to run OSPP.vbs correctly (in the background).
<br/>
<br/>
<br/>
<br/>
***
### 系统要求 System Environment Requirement
##### 运行要求
> * 安装有.NET 6运行时
> * 不低于Windows 7 Ultimate x64版本
> * 双核2.4GHz或更高的处理器，至少1 GiB的内存大小
### 编译的系统要求 System Requirements for Compilation
> * 至少Windows 10 1809 x64的版本
> * 安装了Visual Studio 2022，且安装了.NET 6及以上的.NET SDK
> * 双核3.2 GHz或更高的处理器，至少4 GiB的内存大小
***
### 文件目录详情 Details of Directory
> ##### 主目录
> * Animations_Code <br/> 存放与UI动画相关的源文件
>	> Animations_Related.cs：与UI动画效果相关的静态函数
> * Documents <br/> 存放程序的XML文档
>	> Anawaert KMS Activator.xml
> * Images <br/> 存放程序贴图相关的文件
>	> Office19_Logo.png
>	> Win10_Logo.png
>	> KMS_Activator_LOGO.png
>	> KMS_Activator_LOGO_ICO.ico
> * Office_Activate_Code <br/> 存放与操作Office相关的源文件
>	> Office_Activate_KMS.cs：与激活Office相关的类与函数
>	> Office_Configuration_KMS.cs：与获取Office信息与状态相关的静态类与静态函数
> * RW_Configuration_Code <br/> 存放读写程序配置的源文件
>	> Config_Read.cs：与读取配置文件相关的静态函数
>	> Config_Type.cs：定义了JSON文件的序列化与反序列化的类型
>	> Config_Write.cs：与写入配置文件相关的静态函数
> * Shared_Code <br/> 存放全局共享使用的源文件
>	> Current_Config.cs：程序全局共享的程序界面配置静态变量
>	> Shared.cs：全程序集都可以使用的一些静态函数与静态变量
>	> UI_Thread_Operations.cs：关于线程UI的Invoke函数操作的封装，暂时为空
> * UI_Style_Code <br/> 存放与（控件）UI样式相关的源文件
>	> UI_Style_Related.xaml：自定义的控件样式的XAML文件，与MainWindow.xaml关联
> * Windows_Activate_Code <br/> 存放与激活Windows相关的源文件
>	> Win_Activate_KMS.cs：与Windows激活相关的类与函数
> * MainWindow.xaml、MainWindow.xaml.cs、Program.cs：程序的UI界面XAML、C#文件与入口点源文件
***
### 免责声明 Disclaimer
&emsp;&emsp;Anawaert KMS Activator的开源旨在让更多桌面开发者学习、了解如C#中Thread类、Process类，Windows系统中slmgr.vbs的使用，从而对市面上的KMS激活软件的原理、Windows的通过KMS方式激活的过程有更清晰的理解，并非鼓励大家去使用非微软官方授权的Windows或Office产品以逃避购买正版许可。该应用完全免费、开源，且将保持长期更新，任何个人与组织都不得以任何形式修改本程序的源代码后进行诸如售卖这样的商业行为，这并非Anawaert Studio的本意，因此上述商业行为与Anawaert Studio无关。Anawaert Studio对该程序发布在Github上的源代码保留有最终解释权。
&nbsp;&nbsp;&nbsp;&nbsp;Anawaert KMS Activator is open source and aims to let more desktop developers learn and understand the use of Thread class and Process class in C#, SLMgr. vbs in Windows system, and slmgr. In this way, the principle of KMS activation software on the market and the activation process of Windows through KMS are more clearly understood, rather than encouraging people to use Windows or Office products that are not officially authorized by Microsoft to avoid purchasing the genuine license. This application is completely free and open source, and will keep long-term update. Any individual or organization is not allowed to modify the source code of this program in any form for commercial activities such as selling, which is not the intention of Anawaert Studio, so the above commercial activities have nothing to do with Anawaert Studio. Anawaert Studio reserves the final right to interpret the source code of this program published on Github.
***
### 另请参阅 Extra Reference
* [Windows支持KMS方式激活版本的通用密钥 <br/> Windows supports the KMS mode activation version of the general key](https://learn.microsoft.com/zh-cn/windows-server/get-started/kms-client-activation-keys)
* [为Office批量激活的工具 <br/> The tool for managing Office bulk activations](https://learn.microsoft.com/zh-cn/deployoffice/vlactivation/tools-to-manage-volume-activation-of-office#the-osppvbs-script)
* [cscript.exe的使用方法 <br/> Usage of cscript.exe](https://learn.microsoft.com/zh-cn/previous-versions/windows/it-pro/windows-server-2012-r2-and-2012/ff920171(v=ws.11))
* [.NET 6 API参考 <br/> API reference of .NET 6](https://learn.microsoft.com/zh-cn/dotnet/api/?view=net-6.0)
