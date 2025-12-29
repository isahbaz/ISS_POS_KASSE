using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS.Devices;

namespace IS_KASSE
{
    class FisCitizen
    {
        private OPOSPOSPrinter Printer = null;
        public FisCitizen()
        {
        Printer = new OPOSPOSPrinter();
        int nRC;
        // Open the printer.
        nRC = Printer.Open("CT-S310II_1");
       
        // If succeeded, then claim.
        if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
        {
            nRC = Printer.ClaimDevice(1000);
            // If succeeded, then enable.
            if (nRC == (int)OPOS_Constants.OPOS_SUCCESS)
            {
                Printer.DeviceEnabled = true;
                Program.printer2 = Printer;
                nRC = Printer.ResultCode;
                
            }

        }
        }
    }
}
