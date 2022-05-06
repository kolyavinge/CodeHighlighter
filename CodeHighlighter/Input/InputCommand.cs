using System;
using System.Collections.Generic;
using System.Text;

namespace CodeHighlighter.Input
{
    internal abstract class InputCommand : Command
    {
        protected readonly InputCommandContext _context;

        protected InputCommand(InputCommandContext context)
        {
            _context = context;
        }
    }
}
