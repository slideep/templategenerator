using System;

namespace TemplateGenerator
{
    [Flags]
    public enum TemplateDescriptionType
    {
        None = 1,
        BusinessEntity = 2,
        ConstantClass = 4,
        Controller = 8,
        ControllerInterface = 16,
        DataAccess = 32,
        Everything = DataAccess & Controller & ControllerInterface & ConstantClass & BusinessEntity
    }
}