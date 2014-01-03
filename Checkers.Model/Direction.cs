using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        static List<Directions> frontDirections = new List<Directions>() { Directions.FrontLeft, Directions.FrontRight };
        static List<Directions> backDirections = new List<Directions>() { Directions.BackLeft, Directions.BackRitht };
        static List<Directions> allDirections = new List<Directions>() { Directions.FrontLeft, Directions.FrontRight, Directions.BackLeft, Directions.BackRitht };

        static IEnumerable<Directions> GetFrontDirections()
        {
            return frontDirections;
        }

        static IEnumerable<Directions> GetBackDirections()
        {
            return backDirections;
        }

        public static IEnumerable<Directions> GetDirections(PieceColor color)
        {
            return (color == PieceColor.White)? GetFrontDirections() : GetBackDirections();
        }



        public static IEnumerable<Directions> GetAllDirections()
        {
            return allDirections;  //Enum.GetValues(typeof(Directions)).Cast...
        }
    }
}
