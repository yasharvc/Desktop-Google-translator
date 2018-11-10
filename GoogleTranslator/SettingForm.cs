using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleTranslator
{
    public partial class SettingForm : Form
    {
        List<LanguageAbbrivation> FromLanguages = new List<LanguageAbbrivation>()
        {
            new LanguageAbbrivation("English","en"),
            new LanguageAbbrivation("German","de"),
            new LanguageAbbrivation("فارسی","fa"),
            new LanguageAbbrivation("Türkiye","tr"),
            new LanguageAbbrivation("Italy","it")
        };
        List<LanguageAbbrivation> ToLanguages = new List<LanguageAbbrivation>()
        {
            new LanguageAbbrivation("English","en"),
            new LanguageAbbrivation("German","de"),
            new LanguageAbbrivation("فارسی","fa"),
            new LanguageAbbrivation("Türkiye","tr"),
            new LanguageAbbrivation("Italy","it")
        };
        private Setting Setting { get; set; }
        public SettingForm()
        {
            InitializeComponent();
            Setting = Setting.LoadLocalSetting();
            FromLanguageComboSetting();
            ToLanguageComboSetting();
        }

        private void ToLanguageComboSetting()
        {
            ToLanguageCombo.DataSource = ToLanguages;
            ToLanguageCombo.DisplayMember = nameof(LanguageAbbrivation.Title);
            ToLanguageCombo.ValueMember = nameof(LanguageAbbrivation.Abbrivation);
            ToLanguageCombo.SelectedValue = Setting.ToLanguage;
        }

        private void FromLanguageComboSetting()
        {
            FromLanguageCombo.DataSource = FromLanguages;
            FromLanguageCombo.DisplayMember = nameof(LanguageAbbrivation.Title);
            FromLanguageCombo.ValueMember = nameof(LanguageAbbrivation.Abbrivation);
            FromLanguageCombo.SelectedValue = Setting.FromLanguage;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Setting.FromLanguage = FromLanguageCombo.SelectedValue.ToString();
            Setting.ToLanguage = ToLanguageCombo.SelectedValue.ToString();
            Setting.SaveLocalSetting(Setting);
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
