using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.BrawlManagerLib.Songs
{
    public partial class SongNameBar : UserControl
    {
        private int _index;

        private ResourceNode info_pac, info_training_pac;
        private MSBinNode info, info_training;
        private string _currentFile, _currentTrainingFile;

        public class SongIndexEntry
        {
            public ushort ID;
            public int Index;

            public override string ToString()
            {
                return ID.ToString("X4") + " --> " + Index;
            }
        }

        private List<SongIndexEntry> common2_titledata;
        private HashSet<int> modifiedStringIndices;
        private List<string> fileStrings;
        private bool updateTextColor;

        public SongNameBar()
        {
            InitializeComponent();
            modifiedStringIndices = new HashSet<int>();
            fileStrings = new List<string>(265);
            updateTextColor = true;
        }

        private string TextBoxText
        {
            set
            {
                updateTextColor = false;
                textBox1.Text = value;
                updateTextColor = true;
            }
        }

        public string NegativeIndexText
        {
            set
            {
                if (Index < 0)
                    TextBoxText = value;
            }
        }

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                if (_index < 0 || info == null)
                {
                    textBox1.Enabled = button1.Enabled = button2.Enabled = false;
                    textBox1.BackColor = SystemColors.Control;
                    TextBoxText = "";
                }
                else
                {
                    TextBoxText = info._strings[_index];
                    refreshColor();
                    textBox1.Enabled = button1.Enabled = button2.Enabled = true;
                }
            }
        }

        public bool InfoLoaded => info != null;
        public bool IsDirty => modifiedStringIndices.Count > 0;

        private void refreshColor()
        {
            if (_index < 0 || info == null)
            {
                textBox1.BackColor = SystemColors.Control;
                return;
            }

            if (modifiedStringIndices.Contains(_index))
                textBox1.BackColor = Color.Wheat;
            else if (info_training != null && info_training._strings[_index] != info._strings[_index])
                textBox1.BackColor = Color.LightPink;
            else
                textBox1.BackColor = SystemColors.Window;
        }

        private void copyIntoFileStrings()
        {
            fileStrings.Clear();
            info._strings.ForEach(s => fileStrings.Add(s));
        }

        public string findInfoFile()
        {
            _index = -1;
            info = info_training = null;
            _currentFile = _currentTrainingFile = null;
            common2_titledata = new List<SongIndexEntry>();

            string tempfile = Path.GetTempFileName();

            // Try common2.pac first
            string[] sndBgmTitleDataPaths =
            {
                "..\\..\\system\\common2.pac",
                "..\\..\\system\\common2_en.pac",
                "..\\common2.pac"
            };

            foreach (string relativepath in sndBgmTitleDataPaths)
            {
                string full = Path.GetFullPath(relativepath);
                if (File.Exists(full))
                {
                    File.Copy(full, tempfile, true);
                    using (ResourceNode node = NodeFactory.FromFile(null, tempfile))
                    {
                        foreach (ResourceNode child in node.Children)
                        {
                            if (child is Common2MiscDataNode)
                            {
                                SndBgmTitleDataNode sndBgmTitleData =
                                    child.Children.FirstOrDefault() as SndBgmTitleDataNode;
                                if (sndBgmTitleData != null)
                                {
                                    common2_titledata = sndBgmTitleData.Children.Select(n => new SongIndexEntry
                                    {
                                        ID = (ushort)((SndBgmTitleEntryNode)n).ID,
                                        Index = ((SndBgmTitleEntryNode)n).SongTitleIndex
                                    }).ToList();
                                    break;
                                }
                            }
                        }
                    }
                }

                if (common2_titledata.Count > 0)
                    break;
            }

            // fallback to SongIDMap
            if (common2_titledata.Count == 0)
            {
                common2_titledata = SongIDMap.Songs.Where(s => s.InfoPacIndex != null)
                    .Select(s => new SongIndexEntry
                    {
                        ID = s.ID,
                        Index = s.InfoPacIndex ?? 0
                    }).ToList();
            }

            // Load info MSBin first
            tempfile = Path.GetTempFileName();
            if (File.Exists("Misc Data [140].msbin"))
            {
                _currentFile = "Misc Data [140].msbin";
                File.Copy(_currentFile, tempfile, true);
                info = NodeFactory.FromFile(null, tempfile) as MSBinNode;
                return "Loaded .\\Misc Data [140].msbin";
            }

            // fallback to info.pac
            string[] infopaths = { "..\\..\\info2\\info.pac", "..\\..\\info2\\info_en.pac", "..\\info.pac" };
            foreach (string path in infopaths)
            {
                if (info == null)
                {
                    string full = Path.GetFullPath(path);
                    if (File.Exists(full))
                    {
                        _currentFile = full;
                        File.Copy(full, tempfile, true);
                        info_pac = NodeFactory.FromFile(null, tempfile);
                        info = (MSBinNode)info_pac.FindChild("Misc Data [140]", true);
                    }
                }
            }

            if (info == null)
                return "No song list loaded";

            modifiedStringIndices.Clear();
            copyIntoFileStrings();

            // try info_training
            string trainingpath = _currentFile.Replace("info.pac", "info_training.pac")
                .Replace("info_en.pac", "info_training_en.pac");

            if (trainingpath != _currentFile && File.Exists(trainingpath))
            {
                _currentTrainingFile = trainingpath;
                string tempfile_training = Path.GetTempFileName();
                File.Copy(trainingpath, tempfile_training, true);
                info_training_pac = NodeFactory.FromFile(null, tempfile_training);
                info_training = (MSBinNode)info_training_pac.FindChild("Misc Data [140]", true);

                if (info_training != null && info._strings.Count != info_training._strings.Count)
                {
                    MessageBox.Show("info.pac and info_training.pac have different Misc Data [140] lengths. Ignoring info_training.pac.");
                    info_training = null;
                    info_training_pac = null;
                }
            }

            return info_training != null ? "Loaded info.pac and info_training.pac" : "Loaded info.pac";
        }

        private void updateNodeString()
        {
            if (_index < 0 || info == null)
                return;

            if (textBox1.Text != info._strings[_index])
            {
                info._strings[_index] = textBox1.Text;
                info.SignalPropertyChange();

                if (info_training != null)
                {
                    info_training._strings[_index] = textBox1.Text;
                    info_training.SignalPropertyChange();
                }
            }

            refreshColor();
        }

        public void save()
        {
            if (!IsDirty)
                return;

            // update all modified indices
            foreach (int i in modifiedStringIndices)
            {
                info._strings[i] = fileStrings[i];
                if (info_training != null)
                    info_training._strings[i] = fileStrings[i];
            }

            info.Rebuild();
            (info_pac ?? info).Merge();
            (info_pac ?? info).Export(_currentFile);

            if (info_training != null)
            {
                info_training.Rebuild();
                info_training_pac.Merge();
                info_training_pac.Export(_currentTrainingFile);
            }

            modifiedStringIndices.Clear();
            copyIntoFileStrings();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (updateTextColor)
            {
                updateNodeString();
                modifiedStringIndices.Add(_index);
                refreshColor();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && IsDirty)
            {
                DialogResult res =
                    MessageBox.Show("Overwrite info.pac" + (info_training == null ? "" : " and info_training.pac") + "?", "Saving",
                        MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                    save();
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_index >= 0 && _index < fileStrings.Count)
            {
                TextBoxText = fileStrings[_index];
                updateNodeString();
                modifiedStringIndices.Add(_index);
                refreshColor();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SongIndexEntry titleEntry = common2_titledata.FirstOrDefault(c => c.Index == _index);
            var song = SongIDMap.Songs.FirstOrDefault(s => s.ID == titleEntry?.ID);
            TextBoxText = song?.DefaultName ?? "Title index not found in common2";
            updateNodeString();
            modifiedStringIndices.Add(_index);
            refreshColor();
        }

        public int GetInfoPacIndex(ushort id)
        {
            return common2_titledata.Where(c => c.ID == id).Select(c => c.Index).DefaultIfEmpty(-1).First();
        }

        public void ExportMSBin(string path)
        {
            updateNodeString();
            info.Rebuild();
            info.Export(path);
        }

        private class MyTextBox : TextBox
        {
            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                if (e.KeyChar == (char)Keys.Enter)
                    e.Handled = true;

                base.OnKeyPress(e);
            }
        }
    }
}