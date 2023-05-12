using System.Reflection;

namespace Sadin.EmailEngine.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}