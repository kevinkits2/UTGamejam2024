using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHungerBar : MonoBehaviour {

    [SerializeField] private GameObject normalHungerBar;
    [SerializeField] private GameObject rageHungerBar;
    [SerializeField] private Image normalFillbar;
    [SerializeField] private Image rageFillbar;

    private Creature creature;


    private void Awake() {
        creature = GetComponentInParent<Creature>();
        normalHungerBar.SetActive(false);
        rageHungerBar.SetActive(false);

        GameManagerEvents.OnMouseOverCreature += HandleMouseOverCreature;
        GameManagerEvents.OnMouseNotOverCreature += HandleMouseNotOverCreature;
    }

    private void OnDestroy() {
        GameManagerEvents.OnMouseOverCreature -= HandleMouseOverCreature;
        GameManagerEvents.OnMouseNotOverCreature -= HandleMouseNotOverCreature;
    }

    private void HandleMouseNotOverCreature() {
        if (creature.GetState() == CreatureState.Rage) {
            rageHungerBar.SetActive(false);
        }
        else {
            normalHungerBar.SetActive(false);
        }
    }

    private void HandleMouseOverCreature(Creature creature) {
        if (creature != this.creature) return;

        if (creature.GetState() == CreatureState.Rage) {
            rageHungerBar.SetActive(true);
        }
        else {
            normalHungerBar.SetActive(true);
        }
    }

    private void Update() {
        normalFillbar.fillAmount = (float)creature.GetHunger() / 100;
        rageFillbar.fillAmount = (float)creature.GetHunger() / 100;
    }
}
