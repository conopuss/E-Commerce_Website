namespace WebApplication1.Models
{
    public class Cls_Supplier
    {
        Baglanti context = new Baglanti();
        public List<Supplier> SupplierSelect()
        {
            List<Supplier> supplier = context.Suppliers.ToList();

            return supplier;
        }

        public static bool SupplierInsert(Supplier supplier)
        {
            Baglanti context = new Baglanti();
            try
            {
                context.Suppliers.Add(supplier);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Supplier SupplierDetails(int? id)
        {
            Supplier? supplier = context.Suppliers.Find(id);
            return supplier;
        }

        public static bool SupplierUpdate(Supplier supplier)
        {
            Baglanti context = new Baglanti();
            try
            {
                context.Update(supplier);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool SupplierDelete(int? id)
        {
            try
            {
                Supplier? supplier = context.Suppliers.FirstOrDefault(s => s.SupplierID == id);
                supplier.Active = false;
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
