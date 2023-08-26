using System.Collections.Generic;
using System;

namespace Platformer
{
    public class Localization : ILocalization
    {
        private Dictionary<ETexts, string> Texts = new Dictionary<ETexts, string>();

        public Localization()
        {
            var type = typeof(ETexts);
            foreach (ETexts item in Enum.GetValues(type))
            {
                Texts.Add(item, "");
            }

            LoadLocalization(ELocalizations.English);
            //LoadLocalization(ELocalizations.Russian);
        }

        public string Text(ETexts key)
        {
            return Texts[key];
        }

        public void LoadLocalization(ELocalizations localization)
        {
            if (localization == ELocalizations.English)
            {
                LoadEnglish();
            }

            if (localization == ELocalizations.Russian)
            {
                LoadRussian();
            }
        }

        private void LoadEnglish()
        {
            Texts[ETexts.Enter] = "ENTER 'C'";
            Texts[ETexts.Exit] = "EXIT 'C'";
            Texts[ETexts.Next] = "Next 'C'";
            Texts[ETexts.Talk] = "TALK 'C'";
            Texts[ETexts.TalkToMom] = "TALK TO MOM 'C'";
            Texts[ETexts.TalkToMomAgain] = "TALK TO MOM AGAIN";
            Texts[ETexts.PullLever] = "PULL LEVER 'C'";
            Texts[ETexts.VillageCommoner] = "* Village Commoner *";
            Texts[ETexts.UpgradeHealthTip] = "STRENGTHEN HEALTH";
            Texts[ETexts.UpgradeHealthTitle] = "* Strengthen Health *";
            Texts[ETexts.KindleFire] = "KINDLE A CAMPFIRE";
            Texts[ETexts.RemoveObstacle] = "REMOVE OBSTACLE";

            Texts[ETexts.English] = "English";
            Texts[ETexts.Russian] = "Русский";

            Texts[ETexts.TryAgain] = "Try Again";
            Texts[ETexts.ToMenu] = "Menu";

            Texts[ETexts.PieDialogue1] = "* A Pie for a Grandma *";
            Texts[ETexts.DialoguePie1_1] = "- Hello, mom!";
            Texts[ETexts.DialoguePie1_2] = "- Guten Tag, meine Dear.";
            Texts[ETexts.DialoguePie1_3] = "- You.. called me? ))";
            Texts[ETexts.DialoguePie1_4] = "- Oh ja, Ich hasse ein Request fur dich. Could you bringst mich Schrooms und Berries for a Pie?";
            Texts[ETexts.DialoguePie1_5] = "- Sure, I'll do it! See ya )";
            Texts[ETexts.DialoguePie1_6] = "- Be careful, meine Hertz.";

            Texts[ETexts.FamilyMonologue] = "* My Wonderful Family *";
            Texts[ETexts.Family_1] = "It's about my mom.";
            Texts[ETexts.Family_2] = "She came from far away country called Stiria, and still didn't get used to local customs. Even couldn't get rid of her foreign accent.";
            Texts[ETexts.Family_3] = "Though sometimes she behave weird, and doesn't even distinguish her own husband from a mere bear in the forest (I love you, dad!), she is a nice person!";
            Texts[ETexts.Family_4] = "Today I'm going to collect some stuff for my mother to make a treat for Granny! Hope to meet there daddy too ))";

            Texts[ETexts.FatherDialogue] = "* Unexpected Encounter *";
            Texts[ETexts.Father_1] = "GRRRRRRRrrr !!";
            Texts[ETexts.Father_2] = "Uwaaa !!";
            Texts[ETexts.Father_3] = "Helllooo, daddy !";
            Texts[ETexts.Father_4] = "GRRRrrr.";
            Texts[ETexts.Father_5] = "I'm fine. Mom is good too. She's been missing you lately. Come back home soon.";
            Texts[ETexts.Father_6] = "Grwaah, huwaaa.";
            Texts[ETexts.Father_7] = "Did you fix the bridge? How nice of you! Then I'll better go home, bye!";

            Texts[ETexts.PieDialogue2] = "* Ingredients collected *";
            Texts[ETexts.DialoguePie2_1] = "- I've brought you everything you asked for, let's cook (- o -)/";
            Texts[ETexts.DialoguePie2_2] = "- Ah, danke, meine Dear! Hast du some Rest wilst I bake a Pie..";
            Texts[ETexts.DialoguePie2_3] = "(You'll sure be liking it, old Berta, just wait a little bit more)";
            Texts[ETexts.DialoguePie2_4] = "- Did you say something, mother?";
            Texts[ETexts.DialoguePie2_5] = "- What? Oh, das ist nicht, der Pie ist ready. Go, take it.";
            Texts[ETexts.DialoguePie2_6] = "- Yay!";

            Texts[ETexts.UpgradeHealth1_1] = "- Ich weise how you liebst sweet, but if you findest any Candies, eat them nicht! Das ist definitively a Witch's Bait fur Kinder.";
            Texts[ETexts.UpgradeHealth1_2] = "Besser du bringst them to me und ich cooke something gut fur strengthening deinе Health.";
            Texts[ETexts.UpgradeHealth1_3] = "- Haaa? (my poor candies!) What for on Earth I have to strenghten my health?! I'm healthy enough!";
            Texts[ETexts.UpgradeHealth1_4] = "- Don't be capricious, meine Kinder.";
            Texts[ETexts.UpgradeHealth1_5] = "- Ok, I get it... (T__T)";
            Texts[ETexts.UpgradeHealth2_1] = "If you hast at least 10 Candies, I'll make it fur dich!";
            Texts[ETexts.UpgradeHealth2_2] = "Sehr gut! Here you are!";
            Texts[ETexts.UpgradeHealth2_3] = "Ohh, du hast nicht genug! Bringst mich more )";

            Texts[ETexts.BlacksmithTitle1] = "* Blacksmith's Errand *";
            Texts[ETexts.Blacksmith1_1] = "- Meister Schmied! Please, craft a new shiny grey key for me!";
            Texts[ETexts.Blacksmith1_2] = "- Don't you rush, young lady. I'm pretty busy right now, have to polish my anvil...";
            Texts[ETexts.Blacksmith1_3] = "- Please, please, pleasepleasepleasee!";
            Texts[ETexts.Blacksmith1_4] = "- Okay, okay, why so loud? Well, I can make the key, but I need you to bring me, let's say, 3 pieces of iron ore. That should be enough for a deal.";
            Texts[ETexts.Blacksmith1_5] = "- Fine, I'll bring it to you then!";

            Texts[ETexts.Blacksmith2_1] = "- So you brought me what I want?";
            Texts[ETexts.Blacksmith2_2] = "- Sure, take it! (stupid rocks!)";
            Texts[ETexts.Blacksmith2_3] = "- Ok, here is your key, i just finished it.";
            Texts[ETexts.Blacksmith2_4] = "- Emm.. still working on it!";

            Texts[ETexts.Commoner1_1] = "Market elevator is not working now, come back later.";
            Texts[ETexts.Commoner1_2] = "Don't walk around the ruins, it's dangerous!";
            Texts[ETexts.GatekeeperTitle] = "* Gatekeeper *";
            Texts[ETexts.Gatekeeper1_1] = "If you want to pass freely through this gate, go ask the smith to make you a new grey key, since I've lost a spare one.";
            Texts[ETexts.CommonerSecret] = "Psst! There is a secret place in the village. You can only reach it by crushing the cursed wall. You obviousely need a holy weapon for it...";
            Texts[ETexts.WomanCandy] = "Here you are, darling, take this!";
            Texts[ETexts.ShopkeeperTitle] = "* Shopkeeper *";
            Texts[ETexts.Shopkeeper1] = "Oh, young lady! If you happen to find this shiny circles, don't throw them out. I can give you something really useful in exchange!";
        }

