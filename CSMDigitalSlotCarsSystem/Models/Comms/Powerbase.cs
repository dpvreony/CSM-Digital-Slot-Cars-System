﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static CSMDigitalSlotCarsSystem.Enums;

namespace CSMDigitalSlotCarsSystem.Models.Comms
{
    /// <summary>
    /// The Powerbase class manages all IO to and from the Powerbase device over a USB 
    /// Serial Port connection, according to the "C7042 Scalextric 6 Car Power Base SNC
    /// Communication Protocol v1" produced by "Sagentia" for "Hornby plc". 
    /// Utilises IncomingPacket and OutgoingPacket objects in an asynchronous BufferBlock 
    /// using the ProducerConsumer pattern.
    /// </summary>
    class Powerbase
    {
        private static OutgoingPacket SuccessOutgoingPacket = new OutgoingPacket(true);
        private static OutgoingPacket NotRecognisedOutgoingPacket = new OutgoingPacket(false);
        private BufferBlock<IncomingPacket> incomingPackets;
        private BufferBlock<OutgoingPacket> outgoingPackets;

        public CancellationToken PowerbaseRunCancellationToken;
        private CancellationTokenSource PowerbaseRunCancellationTokenSource, IncomingCancellationTokenSource, OutgoingCancellationTokenSource;
        private CancellationToken IncomingCancellationToken, OutgoingCancellationToken;

        private SerialPort port;
        int msgRcvdCnt=0, msgInCnt = 0, msgOutCnt = 0;

        /// <summary>
        /// Contructs an instance of the Powerbase class, to manage IO from the Powerbase device.
        /// </summary>
        internal Powerbase()
        {
            Powerbase.SuccessOutgoingPacket.Checksum = Powerbase.CrcCheck(SuccessOutgoingPacket.Data, PacketType.Outgoing);
            Powerbase.NotRecognisedOutgoingPacket.Checksum = Powerbase.CrcCheck(NotRecognisedOutgoingPacket.Data, PacketType.Outgoing);
            this.IncomingPackets = new BufferBlock<IncomingPacket>();
            this.OutgoingPackets = new BufferBlock<OutgoingPacket>();
            this.RefreshAllCancellationTokens();
            this.Port = new SerialPort("COM3", 19200, Parity.None, 8, StopBits.One);
            this.Port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);
            this.Port.DtrEnable = true;

        }

        public BufferBlock<IncomingPacket> IncomingPackets { get => this.incomingPackets; set => this.incomingPackets = value; }
        public BufferBlock<OutgoingPacket> OutgoingPackets { get => this.outgoingPackets; set => this.outgoingPackets = value; }
        public SerialPort Port { get => port; set => port = value; }

