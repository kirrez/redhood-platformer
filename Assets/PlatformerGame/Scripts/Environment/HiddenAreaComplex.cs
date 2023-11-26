using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Platformer
{
    public class HiddenAreaComplex : MonoBehaviour
    {
        [SerializeField]
        private TilemapRenderer Area;

        [SerializeField]
        private bool EnableDiscoverySound;

        private bool SoundPlayed;

        public List<Collider2D> Entrances;

        public List<Collider2D> Exits;

        private IAudioManager AudioManager;
        private IPlayer Player;
        private bool Inside;

        private void Awake()
        {
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
        {
            SoundPlayed = false;
        }

        private void Update()
        {

            foreach (Collider2D entrance in Entrances)
            {
                if (entrance.bounds.Contains(Player.Position) && !Inside)
                {
                    Area.enabled = false;
                    Inside = true;

                    if (EnableDiscoverySound == true && SoundPlayed == false)
                    {
                        AudioManager.PlaySound(ESounds.Collect7CampFire);
                        SoundPlayed = true;
                    }
                }
            }

            foreach (Collider2D exit in Exits)
            {
                if (exit.bounds.Contains(Player.Position) && Inside)
                {
                    Area.enabled = true;
                    Inside = false;
                }
            }
        }
    }
}