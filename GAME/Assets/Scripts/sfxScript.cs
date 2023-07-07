using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxScript : MonoBehaviour
{
    // Lista de sons que estão sendo tocados
    public List<audioTuple> audioQueue = new List<audioTuple>();

    // Tupla com o tempo de duração de um som e o som
    public struct audioTuple
    {
        public float time;
        public AudioClip clip;

        public audioTuple(float time_,AudioClip clip_)
        {
            time = time_;
            clip = clip_;
        }
    }

    public void playSoundContinuously(AudioClip clip)
    {
        bool newClip = true;

        for (int i = 0;i < audioQueue.Count;i ++)
        {
            if (audioQueue[i].clip == clip)
            { // O som já está sendo tocado
                newClip = false;
            }

            if (audioQueue[i].time <= 0)
            { // O tempo restante do som acabou -> removendo o som da lista
                audioQueue.RemoveAt(i);
                i --;
            }
            else { // Diminuindo o tempo restante
                audioQueue[i] = new audioTuple(audioQueue[i].time - Time.deltaTime, audioQueue[i].clip);
            }
        }

        if (newClip)
        { // Adicionando um som novo a lista
            audioQueue.Add(new audioTuple(clip.length, clip));
            GetComponent<AudioSource>().PlayOneShot(clip);
        }

    }


}
