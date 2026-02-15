using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using BrawlLib.Internal.Audio;
using BrawlLib.SSBB.ResourceNodes;
using PropertyGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BrawlLib.BrawlManagerLib.Songs
{
    public partial class SongPanel : UserControl
    {
        /// <summary>
        /// The currently opened .brstm file's root node.
        /// </summary>
        private ResourceNode _rootNode;

        /// <summary>
        /// The full path to the currently opened .brstm file.
        /// </summary>
        private string _rootPath;

        public bool LoadNames { get; set; } = true;

        public bool LoadBrstms { get; set; } = true;

        public bool ShowPropertyGrid
        {
            get => propertyGrid.IsVisible;
            set => propertyGrid.IsVisible = value;
        }

        public bool ShowFilename
        {
            get => lblFilename.IsVisible;
            set => lblFilename.IsVisible = value;
        }

        public bool ShowVolumeSpinner
        {
            get => nudVolume.IsVisible;
            set => nudVolume.IsVisible = value;
        }

        public string RootPath => _rootPath;

        public bool FileOpen => _rootPath != null;

        public bool InfoLoaded => songNameBar.InfoLoaded;

        public string LastFileCalledFor { get; private set; }

        public byte? VolumeByte
        {
            set => nudVolume.Value = value ?? -1;
        }

        public IDictionary<ushort, string> CustomSongTitles { private get; set; }

        public event EventHandler AudioEnded;

        public SongPanel()
        {
            InitializeComponent();

            AddHandler(DragDrop.DropEvent, SongPanel_Drop);
            AddHandler(DragDrop.DragOverEvent, SongPanel_DragOver);
        }

        public void Close()
        {
            _rootNode?.Dispose();
            _rootNode = null;

            _rootPath = null;

            propertyGrid.DataContext = null;

            app.TargetSource = null;

            app.IsEnabled = false;

            lblFilename.Text = "";

            songNameBar.Index = -1;
        }

        public void Open(FileInfo fi, string fallbackDir = null)
        {
            LastFileCalledFor = fi.FullName;

            lblFilename.Text =
                Path.GetFileNameWithoutExtension(LastFileCalledFor);

            _rootNode?.Dispose();

            _rootNode = null;

            if (fi.Exists)
            {
                _rootPath = fi.FullName;

                _rootNode =
                    NodeFactory.FromFile(null, _rootPath);
            }
            else if (fallbackDir != null)
            {
                FileInfo fallback =
                    new FileInfo(
                        Path.Combine(fallbackDir, fi.Name));

                if (fallback.Exists)
                {
                    _rootPath = null;

                    _rootNode =
                        NodeFactory.FromFile(null,
                        fallback.FullName);
                }
            }

            string filename =
                Path.GetFileNameWithoutExtension(
                    LastFileCalledFor).ToUpper();

            Song song =
                SongIDMap.Songs
                .FirstOrDefault(s =>
                s.Filename == filename);

            if (song != null &&
                CustomSongTitles != null &&
                CustomSongTitles.TryGetValue(
                    song.ID,
                    out string name))
            {
                songNameBar.Index = -1;

                songNameBar.NegativeIndexText = name;
            }
            else if (LoadNames)
            {
                songNameBar.Index =
                    song == null
                    ? -1
                    : songNameBar.GetInfoPacIndex(song.ID);
            }
            else
            {
                songNameBar.Index = -1;
            }

            if (LoadBrstms &&
                _rootNode is IAudioSource node)
            {
                propertyGrid.DataContext =
                    _rootNode;

                app.TargetSource =
                    node;

                app.IsEnabled = true;
            }
            else
            {
                propertyGrid.DataContext =
                    null;

                app.TargetSource =
                    null;

                app.IsEnabled =
                    false;
            }
        }

        public void Play()
        {
            app.Play();
        }

        public async Task Export()
        {
            var dialog =
                new SaveFileDialog
                {
                    Filters =
                    {
                        new FileDialogFilter
                        {
                            Name = "BRSTM stream",
                            Extensions =
                            {
                                "brstm"
                            }
                        }
                    }
                };

            string path =
                await dialog.ShowAsync(
                    GetWindow());

            if (!string.IsNullOrEmpty(path))
            {
                File.Copy(
                    RootPath,
                    path,
                    true);
            }
        }

        public async Task Rename()
        {
            var dialog =
                new SaveFileDialog
                {
                    InitialFileName =
                        Path.GetFileName(RootPath),

                    Filters =
                    {
                        new FileDialogFilter
                        {
                            Name = "BRSTM",
                            Extensions =
                            {
                                "brstm"
                            }
                        }
                    }
                };

            string newPath =
                await dialog.ShowAsync(
                    GetWindow());

            if (string.IsNullOrEmpty(newPath))
                return;

            string from = RootPath;

            Close();

            File.Move(
                from,
                newPath);
        }

        public void Delete()
        {
            if (_rootNode != null)
            {
                _rootNode.Dispose();

                File.Delete(_rootPath);

                Close();
            }
        }

        public void Replace(string filepath)
        {
            _rootNode?.Dispose();

            copyBrstm(
                filepath,
                LastFileCalledFor);

            Open(
                new FileInfo(
                LastFileCalledFor));
        }

        public string findInfoFile()
        {
            return
                songNameBar
                .findInfoFile();
        }

        public bool IsInfoBarDirty()
        {
            return
                songNameBar
                .IsDirty;
        }

        public void save()
        {
            songNameBar.save();
        }

        public void ExportMSBin(string path)
        {
            songNameBar.ExportMSBin(path);
        }

        private void SongPanel_DragOver(
            object sender,
            DragEventArgs e)
        {
            if (e.Data.Contains(
                DataFormats.Files))
            {
                e.DragEffects =
                    DragDropEffects.Copy;
            }
        }

        private void SongPanel_Drop(
            object sender,
            DragEventArgs e)
        {
            var files =
                e.Data.GetFiles();

            if (files?.Count > 0)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    Replace(
                        files[0]
                        .Path
                        .LocalPath);
                });
            }
        }

        public static void copyBrstm(
            string src,
            string dest)
        {
            if (src.EndsWith(".brstm"))
            {
                File.Copy(
                    src,
                    dest,
                    true);
            }
            else
            {
                var converter =
                    new BrstmConverterDialog();

                converter.AudioSource =
                    src;

                if (converter.ShowDialog()
                    == true)
                {
                    var node =
                        new RSTMNode();

                    node.ReplaceRaw(
                        converter.AudioData);

                    node.Export(
                        dest);

                    node.Dispose();
                }
            }
        }

        private void nudVolume_ValueChanged(
            object sender,
            EventArgs e)
        {
            app.VolumePercent =
                nudVolume.Value <= 0
                ? 1.0
                : nudVolume.Value / 127.0;
        }

        private void app_AudioEnded(
            object sender,
            EventArgs e)
        {
            AudioEnded?.Invoke(
                this,
                e);
        }

        private Window GetWindow()
        {
            return
                VisualRoot
                as Window;
        }
    }
}
