using RandomNameGeneratorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NameGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<Person> randomPersonList = new List<Person>();
            Person newPerson = new Person();
            var personGenerator = new PersonNameGenerator();
            Random rand = new Random();

            string[] centers = new string[] { "11109", "11110" };
            char[] genders = new char[] { 'M', 'F' };
            string[] ethnicities = new string[] { "2135-2", "2186-5" };
            string[] races = new string[] { "1002-5", "2028-9", "2054-5", "2076-8", "2106-3", "ASKU" };
            string[] races2 = new string[] { "1002-5", "2028-9", "2054-5", "2076-8", "2106-3" };
            Console.WriteLine("How many fake people do you want to make?");
            string num = Console.ReadLine();

            for (int i = 0; i < Convert.ToInt32(num); i++)
            {
                personGenerator = new PersonNameGenerator();
                newPerson.Center = centers[rand.Next(0, centers.Length)]; ;
                newPerson.FirstName = personGenerator.GenerateRandomFirstName();
                newPerson.LastName = personGenerator.GenerateRandomLastName();
                newPerson.Gender = genders[rand.Next(0, genders.Length)];
                newPerson.EthnicityCode = ethnicities[rand.Next(0, ethnicities.Length)];
                newPerson.Race = RandomRace(races, races2);
                newPerson.DOB = RandomDOB();
                newPerson.NmdpId = RandomNMDP();
                newPerson.Mrn = RandomMRN();

                randomPersonList.Add(newPerson);
                newPerson = new Person();
            }

            ToCsv(randomPersonList);
        }

        public static string RandomDOB()
        {
            Random rand = new Random();
            DateTime start = new DateTime(1940, 1, 1);
            DateTime end = new DateTime(2000, 1, 1);
            int range = (end - start).Days;
            return start.AddDays(rand.Next(range)).ToString("MM/dd/yyyy");
        }

        public static string[] RandomRace(string[] race1, string[] race2)
        {
            Random rand = new Random();
            string[] races = new string[] { "" , "" };

            races[0] = race1[rand.Next(0, race1.Length)];

            if (races[0] == "ASKU")
                return races;

            var list = new List<string>(race2);
            list.Remove(races[0]);
            race2 = list.ToArray();

            int number = rand.Next(0, 100);
            if(number <= 8)
            {
                races[1] = race2[rand.Next(0, race2.Length)];
            }
            return races;
        }

        public static string RandomMRN()
        {
            Random rand = new Random();

            const string allowedFirstCharacter = "3";
            char[] firstChars = new char[allowedFirstCharacter.Length];

            int stringLength = 8;
            const string allowedChars = "0123456789";
            char[] chars = new char[stringLength];

            chars[0] = allowedFirstCharacter[rand.Next(0, allowedFirstCharacter.Length)];


            for (int i = 1; i < stringLength; i++)
            {
                chars[i] = allowedChars[rand.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        public static string RandomNMDP()
        { 
            Random rand = new Random();
            
            string result = "";
            int number = rand.Next(0, 100);
            if (number <= 10)
            {
                const string chars = "1234567890";
                result = new string(Enumerable.Repeat(chars, 7).Select(s => s[rand.Next(s.Length)]).ToArray());
                string formattedResult = String.Format("{0:###-###-#}", Convert.ToInt64(result));
                return formattedResult;
            }
            else
            {
                return null;
            }

        }

        public static void ToCsv(List<Person> pList)
        {
            string file = ("E:\\projects\\NameGenerator\\NameGenerator\\Dataset.csv");
            string newLine = "";
            System.IO.File.WriteAllText(file, String.Empty);
            newLine = "Center" + "," + "FirstName" + "," + "LastName" + "," + "Gender" + "," + "DateOfBirth" + ","
                + "EthnicityCode" + "," + "Race" + "," + "NmdpId" + "," + "Mrn" + Environment.NewLine;
            System.IO.File.AppendAllText(file, newLine);

            foreach (var p in pList)
            {
                if(p.Race[1] == "")
                {
                    newLine = p.Center + "," + p.FirstName + "," + p.LastName + "," + p.Gender + "," + p.DOB + ","
                        + p.EthnicityCode + "," + p.Race[0] + "," + p.NmdpId + "," + p.Mrn + Environment.NewLine;
                    System.IO.File.AppendAllText(file, newLine);
                }
                else
                {
                    newLine = p.Center + "," + p.FirstName + "," + p.LastName + "," + p.Gender + "," + p.DOB + ","
                        + p.EthnicityCode + "," + p.Race[0] + ";" + p.Race[1] + "," + p.NmdpId + "," + p.Mrn + Environment.NewLine;
                    System.IO.File.AppendAllText(file, newLine);
                }

            }
        }
            

    }
}
