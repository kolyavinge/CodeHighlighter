﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

public interface ISearchPanelModel
{
    string Pattern { get; set; }
    bool IsRegex { get; set; }
    bool MatchCase { get; set; }
    Color HighlightColor { get; set; }
    bool HasResult { get; }
}

internal class SearchPanelModel : ISearchPanelModel, INotifyPropertyChanged
{
    private readonly ICodeTextBoxModel _model;
    private readonly ITextSearchLogic _textSearchLogic;
    private readonly IRegexSearchLogic _regexSearchLogic;
    private List<TextPosition> _currentSearchResult;
    private List<TextHighlight> _currentHighlights;
    private string _pattern;
    private bool _isRegex;
    private bool _matchCase;
    private bool _hasResult;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Pattern
    {
        get => _pattern;
        set { _pattern = value; UpdateSearch(); }
    }

    public bool IsRegex
    {
        get => _isRegex;
        set { _isRegex = value; UpdateSearch(); }
    }

    public bool MatchCase
    {
        get => _matchCase;
        set { _matchCase = value; UpdateSearch(); }
    }

    public bool HasResult
    {
        get => _hasResult;
        private set { _hasResult = value; PropertyChanged?.Invoke(this, new("HasResult")); }
    }

    public Color HighlightColor { get; set; }

    public SearchPanelModel(
        ICodeTextBoxModel model,
        ITextSearchLogic textSearchLogic,
        IRegexSearchLogic regexSearchLogic)
    {
        _model = model;
        _model.TextEvents.TextChanged += (s, e) => UpdateSearch();
        _textSearchLogic = textSearchLogic;
        _regexSearchLogic = regexSearchLogic;
        _currentSearchResult = new();
        _currentHighlights = new();
        _pattern = "";
        _isRegex = false;
        _matchCase = false;
    }

    private void UpdateSearch()
    {
        if (Pattern == "" && !_currentSearchResult.Any()) return;
        _currentSearchResult = IsRegex ? _regexSearchLogic.DoSearch(Pattern, MatchCase).ToList() : _textSearchLogic.DoSearch(Pattern, MatchCase).ToList();
        _model.TextHighlighter.Remove(_currentHighlights);
        _currentHighlights = _currentSearchResult.Select(x => new TextHighlight(x, HighlightColor)).ToList();
        _model.TextHighlighter.Add(_currentHighlights);
        HasResult = _currentSearchResult.Any();
    }
}