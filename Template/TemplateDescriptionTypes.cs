using System;

namespace TemplateGenerator.Template
{
    [Flags]
    public enum TemplateDescriptionTypes
    {
        None = 0,
        BusinessEntity = 1,
        ConstantClass = 2,
        Controller = 4,
        ControllerInterface = 8,
        DataAccess = 16,
        Everything = DataAccess & Controller & ControllerInterface & ConstantClass & BusinessEntity
    }
}