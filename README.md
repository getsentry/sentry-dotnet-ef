The code was moved into the Sentry SDK for .NET
===========



[![AppVeyor](https://ci.appveyor.com/api/projects/status/43u1aqlj5tj33x46/branch/master?svg=true)](https://ci.appveyor.com/project/sentry/sentry-dotnet-ef/branch/master)


|      Name                 |        NuGet         |
| ----------------------------- | -------------------: |
|     **Sentry.EntityFramework**     |   [![NuGet](https://img.shields.io/nuget/vpre/Sentry.EntityFramework.svg)](https://www.nuget.org/packages/Sentry.EntityFramework)   |


This is packages extend [Sentry's .NET SDK](https://github.com/getsentry/sentry-dotnet) with Entity Framework 6 queries as *Breadcrumb*s.
It also processes `DbEntityValidationException`s to extract the validation errors and add to the *Extra* field.
This increases the debuggability of Entity Framework related errors gratefully.


![Example in Sentry](.assets/ef.PNG)

## Usage

There are 2 steps to adding Entity Framework 6 support to your project:

* Call `SentryDatabaseLogging.UseBreadcrumbs()` to either your application's startup method, or into a static constructor inside your Entity Framework object. Make sure you only call this method once! This will add the interceptor to Entity Framework to log database queries.
* When setting up your `SentryClient`, use `SentryOptions.AddEntityFramework()`. This extension method will register all error processors to extract extra data, such as validation errors, from the exceptions thrown by Entity Framework.

## Samples

You may find a usage sample using ASP.NET MVC 5 under `/samples/Sentry.Samples.AspNet.Mvc`
