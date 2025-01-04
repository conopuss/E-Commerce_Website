namespace WebApplication1.Models
{
    public class Cls_Setting
    {

        Baglanti context = new Baglanti();
        public Setting SettingDetail()
        {
            Setting setting = context.Settings.Find(1);
            return setting;
        }

        public static bool SettingUpdate(Setting setting)
        {
            Baglanti context = new Baglanti();
            try
            {
                setting.SettingID = 1;
                context.Update(setting);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
