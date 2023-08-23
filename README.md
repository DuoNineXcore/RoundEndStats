# Round End Stats
The RoundEndStats plugin provides server administrators with a customizable way to display round-end statistics to players. With a variety of placeholders, server admins can shape their round-end broadcast to their will. as shown by the list below. these are the commands that server admins can put in the "DisplayMessage" field that is in the EXILED config file.

# Config
```
later
```

Please Note that the `player` field is client sided, as it shows YOUR stats throughout the round.
Achievements have a hierarchy of importance. 

# Available Placeholders 
`{playerName}` Displays the nickname of the player.
`{playerKills}` Shows the number of players killed by the player.
`{playerDeaths}` Indicates the number of times the player died.
`{playerEscapeTime}` Shows the time taken by the player to escape.
`{playerTopAchievement}` Displays the most important achievement unlocked by the player during the round.
`{matchTime}` Displays the total duration of the round.
`{globalTopAchievement}` Shows the most significant achievement unlocked by any player during the round.
`{firstEscapee}` Shows the nickname of the first player to escape.
`{totalEscapes}` Indicates the total number of players who escaped.
`{topKiller}` Displays the top player with the most kills.
`{topKillerRole}` Indicates the role of the top killer.
`{topKillerKills}` Shows the number of kills by the top killer.
`{topSCPName}` Displays the name of the player who killed the most SCPs.
`{topSCPRole}` Indicates the role of the top SCP killer.
`{topSCPKills}` Shows the number of SCPs killed by the top SCP killer.
`{topSCPItemUserName}` Displays the name of the player who used the most SCP items.
`{topSCPItemUserRole}` Indicates the role of the top SCP item user.
`{topSCPItemUserCount}` Shows the number of SCP items used by the top SCP item user.
`{topSCPItemUserList}` Lists the SCP items used by the top SCP item user.
`{topHumanName}` Displays the name of the player who killed the most humans.
`{topHumanRole}` Indicates the role of the top human killer.
`{topHumanKills}` Shows the number of humans killed by the top human killer.

# Achievements
`SCP-Killer` Recognizes a player for killing an SCP.
`SCP-Killer II` Awarded for killing more than one SCP.
`Suicide Bomber` Given to players who kill 5 or more players using the Pink Candy.
`Peacemaker` For players who survive the round without killing anyone.
`Dimensional Dodger` For escaping SCP-106's pocket dimension twice.
`Staring Contest Champion` Survive SCP-096's rage after triggering it.
`Zombie Slayer` For killing 5 SCP-049-2 instances.
`One Against Many` Recognizes SCPs who kill a significant portion of an MTF or Chaos wave.
`Turncoat` For D-Class or Chaos Insurgents who handcuff and convert a guard, MTF, or scientist.
`Caffeine Junkie` For players who drink 3 or more colas and survive for more than 2 minutes.
`They're just resources.` For Scientists who kill 5 Class-D Personnel.

# Installation
**[EXILED](https://github.com/Exiled-Team/EXILED) must be installed for this to work.**

Place the "RoundEndStats.dll" file in your Plugins folder.

Windows: ``%appdata%/EXILED/Plugins``.

Linux: ``.config/EXILED/Plugins``.

![alt text](https://i5.walmartimages.com/seo/Fresh-Cantaloupe-Each_fb4c18a5-9367-4770-b99f-7518c72db482.5609c32e87a3110b734aad048bf9fe35.jpeg)
