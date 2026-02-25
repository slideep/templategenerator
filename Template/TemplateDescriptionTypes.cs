using System;

namespace TemplateGenerator.Template;

/// <summary>
/// Enumerators for template description types.
/// Describes what kind of class we are templating or generating.
/// </summary>
[Flags]
public enum TemplateDescriptionTypes
{
    /// <summary>
    /// No type defined (probably an error).
    /// </summary>
    None = 0,

    /// <summary>
    /// Entity class
    /// </summary>
    Entity = 1,

    /// <summary>
    /// Constant value class
    /// </summary>
    Constant = 2,

    /// <summary>
    /// Controller class
    /// </summary>
    Controller = 4,

    /// <summary>
    /// Controller class's interface
    /// </summary>
    ControllerInterface = 8,

    /// <summary>
    /// Data Access Layer (DAL) class
    /// </summary>
    DataAccess = 16,

    /// <summary>
    /// Everything (generate or template every layer's class with their interfaces).
    /// </summary>
    Everything = DataAccess | Controller | ControllerInterface | Constant | Entity
}
