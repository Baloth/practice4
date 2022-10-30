using System.Globalization;
using NoteBook.Entities;
using NoteBook.Helpers;

namespace NoteBook.Commands
{
    internal class MainNoteBookCommand
    {
        private List<NoteBookDate>  nbList = new();
        private NoteBookDate currentDate = new();
        private List<string> legendList = new();

        private string padding = "   ";
        private int cursorPosition = 0;
        private int startIndex = 0;
        private bool editMode = false;
        

        public void InitNoteBook()
        {
            nbList = new NoteBookHelpers().FillNoteBook();
            legendList = new NoteBookHelpers().FillLegend();

            cursorPosition = legendList.Count + 1;
            currentDate = nbList.First();

            ShowLegend();
            ShowNoteBookDate();

        }

        private void ShowLegend()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            foreach (var item in legendList)
            {
                Console.WriteLine(padding + item);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }


        private void ShowNoteBookDate()
        {
            Console.CursorVisible = false;
            Console.WriteLine(padding + currentDate.Date.ToString("dd.MM.yyyy"));
            ShowNoteBookDateItems();
            ConsoleKey key = Console.ReadKey().Key;

            while (key != ConsoleKey.Escape)
            {
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        {
                            //перемещение по календарю
                            if (editMode)
                                break;

                            startIndex = 0;

                            var newNBDate = nbList.Where(x => x.Date == currentDate.Date.AddDays(-1)).FirstOrDefault();
                            if (newNBDate != null)
                            {
                                currentDate = newNBDate;
                                ChangeNoteBookDate();
                                ShowNoteBookDateItems();
                            }
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            //перемещение по календарю
                            if (editMode)
                                break;

                            startIndex = 0;

                            var newNBDate = nbList.Where(x => x.Date == currentDate.Date.AddDays(1)).FirstOrDefault();
                            if (newNBDate != null)
                            {
                                currentDate = newNBDate;
                            }
                            else
                            {
                                currentDate = new NoteBookHelpers().GenerateNewDate(currentDate.Date.AddDays(1));
                                nbList.Add(currentDate);
                            }

                            ChangeNoteBookDate();
                            ShowNoteBookDateItems();
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            //перемещение по меню
                            if (editMode)
                                break;

                            if (startIndex > 0)
                            {
                                startIndex--;
                                NavigateMenu(+1);
                            }

                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (editMode)
                                break;

                            //перемещение по меню
                            if (currentDate.NotebookItems != null && (startIndex < currentDate.NotebookItems.Count - 1))
                            {
                                startIndex++;
                                NavigateMenu(-1);
                            }

                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (editMode)
                            {
                                if (startIndex > 0)
                                {
                                    editMode = false;
                                    ChangeNoteBookDate();
                                    ShowNoteBookDateItems();
                                }
                            }
                            else
                            {
                                editMode = true;
                                ChangeNoteBookDate();

                                if (startIndex == 0)
                                {
                                    CreateNewNBDateItem();
                                    ChangeNoteBookDate();
                                    ShowNoteBookDateItems();
                                }
                                else
                                {
                                    ShowFullInfoNoteBookDateItem();
                                }
                            }


                            break;
                        }
                    case ConsoleKey.Delete:
                        {
                            if (editMode || startIndex < 1)
                                break;


                            var nbItem = currentDate.NotebookItems.Take(startIndex+1).LastOrDefault();
                            if (nbItem != null && nbItem.Id > 0)
                            {
                                currentDate.NotebookItems.Remove(nbItem);
                                startIndex = 0;
                                ChangeNoteBookDate();
                                ShowNoteBookDateItems();
                            }

                            break;
                        }
                    default:
                        {
                            if (editMode)
                                break;

                            
                            startIndex = 0;
                            break;
                        }
                }

                key = Console.ReadKey().Key;
            }
        }


        private void ChangeNoteBookDate()
        {
            Console.Clear();
            ShowLegend();
            Console.WriteLine(padding + currentDate.Date.ToString("dd.MM.yyyy"));
        }

        private void ShowNoteBookDateItems()
        {
            foreach (var item in currentDate.NotebookItems)
            {
                Console.WriteLine(padding + item.Name);
            }
            Console.SetCursorPosition(0, cursorPosition + startIndex);
            Console.Write("=>");

        }

        private void NavigateMenu(int navIndex)
        {
            Console.SetCursorPosition(0, cursorPosition + startIndex);
            Console.Write("=>");
            Console.SetCursorPosition(0, cursorPosition + startIndex + navIndex);
            Console.Write("  ");
        }

        private void CreateNewNBDateItem()
        {
            Console.CursorVisible = true;
            Console.WriteLine(padding + "Создание новой заметки");
            Console.WriteLine(padding + "Введите назание заметки:");
            Console.SetCursorPosition(3, Console.CursorTop);


            var name = Console.ReadLine();
            while (string.IsNullOrEmpty(name))
            {
                Console.WriteLine(padding + "Введите назание заметки:");
                Console.SetCursorPosition(3, Console.CursorTop);
                name = Console.ReadLine();
            }

            Console.WriteLine(padding + "Напишите описание заметки:");
            Console.SetCursorPosition(3, Console.CursorTop);
            var value = Console.ReadLine();
            while (string.IsNullOrEmpty(value))
            {
                Console.WriteLine(padding + "Напишите описание заметки:");
                Console.SetCursorPosition(3, Console.CursorTop);
                value = Console.ReadLine();
            }


            Console.WriteLine(padding + "Укажите время выполнения в формате чч:мм (00:00)");
            Console.SetCursorPosition(3, Console.CursorTop);
            var time = Console.ReadLine();
            var initialTime = currentDate.Date;
            var errTime = true;

            while (errTime)
            {
                if (!string.IsNullOrEmpty(time) && time.Contains(':') && time.Length == 5)
                {
                    try
                    {
                        DateTime dateTime = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
                        initialTime = initialTime.AddHours(dateTime.Hour).AddMinutes(dateTime.Minute);
                        errTime = false;
                        break;
                    }
                    catch { }
                }

                Console.WriteLine(padding + "Ошибка в указании времени");
                Console.WriteLine(padding + "Укажите время выполнения в формате чч:мм (00:00)");
                Console.SetCursorPosition(3, Console.CursorTop);
                time = Console.ReadLine();
            }


            var lastId = currentDate.NotebookItems.Last().Id + 1;

            var newItem = new NoteBookDateItem()
            {
                Id = lastId,
                Name = name,
                Value = value,
                CreationDate = DateTime.Now,
                InitialTime = initialTime
            };

            currentDate.NotebookItems.Add(newItem);

            editMode = false;
            startIndex = currentDate.NotebookItems.Count-1;
            Console.CursorVisible = false;
        }

        private void ShowFullInfoNoteBookDateItem()
        {
            if (currentDate.NotebookItems != null)
            {
                var currentnbItem = currentDate.NotebookItems.Take(startIndex + 1).LastOrDefault();

                if (currentnbItem != null)
                {
                    Console.WriteLine();
                    Console.WriteLine(padding + "Дата создания заметки: " + currentnbItem.CreationDate);
                    Console.WriteLine();
                    Console.WriteLine(padding + "Время срабатывания: " + currentnbItem.InitialTime.ToString("dd:MM:yyy HH:mm"));
                    Console.WriteLine();
                    Console.WriteLine(padding + currentnbItem.Name);
                    Console.WriteLine();
                    Console.WriteLine(padding + currentnbItem.Value);
                    return;
                }
                
            }
            
            Console.WriteLine(padding + "не найдена запись");
        }
    }
}
