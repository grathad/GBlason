using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GBlason.ViewModel;

namespace GBlason.Common.CustomCommand
{
    public class CommandParameter
    {
        public CommandParameter(Object parameter, GbsFileViewModel target)
        {
            Parameter = parameter;
            Target = target;
        }

        public Object Parameter { get; set; }
        public GbsFileViewModel Target { get; set; }
    }
}
