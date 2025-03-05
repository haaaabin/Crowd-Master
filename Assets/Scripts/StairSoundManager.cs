using UnityEngine;
using System.Collections.Generic;

public class StairSoundManager : MonoBehaviour
{
    public static StairSoundManager instance;
    public AudioSource audioSource;
    public AudioClip[] notes;
    private HashSet<int> playedStairs = new HashSet<int>(); //이미 소리난 계단 저장

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayNote(int stairIndex, AudioSource newAudioSource)
    {
        if (notes.Length == 0) return;
        if (playedStairs.Contains(stairIndex)) return;
        
        // 계단 인덱스에 맞는 소리를 재생 (8개를 초과하면 다시 '도'로 반복)
        int noteIndex = stairIndex % notes.Length;

        audioSource = newAudioSource;
        audioSource.PlayOneShot(notes[noteIndex]);
        playedStairs.Add(stairIndex);
    }
}
