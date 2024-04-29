﻿using System.Collections.Generic;
using System;

namespace Platformer
{
    public class Localization : ILocalization
    {
        private Dictionary<ETexts, string> Texts = new Dictionary<ETexts, string>();
        private Dictionary<EDestinationNames, string> Destinations = new Dictionary<EDestinationNames, string>();
        private Dictionary<ETutorialTexts, string> Tutorials = new Dictionary<ETutorialTexts, string>();

        public Localization()
        {
            var type = typeof(ETexts);
            foreach (ETexts item in Enum.GetValues(type))
            {
                Texts.Add(item, "");
            }

            type = typeof(EDestinationNames);
            foreach (EDestinationNames item in Enum.GetValues(type))
            {
                Destinations.Add(item, "");
            }

            type = typeof(ETutorialTexts);
            foreach (ETutorialTexts item in Enum.GetValues(type))
            {
                Tutorials.Add(item, "");
            }

            LoadLocalization(ELocalizations.English);
            //LoadLocalization(ELocalizations.Russian);
        }

        public string Text(ETexts key)
        {
            return Texts[key];
        }

        public string Destination(EDestinationNames key)
        {
            return Destinations[key];
        }

        public string Tutorial(ETutorialTexts key)
        {
            return Tutorials[key];
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

        //←♥↑→↓

        private void LoadEnglish()
        {
            Destinations[EDestinationNames.TheVillage] = "The Village";
            Destinations[EDestinationNames.WesternForest] = "Western Forest";
            Destinations[EDestinationNames.MountainPass] = "Mountain Pass";
            Destinations[EDestinationNames.CaveLabyrinth] = "Cave Labyrinth";
            Destinations[EDestinationNames.Lumbermill] = "Lumbermill";
            Destinations[EDestinationNames.WolfTrail] = "Wolf Trail";

            Tutorials[ETutorialTexts.UseArrowsToMove] = "Use ← → to move around (alt. A, D)";
            Tutorials[ETutorialTexts.PressZToJump] = "Press 'Z' to jump (alt. Space)";
            Tutorials[ETutorialTexts.HoldDownAndZ] = "Hold ↓ and press 'Z' to jump down from a one-way platform";
            Tutorials[ETutorialTexts.Crouch] = "Hold ↓ and use ← → to crouch";
            Tutorials[ETutorialTexts.Slide] = "Press 'Z' while holding ↓ to slide";
            Tutorials[ETutorialTexts.ThrowKnife] = "Press 'X' to throw a knife (alt. LMB)";
            Tutorials[ETutorialTexts.ThrowAxe] = "Hold ↑ and press 'X' to throw an axe";
            Tutorials[ETutorialTexts.Interact] = "Press 'C' when you encounter characters or interactable objects (alt. RMB)";
            Tutorials[ETutorialTexts.SecondSubweapon] = "Press 'C' to throw second sub-weapon";
            Tutorials[ETutorialTexts.Final] = "This completes the basic tutorial, intended to cover some unclear parts in control mechanics!";

            Texts[ETexts.Enter_Label] = "ENTER 'C'";
            Texts[ETexts.Exit_Label] = "EXIT 'C'";
            Texts[ETexts.Next_Label] = "Next 'C'";
            Texts[ETexts.Talk_Label] = "TALK 'C'";
            Texts[ETexts.TalkToMom_Label] = "TALK TO MOM 'C'";
            Texts[ETexts.TalkToMomAgain_Label] = "TALK TO MOM AGAIN";
            Texts[ETexts.PullLever_Label] = "PULL LEVER 'C'";
            Texts[ETexts.Commoner_Title] = "* Village Commoner *";
            Texts[ETexts.UpgradeHealth_Label] = "ENHANCE HEALTH";
            Texts[ETexts.UpgradeHealth_Title] = "* Enhance Health *";
            Texts[ETexts.KindleAFire_Label] = "KINDLE A CAMPFIRE";
            Texts[ETexts.RemoveAnObstacle_Label] = "REMOVE AN OBSTACLE";
            Texts[ETexts.SwitchOn_Label] = "Switch On";
            Texts[ETexts.Repair_Label] = "REPAIR";

            Texts[ETexts.StartGameNoTutorial] = "Start Game\nno tutorial";
            Texts[ETexts.PlayTutorialFirst] = "Play tutorial first";


            Texts[ETexts.English] = "English";
            Texts[ETexts.Russian] = "Русский";

            Texts[ETexts.TryAgain] = "Try Again";
            Texts[ETexts.ToMenu] = "Menu";

            Texts[ETexts.PieDialogue1_Title] = "* A Pie for a Grandma *";
            Texts[ETexts.DialoguePie1_1] = "- Hello, mom!";
            Texts[ETexts.DialoguePie1_2] = "- Guten Tag, meine Dear.";
            Texts[ETexts.DialoguePie1_3] = "- You.. called me? ))";
            Texts[ETexts.DialoguePie1_4] = "- Oh ja, Ich hasse ein Request fur dich. Could you bringst mich Schrooms und Berries for a Pie?";
            Texts[ETexts.DialoguePie1_5] = "- Sure, I'll do it! See ya )";
            Texts[ETexts.DialoguePie1_6] = "- Be careful, meine Hertz.";

            Texts[ETexts.DialoguePie1_7] = "- Du brought mich ";
            Texts[ETexts.DialoguePie1_8] = " Schrooms und ";
            Texts[ETexts.DialoguePie1_9] = " Berries! Das ist nicht enough..";

            Texts[ETexts.FamilyMonologue_Title] = "* My Wonderful Family *";
            Texts[ETexts.Family_1] = "It's about my mom.";
            Texts[ETexts.Family_2] = "She came from far away country called Stiria, and still didn't get used to local customs. Even couldn't get rid of her foreign accent.";
            Texts[ETexts.Family_3] = "Though sometimes she behave weird, and doesn't even distinguish her own husband from a mere bear in the forest (I love you, dad!), she is a nice person!";
            Texts[ETexts.Family_4] = "Today I'm going to collect some stuff for my mother to make a treat for Granny! Hope to meet there daddy too ))";

            Texts[ETexts.FatherDialogue_Title] = "* Unexpected Encounter *";
            Texts[ETexts.Father_1] = "GRRRRRRRrrr !!";
            Texts[ETexts.Father_2] = "Uwaaa !!";
            Texts[ETexts.Father_3] = "Helllooo, daddy !";
            Texts[ETexts.Father_4] = "GRRRrrr.";
            Texts[ETexts.Father_5] = "I'm fine. Mom is good too. She's been missing you lately. Come back home soon.";
            Texts[ETexts.Father_6] = "Grwaah, huwaaa.";
            Texts[ETexts.Father_7] = "Oh, your old axe! Thank you, i'll take it!";
            Texts[ETexts.Father_8] = "Did you fix the bridge? How nice of you! Then I'll better go home, bye!";


            Texts[ETexts.PieDialogue2_Title] = "* Ingredients collected *";
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
            Texts[ETexts.UpgradeHealth2_1] = "If you hast at least 5 Candies, I'll make it fur dich!";
            Texts[ETexts.UpgradeHealth2_2] = "Sehr gut! Here you are!";
            Texts[ETexts.UpgradeHealth2_3] = "Ohh, du hast nicht genug! Bringst mich more )";

            Texts[ETexts.Blacksmith_Title1] = "* Blacksmith's Errand *";
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
            Texts[ETexts.Gatekeeper_Title] = "* Gatekeeper *";
            Texts[ETexts.Gatekeeper1_1] = "If you want to pass freely through this gate, go ask the smith to make you a new grey key, since I've lost a spare one.";
            Texts[ETexts.Gatekeeper1_2] = "Be careful, child!";
            Texts[ETexts.CommonerSecret] = "Psst! There is a secret place in the village. You can only reach it by crushing the cursed wall. You obviousely need a holy weapon for it...";
            Texts[ETexts.WomanCandy] = "Here you are, darling, take this!";
            Texts[ETexts.Shopkeeper_Title] = "* Shopkeeper *";
            Texts[ETexts.Shopkeeper1] = "Oh, young lady! If you happen to find this shiny circles, don't throw them out. I can give you something really useful in exchange!";

            Texts[ETexts.Hermit_Title] = "* Hermit *";
            Texts[ETexts.Hermit1_1] = "- Who decided to visit me today? My eyes can't see... but you may come closer, child.";
            Texts[ETexts.Hermit1_2] = "- Hello! How do you live in such a secluded place? By the way, do you happen to know...";
            Texts[ETexts.Hermit1_3] = "- Oh, I know a lot! There is a holy place here, but there is only one correct path: it is Fire, Mine, Water, Statues, Bones, Fire and Water. Perhaps you'll find there what you're looking for.";
            Texts[ETexts.Hermit1_4] = "- Well, actually, thank you! That is what I really need ))";
            Texts[ETexts.Hermit2] = "- Don't you forget, the right path is Fire, Mine, Water, Statues, Bones, Fire and Water!";

            Texts[ETexts.BrokenLeverTip_Title] = "* Curious Find *";
            Texts[ETexts.BrokenLeverTip1] = "What is this thing? Seems like a part of some mechanism. Might be useful!";
            Texts[ETexts.BrokenLeverInCave_Title] = "* Door With A Switch *";
            Texts[ETexts.BrokenLeverInCave1] = "This is a door with broken switch! Hmm... I must find a way to fix it if I want to go in (*__*)";
            Texts[ETexts.BrokenLeverInCave_HandleNotFound] = "I still don't have the right thing to fix it..";
            Texts[ETexts.BrokenLeverInCave_HandleFound] = "Yay! It fits! That stuff's operable now!";

            Texts[ETexts.DisturbSpirit_Title] = "* Uneasy Suspense *";
            Texts[ETexts.DisturbSpirit1] = "...";
            Texts[ETexts.DisturbSpirit2] = "Who's there?";
            Texts[ETexts.DisturbSpirit3] = "!!!";
            Texts[ETexts.DisturbSpirit4] = "Eeeek! gotta get out of here really quick!";

            Texts[ETexts.HollyMolly_Label] = "Holly & Molly";
            Texts[ETexts.HollyMolly_Title] = "* Holly & Molly *";
            Texts[ETexts.HollyMolly1_1] = "- Holey-moley, what a charming duet of lovely musicians! I wanna musick, play, play, play! (X o X)/";
            Texts[ETexts.HollyMolly1_2] = "- Oh, girl, have we ever met before? But of course we did! ";
            Texts[ETexts.HollyMolly1_3] = "(Psst! Molly, she's our fan! ♥)";
            Texts[ETexts.HollyMolly1_4] = "- Ahem, (*in a declaring posture*) We are generous for gifts of joy and willing to amuse ye, folks, but also we would really appreciate you showing us some of your generosity back.";
            Texts[ETexts.HollyMolly1_5] = "...";
            Texts[ETexts.HollyMolly1_6] = "- So, do you have any charmed candies perchance? Give us 3 of them and you won't regret it, swear! (for us to brew love potions out of them, ha-ha!)";
            Texts[ETexts.HollyMolly1_7] = "- So, do you have any charmed candies perchance? Give us 3 of them and you won't regret it, swear!";
            Texts[ETexts.HollyMolly1_8] = "Just talk to us again if you want the show ))"; //finishes the first chain of conversation
            Texts[ETexts.HollyMolly1_9] = "- I'll be back really soon!";
            Texts[ETexts.HollyMolly1_10] = "- Sure, take it!";
            Texts[ETexts.HollyMolly1_11] = "- Perform your show again!.. since I'm your biggest fan!";

            Texts[ETexts.ForsakenCamp_Title] = "* Forsaken Camp *";
            Texts[ETexts.ForsakenCamp_1] = "Oh my! I must be lost here! At least I can always come back and have some rest..";

            Texts[ETexts.Assistant_Title] = "* Mysterious Assistant *";
            Texts[ETexts.Assistant_frog1] = "I'm here to assist the brave souls, who find this foul beast too difficult to be dealt with.";
            Texts[ETexts.Assistant_frog2_1] = "I can sell you strong amphibian poison, which halvens it's strength. For a reasonable price, ofcourse! Talk to me again and bring along ";
            Texts[ETexts.Assistant_frog2_2] = " shiny circles with you so we can make a deal!";
            Texts[ETexts.Assistant_frog3] = "That's it, pour the poison into the pond! I wish you a good luck with your daring effort, ha-ha!";
            Texts[ETexts.Assistant_frog4] = "Let's see.. you don't have enough shiny circles! Sorry, no charity, lass.";
            Texts[ETexts.Assistant_frog5] = "I belive you can do it!";
            Texts[ETexts.Assistant_frog6] = "Oh, you did it! Way to go, my dear!";
            Texts[ETexts.Assistant_frog7] = "I see, you've managed this problem somehow. My congratulations, he-he!";

            Texts[ETexts.Assistant_tower1] = "I'm here to assist curious travellers, who find this tower too difficult to traverse.";
            Texts[ETexts.Assistant_tower2_1] = "I can switch on this old rusty platform elevator. For a reasonable price, ofcourse! Talk to me again and bring along ";
            Texts[ETexts.Assistant_tower2_2] = " shiny circles with you so we can make a deal!";
            Texts[ETexts.Assistant_tower3] = "That's it! OPEN SESAME! Et voila.. ";
            Texts[ETexts.Assistant_tower4] = "Let's see.. you don't have enough shiny circles! Sorry, no charity, lass.";
            Texts[ETexts.Assistant_tower5] = "Feel free to use it any time you need.";
            Texts[ETexts.Assistant_tower6] = "Hmm, another boring day in a middle of nowhere..";

            Texts[ETexts.Guide_v01_Title] = "* Guide v 0.1 *";
            Texts[ETexts.Guide_v01_1] = "Hello, my dear! I'm here to guide you right to the final part of your adventure in this version of game. The best thing you can do for now is to visit your mother's house and come back to me.";
            Texts[ETexts.Guide_v01_2] = "I also have gifts for you! Try to use your new axe to deal with these tangled roots, blocking your way. Have fun exploring the Cave Labyrinth location.";
            Texts[ETexts.Guide_v01_3] = "I hope you liked this oldschool platformer and look forward for to the full story, which is under active development ))) Follow the upgrades and thank you for playing!";

            Texts[ETexts.Commoner3] = "My Klaus is hunting on the Wolf Trail, deep in the forest. He's brave and experienced, but I'm still worried for him.";
        }

        private void LoadRussian()
        {
            Destinations[EDestinationNames.TheVillage] = "Деревня";
            Destinations[EDestinationNames.WesternForest] = "Западный лес";
            Destinations[EDestinationNames.MountainPass] = "Горный перевал";
            Destinations[EDestinationNames.CaveLabyrinth] = "Пещерный лабиринт";
            Destinations[EDestinationNames.Lumbermill] = "Лесопилка";
            Destinations[EDestinationNames.WolfTrail] = "Волчья тропа";

            Texts[ETexts.Enter_Label] = "ВНУТРЬ 'C'";
            Texts[ETexts.Exit_Label] = "НАРУЖУ 'C'";
            Texts[ETexts.Next_Label] = "Дальше 'C'";
            Texts[ETexts.Talk_Label] = "ГОВОРИТЬ 'C'";
            Texts[ETexts.TalkToMom_Label] = "ОТВЕТИТЬ МАМЕ 'C''";
            Texts[ETexts.TalkToMomAgain_Label] = "СНОВА ПОГОВОРИТЬ С МАМОЙ";
            Texts[ETexts.PullLever_Label] = "ДЕРНУТЬ РЫЧАГ 'C'";
            Texts[ETexts.Commoner_Title] = "* Деревенский житель *";
            Texts[ETexts.UpgradeHealth_Label] = "УКРЕПИТЬ ЗДОРОВЬЕ";
            Texts[ETexts.UpgradeHealth_Title] = "* Укрепить здоровье *";
            Texts[ETexts.KindleAFire_Label] = "РАЗЖЕЧЬ КОСТЁР";
            Texts[ETexts.RemoveAnObstacle_Label] = "УБРАТЬ ПРЕГРАДУ";
            Texts[ETexts.SwitchOn_Label] = "Включить";
            Texts[ETexts.Repair_Label] = "ПОЧИНИТЬ";

            Texts[ETexts.StartGameNoTutorial] = "Начать игру\nбез обучения";
            Texts[ETexts.PlayTutorialFirst] = "Сперва пройти обучение";

            Texts[ETexts.English] = "English";
            Texts[ETexts.Russian] = "Русский";

            Texts[ETexts.TryAgain] = "Ещё разок";
            Texts[ETexts.ToMenu] = "В меню";

            Texts[ETexts.PieDialogue1_Title] = "* Пирог для Бабули *";
            Texts[ETexts.DialoguePie1_1] = "- Привет, мама!";
            Texts[ETexts.DialoguePie1_2] = "- Добрий дэн, мой радост.";
            Texts[ETexts.DialoguePie1_3] = "- Ты.. звала меня? ))";
            Texts[ETexts.DialoguePie1_4] = "- О, да! У менья ест Просба к тебье. Приносить мнье грыбы та ягоды для вкусный Пирог?";
            Texts[ETexts.DialoguePie1_5] = "- Хорошо, мам, сделаю!";
            Texts[ETexts.DialoguePie1_6] = "- Спасибо, сольнышко.";

            Texts[ETexts.DialoguePie1_7] = "- Тфоя приносить ";
            Texts[ETexts.DialoguePie1_8] = " Грибов и ";
            Texts[ETexts.DialoguePie1_9] = " Йагот! Этафа нье достатошно..";

            Texts[ETexts.FamilyMonologue_Title] = "* Моя прекрасная семья *";
            Texts[ETexts.Family_1] = "О моей маме..";
            Texts[ETexts.Family_2] = "Она - иностранка, пришла в эти края из далёкой Штирии и всё ещё не может привыкнуть. Даже говорит до сих пор с акцентом. ";
            Texts[ETexts.Family_3] = "Хоть она и ведёт себя странно иногда, и порой не может отличить своего мужа от медведя в лесу (люблю тебя, папуля!), она замечательная женщина! ";
            Texts[ETexts.Family_4] = "Сегодня мне нужно собрать кое-что для пирога, который мама будет печь для бабушки. Надеюсь встретить там и папочку тоже ))";

            Texts[ETexts.FatherDialogue_Title] = "* Неожиданная встреча *";
            Texts[ETexts.Father_1] = "ГРРРРРррр !!";
            Texts[ETexts.Father_2] = "Уваа !!";
            Texts[ETexts.Father_3] = "Привееет, пап!";
            Texts[ETexts.Father_4] = "ГРРРррр.";
            Texts[ETexts.Father_5] = "Я в порядке, мама тоже. Она по тебе соскучилась, возвращайся скорее!";
            Texts[ETexts.Father_6] = "Грввах, уваах.";
            Texts[ETexts.Father_7] = "О, твой старый топор! Спасибо, я возьму его!";
            Texts[ETexts.Father_8] = "Ты починил мост? Как здорово! Тогда я побегу домой, до встречи!";

            Texts[ETexts.PieDialogue2_Title] = "* Собранные ингредиенты *";
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
            Texts[ETexts.UpgradeHealth2_1] = "Еслы найдеш хотья бы 5 Конфекты, йа смогу приготофить тебье!";
            Texts[ETexts.UpgradeHealth2_2] = "Ошень харашо, фот, возмьи!";
            Texts[ETexts.UpgradeHealth2_3] = "Этого мало, солнышько! Прынеси больще )";

            Texts[ETexts.Blacksmith_Title1] = "* Просьба Кузнеца *";
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
            Texts[ETexts.Gatekeeper_Title] = "* Привратник *";
            Texts[ETexts.Gatekeeper1_1] = "Если хочешь свободно пройти через эти ворота, иди попроси кузнеца сделать тебе новый серый ключ, а то я потерял запасной.";
            Texts[ETexts.Gatekeeper1_2] = "Будь осторожна, девочка!";
            Texts[ETexts.CommonerSecret] = "Эй! В деревне есть тайное место, но добраться до него можно, только разрушив проклятую стену. Для этого тебе понадобится освященное оружие...";
            Texts[ETexts.WomanCandy] = "Вот, возьми конфетку, девочка!";
            Texts[ETexts.Shopkeeper_Title] = "* Продавец *";
            Texts[ETexts.Shopkeeper1] = "О, юная леди! Если вам случится найти эти блестящие кружочки, не выбрасывайте их. Взамен я могу дать вам что-то действительно полезное!";

            Texts[ETexts.Hermit_Title] = "* Отшельник *";
            Texts[ETexts.Hermit1_1] = "- Кто решил прийти ко мне в гости сегодня? Мои глаза не видят... но ты можешь подойти ближе, дитя.";
            Texts[ETexts.Hermit1_2] = "- Здравствуйте! Как вам живется в таком уединенном месте? Кстати, случайно не знаете...";
            Texts[ETexts.Hermit1_3] = "- О, я знаю многое! Здесь есть святое место, но правильный путь только один : это Огонь, Шахта, Вода, Статуи, Кости, Огонь и Вода. Может быть ты найдешь там то, что ищешь.";
            Texts[ETexts.Hermit1_4] = "- Ну, на самом деле, спасибо вам! Это то, что мне действительно нужно ))";
            Texts[ETexts.Hermit2] = "- Не забывай, дитя, верная дорога - это Огонь, Шахта, Вода, Статуи, Кости, Огонь и Вода!";

            Texts[ETexts.BrokenLeverTip_Title] = "* Любопытная находка *";
            Texts[ETexts.BrokenLeverTip1] = "Что это за штука такая? Похожа на детальку от какого-то механизма. Пригодится!";
            Texts[ETexts.BrokenLeverInCave_Title] = "* Дверь с переключателем *";
            Texts[ETexts.BrokenLeverInCave1] = "Это дверь со сломанным выключателем! Хмм.. мне нужно как-то починить выключатель, чтобы попасть внутрь (*__*)";
            Texts[ETexts.BrokenLeverInCave_HandleNotFound] = "У меня нет нужной вещи для ремонта..";
            Texts[ETexts.BrokenLeverInCave_HandleFound] = "Ура, подходит! Эта штука теперь работает!";

            Texts[ETexts.DisturbSpirit_Title] = "* Дурное предчуствие *";
            Texts[ETexts.DisturbSpirit1] = "...";
            Texts[ETexts.DisturbSpirit2] = "Кто здесь?";
            Texts[ETexts.DisturbSpirit3] = "!!!";
            Texts[ETexts.DisturbSpirit4] = "Ааай! нужно поскорее убираться отсюда!";

            Texts[ETexts.HollyMolly_Title] = "* Ёлка и Палка *";
            Texts[ETexts.HollyMolly1_1] = "- Ёлки-палки, какие милые актрисы! Хочу услышать музыку, музыку, музыкууу! (Х о Х)/";
            Texts[ETexts.HollyMolly1_2] = "- О, девочка! Разве мы знакомы? Хотя.. конечно же знакомы!";
            Texts[ETexts.HollyMolly1_3] = "(Эй, Палка! она - наша поклонница ♥)";
            Texts[ETexts.HollyMolly1_4] = "- Кхм, (*с выражением на публику*) We are generous for gifts of joy and willing to amuse ye, folks, but also we would really appreciate you showing us some of your generosity back.";
            Texts[ETexts.HollyMolly1_5] = "...";
            Texts[ETexts.HollyMolly1_6] = "- So, do you have any charmed candies perchance? Give us 3 of them and you won't regret it, swear! "; //also begins the chain where they start to play
            Texts[ETexts.HollyMolly1_7] = "Just talk to us again if you want the show ))"; //finishes the first chain of conversation
            Texts[ETexts.HollyMolly1_8] = "- I'll be back really soon!";
            Texts[ETexts.HollyMolly1_9] = "- Sure, take it!";
            Texts[ETexts.HollyMolly1_10] = "- Perform your show again!.. since I'm your biggest fan!"; //repeats the show, for free ))

            Texts[ETexts.ForsakenCamp_Title] = "* Заброшенный лагерь *";
            Texts[ETexts.ForsakenCamp_1] = "Oh my! I must be lost here! At least I can always come back and have some rest..";

            Texts[ETexts.Assistant_Title] = "* Mysterious Assistant *";
            Texts[ETexts.Assistant_frog1] = "I'm here to assist the brave souls, who find this foul beast too difficult to be dealt with.";
            Texts[ETexts.Assistant_frog2_1] = "I can sell you strong amphibian poison, which halvens it's strength. For a reasonable price, ofcourse! Talk to me again and bring along ";
            Texts[ETexts.Assistant_frog2_2] = " shiny circles with you so we can make a deal!";
            Texts[ETexts.Assistant_frog3] = "That's it, pour the poison into the pond! I wish you a good luck with your daring effort, ha-ha!";
            Texts[ETexts.Assistant_frog4] = "Let's see.. you don't have enough shiny circles! Sorry, no charity, lass.";
            Texts[ETexts.Assistant_frog5] = "I belive you can do it!";
            Texts[ETexts.Assistant_frog6] = "Oh, you did it! Way to go, my dear!";
            Texts[ETexts.Assistant_frog7] = "I see, you've managed this problem somehow. My congratulations, he-he!";

            Texts[ETexts.Assistant_tower1] = "I'm here to assist curious travellers, who find this tower too difficult to traverse.";
            Texts[ETexts.Assistant_tower2_1] = "I can switch on this old rusty platform elevator. For a reasonable price, ofcourse! Talk to me again and bring along ";
            Texts[ETexts.Assistant_tower2_2] = " shiny circles with you so we can make a deal!";
            Texts[ETexts.Assistant_tower3] = "That's it! OPEN SESAME! Et voila.. ";
            Texts[ETexts.Assistant_tower4] = "Let's see.. you don't have enough shiny circles! Sorry, no charity, lass.";
            Texts[ETexts.Assistant_tower5] = "Feel free to use it any time you need.";
            Texts[ETexts.Assistant_tower6] = "Hmm, another boring day in a middle of nowhere..";

            Texts[ETexts.Commoner3] = "Мой Клаус охотится на волчьей тропе далеко в лесу. Он смелый и опытный, но я все равно волнуюсь за него.";
        }
    }
}