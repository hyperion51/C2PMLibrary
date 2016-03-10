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
using System.Text;

namespace Concept2
{
    public class PerformanceMonitor
    {
        public ushort DeviceNumber { get; set; }
        public PerformanceMonitor(ushort DeviceNumber)
        {
            this.DeviceNumber = DeviceNumber;
        }

        public PMUSBInterface.CSAFECommand CreateCommand()
        {
            return new PMUSBInterface.CSAFECommand(this.DeviceNumber);
        }

        public void Reset()
        {
            PMUSBInterface.CSAFECommand cmd = CreateCommand();
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_GOFINISHED_CMD);
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_GOIDLE_CMD);
            cmd.Execute();
        }

        public void Start()
        {
            PMUSBInterface.CSAFECommand cmd = CreateCommand();
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_GOHAVEID_CMD);
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_GOINUSE_CMD);
            cmd.Execute();
        }

        public void Test()
        {
            PMUSBInterface.CSAFECommand cmd = CreateCommand();
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_GOHAVEID_CMD);
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_SETPROGRAM_CMD);
            cmd.CommandsBytes.Add(2);
            cmd.CommandsBytes.Add(1);
            cmd.CommandsBytes.Add(0);
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_GOINUSE_CMD);
            cmd.Execute();
        }

    }
}
