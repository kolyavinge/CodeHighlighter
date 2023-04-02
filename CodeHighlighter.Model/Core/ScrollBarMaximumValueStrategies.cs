using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Ancillary;
using CodeHighlighter.Model;

namespace CodeHighlighter.Core;

public interface IVerticalScrollBarMaximumValueStrategy
{
    double GetValue();
}

public interface IHorizontalScrollBarMaximumValueStrategy
{
    double GetValue();
}

internal class DefaultVerticalScrollBarMaximumValueStrategy : IVerticalScrollBarMaximumValueStrategy
{
    private readonly IText _text;
    private readonly ITextMeasuresInternal _textMeasures;
    private readonly ILineGapCollection _gaps;
    private readonly ILineFolds _folds;

    public DefaultVerticalScrollBarMaximumValueStrategy(
        IText text,
        ITextMeasuresInternal textMeasures,
        ILineGapCollection gaps,
        ILineFolds folds)
    {
        _text = text;
        _textMeasures = textMeasures;
        _gaps = gaps;
        _folds = folds;
    }

    public double GetValue()
    {
        return (_text.LinesCount + _gaps.Sum(x => x.CountBefore) - _folds.FoldedLinesCount) * _textMeasures.LineHeight;
    }
}

internal class DefaultHorizontalScrollBarMaximumValueStrategy : IHorizontalScrollBarMaximumValueStrategy
{
    private readonly IText _text;
    private readonly ITextMeasuresInternal _textMeasures;

    public DefaultHorizontalScrollBarMaximumValueStrategy(IText text, ITextMeasuresInternal textMeasures)
    {
        _text = text;
        _textMeasures = textMeasures;
    }

    public double GetValue()
    {
        return _text.GetMaxLineWidth() * _textMeasures.LetterWidth;
    }
}

public class MaximumHorizontalScrollBarMaximumValueStrategy : IHorizontalScrollBarMaximumValueStrategy
{
    private readonly List<ICodeTextBoxModel> _models;

    public MaximumHorizontalScrollBarMaximumValueStrategy(IEnumerable<ICodeTextBoxModel> models)
    {
        _models = models.ToList();
    }

    public double GetValue()
    {
        return _models.Select(x => x.AdditionalInfo.TextMaxLineWidth * x.TextMeasures.LetterWidth).Max();
    }
}
