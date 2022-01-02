using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace TestDeckMod
{
    [BepInPlugin( PluginGUID, PluginName, PluginVersion )]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGUID = "MADH.inscryption.TestDeckMod";
        private const string PluginName = "TestDeckMod";
        private const string PluginVersion = "1.1.0";

        internal static ManualLogSource Log;

        public bool GetTestDeck() 
            => Config.Bind( PluginName, "TestDeck", false, new ConfigDescription( "Load starter deck with specified cards" ) ).Value;
        
        public bool DoClearDeckBeforeAddingCards() 
            => Config.Bind( PluginName, "Clear deck before adding cards?", false, new ConfigDescription( "Clear your deck before adding cards so that only the cards in the config will be there" ) ).Value;

        public int GetNumCards() 
            => Config.Bind( PluginName, "Number of Cards", 4 ).Value;

        public string GetCard( int index ) 
            => Config.Bind( PluginName, "Card" + (index + 1), "Opossum" ).Value;

        private void Awake()
        {
            Log = base.Logger;

            Harmony harmony = new( PluginGUID );
            harmony.PatchAll();

            Log.LogInfo( $"Loaded {PluginName}!" );
        }

        private async void Start()
        {
            Plugin.Log.LogDebug($"Starting DeckBuilder...");
            bool enableTestDeck = GetTestDeck();
            bool doClearDeck = DoClearDeckBeforeAddingCards();
            
            TestDeckPatch.canLoadDeck = enableTestDeck;
            TestDeckPatch.doClearDeck = doClearDeck; 
            
            int numCards = GetNumCards();

            GetCard( 0 );

            if ( enableTestDeck)
            {
                bool result = await WaitForAllCardsToBeLoaded();

                for ( int i = 0; i < numCards; i++ )
                {
                    string nameOfCardIter = GetCard( i );
                    
                    if (!CheckThatCardExists(nameOfCardIter))
                    {
                        Logger.LogError( $"Can't find card with name \"{ nameOfCardIter }\" to add to deck" );
                        TestDeckPatch.canLoadDeck = false;
                        continue;
                    }
                    
                    Logger.LogMessage( $"\"{ nameOfCardIter }\" added to deck" );
                    TestDeckPatch.cardsToLoad.Add( nameOfCardIter );
                }
            }
            else
            {
                Logger.LogWarning($"Skipping DeckBuilder as EnableTestDeck is false");
            }
            Plugin.Log.LogDebug($"Finished loading DeckBuilder");
        }

        private static async Task<bool> WaitForAllCardsToBeLoaded()
        {
            bool succeeded = ScriptableObjectLoader<CardInfo>.allData is not null;
            while (!succeeded)
            {
                // do work
                succeeded = ScriptableObjectLoader<CardInfo>.allData is not null; // if it worked, make as succeeded, else retry
                await Task.Delay(1000); // arbitrary delay
            }

            Log.LogDebug("All cards have been loaded! ");
            return succeeded;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool CheckThatCardExists(string nameOfCard)
        {
            bool MatchesNameOfCard(CardInfo card) => card.name == nameOfCard;

            // we only check ScriptableObjectLoader<CardInfo>.allData because once the API loads all custom and modified cards,
            //  this will then return true.
            return ScriptableObjectLoader<CardInfo>.allData.Exists(MatchesNameOfCard);
        }
    }
}
