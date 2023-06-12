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
            Texts[ETexts.Russian] = "Russian";

            Texts[ETexts.PieDialogue1] = "* A Pie for a Grandma *";
            Texts[ETexts.DialoguePie1_1] = "- Hello, mom!";
            Texts[ETexts.DialoguePie1_2] = "- Guten Tag, mein Dear.";
            Texts[ETexts.DialoguePie1_3] = "- You.. called me? ))";
            Texts[ETexts.DialoguePie1_4] = "- Oh ja, Ich hasse ein Request fur dich. Could you bringst mich Schrooms und Berries for a Pie?";
            Texts[ETexts.DialoguePie1_5] = "- Sure, I'll do it! See ya )";
            Texts[ETexts.DialoguePie1_6] = "- Be careful, mein Hertz.";

            Texts[ETexts.FamilyMonologue] = "* My Wonderful Family *";
            Texts[ETexts.Family_1] = "It's about my mom.";
            Texts[ETexts.Family_2] = "She's come from far away, and still doesn't got used to local customs. Even couldn't get rid of her foreign accent.";
            Texts[ETexts.Family_3] = "Though she sometimes behave weird, and doesn't even distinguish her own husband from a mere bear in the forest (I love you, dad!), she is a nice person!";
            Texts[ETexts.Family_4] = "Today I'm going to collect some stuff for my mother to make a treat for Granny! Hope to meet daddy soon ))";
        }

        private void LoadRussian()
        {
            Texts[ETexts.Enter] = "������ 'C'";
            Texts[ETexts.Exit] = "������ 'C'";
            Texts[ETexts.Next] = "������  'C'";
            Texts[ETexts.TalkToMom] = "�������� ���� 'C''";
            Texts[ETexts.TalkToMomAgain] = "����� �������� ����";
            Texts[ETexts.PullLever] = "������� ����� 'C'";

            Texts[ETexts.English] = "����������";
            Texts[ETexts.Russian] = "�������";

            Texts[ETexts.PieDialogue1] = "* ����� ��� ������ *";
            Texts[ETexts.DialoguePie1_1] = "- ������, ����!";
            Texts[ETexts.DialoguePie1_2] = "- ������ ���, ��� ������.";
            Texts[ETexts.DialoguePie1_3] = "- ��.. ����� ����? ))";
            Texts[ETexts.DialoguePie1_4] = "- �, ��! � ����� ��� ������ � �����. ��������� ���� ����� �� ����� ��� ������� �����?";
            Texts[ETexts.DialoguePie1_5] = "- ������, ���, ������!";
            Texts[ETexts.DialoguePie1_6] = "- �������, ���������.";

            Texts[ETexts.FamilyMonologue] = "* ��� ���������� ����� *";
            Texts[ETexts.Family_1] = "� ���� ����..";
            Texts[ETexts.Family_2] = "��� - ����������, ������ � ��� ���� ������-�� �������� � �� ��� ��� �� ����� ����������. ���� ������� �� ��� ��� � ��������. ";
            Texts[ETexts.Family_3] = "���� ��� � ����� ���� ������� ������, � ���� �� ����� �������� ������ ���� �� ������� � ���� (����� ����, ������!), ��� ������������� �������! ";
            Texts[ETexts.Family_4] = "������� ��� ����� ������� ���-��� ��� ��������, ������� ���� ����� ���� ��� �������. ������� ��������� ������� � ���� ))";
        }
    }
}