using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IConsoleRenderer
    {
        void Render(Board board);
        string ReadGuess(int wordLength);
        void ShowMessage(string message);
        void RenderEnd(bool won, string target);
    }
}
