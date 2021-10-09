using RandomNameGeneratorLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<Person> randomPersonList = new List<Person>();
            List<Observation> randomObservationList = new List<Observation>();
            Observation observation = new Observation();

            randomPersonList = NameGenerator();
            
            Console.WriteLine("How many fake observations do you want to make?");
            string num = Console.ReadLine();

            int count = 0;
            foreach (var p in randomPersonList)
            {
                observation = DarwinGenerator(p.Mrn);
                randomObservationList.Add(observation);

                count++;
                if(count >= Convert.ToInt32(num))
                    break;
            }

            DarwinToCsv(randomObservationList);
        }

        public static List<Person> NameGenerator()
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
                newPerson.Center = SelectRandomStringFromList(rand, centers);
                newPerson.FirstName = personGenerator.GenerateRandomFirstName();
                newPerson.LastName = personGenerator.GenerateRandomLastName();
                newPerson.Gender = genders[rand.Next(0, genders.Length)];
                newPerson.EthnicityCode = SelectRandomStringFromList(rand, ethnicities);
                newPerson.Race = RandomRace(races, races2, rand);
                newPerson.DOB = RandomDOB(rand);
                newPerson.NmdpId = RandomNMDP(rand);
                newPerson.Mrn = RandomMRN(rand);

                randomPersonList.Add(newPerson);
                newPerson = new Person();
            }

            NameToCsv(randomPersonList);

            return randomPersonList;
        }

        public static Observation DarwinGenerator(string mrn)
        {
            Observation newObservation = new Observation();
            Random rand = new Random();

            string[] ptName = new string[] { "Kernan, Nancy", "Cheung, Nai-Kong", "Kushner, Brian", "O'Reilly, Richard", "Boulad, Farid",
                                            "Dunkel, Ira", "Kramer, Kim", "Khakoo, Yasmin", "Modak, Shakeel I.", "Prockop, Susan",
                                            "Scaradavou, Andromachi", "Gilheeney, Stephen", "Kobos, Rachel", "De Braganca, Kevin",
                                            "Basu, Ellen", "Hasan, Aisha", "Curran, Kevin", "Roberts, Stephen", "Spitzer, Barbara",
                                            "Shukla, Neerav", "Cancio, Maria", "Forlenza, Christopher", "Jackson, Carolyn", "Raju, Praveen",
                                            "Kung, Andrew", "Karajannis, Matthias", "Boelens, Jaap Jan",  "Oved, Joseph" };

            string[] service = new string[] { "Pediatrics - Blue Team", "Intensive Care B", "Neuroblastoma" };
            string[] cellInfusionType = new string[] { "Transplant - Allogenic progenitor cells", "Autologous progenitor cells", 
                                                       "Autologous peripheral blood lymphocytes", "Allogenic peripheral blood lymphocytes" };
            string[] cellSource = new string[] { "Cord Blood", "Bone Marrow", "Peripheral Blood", "Chimeric Antigen Receptor (CAR)", "Cytotoxic T Cell (CTL)", "Donor Lymphocyte Infusion (DLI)"};
            string[] donor = new string[] { "related", "unrelated" };

            newObservation.Mrn = mrn;
            newObservation.PTName = SelectRandomStringFromList(rand, ptName);
            newObservation.ServiceDate = RandomServiceDate(rand);
            newObservation.Service = SelectRandomStringFromList(rand, service);
            newObservation.CellInfusionType = SelectRandomStringFromList(rand, cellInfusionType);
            newObservation.CellSource = SelectRandomStringFromList(rand, cellSource);
            newObservation.Donor = SelectRandomStringFromList(rand, donor);

            return newObservation;
        }

        public static string SelectRandomStringFromList(Random rand, string[] sList)
        {
            return sList[rand.Next(0, sList.Length)];
        }

        public static string RandomDOB(Random rand)
        {
            DateTime start = new DateTime(1940, 1, 1);
            DateTime end = new DateTime(2000, 1, 1);
            int range = (end - start).Days;
            return start.AddDays(rand.Next(range)).ToString("MM/dd/yyyy");
        }

        public static string RandomServiceDate(Random rand)
        {
            DateTime start = new DateTime(2021, 1, 1);
            DateTime end = new DateTime(2021, 7, 31);
            int range = (end - start).Days;
            return start.AddDays(rand.Next(range)).ToString("MM/dd/yyyy");
        }

        public static string[] RandomRace(string[] race1, string[] race2, Random rand)
        {
            string[] races = new string[2];

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

        public static string RandomMRN(Random rand)
        {
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

        public static string RandomNMDP(Random rand)
        {             
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

        public static void NameToCsv(List<Person> pList)
        {
            string file = "..\\Data\\FakePHI.csv";

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

        public static void DarwinToCsv(List<Observation> oList)
        {
            string file = "..\\Data\\FakeDarwin.csv";

            string newLine = "";
            System.IO.File.WriteAllText(file, String.Empty);
            newLine = "Mrn" + "," + "PT Name" + "," + "Service Date" + "," + "Service" + "," + "Cell Infusion Type" + ","
                + "Cell Source" + "," + "Donor" + Environment.NewLine;
            System.IO.File.AppendAllText(file, newLine);

            foreach (var o in oList)
            {
                newLine = o.Mrn + ",\"" + o.PTName + "\"," + o.ServiceDate + "," + o.Service + "," + o.CellInfusionType + ","
                    + o.CellSource + "," + o.Donor + Environment.NewLine;
                System.IO.File.AppendAllText(file, newLine);
            }
        }


    }
}
