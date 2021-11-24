
# How to use

Once installed, run the game once, you should get a file in your BepInEx/config folder called MADH.inscryption.DeckbuilderMod.cfg, this is what you need to edit to add cards to your starting hand.

Simply enable test deck by setting

```
## Load starter deck with specified cards
# Setting type: Boolean
# Default value: false
Enable Deckbuilder = false
```
to
```
## Load starter deck with specified cards
# Setting type: Boolean
# Default value: false
Enable Deckbuilder = true
```

Next you can change the Number of Cards field to the number of cards you want in your starting deck.

Finally, you rename the name after the equals following "Card1". You can copy the assignment to card one below it to add the rest of your cards, simply increase the number by 1 as you add cards.

___

The easiest way to check if the plugin is working properly or to debug an error is to enable the console. This can be done by changing
```
[Logging.Console]
\## Enables showing a console for log output.
\# Setting type: Boolean
\# Default value: false
Enabled = false
```
to
```
[Logging.Console]
\## Enables showing a console for log output.
\# Setting type: Boolean
\# Default value: false
Enabled = true
```
in **Inscryption/BepInEx/Config/BepInEx.cfg**