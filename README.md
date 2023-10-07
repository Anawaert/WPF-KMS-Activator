# KMS Activator
### 程序简介 Introduction
##### &emsp;&emsp;如你所见，这是一个激活Windows和Office的小工具。Anwaert KMS Activator只在让用户点击“一键激活”按钮即可激活当前的Windows系统或Office软件，无需关心具体的实现过程。该程序是完全开源的，开放的，完全安全和完全可重新编辑。
##### &nbsp;&nbsp;&nbsp;&nbsp;As you can see, this is a small tool for activating Windows and Office. Anawaert KMS Activator is designed to allow users to activate the current Windows system or Office software with the click of a "one-click activation" button, without having to care about the specific implementation process. The program is completely open source, open, secure and re-editable.
***
### 程序原理 Main functions
##### &emsp;&emsp;在现代Windows系统上，Education、Professional和Enterprise的官方发行版都提供了批量激活选项，可以在C:\Window\system32\slmgr.vbs获得许可。另一方面，使用位于Office核心安装目录下的OSPP.vbs可为Office提供批量激活。因此，只需要隐式调用这两个Vbscript文件并传递正确的命令就可以激活Windows/Office。
##### &emsp;&emsp;当要执行Windows激活时，使用注册表判断当前的Windows版本,通过使用C:\Window\System32\cscript.exe并设置适当的参数后，运行slmgr.vbs即可。当要激活Office时，首先需要确定Office的版本，然后利用注册表和文件系统中的信息找到正确的Office核心安装路径，最后使用cscript.exe运行OSPP.vbs激活Office。
##### &emsp;&emsp;当然，附加功能如“自动更新”和“自动检测更新”，将分别通过C:\Windows\System32\schtasks.exe和读取Github上此存储库的发布信息来实现。自动更新是为了让Windows/Office长期处于活跃状态，而自动更新则是为了保证bug能够及时修复，新的功能和特性能够不断添加，带来更好的体验
##### &nbsp;&nbsp;&nbsp;&nbsp;On modern Windows systems, official releases of Education, Professional, and Enterprise each provide bulk activation options that can be licensed at C:\Window\system32\slmgr.vbs. Modern Office software, on the other hand, provides batch activation through OSPP.vbs license located in the Office core installation directory. Therefore, you only need to implicitly call the two Vbscript files and pass the correct command to activate Windows/Office.
##### &nbsp;&nbsp;&nbsp;&nbsp;When performing the activation of Windows, you can run slmgr.vbs (in the background) by using C:\Window\system32 \cscript.exe and setting the appropriate parameters after judging the current Windows version through the registry. To activate Office, you first need to determine the version of Office, then use information in the registry and file system to find the correct Office core installation path, and finally use cscript.exe to run OSPP.vbs correctly (in the background).
##### &nbsp;&nbsp;&nbsp;&nbsp;Additional features such as "auto-renew" and "auto-detect updates" will be implemented by C:\Windows\System32\schtasks.exe and by reading the release information of this repository on Github, respectively. Automatic renewal is to keep Windows/Office active for a long time, and automatic update is to ensure that bugs can be fixed in time and new functions and features can be continuously added, bringing a better experience.
***
### 文件目录详情 Details of the directory
> ##### 主目录
> * Animations_Code <br/> 存放与UI动画相关的源文件
>	> ~~Animations_Related.cs~~：已弃用，集成至MainWindow.xaml.cs中
> * Images <br/> 存放程序贴图相关的文件
>	> Office19_Logo.png
>	> Win10_Logo.png
> * Office_Activate_Code <br/> 存放与操作Office相关的源文件
>	> Office_Activate_Related.cs：与激活Office相关的类与函数
>	> Office_Configuration_Related.cs：与获取Office信息与状态相关的静态类与静态函数
> * Shared_Code <br/> 存放全局共享使用的源文件
>	> Shared.cs：全程序集都可以使用的一些静态函数与静态变量
> * Windows_Activate_Code <br/> 存放与激活Windows相关的源文件
>	> Win_Activate_Related.cs：与Windows激活相关的类与函数
***
### 另请参阅 Extra Reference
* [为Office批量激活的工具 <br/> The tool for managing Office bulk activations](https://learn.microsoft.com/zh-cn/deployoffice/vlactivation/tools-to-manage-volume-activation-of-office#the-osppvbs-script)
* [cscript.exe的使用方法 <br/> Usage of cscript.exe](https://learn.microsoft.com/zh-cn/previous-versions/windows/it-pro/windows-server-2012-r2-and-2012/ff920171(v=ws.11))
* [.NET 6 API参考 <br/> API reference of .NET 6](https://learn.microsoft.com/zh-cn/dotnet/api/?view=net-6.0)
