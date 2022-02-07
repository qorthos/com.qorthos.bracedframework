using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracedFramework
{
    public interface IShowable
    {
        bool IsShown { get; }

        void Hide();

        void Show();
    }
}