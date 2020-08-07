using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.Engine
{
    public interface ICalcContext
    {
        CnlData GetCnlData(int cnlNum);

        CnlData GetPrevCnlData(int cnlNum);

        DateTime GetCnlTime(int cnlNum);

        DateTime GetPrevCnlTime(int cnlNum);

        void SetCnlData(int cnlNum, CnlData cnlData);
    }
}
