using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundManager_", menuName = "ScriptableObjects/SoundSystem/Manager", order = 1)]
public class SoundManagerData : ScriptableObject
{
    [SerializeField] private List<Sound> _sounds = new List<Sound>();
    public List<Sound> Sounds => _sounds;
    
    public Sound GetSoundData(SoundKey soundKey)
    {
        foreach (var sound in _sounds)
        {
            if(sound.SoundKey.Equals(soundKey))
                return sound;
        }
        Debug.Log("SOUND NOT FOUND");
        return null;
    }
}
