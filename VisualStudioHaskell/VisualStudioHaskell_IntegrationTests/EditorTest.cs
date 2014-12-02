using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VsSDK.IntegrationTestLibrary;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace VisualStudioHaskell_IntegrationTests {

    [TestClass()]
    public class EditorTest {
        private delegate void ThreadInvoker();

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test for opening the editor
        ///</summary>
        [TestMethod()]
        [HostType("VS IDE")]
        public void ValidateNewFileOpenedWithEditor() {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate() {
                TestUtils testUtils = new TestUtils();
                testUtils.CloseCurrentSolution(__VSSLNSAVEOPTIONS.SLNSAVEOPT_NoSave);
                testUtils.CreateEmptySolution(TestContext.TestDir, "CreateEmptySolution");

                //Add new file to the solution and save all
                string name = "mynewfile";
                EnvDTE.DTE dte = VsIdeTestHostContext.Dte;
                EnvDTE.Window win = dte.ItemOperations.NewFile(@"Haskell editor Files\Haskell editor", name, EnvDTE.Constants.vsViewKindPrimary);
                Assert.IsNotNull(win);
                dte.ExecuteCommand("File.SaveAll", string.Empty);

                //get the currect misc files state
                object OriginalValueMiscFilesSavesLastNItems = dte.get_Properties("Environment", "Documents").Item("MiscFilesProjectSavesLastNItems").Value;
                if ((int)OriginalValueMiscFilesSavesLastNItems == 0) {
                    dte.get_Properties("Environment", "Documents").Item("MiscFilesProjectSavesLastNItems").Value = 5;
                }

                //get a handle to the project item in the solution explorer
                EnvDTE.ProjectItem item = win.Document.ProjectItem;
                Assert.IsNotNull(item);

                //close window
                win.Close(EnvDTE.vsSaveChanges.vsSaveChangesNo);

                //reset the miscfiles property if it was modified
                if (OriginalValueMiscFilesSavesLastNItems != dte.get_Properties("Environment", "Documents").Item("MiscFilesProjectSavesLastNItems").Value) {
                    dte.get_Properties("Environment", "Documents").Item("MiscFilesProjectSavesLastNItems").Value = OriginalValueMiscFilesSavesLastNItems;
                }

            });
        }

    }
}
