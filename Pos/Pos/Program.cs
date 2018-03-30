using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos
{
    class Program
    {
        static void Main(string[] args)
        {
            var pos = new Pos();
            pos.InitDefaultValues();
            pos.Start();
        }
    }
}
