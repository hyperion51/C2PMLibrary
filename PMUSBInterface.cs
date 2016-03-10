/**
The MIT License (MIT)

Copyright (c) 2016 Marek GAMELASTER Kraus

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Concept2
{

    public class PMUSBInterface
    {
        public class PMUSBException : Exception
        {
            public PMUSBException(string message, ushort ErrorCode) : base(message)
            {
                this.ErrorCode = ErrorCode;
                char[] errorName = new char[64];
                tkcmdsetDDI_get_error_name(ErrorCode, errorName, 64);
                this.ErrorName = new string(errorName);
                char[] errorText = new char[256];
                tkcmdsetDDI_get_error_text(ErrorCode, errorText, 256);
                this.ErrorDescription = new string(errorText);
            }

            public ushort ErrorCode { get; }
            public string ErrorName { get; }
            public string ErrorDescription { get; }
        }

        public static void Initialize()
        {
            /*try
            {*/
            ushort error = tkcmdsetDDI_init();
            if (error != 0)
            {
                throw new PMUSBException("Cannot initialize a USB connection!", error);
            }
            /*}
            catch(Exception ex)
            {
                throw new Exception("Cannot initialize a USB connection!", ex);
            }*/
        }

        public static void InitializeProtocol(ushort timeout)
        {
            //try
            //{
            ushort error = tkcmdsetCSAFE_init_protocol(timeout);
            if (error != 0)
            {
                throw new PMUSBException("Cannot initialize a CSAFE protocol!", error);
            }
            /*}
            catch (Exception ex)
            {
                throw new Exception("Cannot initialize a CSAFE protocol!", ex);
            }*/
        }

        public static ushort DiscoverPMs(PMtype PMType)
        {
            string PMname = "";
            switch (PMType)
            {
                case PMtype.PM3_PRODUCT_NAME: PMname = "Concept II PM3"; break;
                case PMtype.PM3_PRODUCT_NAME2: PMname = "Concept2 Performance Monitor 3 (PM3)"; break;
                case PMtype.PM3TESTER_PRODUCT_NAME: PMname = "Concept 2 PM3 Tester"; break;
                case PMtype.PM4_PRODUCT_NAME: PMname = "Concept2 Performance Monitor 4 (PM4)"; break;
                case PMtype.PM5_PRODUCT_NAME: PMname = "Concept2 Performance Monitor 5 (PM5)"; break;
            }
            //try
            //{
            ushort count = 0;
            ushort error = tkcmdsetDDI_discover_pm3s(PMname, 0, ref count);
            if (error != 0)
            {
                throw new PMUSBException("Cannot initialize a CSAFE protocol!", error);
            }
            return count;
            /*}
            catch (Exception ex)
            {
                throw new Exception("Cannot initialize a CSAFE protocol!", ex);
            }*/
        }

        public static void ShutdownAll()
        {
            ushort error = tkcmdsetDDI_shutdown_all();
            if (error != 0) throw new PMUSBException("Cannot shutdown all connections!", error);
        }


        public enum PMtype
        {
            PM3_PRODUCT_NAME,
            PM3_PRODUCT_NAME2,
            PM3TESTER_PRODUCT_NAME,
            PM4_PRODUCT_NAME,
            PM5_PRODUCT_NAME,
        }

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort tkcmdsetDDI_init();

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void tkcmdsetDDI_get_error_name(
            ushort ecode,
            char[] nameptr,
            ushort namelen);

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort tkcmdsetDDI_shutdown_all();

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void tkcmdsetDDI_get_error_text(
            ushort ecode,
            char[] textptr,
            ushort textlen);

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort tkcmdsetDDI_discover_pm3s(
           string product_name,
           ushort starting_address,
           ref ushort num_units);

        [DllImport("PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort tkcmdsetCSAFE_init_protocol(ushort timeout);

        [DllImport("PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort tkcmdsetCSAFE_command(
           ushort unit_address,
           ushort cmd_data_size,
           uint[] cmd_data,
           ref ushort rsp_data_size,
           uint[] rsp_data);


    }
}
