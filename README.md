<h1 align="center">
  <br>
  <img src="https://github.com/efdgdfgdf/AMS202024111207/assets/107661395/6ce72ba2-8248-4aa2-a486-6080fc91679a" width="500" />
  <br>
  AssetPro
  <br>
</h1>
<h4 align="center">基于 ASP.NET Core MVC 的固定资产管理系统设计与实现</h4>

<p align="center">
  <a href="https://img.shields.io/badge/-HTML5-E34F26?style=flat-square&logo=html5&logoColor=white">
    <img src="https://img.shields.io/badge/-HTML5-E34F26?style=flat-square&logo=html5&logoColor=white" alt="HTML5">
  </a>
  <a href="https://img.shields.io/badge/-CSS3-1572B6?style=flat-square&logo=css3"><img src="https://img.shields.io/badge/-CSS3-1572B6?style=flat-square&logo=css3" alt="CSS3"></a>
  <a href="https://img.shields.io/badge/-JavaScript-oringe?style=flat-square&logo=javascript">
      <img src="https://img.shields.io/badge/-JavaScript-oringe?style=flat-square&logo=javascript" alt="JavaScript">
  </a>
  <a href="https://img.shields.io/badge/jquery-%230769AD.svg?style=style=flat-square&logo=jquery&logoColor=white">
    <img src="https://img.shields.io/badge/jquery-%230769AD.svg?style=style=flat-square&logo=jquery&logoColor=white" alt="jQuery"/>
  </a>
  <a href="https://img.shields.io/badge/Bootstrap5.2.3-%2300f.svg?style=flat-square&logo=bootstrap&logoColor=white">
     <img src="https://img.shields.io/badge/Bootstrap5.2.3-%2300f.svg?style=flat-square&logo=bootstrap&logoColor=white" alt="Bootstrap"/>
  </a>
  <a href="https://img.shields.io/badge/csharp-239120?style=flat-square&logo=csharp&logoColor=white">
     <img src="https://img.shields.io/badge/csharp-239120?style=flat-square&logo=csharp&logoColor=white" alt="C#"/>
  </a>
  <a href="https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white">
     <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white" alt="SQL Server"/>
  </a>
  <a href="https://img.shields.io/badge/Windows-0078D6?style=flat-square&logo=windows&logoColor=white">
     <img src="https://img.shields.io/badge/Windows-0078D6?style=flat-square&logo=windows&logoColor=white" alt="Windows"/>
  </a>
  <a href="https://img.shields.io/badge/-Docker-FCC624?style=flat-square&logo=docker">
     <img src="https://img.shields.io/badge/-Docker-FCC624?style=flat-square&logo=docker" alt="Docker"/>
  </a>
  <a href="https://img.shields.io/badge/Visual%20Studio%202022-5C2D91?style=flat-square&logo=visualstudio&logoColor=white">
     <img src="https://img.shields.io/badge/Visual%20Studio%202022-5C2D91?style=flat-square&logo=visualstudio&logoColor=white" alt="visualstudio"/>
  </a>
</p>

## 系统开发目的：
固定资产管理（Fixed Assets Management）是指对企业固定资产的使用计划、购置过程、验收过程、登记工作、领用工作、使用过程、维修过程、报废处置等过程的科学性管理。而透过合适的软件协助管理更能使前述工作顺利进行。本实验将建立一个简易的 WebBased 固定资产管理系统，以管理企业内的各种固定资产（设备），记录设备的状况及保管人信息，以提供企业内部了解设备的保管状况并可进一步实施设备资源的共享，以避免不必要的重复购入相同设备并达成资源共享理念。

## 功能性需求：
* 固定资产（设备）可由管理人进行分类管理，如将高校的固定资产分为电脑类、网络类、空调类及教学类。所有固定资产均应纳入某个类别，只能属于某一个类别。资产类别及资产名称请合理自订。每项资产必须使用 1 张图片表示，以方便用户管理。
* 员工分系统管理人及一般员工，所有人员都可进行资产保管。人员均需编列于特定部门中，如财务部或销售部，一般企业部门名称请自订。
* 须有用户登录功能，所有人员使用系统需先行登陆，并按人员身份（角色）操作适当的功能（页面），如只有系统管理人可添加资产。
* 系统管理人可作 A.资产类别，B.资产，C.部门，D.员工的ＣＲＵＤ；其他人员只能进行各项查询。
* 查询功能：能分别按下列条件查询资产，包含：A.按资产编号、B.按资产名称、C.按资产类别、D.按购入日期（年度）、E.按存放位置、F.资产保管人姓名、G.按部门名称。
* 一般游客只能看到登录页，不能操作任何功能或页面。

