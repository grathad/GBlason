using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GBlason.Common.CustomCommand
{
    public class CommandRepository
    {
        public CommandRepository()
        {
            CommandHistory = new ObservableCollection<CommandGeneric>();
        }

        public ObservableCollection<CommandGeneric> CommandHistory { get; set; }
    }
}
