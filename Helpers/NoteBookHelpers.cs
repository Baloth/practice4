using NoteBook.Entities;

namespace NoteBook.Helpers
{
    internal class NoteBookHelpers
    {

        protected DateTime startDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        protected List<ToDoItem> defaultList = new();


        public List<NoteBookDate> FillNoteBook()
        {
            var notes = new List<NoteBookDate>();
            defaultList = GetToDoList();

            var endDate = startDate.AddDays(10);

            var currentDate = startDate;

            while (currentDate < endDate)
            {
                var nDate = new NoteBookDate()
                {
                    Date = currentDate,
                    NotebookItems = GenerateNewDefaultNBDateItem(currentDate)
                };
                notes.Add(nDate);
                currentDate = currentDate.AddDays(1);
            }


            return notes;
        }

        protected List<NoteBookDateItem>  GenerateNewDefaultNBDateItem(DateTime date)
        {  
            var nbItemList = new List<NoteBookDateItem>() { new NoteBookDateItem() { Id = 0, Name = "+ Создать заметку", Value = "", CreationDate = GetRandomDate(), InitialTime = InitialTime(date) } };

            if (defaultList.Count == 0)
                return nbItemList;

            var randomNum = new Random();
            int toDoCount = randomNum.Next(2, 4);
            var newList = new List<ToDoItem>();

            if (defaultList.Count >= toDoCount)
            {
                newList = defaultList.Take(toDoCount).ToList();
                defaultList.RemoveRange(0, toDoCount);
            }
            else
            {
                newList = defaultList;
                defaultList.Clear();
            }

            int id = 1;
            foreach(var item in newList)
            {
                nbItemList.Add(new NoteBookDateItem() { Id = id, Name = item.Name, Value = item.Description, CreationDate = GetRandomDate(), InitialTime = InitialTime(date) });
                id++;
            }

            return nbItemList;
        }


        public List<string> FillLegend()
        {
            return new List<string>() { 
                "Используйте стрелки вправо, влево для перемешения по датам календаря",
                "Используйте стрелки вверх, вниз для перемешения по пунктам меню",
                "Используйте Enter для просмотра описания заметки, создания новой или возврата в основное меню заметок",
                "Используйте Del для удаления заметки из списка",
                "Используйте ESC для выхода из календаря",
                "---------------------------------------------------------------"
            };

        }

        public NoteBookDate GenerateNewDate(DateTime date)
        {
            var nDate = new NoteBookDate()
            {
                Date = date,
                NotebookItems = new List<NoteBookDateItem>() { new NoteBookDateItem() { Id = 0, Name = "+ Создать заметку", Value = "", CreationDate = GetRandomDate(), InitialTime = InitialTime(date) } }
            };

            return nDate;
        }


        public DateTime GetRandomDate()
        {
            var randomDate = new Random();
            var startDate =  DateTime.Now.AddDays(-20);

            TimeSpan timeSpan =DateTime.Now.AddDays(-1)  - startDate;
            TimeSpan newSpan = new TimeSpan(0, randomDate.Next(0, (int)timeSpan.TotalMinutes), 0);
            DateTime newDate = startDate + newSpan;

            return newDate;
        }

        public DateTime InitialTime(DateTime date)
        {
            var randomNum = new Random();
            int hour = randomNum.Next(0, 23);
            int minute = randomNum.Next(0, 59);

            date = date.AddHours(hour).AddMinutes(minute);
           
            return date;
        }

