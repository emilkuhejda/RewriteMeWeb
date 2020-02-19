// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "By design", Scope = "type", Target = "~T:RewriteMe.WebApi.Services.Bootstrapper")]
[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "By design", Scope = "type", Target = "~T:RewriteMe.WebApi.Program")]
[assembly: SuppressMessage("Design", "CA1506:Avoid excessive class coupling", Justification = "By design", Scope = "type", Target = "~T:RewriteMe.WebApi.Startup")]
[assembly: SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "By design", Scope = "type", Target = "~T:RewriteMe.WebApi.Filters.ApiExceptionFilter")]
[assembly: SuppressMessage("Design", "CA1019:Define accessors for attribute arguments", Justification = "By design", Scope = "member", Target = "~M:RewriteMe.WebApi.Filters.ApiExceptionFilter.#ctor(RewriteMe.Domain.Interfaces.Services.IApplicationLogService)")]
[assembly: SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "By design", Scope = "type", Target = "~T:RewriteMe.WebApi.Filters.ApiExceptionFilter")]
