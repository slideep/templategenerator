using System.Collections.Generic;

namespace TemplateGenerator
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
            get
            {
                if (_instance == null)
                {
                    _instance = new TemplateStorage();
                }

                return _instance;
            }
            set { _instance = value; }
        }

        public IDictionary<string, ITemplate> Templates
        {
            get { return Storage; }
        }
    }
}