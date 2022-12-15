using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ItemManager;
using PieceManager;
using ServerSync;
using UnityEngine;


namespace OdinsSteelworks
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class OdinsSteelworks : BaseUnityPlugin
    {
        private const string ModName = "OdinsSteelworks";
        private const string ModVersion = "0.0.15";
        private const string ModGUID = "org.bepinex.plugins.odinssteelworks";

        public void Awake()
        {
            #region pieces 

            BuildPiece CWS_Forge = new BuildPiece("cwsassets", "CWS_Forge");
            CWS_Forge.Name.English("Steel Forge");
            CWS_Forge.Description.English("A steel foundry workbench");
            CWS_Forge.RequiredItems.Add("Stone", 10, true);
            CWS_Forge.RequiredItems.Add("Iron", 5, true);
            CWS_Forge.RequiredItems.Add("DragonTear", 1, true);
            CWS_Forge.Category.Add(BuildPieceCategory.Crafting);

            BuildPiece CWS_Slack_Tub = new BuildPiece("cwsassets", "CWS_Slack_Tub");
            CWS_Slack_Tub.Name.English("Steel Slack Tub");
            CWS_Slack_Tub.Description.English("An upgrade station for cooling steel");
            CWS_Slack_Tub.RequiredItems.Add("Iron", 2, true);
            CWS_Slack_Tub.RequiredItems.Add("CWS_SurtlingCoal", 2, true);
            CWS_Slack_Tub.RequiredItems.Add("RoundLog", 5, true);
            CWS_Slack_Tub.Category.Add(BuildPieceCategory.Crafting);
            
            BuildPiece CWS_Stone_Kiln = new BuildPiece("cwsassets", "CWS_Stone_Kiln");
            CWS_Stone_Kiln.Name.English("Steel Kiln");
            CWS_Stone_Kiln.Description.English("A clay and stone kiln to smelt steel.");
            CWS_Stone_Kiln.RequiredItems.Add("CWS_SurtlingCoal", 4, true);
            CWS_Stone_Kiln.RequiredItems.Add("Iron", 5, true);
            CWS_Stone_Kiln.RequiredItems.Add("Stone", 20, true);
            CWS_Stone_Kiln.Category.Add(BuildPieceCategory.Crafting);

            BuildPiece CWS_Steel_Mold = new BuildPiece("cwsassets", "CWS_Steel_Mold");
            CWS_Steel_Mold.Name.English("Steel Mold");
            CWS_Steel_Mold.Description.English("A mold for forming raw steel");
            CWS_Steel_Mold.RequiredItems.Add("Iron", 1, true);
            CWS_Steel_Mold.RequiredItems.Add("CWS_SurtlingCoal", 2, true);
            CWS_Steel_Mold.RequiredItems.Add("Stone", 5, true);
            CWS_Steel_Mold.Category.Add(BuildPieceCategory.Crafting);

            BuildPiece CWS_Item_Stand = new BuildPiece("cwsassets", "CWS_Item_Stand");
            CWS_Item_Stand.Name.English("Blue Weapon Display");
            CWS_Item_Stand.Description.English("A blue display for weapons");
            CWS_Item_Stand.RequiredItems.Add("CWS_Cold_Steel", 4, true);
            CWS_Item_Stand.RequiredItems.Add("CWS_SurtlingCoal", 2, true);
            CWS_Item_Stand.RequiredItems.Add("DeerHide", 2, true);
            CWS_Item_Stand.Category.Add(BuildPieceCategory.Crafting);

            BuildPiece CWS_Item_Stand_2 = new BuildPiece("cwsassets", "CWS_Item_Stand_2");
            CWS_Item_Stand_2.Name.English("Red Weapon Display");
            CWS_Item_Stand_2.Description.English("A red display for weapons");
            CWS_Item_Stand_2.RequiredItems.Add("CWS_Cold_Steel", 4, true);
            CWS_Item_Stand_2.RequiredItems.Add("CWS_SurtlingCoal", 2, true);
            CWS_Item_Stand_2.RequiredItems.Add("DeerHide", 2, true);
            CWS_Item_Stand_2.Category.Add(BuildPieceCategory.Crafting);

            BuildPiece CWS_Steel_Pile = new BuildPiece("cwsassets", "CWS_Steel_Pile");
            CWS_Steel_Pile.Name.English("Steel Pile");
            CWS_Steel_Pile.Description.English("A pile of cold steel, ready for tossing into the kiln to forge.");
            CWS_Steel_Pile.RequiredItems.Add("CWS_Cold_Steel", 20, true);
            CWS_Steel_Pile.Category.Add(BuildPieceCategory.Crafting);

            #endregion
            #region materials


            Item CWS_SurtlingCoal = new Item("cwsassets", "CWS_SurtlingCoal");
            CWS_SurtlingCoal.Name.English("Surtling Coal");
            CWS_SurtlingCoal.Description.English("A coal that burns hotter than the sun");
            CWS_SurtlingCoal.Crafting.Add("CWS_Forge", 1);
            CWS_SurtlingCoal.RequiredItems.Add("Coal", 5);
            CWS_SurtlingCoal.RequiredItems.Add("SurtlingCore", 1);
            CWS_SurtlingCoal.CraftAmount = 10;

            Item CWS_Crucible_Full = new Item("cwsassets", "CWS_Crucible_Full");
            CWS_Crucible_Full.Name.English("Cold Steel Crucible");
            CWS_Crucible_Full.Description.English("A container with carbon and iron scrap");
            CWS_Crucible_Full.Crafting.Add("CWS_Forge", 1);
            CWS_Crucible_Full.RequiredItems.Add("CWS_SurtlingCoal", 2);
            CWS_Crucible_Full.RequiredItems.Add("IronScrap", 4);
            CWS_Crucible_Full.CraftAmount = 1;

            Item CWS_Crucible_Finished = new Item("cwsassets", "CWS_Crucible_Finished");
            CWS_Crucible_Finished.Name.English("Liquid Steel Crucible");
            CWS_Crucible_Finished.Description.English("A molten steel filled container");
            CWS_Crucible_Finished.Crafting.Add("CWS_Forge", 30);
            CWS_Crucible_Finished.RequiredItems.Add("SwordCheat", 1);
            CWS_Crucible_Finished.CraftAmount = 1;

            Item CWS_Cold_Steel = new Item("cwsassets", "CWS_Cold_Steel");
            CWS_Cold_Steel.Name.English("Cold Steel");
            CWS_Cold_Steel.Description.English("A cold steel material, great for storage.");
            CWS_Cold_Steel.Crafting.Add("CWS_Forge", 30);
            CWS_Cold_Steel.RequiredItems.Add("SwordCheat", 1);
            CWS_Cold_Steel.CraftAmount = 1;

            Item CWS_Hot_Steel = new Item("cwsassets", "CWS_Hot_Steel");
            CWS_Hot_Steel.Name.English("Oxidized Steel");
            CWS_Hot_Steel.Description.English("An oxidized steel material");
            CWS_Hot_Steel.Crafting.Add("CWS_Forge", 30);
            CWS_Hot_Steel.RequiredItems.Add("SwordCheat", 1);
            CWS_Hot_Steel.CraftAmount = 1;

            Item CWS_Hot_Steel_Finished = new Item("cwsassets", "CWS_Hot_Steel_Finished");
            CWS_Hot_Steel_Finished.Name.English("Finished Steel");
            CWS_Hot_Steel_Finished.Description.English("A heated steel, ready for forging a weapon. Placing this back into the Water Barrel will let it cool.");
            CWS_Hot_Steel_Finished.Crafting.Add("CWS_Forge", 30);
            CWS_Hot_Steel_Finished.RequiredItems.Add("SwordCheat", 1);
            CWS_Hot_Steel_Finished.CraftAmount = 1;

            #endregion
            # region 2h axes

            Item CWS_TH_Axe_1 = new Item("cwsassets", "CWS_TH_Axe_1");
            CWS_TH_Axe_1.Name.English("Steel Forged Axe");
            CWS_TH_Axe_1.Description.English("A steel forged skulls splitter axe. The joy of all vikings.");
            CWS_TH_Axe_1.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_1.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_1.Configurable = Configurability.Recipe;
            CWS_TH_Axe_1.CraftAmount = 1;


            Item CWS_TH_Axe_2 = new Item("cwsassets", "CWS_TH_Axe_2");
            CWS_TH_Axe_2.Name.English("The Splitter");
            CWS_TH_Axe_2.Description.English("A steel forged skulls splitter axe. The joy of all vikings.");
            CWS_TH_Axe_2.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_2.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_2.Configurable = Configurability.Recipe;
            CWS_TH_Axe_2.CraftAmount = 1;


            Item CWS_TH_Axe_3 = new Item("cwsassets", "CWS_TH_Axe_3");
            CWS_TH_Axe_3.Name.English("Double Edge Axe");
            CWS_TH_Axe_3.Description.English("A steel forged skulls splitter axe. The joy of all vikings.");
            CWS_TH_Axe_3.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_3.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_3.Configurable = Configurability.Recipe;
            CWS_TH_Axe_3.CraftAmount = 1;


            Item CWS_TH_Axe_4 = new Item("cwsassets", "CWS_TH_Axe_4");
            CWS_TH_Axe_4.Name.English("The Butcher");
            CWS_TH_Axe_4.Description.English("A steel forged skulls splitter axe. The joy of all vikings.");
            CWS_TH_Axe_4.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_4.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_4.Configurable = Configurability.Recipe;
            CWS_TH_Axe_4.CraftAmount = 1;


            Item CWS_TH_Axe_5 = new Item("cwsassets", "CWS_TH_Axe_5");
            CWS_TH_Axe_5.Name.English("Nott Gardkell");
            CWS_TH_Axe_5.Description.English("A steel forged skin-tear apart axe. Use under adults supervision.");
            CWS_TH_Axe_5.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_5.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_5.Configurable = Configurability.Recipe;
            CWS_TH_Axe_5.CraftAmount = 1;


            Item CWS_TH_Axe_6 = new Item("cwsassets", "CWS_TH_Axe_6");
            CWS_TH_Axe_6.Name.English("Dane Axe");
            CWS_TH_Axe_6.Description.English("A steel forged skulls splitter axe. The joy of all vikings.");
            CWS_TH_Axe_6.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_6.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_6.Configurable = Configurability.Recipe;
            CWS_TH_Axe_6.CraftAmount = 1;


            Item CWS_TH_Axe_7 = new Item("cwsassets", "CWS_TH_Axe_7");
            CWS_TH_Axe_7.Name.English("Orta");
            CWS_TH_Axe_7.Description.English("A steel forged skulls splitter axe. The joy of all vikings.");
            CWS_TH_Axe_7.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_7.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_7.Configurable = Configurability.Recipe;
            CWS_TH_Axe_7.CraftAmount = 1;
;

            Item CWS_TH_Axe_8 = new Item("cwsassets", "CWS_TH_Axe_8");
            CWS_TH_Axe_8.Name.English("Kaba Teiris");
            CWS_TH_Axe_8.Description.English("A steel forged skulls splitter axe. The joy of all vikings.");
            CWS_TH_Axe_8.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_8.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_8.Configurable = Configurability.Recipe;
            CWS_TH_Axe_8.CraftAmount = 1;


            Item CWS_TH_Axe_9 = new Item("cwsassets", "CWS_TH_Axe_9");
            CWS_TH_Axe_9.Name.English("Solefald");
            CWS_TH_Axe_9.Description.English("A steel forged skulls splitter axe. The two blades are called Death and Revenge");
            CWS_TH_Axe_9.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_9.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_9.Configurable = Configurability.Recipe;
            CWS_TH_Axe_9.CraftAmount = 1;


            Item CWS_TH_Axe_10 = new Item("cwsassets", "CWS_TH_Axe_10");
            CWS_TH_Axe_10.Name.English("Scarfuld");
            CWS_TH_Axe_10.Description.English("A steel forged axe. This will make a brutal mess of things.");
            CWS_TH_Axe_10.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Axe_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Axe_10.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Axe_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Axe_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Axe_10.Configurable = Configurability.Recipe;
            CWS_TH_Axe_10.CraftAmount = 1;


            #endregion
            #region 2h maces

            Item CWS_Sledge_1 = new Item("cwsassets", "CWS_Sledge_1");
            CWS_Sledge_1.Name.English("Aendrider");
            CWS_Sledge_1.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_1.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_1.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_1.Configurable = Configurability.Recipe;
            CWS_Sledge_1.CraftAmount = 1;


            Item CWS_Sledge_2 = new Item("cwsassets", "CWS_Sledge_2");
            CWS_Sledge_2.Name.English("Khanafoss");
            CWS_Sledge_2.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_2.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_2.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_2.Configurable = Configurability.Recipe;
            CWS_Sledge_2.CraftAmount = 1;


            Item CWS_Sledge_3 = new Item("cwsassets", "CWS_Sledge_3");
            CWS_Sledge_3.Name.English("Defiant Farmer");
            CWS_Sledge_3.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_3.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_3.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_3.Configurable = Configurability.Recipe;
            CWS_Sledge_3.CraftAmount = 1;


            Item CWS_Sledge_4 = new Item("cwsassets", "CWS_Sledge_4");
            CWS_Sledge_4.Name.English("Darel");
            CWS_Sledge_4.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_4.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_4.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_4.Configurable = Configurability.Recipe;
            CWS_Sledge_4.CraftAmount = 1;


            Item CWS_Sledge_5 = new Item("cwsassets", "CWS_Sledge_5");
            CWS_Sledge_5.Name.English("Enkak");
            CWS_Sledge_5.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_5.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_5.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_5.Configurable = Configurability.Recipe;
            CWS_Sledge_5.CraftAmount = 1;


            Item CWS_Sledge_6 = new Item("cwsassets", "CWS_Sledge_6");
            CWS_Sledge_6.Name.English("Kabayan");
            CWS_Sledge_6.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_6.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_6.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_6.Configurable = Configurability.Recipe;
            CWS_Sledge_6.CraftAmount = 1;


            Item CWS_Sledge_7 = new Item("cwsassets", "CWS_Sledge_7");
            CWS_Sledge_7.Name.English("Sagavatn");
            CWS_Sledge_7.Description.English("An heavy, wonderful-looking steel forged hammer. Refined with attention. Not a single bone will be left unbroken.");
            CWS_Sledge_7.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_7.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_7.Configurable = Configurability.Recipe;
            CWS_Sledge_7.CraftAmount = 1;


            Item CWS_Sledge_8 = new Item("cwsassets", "CWS_Sledge_8");
            CWS_Sledge_8.Name.English("Vandrad");
            CWS_Sledge_8.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_8.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_8.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_8.Configurable = Configurability.Recipe;
            CWS_Sledge_8.CraftAmount = 1;


            Item CWS_Sledge_9 = new Item("cwsassets", "CWS_Sledge_9");
            CWS_Sledge_9.Name.English("Wakkein");
            CWS_Sledge_9.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_9.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_9.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_9.Configurable = Configurability.Recipe;
            CWS_Sledge_9.CraftAmount = 1;


            Item CWS_Sledge_10 = new Item("cwsassets", "CWS_Sledge_10");
            CWS_Sledge_10.Name.English("Seekershell Facesplitter");
            CWS_Sledge_10.Description.English("An heavy steel forged hammer. Not a single bone will be left unbroken.");
            CWS_Sledge_10.Crafting.Add("CWS_Forge", 2);
            CWS_Sledge_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sledge_10.RequiredItems.Add("RoundLog", 4);
            CWS_Sledge_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sledge_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sledge_10.Configurable = Configurability.Recipe;
            CWS_Sledge_10.CraftAmount = 1;


            #endregion
            #region 2h swords

            Item CWS_TH_Sword_1 = new Item("cwsassets", "CWS_TH_Sword_1");
            CWS_TH_Sword_1.Name.English("Daidalos");
            CWS_TH_Sword_1.Description.English("Daidalos is a symbol of wisdom, knowledge, and power. Use this weapon properly.");
            CWS_TH_Sword_1.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_1.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_1.Configurable = Configurability.Recipe;
            CWS_TH_Sword_1.CraftAmount = 1;


            Item CWS_TH_Sword_2 = new Item("cwsassets", "CWS_TH_Sword_2");
            CWS_TH_Sword_2.Name.English("Gardakan");
            CWS_TH_Sword_2.Description.English("A steel forged sword.");
            CWS_TH_Sword_2.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_2.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_2.Configurable = Configurability.Recipe;
            CWS_TH_Sword_2.CraftAmount = 1;


            Item CWS_TH_Sword_3 = new Item("cwsassets", "CWS_TH_Sword_3");
            CWS_TH_Sword_3.Name.English("Ritcher");
            CWS_TH_Sword_3.Description.English("A steel forged sword. This sword has the only duty to judge the enemies.");
            CWS_TH_Sword_3.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_3.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_3.Configurable = Configurability.Recipe;
            CWS_TH_Sword_3.CraftAmount = 1;


            Item CWS_TH_Sword_4 = new Item("cwsassets", "CWS_TH_Sword_4");
            CWS_TH_Sword_4.Name.English("Flamberge");
            CWS_TH_Sword_4.Description.English("A steel forged sword. Its shape is made to inflict more pain as it cuts or pierces enemies.");
            CWS_TH_Sword_4.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_4.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_4.Configurable = Configurability.Recipe;
            CWS_TH_Sword_4.CraftAmount = 1;


            Item CWS_TH_Sword_5 = new Item("cwsassets", "CWS_TH_Sword_5");
            CWS_TH_Sword_5.Name.English("Sheeden");
            CWS_TH_Sword_5.Description.English("A steel forged sword.");
            CWS_TH_Sword_5.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_5.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_5.Configurable = Configurability.Recipe;
            CWS_TH_Sword_5.CraftAmount = 1;


            Item CWS_TH_Sword_6 = new Item("cwsassets", "CWS_TH_Sword_6");
            CWS_TH_Sword_6.Name.English("Seírios");
            CWS_TH_Sword_6.Description.English("This steel forged sword is inspired by a Latin word Sirius meaning scorching, destructive.");
            CWS_TH_Sword_6.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_6.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_6.Configurable = Configurability.Recipe;
            CWS_TH_Sword_6.CraftAmount = 1;


            Item CWS_TH_Sword_7 = new Item("cwsassets", "CWS_TH_Sword_7");
            CWS_TH_Sword_7.Name.English("Engeram");
            CWS_TH_Sword_7.Description.English("A steel forged sword.");
            CWS_TH_Sword_7.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_7.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_7.Configurable = Configurability.Recipe;
            CWS_TH_Sword_7.CraftAmount = 1;


            Item CWS_TH_Sword_8 = new Item("cwsassets", "CWS_TH_Sword_8");
            CWS_TH_Sword_8.Name.English("Aleister");
            CWS_TH_Sword_8.Description.English("A steel forged sword.");
            CWS_TH_Sword_8.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_8.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_8.Configurable = Configurability.Recipe;
            CWS_TH_Sword_8.CraftAmount = 1;


            Item CWS_TH_Sword_9 = new Item("cwsassets", "CWS_TH_Sword_9");
            CWS_TH_Sword_9.Name.English("Caltrain");
            CWS_TH_Sword_9.Description.English("A steel forged sword. Its unusual shape makes it difficult to swing. Only for the best of vikings.");
            CWS_TH_Sword_9.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_9.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_9.Configurable = Configurability.Recipe;
            CWS_TH_Sword_9.CraftAmount = 1;


            Item CWS_TH_Sword_10 = new Item("cwsassets", "CWS_TH_Sword_10");
            CWS_TH_Sword_10.Name.English("August Aleister");
            CWS_TH_Sword_10.Description.English("A steel forged sword.");
            CWS_TH_Sword_10.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_10.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_10.Configurable = Configurability.Recipe;
            CWS_TH_Sword_10.CraftAmount = 1;


            Item CWS_TH_Sword_11 = new Item("cwsassets", "CWS_TH_Sword_11");
            CWS_TH_Sword_11.Name.English("RavenStorm");
            CWS_TH_Sword_11.Description.English("A steel forged sword named after a young goddess.");
            CWS_TH_Sword_11.Crafting.Add("CWS_Forge", 2);
            CWS_TH_Sword_11.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_TH_Sword_11.RequiredItems.Add("RoundLog", 4);
            CWS_TH_Sword_11.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_TH_Sword_11.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_TH_Sword_11.Configurable = Configurability.Recipe;
            CWS_TH_Sword_11.CraftAmount = 1;


            #endregion
            #region atgeir

            Item CWS_Atgeir_1 = new Item("cwsassets", "CWS_Atgeir_1");
            CWS_Atgeir_1.Name.English("Yuki-onna's Embrace");
            CWS_Atgeir_1.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_1.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_1.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_1.Configurable = Configurability.Recipe;
            CWS_Atgeir_1.CraftAmount = 1;


            Item CWS_Atgeir_2 = new Item("cwsassets", "CWS_Atgeir_2");
            CWS_Atgeir_2.Name.English("Hayakawa");
            CWS_Atgeir_2.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_2.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_2.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_2.Configurable = Configurability.Recipe;
            CWS_Atgeir_2.CraftAmount = 1;


            Item CWS_Atgeir_3 = new Item("cwsassets", "CWS_Atgeir_3");
            CWS_Atgeir_3.Name.English("Ikezawa");
            CWS_Atgeir_3.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_3.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_3.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_3.Configurable = Configurability.Recipe;
            CWS_Atgeir_3.CraftAmount = 1;


            Item CWS_Atgeir_4 = new Item("cwsassets", "CWS_Atgeir_4");
            CWS_Atgeir_4.Name.English("Iwase");
            CWS_Atgeir_4.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_4.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_4.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_4.Configurable = Configurability.Recipe;
            CWS_Atgeir_4.CraftAmount = 1;


            Item CWS_Atgeir_5 = new Item("cwsassets", "CWS_Atgeir_5");
            CWS_Atgeir_5.Name.English("Kuwata");
            CWS_Atgeir_5.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_5.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_5.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_5.Configurable = Configurability.Recipe;
            CWS_Atgeir_5.CraftAmount = 1;


            Item CWS_Atgeir_6 = new Item("cwsassets", "CWS_Atgeir_6");
            CWS_Atgeir_6.Name.English("Sadamune");
            CWS_Atgeir_6.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_6.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_6.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_6.Configurable = Configurability.Recipe;
            CWS_Atgeir_6.CraftAmount = 1;


            Item CWS_Atgeir_7 = new Item("cwsassets", "CWS_Atgeir_7");
            CWS_Atgeir_7.Name.English("Oki Sato");
            CWS_Atgeir_7.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_7.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_7.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_7.Configurable = Configurability.Recipe;
            CWS_Atgeir_7.CraftAmount = 1;


            Item CWS_Atgeir_8 = new Item("cwsassets", "CWS_Atgeir_8");
            CWS_Atgeir_8.Name.English("Tonbokiri");
            CWS_Atgeir_8.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_8.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_8.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_8.Configurable = Configurability.Recipe;
            CWS_Atgeir_8.CraftAmount = 1;


            Item CWS_Atgeir_9 = new Item("cwsassets", "CWS_Atgeir_9");
            CWS_Atgeir_9.Name.English("Benzaiten");
            CWS_Atgeir_9.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_9.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_9.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_9.Configurable = Configurability.Recipe;
            CWS_Atgeir_9.CraftAmount = 1;


            Item CWS_Atgeir_10 = new Item("cwsassets", "CWS_Atgeir_10");
            CWS_Atgeir_10.Name.English("Nethersong");
            CWS_Atgeir_10.Description.English("A steel forged atgeir. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Atgeir_10.Crafting.Add("CWS_Forge", 2);
            CWS_Atgeir_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Atgeir_10.RequiredItems.Add("RoundLog", 4);
            CWS_Atgeir_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Atgeir_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Atgeir_10.Configurable = Configurability.Recipe;
            CWS_Atgeir_10.CraftAmount = 1;


            #endregion
            #region axes

            Item CWS_Axe_1 = new Item("cwsassets", "CWS_Axe_1");
            CWS_Axe_1.Name.English("Hakarl");
            CWS_Axe_1.Description.English("A steel forged axe.");
            CWS_Axe_1.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_1.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_1.Configurable = Configurability.Recipe;
            CWS_Axe_1.CraftAmount = 1;


            Item CWS_Axe_2 = new Item("cwsassets", "CWS_Axe_2");
            CWS_Axe_2.Name.English("Svadilfari");
            CWS_Axe_2.Description.English("A steel forged axe.");
            CWS_Axe_2.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_2.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_2.Configurable = Configurability.Recipe;
            CWS_Axe_2.CraftAmount = 1;


            Item CWS_Axe_3 = new Item("cwsassets", "CWS_Axe_3");
            CWS_Axe_3.Name.English("Garmia");
            CWS_Axe_3.Description.English("A steel forged axe.");
            CWS_Axe_3.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_3.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_3.Configurable = Configurability.Recipe;
            CWS_Axe_3.CraftAmount = 1;


            Item CWS_Axe_4 = new Item("cwsassets", "CWS_Axe_4");
            CWS_Axe_4.Name.English("Bashok");
            CWS_Axe_4.Description.English("A steel forged axe.");
            CWS_Axe_4.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_4.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_4.Configurable = Configurability.Recipe;
            CWS_Axe_4.CraftAmount = 1;


            Item CWS_Axe_5 = new Item("cwsassets", "CWS_Axe_5");
            CWS_Axe_5.Name.English("Green Oskoreia");
            CWS_Axe_5.Description.English("A steel forged axe. This one can easily get rid of any arm or leg. Test it on necks too.");
            CWS_Axe_5.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_5.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_5.Configurable = Configurability.Recipe;
            CWS_Axe_5.CraftAmount = 1;


            Item CWS_Axe_6 = new Item("cwsassets", "CWS_Axe_6");
            CWS_Axe_6.Name.English("Landvaettir");
            CWS_Axe_6.Description.English("A steel forged axe.");
            CWS_Axe_6.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_6.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_6.Configurable = Configurability.Recipe;
            CWS_Axe_6.CraftAmount = 1;


            Item CWS_Axe_7 = new Item("cwsassets", "CWS_Axe_7");
            CWS_Axe_7.Name.English("Grafvitner");
            CWS_Axe_7.Description.English("A steel forged axe.");
            CWS_Axe_7.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_7.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_7.Configurable = Configurability.Recipe;
            CWS_Axe_7.CraftAmount = 1;


            Item CWS_Axe_8 = new Item("cwsassets", "CWS_Axe_8");
            CWS_Axe_8.Name.English("Half Moon");
            CWS_Axe_8.Description.English("A steel, very well refined, forged axe.");
            CWS_Axe_8.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_8.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_8.Configurable = Configurability.Recipe;
            CWS_Axe_8.CraftAmount = 1;


            Item CWS_Axe_9 = new Item("cwsassets", "CWS_Axe_9");
            CWS_Axe_9.Name.English("Cursed Seithr");
            CWS_Axe_9.Description.English("A steel forged axe.");
            CWS_Axe_9.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_9.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_9.Configurable = Configurability.Recipe;
            CWS_Axe_9.CraftAmount = 1;


            Item CWS_Axe_10 = new Item("cwsassets", "CWS_Axe_10");
            CWS_Axe_10.Name.English("Harmr");
            CWS_Axe_10.Description.English("A steel forged axe.");
            CWS_Axe_10.Crafting.Add("CWS_Forge", 2);
            CWS_Axe_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Axe_10.RequiredItems.Add("RoundLog", 4);
            CWS_Axe_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Axe_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Axe_10.Configurable = Configurability.Recipe;
            CWS_Axe_10.CraftAmount = 1;


            #endregion
            #region bows

            Item CWS_Bow_1 = new Item("cwsassets", "CWS_Bow_1");
            CWS_Bow_1.Name.English("Whisperwind");
            CWS_Bow_1.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_1.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_1.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_1.Configurable = Configurability.Recipe;
            CWS_Bow_1.CraftAmount = 1;


            Item CWS_Bow_2 = new Item("cwsassets", "CWS_Bow_2");
            CWS_Bow_2.Name.English("Seamsplitter");
            CWS_Bow_2.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_2.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_2.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_2.Configurable = Configurability.Recipe;
            CWS_Bow_2.CraftAmount = 1;


            Item CWS_Bow_3 = new Item("cwsassets", "CWS_Bow_3");
            CWS_Bow_3.Name.English("Eurytus");
            CWS_Bow_3.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_3.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_3.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_3.Configurable = Configurability.Recipe;
            CWS_Bow_3.CraftAmount = 1;


            Item CWS_Bow_4 = new Item("cwsassets", "CWS_Bow_4");
            CWS_Bow_4.Name.English("Kodandam");
            CWS_Bow_4.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_4.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_4.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_4.Configurable = Configurability.Recipe;
            CWS_Bow_4.CraftAmount = 1;


            Item CWS_Bow_5 = new Item("cwsassets", "CWS_Bow_5");
            CWS_Bow_5.Name.English("Vijaya");
            CWS_Bow_5.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_5.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_5.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_5.Configurable = Configurability.Recipe;
            CWS_Bow_5.CraftAmount = 1;


            Item CWS_Bow_6 = new Item("cwsassets", "CWS_Bow_6");
            CWS_Bow_6.Name.English("Belthronding");
            CWS_Bow_6.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_6.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_6.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_6.Configurable = Configurability.Recipe;
            CWS_Bow_6.CraftAmount = 1;


            Item CWS_Bow_7 = new Item("cwsassets", "CWS_Bow_7");
            CWS_Bow_7.Name.English("Houyi Wings");
            CWS_Bow_7.Description.English("A wing like bow. It is said you can hear crows as you shoot the arrow.");
            CWS_Bow_7.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_7.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_7.Configurable = Configurability.Recipe;
            CWS_Bow_7.CraftAmount = 1;


            Item CWS_Bow_8 = new Item("cwsassets", "CWS_Bow_8");
            CWS_Bow_8.Name.English("Brankan");
            CWS_Bow_8.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_8.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_8.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_8.Configurable = Configurability.Recipe;
            CWS_Bow_8.CraftAmount = 1;


            Item CWS_Bow_9 = new Item("cwsassets", "CWS_Bow_9");
            CWS_Bow_9.Name.English("Używany");
            CWS_Bow_9.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_9.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_9.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_9.Configurable = Configurability.Recipe;
            CWS_Bow_9.CraftAmount = 1;


            Item CWS_Bow_10 = new Item("cwsassets", "CWS_Bow_10");
            CWS_Bow_10.Name.English("Hamstring");
            CWS_Bow_10.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_10.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_10.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_10.Configurable = Configurability.Recipe;
            CWS_Bow_10.CraftAmount = 1;


            Item CWS_Bow_11 = new Item("cwsassets", "CWS_Bow_11");
            CWS_Bow_11.Name.English("Huntress");
            CWS_Bow_11.Description.English("Rooty tooty, point and shooty!.");
            CWS_Bow_11.Crafting.Add("CWS_Forge", 2);
            CWS_Bow_11.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Bow_11.RequiredItems.Add("RoundLog", 4);
            CWS_Bow_11.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Bow_11.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Bow_11.Configurable = Configurability.Recipe;
            CWS_Bow_11.CraftAmount = 1;


            #endregion
            #region daggers

            Item CWS_Dagger_1 = new Item("cwsassets", "CWS_Dagger_1");
            CWS_Dagger_1.Name.English("Rantia");
            CWS_Dagger_1.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_1.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_1.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_1.Configurable = Configurability.Recipe;
            CWS_Dagger_1.CraftAmount = 1;


            Item CWS_Dagger_2 = new Item("cwsassets", "CWS_Dagger_2");
            CWS_Dagger_2.Name.English("Guron");
            CWS_Dagger_2.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_2.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_2.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_2.Configurable = Configurability.Recipe;
            CWS_Dagger_2.CraftAmount = 1;


            Item CWS_Dagger_3 = new Item("cwsassets", "CWS_Dagger_3");
            CWS_Dagger_3.Name.English("Gilian");
            CWS_Dagger_3.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_3.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_3.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_3.Configurable = Configurability.Recipe;
            CWS_Dagger_3.CraftAmount = 1;


            Item CWS_Dagger_4 = new Item("cwsassets", "CWS_Dagger_4");
            CWS_Dagger_4.Name.English("Tiffany");
            CWS_Dagger_4.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_4.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_4.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_4.Configurable = Configurability.Recipe;
            CWS_Dagger_4.CraftAmount = 1;


            Item CWS_Dagger_5 = new Item("cwsassets", "CWS_Dagger_5");
            CWS_Dagger_5.Name.English("Slayton");
            CWS_Dagger_5.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_5.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_5.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_5.Configurable = Configurability.Recipe;
            CWS_Dagger_5.CraftAmount = 1;


            Item CWS_Dagger_6 = new Item("cwsassets", "CWS_Dagger_6");
            CWS_Dagger_6.Name.English("Marcellis");
            CWS_Dagger_6.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_6.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_6.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_6.Configurable = Configurability.Recipe;
            CWS_Dagger_6.CraftAmount = 1;


            Item CWS_Dagger_7 = new Item("cwsassets", "CWS_Dagger_7");
            CWS_Dagger_7.Name.English("Hilord");
            CWS_Dagger_7.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_7.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_7.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_7.Configurable = Configurability.Recipe;
            CWS_Dagger_7.CraftAmount = 1;


            Item CWS_Dagger_8 = new Item("cwsassets", "CWS_Dagger_8");
            CWS_Dagger_8.Name.English("Eveanne");
            CWS_Dagger_8.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_8.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_8.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_8.Configurable = Configurability.Recipe;
            CWS_Dagger_8.CraftAmount = 1;


            Item CWS_Dagger_9 = new Item("cwsassets", "CWS_Dagger_9");
            CWS_Dagger_9.Name.English("Craymen");
            CWS_Dagger_9.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_9.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_9.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_9.Configurable = Configurability.Recipe;
            CWS_Dagger_9.CraftAmount = 1;

            
            Item CWS_Dagger_10 = new Item("cwsassets", "CWS_Dagger_10");
            CWS_Dagger_10.Name.English("Narzan");
            CWS_Dagger_10.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_10.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_10.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_10.Configurable = Configurability.Recipe;
            CWS_Dagger_10.CraftAmount = 1;


            Item CWS_Dagger_11 = new Item("cwsassets", "CWS_Dagger_11");
            CWS_Dagger_11.Name.English("Atolm");
            CWS_Dagger_11.Description.English("A steel forged dagger. Swift, silent, deadly.");
            CWS_Dagger_11.Crafting.Add("CWS_Forge", 2);
            CWS_Dagger_11.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Dagger_11.RequiredItems.Add("RoundLog", 4);
            CWS_Dagger_11.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Dagger_11.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Dagger_11.Configurable = Configurability.Recipe;
            CWS_Dagger_11.CraftAmount = 1;


            #endregion
            #region maces

            Item CWS_Mace_1 = new Item("cwsassets", "CWS_Mace_1");
            CWS_Mace_1.Name.English("Berinon");
            CWS_Mace_1.Description.English("A steel forged mace. You can hear skulls crushing just by holding this.");
            CWS_Mace_1.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_1.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_1.Configurable = Configurability.Recipe;
            CWS_Mace_1.CraftAmount = 1;


            Item CWS_Mace_2 = new Item("cwsassets", "CWS_Mace_2");
            CWS_Mace_2.Name.English("Cunningham");
            CWS_Mace_2.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_2.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_2.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_2.Configurable = Configurability.Recipe;
            CWS_Mace_2.CraftAmount = 1;


            Item CWS_Mace_3 = new Item("cwsassets", "CWS_Mace_3");
            CWS_Mace_3.Name.English("Ellyn");
            CWS_Mace_3.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_3.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_3.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_3.Configurable = Configurability.Recipe;
            CWS_Mace_3.CraftAmount = 1;


            Item CWS_Mace_4 = new Item("cwsassets", "CWS_Mace_4");
            CWS_Mace_4.Name.English("Lamorak");
            CWS_Mace_4.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_4.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_4.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_4.Configurable = Configurability.Recipe;
            CWS_Mace_4.CraftAmount = 1;


            Item CWS_Mace_5 = new Item("cwsassets", "CWS_Mace_5");
            CWS_Mace_5.Name.English("Orselen");
            CWS_Mace_5.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_5.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_5.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_5.Configurable = Configurability.Recipe;
            CWS_Mace_5.CraftAmount = 1;


            Item CWS_Mace_6 = new Item("cwsassets", "CWS_Mace_6");
            CWS_Mace_6.Name.English("Sagamore");
            CWS_Mace_6.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_6.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_6.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_6.Configurable = Configurability.Recipe;
            CWS_Mace_6.CraftAmount = 1;


            Item CWS_Mace_7 = new Item("cwsassets", "CWS_Mace_7");
            CWS_Mace_7.Name.English("Gustave");
            CWS_Mace_7.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_7.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_7.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_7.Configurable = Configurability.Recipe;
            CWS_Mace_7.CraftAmount = 1;


            Item CWS_Mace_8 = new Item("cwsassets", "CWS_Mace_8");
            CWS_Mace_8.Name.English("Duellicosa");
            CWS_Mace_8.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_8.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_8.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_8.Configurable = Configurability.Recipe;
            CWS_Mace_8.CraftAmount = 1;


            Item CWS_Mace_9 = new Item("cwsassets", "CWS_Mace_9");
            CWS_Mace_9.Name.English("Meribia");
            CWS_Mace_9.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_9.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_9.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_9.Configurable = Configurability.Recipe;
            CWS_Mace_9.CraftAmount = 1;


            Item CWS_Mace_10 = new Item("cwsassets", "CWS_Mace_10");
            CWS_Mace_10.Name.English("Svartr Sol");
            CWS_Mace_10.Description.English("A steel forged mace. You can hear skulls crushing just by holding this..");
            CWS_Mace_10.Crafting.Add("CWS_Forge", 2);
            CWS_Mace_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Mace_10.RequiredItems.Add("RoundLog", 4);
            CWS_Mace_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Mace_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Mace_10.Configurable = Configurability.Recipe;
            CWS_Mace_10.CraftAmount = 1;


            #endregion
            #region shields

            Item CWS_Buckler_Shield_1 = new Item("cwsassets", "CWS_Buckler_Shield_1");
            CWS_Buckler_Shield_1.Name.English("Apostate");
            CWS_Buckler_Shield_1.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Buckler_Shield_1.Crafting.Add("CWS_Forge", 2);
            CWS_Buckler_Shield_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Buckler_Shield_1.RequiredItems.Add("RoundLog", 4);
            CWS_Buckler_Shield_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Buckler_Shield_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Buckler_Shield_1.Configurable = Configurability.Recipe;
            CWS_Buckler_Shield_1.CraftAmount = 1;


            Item CWS_Buckler_Shield_2 = new Item("cwsassets", "CWS_Buckler_Shield_2");
            CWS_Buckler_Shield_2.Name.English("Decrepit");
            CWS_Buckler_Shield_2.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Buckler_Shield_2.Crafting.Add("CWS_Forge", 2);
            CWS_Buckler_Shield_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Buckler_Shield_2.RequiredItems.Add("RoundLog", 4);
            CWS_Buckler_Shield_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Buckler_Shield_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Buckler_Shield_2.Configurable = Configurability.Recipe;
            CWS_Buckler_Shield_2.CraftAmount = 1;


            Item CWS_Round_Shield_1 = new Item("cwsassets", "CWS_Round_Shield_1");
            CWS_Round_Shield_1.Name.English("Perfuga");
            CWS_Round_Shield_1.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Round_Shield_1.Crafting.Add("CWS_Forge", 2);
            CWS_Round_Shield_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Round_Shield_1.RequiredItems.Add("RoundLog", 4);
            CWS_Round_Shield_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Round_Shield_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Round_Shield_1.Configurable = Configurability.Recipe;
            CWS_Round_Shield_1.CraftAmount = 1;


            Item CWS_Round_Shield_2 = new Item("cwsassets", "CWS_Round_Shield_2");
            CWS_Round_Shield_2.Name.English("Infidelis");
            CWS_Round_Shield_2.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Round_Shield_2.Crafting.Add("CWS_Forge", 2);
            CWS_Round_Shield_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Round_Shield_2.RequiredItems.Add("RoundLog", 4);
            CWS_Round_Shield_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Round_Shield_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Round_Shield_2.Configurable = Configurability.Recipe;
            CWS_Round_Shield_2.CraftAmount = 1;


            Item CWS_Round_Shield_3 = new Item("cwsassets", "CWS_Round_Shield_3");
            CWS_Round_Shield_3.Name.English("Miscreant");
            CWS_Round_Shield_3.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Round_Shield_3.Crafting.Add("CWS_Forge", 2);
            CWS_Round_Shield_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Round_Shield_3.RequiredItems.Add("RoundLog", 4);
            CWS_Round_Shield_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Round_Shield_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Round_Shield_3.Configurable = Configurability.Recipe;
            CWS_Round_Shield_3.CraftAmount = 1;


            Item CWS_Round_Shield_4 = new Item("cwsassets", "CWS_Round_Shield_4");
            CWS_Round_Shield_4.Name.English("Muirgheal");
            CWS_Round_Shield_4.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Round_Shield_4.Crafting.Add("CWS_Forge", 2);
            CWS_Round_Shield_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Round_Shield_4.RequiredItems.Add("RoundLog", 4);
            CWS_Round_Shield_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Round_Shield_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Round_Shield_4.Configurable = Configurability.Recipe;
            CWS_Round_Shield_4.CraftAmount = 1;


            Item CWS_Round_Shield_5 = new Item("cwsassets", "CWS_Round_Shield_5");
            CWS_Round_Shield_5.Name.English("Lacrimosa");
            CWS_Round_Shield_5.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Round_Shield_5.Crafting.Add("CWS_Forge", 2);
            CWS_Round_Shield_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Round_Shield_5.RequiredItems.Add("RoundLog", 4);
            CWS_Round_Shield_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Round_Shield_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Round_Shield_5.Configurable = Configurability.Recipe;
            CWS_Round_Shield_5.CraftAmount = 1;


            Item CWS_Round_Shield_6 = new Item("cwsassets", "CWS_Round_Shield_6");
            CWS_Round_Shield_6.Name.English("Radulfr");
            CWS_Round_Shield_6.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Round_Shield_6.Crafting.Add("CWS_Forge", 2);
            CWS_Round_Shield_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Round_Shield_6.RequiredItems.Add("RoundLog", 4);
            CWS_Round_Shield_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Round_Shield_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Round_Shield_6.Configurable = Configurability.Recipe;
            CWS_Round_Shield_6.CraftAmount = 1;


            Item CWS_Round_Shield_7 = new Item("cwsassets", "CWS_Round_Shield_7");
            CWS_Round_Shield_7.Name.English("Torhild");
            CWS_Round_Shield_7.Description.English("A steel forged round shield. It is not so heavy, you can move freely.");
            CWS_Round_Shield_7.Crafting.Add("CWS_Forge", 2);
            CWS_Round_Shield_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Round_Shield_7.RequiredItems.Add("RoundLog", 4);
            CWS_Round_Shield_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Round_Shield_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Round_Shield_7.Configurable = Configurability.Recipe;
            CWS_Round_Shield_7.CraftAmount = 1;


            Item CWS_Heater_Shield_1 = new Item("cwsassets", "CWS_Heater_Shield_1");
            CWS_Heater_Shield_1.Name.English("Birger");
            CWS_Heater_Shield_1.Description.English("A steel forged heater shield. Good mobility and defense.");
            CWS_Heater_Shield_1.Crafting.Add("CWS_Forge", 2);
            CWS_Heater_Shield_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Heater_Shield_1.RequiredItems.Add("RoundLog", 4);
            CWS_Heater_Shield_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Heater_Shield_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Heater_Shield_1.Configurable = Configurability.Recipe;
            CWS_Heater_Shield_1.CraftAmount = 1;


            Item CWS_Heater_Shield_2 = new Item("cwsassets", "CWS_Heater_Shield_2");
            CWS_Heater_Shield_2.Name.English("Siggebane");
            CWS_Heater_Shield_2.Description.English("A steel forged heater shield. Good mobility and defense.");
            CWS_Heater_Shield_2.Crafting.Add("CWS_Forge", 2);
            CWS_Heater_Shield_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Heater_Shield_2.RequiredItems.Add("RoundLog", 4);
            CWS_Heater_Shield_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Heater_Shield_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Heater_Shield_2.Configurable = Configurability.Recipe;
            CWS_Heater_Shield_2.CraftAmount = 1;


            Item CWS_Heater_Shield_3 = new Item("cwsassets", "CWS_Heater_Shield_3");
            CWS_Heater_Shield_3.Name.English("Glebs Eydis");
            CWS_Heater_Shield_3.Description.English("A steel forged heater shield. Good mobility and defense.");
            CWS_Heater_Shield_3.Crafting.Add("CWS_Forge", 2);
            CWS_Heater_Shield_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Heater_Shield_3.RequiredItems.Add("RoundLog", 4);
            CWS_Heater_Shield_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Heater_Shield_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Heater_Shield_3.Configurable = Configurability.Recipe;
            CWS_Heater_Shield_3.CraftAmount = 1;


            Item CWS_Heater_Shield_4 = new Item("cwsassets", "CWS_Heater_Shield_4");
            CWS_Heater_Shield_4.Name.English("Runar Ingvild");
            CWS_Heater_Shield_4.Description.English("A steel forged heater shield. Good mobility and defense.");
            CWS_Heater_Shield_4.Crafting.Add("CWS_Forge", 2);
            CWS_Heater_Shield_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Heater_Shield_4.RequiredItems.Add("RoundLog", 4);
            CWS_Heater_Shield_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Heater_Shield_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Heater_Shield_4.Configurable = Configurability.Recipe;
            CWS_Heater_Shield_4.CraftAmount = 1;


            Item CWS_Heater_Shield_5 = new Item("cwsassets", "CWS_Heater_Shield_5");
            CWS_Heater_Shield_5.Name.English("Vigdis Ingegerd");
            CWS_Heater_Shield_5.Description.English("A steel forged heater shield. Good mobility and defense.");
            CWS_Heater_Shield_5.Crafting.Add("CWS_Forge", 2);
            CWS_Heater_Shield_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Heater_Shield_5.RequiredItems.Add("RoundLog", 4);
            CWS_Heater_Shield_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Heater_Shield_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Heater_Shield_5.Configurable = Configurability.Recipe;
            CWS_Heater_Shield_5.CraftAmount = 1;


            Item CWS_Tower_Shield_1 = new Item("cwsassets", "CWS_Tower_Shield_1");
            CWS_Tower_Shield_1.Name.English("Hel Hilda");
            CWS_Tower_Shield_1.Description.English("A steel forged tower shield. It is heavy, but you feel like being behind a wall.");
            CWS_Tower_Shield_1.Crafting.Add("CWS_Forge", 2);
            CWS_Tower_Shield_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Tower_Shield_1.RequiredItems.Add("RoundLog", 4);
            CWS_Tower_Shield_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Tower_Shield_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Tower_Shield_1.Configurable = Configurability.Recipe;
            CWS_Tower_Shield_1.CraftAmount = 1;


            Item CWS_Tower_Shield_2 = new Item("cwsassets", "CWS_Tower_Shield_2");
            CWS_Tower_Shield_2.Name.English("Melpomene");
            CWS_Tower_Shield_2.Description.English("A steel forged tower shield. It is heavy, but you feel like being behind a wall.");
            CWS_Tower_Shield_2.Crafting.Add("CWS_Forge", 2);
            CWS_Tower_Shield_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Tower_Shield_2.RequiredItems.Add("RoundLog", 4);
            CWS_Tower_Shield_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Tower_Shield_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Tower_Shield_2.Configurable = Configurability.Recipe;
            CWS_Tower_Shield_2.CraftAmount = 1;


            Item CWS_Tower_Shield_3 = new Item("cwsassets", "CWS_Tower_Shield_3");
            CWS_Tower_Shield_3.Name.English("Dies Irae");
            CWS_Tower_Shield_3.Description.English("A steel forged tower shield. It is heavy, but you feel like being behind a wall.");
            CWS_Tower_Shield_3.Crafting.Add("CWS_Forge", 2);
            CWS_Tower_Shield_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Tower_Shield_3.RequiredItems.Add("RoundLog", 4);
            CWS_Tower_Shield_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Tower_Shield_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Tower_Shield_3.Configurable = Configurability.Recipe;
            CWS_Tower_Shield_3.CraftAmount = 1;


            Item CWS_Tower_Shield_4 = new Item("cwsassets", "CWS_Tower_Shield_4");
            CWS_Tower_Shield_4.Name.English("Rasalhague");
            CWS_Tower_Shield_4.Description.English("A steel forged tower shield. It is heavy, but you feel like being behind a wall.");
            CWS_Tower_Shield_4.Crafting.Add("CWS_Forge", 2);
            CWS_Tower_Shield_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Tower_Shield_4.RequiredItems.Add("RoundLog", 4);
            CWS_Tower_Shield_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Tower_Shield_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Tower_Shield_4.Configurable = Configurability.Recipe;
            CWS_Tower_Shield_4.CraftAmount = 1;


            Item CWS_Tower_Shield_5 = new Item("cwsassets", "CWS_Tower_Shield_5");
            CWS_Tower_Shield_5.Name.English("Bornite");
            CWS_Tower_Shield_5.Description.English("A steel forged tower shield. It is heavy, but you feel like being behind a wall.");
            CWS_Tower_Shield_5.Crafting.Add("CWS_Forge", 2);
            CWS_Tower_Shield_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Tower_Shield_5.RequiredItems.Add("RoundLog", 4);
            CWS_Tower_Shield_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Tower_Shield_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Tower_Shield_5.Configurable = Configurability.Recipe;
            CWS_Tower_Shield_5.CraftAmount = 1;


            #endregion
            #region spears

            Item CWS_Spear_1 = new Item("cwsassets", "CWS_Spear_1");
            CWS_Spear_1.Name.English("Belloth");
            CWS_Spear_1.Description.English("A swift steel forged spear. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Spear_1.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_1.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_1.Configurable = Configurability.Recipe;
            CWS_Spear_1.CraftAmount = 1;

            GameObject CWS_Spear_1_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_1_Projectile");

            Item CWS_Spear_2 = new Item("cwsassets", "CWS_Spear_2");
            CWS_Spear_2.Name.English("Crowley");
            CWS_Spear_2.Description.English("A swift steel forged spear. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Spear_2.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_2.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_2.Configurable = Configurability.Recipe;
            CWS_Spear_2.CraftAmount = 1;

            GameObject CWS_Spear_2_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_2_Projectile");

            Item CWS_Spear_3 = new Item("cwsassets", "CWS_Spear_3");
            CWS_Spear_3.Name.English("Edgars");
            CWS_Spear_3.Description.English("A swift steel forged spear. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Spear_3.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_3.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_3.Configurable = Configurability.Recipe;
            CWS_Spear_3.CraftAmount = 1;

            GameObject CWS_Spear_3_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_3_Projectile");

            Item CWS_Spear_4 = new Item("cwsassets", "CWS_Spear_4");
            CWS_Spear_4.Name.English("Frazar");
            CWS_Spear_4.Description.English("A swift steel forged spear. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Spear_4.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_4.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_4.Configurable = Configurability.Recipe;
            CWS_Spear_4.CraftAmount = 1;

            GameObject CWS_Spear_4_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_4_Projectile");

            Item CWS_Spear_5 = new Item("cwsassets", "CWS_Spear_5");
            CWS_Spear_5.Name.English("Gibbons");
            CWS_Spear_5.Description.English("A swift steel forged spear. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Spear_5.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_5.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_5.Configurable = Configurability.Recipe;
            CWS_Spear_5.CraftAmount = 1;

            GameObject CWS_Spear_5_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_5_Projectile");

            Item CWS_Spear_6 = new Item("cwsassets", "CWS_Spear_6");
            CWS_Spear_6.Name.English("Rivalen");
            CWS_Spear_6.Description.English("A swift steel forged spear. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Spear_6.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_6.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_6.Configurable = Configurability.Recipe;
            CWS_Spear_6.CraftAmount = 1;

            GameObject CWS_Spear_6_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_6_Projectile");

            Item CWS_Spear_7 = new Item("cwsassets", "CWS_Spear_7");
            CWS_Spear_7.Name.English("Tormod");
            CWS_Spear_7.Description.English("A swift steel forged spear. Keep distance from enemies, you don't like blood on your armor.");
            CWS_Spear_7.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_7.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_7.Configurable = Configurability.Recipe;
            CWS_Spear_7.CraftAmount = 1;

            GameObject CWS_Spear_7_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_7_Projectile");

            Item CWS_Spear_8 = new Item("cwsassets", "CWS_Spear_8");
            CWS_Spear_8.Name.English("Audgunn");
            CWS_Spear_8.Description.English("A swift steel forged spear. Forged of for war.");
            CWS_Spear_8.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_8.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_8.Configurable = Configurability.Recipe;
            CWS_Spear_8.CraftAmount = 1;

            GameObject CWS_Spear_8_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_8_Projectile");

            Item CWS_Spear_9 = new Item("cwsassets", "CWS_Spear_9");
            CWS_Spear_9.Name.English("Geir Liv");
            CWS_Spear_9.Description.English("A swift steel forged spear. Protector of life.");
            CWS_Spear_9.Crafting.Add("CWS_Forge", 2);
            CWS_Spear_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Spear_9.RequiredItems.Add("RoundLog", 4);
            CWS_Spear_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Spear_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Spear_9.Configurable = Configurability.Recipe;
            CWS_Spear_9.CraftAmount = 1;

            GameObject CWS_Spear_9_Projectile = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Spear_9_Projectile");

            #endregion
            #region swords

            Item CWS_Sword_1 = new Item("cwsassets", "CWS_Sword_1");
            CWS_Sword_1.Name.English("Munifex");
            CWS_Sword_1.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_1.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_1.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_1.Configurable = Configurability.Recipe;
            CWS_Sword_1.CraftAmount = 1;


            Item CWS_Sword_2 = new Item("cwsassets", "CWS_Sword_2");
            CWS_Sword_2.Name.English("Trusty");
            CWS_Sword_2.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_2.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_2.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_2.Configurable = Configurability.Recipe;
            CWS_Sword_2.CraftAmount = 1;


            Item CWS_Sword_3 = new Item("cwsassets", "CWS_Sword_3");
            CWS_Sword_3.Name.English("Veteran");
            CWS_Sword_3.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_3.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_3.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_3.Configurable = Configurability.Recipe;
            CWS_Sword_3.CraftAmount = 1;


            Item CWS_Sword_4 = new Item("cwsassets", "CWS_Sword_4");
            CWS_Sword_4.Name.English("Decanus");
            CWS_Sword_4.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_4.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_4.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_4.Configurable = Configurability.Recipe;
            CWS_Sword_4.CraftAmount = 1;


            Item CWS_Sword_5 = new Item("cwsassets", "CWS_Sword_5");
            CWS_Sword_5.Name.English("Optio");
            CWS_Sword_5.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_5.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_5.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_5.Configurable = Configurability.Recipe;
            CWS_Sword_5.CraftAmount = 1;


            Item CWS_Sword_6 = new Item("cwsassets", "CWS_Sword_6");
            CWS_Sword_6.Name.English("Auratum");
            CWS_Sword_6.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_6.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_6.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_6.Configurable = Configurability.Recipe;
            CWS_Sword_6.CraftAmount = 1;


            Item CWS_Sword_7 = new Item("cwsassets", "CWS_Sword_7");
            CWS_Sword_7.Name.English("Pax Regis");
            CWS_Sword_7.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_7.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_7.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_7.Configurable = Configurability.Recipe;
            CWS_Sword_7.CraftAmount = 1;


            Item CWS_Sword_8 = new Item("cwsassets", "CWS_Sword_8");
            CWS_Sword_8.Name.English("Cranium");
            CWS_Sword_8.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_8.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_8.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_8.Configurable = Configurability.Recipe;
            CWS_Sword_8.CraftAmount = 1;


            Item CWS_Sword_9 = new Item("cwsassets", "CWS_Sword_9");
            CWS_Sword_9.Name.English("Praetor");
            CWS_Sword_9.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_9.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_9.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_9.Configurable = Configurability.Recipe;
            CWS_Sword_9.CraftAmount = 1;

            
            Item CWS_Sword_10 = new Item("cwsassets", "CWS_Sword_10");
            CWS_Sword_10.Name.English("Auceps");
            CWS_Sword_10.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_10.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_10.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_10.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_10.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_10.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_10.Configurable = Configurability.Recipe;
            CWS_Sword_10.CraftAmount = 1;


            Item CWS_Sword_11 = new Item("cwsassets", "CWS_Sword_11");
            CWS_Sword_11.Name.English("Veritas");
            CWS_Sword_11.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_11.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_11.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_11.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_11.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_11.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_11.Configurable = Configurability.Recipe;
            CWS_Sword_11.CraftAmount = 1;


            Item CWS_Sword_12 = new Item("cwsassets", "CWS_Sword_12");
            CWS_Sword_12.Name.English("Widow's Cry");
            CWS_Sword_12.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_12.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_12.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_12.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_12.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_12.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_12.Configurable = Configurability.Recipe;
            CWS_Sword_12.CraftAmount = 1;


            Item CWS_Sword_13 = new Item("cwsassets", "CWS_Sword_13");
            CWS_Sword_13.Name.English("Cauda Serpentis");
            CWS_Sword_13.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_13.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_13.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_13.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_13.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_13.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_13.Configurable = Configurability.Recipe;
            CWS_Sword_13.CraftAmount = 1;


            Item CWS_Sword_14 = new Item("cwsassets", "CWS_Sword_14");
            CWS_Sword_14.Name.English("Immanis");
            CWS_Sword_14.Description.English("A steel forged sword. The straight line between life and death runs along the edge of this blade.");
            CWS_Sword_14.Crafting.Add("CWS_Forge", 2);
            CWS_Sword_14.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Sword_14.RequiredItems.Add("RoundLog", 4);
            CWS_Sword_14.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Sword_14.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Sword_14.Configurable = Configurability.Recipe;
            CWS_Sword_14.CraftAmount = 1;


            #endregion
            #region crossbows

            Item CWS_Crossbow_1 = new Item("cwsassets", "CWS_Crossbow_1");
            CWS_Crossbow_1.Name.English("Repeater");
            CWS_Crossbow_1.Description.English("As slow as deadly. This one can easily separate a branch from a tree, you know.");
            CWS_Crossbow_1.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_1.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_1.Configurable = Configurability.Recipe;
            CWS_Crossbow_1.CraftAmount = 1;


            Item CWS_Crossbow_2 = new Item("cwsassets", "CWS_Crossbow_2");
            CWS_Crossbow_2.Name.English("Deadeye");
            CWS_Crossbow_2.Description.English("As slow as deadly. This one can easily separate a branch from a tree, you know.");
            CWS_Crossbow_2.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_2.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_2.Configurable = Configurability.Recipe;
            CWS_Crossbow_2.CraftAmount = 1;


            Item CWS_Crossbow_3 = new Item("cwsassets", "CWS_Crossbow_3");
            CWS_Crossbow_3.Name.English("Monsoon");
            CWS_Crossbow_3.Description.English("As slow as deadly. This one can easily separate a branch from a tree, you know.");
            CWS_Crossbow_3.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_3.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_3.Configurable = Configurability.Recipe;
            CWS_Crossbow_3.CraftAmount = 1;


            Item CWS_Crossbow_4 = new Item("cwsassets", "CWS_Crossbow_4");
            CWS_Crossbow_4.Name.English("Netherstrand");
            CWS_Crossbow_4.Description.English("As slow as deadly. This one can easily separate a branch from a tree, you know.");
            CWS_Crossbow_4.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_4.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_4.Configurable = Configurability.Recipe;
            CWS_Crossbow_4.CraftAmount = 1;


            Item CWS_Crossbow_5 = new Item("cwsassets", "CWS_Crossbow_5");
            CWS_Crossbow_5.Name.English("Shrike");
            CWS_Crossbow_5.Description.English("As slow as deadly. This one can easily separate a branch from a tree, you know.");
            CWS_Crossbow_5.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_5.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_5.Configurable = Configurability.Recipe;
            CWS_Crossbow_5.CraftAmount = 1;


            Item CWS_Crossbow_6 = new Item("cwsassets", "CWS_Crossbow_6");
            CWS_Crossbow_6.Name.English("Drawback");
            CWS_Crossbow_6.Description.English("As slow as deadly. This one can easily separate a branch from a tree, you know.");
            CWS_Crossbow_6.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_6.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_6.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_6.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_6.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_6.Configurable = Configurability.Recipe;
            CWS_Crossbow_6.CraftAmount = 1;


            Item CWS_Crossbow_7 = new Item("cwsassets", "CWS_Crossbow_7");
            CWS_Crossbow_7.Name.English("Pincer");
            CWS_Crossbow_7.Description.English("As slow as deadly. This one can easily separate a branch from a tree, you know.");
            CWS_Crossbow_7.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_7.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_7.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_7.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_7.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_7.Configurable = Configurability.Recipe;
            CWS_Crossbow_7.CraftAmount = 1;


            Item CWS_Crossbow_8 = new Item("cwsassets", "CWS_Crossbow_8");
            CWS_Crossbow_8.Name.English("Hornet");
            CWS_Crossbow_8.Description.English("As slow as deadly. This one can easily separate a branch from a tree, you know.");
            CWS_Crossbow_8.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_8.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_8.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_8.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_8.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_8.Configurable = Configurability.Recipe;
            CWS_Crossbow_8.CraftAmount = 1;


            Item CWS_Crossbow_9 = new Item("cwsassets", "CWS_Crossbow_9");
            CWS_Crossbow_9.Name.English("Steinn Orka");
            CWS_Crossbow_9.Description.English("A shot in the dark.");
            CWS_Crossbow_9.Crafting.Add("CWS_Forge", 2);
            CWS_Crossbow_9.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Crossbow_9.RequiredItems.Add("RoundLog", 4);
            CWS_Crossbow_9.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Crossbow_9.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Crossbow_9.Configurable = Configurability.Recipe;
            CWS_Crossbow_9.CraftAmount = 1;


            #endregion
            #region arrows

            Item CWS_Arrow_1 = new Item("cwsassets", "CWS_Arrow_1");
            CWS_Arrow_1.Name.English("Barbed Crossbow Bolt");
            CWS_Arrow_1.Description.English("a stack of barbed ammo for crossbows");
            CWS_Arrow_1.Crafting.Add("CWS_Forge", 2);
            CWS_Arrow_1.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Arrow_1.RequiredItems.Add("RoundLog", 4);
            CWS_Arrow_1.RequiredItems.Add("ElderBark", 1);
            CWS_Arrow_1.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Arrow_1.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Arrow_1.Configurable = Configurability.Recipe;
            CWS_Arrow_1.CraftAmount = 20;
            
            GameObject CWS_Arrow_Pro_1 = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Arrow_Pro_1");

            Item CWS_Arrow_2 = new Item("cwsassets", "CWS_Arrow_2");
            CWS_Arrow_2.Name.English("Elemental Crossbow Bolt");
            CWS_Arrow_2.Description.English("a stack of lightning ammo for crossbows");
            CWS_Arrow_2.Crafting.Add("CWS_Forge", 2);
            CWS_Arrow_2.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Arrow_2.RequiredItems.Add("RoundLog", 4);
            CWS_Arrow_2.RequiredItems.Add("Obsidian", 1);
            CWS_Arrow_2.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Arrow_2.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Arrow_2.Configurable = Configurability.Recipe;
            CWS_Arrow_2.CraftAmount = 20;
            
            GameObject CWS_Arrow_Pro_2 = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Arrow_Pro_2");

            Item CWS_Arrow_3 = new Item("cwsassets", "CWS_Arrow_3");
            CWS_Arrow_3.Name.English("Burning Crossbow Bolt");
            CWS_Arrow_3.Description.English("a stack of burning ammo for crossbows");
            CWS_Arrow_3.Crafting.Add("CWS_Forge", 2);
            CWS_Arrow_3.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Arrow_3.RequiredItems.Add("RoundLog", 4);
            CWS_Arrow_3.RequiredItems.Add("Entrails", 1);
            CWS_Arrow_3.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Arrow_3.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Arrow_3.Configurable = Configurability.Recipe;
            CWS_Arrow_3.CraftAmount = 20;
            
            GameObject CWS_Arrow_Pro_3 = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Arrow_Pro_3");

            Item CWS_Arrow_4 = new Item("cwsassets", "CWS_Arrow_4");
            CWS_Arrow_4.Name.English("Toxic Crossbow Bolt");
            CWS_Arrow_4.Description.English("a stack of poison ammo for crossbows");
            CWS_Arrow_4.Crafting.Add("CWS_Forge", 2);
            CWS_Arrow_4.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Arrow_4.RequiredItems.Add("RoundLog", 4);
            CWS_Arrow_4.RequiredItems.Add("Ooze", 1);
            CWS_Arrow_4.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Arrow_4.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Arrow_4.Configurable = Configurability.Recipe;
            CWS_Arrow_4.CraftAmount = 20;
            
            GameObject CWS_Arrow_Pro_4 = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Arrow_Pro_4");
            Item CWS_Arrow_5 = new Item("cwsassets", "CWS_Arrow_5");           
            CWS_Arrow_5.Name.English("Frozen Crossbow Bolt");
            CWS_Arrow_5.Description.English("a stack of frozen ammo for crossbows");
            CWS_Arrow_5.Crafting.Add("CWS_Forge", 2);
            CWS_Arrow_5.RequiredItems.Add("CWS_Hot_Steel_Finished", 1);
            CWS_Arrow_5.RequiredItems.Add("RoundLog", 4);
            CWS_Arrow_5.RequiredItems.Add("FreezeGland", 1);
            CWS_Arrow_5.RequiredUpgradeItems.Add("CWS_Hot_Steel_Finished", 2);
            CWS_Arrow_5.RequiredUpgradeItems.Add("RoundLog", 2);
            CWS_Arrow_5.Configurable = Configurability.Recipe;
            CWS_Arrow_5.CraftAmount = 20;
            
            GameObject CWS_Arrow_Pro_5 = ItemManager.PrefabManager.RegisterPrefab("cwsassets", "CWS_Arrow_Pro_5");

            #endregion


        }
    }
}