        /// <summary>
        /// Drives the Powerbase class to start processing incoming and outgoing packets 
        /// from the Powerbase device.
        /// </summary>
        internal void Run()
        {
            this.RefreshAllCancellationTokens();

            this.Port.WriteBufferSize = 72;
            this.Port.ReceivedBytesThreshold = 15;
            try
            {
                this.Port.Open();
                Console.WriteLine($"{DateTime.Now.ToString()}: {Port.PortName} is open");

                // new task to start input processing
                Task incomingProcessorTask = new Task(() => { this.ProcessIncomingPacketBuffer(); }, TaskCreationOptions.LongRunning);                
                
                // new task to start output processing
                Task outgoingProcessorTask = new Task(() => { this.ProcessOutgoingPacketBuffer(); }, TaskCreationOptions.LongRunning);
                incomingProcessorTask.Start();
                outgoingProcessorTask.Start();

                // add start packet to output processing list
                this.OutgoingPackets.Post(SuccessOutgoingPacket);
            }
            catch (IOException e)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}: {e.Message}");
                this.PowerbaseRunCancellationTokenSource.Cancel();
            }

            while (!this.PowerbaseRunCancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(5000);
                System.Diagnostics.Debug.WriteLine($"Port is still open: {this.Port.IsOpen}");
                this.OutgoingPackets.Post(SuccessOutgoingPacket);

            }
        }

        /// <summary>
        /// Reinstantiates CancellationTokenSource and Tokens for the powerbase run and 
        /// incoming/outgoing buffer processing loops, in event of Powerbase Run needing 
        /// to be restarted.
        /// </summary>
        private void RefreshAllCancellationTokens()
        {
            this.PowerbaseRunCancellationTokenSource = new CancellationTokenSource();
            this.IncomingCancellationTokenSource = new CancellationTokenSource();
            this.OutgoingCancellationTokenSource = new CancellationTokenSource();
            this.PowerbaseRunCancellationToken = PowerbaseRunCancellationTokenSource.Token;
            this.IncomingCancellationToken = IncomingCancellationTokenSource.Token;
            this.OutgoingCancellationToken = OutgoingCancellationTokenSource.Token;
        }

        /// <summary>
        /// Event handler for data received events
        /// </summary>
        /// <param name="sender">The serial port sender.</param>
        /// <param name="e">The serial data received event args.</param>
        private async void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;
            int bytesToRead = serialPort.BytesToRead;
            byte[] data = new byte[bytesToRead];
            serialPort.Read(data, 0, bytesToRead);

            if (data.Length == 15)
            {

                System.Diagnostics.Debug.WriteLine( $"RCVD: {++msgRcvdCnt} " +
                    $"{data[0].ToString()},{data[1].ToString()},{data[2].ToString()},{data[3].ToString()},{data[4].ToString()}," +
                    $"{data[5].ToString()},{data[6].ToString()},{data[7].ToString()},{data[8].ToString()},{data[9].ToString()}," +
                    $"{data[10].ToString()},{data[11].ToString()},{data[12].ToString()},{data[13].ToString()},{data[14].ToString()}");

                // CRC check, send notunderstood if fails
                if (Powerbase.CrcCheck(data, PacketType.Incoming) == data[14])
                {
                    //                this.IncomingPackets.Add(new IncomingPacket(data));
                    //                    this.Port.Write(new OutgoingPacket(data).Data, 0, 9);

                    // LATEST
                    //                    await this.OutgoingPackets.SendAsync(new OutgoingPacket(data));
                    await this.IncomingPackets.SendAsync(new IncomingPacket(data));

                    System.Diagnostics.Debug.WriteLine("Sent Success");

                }
                else
                {
                    //                    this.Port.Write(NotRecognisedOutgoingPacket.Data, 0, 9);
                    await this.OutgoingPackets.SendAsync(NotRecognisedOutgoingPacket);
                    System.Diagnostics.Debug.WriteLine("Sent Unrecognised");
                    msgRcvdCnt -= 1;
                    msgOutCnt -= 1;
                    //                this.OutgoingPackets.Add(NotRecognisedOutgoingPacket);
                }
            }
        }

        public async void ProcessIncomingPacketBuffer()
        {
            IncomingPacket tmpIncomingPacket;
            while (!this.IncomingCancellationToken.IsCancellationRequested)
            {
                if (this.IncomingPackets.Count > 0)
                {
                    tmpIncomingPacket = await this.IncomingPackets.ReceiveAsync();
/*                    System.Diagnostics.Debug.WriteLine($"IN P'D:{++msgInCnt} " +
                    $"{tmpIncomingPacket.Data[0].ToString()},{tmpIncomingPacket.Data[1].ToString()},{tmpIncomingPacket.Data[2].ToString()},{tmpIncomingPacket.Data[3].ToString()},{tmpIncomingPacket.Data[4].ToString()}," +
                    $"{tmpIncomingPacket.Data[5].ToString()},{tmpIncomingPacket.Data[6].ToString()},{tmpIncomingPacket.Data[7].ToString()},{tmpIncomingPacket.Data[8].ToString()},{tmpIncomingPacket.Data[9].ToString()}," +
                    $"{tmpIncomingPacket.Data[10].ToString()},{tmpIncomingPacket.Data[11].ToString()},{tmpIncomingPacket.Data[12].ToString()},{tmpIncomingPacket.Data[13].ToString()},{tmpIncomingPacket.Data[14].ToString()}");
*/
                    // Send to game manager, which will process SF line and return modified outgoing packets

                    await this.OutgoingPackets.SendAsync(new OutgoingPacket(tmpIncomingPacket.Data));
                }
            }
        }

        public async void ProcessOutgoingPacketBuffer()
        {
            OutgoingPacket tmpOutgoingPacket;
            while (!OutgoingCancellationToken.IsCancellationRequested)
            {
                if (this.OutgoingPackets.Count > 0)
                {
                    tmpOutgoingPacket = await this.OutgoingPackets.ReceiveAsync();
                    System.Diagnostics.Debug.WriteLine($"OUT P'D:{++msgOutCnt} " +
                    $"{tmpOutgoingPacket.Data[0].ToString()},{tmpOutgoingPacket.Data[1].ToString()},{tmpOutgoingPacket.Data[2].ToString()},{tmpOutgoingPacket.Data[3].ToString()},{tmpOutgoingPacket.Data[4].ToString()}," +
                    $"{tmpOutgoingPacket.Data[5].ToString()},{tmpOutgoingPacket.Data[6].ToString()},{tmpOutgoingPacket.Data[7].ToString()},{tmpOutgoingPacket.Data[8].ToString()}");

                    await this.Port.BaseStream.WriteAsync(tmpOutgoingPacket.Data, 0, (int)Enums.PacketType.Outgoing);
                    // How to send data down port
//                    this.Port.BaseStream.WriteAsync(this.OutgoingPackets[0].Data, 0, this.OutgoingPackets[0].Data.Length);
//                                        this.Port.Write(this.OutgoingPackets[0].Data, 0, this.OutgoingPackets[0].Data.Length);
                    //                    this.Port.WriteLine(this.OutgoingPackets[0].Data.ToString());
                    //                   this.OutgoingPackets.Remove(this.OutgoingPackets[0]);
                }
            }
            System.Diagnostics.Debug.WriteLine("Out - See you then");

        }

        /// <summary>
        /// Cancels the OutgoingPacket processor loop.
        /// </summary>
        public void CancelProcessOutgoingPacketBuffer()
        {
            this.OutgoingCancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Cancels the IncomingPacket processor loop.
        /// </summary>
        public void CancelProcessIncomingPacketBuffer()
        {
            this.IncomingCancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Performs a CRC8 calculation on the n-1 bytes in an incoming or outgoing packet.
        /// </summary>
        /// <param name="from6CPB_Msg">The packet to generate a checksum for.</param>
        /// <param name="packetType">The tpe of packet (Incoming or Outgoing).</param>
        /// <returns>The checksum byte.</returns>
        internal static byte CrcCheck(byte[] from6CPB_Msg, PacketType packetType)
        {
            int numLoops = packetType == PacketType.Incoming ? (int)PacketType.Incoming : (int)PacketType.Outgoing;
            numLoops -= 1; // Ignore checksum byte
            byte crc8Rx = 0, nxtCrc8Rx = 0;
            int i = 0;
            byte[] CRC8_LOOK_UP_TABLE = new byte[]
            {
                0x00,0x07,0x0e,0x09,0x1c,0x1b,0x12,0x15,0x38,0x3f,0x36,0x31,0x24,0x23,0x2a,0x2d,
                0x70,0x77,0x7E,0x79,0x6C,0x6B,0x62,0x65,0x48,0x4F,0x46,0x41,0x54,0x53,0x5A,0x5D,
                0xE0,0xE7,0xEE,0xE9,0xFC,0xFB,0xF2,0xF5,0xD8,0xDF,0xD6,0xD1,0xC4,0xC3,0xCA,0xCD,
                0x90,0x97,0x9E,0x99,0x8C,0x8B,0x82,0x85,0xA8,0xAF,0xA6,0xA1,0xB4,0xB3,0xBA,0xBD,
                0xC7,0xC0,0xC9,0xCE,0xDB,0xDC,0xD5,0xD2,0xFF,0xF8,0xF1,0xF6,0xE3,0xE4,0xED,0xEA,
                0xB7,0xB0,0xB9,0xBE,0xAB,0xAC,0xA5,0xA2,0x8F,0x88,0x81,0x86,0x93,0x94,0x9D,0x9A,
                0x27,0x20,0x29,0x2E,0x3B,0x3C,0x35,0x32,0x1F,0x18,0x11,0x16,0x03,0x04,0x0D,0x0A,
                0x57,0x50,0x59,0x5E,0x4B,0x4C,0x45,0x42,0x6F,0x68,0x61,0x66,0x73,0x74,0x7D,0x7A,
                0x89,0x8E,0x87,0x80,0x95,0x92,0x9B,0x9C,0xB1,0xB6,0xBF,0xB8,0xAD,0xAA,0xA3,0xA4,
                0xF9,0xFE,0xF7,0xF0,0xE5,0xE2,0xEB,0xEC,0xC1,0xC6,0xCF,0xC8,0xDD,0xDA,0xD3,0xD4,
                0x69,0x6E,0x67,0x60,0x75,0x72,0x7B,0x7C,0x51,0x56,0x5F,0x58,0x4D,0x4A,0x43,0x44,
                0x19,0x1E,0x17,0x10,0x05,0x02,0x0B,0x0C,0x21,0x26,0x2F,0x28,0x3D,0x3A,0x33,0x34,
                0x4E,0x49,0x40,0x47,0x52,0x55,0x5C,0x5B,0x76,0x71,0x78,0x7F,0x6A,0x6D,0x64,0x63,
                0x3E,0x39,0x30,0x37,0x22,0x25,0x2C,0x28,0x06,0x01,0x08,0x0F,0x1A,0x1D,0x14,0x13,
                0xAE,0xA9,0xA0,0xA7,0xB2,0xB5,0xBC,0xBB,0x96,0x91,0x98,0x9F,0x8A,0x8D,0x84,0x83,
                0xDE,0xD9,0xD0,0xD7,0xC2,0xC5,0xCC,0xCB,0xE6,0xE1,0xE8,0xEF,0xFA,0xFD,0xF4,0xF3
            };
            // Routine for the CRC
            crc8Rx = CRC8_LOOK_UP_TABLE[from6CPB_Msg[0]]; //first byte

            for (i=1; i < numLoops; i++)
            {
                nxtCrc8Rx = from6CPB_Msg[i];
                crc8Rx = CRC8_LOOK_UP_TABLE[crc8Rx ^ nxtCrc8Rx];
            }

            return crc8Rx;
        }


    }
}
