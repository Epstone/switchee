using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaHomeAutomation.MainApp
{
    public class AddDialogViewModel
   {
        public AddDialogViewModel()
        {
            this.MasterSwitches = new ObservableCollection<string>();
            for (char i = 'A'; i < 'G'; i++)
            {
                MasterSwitches.Add(i.ToString());
            }

            this.SlaveSwitches = new ObservableCollection<string>();

            for (int i = 1; i < 17; i++)
            {
                this.SlaveSwitches.Add(i.ToString());
            }

            this.Device = new Intertechno()
            {
                Masterdip = MasterSwitches.First(),
                Slavedip = SlaveSwitches.First(),
                UserDefinedName = ""
            };
        }

        public Intertechno Device { get; set; }


        public ObservableCollection<string> MasterSwitches { get; set; }

        public ObservableCollection<string> SlaveSwitches { get; set; }
    }
}
