using Avalonia.Controls;
using Avalonia.Threading;
using System.IO;
using System.Text;

namespace lKHM
{
    public class ControlWriter : TextWriter
    {
        private Control _control;

        public ControlWriter(Control control)
        {
            _control = control;
        }

        public override void Write(char value)
        {
            AppendText(value.ToString());
        }

        public override void Write(string value)
        {
            AppendText(value);
        }

        private void AppendText(string text)
        {
            Dispatcher.UIThread.Post(() =>
            {
                switch (_control)
                {
                    case TextBox tb:
                        tb.Text += text;
                        break;

                    case TextBlock tbl:
                        tbl.Text += text;
                        break;

                    case SelectableTextBlock stb:
                        stb.Text += text;
                        break;
                }
            });
        }

        public override Encoding Encoding => Encoding.ASCII;
    }
}
