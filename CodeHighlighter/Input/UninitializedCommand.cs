using System;
using System.Collections.Generic;
using System.Text;

namespace CodeHighlighter.Input
{
    internal class UninitializedCommand : Command
    {
        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
