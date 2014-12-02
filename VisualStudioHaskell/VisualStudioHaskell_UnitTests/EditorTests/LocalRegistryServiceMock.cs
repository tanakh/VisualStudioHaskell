/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VsSDK.UnitTestLibrary;

namespace VisualStudioHaskell_UnitTests.EditorTests {
    /// <summary>
    /// Help code to create ILocalRegistry mock
    /// </summary>
    internal static class LocalRegistryServiceMock {
        internal static BaseMock GetILocalRegistryInstance() {
            GenericMockFactory factory = new GenericMockFactory("ILocalRegistry", new Type[] { typeof(ILocalRegistry) });
            BaseMock mockObj = factory.GetInstance();
            string name = string.Format("{0}.{1}", typeof(ILocalRegistry).FullName, "CreateInstance");
            mockObj.AddMethodCallback(name, new EventHandler<CallbackArgs>(CreateInstanceCallBack));
            return mockObj;
        }

        #region Callbacks
        private static void CreateInstanceCallBack(object caller, CallbackArgs arguments) {
            // Create the output mock object for the frame
            IVsTextLines textLines = (IVsTextLines)GetIVsTextLinesInstance();
            ///GCHandle handle = GCHandle.Alloc(textLines);
            arguments.SetParameter(4, Marshal.GetComInterfaceForObject(textLines, typeof(IVsTextLines)));
            arguments.ReturnValue = VSConstants.S_OK;
        }
        #endregion

        private static BaseMock GetIVsTextLinesInstance() {
            GenericMockFactory factory = new GenericMockFactory("IVsTextLines", new Type[] { typeof(IVsTextLines), typeof(IObjectWithSite) });
            BaseMock mockObj = factory.GetInstance();
            return mockObj;
        }
    }
}
