using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public Player player;
    public void PlayPunk()
    {
        if (!player.usingKeyboard)
            player.PlayPunk();
    }
    public void PlayClassic()
    {
        if (!player.usingKeyboard)
            player.PlayClassic();
    }
    public void PlayReggae()
    {
        if (!player.usingKeyboard)
            player.PlayReggae();
    }
    public void PlayEletronic()
    {
        if (!player.usingKeyboard)
            player.PlayEletronic();
    }
}
