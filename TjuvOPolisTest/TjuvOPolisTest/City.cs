using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TjuvOPolisTest;

namespace TjuvOPolisTest
{
    internal class City
    {
        public int CitySizeRows { get; set; } //Höjd på staden
        public int CitySizeCols { get; set; } //Bredd på staden
        public bool Robbery { get; set; }
        public bool Arrested { get; set; }
        public int AmountOfRobbery { get; set; }
        public int AmountOfArrested { get; set; }
        public List<Person> PersonsInCity { get; set; }

        public City(int citySizeRows, int citySizeCols)
        {
            Person person = new Person();
            CitySizeRows = citySizeRows;
            CitySizeCols = citySizeCols;
            PersonsInCity = person.MakePersons();
            Robbery = false;
            Arrested = false;
            AmountOfRobbery = 0;
            AmountOfArrested = 0;
        }

        public City()
        {

        }

        public void TheHunt()
        {
            while (true)
            {
                DrawFrame();                                                          //Ritar upp ramen runt staden
                Robbery = CheckRobbery(PersonsInCity);                                //Anropar metod som kontrollerar om ett rån pågår
                Arrested = CheckArrest(PersonsInCity);                                //Anropar metod som kontrollerar om en tjuv arresteras

                foreach (Person person in PersonsInCity)                              //Loopar igenom varje person i listan av tjuvar, poliser och medborgare
                {
                    if (person is Citizen)                                            //Ändra färg på namnen
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (person is Police)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (person is Thief)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    person.PrintMe();                                               //Anropar metod som skriver ut antingen namn, eller [X] om ett möte med rån/arrest sker
                    Console.ResetColor();

                    person.MoveMent(CitySizeRows, CitySizeCols);                    //Anropar metod som får personerna att röra på sig. Inparametrar är storleken på staden
                    
                    if (person is Thief)
                    {                                                                 //Skriv ut vad tjuven snott bredvid tjuven
                        foreach (Thing item in person.Inventory)
                            Console.Write(item.Name);
                    }
                }
                if (Robbery == true)                                                //Om villkoret stämmer (kontrolleras i Checkrobbery):
                {
                    AmountOfRobbery++;                                              //Öka antalet rån med 1
                    Console.SetCursorPosition(60, CitySizeRows + 4);                //Sätt cursorpos till en rad nedanför "staden"
                    Console.WriteLine("******* Ett rån begås!*******");
                    Thread.Sleep(1000);
                    Robbery = false;                                                //Återstället värdet på Robbery
                }
                if (Arrested == true)                                               //Om villkoret stämmer (kontrolleras i CheckArrest):
                {
                    AmountOfArrested++;                                             //Öka antalet arresteringar med 1
                    Console.SetCursorPosition(60, CitySizeRows + 5);                //Sätter cursorpos till två rader nedanför "staden"
                    Console.WriteLine("******* En tjuv arresteras!*******");
                    Thread.Sleep(1000);
                    Arrested = false;                                               //Återställer värdet på Arrested
                }

                Console.SetCursorPosition(60, CitySizeRows + 6);                    //Sätter cursorpos till tre rader nedanför "staden"
                Console.WriteLine("Totalt antal rån: " + AmountOfRobbery);          //Skriver ut hur många rån som begåtts totalt
                Console.SetCursorPosition(60, CitySizeRows + 7);                    //Sätter cursorpos till fyra rader nedanför "staden"
                Console.WriteLine("Totalt antal arresteringar: " + AmountOfArrested);//Skriver ut hur många tjuvar som arresterats totalt

                Thread.Sleep(200);
                Console.Clear();
            }

        }
        public void DrawFrame()
        {
            for (int i = 0; i < CitySizeRows + 2; i++)
            {
                for (int j = 0; j < CitySizeCols + 2; j++)
                {
                    if (j == 0 || j == CitySizeCols + 1)
                    {
                        Console.Write("|");
                    }

                    else if (i == 0 || i == CitySizeRows + 1)
                    {
                        Console.Write("=");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        public bool CheckRobbery(List<Person> persons)                              //Metod för att kontrollera om rån begås och vad som ska hända
        {
            bool robbery = false;


            for (int i = 0; i < persons.Count; i++)                                 //För varje i: Leta vidare genom listan med start på i och till slutet av listan
            {
                for (int j = 0; j < i; j++)                                         //Se om någon mer persons[j] är på samma plats som persons[i]

                    if ((persons[i].LocationCol == persons[j].LocationCol) && (persons[i].LocationRow == persons[j].LocationRow))//Om det finns två på samma plats och i och j är åtskilda från varandra.
                    {
                        if ((persons[i] is Citizen && persons[j] is Thief) || (persons[i] is Thief && persons[j] is Citizen)) //Det är ett rån om en Citizen och Thief möts. Både persons[i] och persons[j] måste kontrolleras
                        {
                            int citIdx = (persons[i] is Citizen) ? i : j; //Kortversion av:
                            //int citIdx;
                            //if (persons[i] is Citizen)
                            //{
                            //    citIdx = i;
                            //}
                            //else
                            //{
                            //    citidx = j;
                            //}

                            int thiefIdx = (persons[i] is Thief) ? i : j;               //""
                            persons[citIdx].Interaction = true;                         // Flagga för PrintMe att ett möte skett
                            persons[thiefIdx].Interaction = true;                       //""
                            persons[thiefIdx].TakeOneThing(persons[citIdx].Inventory);  // Ta en sak från Citizen                           
                            robbery = true;                                             //Robbery blir true när ett rån begåtts



                        }
                    }
            }
            return robbery;
        }

        static bool CheckArrest(List<Person> persons) //Metod för att kontrollera om en tjuv arresteras och vad som ska hända
        {
            bool arrest = false;


            for (int i = 0; i < persons.Count; i++) //För varje i: Leta vidare genom listan med start på i och till slutet av listan
            {
                for (int j = 0; j < i; j++) //  Se om någon mer persons[j] är på samma plats som persons[i]

                    if ((persons[i].LocationCol == persons[j].LocationCol) && (persons[i].LocationRow == persons[j].LocationRow)) //Om det finns två på samma plats
                    {
                        //Det sker en arrestering om Police och Thief möts. Både persons[i] och persons[j] måste kontrolleras
                        if ((persons[i] is Police && persons[j] is Thief) || (persons[i] is Thief && persons[j] is Police))
                        {
                            int polIdx = (persons[i] is Police) ? i : j;
                            int thiefIdx = (persons[i] is Thief) ? i : j;

                            if (persons[thiefIdx].Inventory.Count > 0)      //Om Tjuvens inventory har innehåll:
                            {
                                persons[polIdx].Interaction = true;         //flaggar för PrintMe att ett möte skett       
                                persons[thiefIdx].Interaction = true;       //""       
                                persons[polIdx].TakeAllThings(persons[thiefIdx].Inventory); //Tar alla saker från tjuvens inventory
                                arrest = true;                              //Arrest blir true när en arrestering skett
                            }
                        }
                    }
            }
            return arrest;
        }


    }
}
//PRINTAR ALLT SOM HÄNDER I SKRIFT

//    if (person is Citizen)
//    {
//        foreach (Thing item in person.Inventory)
//            Console.Write(item.Name);
//    }
//    if (person is Police)
//    {
//        foreach (Thing item in person.Inventory)
//            Console.Write(item.Name);
//    }
//    if (person is Thief)
//    {
//        foreach (Thing item in person.Inventory)
//            Console.Write(item.Name);
//    }
//    Console.WriteLine($"{person.GetType().Name}" +
//        $"X:   {person.LocationCol} " +
//        $"Y:   {person.LocationRow} " +
//        $"Xdir: {person.DirectionCol} " +
//        $"Ydir: {person.DirectionRow}");
