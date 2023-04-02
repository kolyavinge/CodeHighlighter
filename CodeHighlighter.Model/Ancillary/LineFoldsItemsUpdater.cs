using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Ancillary;

public interface ILineFoldsItemsUpdater
{
    void Update(ILineFolds folds, IEnumerable<LineFold> items);
}

public class LineFoldsItemsUpdater : ILineFoldsItemsUpdater
{
    public void Update(ILineFolds folds, IEnumerable<LineFold> items)
    {
        var newFolds = new List<LineFold>();
        var foldsDictionary = folds.Items.ToDictionary(k => k.LineIndex, v => v);
        var needToSet = false;
        foreach (var item in items)
        {
            if (foldsDictionary.ContainsKey(item.LineIndex) && item.LinesCount == foldsDictionary[item.LineIndex].LinesCount)
            {
                newFolds.Add(foldsDictionary[item.LineIndex]);
            }
            else
            {
                newFolds.Add(item);
                needToSet = true;
            }
        }
        if (needToSet || folds.Items.Count != newFolds.Count)
        {
            folds.Items = newFolds;
        }
    }
}
