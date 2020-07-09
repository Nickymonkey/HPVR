using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class SpellDictionary : MonoBehaviour
    {
        public List<Tuple<string, string, string>> SpellList = new List<Tuple<string, string, string>>();
        public List<Tuple<Tuple<string, string, string>, Tuple<string, string, string>>> SpellPageList = new List<Tuple<Tuple<string, string, string>, Tuple<string, string, string>>>();

        public SpellDictionary()
        {
            Tuple<string, string, string> tpBlank = new Tuple<string, string, string>("", "", "");
            Tuple<string, string, string> tpInstructions1 = new Tuple<string, string, string>("Spellbook", "", "This is your spellbook, it contains all the known spells a wizard or witch can cast using their wands! Flip through the pages to learn the spells and their uses.");
            Tuple<string, string, string> tpInstructions2 = new Tuple<string, string, string>("Spellcasting", "", "Every spell is different but to cast any spells you must have your wand in hand! Go ahead and pull out your wand and try!");
            Tuple<string, string, string> tp1 = new Tuple<string, string, string>("Accio", "To me", "Summons the affected object to the user's free hand");
            Tuple<string, string, string> tp2 = new Tuple<string, string, string>("Alarte", "Up", "Flings the affected object upwards into the air");
            Tuple<string, string, string> tp3 = new Tuple<string, string, string>("Arresto Momentum", "Freeze", "Briefly freezes an object in place");
            Tuple<string, string, string> tp4 = new Tuple<string, string, string>("Bombarda", "Bomb", "Creates a spell that can be shot and has explosive power");
            Tuple<string, string, string> tp5 = new Tuple<string, string, string>("Diminuendo", "Smaller", "Makes the affected object shrink in size");
            Tuple<string, string, string> tp6 = new Tuple<string, string, string>("Duro", "Stone", "Turns the affected object to stone increasing its mass");
            Tuple<string, string, string> tp7 = new Tuple<string, string, string>("Engorgio", "Bigger", "Makes the affected object grow in size");
            Tuple<string, string, string> tp8 = new Tuple<string, string, string>("Finite Incantatem", "Nullify", "Nullifies the affects of any spell on an object returning it to it's original state");
            Tuple<string, string, string> tp9 = new Tuple<string, string, string>("Geminio", "Duplicate", "Duplicates the affected object");
            Tuple<string, string, string> tp10 = new Tuple<string, string, string>("Nox", "Extinguish", "Extinguishes any spell currently on the tip of your wand");
            Tuple<string, string, string> tp11 = new Tuple<string, string, string>("Incendio", "Fireball", "Creates a ball of fire that can be shot");
            Tuple<string, string, string> tp12 = new Tuple<string, string, string>("Lumos", "Light", "Creates a ball of light that can be shot");
            Tuple<string, string, string> tp13 = new Tuple<string, string, string>("Stupify", "", "Creates a spell that an be shot and stun opponents");
            Tuple<string, string, string> tp14 = new Tuple<string, string, string>("Wingardium Leviosa", "Levitate", "Makes the affected object weightless");

            SpellList.Add(tp1);
            SpellList.Add(tp2);
            SpellList.Add(tp3);
            SpellList.Add(tp4);
            SpellList.Add(tp5);
            SpellList.Add(tp6);
            SpellList.Add(tp7);
            SpellList.Add(tp8);
            SpellList.Add(tp9);
            SpellList.Add(tp10);
            SpellList.Add(tp11);
            SpellList.Add(tp12);
            SpellList.Add(tp13);
            SpellList.Add(tp14);

            SpellPageList.Add(new Tuple<Tuple<string, string, string>, Tuple<string, string, string>>(tpBlank, tpBlank));
            SpellPageList.Add(new Tuple<Tuple<string, string, string>, Tuple<string, string, string>>(tpInstructions1, tpInstructions2));
            for (int i=0; i<SpellList.Count; i+=2)
            {
                Tuple<Tuple<string, string, string>, Tuple<string, string, string>> temp;
                if (SpellList.Count <= i + 1)
                {
                    temp = new Tuple<Tuple<string, string, string>, Tuple<string, string, string>>(SpellList[i], tpBlank);
                }
                else
                {
                    temp = new Tuple<Tuple<string, string, string>, Tuple<string, string, string>>(SpellList[i], SpellList[i + 1]);
                }
                SpellPageList.Add(temp);
            }
            SpellPageList.Add(new Tuple<Tuple<string, string, string>, Tuple<string, string, string>>(tpBlank, tpBlank));
        } 

        public Tuple<Tuple<string, string>, Tuple<string, string>> GetPageContents(int index)
        {
            if(index > SpellPageList.Count || index < 0)
            {
                return new Tuple<Tuple<string, string>, Tuple<string, string>>(new Tuple<string, string>("", ""), new Tuple<string, string>("", ""));
            }
            return new Tuple<Tuple<string, string>, Tuple<string, string>>(new Tuple<string, string>(SpellPageList[index].Item1.Item1, SpellPageList[index].Item1.Item3), new Tuple<string, string>(SpellPageList[index].Item2.Item1, SpellPageList[index].Item2.Item3));
        }
    }
}
