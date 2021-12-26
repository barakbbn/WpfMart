using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMartSamples
{
    enum MachineState
    {
        None,
        [Description("Wax On")]
        On,
        [Description("Wax Off")]
        Off,
        [Description("Wax Burn")]
        Faulted
    }

    enum CustomerType
    {
        Regular,
        Repeating,
        Loyal,
        Vip,
        OurEmployee
    }
}