        protected List<ToDoItem> GetToDoList()
        {
            var list = new List<ToDoItem>();
            list.Add(
                new ToDoItem() {
            Name= "Разделите список на три части: «обязательно», «нужно» и «хочется»",
            Description = "Начните составлять список с трех пунктов: 1) что вы должны сделать (безотлагательное и важное); 2) что сделать желательно (это важно для ваших долгосрочных целей); 3) что вы по-настоящему хотите сделать. Это поможет соблюсти баланс между краткосрочными и долгосрочными целями и включить в список что-то, что вас радует."
            });

            list.Add(
                new ToDoItem()
                {
                    Name = "Ведите отдельные списки в электронном виде и на бумаге",
                    Description = "Все мы хотим забыть о бумаге, но если вы заведете отдельный бумажный список дел, он дополнит тот, что вы ведете на компьютере. В цифровой список можно сваливать вообще все важные для вас дела, а бумажный тогда будет списком дел на каждый день. Это поможет сосредоточиться и не перегружаться."
                });

            list.Add(
                new ToDoItem()
                {
                    Name = "Правило 1-3-5",
                    Description = "Не делайте список слишком длинным. В повседневном списке стремитесь к такой структуре: одно большое дело, три средних и пять маленьких, которые вы можете выполнить за день. Альтернативой может служить правило 3+2: три больших и два маленьких дела."
                });

            list.Add(
                new ToDoItem()
                {
                    Name = "Удаляйте все и переписывайте список",
                    Description = "Со списком дел трудно управляться, если в нем есть задачи, которые вам, может, вообще не понадобиться выполнять. Чтобы не тратить время зря, пересматривайте свои обязательства: ежедневно пишите новый список (можно опираться на предыдущий) и включайте в него только то, что нужно сделать, а не то, что вы хотели бы сделать. Иначе он превратится не то в список ваших мечтаний, не то в кладбище для задач."
                });

            list.Add(
                new ToDoItem()
                {
                    Name = "Превратите свой список дел в историю",
                    Description = "Мысленно представьте себе свой список и превратите свой план действий в историю, в рассказ о своем дне. Эта техника не только мотивирует закончить дела, но и укрепит память и привнесет больше осмысленности в ваш график."
                });

            list.Add(
                new ToDoItem()
                {
                    Name = "Заведите отдельный список мелких дел",
                    Description = "Даже когда список дел хорошо организован и в нем правильно раставлены приорететы, все равно нас периодически настигает прокрастинация. На такие моменты, когда у вас недостаточно энергиии, стоит завести отдельный список дел, которые «хорошо было бы сделать». Это дела, которые можно сделать в любом настроении — навести порядок на столе или почитать профессиональные журналы."
                });

            list.Add(
                new ToDoItem()
                {
                    Name = "Вычеркните одно дело",
                    Description = "Иногда нужно просто сказать нет. Когда задач слишком много, нам не хватает пространства для креативности, отдыха или просто для размышлений. Если ваш список выглядит загроможденным и переполненным, не стесняйтесь удалить какое-нибудь из своих обязательств."
                });

            list.Add(
                new ToDoItem()
                {
                    Name = "Добавляйте приятные задачи для дополнительной мотивации",
                    Description = "Всегда хорошо, когда вас что-то подталкивает закончить дела. Таким импульсом может быть поощрение: например, если вы выполнили 10 самых важных дел, можно вздремнуть минут десять. Можно отсортировать задачи в списке, привязав к каждой что-то приятное или какую-то награду. Это лучше мотивирует."
                });
            list.Add(
                new ToDoItem()
                {
                    Name = "Метод Уоррена Баффета",
                    Description = "Хотите по-настоящему расставить приоритеты? Следуйте советам Уоррена Баффета: запишите топ-25 вещей, которые вы хотите сделать в будущем. Затем выберите 5 самых важных, а остальные добавьте в список «избегать любой ценой». И все ваши дела в каждодневном списке должны быть как-то связаны с пунктами из топ-5."
                });
            list.Add(
                new ToDoItem()
                {
                    Name = "Вместо списка составьте расписание",
                    Description = "Список дел позволяет отслеживать дела, которые нам нужно сделать, но незавершенные задачи в длинном списке мучают нас и делают несчастливыми. Может быть, тогда вместо списка стоит запрограммировать все свои задачи и с помощью сервиса вроде FollowUp.cc организовать напоминания по электронной почте. Наличие дедлайнов помогает больше успевать (ведь в этом основной смысл списка). Некоторым людям проще справляться с делами, когда над ними не довлеет список."
                });
        


            return list;
        }


    }
}
