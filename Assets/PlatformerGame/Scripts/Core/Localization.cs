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
            Texts[ETexts.Next] = "Next  'C'";
            Texts[ETexts.TalkToMom] = "Talk to Mom 'C'";
            Texts[ETexts.TalkToMomAgain] = "Talk to Mom again";
            Texts[ETexts.PullLever] = "PULL LEVER 'C'";

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
        }

        private void LoadRussian()
        {
            Texts[ETexts.Enter] = "������ 'C'";
            Texts[ETexts.Exit] = "������ 'C'";
            Texts[ETexts.Next] = "������  'C'";
            Texts[ETexts.TalkToMom] = "�������� ���� 'C''";
            Texts[ETexts.TalkToMomAgain] = "����� �������� ����";
            Texts[ETexts.PullLever] = "������� ����� 'C'";

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
            Texts[ETexts.Family_2] = "��� - ����������, ������ � ��� ���� �� ������� ������ � �� ��� ��� �� ����� ����������. ���� ������� �� ��� ��� � ��������. ";
            Texts[ETexts.Family_3] = "���� ��� � ����� ���� ������� ������, � ���� �� ����� �������� ������ ���� �� ������� � ���� (����� ����, ������!), ��� ������������� �������! ";
            Texts[ETexts.Family_4] = "������� ��� ����� ������� ���-��� ��� ������, ������� ���� ����� ���� ��� �������. ������� ��������� ��� � ������� ���� ))";

            Texts[ETexts.FatherDialogue] = "* ����������� ������� *";
            Texts[ETexts.Father_1] = "��������� !!";
            Texts[ETexts.Father_2] = "���� !!";
            Texts[ETexts.Father_3] = "��������, ���!";
            Texts[ETexts.Father_4] = "�������.";
            Texts[ETexts.Father_5] = "� � �������, ���� ����. ��� �� ���� �����������, ����������� ������ �����!";
            Texts[ETexts.Father_6] = "������, �����.";
            Texts[ETexts.Father_7] = "�� ������� ����? ��� �������! ����� � ������ �����, �� �������!";

            Texts[ETexts.PieDialogue2] = "* ��������� ����������� *";
            Texts[ETexts.DialoguePie2_1] = "- I've brought you everything you asked for, let's cook (- o -)/";
            Texts[ETexts.DialoguePie2_2] = "- Ah, danke, meine Dear! Hast du some Rest wilst I bake a Pie..";
            Texts[ETexts.DialoguePie2_3] = "(You'll sure be liking it, old Berta, just wait a little bit more)";
            Texts[ETexts.DialoguePie2_4] = "- Did you say something, mother?";
            Texts[ETexts.DialoguePie2_5] = "- What? Oh, das ist nicht, der Pie ist ready. Go, take it.";
            Texts[ETexts.DialoguePie2_6] = "- Yay!";
        }
    }
}