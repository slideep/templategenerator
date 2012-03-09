//using System;
//using System.Drawing;
//using System.Text;
//using ICSharpCode.TextEditor;
//using ICSharpCode.TextEditor.Actions;
//using ICSharpCode.TextEditor.Document;

//namespace TemplateGenerator.Kontrollit
//{
//    internal class TekstiEditori : TextEditorControl
//    {
//        public TekstiEditori()
//        {
//            base.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
//            base.Document.FoldingManager.FoldingStrategy = new IndentFoldingStrategy();
//            base.Document.FormattingStrategy = new DefaultFormattingStrategy();
//            base.TextEditorProperties = InitializeProperties();
//            base.Document.DocumentChanged += this.Document_DocumentChanged;
//        }

//        public bool CanCopy()
//        {
//            return base.TextArea.SelectionManager.HasSomethingSelected;
//        }

//        public bool CanCut()
//        {
//            return base.TextArea.SelectionManager.HasSomethingSelected;
//        }

//        public bool CanDelete()
//        {
//            return this.CanCopy();
//        }

//        public bool CanFormatXaml()
//        {
//            return this.CanSelectAll();
//        }

//        public bool CanPaste()
//        {
//            return base.TextArea.ClipboardHandler.EnablePaste;
//        }

//        public bool CanRedo()
//        {
//            return base.Document.UndoStack.CanRedo;
//        }

//        public bool CanSelectAll()
//        {
//            if (base.Document.TextContent == null)
//            {
//                return false;
//            }
//            if (base.Document.TextContent.Trim().Equals(String.Empty))
//            {
//                return false;
//            }
//            return true;
//        }

//        public bool CanUndo()
//        {
//            return base.Document.UndoStack.CanUndo;
//        }

//        public void Copy()
//        {
//            new Copy().Execute(base.TextArea);
//            base.TextArea.Focus();
//        }

//        public void Cut()
//        {
//            new Cut().Execute(base.TextArea);
//            base.TextArea.Focus();
//        }

//        public void Delete()
//        {
//            new Delete().Execute(base.TextArea);
//            base.TextArea.Focus();
//        }

//        private void Document_DocumentChanged(object sender, DocumentEventArgs e)
//        {
//            this.UpdateFoldings();
//        }

//        public void FormatXaml()
//        {
//            new FormatBuffer().Execute(base.TextArea);
//            base.TextArea.Focus();
//        }

//        private static ITextEditorProperties InitializeProperties()
//        {
//            DefaultTextEditorProperties properties = new DefaultTextEditorProperties();

//            properties.Font = new Font("Consolas", 10f);
//            properties.IndentStyle = IndentStyle.Auto;
//            properties.ShowSpaces = false;
//            properties.LineTerminator = "\n";
//            properties.ShowTabs = false;
//            properties.ShowInvalidLines = true;
//            properties.ShowEOLMarker = false;
//            properties.UseAntiAliasedFont = true;
//            properties.TabIndent = 4;
//            properties.CutCopyWholeLine = true;
//            properties.LineViewerStyle = LineViewerStyle.FullRow;
//            properties.MouseWheelScrollDown = true;
//            properties.MouseWheelTextZoom = true;
//            properties.AllowCaretBeyondEOL = false;
//            properties.AutoInsertCurlyBracket = true;
//            properties.BracketMatchingStyle = BracketMatchingStyle.Before;
//            properties.ConvertTabsToSpaces = false;
//            properties.ShowMatchingBracket = true;
//            properties.EnableFolding = true;
//            properties.ShowVerticalRuler = false;
//            properties.IsIconBarVisible = false;
//            properties.Encoding = Encoding.Unicode;

//            return properties;
//        }

//        public void Paste()
//        {
//            new Paste().Execute(base.TextArea);
//            base.TextArea.Focus();
//        }

//        public void SelectAll()
//        {
//            new SelectWholeDocument().Execute(base.TextArea);
//            base.TextArea.Focus();
//        }

//        public void SelectText(int start, int length)
//        {
//            int textLength = base.Document.TextLength;
//            if (textLength < (start + length))
//            {
//                length = (textLength - 1) - start;
//            }
//            base.TextArea.Caret.Position = base.Document.OffsetToPosition(start + length);
//            base.TextArea.SelectionManager.ClearSelection();
//            base.TextArea.SelectionManager.SetSelection(
//                new DefaultSelection(
//                    base.Document, 
//                    base.Document.OffsetToPosition(start),
//                    base.Document.OffsetToPosition(start + length)));

//            base.Refresh();
//        }

//        public void UpdateFoldings()
//        {
//            base.Document.FoldingManager.UpdateFoldings(string.Empty, null);
//            base.TextArea.Refresh(base.TextArea.FoldMargin);
//        }
//    }
//}