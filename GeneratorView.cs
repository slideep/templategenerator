//using System;
//using System.IO;
//using System.Text;
//using System.Windows.Forms;
//using System.Xml.Linq;
//using System.Xml.XPath;

//using TemplateGenerator.Properties;

//using ICSharpCode.TextEditor.Document;

//namespace TemplateGenerator
//{
//    public partial class GeneratorView : Form
//    {
//        private const string LuokkaPohjaPaate = ".txaClass";
                    
//        public GeneratorView()
//            : this(null)
//        {
//        }

//        public GeneratorView(GeneratorController ohjain)
//        {
//            if (ohjain == null)
//            {
//                throw new ArgumentNullException("ohjain");
//            }

//            InitializeComponent();
//            Ohjain = ohjain;

//            SisainenAlustus();
//        }

//        public XPathNavigator XmlNavigaattori { get; private set; }

//        public GeneratorController Ohjain { get; private set; }

//        protected IDescription ValittuDescription { get; set; }

//        private void SisainenAlustus()
//        {
//            this.xmlTekstiEditori.Document.DocumentChanged += Document_DocumentChanged;
//        }

//        private void Document_DocumentChanged(object sender, DocumentEventArgs e)
//        {
//            this.xmlTekstiEditori.Document.CommitUpdate();
//            this.xmlTekstiEditori.Refresh();
//        }

//        private void TogetherNakyma_Load(object sender, EventArgs e)
//        {
//            cboLuokat.DataSource = Ohjain.Descriptions;
//            cboLuokat.DisplayMember = "Name";
//            cboLuokkaPohjat.DataSource = Ohjain.Templates;
//            cboLuokkaPohjat.DisplayMember = "Name";
//        }

//        private void btnGeneroi_Click(object sender, EventArgs e)
//        {
//            if (cboLuokat.SelectedItem != null && cboLuokkaPohjat.SelectedItem != null)
//            {
//                var kuvaus = cboLuokat.SelectedItem as IDescription;
//                var malliPohja = cboLuokkaPohjat.SelectedItem as ITemplate;

//                if (kuvaus != null && malliPohja != null)
//                {
//                    tekstiEditori.Text = Ohjain.GenerateDescription(kuvaus, malliPohja.Name);
//                    if (!string.IsNullOrWhiteSpace(tekstiEditori.Text))
//                    {
//                        var generoitavaMalliPohja = cboLuokkaPohjat.SelectedItem as ITemplate;
//                        if (generoitavaMalliPohja != null)
//                        {
//                            string tiedostoNimi = LuoTiedostoNimi(kuvaus, generoitavaMalliPohja);
//                            if (tiedostoNimi != null)
//                            {
//                                if (File.Exists(tiedostoNimi))
//                                {
//                                    var tulos =
//                                        MessageBox.Show(
//                                            string.Format("Haluatko varmasti korvata aiemmin luodun tiedoston '{0}'", tiedostoNimi),
//                                            "TemplateGenerator",
//                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
//                                    switch (tulos)
//                                    {
//                                        case DialogResult.Yes:
//                                            File.WriteAllText(tiedostoNimi, tekstiEditori.Text, Encoding.Default);
//                                            break;
//                                        case DialogResult.No:
//                                            break;
//                                        default:
//                                            break;
//                                    }
//                                }
//                                else
//                                {
//                                    File.WriteAllText(tiedostoNimi, tekstiEditori.Text, Encoding.Default);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        private static string LuoTiedostoNimi(IDescription luokkaDescription, ITemplate generoituTemplate)
//        {
//            var tiedostoNimi = "";

//            switch (generoituTemplate.DescriptionType)
//            {
//                case TemplateDescriptionType.DataAccess:
//                    tiedostoNimi = string.Concat(luokkaDescription.Name, "DataAccess", ".cs");
//                    break;
//                case TemplateDescriptionType.Controller:
//                    tiedostoNimi = string.Concat(luokkaDescription.Name, "Controller", ".cs");
//                    break;
//                case TemplateDescriptionType.ConstantClass:
//                    tiedostoNimi = string.Concat(luokkaDescription.Name, "Vakiot", ".cs");
//                    break;
//                case TemplateDescriptionType.BusinessEntity:
//                    tiedostoNimi = string.Concat(luokkaDescription.Name, ".cs");
//                    break;
//                case TemplateDescriptionType.ControllerInterface:
//                    tiedostoNimi = string.Concat("I" + luokkaDescription.Name, "Controller", ".cs");
//                    break;
//            }

//            return Path.Combine(Settings.Default.LuokkapohjaHakemisto, tiedostoNimi);
//        }

//        private void chkKopioi_CheckedChanged(object sender, EventArgs e)
//        {
//            if (chkKopioi.Checked)
//            {
//            }
//        }

//        private void cboLuokat_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            var luokkaKuvaus = cboLuokat.SelectedItem as IDescription;
//            if (luokkaKuvaus != null)
//            {
//                XmlNavigaattori = LuoNavigaattori(new FileInfo(luokkaKuvaus.FileFullPath));

//                var xml = File.ReadAllText(luokkaKuvaus.FileFullPath, Encoding.Default);
//                if (!string.IsNullOrEmpty(xml))
//                {
//                    xmlTekstiEditori.Text = xml;
//                    tvMallipohja.LoadXml(xml);
//                    tvMallipohja.ExpandAll();
//                }

//                ValittuDescription = luokkaKuvaus;
//            }
//        }

//        private static XPathNavigator LuoNavigaattori(FileInfo malliPohja)
//        {
//            if (malliPohja == null)
//            {
//                throw new ArgumentNullException("malliPohja");
//            }

//            if (malliPohja.Extension.Equals(LuokkaPohjaPaate))
//            {
//                return XDocument.Load(malliPohja.FullName).CreateNavigator();
//            }

//            return null;
//        }

//        private void valitseKaikkiToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            tekstiEditori.SelectAll();
//        }

//        private void toolStripMenuItem1_Click(object sender, EventArgs e)
//        {
//            saveFileDialog1.Title = "Tallenna tiedosto nimellä";
//            saveFileDialog1.InitialDirectory = Settings.Default.LuokkapohjaHakemisto;
//            saveFileDialog1.CheckFileExists = true;
//            saveFileDialog1.CreatePrompt = true;
//            saveFileDialog1.DefaultExt = "cs";
//            saveFileDialog1.FileName = ValittuDescription.Name;

//            saveFileDialog1.ShowDialog(this);
//        }
//    }
//}