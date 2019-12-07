//Código de uso público: https://bitbucket.org/richardfine/scriptableobjectdemo/commits/03a730f1b0581c0d424268bc03e33dac21f34248?w=0#chg-Assets/ScriptableObject/Audio/MinMaxRangeAttribute.cs
//Creado por: Richard Fine https://bitbucket.org/richardfine/
//Definición de un SimpleAudioEvent
using UnityEngine;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	public AudioClip[] clips;

	public RangedFloat volume;

	[MinMaxRange(0, 2)]
	public RangedFloat pitch;

	public override void Play(AudioSource source)
	{
		if (clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.PlayOneShot(source.clip);
	}
}
