using FMODUnity;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class SoundEffectManager : Singleton<SoundEffectManager>
{
    [SerializeField] public SerializableDictionaryBase<string, EventReference> sounds = new();

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(string key)
    {
        RuntimeManager.CreateInstance(sounds[key]);
    }
}
