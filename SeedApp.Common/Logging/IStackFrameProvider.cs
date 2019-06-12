using System;

namespace SeedApp.Common.Logging
{
    public interface IStackFrameProvider
    {
        string GetCallerFullTypeName(int? skipFrames = null);

        Type GetCallerFullType(int? skipFrames = null);
    }
}