using Labb_1_Avancerad.NET.Data;
using Labb_1_Avancerad.NET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_1_Avancerad.NET
{
    internal class Methods
    {
        public static void Run()
        {
            Console.WriteLine("Welcome!\nPress ENTER to continue");
            Console.ReadKey();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nChoose something below:");
                Console.WriteLine("[1] Math Teachers");
                Console.WriteLine("[2] Students + Teachers");
                Console.WriteLine("[3] 'Programming 1' exists?");
                Console.WriteLine("[4] Edit Subject name");
                Console.WriteLine("[5] Change teacher from Anas");
                Console.WriteLine("[6] All Subjects");
                Console.WriteLine("[7] Subjects + Teachers");
                Console.WriteLine("[8] Shut down the application");
                Console.Write("Answer: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        MathTeachers();
                        Utility();
                        break;

                    case "2":
                        Console.Clear();
                        PrintStudentsWithTeachers();
                        Utility();
                        break;

                    case "3":
                        Console.Clear();
                        containsProgramming();
                        Utility();
                        break;

                    case "4":
                        Console.Clear();
                        editSubject();
                        Utility();
                        break;

                    case "5":
                        Console.Clear();
                        fromAnasToReidar();
                        Utility();
                        break;

                    case "6":
                        Console.Clear();
                        GetAllSubjects();
                        Utility();
                        break;

                    case "7":
                        Console.Clear();
                        showAllTeachersAndTheirSubjects();
                        Utility(); 
                        break;

                    case "8":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }

            }
        }

        public static void MathTeachers()
        {
            using SDbContext context = new SDbContext();

            var mathTeachers = context.Teachers.Where(t => t.Subjects
                                                .Any(s => s.Name == "Math"))
                                                .ToList();

            Console.WriteLine("\nAll math teachers in this school:");
            foreach (var item in mathTeachers)
            {
                Console.WriteLine(item.Name);
            }
        }

        public static void PrintStudentsWithTeachers()
        {
            using (SDbContext context = new SDbContext())
            {
                var studentsWithTeachers = context.Students
                    .Join(context.Courses, student => student.CourseId, course => course.CourseId, (student, course) => new
                    {
                        Student = student,
                        Teachers = course.Subjects.SelectMany(subject => subject.Teachers).Distinct()
                    }).ToList();

                Console.WriteLine("Students with their teachers:");

                foreach (var item in studentsWithTeachers)
                {
                    Console.WriteLine($"\nStudent: {item.Student.Name}\nTeachers:");
                    foreach (var teacher in item.Teachers)
                    {
                        Console.WriteLine(teacher.Name);
                    }
                }

            }
        }

        public static void containsProgramming()
        {
            using SDbContext context = new SDbContext();

            bool containsProgramming1 = context.Subjects.Any(s => s.Name == "Programming 1");

            if (containsProgramming1)
            {
                Console.WriteLine("\nProgramming 1 Exists!");

            }
            else
            {
                Console.WriteLine("\nProgramming 1 DOESN'T exist! ");
            }
        }

        public static void editSubject()
        {
            using SDbContext context = new SDbContext();

            var subjectToUpdate = context.Subjects.FirstOrDefault(s => s.Name == "Programming 2");

            if (subjectToUpdate != null)
            {
                Console.WriteLine($"\nSubject: {subjectToUpdate.Name} found! Are you sure you want to edit 'Programming 2' to 'OOP'?\nYes[1] or No[2]");
                string answer = Console.ReadLine();

                if (answer == "1")
                {
                    subjectToUpdate.Name = "OOP";
                    context.SaveChanges();
                    Console.WriteLine("\nProgramming 2 has been edited to OOP.");
                }
                else if (answer == "2")
                {
                    Console.WriteLine("\nNo changes has been made.");
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Press ENTER to continue");
                }


            }
            else
            {
                Console.WriteLine("\nThere isn't any subject to edit. No changes has been made.");
            }
        }

        public static void fromAnasToReidar()
        {
            using SDbContext context = new SDbContext();

            int anasId = 1;
            int reidarId = 2;

            var anasTeacher = context.Teachers
                                 .Include(t => t.Subjects)
                                 .ThenInclude(subject => subject.Teachers)
                                 .FirstOrDefault(t => t.TeacherId == anasId);

            var reidarTeacher = context.Teachers.FirstOrDefault(t => t.TeacherId == reidarId);

            if (reidarTeacher == null)
            {
                Console.WriteLine("\nThe teacher Reidar was not found. Creating a new teacher called 'Reidar'");
                reidarTeacher = new Teacher { Name = "Reidar" };
                context.Teachers.Add(reidarTeacher);
                context.SaveChanges();
                Console.WriteLine("Reidar has been added to the Database");
            }

            if (anasTeacher != null)
            {
                if (anasTeacher.Subjects.Count == 0)
                {
                    Console.WriteLine("\nAnas doesn't teach any subject at the moment. ");
                }
                else
                {
                    Console.WriteLine("\nListing all subjects currently taught by Anas:");
                    foreach (var subject in anasTeacher.Subjects)
                    {
                        Console.WriteLine($"Subject: {subject.Name}");
                    }

                    Console.WriteLine("\nDo you want to change all these subjects to be taught by Reidar? (yes/no)");
                    string input = Console.ReadLine();
                    if (input.ToLower() == "yes")
                    {
                        foreach (var subject in anasTeacher.Subjects)
                        {
                            if (subject.Teachers.Contains(anasTeacher))
                            {
                                subject.Teachers.Remove(anasTeacher);
                                if (!subject.Teachers.Contains(reidarTeacher))
                                {
                                    subject.Teachers.Add(reidarTeacher);
                                }
                            }
                        }
                        context.SaveChanges();
                        Console.WriteLine("\nAll subjects have been updated to have Reidar as a teacher.");
                    }
                    else
                    {
                        Console.WriteLine("\nNo changes have been made.");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nAnas was not found in the database.");
            }
        }



        public static void GetAllSubjects()
        {
            using SDbContext context = new SDbContext();

            var allSubjects = context.Subjects.ToList();

            if (allSubjects.Count == 0)
            {
                Console.WriteLine("\nThere are no subjects in this database.");
            }
            else
            {
                Console.WriteLine("\nHere is a list of all subjects:");
                foreach (var subject in allSubjects)
                {
                    Console.WriteLine($"{subject.SubjectId}: {subject.Name}");
                }
            }
        }

        public static void showAllTeachersAndTheirSubjects()
        {
            using (SDbContext context = new SDbContext())
            {
                var teachers = context.Teachers
                    .Include(t => t.Subjects)
                    .ToList(); 

                if (!teachers.Any())
                {
                    Console.WriteLine("\nNo teachers found in the database.");
                    return;
                }

                Console.WriteLine("\nListing all teachers and the subjects they teach:");
                foreach (var teacher in teachers)
                {
                    Console.WriteLine($"\n{teacher.Name} teaches the following subjects:");
                    foreach (var subject in teacher.Subjects)
                    {
                        Console.WriteLine($"- {subject.Name}");
                    }
                }
            }
        }

        public static void Utility()
        {
            {
                Console.WriteLine("\nPress ENTER to continue");
                Console.ReadKey();
            }
        }
    }
}

