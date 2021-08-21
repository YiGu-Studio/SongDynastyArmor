using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SGAddItem
{
    public class SubModule : MBSubModuleBase
    {
        
        public override bool DoLoading(Game game)
        {

            Init();

            try
            {
                if (MobileParty.MainParty == null || MobileParty.MainParty.ItemRoster == null || Hero.MainHero == null)
                {
                    InformationManager.DisplayMessage(new InformationMessage("未获取玩家信息，请存档后重新读取。", new Color(.65f, .35f, 0)));
                    return true;
                }

                var items = new List<ItemObject>();

                for (int i = 0; i < MobileParty.MainParty.ItemRoster.Count; i++)
                {
                    items.Add(MobileParty.MainParty.ItemRoster.GetItemAtIndex(i));
                }

                for (int i = 0; i < 4; i++)
                {
                    var equip = Hero.MainHero.CharacterObject.Equipment[i];
                    items.Add(equip.Item);
                }

                LPLog.Log("玩家背包道具数量 " + MobileParty.MainParty.ItemRoster.Count.ToString());

                foreach (ItemObject item2 in Items.All)
                {
                    if (item2 != null && !string.IsNullOrEmpty(item2.StringId) && item2.StringId.ToLower().Contains(NowModNameId) && !items.Contains(item2))
                    {
                        MobileParty.MainParty.ItemRoster.AddToCounts(new EquipmentElement(item2), 1);
                        LPLog.Log("添加道具 " + item2.Name.ToString());
                        InformationManager.DisplayMessage(new InformationMessage("添加道具 " + item2.Name.ToString(), new Color(.25f, 1, 0)));

                    }
                }
            }
            catch (Exception e)
            {
                LPLog.LogWarning("请存档后重新读取即可获取 宋朝甲胄。\n Please Save and Reload later to get the Song Dynasty Armors.");
            }


            return true;
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private string NowModNameId;
        private string NowModName;

        private void Init()
        {
            var path = BasePath.Name;
            var nowpath = AssemblyDirectory;
            var paths = nowpath.Split('\\');
            NowModName = paths[paths.Length - 3];
            NowModNameId = NowModName.ToLower();

            var filePath = BasePath.Name + @"Modules\" + NowModName + @"\MBLog.txt";
            LPLog.LogPath = filePath;

            LPLog.Log("当前 Mod Name：" + NowModName);
            LPLog.Log("当前 Mod ID：" + NowModNameId);

            //path += "/" + System.AppDomain.CurrentDomain.BaseDirectory;

        }
    }



}

public class SGAddItemData
{
    public List<string> AddSaveRecord = new List<string>();
    public List<bool> isAdd = new List<bool>();
}
