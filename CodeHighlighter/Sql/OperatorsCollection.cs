using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CodeHighlighter.Sql
{
    internal class OperatorsCollection : IEnumerable<string>
    {
        private readonly List<string> _keywords = new()
        {
            "AND",
            "OR",
            "NOT",
            "NULL",
            "IN",
            "IS",
            "EXISTS",
        };

        public IEnumerator<string> GetEnumerator()
        {
            return _keywords.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _keywords.GetEnumerator();
        }
    }
}
