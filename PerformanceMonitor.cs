﻿/**
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

        public List<uint> GetHorizontalDistance()
        {
            PMUSBInterface.CSAFECommand cmd = CreateCommand();
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_GETHORIZONTAL_CMD);
            List<uint> data = cmd.Execute();
            return data;
        }

        public void SetHorizontalDistance(int value, PMUSBInterface.CSAFEUnits unit)
        {
            PMUSBInterface.CSAFECommand cmd = CreateCommand();
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_SETHORIZONTAL_CMD);
            cmd.CommandsBytes.Add(0x03);
            byte[] bytes = BitConverter.GetBytes(value);
            /*switch (unit)
            {
                case PMUSBInterface.CSAFEUnits.Kilometers:
                    cmd.CommandsBytes.Add(bytes[0]);
                    cmd.CommandsBytes.Add(0x00);
                    break;
                case PMUSBInterface.CSAFEUnits.Meters:*/
            cmd.CommandsBytes.Add(bytes[0]);
            cmd.CommandsBytes.Add(bytes[1]);
            /*        break;
            }*/
            cmd.CommandsBytes.Add((uint)unit);
            //cmd.CommandsBytes.Add(0x85);
            cmd.Execute();
        }

        public void SetWorkout(PMUSBInterface.WorkoutTypes type)
        {
            PMUSBInterface.CSAFECommand cmd = CreateCommand();
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_SETPROGRAM_CMD);
            cmd.CommandsBytes.Add(0x02);
            cmd.CommandsBytes.Add((uint) type);
            cmd.CommandsBytes.Add(0x00);
            cmd.Execute();
        }

        public void GoInUse()
        {
            PMUSBInterface.CSAFECommand cmd = CreateCommand();
            cmd.CommandsBytes.Add((uint)PMUSBInterface.CSAFECommands.CSAFE_GOINUSE_CMD);
            cmd.Execute();
        }

        public void Test()
        {
            PMUSBInterface.CSAFECommand cmd = CreateCommand();
            /*cmd.CommandsBytes.Add(0x21);
            cmd.CommandsBytes.Add(0x03);
            cmd.CommandsBytes.Add(0x03);
            cmd.CommandsBytes.Add(0x00);
            cmd.CommandsBytes.Add(0x21);
            cmd.CommandsBytes.Add(0x1a);
            cmd.CommandsBytes.Add(0x07);
            cmd.CommandsBytes.Add(0x05);
            cmd.CommandsBytes.Add(0x05);
            cmd.CommandsBytes.Add(0x80);
            cmd.CommandsBytes.Add(0xf4);
            cmd.CommandsBytes.Add(0x01);
            cmd.CommandsBytes.Add(0x00);
            cmd.CommandsBytes.Add(0x00);
            cmd.CommandsBytes.Add(0x34);
            cmd.CommandsBytes.Add(0x03);
            cmd.CommandsBytes.Add(0x2c);
            cmd.CommandsBytes.Add(0x01);
            cmd.CommandsBytes.Add(0x58);*/
            cmd.CommandsBytes.Add(0x24);
            cmd.CommandsBytes.Add(0x02);
            cmd.CommandsBytes.Add(0x00);
            cmd.CommandsBytes.Add(0x00);
            cmd.CommandsBytes.Add(0x85);

            List<uint> ret = cmd.Execute();
        }

    }
}
