using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Plugins.PlgScheme.Model;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Factory for creating basic scheme components.
    /// </summary>
    public class BasicCompFactory : CompFactory
    {
        public override ComponentBase CreateComponent(string typeName, bool nameIsShort)
        {
            if (NameEquals("BasePipe", typeof(BasePipe).FullName, typeName, nameIsShort))
                return new BasePipe();
            else if (NameEquals("WaterLevel", typeof(WaterLevel).FullName, typeName, nameIsShort))
                return new WaterLevel();
            else if (NameEquals("Turbine", typeof(Turbine).FullName, typeName, nameIsShort))
                return new Turbine();
            else if (NameEquals("TextScroll", typeof(TextScroll).FullName, typeName, nameIsShort))
                return new TextScroll();
            else if (NameEquals("TextReflect", typeof(TextReflect).FullName, typeName, nameIsShort))
                return new TextReflect();
            else if (NameEquals("SignalLight", typeof(SignalLight).FullName, typeName, nameIsShort))
                return new SignalLight();
            else if (NameEquals("EditablePipe", typeof(EditablePipe).FullName, typeName, nameIsShort))
                return new EditablePipe();
            else if (NameEquals("ConveyerBelt", typeof(ConveyerBelt).FullName, typeName, nameIsShort))
                return new ConveyerBelt();
            else if (NameEquals("ColoredButton", typeof(ColoredButton).FullName, typeName, nameIsShort))
                return new ColoredButton();
            else
                return new BasePipe();
        }
    }
}
