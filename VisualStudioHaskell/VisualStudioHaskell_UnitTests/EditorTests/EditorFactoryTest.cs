/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Reflection;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.UnitTestLibrary;
using Company.VisualStudioHaskell;

namespace VisualStudioHaskell_UnitTests.EditorTests {
    [TestClass()]
    public class EditorFactoryTest {
        [TestMethod()]
        public void CreateInstance() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();

            EditorFactory editorFactory = new EditorFactory(package);
            Assert.IsNotNull(editorFactory, "Failed to initialize new instance of EditorFactory.");
        }

        [TestMethod()]
        public void IsIVsEditorFactory() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();
            EditorFactory editorFactory = new EditorFactory(package);
            Assert.IsNotNull(editorFactory as IVsEditorFactory, "The object does not implement IVsEditorFactory");
        }

        [TestMethod()]
        public void SetSite() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();

            //Create the editor factory
            EditorFactory editorFactory = new EditorFactory(package);

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();

            // Site the editor factory
            Assert.AreEqual(0, editorFactory.SetSite(serviceProvider), "SetSite did not return S_OK");
        }

        [TestMethod()]
        public void CreateEditorInstance() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();

            //Create the editor factory
            EditorFactory editorFactory = new EditorFactory(package);

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();
            serviceProvider.AddService(typeof(SLocalRegistry), (ILocalRegistry)LocalRegistryServiceMock.GetILocalRegistryInstance(), true);

            // Site the editor factory
            Assert.AreEqual(0, editorFactory.SetSite(serviceProvider), "SetSite did not return S_OK");

            IntPtr ppunkDocView;
            IntPtr ppunkDocData;
            string pbstrEditorCaption = String.Empty;
            Guid pguidCmdUI = Guid.Empty;
            int pgrfCDW = 0;
            Assert.AreEqual(VSConstants.S_OK, editorFactory.CreateEditorInstance(VSConstants.CEF_OPENFILE,
                null, null, null, 0, IntPtr.Zero, out ppunkDocView, out ppunkDocData, out pbstrEditorCaption,
                out pguidCmdUI, out pgrfCDW));
        }

        [TestMethod()]
        public void CheckLogicalView() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();

            //Create the editor factory
            EditorFactory editorFactory = new EditorFactory(package);

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();
            serviceProvider.AddService(typeof(SLocalRegistry), (ILocalRegistry)LocalRegistryServiceMock.GetILocalRegistryInstance(), true);

            // Site the editor factory
            Assert.AreEqual(0, editorFactory.SetSite(serviceProvider), "SetSite did not return S_OK");

            IntPtr ppunkDocView;
            IntPtr ppunkDocData;
            string pbstrEditorCaption = String.Empty;
            Guid pguidCmdUI = Guid.Empty;
            int pgrfCDW = 0;
            Assert.AreEqual(VSConstants.S_OK, editorFactory.CreateEditorInstance(VSConstants.CEF_OPENFILE,
                null, null, null, 0, IntPtr.Zero, out ppunkDocView, out ppunkDocData, out pbstrEditorCaption,
                out pguidCmdUI, out pgrfCDW));          //check for successfull creation of editor instance

            string bstrPhysicalView = string.Empty;
            Guid refGuidLogicalView = VSConstants.LOGVIEWID_Debugging;
            Assert.AreEqual(VSConstants.E_NOTIMPL, editorFactory.MapLogicalView(ref refGuidLogicalView, out bstrPhysicalView));

            refGuidLogicalView = VSConstants.LOGVIEWID_Code;
            Assert.AreEqual(VSConstants.E_NOTIMPL, editorFactory.MapLogicalView(ref refGuidLogicalView, out bstrPhysicalView));

            refGuidLogicalView = VSConstants.LOGVIEWID_TextView;
            Assert.AreEqual(VSConstants.S_OK, editorFactory.MapLogicalView(ref refGuidLogicalView, out bstrPhysicalView));

            refGuidLogicalView = VSConstants.LOGVIEWID_UserChooseView;
            Assert.AreEqual(VSConstants.E_NOTIMPL, editorFactory.MapLogicalView(ref refGuidLogicalView, out bstrPhysicalView));

            refGuidLogicalView = VSConstants.LOGVIEWID_Primary;
            Assert.AreEqual(VSConstants.S_OK, editorFactory.MapLogicalView(ref refGuidLogicalView, out bstrPhysicalView));
        }

        /// <summary>
        /// Verify that the object implements the IDisposable interface
        /// </summary>
        [TestMethod()]
        public void IsIDisposableTest() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();

            using (EditorFactory editorFactory = new EditorFactory(package)) {
                Assert.IsNotNull(editorFactory as IDisposable, "The object does not implement IDisposable interface");
            }
        }

        /// <summary>
        /// Object is destroyed deterministically by Dispose() method call test
        /// </summary>
        [TestMethod()]
        public void DisposeTest() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();

            EditorFactory editorFactory = new EditorFactory(package);
            editorFactory.Dispose();
        }

        /// <summary>
        /// Check that all disposable members are disposed after Dispose method call
        /// </summary>
        [TestMethod()]
        public void DisposeDisposableMembersTest() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();

            EditorFactory editorFactory = new EditorFactory(package);
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();
            editorFactory.SetSite(serviceProvider);
            object service = editorFactory.GetService(typeof(IProfferService));
            Assert.IsNotNull(service);
            editorFactory.Dispose(); //service provider contains no services after this call
            service = editorFactory.GetService(typeof(IProfferService));
            Assert.IsNull(service, "serviceprovider has not beed disposed as expected");
        }

        /// <summary>
        ///A test for Close ()
        ///</summary>
        [TestMethod()]
        public void CloseTest() {
            VisualStudioHaskellPackage package = new VisualStudioHaskellPackage();

            EditorFactory editorFactory = new EditorFactory(package);
            Assert.AreEqual(VSConstants.S_OK, editorFactory.Close(), "Close did no return S_OK");
        }
    }
}