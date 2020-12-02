# Wpf.Common
# 此项目仅支持.net framework 4.5+ ，仅供学习参考用，将来会考虑发布到nuget
欢迎使用Wpf.Common，项目构成如下：
库名称  | 说明  | 依赖关系
 ---- | ----- | ------  
 Wpf.Common.Core  | 轻量级的库，仅对PresentationCore库的常用类型进行扩展或实现 |  PresentationCore
 Wpf.Common  | 提供一些wpf常用控件的附加行为及扩展、辅助方法 | PresentationCore,PresentationFramework,Wpf.Common.Core
 Wpf.Common.Controls  | 提供wpf常用控件的样式及一些自定义控件 | PresentationCore,PresentationFramework,Wpf.Common.Core,Wpf.Common
 Wpf.Common.Demo  | 提供一些样例 | PresentationCore,PresentationFramework,Wpf.Common.Core,Wpf.Common,Wpf.Common.Controls
