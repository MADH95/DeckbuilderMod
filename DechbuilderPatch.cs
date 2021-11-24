using System.Collections.Generic;

using DiskCardGame;

using HarmonyLib;

namespace TestDeckMod
{

    [HarmonyPatch( typeof( DeckInfo ), "InitializeAsPlayerDeck" )]
    public class TestDeckPatch
    {
        public static bool canLoadDeck = true;

        public static List<string> cardsToLoad = new();

        [HarmonyPrefix]
        public static bool Prefix( ref DeckInfo __instance )
        {
            if ( !canLoadDeck )
                return true;

            foreach ( string card in cardsToLoad )
			{
                __instance.AddCard( CardLoader.GetCardByName( card ) );
			}

            return false;
        }
    }
}