using System;
using System.IO;
using System.Text;

namespace CSVfile
{
    internal class Program
    {
        /// <summary>
        /// Класс, содержащий все операции для обработки файла.
        /// </summary>
        class Data
        {
            // Массив строк, содержащий таблицу.
            private string[] _sheet;

            /// <summary>
            /// Конструктор по умолчанию.
            /// </summary>
            public Data() 
            {
                _sheet = new string[0];
            }
            /// <summary>
            /// Конструктор класса Data.
            /// </summary>
            /// <param name="path"> Путь к файлу </param>
            public Data(string[] file) 
            {
                this._sheet = file;
                
            }

            /// <summary>
            /// Метод проверки файлов на корректность.
            /// </summary>
            /// <returns> true, если файл корректен, иначе false </returns>
            public bool IsFileCorrect() 
            {
                if (_sheet[0] != "#,Name,Type 1,Type 2,Total,HP,Attack,Defense,Sp. Atk,Sp. Def,Speed,Generation,Legendary")
                {
                    return false;
                }
                // Переменные пустышки для проверки приведения типа.
                int inthelper;
                bool boolhelper;
                // В этом цикле проверяется соответствие типов данных эталонной таблицы и введенного пользователем файла.
                for (int i = 1; i < _sheet.Length; i++) 
                {
                    int hp, atk, def, spatk, spdef, speed;
                    string[] s = _sheet[i].Split(',');
                    if (s.Length != 13) { return false; }
                    if (!int.TryParse(s[0],out inthelper)) { return false; }
                    if (s[1] == null) { return false; }
                    if (s[2] == null) { return false; }
                    if (!int.TryParse(s[5], out hp)) { return false; }
                    if (!int.TryParse(s[6], out atk)) { return false; }
                    if (!int.TryParse(s[7], out def)) { return false; }
                    if (!int.TryParse(s[8], out spatk)) { return false; }
                    if (!int.TryParse(s[9], out spdef)) { return false; }
                    if (!int.TryParse(s[10], out speed)) { return false; }
                    // Проверка корректности суммы.
                    if (!int.TryParse(s[4], out int sum) || sum != (hp + atk + def + spatk + spdef + speed)) { return false; }
                    if (!int.TryParse(s[11], out inthelper)) { return false; }
                    if (!bool.TryParse(s[12], out boolhelper)) { return false; }
                }
                return true;
            }
            /// <summary>
            /// Функция сортировки покемонов по атаке.
            /// </summary>
            /// <param name="arr"> Неотсортированный массив строк. </param>
            /// <param name="atk"> Массив целочисленного параметра Attaсk всех покемонов из arr. </param>
            /// <returns> Отсортированный масств строк. </returns>
            public string[] PokemonAttackSort(string[] arr, int[] atk)
            {
                string temp;
                int atemp;
                for (int i = 0; i < arr.Length; i++)
                {
                    for (int j = i + 1; j < arr.Length; j++)
                    {
                        if (atk[i] > atk[j])
                        {
                            temp = arr[i];
                            arr[i] = arr[j];
                            arr[j] = temp;

                            atemp = atk[i];
                            atk[i] = atk[j];
                            atk[j] = atemp;
                        }
                    }
                }
                return arr;
            }
            /// <summary>
            /// Функция выполняющая вторую комманду пользователя.
            /// </summary>
            public void SecondCommand()
            {
                // Создание таблицы, в которую будет сохранена выборка всех ядовитых покемонов.
                string[] poisonous = new string[1];
                poisonous[0] = _sheet[0];
                // Заполнения массива poisonous.
                for (int i = 1; i < _sheet.Length; i++)
                {
                    string[] s = _sheet[i].Split(',');
                    if (s[2] == "Poison")
                    {
                        Array.Resize(ref poisonous, poisonous.Length + 1);
                        // Добавление нового элемента в конец массива.
                        poisonous[^1] = string.Join("", _sheet[i]); 
                    }
                }
                if (poisonous.Length > 1)
                {
                    // Запись таблицы всех ядовитых покемонов в файл.
                    File.WriteAllLines(@"Pokemon-Poison.csv", poisonous); 
                    // Приведение данных к читаемому виду и вывод в консоль.
                    for (int i = 0; i < poisonous.Length; i++)
                    {
                        poisonous[i] = poisonous[i].Replace(',', (char)32);
                        Console.WriteLine(poisonous[i]);
                    }
                }
                // Выполняется, если не был найден ни один искомый покемон.
                else
                {
                    File.WriteAllText(@"Pokemon-Poizon.csv", "No poisonous pockemons.");
                    Console.WriteLine("Отсутствуют ядовитые покемоны.");
                }
            }
            /// <summary>
            /// Функция выполняющая вторую команду пользователя.
            /// </summary>
            public void ThirdCommand()
            {
                // Словарь, принимающий Type 1 покемона в качетсве ключа,
                // и хранящий массив строк с информацией о покемонах этого типа.
                Dictionary<string, string[]> types = new Dictionary<string, string[]>(); 
                string[] helper = new string[1];
                // Заполнения словаря types.
                for (int i = 1; i < _sheet.Length; i++)
                {
                    string[] s = _sheet[i].Split(',');
                    // В качестве ключа используется Type 1, который хранится в третьем столбце.
                    if (!types.ContainsKey(s[2])) 
                    {
                        types[s[2]] = new string[0];
                    }
                    // Добавление нового "покемона" в массив "покемонов" того же типа.
                    string[] tmp = types[s[2]];
                    Array.Resize(ref tmp, tmp.Length + 1);
                    tmp[^1] = _sheet[i].Substring(_sheet[i].IndexOf(',')-1); 
                    types[s[2]] = tmp; 
                }
                
                string filename = @"Sorted-Pokemon.csv";
                StreamWriter writer = new StreamWriter(filename, false);
                Console.WriteLine(_sheet[0].Replace(',', (char)32));
                writer.WriteLine(_sheet[0]); 
                foreach (var type in types)
                {
                    // Извлечение массива с информацией о покемонах из словаря для упрощения сортировки и вывода.
                    string[] typelist = type.Value;

                    /* Массив для ускорения сортировки.
                    Он сделан, чтобы на каждом шаге сортировки не выполнять операцию приведения типов,
                    что приводит к экономии времени. */
                    int[] sortHelper = new int[typelist.Length];

                    // Заполнение массива sortHelper целочисленными значениями атаки покемонов. 
                    for (int i = 0; i < typelist.Length; i++)
                    {
                        string[] s = typelist[i].Split(',');
                        sortHelper[i] = (int.Parse(s[6]));
                    }
                    typelist = PokemonAttackSort(typelist, sortHelper);
                    // Вычисление разницы между наибольшей и наименьшей атакой в группе.
                    int delta = int.Parse(typelist[^1].Split(',')[6]) - int.Parse(typelist[0].Split(',')[6]); 
                   
                    Console.WriteLine($"{type.Key} attack delta: {delta}.");
                    // Вывод данных в консоль и в файл.
                    for (int i = 0; i < typelist.Length; i++) 
                    {
                        // Приведение данных к читаемому виду.
                        Console.WriteLine(($"{i+1}" + string.Join("", typelist[i].Substring(typelist[i].IndexOf(',')).Replace(',', (char)32))));
                        writer.WriteLine(($"{i + 1}," + string.Join("", typelist[i].Substring(typelist[i].IndexOf(',')+1))));
                    }
                }
                writer.Close(); 
            }
            /// <summary>
            /// Функция, выполняющая четвертую комманду пользователя.
            /// </summary>
            /// <param name="filename"> Имя файла для записи, полученное от пользователя. </param>
            public void FourthCommand(string filename)
            {
                string[] noType2 = new string[0];
                int hpSum = 0;
                // Поиск элементов без Type 2 и добавление их в массив.
                for (int i = 1; i < _sheet.Length; i++) 
                {
                    string[] s = _sheet[i].Split(',');
                    if (s[3] == "")
                    {
                        Array.Resize(ref noType2, noType2.Length + 1);
                        noType2[^1] = _sheet[i];
                        hpSum += int.Parse(s[5]);
                    }
                }
                double averageHp = 0;
                // Проверка, что нашелся хотя бы один искомый покемон.
                if (noType2.Length != 0) 
                {
                    averageHp = hpSum / noType2.Length;
                    // Вывод требуемой информации в консоль и в файл.
                    StreamWriter writer = new StreamWriter(filename, false); 
                    writer.WriteLine(_sheet[0]);
                    Console.WriteLine(_sheet[0].Replace(',', (char)32));
                    Console.WriteLine($"Average Pokemon without Type 2 HP: {averageHp}.");
                    for (int i = 1; i < noType2.Length; i++)
                    {
                        Console.WriteLine(noType2[i].Replace(',', (char)32));
                        writer.WriteLine(noType2[i]);
                    }
                    writer.Close();
                }
                // Выполняется, если не был найден ни один искомый покемон.
                else
                {
                    Console.WriteLine("Отсутствуют покемоны без Type 2.");
                    File.WriteAllText(filename, "No Pokemons without Type 2.");
                }
            }
            /// <summary>
            /// Функция, выолняющая пятую команду пользователя.
            /// </summary>
            public void FifthCommand()
            {
                Console.WriteLine($"a) Всего строк с данными о покемонах: {_sheet.Length - 1}.");
                // Создание идентичного ThirdCommand словаря
                Dictionary<string, string[]> types = new Dictionary<string, string[]>();
                // Словарь, в качетсве ключа принимающий Type 1 покемона, а в качестве значений,
                // хранящий количество покемонов, у которых любой из типов равен "Ghost"
                Dictionary<string, int> gostAmount = new Dictionary<string, int>(); 
                string[] helper = new string[1];
                // Заполнение types и ghostamount.
                for (int i = 1; i < _sheet.Length; i++) 
                {
                    string[] s = _sheet[i].Split(',');
                    if (!types.ContainsKey(s[2]))
                    {
                        types[s[2]] = new string[0];
                        gostAmount[s[2]] = 0;
                    }
                    // Добавление нового "покемона" в массив "покемонов" того же типа.
                    string[] tmp = types[s[2]];
                    Array.Resize(ref tmp, tmp.Length + 1);
                    tmp[^1] = _sheet[i].Substring(_sheet[i].IndexOf(',') - 1);
                    types[s[2]] = tmp;
                    // Увеличение счетчика покемонов типа Ghost в группе, если текущий покемон имеет этот тип.
                    if (s[2] == "Ghost" || s[3] == "Ghost")
                    {
                        gostAmount[s[2]]++;
                    }
                }
                Console.WriteLine($"b) Всего групп покемонов по основному типу: {types.Count()}.");
                Console.WriteLine("Количество покемонов в группах: ");
                // Вывод количества покемонов в каждой группе.
                foreach (var type in types) 
                {
                    Console.WriteLine($"{type.Key}: {type.Value.Length}");
                }
                Console.WriteLine("c) Количество покемонов с типом Ghost в каждой группе:");
                // Вывод количества призраков в каждой группе.
                foreach (var group in gostAmount) 
                {
                    Console.WriteLine($"{group.Key}: {group.Value}");
                }
                int flyingDragonsCnt = 0;
                // Проверка, что в файле присутствует хотя бы один покемон с типом Flying.
                if (types.ContainsKey("Flying")) 
                {
                    // Подсчет Flying Dragon покемонов.
                    for (int i = 0; i < types["Flying"].Length; i++) 
                    {
                        string[] s = types["Flying"][i].Split(',');
                        if (s[3] == "Dragon")
                        {
                            flyingDragonsCnt++;
                        }
                    }
                    Console.WriteLine($"d) Количество Покемонов с типами Flying Dragon: {flyingDragonsCnt}.");
                }
                else
                {
                    Console.WriteLine("Не найдено ни одного Flying покемона.");
                }
            }
        }
        /// <summary>
        /// Точка входа.
        /// </summary>
        static void Main(string[] args)
        {
            Data data = new Data();
            bool pathRecieved = false, fileIsCorrect = false;
            Console.WriteLine("Нажмите любую кнопку, чтобы начать.");
            // Создание меню до нажатия Esc.
            while (Console.ReadKey().Key != ConsoleKey.Escape) 
            {
                // Вывод меню на экран.
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Ввести адрес файла.");
                Console.WriteLine("2. Получить информацию о всех покемонах со свойством Poison.");
                Console.WriteLine("3. Получить список покемонов отсортированных по типу и атаке, а также максимальную разницу в силе покемнов типа.");
                Console.WriteLine("4. Получить список покемонов без дополнительных особенностей.");
                Console.WriteLine("5. Вывести статистику по файлу");
                Console.WriteLine("Введите номер команды: ");
                // Проверка корректности ввода комманды.
                if (!int.TryParse(Console.ReadLine(), out int command) || (command < 1 || command > 5)) 
                {
                    Console.WriteLine("Неверный ввод. Введите число от 1 до 5");
                    Console.WriteLine("Нажмите любую кнопку, чтобы продолжить или Esc, чтобы выйти.");
                }
                else
                {
                    if (command == 1)
                    {
                        Console.WriteLine("Введите адрес файла: ");
                        string path = Console.ReadLine();
                        // Проверка корректности пути.
                        if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1 && File.Exists(path))  
                        {
                            pathRecieved = true;
                            bool NoOneUsing = true;
                            string[] helper = new string[0];
                            // Проверка, что файл не используется другой программой.
                            try
                            {
                                helper = File.ReadAllLines(path, Encoding.UTF8);
                            }
                            catch
                            {
                                Console.WriteLine("Данный файл уже используется другой программой, закройте его и повторите операцию, чтобы продолжить работу.");
                                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить или Esc, чтобы выйти.");
                                NoOneUsing = false;
                            }
                            if (!NoOneUsing)
                            {
                                continue;
                            }
                            data = new Data(helper);
                            // Проверка корректности структуры файла.
                            if (NoOneUsing && data.IsFileCorrect()) 
                            {
                                Console.WriteLine("Файл получен.");
                                fileIsCorrect = true;
                            }
                            else
                            {
                                Console.WriteLine("Файл имеет некорректную структуру.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Введите корректный адрес файла!");
                        }
                    }
                    // Проверка получения исходного файла для выполнения дальнейших комманд.
                    else if (command != 1 && pathRecieved == false) 
                    {
                        Console.WriteLine("Неоткуда брать информацию, сначала выберите команду 1 и введите адрес файла.");
                    }
                    // Проверка является ли исходный файл корректным.
                    else if (command != 1 && fileIsCorrect == false) 
                    {
                        Console.WriteLine("Файл, полученный по введенному пути некорректен, сначала выберите команду 1 и введите адрес корректного файла.");
                    }
                    else if (command == 2)
                    {
                        data.SecondCommand();   
                    }
                    else if (command == 3)
                    {
                        data.ThirdCommand();
                    }
                    else if (command == 4)
                    {
                        Console.WriteLine("Введите имя файла для записи:");
                        string userFileName = Console.ReadLine();
                        // Проверка введенного пользователем имени файла на корректность.
                        if (userFileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 && userFileName.IndexOf('?') == -1 && userFileName.EndsWith(".csv")) 
                        {
                            data.FourthCommand(userFileName);
                        }
                        else
                        {
                            Console.WriteLine("Данное имя файла некорректно, еще раз выберите команду 4 и введите имя файла с расшиернием .csv.");
                        }
                    }
                    else if (command == 5)
                    {
                        data.FifthCommand();
                    }
                    Console.WriteLine("Нажмите любую кнопку, чтобы продолжить или Esc, чтобы выйти.");
                }
            }
            System.Environment.Exit(-1);
        }
    }
}