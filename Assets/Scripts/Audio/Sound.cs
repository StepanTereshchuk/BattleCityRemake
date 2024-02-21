using UnityEngine;
using UnityEngine.Audio;

public enum LayerType
{
    Additive,
    Single
}
public enum SoundKey
{
    MotorIdle, //
    MotorMove, //
    BulletLaunch, //
    HighScore, //UI
    Defeat,//
    ScoreIncrease,//UI
    BonusAdded, //
    AdditionalLife,//
    BonusTaken, //
    BonusTankShooted, //
    BaseDestroyed,//
    EnemyKilled, //
    BlockDestroyed, // louder needs
    BlockNotDestroyed, //
    ShootAtArmoredTank, //
    MainTheme,  //UI
    ButtonPressed,
    ButtonSelected
}

[CreateAssetMenu(fileName = "Sound_", menuName = "ScriptableObjects/SoundSystem/Sound", order = 1)]
public class Sound : ScriptableObject
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private LayerType _layerType;
    [SerializeField] private SoundKey _soundKey;
    [SerializeField] private bool _looping;
    public AudioClip AudioClip => _audioClip;
    public AudioMixerGroup AudioMixerGroup => _audioMixerGroup;
    public LayerType LayerType => _layerType;

    public SoundKey SoundKey => _soundKey;
    public bool Looping => _looping;
    public float Lenght => _audioClip.length;

}
