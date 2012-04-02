using System;
using System.ComponentModel.DataAnnotations;

namespace TemplateGenerator.Template
{
    /// <summary>
    /// An base class for derived concrete template implementations.
    /// </summary>
    [Serializable]
    public abstract class TemplateBase
    {
        /// <summary>
        ///  Gets the name of the template.
        ///  </summary>
        [Required, DataType(DataType.Text)]
        public abstract string Name { get; protected internal set; }

        /// <summary>
        ///   Gets what kind of class/xml we are templating or generating.
        ///  </summary>
        public abstract TemplateDescriptionTypes DescriptionType { get; }

        /// <summary>
        ///  Gets the template string for the class.
        ///  </summary>
        [Required, DataType(DataType.Text)]
        public abstract string ClassTemplate { get; }

        /// <summary>
        ///  Gets the template string for the property.
        ///  </summary>
        [Required, DataType(DataType.Text)]
        public abstract string PropertyTemplate { get; }

        /// <summary>
        ///  Gets the template string for the parameter.
        ///  </summary>
        [Required, DataType(DataType.Text)]
        public abstract string ParameterTemplate { get; }

        /// <summary>
        ///  Gets the template string for the sovitus parameter.
        ///  </summary>
        [Required, DataType(DataType.Text)]
        public abstract string SovitusParameterTemplate { get; }
    }
}