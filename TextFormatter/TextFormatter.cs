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
        private int _fontSize;
        private XFontStyle _style;


        private int _row;
        private int _indent;

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
        }

        public void Save(string fileName)
        {
            _document.Save(fileName);
        }

        private int GetCharWidth(char cw)
        {
            return 20;
        }

        private int GetCharHeight()
        {
            return _fontSize;
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
                _row += GetCharHeight();
                break;
            case ".fill":
            case ".nofill":
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
                        _indent += int.Parse (command.Replace (".indent ", ""));
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
            int charsInRow = _indent;
            for (int i = 0; i < text.Length; i++)
            {
                _graphics.DrawString(text[i].ToString(), new XFont("Verdana", _fontSize, _style), XBrushes.Black,
                    new XRect(charsInRow, _row, GetCharWidth(text[i]), GetCharHeight()),
                    XStringFormats.TopLeft);
                charsInRow += GetCharWidth(text[i]);
                if (charsInRow >= _page.Width)
                {
                    charsInRow = _indent;
                    _row += GetCharHeight();
                }
            }


        }
    }
}

