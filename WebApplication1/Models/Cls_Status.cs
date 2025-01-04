namespace WebApplication1.Models
{
    public class Cls_Status
    {
        Baglanti context = new Baglanti();
        public List<Status> StatusSelect()
        {
            List<Status> statuses = context.Statuses.ToList();
            return statuses;
        }

        public static bool StatusCreate(Status status)
        {
            Baglanti context = new Baglanti();
            try
            {
                context.Add(status);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public Status StatusDetails(int? id)
        {
            Status? status = context.Statuses.FirstOrDefault(c => c.StatusID == id);
            return status;
        }

        public static bool StatusUpdate(Status status)
        {
            Baglanti context = new Baglanti();
            try
            {
                context.Update(status);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool StatusDelete(int id)
        {
            Baglanti context = new Baglanti();
            try
            {
                Status? status = context.Statuses.FirstOrDefault(c => c.StatusID == id);
                status.Active = false;
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
