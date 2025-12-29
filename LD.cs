using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS.Devices;
//using OposLineDisplay_1_11_Lib;
using Microsoft.PointOfService;
using System.Data;
using System.Windows.Forms;

namespace IS_KASSE
{

    class LD
    {
        OPOSLineDisplay dsp = null;
        public LD()
        {


            try
            {
                dsp = new OPOSLineDisplay();
            }
            catch (Exception ee)
            {
                F_GenericError frmerr = new F_GenericError();
                frmerr.lblMesaj.Text = ee.Message;
                frmerr.ShowDialog();
            }
            /*if (printer == null)
            {
                string myMsrName = "CT-S310II_1";
                //PosPrinter msr = null;
                PosExplorer explorer = new PosExplorer();
                DeviceInfo deviceInfo = explorer.GetDevice(DeviceType.PosPrinter, myMsrName);
                if (deviceInfo == null)
                {
                    printer = null;
                    F_GenericError frmerr = new F_GenericError();
                    frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo=null";
                    frmerr.ShowDialog();
                }
                else
                {
                    printer = explorer.CreateInstance(deviceInfo) as PosPrinter;
                    ConnectToPrinter(printer);
                    /* F_GenericError frmerr = new F_GenericError();
                      frmerr.lblMesaj.Text = "getreceiptprinter->deviceinfo"+printer.DeviceName;
                      frmerr.ShowDialog();*9/
                    //return msr;
                }
            }*/
            try
            {
                if (Program.displayType == "WN")
                {
                    dsp.CharacterSet = (int)OPOSLineDisplayConstants.DISP_CCS_UNICODE;
                    dsp.Open(Program.displaySO);//USB´, com
                    dsp.ClaimDevice(100);
                    if (dsp.Claimed)
                    {
                        dsp.DeviceEnabled = true;
                        Program.lineDsp = dsp;

                        dsp.CharacterSet = 858;
                        dsp.DisplayText("ISS POS KASSENSYSTEM", (int)OPOSLineDisplayConstants.DISP_DT_BLINK);
                        dsp.DisplayTextAt(1, 0, "€ $ ½ & % ? " + ((char)213), (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
                        int[] aa = new int[dsp.CharacterSetList.Length];
                        /* MessageBox.Show("adet"+aa.Length);

                         foreach (int b in aa)
                         {
                             MessageBox.Show(b.ToString());
                         }*/

                    }
                }
                else if (Program.displayType == "star")
                {
                    dsp.CharacterSet = 858;
                    dsp.Open(Program.displaySO);
                    dsp.ClaimDevice(100);
                    if (dsp.Claimed)
                    {

                        dsp.DeviceEnabled = true;
                        Program.lineDsp = dsp;
                        dsp.CharacterSet = (int)OPOSLineDisplayConstants.DISP_CCS_UNICODE;
                        dsp.DisplayText("ISS POS KASSENSYS", (int)OPOSLineDisplayConstants.DISP_DT_BLINK);
                        dsp.DisplayTextAt(1, 0, "€ $ ½ & % ? " + ((char)213), (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
                    }
                }
                else if (Program.displayType == "epson")
                {
                    try
                    {
                        //dsp.CharacterSet = 1252;
                        dsp.Open(Program.displaySO);
                        dsp.ClaimDevice(1000);
                        if (dsp.Claimed)
                        {
                           // dsp.CharacterSet = 1252;
                            dsp.MapCharacterSet = true;
                            dsp.DeviceEnabled = true;
                            Program.lineDsp = dsp;
                            
                            dsp.DisplayText("ISS POS KASSENSYSTEM", (int)OPOSLineDisplayConstants.DISP_DT_BLINK);
                            dsp.DisplayTextAt(1, 0, "€ $ ½ & % ? " + ((char)213), (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message);
                    }
                }
                else if (Program.displayType == "IBM")
                {
                    dsp.CharacterSet = 858;


                    dsp.Open(Program.displaySO);
                    dsp.ClaimDevice(100);
                    if (dsp.Claimed)
                    {
                        dsp.DeviceEnabled = true;
                        Program.lineDsp = dsp;
                        dsp.CharacterSet = 858;
                        dsp.MapCharacterSet = true;
                       // MessageBox.Show("List:"+dsp.CharacterSetList.ToString()+"charset  Def:"+dsp.CharacterSet+" cap:"+ dsp.CapCharacterSet);
                        dsp.DisplayText("ISS POS KASSENSYS", (int)OPOSLineDisplayConstants.DISP_DT_BLINK);
                        dsp.DisplayTextAt(1, 0, "€ $ ½ & % ? " + ((char)213), (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
                    }
                }
                else if (Program.displayType == "NCR")
                {
                    dsp.CharacterSet = 858;


                    dsp.Open(Program.displaySO);
                    dsp.ClaimDevice(100);
                    if (dsp.Claimed)
                    {
                        dsp.DeviceEnabled = true;
                        Program.lineDsp = dsp;
                        dsp.CharacterSet = 858;
                        dsp.MapCharacterSet = true;
                        // MessageBox.Show("List:"+dsp.CharacterSetList.ToString()+"charset  Def:"+dsp.CharacterSet+" cap:"+ dsp.CapCharacterSet);
                        dsp.DisplayText("ISS POS KASSENSYS", (int)OPOSLineDisplayConstants.DISP_DT_BLINK);
                        dsp.DisplayTextAt(1, 0, "€ $ ½ & % ? " + ((char)213), (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
                    }
                }
                else
                {
                    
                
                    dsp.CharacterSet = 858;


                    dsp.Open(Program.displaySO);
                    dsp.ClaimDevice(100);
                    if (dsp.Claimed)
                    {
                        dsp.DeviceEnabled = true;
                        Program.lineDsp = dsp;
                        dsp.CharacterSet = 858;
                        dsp.MapCharacterSet = true;
                        // MessageBox.Show("List:"+dsp.CharacterSetList.ToString()+"charset  Def:"+dsp.CharacterSet+" cap:"+ dsp.CapCharacterSet);
                        dsp.DisplayText("ISS POS KASSENSYS", (int)OPOSLineDisplayConstants.DISP_DT_BLINK);
                        dsp.DisplayTextAt(1, 0, "€ $ ½ & % ? " + ((char)213), (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
                    }
                }
                
            }
            catch (Exception ee)
            {
                F_GenericError frmerr = new F_GenericError();
                frmerr.lblMesaj.Text = ee.Message;
                frmerr.ShowDialog();
            }



        }

    }
}
