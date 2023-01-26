using Scada.Comm.Drivers.DrvSms.Logic.Protocol;

namespace Scada.Comm.Drivers.DrvSms.Tests
{
    [TestClass]
    public class PduConverterTests
    {
        [TestMethod]
        public void DecodePduTest1()
        {
            const string PDU = "07919761980614F8040B919701439506F300003210625122842104D4F29C0E";
            Message msg = new() { Length = 23 };

            if (PduConverter.DecodePDU(PDU, msg, out string logMsg))
            {
                Assert.AreEqual(msg.Phone, "+79103459603");
                Assert.AreEqual(msg.Text, "Test");
            }
            else
            {
                Assert.Fail(logMsg);
            }
        }

        [TestMethod]
        public void DecodePduTest2()
        {
            const string PDU = "07919761980614F8040B919701439506F30008321062611413210C0041006100610020D83DDE42";
            Message msg = new() { Length = 31 };

            if (PduConverter.DecodePDU(PDU, msg, out string logMsg))
            {
                Assert.AreEqual(msg.Phone, "+79103459603");
                Assert.AreEqual(msg.Text, "Aaa 🙂");
            }
            else
            {
                Assert.Fail(logMsg);
            }
        }
    }
}
