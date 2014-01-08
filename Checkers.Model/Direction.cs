using System.Collections.Generic;

namespace Checkers.Model
{
    public enum Directions
    {
        FrontLeft,
        FrontRight,
        BackLeft,
        BackRitht
    }

    public static class DirectionUtils
    {
        static readonly List<Directions> FrontDirections = new List<Directions>() { Directions.FrontLeft, Directions.FrontRight };
        static readonly List<Directions> BackDirections = new List<Directions>() { Directions.BackLeft, Directions.BackRitht };
        static readonly List<Directions> AllDirections = new List<Directions>() { Directions.FrontLeft, Directions.FrontRight, Directions.BackLeft, Directions.BackRitht };

        static IEnumerable<Directions> GetFrontDirections()
        {
            return FrontDirections;
        }

        static IEnumerable<Directions> GetBackDirections()
        {
            return BackDirections;
        }

        public static IEnumerable<Directions> GetDirections(PieceColor color)
        {
            return (color == PieceColor.White)? GetFrontDirections() : GetBackDirections();
        }



        public static IEnumerable<Directions> GetAllDirections()
        {
            return AllDirections;
        }
    }
}
