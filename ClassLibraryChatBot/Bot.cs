using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClassLibraryChatBot
{
    public class Bot
    {

        private static string m_BotName = "Шарпик";
        private string m_UserName = new String("");
        private bool m_IsFinal = false;
        public bool IsFinal
        {
            get { return m_IsFinal; }
        }

        Random randomizer = new Random();

        internal delegate string CommandMessage();

         readonly List<Answer> Jokes = new List<Answer>();
         readonly List<Answer> Aphorisms = new List<Answer>();
         readonly List<Answer> Meetings = new List<Answer>();
         readonly List<Answer> Answers = new List<Answer>();
         readonly List<Answer> InitPhrases = new List<Answer>();
         readonly List<Answer> Errors = new List<Answer>();
         readonly Dictionary<Question, CommandMessage> Commands = new Dictionary<Question, CommandMessage>();
         readonly List<Question> listQuestions = new List<Question>();
            List<int> ListCrctIDAns;

        static readonly List<String> lPhrases = new List<string>();

        public Bot()
        {
            GetBase();
        }


        //главный метод, принимающий вопрос пользователя и выдающиий подходящий ответ из базы
        public string TakeAnswer(string asking)
        {

            asking = asking.ToLower();
            string answer_line = String.Empty;
            lPhrases.Clear();

            foreach (var command in Commands)
                if (command.Key.Phrase == asking)
                {
                    return command.Value();
                }

            foreach (Question oneQuestion in listQuestions)
            {
                if (oneQuestion.Phrase == asking)
                {
                    foreach (Answer an in Meetings)
                    {
                        foreach (int id in an.questionIDs)
                        {
                            if (oneQuestion.ID == id)
                            {
                                lPhrases.Add(an.Phrase);
                            }
                        }

                    }
                    var va = TakeRandFromList(lPhrases);
                    va ??= "NAN";
                    return va;
                }
            }
            answer_line = TakeRandAnswer(InitPhrases);
            string secondpart = TakeRandAnswer(Aphorisms);
            answer_line += " ";
            answer_line += secondpart;
            return answer_line;
        }


        //выбрать случайный ответ из выборки ответов
        string TakeRandFromList(List<String> lPhrases)
        {
            int r = randomizer.Next(0, lPhrases.Count());
            var an = lPhrases.Skip(r).Take(1).ToArray();
            return an[0];
        }

        //выбрать случайный ответ из всей переданной базы
        string TakeRandAnswer(List<Answer> lAnswer)
        {
            int r = randomizer.Next(0, lAnswer.Count());
            var an1 = lAnswer.Skip(r).Take(1).ToArray() ?? Answers.Where(i => i.AnswerType == "err").ToArray();
            return an1[0].Phrase;
        }


        //выгрузка базы со всеми вопросами-ответами
        private void GetBase()// да-знаю что дохрена. с базой бы сократилось до строчек 30. 
        {
            string type_ans = string.Empty;

            Question qBuff;
            qBuff = new Question("...");
            listQuestions.Add(qBuff);

            qBuff = new Question("хай");
            listQuestions.Add(qBuff);

            qBuff = new Question("привет");
            listQuestions.Add(qBuff);

            qBuff = new Question("приветствую");
            listQuestions.Add(qBuff);

            qBuff = new Question("здравствуй");
            listQuestions.Add(qBuff);

            qBuff = new Question("здравствуйте");
            listQuestions.Add(qBuff);

            qBuff = new Question("добрый день");
            listQuestions.Add(qBuff);

            qBuff = new Question("добрый вечер");
            listQuestions.Add(qBuff);

            qBuff = new Question("доброе утро");
            listQuestions.Add(qBuff);

            qBuff = new Question("доброй ночи");
            listQuestions.Add(qBuff);

            type_ans = "com";
            
            qBuff=new Question("как тебя зовут", type_ans);
            Commands.Add(qBuff, () => { return "Меня зовут " + m_BotName; });//9

            qBuff = new Question("анекдот", type_ans);
            Commands.Add(qBuff, () => { return TakeRandAnswer(Jokes); });//10

            qBuff = new Question("который час", type_ans);
            Commands.Add(qBuff, () => { return DateTime.Now.ToString().Split(" ").ToArray()[1]; });//11

            qBuff = new Question("сколько времени", type_ans);
            Commands.Add(qBuff, () => { return DateTime.Now.ToString().Split(" ").ToArray()[1]; });//12

            qBuff = new Question("какая сейчас дата", type_ans);
            Commands.Add(qBuff, () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });//15

            qBuff = new Question("брось кубик", type_ans);
            Commands.Add(qBuff, () => { return randomizer.Next(0, 6).ToString(); });//16

            qBuff = new Question("подбрось кубик", type_ans);
            Commands.Add(qBuff, () => { return randomizer.Next(0, 6).ToString(); });//17

            qBuff = new Question("брось монетку", type_ans);
            Commands.Add(qBuff, () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });//18

            qBuff = new Question("подбрось монетку", type_ans);
            Commands.Add(qBuff, () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });//19

            qBuff=new Question("пока", type_ans);
            Commands.Add(qBuff, () =>
            {
                m_IsFinal = true;
                if (m_UserName != "")
                    return "до cвидания, " + m_UserName;
                else
                    return "до cвидания";
            }
               );


            qBuff=new Question("до свидания");
            Commands.Add(qBuff, () =>
            {
                m_IsFinal = true;
                if (m_UserName != "")
                    return "до cвидания, " + m_UserName;
                else
                    return "до cвидания";
            }
               );


            type_ans = "ans";

            Answer aBuff;
            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Здравствуйте", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Хола", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Ни хао", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Халло", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Шалом", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Буенос диас", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Гутен таг", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Здраво", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 5, 6 };
            aBuff = new Answer(ListCrctIDAns, "Добрый", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 8 };
            aBuff = new Answer(ListCrctIDAns, "Доброй", type_ans);
            Meetings.Add(aBuff);

            ListCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            aBuff = new Answer(ListCrctIDAns, "Аве!", type_ans);
            Meetings.Add(aBuff);


            type_ans = "jok";
            aBuff = new Answer("есть опыт учёбы, есть опыт работы, хочу ещё опыт зарплаты.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("самая неподкупная очередь — в туалет", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("девушки как печеньки — ломаются, пока не намокнут", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("улыбайтесь — это всех раздражает!", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("не волнуйтесь, какое бы ни было у вас здоровье — его хватит до конца вашей жизни!", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("если хочешь выглядеть молодой и стройной — держись поближе к старым и толстым", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("три причины отсутствия студента на занятиях: забыл, запил, забил.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("девушка не воробей, залетит мало не покажется.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("труднее всего тянуть до зарплаты последние 3,5 недели.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("всё, что нас не убивает — впоследствии очень сильно об этом жалеет.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("русский язык без мата превращается в доклад.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("если Вас окружают одни дураки, значит Вы центральный.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("любовь — это когда ты не хочешь засыпать, потому что реальность лучше, чем сон.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("родился сам — помоги другому.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("если у тебя прекрасная жена, офигительная любовница, крутая тачка, нет проблем с властями и налоговыми службами, а когда ты выходишь на улицу всегда светит солнце и прохожие тебе улыбаются — скажи уже НЕТ наркотикам!", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("подбитый глаз уменьшает обзор, но увеличивает опыт.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("детство заканчивается тогда, когда хочется, чтобы желания исполнял не Дед Мороз, а Снегурочка.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("каждый человек по-своему прав, а по-моему — нет.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("будет и на вашем кладбище праздник.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("люди, имеющие большие деньги, либо охраняются полицией, либо разыскиваются ею.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("сделать женщину счастливой не трудно, трудно самому при этом остаться счастливым.", type_ans);
            Jokes.Add(aBuff);

            aBuff = new Answer("положительные эмоции — это эмоции, которые возникают, если на всё положить.", type_ans);
            Jokes.Add(aBuff);


            type_ans = "aph";

            aBuff = new Answer("Все у меня идет по плану. Осталось узнать - чей это план.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Наступило лето. Девушки достали из шкафов голые коленки...", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Выхожу сейчас из квартиры, а там двое прямо на лестничной площадке вакцинируются. Уважуха.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Финансовый совет: если вы не можете купить квартиру, просто унаследуйте ее.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Когда кот сидит у закрытой двери, это не просто кот. Это кот доступа!", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Гюльчатай, сними масочку.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Индийское кино без песен и танцев - документальное.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Рабочий лайфхак: чтобы кола не пенилась при наливании в стакан, пейте виски.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Зарплата назывется еще и жалованием, потому что на нее все жалуются.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Прошу осудить мою зарплату за то, что она ограничивает мою свободу передвижения и причиняет мне физическую боль.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Добро пожаловать в мастерскую по ремонту айфонов «Яблочный спас»!", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Koмпьютeрщик — этo eдинcтвeнный чeлoвeк, кoтoрый мoжeт пoпрocить у нaчaльникa двecти бaкcoв нa пaмять и иx пoлучить.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Когда захлопнулась ловушка. Я стал метаться и кричать. Но в паспорте уже стояла. Печать", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Маньяк-филолог убивает только людей в польтах.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Любая школа автоматически получает статус гимназии, если трудовик и физрук закодируются.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Ненавижу перепады настроения, это просто потрясающе!", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Пробка - это две русские беды в одном месте.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Heт, чтo зa люди, a? Зaшлa в гocти чaю пoпить — нa трeтий дeнь чувcтвую: кaк-тo нe oчeнь мнe тут и рaды…", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("На Василия, который двадцать лет живет в однокомнатной квартире с женой и тещей, комары уже не садятся.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Продавщица в хлебном отделе в моменты одиночества от нечего делать шевелит булками.", type_ans);
            Aphorisms.Add(aBuff);

            aBuff = new Answer("Продавщица в хлебном отделе в моменты одиночества от нечего делать шевелит булками.", type_ans);
            Aphorisms.Add(aBuff);

            type_ans = "ini";

            aBuff = new Answer("Я вас не понял, поэтому лучше пошучу  :", type_ans);
            InitPhrases.Add(aBuff);

            aBuff = new Answer("Моя твоя не понимайт, держи шутку - ", type_ans);
            InitPhrases.Add(aBuff);

            aBuff = new Answer("Я не достаточно умён для такого. Но мудр - ", type_ans);
            InitPhrases.Add(aBuff);

            aBuff = new Answer("Эээ... ", type_ans);
            InitPhrases.Add(aBuff);

            aBuff = new Answer("Эээ... ", type_ans);
            InitPhrases.Add(aBuff);


            type_ans = "err";
            Errors.Add(new Answer("Ошибка  :", type_ans));
        }
    }
}
