using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpellManager : MonoBehaviour {
    public Spell[] spellSlot = new Spell[4];
    public Image[] spellIcon = new Image[4];
    public int selectedSpell = 0;
    public string currentSpell;

    private bool[] spellsInEffect;

    public bool casting = false; // if casting a spell it means you cant switch and or start another spell

    public RectTransform spellHighlight;
    private float spellHighlightStartX;
    public float spellHighlightIncrement;
    // Use this for initialization
    void Start () {
        spellsInEffect = new bool[spellSlot.Length];
        for(int i = 0;i <= spellSlot.Length-1; i++) {
            spellSlot[i].master = this;
            spellSlot[i].spellID = i;
        }
        
        currentSpell = spellSlot[selectedSpell].spellName;
        spellHighlightStartX = spellHighlight.anchoredPosition.x;
    }
	
	// Update is called once per frame
	void Update () {
        SelectSpell();
        CheckForCoolDown();
	}
    public void SelectSpell() {
        if (casting == false) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                selectedSpell = 0;
                currentSpell = spellSlot[selectedSpell].spellName;
                float temp = spellHighlightStartX + (spellHighlightIncrement * (0));
                spellHighlight.anchoredPosition = new Vector2(temp, spellHighlight.anchoredPosition.y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                selectedSpell = 1;
                currentSpell = spellSlot[selectedSpell].spellName;
                float temp = spellHighlightStartX + (spellHighlightIncrement * (1));
                spellHighlight.anchoredPosition = new Vector2(temp, spellHighlight.anchoredPosition.y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                selectedSpell = 2;
                currentSpell = spellSlot[selectedSpell].spellName;
                float temp = spellHighlightStartX + (spellHighlightIncrement * (2));
                spellHighlight.anchoredPosition = new Vector2(temp, spellHighlight.anchoredPosition.y);
            }
            /*if (Input.GetKeyDown(KeyCode.Alpha4)) {
                selectedSpell = 4;
                currentSpell = spellSlot[selectedSpell].spellName;
                float temp = spellHighlightStartX + (spellHighlightIncrement * (3));
                spellHighlight.anchoredPosition = new Vector2(temp, spellHighlight.anchoredPosition.y);
            }*/
        }
    }
    public void CheckForCoolDown() {
        for(int i =0; i<= spellSlot.Length-1;i++) {
            if (spellSlot[i].onCoolDown)
                spellIcon[i].enabled = false;
            else
                spellIcon[i].enabled = true;
        }
    }

    public bool Casting(int id) {
        if (casting == false) {
            casting = true;
            InEffect(id, true);
            return (true);
        }
        return (false);
    }
    public void InEffect(int id, bool isActive) {
        spellsInEffect[id] = isActive;
    }
}
