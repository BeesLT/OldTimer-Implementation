public class ManeuverFactory
{
    public IManeuverBehaviour Create(ManeuverType type)
    {
        return type switch
        {
            ManeuverType.Weaving    => new WeavingManeuver(),
            ManeuverType.Charging   => new ChargingManeuver(),
            ManeuverType.Orbiting   => new OrbitingManeuver(),
            ManeuverType.Retreating => new RetreatingManeuver(),
            ManeuverType.Wobble     => new WobbleManeuver(),
            _                       => null
        };
    }
}