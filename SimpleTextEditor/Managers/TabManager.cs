using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SimpleTextEditor.Theme;
using SimpleTextEditor.UI.Controls;

namespace SimpleTextEditor.UI
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  TabManager  –  manages editors, tab buttons and content switching
    // ─────────────────────────────────────────────────────────────────────────────
    public class TabManager
    {
        private readonly FlowLayoutPanel _tabStrip;
        private readonly Panel           _contentPanel;

        private readonly List<RichTextBox>                     _editors   = new();
        private readonly Dictionary<RichTextBox, string>       _files     = new();
        private readonly Dictionary<RichTextBox, ModernTabButton> _tabs   = new();
        private readonly Dictionary<RichTextBox, string>       _syntax    = new();
        private readonly Dictionary<RichTextBox, bool>         _dirty     = new();

        public MarkdownPreviewManager PreviewManager { get; }
        public RichTextBox CurrentEditor { get; private set; }

        public event EventHandler EditorChanged;
        public event EventHandler NoTabsLeft;

        // ── New Tab shortcut button ("+" in the strip) ───────────────────────────
        private readonly Button _addTabBtn;

        public TabManager(FlowLayoutPanel tabStrip, Panel contentPanel)
        {
            _tabStrip     = tabStrip;
            _contentPanel = contentPanel;
            PreviewManager = new MarkdownPreviewManager(contentPanel);

            // "+" button at end of tab strip
            _addTabBtn = new Button
            {
                Text      = "+",
                Size      = new Size(36, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
                Font      = new Font("Segoe UI", 14f),
                Margin    = new Padding(4, 4, 0, 0),
                ForeColor = AppTheme.AccentLight,
                BackColor = AppTheme.TabBackground
            };
            _addTabBtn.FlatAppearance.BorderSize  = 0;
            _addTabBtn.FlatAppearance.MouseOverBackColor = AppTheme.SurfaceHover;
            _addTabBtn.Click += (s, e) => AddNewTab("Untitled", "");
            _tabStrip.Controls.Add(_addTabBtn);
        }

        // ────────────────────────────────────────────────────────────────────────
        public void AddNewTab(string title, string content, string filePath = null,
                              string syntax = "Звичайний текст")
        {
            // ── Editor container with line numbers ───────────────────────────────
            var (container, rtb) = EditorBuilder.BuildEditor(content);

            // ── Dirty tracking ───────────────────────────────────────────────────
            rtb.TextChanged += (s, e) =>
            {
                if (!_dirty.ContainsKey(rtb) || !_dirty[rtb])
                {
                    _dirty[rtb] = true;
                    if (_tabs.TryGetValue(rtb, out var t)) t.IsDirty = true;
                }
                EditorChanged?.Invoke(s, e);
            };
            rtb.SelectionChanged += (s, e) => EditorChanged?.Invoke(s, e);

            _editors.Add(rtb);
            _contentPanel.Controls.Add(container);
            _syntax[rtb] = syntax;
            _dirty[rtb]  = false;
            _isFormattedMode[rtb] = true;
            PreviewManager.SetPreviewMode(rtb, false, null);
            if (filePath != null) _files[rtb] = filePath;

            // ── Modern tab button ─────────────────────────────────────────────────
            var tab = new ModernTabButton(title);
            tab.TabClicked   += (s, e) => SelectTab(rtb);
            tab.CloseClicked += (s, e) => CloseTab(rtb);

            _tabs[rtb] = tab;

            // Insert before the "+" button
            int insertIdx = _tabStrip.Controls.IndexOf(_addTabBtn);
            _tabStrip.Controls.Add(tab);
            _tabStrip.Controls.SetChildIndex(tab, insertIdx);

            SelectTab(rtb);
        }

        // ────────────────────────────────────────────────────────────────────────
        public void SelectTab(RichTextBox rtb)
        {
            if (rtb == null || !_editors.Contains(rtb)) return;

            CurrentEditor = rtb;
            bool isPreview = PreviewManager.IsPreviewMode(rtb);

            foreach (var editor in _editors)
            {
                bool isSelected = (editor == rtb);
                editor.Parent.Visible = isSelected && !isPreview;
                if (isSelected && !isPreview) editor.Parent.BringToFront();

                if (_tabs.TryGetValue(editor, out var t))
                    t.IsActive = isSelected;
            }

            PreviewManager.ShowPreviewIfActive(rtb);
            EditorChanged?.Invoke(rtb, EventArgs.Empty);
        }

        // ────────────────────────────────────────────────────────────────────────
        public void CloseTab(RichTextBox rtb)
        {
            if (rtb == null || !_editors.Contains(rtb)) return;

            if (_editors.Count > 1)
            {
                if (_tabs.TryGetValue(rtb, out var tab))
                    _tabStrip.Controls.Remove(tab);

                _contentPanel.Controls.Remove(rtb.Parent);
                _editors.Remove(rtb);
                _tabs.Remove(rtb);
                _files.Remove(rtb);
                _dirty.Remove(rtb);
                PreviewManager.RemoveEditor(rtb);

                if (CurrentEditor == rtb)
                    SelectTab(_editors[_editors.Count - 1]);
            }
            else
            {
                NoTabsLeft?.Invoke(this, EventArgs.Empty);
            }
        }

        public void CloseCurrentTab() => CloseTab(CurrentEditor);

        // Ctrl+Tab cycling
        public void SelectNextTab()
        {
            if (_editors.Count < 2) return;
            int idx = _editors.IndexOf(CurrentEditor);
            SelectTab(_editors[(idx + 1) % _editors.Count]);
        }

        public void SelectPrevTab()
        {
            if (_editors.Count < 2) return;
            int idx = _editors.IndexOf(CurrentEditor);
            SelectTab(_editors[(idx - 1 + _editors.Count) % _editors.Count]);
        }

        // ────────────────────────────────────────────────────────────────────────
        public IEnumerable<RichTextBox> GetAllEditors() => _editors;
        public int EditorCount => _editors.Count;

        public string GetFilePath(RichTextBox rtb) => _files.GetValueOrDefault(rtb);
        public string GetSyntax(RichTextBox rtb)   => _syntax.GetValueOrDefault(rtb, "Звичайний текст");

        public void SetFilePath(RichTextBox rtb, string path)
        {
            _files[rtb] = path;
            _dirty[rtb] = false;
            if (path != null && _tabs.TryGetValue(rtb, out var tab))
            {
                tab.Title   = System.IO.Path.GetFileName(path);
                tab.IsDirty = false;
            }
        }

        public void MarkSaved(RichTextBox rtb)
        {
            _dirty[rtb] = false;
            if (_tabs.TryGetValue(rtb, out var tab)) tab.IsDirty = false;
        }

        // ── Formatting Mode ──────────────────────────────────────────────────────
        private readonly Dictionary<RichTextBox, bool> _isFormattedMode = new();

        public bool IsFormattedMode(RichTextBox rtb)
        {
            if (rtb == null) return true;
            return _isFormattedMode.GetValueOrDefault(rtb, true);
        }

        public void SetFormattedMode(RichTextBox rtb, bool isFormatted)
        {
            if (rtb == null) return;
            _isFormattedMode[rtb] = isFormatted;
            
            // Auto hide preview if switching to formatted mode
            if (isFormatted && PreviewManager.IsPreviewMode(rtb))
            {
                PreviewManager.SetPreviewMode(rtb, false, CurrentEditor);
            }
        }
    }
}
