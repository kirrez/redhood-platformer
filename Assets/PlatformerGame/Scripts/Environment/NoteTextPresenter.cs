using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class NoteTextPresenter : MonoBehaviour
    {
        [SerializeField]
        private Text Text;

        [SerializeField]
        private ETutorialTexts Label;

        private ILocalization Localization;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
        }

        private void OnEnable()
        {
            Text.text = Localization.Tutorial(Label);
        }
    }
}