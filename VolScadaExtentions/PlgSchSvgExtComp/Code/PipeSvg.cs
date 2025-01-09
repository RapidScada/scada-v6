using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{

    [Serializable]
    public class PipeSvgLeft: BasePipe
    {
        public PipeSvgLeft():base()
        {
            PipeDirection = PipeDirections.左;
            Size = new Size(170, 50);
        }
    }
    [Serializable]
    public class PipeSvgRight : BasePipe
    {
        public PipeSvgRight() : base()
        {
            PipeDirection = PipeDirections.右;
            Size = new Size(170, 50);
        }
    }
    [Serializable]
    public class PipeSvgUp : BasePipe
    {
        public PipeSvgUp() : base()
        {
            PipeDirection = PipeDirections.上;
            Size = new Size(50, 170);
        }
    }
    [Serializable]
    public class PipeSvgDown : BasePipe
    {
        public PipeSvgDown() : base()
        {
            PipeDirection = PipeDirections.下;
            Size = new Size(50, 170);
        }
    }
}