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
            Texts[ETexts.Russian] = "�������";

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
            Texts[ETexts.UpgradeHealth1_2] = "Besser�du bringst them to me und ich cooke something gut fur strengthening dein� Health.";
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
            Texts[ETexts.Enter] = "������ 'C'";
            Texts[ETexts.Exit] = "������ 'C'";
            Texts[ETexts.Next] = "������ 'C'";
            Texts[ETexts.Talk] = "�������� 'C'";
            Texts[ETexts.TalkToMom] = "�������� ���� 'C''";
            Texts[ETexts.TalkToMomAgain] = "����� ���������� � �����";
            Texts[ETexts.PullLever] = "������� ����� 'C'";
            Texts[ETexts.VillageCommoner] = "* ����������� ������ *";
            Texts[ETexts.UpgradeHealthTip] = "�������� ��������";
            Texts[ETexts.UpgradeHealthTitle] = "* �������� �������� *";
            Texts[ETexts.KindleFire] = "������� ���Ҩ�";
            Texts[ETexts.RemoveObstacle] = "������ ��������";

            Texts[ETexts.English] = "English";
            Texts[ETexts.Russian] = "�������";

            Texts[ETexts.TryAgain] = "��� �����";
            Texts[ETexts.ToMenu] = "� ����";

            Texts[ETexts.PieDialogue1] = "* ����� ��� ������ *";
            Texts[ETexts.DialoguePie1_1] = "- ������, ����!";
            Texts[ETexts.DialoguePie1_2] = "- ������ ���, ��� ������.";
            Texts[ETexts.DialoguePie1_3] = "- ��.. ����� ����? ))";
            Texts[ETexts.DialoguePie1_4] = "- �, ��! � ����� ��� ������ � �����. ��������� ���� ����� �� ����� ��� ������� �����?";
            Texts[ETexts.DialoguePie1_5] = "- ������, ���, ������!";
            Texts[ETexts.DialoguePie1_6] = "- �������, ���������.";

            Texts[ETexts.FamilyMonologue] = "* ��� ���������� ����� *";
            Texts[ETexts.Family_1] = "� ���� ����..";
            Texts[ETexts.Family_2] = "��� - ����������, ������ � ��� ���� �� ������ ������ � �� ��� �� ����� ����������. ���� ������� �� ��� ��� � ��������. ";
            Texts[ETexts.Family_3] = "���� ��� � ���� ���� ������� ������, � ����� �� ����� �������� ������ ���� �� ������� � ���� (����� ����, ������!), ��� ������������� �������! ";
            Texts[ETexts.Family_4] = "������� ��� ����� ������� ���-��� ��� ������, ������� ���� ����� ���� ��� �������. ������� ��������� ��� � ������� ���� ))";

            Texts[ETexts.FatherDialogue] = "* ����������� ������� *";
            Texts[ETexts.Father_1] = "��������� !!";
            Texts[ETexts.Father_2] = "���� !!";
            Texts[ETexts.Father_3] = "��������, ���!";
            Texts[ETexts.Father_4] = "�������.";
            Texts[ETexts.Father_5] = "� � �������, ���� ����. ��� �� ���� �����������, ����������� ������!";
            Texts[ETexts.Father_6] = "������, �����.";
            Texts[ETexts.Father_7] = "�� ������� ����? ��� �������! ����� � ������ �����, �� �������!";

            Texts[ETexts.PieDialogue2] = "* ��������� ����������� *";
            Texts[ETexts.DialoguePie2_1] = "- � �������� ��, ��� �� �������. ����� ������ ��������! (- � -)/";
            Texts[ETexts.DialoguePie2_2] = "- ��, ��������, ������ ����! ��������� ��������, ���� � ������� �����.";
            Texts[ETexts.DialoguePie2_3] = "(����� ������ ����������, �������� �����, ������ ������� ��� ��������)";
            Texts[ETexts.DialoguePie2_4] = "- ���, �� ���-�� �������?";
            Texts[ETexts.DialoguePie2_5] = "- ��, ��� �������, ������ �������. ����� ��� �����, ����� ��� �������.";
            Texts[ETexts.DialoguePie2_6] = "- ����!";

            Texts[ETexts.UpgradeHealth1_1] = "- �� �����, ��� �� ����� �������, �� ���� ������� ��������, ����� �� ����! ��� ����� ��� �������� �������� ��� �����.";
            Texts[ETexts.UpgradeHealth1_2] = "����� ������� �� �� ����, � � ���������� ���-������ �������� ��� ���������� ������ �������.";
            Texts[ETexts.UpgradeHealth1_3] = "- �����? (��� ��������!) � ����� ��� ��� ��������� ��������?! � � ��� ��������!";
            Texts[ETexts.UpgradeHealth1_4] = "- �� ������������ �����, ���� ����.";
            Texts[ETexts.UpgradeHealth1_5] = "- ������, � ������... (T__T)";
            Texts[ETexts.UpgradeHealth2_1] = "���� ������ ����� �� 10 ��������, �� ����� ����������� �����!";
            Texts[ETexts.UpgradeHealth2_2] = "����� ������, ���, ������!";
            Texts[ETexts.UpgradeHealth2_3] = "����� ����, ���������! ������� ������ )";

            Texts[ETexts.BlacksmithTitle1] = "* ������� ������� *";
            Texts[ETexts.Blacksmith1_1] = "- ���� ������! ����������, �������� ��� ����� ��������� ����� ������!";
            Texts[ETexts.Blacksmith1_2] = "- ���������� �����, �������. �, ������ ��, ����� �����. ��� ��� ����� ������������ ����������...";
            Texts[ETexts.Blacksmith1_3] = "- �� ��������, ������������������������!";
            Texts[ETexts.Blacksmith1_4] = "- ������, ������! ����� ��� ������? � ������ ���� ����, �� �� ������ ��� ��������, ������, ��� ����� �������� ����, �� �����?";
            Texts[ETexts.Blacksmith1_5] = "- � ������� ��� ����!";

            Texts[ETexts.Blacksmith2_1] = "- ����, �� �����, ��� � ������?";
            Texts[ETexts.Blacksmith2_2] = "- �������, ��������! (�������� ���������!)";
            Texts[ETexts.Blacksmith2_3] = "- ������, ��� ���� ����, � ��� ��� ��� ��������.";
            Texts[ETexts.Blacksmith2_4] = "- ���... ���, �� � ��������!";

            Texts[ETexts.Commoner1_1] = "���� �� ����� ������ �� ��������, ������� �����.";
            Texts[ETexts.Commoner1_2] = "�� ����� �� ����������, �������, ��� ������!";
            Texts[ETexts.GatekeeperTitle] = "* ���������� *";
            Texts[ETexts.Gatekeeper1_1] = "���� ������ �������� ������ ����� ��� ������, ��� ������� ������� ������� ���� ����� ����� ����, � �� � ������� ��������.";
            Texts[ETexts.CommonerSecret] = "��! � ������� ���� ������ �����, �� ��������� �� ���� �����, ������ �������� ��������� �����. ��� ����� ���� ����������� ���������� ������...";
            Texts[ETexts.WomanCandy] = "���, ������ ��������, �������!";
            Texts[ETexts.ShopkeeperTitle] = "* �������� *";
            Texts[ETexts.Shopkeeper1] = "�, ���� ����! ���� ��� �������� ����� ��� ��������� ��������, �� ������������ ��. ������ � ���� ���� ��� ���-�� ������������� ��������!";
        }
    }
}