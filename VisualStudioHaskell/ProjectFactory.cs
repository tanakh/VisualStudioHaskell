using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Company.VisualStudioHaskell {
    [Guid(GuidList.guidVisualStudioHaskellProjectFactoryString)]
    class ProjectFactory : IVsProjectFactory {
        public ProjectFactory() {
            Console.WriteLine("TestTestTest!");
        }

        public int CanCreateProject(string pszFilename, uint grfCreateFlags, out int pfCanCreate) {
            throw new NotImplementedException();
        }

        public int Close() {
            throw new NotImplementedException();
        }

        public int CreateProject(string pszFilename, string pszLocation, string pszName, uint grfCreateFlags, ref Guid iidProject, out IntPtr ppvProject, out int pfCanceled) {
            throw new NotImplementedException();
        }

        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp) {
            throw new NotImplementedException();
        }
    }
}
