using System;
using System.Drawing;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace XamarinInterview
{
    public class TextFormatter
    {
        private PdfDocument _document;
        private PdfPage _page;
        private XGraphics _graphics;
        private double _fontSize;
        private XFontStyle _style;
        private XFont _font
        {
            get
            {
                return new XFont("Arial", _fontSize, _style);
            }
        }

        private double _indentWidth
        {
            get
            {
                return GetWidth("    ") * _indent;
            }
        }
        private double _row;
        private int _indent;
        private double _rowWidth;
        private bool _fill;

        public TextFormatter ()
        {
            // Create a new PDF document
            _document = new PdfDocument();
            _document.Info.Title = "Poor Man's Text Formatter";

            // Create an empty page
            _page = _document.AddPage();

            // Get an XGraphics object for drawing
            _graphics = XGraphics.FromPdfPage(_page);

            _row = 0;
            _indent = 0;
            _fontSize = 14;
            _rowWidth = 0;
            _fill = false;
        }

        public void Save(string fileName)
        {
            _document.Save(fileName);
        }

        private double GetWidth(string word)
        {
            double result;
            if (String.IsNullOrWhiteSpace(word))
            {
                // Apparently in this library a space on it's own is 0 width, so we'll just wrap it in some 
                // other characters and subtract them out
                result = _graphics.MeasureString(String.Format("|{0}|", word), _font).Width - (2.0 * _graphics.MeasureString("|", _font).Width);
            }
            else
            {
                result = _graphics.MeasureString(word, _font).Width;
            }
            return result;
        }

        private double GetCharHeight()
        {
            return _graphics.MeasureString("Ay", _font).Height;
        }

        public void IssueCommand(string command)
        {
            switch (command) {
            case ".large":
                _fontSize = 20;
                break;
            case ".normal":
                _fontSize = 14;
                break;
            case ".paragraph":
                _row += (GetCharHeight() * 2);
                _rowWidth = _indentWidth;
                break;
            case ".fill":
                _fill = true;
                break;
            case ".nofill":
                _fill = false;
                break;
            case ".regular":
                _style = XFontStyle.Regular;
                break;
            case ".italic":
                if (_style == XFontStyle.Bold)
                    _style = XFontStyle.BoldItalic;
                else
                    _style = XFontStyle.Italic;
                break;
            case ".bold":
                if (_style == XFontStyle.Italic)
                    _style = XFontStyle.BoldItalic;
                else
                    _style = XFontStyle.Bold;
                break;
            default:
                if (command.StartsWith(".")) {
                    if (command.Contains (".indent")) {
                        if (_rowWidth > _indentWidth)
                            _row += (GetCharHeight() * 2);

                        _indent += int.Parse (command.Replace (".indent ", ""));
                        _rowWidth = _indentWidth;
                    }
                } else {
                    // It's just text, so write it out
                    WriteText (command);
                }
                break;
            }
        }

        private void WriteText(string text)
        {
            // Draw the text
            string[] words = text.Split(' ');

            

            for (int i = 0; i < words.Length; i++)
            {
                Draw(words[i]);
                if (_rowWidth>_indentWidth)
                    Draw(" ");
            }


        }

        private void Draw(string text)
        {
            _graphics.DrawString(text, _font, XBrushes.Black,
                    new XRect(_rowWidth, _row, GetWidth(text), GetCharHeight()),
                    XStringFormats.TopLeft);

            _rowWidth += GetWidth(text);
            if (_rowWidth >= _page.Width - _indentWidth)
            {
                _rowWidth = _indentWidth;
                _row += GetCharHeight();
            }
        }
    }
}

