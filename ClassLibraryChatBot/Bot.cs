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
        public bool isFinal = false;

        Random randomizer = new Random();

        internal delegate string CommandMessage();
        internal CommandMessage cMes;

        static readonly List<Answer> lJokes = new List<Answer>();
        static readonly List<Answer> lAphorisms = new List<Answer>();
        static readonly List<Answer> lMeetings = new List<Answer>();
        static readonly List<Answer> lAnswers = new List<Answer>();
        static readonly List<Answer> lIniPhrases = new List<Answer>();
        static readonly List<Answer> lErrors = new List<Answer>();
        static readonly Dictionary<Question, CommandMessage> dCommands = new Dictionary<Question, CommandMessage>();
        static readonly List<Question> lQuestions = new List<Question>();
        List<int> lCrctIDAns;

        static readonly List<String> lPhrases = new List<string>();

        public Bot()
        {
            GetBase();
        }

        public string TakeAnswer(string asking)
        {

            asking = asking.ToLower();
            string answer_line = new string("");
            lPhrases.Clear();

            foreach (var command in dCommands)
                if (command.Key.Phrase == asking)
                {
                    return command.Value();
                }

            foreach (Question oneQuestion in lQuestions)
            {
                if (oneQuestion.Phrase == asking)
                {
                    foreach (Answer an in lMeetings)
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
            answer_line = TakeRandAnswer(lIniPhrases);
            string secondpart = TakeRandAnswer(lAphorisms);
            answer_line += " ";
            answer_line += secondpart;
            return answer_line;
        }

        string TakeRandFromList(List<String> lPhrases)
        {
            int r = randomizer.Next(0, lPhrases.Count());
            var an = lPhrases.Skip(r).Take(1).ToArray();
            return an[0];
        }

        string TakeRandAnswer(List<Answer> lAnswer)
        {
            int r = randomizer.Next(0, lAnswer.Count());
            var an1 = lAnswer.Skip(r).Take(1).ToArray() ?? lAnswers.Where(i => i.AnswerType == "err").ToArray();
            return an1[0].Phrase;
        }

        private void GetBase()
        {
            string type_ans = "";

            lQuestions.Add(new Question("..."));
            lQuestions.Add(new Question("хай"));
            lQuestions.Add(new Question("привет"));
            lQuestions.Add(new Question("приветствую"));
            lQuestions.Add(new Question("здравствуй"));
            lQuestions.Add(new Question("здравствуйте"));
            lQuestions.Add(new Question("добрый день"));
            lQuestions.Add(new Question("добрый вечер"));
            lQuestions.Add(new Question("доброе утро"));
            lQuestions.Add(new Question("доброй ночи"));

            type_ans = "com";

            dCommands.Add(new Question("как тебя зовут", type_ans), () => { return "Меня зовут " + m_BotName; });//9
            dCommands.Add(new Question("анекдот", type_ans), () => { return TakeRandAnswer(lJokes); });//10
            dCommands.Add(new Question("который час", type_ans), () => { return DateTime.Now.ToString().Split(" ").ToArray()[1]; });//11
            dCommands.Add(new Question("сколько времени", type_ans), () => { return DateTime.Now.ToString().Split(" ").ToArray()[1]; });//12
            dCommands.Add(new Question("какая сейчас дата", type_ans), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });//15
            dCommands.Add(new Question("брось кубик", type_ans), () => { return randomizer.Next(0, 6).ToString(); });//16
            dCommands.Add(new Question("подбрось кубик", type_ans), () => { return randomizer.Next(0, 6).ToString(); });//17
            dCommands.Add(new Question("брось монетку", type_ans), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });//18
            dCommands.Add(new Question("подбрось монетку", type_ans), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });//19
            dCommands.Add(new Question("пока", type_ans), () =>
            {
                isFinal = true;
                if (m_UserName != "")
                    return "до cвидания, " + m_UserName;
                else
                    return "до cвидания";
            }
               );
            dCommands.Add(new Question("до свидания"), () =>
            {
                isFinal = true;
                if (m_UserName != "")
                    return "до cвидания, " + m_UserName;
                else
                    return "до cвидания";
            }
               );

            type_ans = "ans";

            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Здравствуйте", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Хола", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Ни хао", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Халло", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Шалом", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Буенос диас", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Гутен таг", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Здраво", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 5, 6 }, "Добрый", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 8 }, "Доброй", type_ans));
            lMeetings.Add(new Answer(lCrctIDAns = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Аве!", type_ans));

            type_ans = "jok";

            lJokes.Add(new Answer("есть опыт учёбы, есть опыт работы, хочу ещё опыт зарплаты.", type_ans));
            lJokes.Add(new Answer("самая неподкупная очередь — в туалет", type_ans));
            lJokes.Add(new Answer("девушки как печеньки — ломаются, пока не намокнут", type_ans));
            lJokes.Add(new Answer("улыбайтесь — это всех раздражает!", type_ans));
            lJokes.Add(new Answer("не волнуйтесь, какое бы ни было у вас здоровье — его хватит до конца вашей жизни!", type_ans));
            lJokes.Add(new Answer("если хочешь выглядеть молодой и стройной — держись поближе к старым и толстым", type_ans));
            lJokes.Add(new Answer("три причины отсутствия студента на занятиях: забыл, запил, забил.", type_ans));
            lJokes.Add(new Answer("девушка не воробей, залетит мало не покажется.", type_ans));
            lJokes.Add(new Answer("труднее всего тянуть до зарплаты последние 3,5 недели.", type_ans));
            lJokes.Add(new Answer("всё, что нас не убивает — впоследствии очень сильно об этом жалеет.", type_ans));
            lJokes.Add(new Answer("русский язык без мата превращается в доклад.", type_ans));
            lJokes.Add(new Answer("если Вас окружают одни дураки, значит Вы центральный.", type_ans));
            lJokes.Add(new Answer("любовь — это когда ты не хочешь засыпать, потому что реальность лучше, чем сон.", type_ans));
            lJokes.Add(new Answer("родился сам — помоги другому.", type_ans));
            lJokes.Add(new Answer("если у тебя прекрасная жена, офигительная любовница, крутая тачка, нет проблем с властями и налоговыми службами, а когда ты выходишь на улицу всегда светит солнце и прохожие тебе улыбаются — скажи уже НЕТ наркотикам!", type_ans));
            lJokes.Add(new Answer("подбитый глаз уменьшает обзор, но увеличивает опыт.", type_ans));
            lJokes.Add(new Answer("детство заканчивается тогда, когда хочется, чтобы желания исполнял не Дед Мороз, а Снегурочка.", type_ans));
            lJokes.Add(new Answer("каждый человек по-своему прав, а по-моему — нет.", type_ans));
            lJokes.Add(new Answer("будет и на вашем кладбище праздник.", type_ans));
            lJokes.Add(new Answer("люди, имеющие большие деньги, либо охраняются полицией, либо разыскиваются ею.", type_ans));
            lJokes.Add(new Answer("сделать женщину счастливой не трудно, трудно самому при этом остаться счастливым.", type_ans));
            lJokes.Add(new Answer("положительные эмоции — это эмоции, которые возникают, если на всё положить.", type_ans));

            type_ans = "aph";

            lAphorisms.Add(new Answer("Все у меня идет по плану. Осталось узнать - чей это план.", type_ans));
            lAphorisms.Add(new Answer("Наступило лето. Девушки достали из шкафов голые коленки...", type_ans));
            lAphorisms.Add(new Answer("Выхожу сейчас из квартиры, а там двое прямо на лестничной площадке вакцинируются. Уважуха.", type_ans));
            lAphorisms.Add(new Answer("Финансовый совет: если вы не можете купить квартиру, просто унаследуйте ее.", type_ans));
            lAphorisms.Add(new Answer("Когда кот сидит у закрытой двери, это не просто кот. Это кот доступа!", type_ans));
            lAphorisms.Add(new Answer("Гюльчатай, сними масочку.", type_ans));
            lAphorisms.Add(new Answer("Индийское кино без песен и танцев - документальное.", type_ans));
            lAphorisms.Add(new Answer("Рабочий лайфхак: чтобы кола не пенилась при наливании в стакан, пейте виски.", type_ans));
            lAphorisms.Add(new Answer("Зарплата назывется еще и жалованием, потому что на нее все жалуются.", type_ans));
            lAphorisms.Add(new Answer("Прошу осудить мою зарплату за то, что она ограничивает мою свободу передвижения и причиняет мне физическую боль.", type_ans));
            lAphorisms.Add(new Answer("Добро пожаловать в мастерскую по ремонту айфонов «Яблочный спас»!", type_ans));
            lAphorisms.Add(new Answer("Koмпьютeрщик — этo eдинcтвeнный чeлoвeк, кoтoрый мoжeт пoпрocить у нaчaльникa двecти бaкcoв нa пaмять и иx пoлучить.", type_ans));
            lAphorisms.Add(new Answer("Когда захлопнулась ловушка. Я стал метаться и кричать. Но в паспорте уже стояла. Печать", type_ans));
            lAphorisms.Add(new Answer("Маньяк-филолог убивает только людей в польтах.", type_ans));
            lAphorisms.Add(new Answer("Любая школа автоматически получает статус гимназии, если трудовик и физрук закодируются.", type_ans));
            lAphorisms.Add(new Answer("Ненавижу перепады настроения, это просто потрясающе!", type_ans));
            lAphorisms.Add(new Answer("Пробка - это две русские беды в одном месте.", type_ans));
            lAphorisms.Add(new Answer("Heт, чтo зa люди, a? Зaшлa в гocти чaю пoпить — нa трeтий дeнь чувcтвую: кaк-тo нe oчeнь мнe тут и рaды…", type_ans));
            lAphorisms.Add(new Answer("На Василия, который двадцать лет живет в однокомнатной квартире с женой и тещей, комары уже не садятся.", type_ans));
            lAphorisms.Add(new Answer("Продавщица в хлебном отделе в моменты одиночества от нечего делать шевелит булками.", type_ans));
            lAphorisms.Add(new Answer("Продавщица в хлебном отделе в моменты одиночества от нечего делать шевелит булками.", type_ans));

            type_ans = "ini";

            lIniPhrases.Add(new Answer("Я вас не понял, поэтому лучше пошучу  :", type_ans));
            lIniPhrases.Add(new Answer("Моя твоя не понимайт, держи шутку - ", type_ans));
            lIniPhrases.Add(new Answer("Я не достаточно умён для такого. Но мудр - ", type_ans));
            lIniPhrases.Add(new Answer("Эээ... ", type_ans));
            lIniPhrases.Add(new Answer("Эээ... ", type_ans));

            type_ans = "err";
            lErrors.Add(new Answer("Ошибка  :", type_ans));
        }
    }
}
