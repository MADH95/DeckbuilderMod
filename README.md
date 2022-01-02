
# How to use

Once installed, run the game once, you should get a file in your BepInEx/config folder called MADH.inscryption.DeckbuilderMod.cfg, this is what you need to edit to add cards to your starting hand.

## Enabling Options

### Enabling the DeckBuilder

- `Enable DeckBuilder = false` to `Enable DeckBuilder = true`

### Clear deck before adding cards?

- Change `Clear deck before adding cards?` option from `false` to `true` so that only the cards you want to use are there.

### Number of Cards

- Change the `Number of Cards` option to the number of cards you want in your starting deck:

```
# Setting type: Int32
# Default value: 4
Number of Cards = 4

# Setting type: String
# Default value: Opossum
Card1 = Geck

Card2 = Geck

Card3 = PackRat

Card4 = PackRat
```

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
in `Inscryption/BepInEx/Config/BepInEx.cfg`