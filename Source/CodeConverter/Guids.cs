// Guids.cs
// MUST match guids.h
using System;

namespace LevelUp.CodeConverter
{
    static class GuidList
    {
        public const string guidCodeConverterPkgString = "4e5ea1a3-9490-4c9d-9acb-17220ada8c4d";
        public const string guidCodeConverterCmdSetString = "6fc81f32-11fa-4e62-92b0-c3c95523526b";

        public static readonly Guid guidCodeConverterCmdSet = new Guid(guidCodeConverterCmdSetString);
    };
}