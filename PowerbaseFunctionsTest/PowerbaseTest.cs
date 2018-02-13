
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlotCarsGo.Models.Comms;
using SlotCarsGo.Helpers;

namespace PowerbaseFunctionsTest
{
    [TestClass]
    public class PowerbaseTest
    {
        Powerbase powerbase = new Powerbase();
        OutgoingPacket outgoingPacketSuccess = new OutgoingPacket(true);
        OutgoingPacket outgoingPacketNotRecognised = new OutgoingPacket(false);
        IncomingPacket incomingPacket = new IncomingPacket(new byte[] 
        {
            0x83, // bit7 ON plus Handset#1 (bit1) + Track Power (bit0)
            0xFF, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, // All handsets = 0, ones compliment
            0x00, // Aux port current
            0xF8, // CarId 1 first passed the SF-line
            0x9B, 0xB7, 0x3A, 0x00, // 32-bit counter value
            0xFF,
            0xBB
        });



        [TestMethod]
        public void IncomingCrcChecksumByteAreEqualTest()
        {
            
            Enums.PacketType packetType = Enums.PacketType.Incoming;
            byte validCrc8Rx = 187;

            byte crc8Rx = powerbase.CalculateCrcChecksum(this.incomingPacket.Data, packetType);

            Assert.AreEqual(validCrc8Rx, crc8Rx);
        }

        [TestMethod]
        public void OutgoingSuccessCrcChecksumByteAreEqualTest()
        {

            Enums.PacketType packetType = Enums.PacketType.Outgoing;
            byte validCrc8Rx = 39;

            byte crc8Rx = powerbase.CalculateCrcChecksum(this.outgoingPacketSuccess.Data, packetType);

            Assert.AreEqual(validCrc8Rx, crc8Rx);
        }

        [TestMethod]
        public void OutgoingNotRecognisedCrcChecksumByteAreEqualTest()
        {

            Enums.PacketType packetType = Enums.PacketType.Outgoing;
            byte validCrc8Rx = 152;

            byte crc8Rx = powerbase.CalculateCrcChecksum(this.outgoingPacketNotRecognised.Data, packetType);

            Assert.AreEqual(validCrc8Rx, crc8Rx);
        }

        [TestMethod]
        public void FailingLastPacketTest()
        {

            Enums.PacketType packetType = Enums.PacketType.Outgoing;
            byte validCrc8Rx = 152;

            byte crc8Rx = powerbase.CalculateCrcChecksum(this.outgoingPacketNotRecognised.Data, packetType);

            Assert.AreEqual(validCrc8Rx, crc8Rx);
        }
    }
}
