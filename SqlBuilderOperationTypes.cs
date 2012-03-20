using System;

namespace TemplateGenerator
{
	[Flags]
    public enum SqlBuilderOperationTypes
    {
        Select = 1,
        Insert = 2,
        Update = 4,
		Delete = 8
    }
}
