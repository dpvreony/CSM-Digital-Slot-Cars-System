using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSMDigitalSlotCarsSystem;

namespace PowerbaseTest
{
    [TestClass]
    public class PowerbasePacketsTest
    {
        Powerbase powerbase = new Powerbase();
        OutgoingPacket outgoingPacketSuccess = new OutgoingPacket(true);
        OutgoingPacket outgoingPacketNotRecognised = new OutgoingPacket(false);
        IncomingPacket incomingPacket = new IncomingPacket(new byte[] 
        {
            131, // bit7 ON plus Handset#1 (bit1) + Track Power (bit0)
            255, 255, 255, 255, 255, 255, // All handsets = 0, ones compliment
            0, // Aux port current
            249, // CarId 1 first passed the SF-line
            0, 0, 100, 255, // 32-bit counter value
            0
        });



        [TestMethod]
        public void IncomingCrcChecksumByteAreEqualTest()
        {
            
            Enums.PacketType packetType = Enums.PacketType.Incoming;
            byte validCrc8Rx = 101;

            byte crc8Rx = this.powerbase.CrcCheck(this.incomingPacket.Data, packetType);

            Assert.AreEqual(validCrc8Rx, crc8Rx);
        }
    }
}
