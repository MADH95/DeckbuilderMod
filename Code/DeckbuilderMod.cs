using System.Runtime.CompilerServices;

using DiskCardGame;

using BepInEx;
using BepInEx.Logging;
using BepInEx.Bootstrap;
using BepInEx.Configuration;

using HarmonyLib;

using APIPlugin;

namespace TestDeckMod
{
    [BepInPlugin( PluginGUID, PluginName, PluginVersion )]
    [BepInDependency( APIGUID, BepInDependency.DependencyFlags.SoftDependency )]
    [BepInDependency( JLoaderGUID, BepInDependency.DependencyFlags.SoftDependency )]
    public class Plugin : BaseUnityPlugin
    {
		private const string APIGUID = "cyantist.inscryption.api";
        private const string JLoaderGUID = "MADH.inscryption.JSONLoader";
        private const string PluginGUID = "MADH.inscryption.TestDeckMod";
        private const string PluginName = "TestDeckMod";
        private const string PluginVersion = "1.0.0.0";

        internal static ManualLogSource Log;

        public bool GetTestDeck() 
            => Config.Bind( PluginName, "TestDeck", false, new ConfigDescription( "Load starter deck with specified cards" ) ).Value;

        public int GetNumCards() 
            => Config.Bind( PluginName, "Number of Cards", 4 ).Value;

        public string GetCard( int index ) 
            => Config.Bind( PluginName, "Card" + (index + 1), "Opossum" ).Value;

        private void Awake()
        {
            Logger.LogInfo( $"Loaded {PluginName}!" );
            Log = base.Logger;

            Harmony harmony = new( PluginGUID );
            harmony.PatchAll();

        }

        private void Start()
        {
            bool TryCheckAPICards( string name )
            {
                if ( !Chainloader.PluginInfos.ContainsKey( APIGUID ) )
                    return false;

                return CheckAPICards( name );
            }

            bool enableTestDeck = GetTestDeck();

            int numCards = GetNumCards();

            GetCard( 0 );

            if ( !enableTestDeck )
            {
                TestDeckPatch.canLoadDeck = false;
                return;
            }

            for ( int i = 0; i < numCards; i++ )
            {
                string name = GetCard( i );

                bool gameCard = ScriptableObjectLoader<CardInfo>.AllData.Exists( elem => elem.name == name );

                bool APICard = TryCheckAPICards( name );

                if ( !gameCard && !APICard )
                {
                    Logger.LogError( $"Can't find card with name \"{ name }\" to add to deck" );
                    TestDeckPatch.canLoadDeck = false;
                    continue;
                }

                

                Logger.LogMessage( $"\"{ name }\" added to deck" );
                TestDeckPatch.cardsToLoad.Add( name );
            }

            ScriptableObjectLoader<CardInfo>.allData = null;
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        private bool CheckAPICards(string name)
            => CustomCard.cards.Exists( elem => elem.name == name ) ||
               NewCard.cards.Exists( elem => elem.name == name );
    }
}
