// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "By design", Scope = "type", Target = "~T:RewriteMe.WebApi.Services.Bootstrapper")]
[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "By design", Scope = "type", Target = "~T:RewriteMe.WebApi.Program")]
[assembly: SuppressMessage("Design", "CA1506:Avoid excessive class coupling", Justification = "By design", Scope = "type", Target = "~T:RewriteMe.WebApi.Startup")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design", Scope = "member", Target = "~M:RewriteMe.WebApi.Controllers.FileItemController.Create(System.String,System.String,System.String,System.Guid,Microsoft.AspNetCore.Http.IFormFile)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design", Scope = "member", Target = "~M:RewriteMe.WebApi.Controllers.FileItemController.Update(System.Guid,System.String,System.String,System.String,System.Guid,Microsoft.AspNetCore.Http.IFormFile)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design", Scope = "member", Target = "~M:RewriteMe.WebApi.Controllers.FileItemController.Upload(System.String,System.String,System.String,System.Guid,Microsoft.AspNetCore.Http.IFormFile)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]