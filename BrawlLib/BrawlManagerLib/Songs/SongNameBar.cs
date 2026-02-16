using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Interactivity;

namespace BrawlInstaller.Views
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

            public override string ToString() => ID.ToString("X4") + " --> " + Index;
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
                    textBox1.IsEnabled = button1.IsEnabled = button2.IsEnabled = false;
                    textBox1.Background = Brushes.LightGray;
                    TextBoxText = "";
                }
                else
                {
                    TextBoxText = info._strings[_index];
                    RefreshColor();
                    textBox1.IsEnabled = button1.IsEnabled = button2.IsEnabled = true;
                }
            }
        }

        public bool InfoLoaded => info != null;
        public bool IsDirty => modifiedStringIndices.Count > 0;

        private void RefreshColor()
        {
            if (_index < 0 || info == null)
            {
                textBox1.Background = Brushes.LightGray;
                return;
            }

            if (modifiedStringIndices.Contains(_index))
                textBox1.Background = Brushes.Wheat;
            else if (info_training != null && info_training._strings[_index] != info._strings[_index])
                textBox1.Background = Brushes.LightPink;
            else
                textBox1.Background = Brushes.White;
        }

        private void CopyIntoFileStrings()
        {
            fileStrings.Clear();
            fileStrings.AddRange(info._strings);
        }

        public string FindInfoFile()
        {
            _index = -1;
            info = info_training = null;
            _currentFile = _currentTrainingFile = null;
            common2_titledata = new List<SongIndexEntry>();

            string tempfile = Path.GetTempFileName();
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
                                var sndBgmTitleData = child.Children.FirstOrDefault() as SndBgmTitleDataNode;
                                if (sndBgmTitleData != null)
                                {
                                    common2_titledata = sndBgmTitleData.Children
                                        .Select(n => new SongIndexEntry
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

            if (common2_titledata.Count == 0)
            {
                common2_titledata = SongIDMap.Songs.Where(s => s.InfoPacIndex != null)
                    .Select(s => new SongIndexEntry
                    {
                        ID = s.ID,
                        Index = s.InfoPacIndex ?? 0
                    }).ToList();
            }

            tempfile = Path.GetTempFileName();
            if (File.Exists("Misc Data [140].msbin"))
            {
                _currentFile = "Misc Data [140].msbin";
                File.Copy(_currentFile, tempfile, true);
                info = NodeFactory.FromFile(null, tempfile) as MSBinNode;
                return "Loaded .\\Misc Data [140].msbin";
            }

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
                        info = info_pac.FindChild("Misc Data [140]", true) as MSBinNode;
                    }
                }
            }

            if (info == null)
                return "No song list loaded";

            modifiedStringIndices.Clear();
            CopyIntoFileStrings();

            string trainingpath = _currentFile.Replace("info.pac", "info_training.pac")
                .Replace("info_en.pac", "info_training_en.pac");

            if (trainingpath != _currentFile && File.Exists(trainingpath))
            {
                _currentTrainingFile = trainingpath;
                string tempfile_training = Path.GetTempFileName();
                File.Copy(trainingpath, tempfile_training, true);
                info_training_pac = NodeFactory.FromFile(null, tempfile_training);
                info_training = info_training_pac.FindChild("Misc Data [140]", true) as MSBinNode;

                if (info_training != null && info._strings.Count != info_training._strings.Count)
                {
                    var dlg = new Window { Title = "Warning" };
                    // For simplicity, just write to console in this example
                    Console.WriteLine("info.pac and info_training.pac have different lengths. Ignoring info_training.pac.");
                    info_training = null;
                    info_training_pac = null;
                }
            }

            return info_training != null ? "Loaded info.pac and info_training.pac" : "Loaded info.pac";
        }

        private void UpdateNodeString()
        {
            if (_index < 0 || info == null) return;

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

            RefreshColor();
        }

        public void Save()
        {
            if (!IsDirty) return;

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
            CopyIntoFileStrings();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (updateTextColor)
            {
                UpdateNodeString();
                modifiedStringIndices.Add(_index);
                RefreshColor();
            }
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && IsDirty)
            {
                // Show a dialog using Avalonia Window logic (placeholder)
                Save();
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (_index >= 0 && _index < fileStrings.Count)
            {
                TextBoxText = fileStrings[_index];
                UpdateNodeString();
                modifiedStringIndices.Add(_index);
                RefreshColor();
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            SongIndexEntry titleEntry = common2_titledata.FirstOrDefault(c => c.Index == _index);
            var song = SongIDMap.Songs.FirstOrDefault(s => s.ID == titleEntry?.ID);
            TextBoxText = song?.DefaultName ?? "Title index not found in common2";
            UpdateNodeString();
            modifiedStringIndices.Add(_index);
            RefreshColor();
        }

        public int GetInfoPacIndex(ushort id) =>
            common2_titledata.Where(c => c.ID == id).Select(c => c.Index).DefaultIfEmpty(-1).First();

        public void ExportMSBin(string path)
        {
            UpdateNodeString();
            info.Rebuild();
            info.Export(path);
        }
    }
}