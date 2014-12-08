// Guids.cs
// MUST match guids.h
using System;

namespace Company.VisualStudioHaskell
{
    static class GuidList
    {
        public const string guidVisualStudioHaskellPkgString = "c50cf4a9-ade0-4810-9e45-5fb2124c3cbb";
        public const string guidVisualStudioHaskellCmdSetString = "dd10264f-b92d-44e5-a466-83d2a581a02f";
        public const string guidToolWindowPersistanceString = "dbd0230e-5550-4845-8388-209abbca3617";
        public const string guidVisualStudioHaskellEditorFactoryString = "a388943a-a759-4945-826a-f41a8a1ddbea";
        public const string guidVisualStudioHaskellProjectFactoryString = "C11D8F88-2496-43A0-8758-6690DFD8C552";

        public static readonly Guid guidVisualStudioHaskellCmdSet = new Guid(guidVisualStudioHaskellCmdSetString);
        public static readonly Guid guidVisualStudioHaskellEditorFactory = new Guid(guidVisualStudioHaskellEditorFactoryString);
    };
}