using Scada.Comm.Drivers.DrvSms.Logic.Protocol;

namespace Scada.Comm.Drivers.DrvSms.Tests
{
    [TestClass]
    public class PduConverterTests
    {
        [TestMethod]
        public void EncodePduTest()
        {
            Pdu pdu = PduConverter.EncodePDU("+79103459603", "Hello");
            Assert.AreEqual("0001000B919701439506F3000005C8329BFD06", pdu.Data);
            Assert.AreEqual(18, pdu.Length);
        }

        [TestMethod]
        public void DecodePduTest1()
        {
            const string PDU = "07919761980614F8040B919701439506F300003210625122842104D4F29C0E";
            Message msg = new() { Length = 23 };

            if (PduConverter.DecodePDU(PDU, msg, out string logMsg))
            {
                Assert.AreEqual("+79103459603", msg.Phone);
                Assert.AreEqual("Test", msg.Text);
            }
            else
            {
                Assert.Fail(logMsg);
            }
        }

        [TestMethod]
        public void DecodePduTest2()
        {
            // Message "Aaa 🙂"
            const string PDU = "07919761980614F8040B919701439506F30008321062611413210C0041006100610020D83DDE42";
            Message msg = new() { Length = 31 };

            if (PduConverter.DecodePDU(PDU, msg, out string logMsg))
            {
                Assert.AreEqual("+79103459603", msg.Phone);
                Assert.AreEqual("Aaa   ", msg.Text);
            }
            else
            {
                Assert.Fail(logMsg);
            }
        }
    }
}
