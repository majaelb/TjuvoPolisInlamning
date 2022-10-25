using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TjuvOPolisTest
{
    internal class Person
    {
        //Testar

        public string Name { get; set; } //"M" "T" "P"
        public int LocationRow { get; set; } //X-pos
        public int LocationCol { get; set; } //Y-pos
        public int DirectionRow { get; set; } //X- riktning
        public int DirectionCol { get; set; } //Y-riktning
        public bool Interaction { get; set; } //Tar in värde från rån/arrestmetod i city
        public List<Thing> Inventory { get; set; }


        public Person(string name, int locationRow, int locationCol, int directionRow, int directionCol, List<Thing> inventory)
        {
            Random rnd = new Random();
            Name = " ";
            LocationRow = rnd.Next(0, 25); //Stadens höjd
            LocationCol = rnd.Next(0, 100); //Stadens bredd
            DirectionRow = rnd.Next(-1, 2); //Slumpar x-riktning mellan -1 & 1.
            DirectionCol = rnd.Next(-1, 2); //Slumpar y-riktning
            Interaction = false;
            Inventory = GetThings(); //Skapar lista av tillhörigheter

        }
        public Person()
        {

        }

        public virtual List<Thing> GetThings() //Virtual-metod för att skapa listor som används med override i subklasserna
        {

            List<Thing> inventory = new List<Thing>();

            return inventory;
        }
        public List<Person> MakePersons() //Skapar EN lista av tre olika sorters personer
        {
            //Varje persontyp får namn, position, riktning och sin lista med inventory

            List<Person> people = new List<Person>();

            for (int i = 0; i < 30; i++)
            {
                people.Add(new Citizen(Name, LocationRow, LocationCol, DirectionRow, DirectionCol, Inventory));
            }
            for (int i = 0; i < 15; i++)
            {
                people.Add(new Thief(Name, LocationRow, LocationCol, DirectionRow, DirectionCol, Inventory));
            }
            for (int i = 0; i < 10; i++)
            {
                people.Add(new Police(Name, LocationRow, LocationCol, DirectionRow, DirectionCol, Inventory));
            }

            return people;
        }

        internal void PrintMe()                                     //Sköter utskriften i staden
        {
            Console.SetCursorPosition(LocationCol, LocationRow);    //Sätter markören på personens position
            if (Interaction == true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[X]");                               //Skriver ut [X] om ett möte skett (kontrolleras i city)
            }
            else
                Console.Write(Name);                                //Skriver ut namnet om inget rån/arrestering skett
            Interaction = false;                                    //Återställer Interaction
        }

        internal void MoveMent(int maxRow, int maxCol)              //Metod som ger rörelse och håller alla på plats i staden. Inparametrar är 
                                                                    // stadens storlek
        {
            LocationRow += DirectionRow;
            LocationCol += DirectionCol;
            if (LocationRow >= maxRow)
                LocationRow = 1;
            if (LocationCol >= maxCol)
                LocationCol = 1;
            if (LocationRow < 1)
                LocationRow = maxRow;
            if (LocationCol < 1)
                LocationCol = maxCol;
        }

        public void TakeOneThing(List<Thing> belongings)            //När tjuv möter medborgare
        {
            if (belongings.Count > 0)                               //Om medborgaren har något i sin Belongings
            {
                Random rnd = new Random();
                int i = rnd.Next(0, belongings.Count);              //Hitta random sak i listan
                Inventory.Add(belongings[i]);                       //Lägg till den i tjuvens lista
                belongings.RemoveAt(i);                             //Ta bort den från medborgarens lista
            }
        }

        public void TakeAllThings(List<Thing> loot)                 //När polis möter tjuv
        {
            foreach (Thing stolen in loot)                          //För varje sak i tjuvens lista
            {
                Inventory.Add(stolen);                              //Lägg till det stulna i polisens lista
            }
            loot.Clear();                                           //töm tjuvens lista
        }
    }
    internal class Citizen : Person
    {
        public List<Thing> Belongings { get; set; }
        public Citizen(string name, int locationRow, int locationCol, int directionRow, int directionCol, List<Thing> belongings) : base(name, locationRow, locationCol, directionRow, directionCol, belongings)
        {
            Belongings = GetThings();
            Interaction = false;
            Name = "M";
        }

        public Citizen()
        {

        }

        public override List<Thing> GetThings()
        {
            List<Thing> belongings = new List<Thing>();
            belongings.Add(new Thing("Wallet "));
            belongings.Add(new Thing("Keys "));
            belongings.Add(new Thing("Phone "));
            belongings.Add(new Thing("Money "));

            return belongings;
        }
    }

    internal class Thief : Person
    {
        public List<Thing> Loot { get; set; }
        public Thief(string name, int locationRow, int locationCol, int directionRow, int directionCol, List<Thing> loot) : base(name, locationRow, locationCol, directionRow, directionCol, loot)
        {
            Loot = new List<Thing>();
            Interaction = false;
            Name = "T";
        }
        public Thief()
        {

        }
        public override List<Thing> GetThings()
        {
            List<Thing> loot = new List<Thing>();

            return loot;
        }
    }

    internal class Police : Person
    {
        public List<Thing> Confiscated { get; set; }
        public int Arrests { get; set; }

        public Police(string name, int locationRow, int locationCol, int directionRow, int directionCol, List<Thing> confiscated) : base(name, locationRow, locationCol, directionRow, directionCol, confiscated)
        {
            Confiscated = new List<Thing>();
            Interaction = false;
            Name = "P";
        }
        public Police()
        {

        }
        public override List<Thing> GetThings()
        {
            List<Thing> confiscated = new List<Thing>();

            return confiscated;
        }
    }

}
//public void ShowPersonStory(/*List<Person> people*/)
//{
//    Random rnd = new Random();
//    List<Person> people = new List<Person>();


//    for (int i = 0; i < 15; i++)
//    {
//        people.Add(new Citizen("M", rnd.Next(0, 25), rnd.Next(0, 100), rnd.Next(-1, 2), rnd.Next(-1, 2)));
//    }
//    for (int i = 0; i < 10; i++)
//    {
//        people.Add(new Person("T", rnd.Next(0, 25), rnd.Next(0, 100), rnd.Next(-1, 2), rnd.Next(-1, 2)));
//    }
//    for (int i = 0; i < 5; i++)
//    {
//        people.Add(new Person("P", rnd.Next(0, 25), rnd.Next(0, 100), rnd.Next(-1, 2), rnd.Next(-1, 2)));
//    }


//    foreach (Person person in people)
//    {

//        if (person is Citizen)
//        {
//            Console.WriteLine($"Medborgaren har blivit rånad: {((Citizen)person).Robbed} gånger");

//        }
//        if (person is Thief)
//        {
//            Console.WriteLine($"Tjuven har blivit arresterad: {((Thief)person).Arrested} gånger");
//        }
//        if (person is Police)
//        {
//            Console.WriteLine($"Polisen har arresterat: {((Police)person).Arrests} tjuvar");
//        }

//        Console.WriteLine($"Namn: {person.Name} \n" +
//                $"Position Y: {person.LocationRow}\n" +
//                $"Position X: {person.LocationCol}\n" +
//                $"Direction Y: {person.DirectionRow}\n" +
//                $"Direction X: {person.DirectionCol}\n");
//                //$"Antal personer: {person.AmountOfPeople}");

//        Console.WriteLine();
//    }
//}
