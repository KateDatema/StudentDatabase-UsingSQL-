using System;
using System.Collections.Generic;
using System.Linq;
using StudentDB.Models;


namespace StudentDB
{
    class Program
    {
        static void Main(string[] args)
        {
            StudentDatabaseContext db = new StudentDatabaseContext();

            List<Student> students = db.Students.ToList();

            Console.WriteLine("Welcome to the May 2021 C# Class!");

            bool again = true;
            while (again == true)
            {
                menu();
                Console.WriteLine("What would you like to do?");
                int choice = GetInteger(5);
                if (choice == 1)
                {
                    PrintList(students, db);
                }
                else if (choice == 2)
                {
                    int nameOrID = int.Parse(GetUserInput("Would you like to search by Name(1) or by ID?(2)"));

                    if (nameOrID == 1)
                    {
                        string name = GetUserInput("who are you searching for?");
                        List<Student> nameStudent = SearchStudentsByName(name, db);
                        Console.WriteLine("Here are your matches:");
                        PrintList(nameStudent, db);

                    }
                    if (nameOrID == 2)
                    {
                        int id = int.Parse(GetUserInput("What is their id?"));
                        Student s = SearchStudentsById(id, db);
                        PrintStudent(s, db);
                    }

                }
                else if (choice == 3)
                {
                    int id = int.Parse(GetUserInput("What is their id?"));
                    UpdateStudent(id, db);
                }
                else if (choice == 4)
                {
                    Student newPerson = NewStudent();
                    AddStudent(newPerson, db);
                }
                else if (choice == 5)
                {
                    int id = int.Parse(GetUserInput("What is their id?"));
                    DeleteStudent(id, db);
                    Console.WriteLine("The student has been deleted.");
                }

                again = GetContinue();
            }

        }

        public static void PrintStudent(Student s, StudentDatabaseContext db)
        {
            Console.WriteLine("ID: " +s.Id);
            Console.WriteLine("Name: " +s.Name);
            Console.WriteLine("Home Town: " +s.HomeTown);
            Console.WriteLine("Favorite Food:" + s.FavoriteFood);
        }
        public static Student SearchStudentsById(int id, StudentDatabaseContext db)
        {
            try
            {
                Student s = db.Students.Find(id);

                return s;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine($"No Student was found at {id} index");
                return null;
            }
        }
        public static bool GetContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Would you like to go back to the main menu? y/n");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                return true;
            }
            else if (answer.ToLower() == "n")
            {
                Console.WriteLine();
                Console.Write("Have a great day!");
                return false;
            }
            else
            {
                Console.WriteLine("I didn't understand your response, please try again...");
                return GetContinue();
            }
        }

        public static void PrintList(List<Student> students, StudentDatabaseContext db)
        {
            if (students.Count == 0)
            {
                Console.WriteLine("There are no students in the input list. nothing was likely found in the database");
            }
            foreach (Student s in students)
            {
                Console.WriteLine("Name: " + s.Name);
                Console.WriteLine("HomeTown: " + s.HomeTown);
                Console.WriteLine("FavoriteFood: " + s.FavoriteFood);
                Console.WriteLine();
            }
        }

        public static Student NewStudent()
        {
            string Name = GetUserInput("What is their name?");
            string HomeTown = GetUserInput("What is their HomeTown?");
            string FavFood = GetUserInput("What is their Favorite food");
            Student s = new Student()
            {
                Name = Name,
                HomeTown = HomeTown,
                FavoriteFood = FavFood
            };
            return s;
        }

        public static void AddStudent(Student newStudent, StudentDatabaseContext db)
        {
            db.Students.Add(newStudent);
            db.SaveChanges();
            Console.WriteLine(newStudent.Name + " has been added the SQL data base!");
        }

        public static void menu()
        {
            Console.WriteLine("1. View all the students");
            Console.WriteLine("2. Search for a student by their ID or name");
            Console.WriteLine("3. Update a student (by id)");
            Console.WriteLine("4. Add a new student");
            Console.WriteLine("5. Delete a student (by id)");
        }

        public static string GetUserInput(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine().ToLower().Trim();
            Console.Clear();
            return input;
        }

        public static int GetInteger(int maxChoices)
        {
            string input = "";
            int output = 0;
            try
            {
                input = Console.ReadLine();
                output = int.Parse(input);
                if (output > maxChoices || output < 1)
                {
                    throw new Exception("That number is out of range. Try again.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("That was not a valid input.");
                output = GetInteger(maxChoices);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                output = GetInteger(maxChoices);
            }
            return output;
        }

        public static void UpdateStudent(int id, StudentDatabaseContext db)
        {
            //Find the student by id 
            Student s = db.Students.Find(id);

            Console.WriteLine("1. Name");
            Console.WriteLine("2. Hometown");
            Console.WriteLine("3. Favorite Food");
            int choice = int.Parse(GetUserInput("What would you like to change?"));
            if (choice == 1)
            {
                string newName = GetUserInput("What is their new name?");
                s.Name = newName;
                db.Students.Update(s);
                db.SaveChanges();
                Console.WriteLine("Name: " + s.Name + "Has been updated in SQL");
            }
            else if (choice == 2)
            {
                string newHome = GetUserInput("What is their new home town?");
                s.HomeTown = newHome;
                db.Students.Update(s);
                db.SaveChanges();
                Console.WriteLine(s.Name + " home town has been updated");
            }
            else if (choice == 3)
            {
                string newFood = GetUserInput("What is their new Favorite Food?");
                s.FavoriteFood = newFood;
                db.Students.Update(s);
                db.SaveChanges();
                Console.WriteLine(s.Name + " home town has been updated");
            }

            //Pass their updates along and save changes
            db.Students.Update(s);
            db.SaveChanges();
        }

        public static void DeleteStudent(int id, StudentDatabaseContext db)
        {
            //Grab the student 
            Student s = db.Students.Find(id);

            //Tries to find and delete the student
            db.Students.Remove(s);

            db.SaveChanges();
        }

        public static List<Student> SearchStudentsByName(string name, StudentDatabaseContext db)
        {
            List<Student> results = db.Students.Where(x => x.Name.Contains(name)).ToList();

            return results;
        }

    }
}