        private void LoadRussian()
        {
            Texts[ETexts.Enter] = "ВНУТРЬ 'C'";
            Texts[ETexts.Exit] = "НАРУЖУ 'C'";
            Texts[ETexts.Next] = "Дальше 'C'";
            Texts[ETexts.Talk] = "ГОВОРИТЬ 'C'";
            Texts[ETexts.TalkToMom] = "ОТВЕТИТЬ МАМЕ 'C''";
            Texts[ETexts.TalkToMomAgain] = "СНОВА ПОГОВОРИТЬ С МАМОЙ";
            Texts[ETexts.PullLever] = "ДЕРНУТЬ РЫЧАГ 'C'";
            Texts[ETexts.VillageCommoner] = "* Деревенский житель *";
            Texts[ETexts.UpgradeHealthTip] = "УКРЕПИТЬ ЗДОРОВЬЕ";
            Texts[ETexts.UpgradeHealthTitle] = "* Укрепить здоровье *";
            Texts[ETexts.KindleFire] = "РАЗЖЕЧЬ КОСТЁР";
            Texts[ETexts.RemoveObstacle] = "УБРАТЬ ПРЕГРАДУ";

            Texts[ETexts.English] = "English";
            Texts[ETexts.Russian] = "Русский";

            Texts[ETexts.TryAgain] = "Ещё разок";
            Texts[ETexts.ToMenu] = "В меню";

            Texts[ETexts.PieDialogue1] = "* Пирог для Бабули *";
            Texts[ETexts.DialoguePie1_1] = "- Привет, мама!";
            Texts[ETexts.DialoguePie1_2] = "- Добрий дэн, мой радост.";
            Texts[ETexts.DialoguePie1_3] = "- Ты.. звала меня? ))";
            Texts[ETexts.DialoguePie1_4] = "- О, да! У менья ест Просба к тебье. Приносить мнье грыбы та ягоды для вкусный Пирог?";
            Texts[ETexts.DialoguePie1_5] = "- Хорошо, мам, сделаю!";
            Texts[ETexts.DialoguePie1_6] = "- Спасибо, сольнышко.";

            Texts[ETexts.FamilyMonologue] = "* Моя прекрасная семья *";
            Texts[ETexts.Family_1] = "О моей маме..";
            Texts[ETexts.Family_2] = "Она - иностранка, пришла в эти края из далёкой Штирии и всё ещё не может привыкнуть. Даже говорит до сих пор с акцентом. ";
            Texts[ETexts.Family_3] = "Хоть она и ведёт себя странно иногда, и порой не может отличить своего мужа от медведя в лесу (люблю тебя, папуля!), она замечательная женщина! ";
            Texts[ETexts.Family_4] = "Сегодня мне нужно собрать кое-что для пирога, который мама будет печь для бабушки. Надеюсь встретить там и папочку тоже ))";

            Texts[ETexts.FatherDialogue] = "* Неожиданная встреча *";
            Texts[ETexts.Father_1] = "ГРРРРРррр !!";
            Texts[ETexts.Father_2] = "Уваа !!";
            Texts[ETexts.Father_3] = "Привееет, пап!";
            Texts[ETexts.Father_4] = "ГРРРррр.";
            Texts[ETexts.Father_5] = "Я в порядке, мама тоже. Она по тебе соскучилась, возвращайся скорее!";
            Texts[ETexts.Father_6] = "Грввах, уваах.";
            Texts[ETexts.Father_7] = "Ты починил мост? Как здорово! Тогда я побегу домой, до встречи!";

            Texts[ETexts.PieDialogue2] = "* Собранные ингредиенты *";
            Texts[ETexts.DialoguePie2_1] = "- Я принесла всё, что ты просила. Давай скорее готовить! (- о -)/";
            Texts[ETexts.DialoguePie2_2] = "- Ох, спасьибо, радост мойа! Отдохнуть ньемного, пока я готовит пирог.";
            Texts[ETexts.DialoguePie2_3] = "(Тебье точшно понравится, старущка Берта, просто подожди ещо ньемного)";
            Texts[ETexts.DialoguePie2_4] = "- Мам, ты что-то сказала?";
            Texts[ETexts.DialoguePie2_5] = "- Ой, это ничьего, просто бормочу. Пирог уше готов, можеш его сабрать.";
            Texts[ETexts.DialoguePie2_6] = "- Ураа!";

            Texts[ETexts.UpgradeHealth1_1] = "- Йа знайт, как ты любиш сладкое, но если найдешь Конфекты, кушай их найн! Они тошно ест Федьмина Наживкой для Детей.";
            Texts[ETexts.UpgradeHealth1_2] = "Лучше приноси их ко мнье, и я приготовлю што-нибуйт полезный тля укреплений тфоего Здоровя.";
            Texts[ETexts.UpgradeHealth1_3] = "- Штааа? (мои конфетки!) И зачем это мне укреплять здоровье?! Я и так здоровая!";
            Texts[ETexts.UpgradeHealth1_4] = "- Не капризничайт много, мойо Дитя.";
            Texts[ETexts.UpgradeHealth1_5] = "- Лаадно, я поняла... (T__T)";
            Texts[ETexts.UpgradeHealth2_1] = "Еслы найдеш хотья бы 10 Конфекты, йа смогу приготофить тебье!";
            Texts[ETexts.UpgradeHealth2_2] = "Ошень харашо, фот, возмьи!";
            Texts[ETexts.UpgradeHealth2_3] = "Этого мало, солнышько! Прынеси больще )";

            Texts[ETexts.BlacksmithTitle1] = "* Просьба Кузнеца *";
            Texts[ETexts.Blacksmith1_1] = "- Мэтр Кузнец! Пожалуйста, сделайте мне новый блестящий серый ключик!";
            Texts[ETexts.Blacksmith1_2] = "- Попридержи коней, девочка. Я, знаешь ли, очень занят. Мне еще нужно отполировать наковальню...";
            Texts[ETexts.Blacksmith1_3] = "- Ну пожалста, пожалстапожалстапазязяяя!";
            Texts[ETexts.Blacksmith1_4] = "- Хорошо, хорошо! Зачем так шуметь? Я сделаю тебе ключ, но ты должна мне принести, скажем, три куска железной руды, по рукам?";
            Texts[ETexts.Blacksmith1_5] = "- Я принесу вам руду!";

            Texts[ETexts.Blacksmith2_1] = "- Итак, ты нашла, что я просил?";
            Texts[ETexts.Blacksmith2_2] = "- Конечно, возьмите! (дурацкие булыжники!)";
            Texts[ETexts.Blacksmith2_3] = "- Хорошо, вот твой ключ, я как раз его закончил.";
            Texts[ETexts.Blacksmith2_4] = "- Эмм... нет, но я стараюсь!";

            Texts[ETexts.Commoner1_1] = "Лифт на рынке сейчас не работает, приходи позже.";
            Texts[ETexts.Commoner1_2] = "Не играй на развалинах, девочка, там опасно!";
            Texts[ETexts.GatekeeperTitle] = "* Привратник *";
            Texts[ETexts.Gatekeeper1_1] = "Если хочешь свободно пройти через эти ворота, иди попроси кузнеца сделать тебе новый серый ключ, а то я потерял запасной.";
            Texts[ETexts.CommonerSecret] = "Эй! В деревне есть тайное место, но добраться до него можно, только разрушив проклятую стену. Для этого тебе понадобится освященное оружие...";
            Texts[ETexts.WomanCandy] = "Вот, возьми конфетку, девочка!";
            Texts[ETexts.ShopkeeperTitle] = "* Продавец *";
            Texts[ETexts.Shopkeeper1] = "О, юная леди! Если вам случится найти эти блестящие кружочки, не выбрасывайте их. Взамен я могу дать вам что-то действительно полезное!";
        }
    }
}