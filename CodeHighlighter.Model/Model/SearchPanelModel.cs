using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

public interface ISearchPanel
{
    bool FocusPattern();
    void SelectAllPattern();
}

public interface ISearchPanelModel
{
    string Pattern { get; set; }
    bool IsRegex { get; set; }
    bool MatchCase { get; set; }
    bool IsWholeWord { get; set; }
    bool IsPatternEntered { get; }
    bool HasResult { get; }
    Color HighlightColor { get; set; }
    ITextPositionNavigator Navigator { get; }
    void AttachSearchPanel(ISearchPanel panel);
    bool FocusPattern();
    void SelectAllPattern();
}

internal class SearchPanelModel : ISearchPanelModel, INotifyPropertyChanged
{
    private readonly ICodeTextBoxModel _codeTextBoxModel;
    private readonly ITextSearchLogic _textSearchLogic;
    private readonly IRegexSearchLogic _regexSearchLogic;
    private readonly IWholeWordLogic _wholeWordLogic;
    private readonly ITextPositionNavigatorInternal _textPositionNavigator;
    private ISearchPanel? _panel;
    private List<TextPosition> _currentSearchResult;
    private List<TextHighlight> _currentHighlights;
    private string _pattern;
    private bool _isRegex;
    private bool _matchCase;
    private bool _isWholeWord;
    private bool _isPatternEntered;
    private bool _hasResult;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Pattern
    {
        get => _pattern;
        set { _pattern = value; UpdateSearch(); PropertyChanged?.Invoke(this, new("Pattern")); }
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

    public bool IsWholeWord
    {
        get => _isWholeWord;
        set { _isWholeWord = value; UpdateSearch(); }
    }

    public bool IsPatternEntered
    {
        get => _isPatternEntered;
        private set { _isPatternEntered = value; PropertyChanged?.Invoke(this, new("IsPatternEntered")); }
    }

    public bool HasResult
    {
        get => _hasResult;
        private set { _hasResult = value; PropertyChanged?.Invoke(this, new("HasResult")); }
    }

    public Color HighlightColor { get; set; }

    public ITextPositionNavigator Navigator => _textPositionNavigator;

    public SearchPanelModel(
        ICodeTextBoxModel codeTextBoxModel,
        ITextSearchLogic textSearchLogic,
        IRegexSearchLogic regexSearchLogic,
        IWholeWordLogic wholeWordLogic,
        ITextPositionNavigatorInternal textPositionNavigator)
    {
        _codeTextBoxModel = codeTextBoxModel;
        _codeTextBoxModel.TextEvents.TextChanged += (s, e) => UpdateSearch();
        _textSearchLogic = textSearchLogic;
        _regexSearchLogic = regexSearchLogic;
        _wholeWordLogic = wholeWordLogic;
        _textPositionNavigator = textPositionNavigator;
        _currentSearchResult = new();
        _currentHighlights = new();
        _pattern = "";
        _isRegex = false;
        _matchCase = false;
    }

    public void AttachSearchPanel(ISearchPanel panel)
    {
        _panel = panel;
    }

    public bool FocusPattern() => _panel?.FocusPattern() ?? false;

    public void SelectAllPattern() => _panel?.SelectAllPattern();

    private void UpdateSearch()
    {
        if (Pattern == "" && !_currentSearchResult.Any()) return;
        var searchResult = IsRegex ? _regexSearchLogic.DoSearch(Pattern, MatchCase) : _textSearchLogic.DoSearch(Pattern, MatchCase);
        if (_isWholeWord)
        {
            searchResult = _wholeWordLogic.GetResult(searchResult);
        }
        _currentSearchResult = searchResult.ToList();
        _codeTextBoxModel.TextHighlighter.Remove(_currentHighlights);
        _currentHighlights = _currentSearchResult.Select(x => new TextHighlight(x, HighlightColor)).ToList();
        _codeTextBoxModel.TextHighlighter.Add(_currentHighlights);
        _textPositionNavigator.SetPositions(_currentSearchResult);
        IsPatternEntered = !String.IsNullOrWhiteSpace(_pattern);
        HasResult = _currentSearchResult.Any();
    }
}
