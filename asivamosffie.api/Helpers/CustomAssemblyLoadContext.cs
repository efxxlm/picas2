using System;
using System.Reflection;
using System.Runtime.Loader;

namespace asivamosffie.api.Helpers
{
    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        // con esto cargo las lib desde el ensamblado.
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }

}
