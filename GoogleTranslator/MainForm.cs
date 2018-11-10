using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using System.Windows.Forms;

namespace GoogleTranslator
{
    public partial class MainForm : Form
    {
        public Setting Setting { get; private set; }
        #region Clipboard Formats

        string[] formatsAll = new string[]
        {
            DataFormats.Bitmap,
            DataFormats.CommaSeparatedValue,
            DataFormats.Dib,
            DataFormats.Dif,
            DataFormats.EnhancedMetafile,
            DataFormats.FileDrop,
            DataFormats.Html,
            DataFormats.Locale,
            DataFormats.MetafilePict,
            DataFormats.OemText,
            DataFormats.Palette,
            DataFormats.PenData,
            DataFormats.Riff,
            DataFormats.Rtf,
            DataFormats.Serializable,
            DataFormats.StringFormat,
            DataFormats.SymbolicLink,
            DataFormats.Text,
            DataFormats.Tiff,
            DataFormats.UnicodeText,
            DataFormats.WaveAudio
        };

        string[] formatsAllDesc = new String[]
        {
            "Bitmap",
            "CommaSeparatedValue",
            "Dib",
            "Dif",
            "EnhancedMetafile",
            "FileDrop",
            "Html",
            "Locale",
            "MetafilePict",
            "OemText",
            "Palette",
            "PenData",
            "Riff",
            "Rtf",
            "Serializable",
            "StringFormat",
            "SymbolicLink",
            "Text",
            "Tiff",
            "UnicodeText",
            "WaveAudio"
        };

        #endregion
        IntPtr _ClipboardViewerNext;
        Queue _hyperlink = new Queue();
        public MainForm()
        {
            InitializeComponent();
            Setting = Setting.LoadLocalSetting();
            Init();
        }

        private void Init()
        {
            _ClipboardViewerNext = User32.SetClipboardViewer(Handle);
            Location = Setting.StartupLocationPoint;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            User32.ChangeClipboardChain(Handle, _ClipboardViewerNext);
            Setting.StartupLocationPoint = Location;
            File.WriteAllText("Setting.txt", Setting.ToString());
        }

        private void Translate(String word)
        {
            WordLabel.Text = word;
            word = word.Replace(" ", "%20")
                .Replace("ü", "%C3%BC")
                .Replace("Ü", "%C3%9C")
                .Replace("ä", "%C3%A4")
                .Replace("Ä", "%C3%84")
                .Replace("ö", "%C3%B6")
                .Replace("Ö","%C3%96")
                .Replace("ß", "%C3%9F");
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={Setting.FromLanguage}&tl={Setting.ToLanguage}&dt=t&q={word}";
            var webClient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            var result = webClient.DownloadString(url);
            try
            {
                result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                MeaningLabel.Text = result;
            }
            catch
            {
                MeaningLabel.Text = @"[معنایی یافت نشد]";
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch ((Msgs)m.Msg)
            {
                case Msgs.WM_DRAWCLIPBOARD:

                    Debug.WriteLine("WindowProc DRAWCLIPBOARD: " + m.Msg, "WndProc");

                    GetClipboardData();
                    User32.SendMessage(_ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    break;
                case Msgs.WM_CHANGECBCHAIN:
                    Debug.WriteLine("WM_CHANGECBCHAIN: lParam: " + m.LParam, "WndProc");
                    if (m.WParam == _ClipboardViewerNext)
                    {
                        _ClipboardViewerNext = m.LParam;
                    }
                    else
                    {
                        User32.SendMessage(_ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;

            }

        }
        private void GetClipboardData()
        {
            IDataObject iData = new DataObject();
            string strText = "clipmon";

            try
            {
                iData = Clipboard.GetDataObject();
            }
            catch (System.Runtime.InteropServices.ExternalException externEx)
            {
                Debug.WriteLine("InteropServices.ExternalException: {0}", externEx.Message);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            if (iData.GetDataPresent(DataFormats.Rtf))
            {
                //ctlClipboardText.Rtf = (string)iData.GetData(DataFormats.Rtf);

                //if (iData.GetDataPresent(DataFormats.Text))
                //{
                //    strText = "RTF";
                //}
            }
            else
            {
                if (iData.GetDataPresent(DataFormats.Text))
                {
                    Translate(Clipboard.GetText());
                    strText = "Text";

                    Debug.WriteLine((string)iData.GetData(DataFormats.Text));
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                User32.ReleaseCapture();
                User32.SendMessage(Handle, (int)Msgs.WM_NCLBUTTONDOWN, (int)Msgs.HT_CAPTION, 0);
            }
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingButton_Click(object sender, EventArgs e)
        {
            using (var frm = new SettingForm())
            {
                frm.ShowDialog(this);
                Setting = Setting.LoadLocalSetting();
            }
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            Translate(Clipboard.GetText());
        }
    }
}
