using System.Collections.Generic;

namespace TemplateGenerator.Template
{
    internal class TemplateStorage
    {
        private static TemplateStorage _instance;

        public TemplateStorage()
        {
            Storage = new Dictionary<string, ITemplate>();
        }

        public IDictionary<string, ITemplate> Storage { get; private set; }

        public static TemplateStorage Instance
        {
            get { return _instance ?? (_instance = new TemplateStorage()); }
        }

        public IDictionary<string, ITemplate> Templates
        {
            get { return Storage; }
        }
    }
}