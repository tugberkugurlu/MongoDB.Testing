using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("MongoDB.Testing")]
[assembly: AssemblyDescription("MongoDB integration testing helper from .NET projects")]
[assembly: AssemblyProduct("MongoDB.Testing")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: InternalsVisibleTo("MongoDB.Testing.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#elif RELEASE
[assembly: AssemblyConfiguration("RELEASE")]
#endif