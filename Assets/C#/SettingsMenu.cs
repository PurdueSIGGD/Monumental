using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SettingsMenu : NetworkBehaviour
{

    public InputField health, movement, interaction, gather, melee, ranged, carry;
    private GameSettings gameSettings;

    public void save()
    {
        float temp = 0;
        float.TryParse(health.text, out temp);
        gameSettings.Health = (int)temp;
        float.TryParse(movement.text, out gameSettings.Movement);
        float.TryParse(interaction.text, out gameSettings.Interaction);
        float.TryParse(gather.text, out gameSettings.Gather);
        float.TryParse(melee.text, out temp);
        gameSettings.Melee = (int)temp;
        float.TryParse(ranged.text, out temp);
        gameSettings.Ranged = (int)temp;
        float.TryParse(carry.text, out temp);
        gameSettings.Carry = (int)temp;

    }

    public void resetSettings()
    {
        if (!gameSettings)
        {
            gameSettings = GameObject.FindObjectOfType<GameSettings>();
        }
        health.text = "" + gameSettings.Health;
        movement.text = "" + gameSettings.Movement;
        interaction.text = "" + gameSettings.Interaction;
        gather.text = "" + gameSettings.Gather;
        melee.text = "" + gameSettings.Melee;
        ranged.text = "" + gameSettings.Ranged;
        carry.text = "" + gameSettings.Carry;

    }

    public void resetToDefault()
    {
        gameSettings.resetSettings();
        resetSettings();
        save();
    }

}