##  数据库设计要求：
* 资产类别：类别代号(PK)、类别名称、类别说明
* 资产：资产编号(PK)、资产名称、资产规格、价格、购入日期、存放位置、资产类别(FK) 、资产图片、资产保管人(FK)。
* 部门：部门代号(PK)、部门名称、部门主管(FK)。
* 员工：员工编号(PK)（作为登陆用）、密码、姓名、联络电话、是否为管理人(以角色表示)、部门代号 (FK)。
* 资产存放地点并不是部门，是否另设实体（数据表）可自行决定。
* 需有Ｅ－Ｒ图设计，并请合理设计上述数据型态及限制。
* 各项命名均需使用英文，所有字段名亦同。英文使用请用意译不要用音译。
![image](https://github.com/efdgdfgdf/AMS202024111207/assets/107661395/ede1d0ae-e6fe-4d9c-9634-89c3489c9e1b)

## EF构建模式
Database First（数据库优先）—— 由数据库生成模型以及实体类（Entity Class）。

## 功能特性
- [x] 首页轮播图
- [x] Layout设置固定导航栏class属性：sticky-top
- [x] 查询不到数据，显示打印机效果：typed.js
- [x] 点击图片放大：Fancybox
- [x] 导出为Excel表格（第三方依赖包实现）
- [x] 首页弹窗提示：sweetalert2插件
- [x] 增删改有取消或重置按钮
- [x] 根据字段选择精确或模糊查询
- [x] 数据分页显示
- [x] 数据排序
- [x] 批量删除数据信息
- [x] 展示所有用户信息——通过API获取QQ头像显示
- [x] 登录界面密码错误有提示，以及能重置密码（TempData和ViewBag的区别）
- [x] 注册功能
- [x] 更改用户名或密码需要重新登录
- [x] 记录显示个人资产和资产类别的数量、所属部门员工数
- [x] 增删改查要做好捕获异常处理try...catch，保证数据操作的正确性与稳定性
- [x] 关于页面，统计信息
- [ ] 注册需审核机制
- [ ] Docker环境下部署项目
- [x] 导航栏响应式美化 

## 发现问题：
- [x] 删除数据时应先删除有关联表的数据（如外键约束），需做好异常处理
- [x] 修改或删除登录中用户的用户名和密码，应该退出重新登录
- [x] 模糊匹配查询Int、String、Date等类型的数据，运用好contains()、Equal()
>更多问题，可查看我的[**问答社区**](https://solvio.zeabur.app/)，欢迎来提问或者解答👋🤝

## 知识点：

##### Include 和 AsNoTracking 是两个不同的方法，它们的作用也不同。
```
1. Include 方法是用于指定需要关联查询的导航属性，例如 Include(a => a.Category) 表示查询 Asset 实体时同时查询和 Asset 关联的 Category 实体。
2. AsNoTracking 方法则是表示查询结果不需要被跟踪，也就是不需要在内存中缓存实体，在极大程度上提高了查询性能。当你进行一些只读操作且对 Entity Framework 进行插入、更新和删除的情况比较少时，可以在查询时使用该方法。
所以说，是否需要使用 AsNoTracking 取决于你的业务需求和设计。如果你已经通过 Include 把所有需要的相关实体都包含进来了，并且在查询过程中不需要对这些实体进行编辑，那么就可以使用 AsNoTracking 来避免额外的开销。如果你需要在后续的操作中对这些实体进行修改，那么就不能使用 AsNoTracking。总结起来，应该基于业务需求谨慎决定是否需要使用 AsNoTracking，而使用 Include 则是必要的，因为它是实现查询关联实体的重要手段。
```

##### 数据常用的注释属性：
```
类文件的顶部需引入：using System.ComponentModel.DataAnnotations;
[DataType(DataType.Date)]
[Display(Name = "QQ邮箱")]
[Required(ErrorMessage = "必须填写")]
[EmailAddress(ErrorMessage = "请输入有效的电子邮箱地址")]
[Phone(ErrorMessage = "请输入有效的电话号码")]
[RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "请输入有效的手机号码")]
[RegularExpression(@"^(?=.*?\d)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[^\w\s]).{8,}$", ErrorMessage = "密码必须有至少一个数字、一个小写字母、一个大写字母和一个特殊字符，并且长度至少为 8 个字符。")]
```

## 相关插件说明：
##### （一）使用前端插件用于图片放大预览，这里我使用[**Fancybox**](https://fancyapps.com/fancybox/getting-started/)
1. Magnific Popup
  Magnific Popup 是一个功能强大的 jQuery 插件，可用于实现图片的缩放、放大和轮播等功能。它支持各种类型的媒体，包括图像、视频、音频等，并且易于使用和自定义。
2. Lightbox
  Lightbox 是一个非常流行的 jQuery 插件，它可以用于创建响应式的图片和视频库。通过点击页面中的图片，Lightbox 会将其展示在一个浮动框中，同时模糊化页面背景，使得图片可以更加突出的展示。
3. Fancybox
  Fancybox 是另一个非常流行的 jQuery 插件，它可以用于创建相册、画廊或媒体展示。通过点击页面中的图片，Fancybox 会在屏幕中央打开一个模态框，包含放大后的图片以及一些控制选项。
以上是一些常用的前端图片放大预览插件，它们都具有自定义选项、配置灵活、易于使用等特点，可以根据项目需求进行选择和调整。


##### （二）[**SweetAlert2**](https://sweetalert2.github.io/)，一个美观、灵活、可定制的弹框插件，用于代替原生 JavaScript 的 alert、confirm 和 prompt 弹框

在使用 SweetAlert2 之前，您需要引入 SweetAlert2 的 CSS 文件和 JS 文件：
```html
<!-- 引入 CSS 文件 -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11.1.0/dist/sweetalert2.min.css">
<!-- 引入 JS 文件 -->
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.1.0/dist/sweetalert2.all.min.js"></script>
```

使用 SweetAlert2 的基本语法如下：
```javascript
Swal.fire({
  title: '提示',
  text: '这是一个提示框。',
  icon: 'info',
  confirmButtonText: '确定'
});
```
该代码将显示一个消息提示框，其中包含标题、文本、图标和确认按钮。其中，title 是提示框的标题，text 是提示框的文本内容，icon 是提示框的图标，confirmButtonText 是确认按钮的文本。
SweetAlert2 还支持许多其它选项，例如自定义按钮、输入框等。

##### （三）打字机效果插件typed.js
使用教程：https://juejin.cn/post/7020208396271878152



