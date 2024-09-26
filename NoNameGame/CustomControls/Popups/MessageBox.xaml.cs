using System;
using System.Windows.Controls;
using System.Windows.Documents;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Popups
{
    public partial class MessageBox : UserControl
    {
        private readonly string _caption;
        private readonly string _text;


        public MessageBox(string caption,string text)
        {
            if (caption == null) throw new ArgumentNullException("caption");
            if (text == null) throw new ArgumentNullException("text");
            _caption = caption;
            _text = text;

            InitializeComponent();

            var paragraph = new Paragraph();
            paragraph.Inlines.Add(text);
            Text_TextBlock.Blocks.Add(paragraph);

            PopupWindowBase.Header = caption;
            Themer.EnableThemesForControls(ResumeButton);
        }
        public string Caption
        {
            get { return _caption; }
        }
        public string Text
        {
            get { return _text; }
        }
    }
}
