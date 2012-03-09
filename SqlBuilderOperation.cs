using System;

namespace TemplateGenerator
{
	[Flags]
    public enum SqlBuilderOperation
    {
        Select = 1,
        Insert = 2,
        Update = 4,
		Delete = 8
    }
}
