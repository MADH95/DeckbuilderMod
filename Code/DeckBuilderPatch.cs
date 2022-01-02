using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace TestDeckMod
{

    [HarmonyPatch( typeof( GameFlowManager ), nameof(GameFlowManager.Start) )]
    public class DeckBuilderPatch
    {
        public static bool canLoadDeck = true;
        public static bool doClearDeck = false;

        public static List<string> cardsToLoad = new();

        [HarmonyPostfix]
        public static void AddCardsToDeckAfterGameStarts()
        {
            if ( !canLoadDeck ) return ;
            
            if (doClearDeck)
            {
                Plugin.Log.LogInfo($"Clearing deck is enabled, clearing deck...");
                SaveManager.SaveFile.CurrentDeck.Cards.Clear();
            }

            foreach ( string card in cardsToLoad )
			{
                SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(card));
			}
        }
    }
}